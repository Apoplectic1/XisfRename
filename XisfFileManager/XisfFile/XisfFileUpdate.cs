using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XisfFileManager.Calculations;

using XisfFileManager.Enums;
using static System.Net.WebRequestMethods;
using Windows.ApplicationModel.VoiceCommands;
using System.Text.RegularExpressions;

namespace XisfFileManager.FileOperations
{
    public class XisfFileUpdate
    {
        private Buffer mBuffer;
        private List<Buffer> mBufferList;

        private static bool KeywordsMatchXml(XisfFile xFile)
        {
            //bool bHasXmlVersion = xFile.XmlString.Contains("<?xml version=");
            //if (!bHasXmlVersion)
            //    return false;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xFile.XmlString);

            // Create an XmlNamespaceManager and add the necessary namespace
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("ns", "http://www.pixinsight.com/xisf");

            // Select all FITSKeyword elements using the namespace
            XmlNodeList fitsKeywordNodes = xmlDoc.SelectNodes("//ns:FITSKeyword", namespaceManager);


            // Alphabetize KeywordList
            List<Keyword> keywords = xFile.KeywordList.mKeywordList.OrderBy(p => p.Name).ToList();

            // Check if fitsKeywordNodes and keywords have identical elements in the same order
            bool bIdentical = keywords.Count == fitsKeywordNodes.Count && Enumerable.Range(0, keywords.Count)
                                .All(index =>
                                {
                                    XmlNode fitsKeywordNode = fitsKeywordNodes[index];
                                    string nameAttribute = fitsKeywordNode.Attributes["name"]?.Value;
                                    string valueAttribute = fitsKeywordNode.Attributes["value"]?.Value;
                                    string commentAttribute = fitsKeywordNode.Attributes["comment"]?.Value;

                                    Keyword keyword = keywords[index];

                                    return nameAttribute == keyword.Name && valueAttribute == keyword.Value && commentAttribute == keyword.Comment;
                                });
            return bIdentical;

        }
        private void ValidateXml(XmlDocument document, XisfFile xFile)
        {
            // Create a new XmlDocument to hold the valid content
            XmlDocument validXmlDoc = new XmlDocument();
            validXmlDoc.AppendChild(validXmlDoc.CreateXmlDeclaration("1.0", "UTF-8", string.Empty));

            // Copy valid nodes to the new document
            XmlNode root = validXmlDoc.ImportNode(document.DocumentElement, true);
            validXmlDoc.AppendChild(root);

            // Update the XmlDocument 'document' to reflect the changes
            document.RemoveAll();
            document.LoadXml(validXmlDoc.OuterXml);
        }

        // ##############################################################################################################################################
        // ##############################################################################################################################################

        public bool UpdateFile(XisfFile xFile)
        {
            // Return if KeywordList and the original xml are identical 
            if (xFile.KeywordUpdateMode == eKeywordUpdateMode.PROTECT)
                return false;
            else
                if (xFile.KeywordUpdateMode == eKeywordUpdateMode.UPDATE_NEW)
                if (KeywordsMatchXml(xFile))
                    return true;

            // Not identical or force, so update the xml

            int xisfStart;
            int xisfEnd;

            byte[] binaryFileData = new byte[(int)1e9];
            mBufferList = new List<Buffer>();

            FileInfo xFileInfo = new FileInfo(xFile.FilePath);
            while (IsFileLocked(xFileInfo))
            {
                Thread.Sleep(500);
                xFileInfo = new FileInfo(xFile.FilePath);
            }

            try
            {
                using (Stream stream = new FileStream(xFile.FilePath, FileMode.Open))
                {
                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Read entire XISF file (up to 1 GB) into rawFileData and create an xml document
                    BinaryReader br = new BinaryReader(stream);
                    binaryFileData = br.ReadBytes((int)1e9);
                    br.Close();

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    xisfStart = BinaryFind(binaryFileData, "<xisf "); // returns the position of '<'
                    // NOTE: This value will change AFTER Keyword Replacement a few lines below
                    xisfEnd = BinaryFind(binaryFileData, "</xisf>") + "</xisf>".Length;  // returns the position immediately after '>'                

                    // convert from and including <xisf to /xisf> to a string and then parse string as xml into a new doc
                    string xmlString = Encoding.UTF8.GetString(binaryFileData, xisfStart, xisfEnd);

                    // Remove Processing History Property
                    string pattern = Regex.Escape("<Property") + @"(.*?)" + Regex.Escape(";</Property>");
                    xmlString = Regex.Replace(xmlString, pattern, "");

                    // The xisfString does not include the comment section if present
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
                    namespaceManager.AddNamespace("ns", "http://www.pixinsight.com/xisf");

                    xmlDoc.LoadXml(xFile.XmlVersionText + xFile.XmlCommentText + xmlString);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Remove all descendants from xFile.mXDoc that contain an "attachment" attribute that do not match the criteria specified by imageNode (our main image)
                    RemoveUnwantedAttachments(xmlDoc);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Replace all existing FITSKeywords with FITSKeywords from our list (mFile.KeywordList)

                    ReplaceAllFitsKeywords(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // We need to set the start address and length of the image and thumbnail attachements due to changes in the <xisf> to </xisf> length
                    // Both image and thumbnail attachements require being padded with 0's prior to the image to BlockAlign them

                    int possibleBogusImageAttachmentLocation = FindExistingImageStartingLocation(binaryFileData);
                    if (possibleBogusImageAttachmentLocation % xFile.BlockAlignmentSize != 0)
                    {
                        if ((possibleBogusImageAttachmentLocation - 1) % xFile.BlockAlignmentSize != 0)
                            MessageBox.Show("Image Attachment Start Location may not be not Block Aligned");
                    }
                    xFile.TargetAttachmentPadding = SetImageAttachmentLocation(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Set the new length of the <xisf to /xisf> section
                    int xmlLength = xmlDoc.OuterXml.Length;

                    // "XISF0100" is the XISF Signature. The length of the xml section is stored in the 9th and 10th bytes of the signature
                    // Assumes that xmlLength is less than 65536 bytes

                    byte[] xisfSignature = new byte[16] { 0x58, 0x49, 0x53, 0x46, 0x30, 0x31, 0x30, 0x30, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    xisfSignature[8] = (byte)(xmlLength & 0xFF); // Least significant byte
                    xisfSignature[9] = (byte)((xmlLength >> 8) & 0xFF); // Most significant byte

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // This will add an xml version if not present
                    //ValidateXml(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Create a new Buffer List. The list will contain the XISF Signature and length, the xml, and the image data
                    // This ordered List sets up data to by written, its type and where to get it from

                    mBufferList.Clear();


                    // XISF Signature and length
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.BINARY,
                        BinaryDataStart = 0,
                        BinaryByteLength = 16,
                        BinaryData = xisfSignature
                    };
                    mBufferList.Add(mBuffer);

                    // Copy "<?xml" to  "/Xisf>"
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ASCII,
                        AsciiData = xmlDoc.OuterXml
                    };
                    mBufferList.Add(mBuffer);

                    // Main Image
                    // In OUTPUT file, pad from current position to the start of image data
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ZEROS,
                        BinaryByteLength = xFile.TargetAttachmentPadding
                    };
                    mBufferList.Add(mBuffer);

                    // In INPUT file, Add the binary image data from rawFileData after padding
                    int iStart = xFile.TargetAttachmentStart;
                    int iLength = xFile.TargetAttachmentLength;

                    mBuffer = new Buffer
                    {
                        Type = eBufferData.BINARY,
                        BinaryDataStart = xFile.TargetAttachmentStart,
                        BinaryByteLength = xFile.TargetAttachmentLength,
                        BinaryData = binaryFileData
                    };
                    mBufferList.Add(mBuffer);

                    // Ignore any other attachments (e.g. thumbnail) in the input file

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Now that the mBuffer List is done, write the XISF File
                    bool bStatus = WriteBinaryFile(xFile.FilePath);
                    if (bStatus == false)
                        return false;
                }
            }
            catch
            {
                DialogResult status = MessageBox.Show("Update Write File Failed", xFile.FilePath, MessageBoxButtons.OKCancel);
                if (status == DialogResult.OK)
                    return false;
                Environment.Exit(-1);
            }

            return true;
        }
        // ##############################################################################################################################################
        // ##############################################################################################################################################

        private int FindStringInByteArray(byte[] bByteArray, string sString, int nMaxSearchLength)
        {
            int stringLength = sString.Length;

            for (int i = 0; i <= nMaxSearchLength; i++)
            {
                bool found = true;

                for (int j = 0; j < stringLength; j++)
                {
                    if (bByteArray[i + j] != (byte)sString[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                    return i;
            }

            return -1;
        }

        // ##############################################################################################################################################
        // ##############################################################################################################################################
        private int GetNewPadding(int length, int alignment)
        {
            // Find the difference between length and the nearest larger value that is evenly divisible by alignment

            if ((length % alignment) == 0)
                return 0;
            else
            {
                int nextdivisible = length + (alignment - length % alignment);
                return nextdivisible - length;
            }
        }
        static void UpdateXmlImageStartingAddress(ref XElement imageNode, int newStartingAddress)
        {
            var locationAttribute = imageNode.Attribute("location");
            if (locationAttribute != null)
            {
                string[] locationParts = locationAttribute.Value.Split(':');
                locationParts[1] = newStartingAddress.ToString();
                locationAttribute.Value = string.Join(":", locationParts);
            }
        }

        private void RemoveUnwantedAttachments(XmlDocument document)
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(document.NameTable);
            string namespaceUri = document.DocumentElement.NamespaceURI;
            nsManager.AddNamespace("ns", namespaceUri);

            document.SelectSingleNode("//ns:Image[@Id='integration' or not(@Id)]/*[@attachment]", nsManager)?.ParentNode?.RemoveChild(document.SelectSingleNode("//ns:Image[@Id='integration' or not(@Id)]/*[@attachment]", nsManager));
            document.SelectSingleNode("//ns:Thumbnail", nsManager)?.ParentNode?.RemoveChild(document.SelectSingleNode("//ns:Thumbnail", nsManager));
            document.SelectSingleNode("//ns:ICCProfile", nsManager)?.ParentNode?.RemoveChild(document.SelectSingleNode("//ns:ICCProfile", nsManager));
            document.SelectSingleNode("//ns:DisplayFunction", nsManager)?.ParentNode?.RemoveChild(document.SelectSingleNode("//ns:DisplayFunction", nsManager));
            document.SelectSingleNode("//ns:Resolution", nsManager)?.ParentNode?.RemoveChild(document.SelectSingleNode("//ns:Resolution", nsManager));
        }

        public int FindExistingImageStartingLocation(byte[] binaryFileData)
        {
            // Ignore Image attachment start location, set and return the found location

            // Find the end of the xml section, then walk through any run of 0's after the </xisf> tag
            // Return the location of the first non-zero byte after the </xisf> tag

            int xmlEndLocation = BinaryFind(binaryFileData, "</xisf>") + "</xisf>".Length;
            // Walk through any run of 0's after the </xisf> tag
            int padding = 0;
            for (int i = xmlEndLocation; i < binaryFileData.Length; i++)
            {
                if (binaryFileData[i] == 0)
                    padding++;
                else
                    break;
            }

            return xmlEndLocation + padding;
        }

        private int SetImageAttachmentLocation(XmlDocument document, XisfFile xFile)
        {
            int documentLengthBeforeNewStartingAddress;
            int documentLengthAfterNewStartingAddress;
            int newPadding = -1;
            int newStartingAddress;

            XmlNamespaceManager nsManager = new XmlNamespaceManager(document.NameTable);
            string namespaceUri = document.DocumentElement.NamespaceURI;
            nsManager.AddNamespace("ns", namespaceUri);


            // Find the first and only (ns + "Image") descendant
            XmlNode imageNode = document.SelectSingleNode("//ns:Image", nsManager);
            //XElement imageNode = xFile.mXDoc.Descendants(ns + "Image").FirstOrDefault();

            if (imageNode != null)
            {
                // New Xml document length before padding
                // Note the the new xml document may not contain the comment section or other changes (is the comment section needed?)
                // We have also added or changed lots of keywords in this new xml document

                // Iterate until the xml document length does not change
                do
                {
                    documentLengthBeforeNewStartingAddress = document.OuterXml.Length;

                    // Include the 16 byte XISF Signature. No Comment section in the padding calculation
                    newPadding = GetNewPadding(documentLengthBeforeNewStartingAddress + 16, xFile.BlockAlignmentSize);

                    // The starting address is the address in the new file (including XISF Signature); (not just in the Xml document section)
                    newStartingAddress = documentLengthBeforeNewStartingAddress + 16 + newPadding;

                    // Update imageNode with the new starting address and can change the size of the xml document 
                    // 1. This can push the image data to a new location - new start address
                    // 2. This can change the padding - new padding
                    // If the document length changes, we need to iterate until it does not change (padding will change with each iteration)

                    //var locationAttribute = imageNode.Attribute("location");
                    XmlAttribute locationAttribute = imageNode.Attributes["location"];
                    if (locationAttribute != null)
                    {
                        string[] locationParts = locationAttribute.Value.Split(':');
                        locationParts[1] = newStartingAddress.ToString();
                        locationAttribute.Value = string.Join(":", locationParts);
                    }

                    documentLengthAfterNewStartingAddress = document.OuterXml.Length;
                }
                while (documentLengthAfterNewStartingAddress != documentLengthBeforeNewStartingAddress);
            }

            /*
            // Update the XmlDocument 'document' to reflect the changes
            // Convert the updated xFile.mXDoc to an XmlDocument
            XmlDocument updatedDocument = new XmlDocument();
            updatedDocument.RemoveAll();
            updatedDocument.LoadXml(xFile.XmlVersionText + xFile.XmlCommentText + xFile.mXDoc.ToString());

            // Clear the 'document' and copy the updated content
            document.RemoveAll();
            document.LoadXml(updatedDocument.OuterXml);
            */
            return newPadding;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private void ReplaceAllFitsKeywords(XmlDocument document, XisfFile xFile)
        {
            XmlNodeList nodeList = document.GetElementsByTagName("FITSKeyword");
            for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                nodeList[i].ParentNode.RemoveChild(nodeList[i]);
            }

            // At this point, our xmlDoc only contains header boilerplate

            // Find the "<Image" tag. We are going to add the entire contents of the KeywordData keyword list as 
            // individual child elements under it (and selectively includeing SubFrame Selector keywords)
            nodeList = document.GetElementsByTagName("Image");

            foreach (XmlNode item in nodeList)
            {
                // Alphabetize the KeywordData FITSKeywords
                // mFile.KeywordData.KeywordList contains the FITSKeywords from the original xisf file (with explicit removals and changes)
                List<Keyword> keywords = xFile.KeywordList.mKeywordList.OrderBy(p => p.Name).ToList();

                // Now add all FITSKeywords found in KeywordData to the xmlDocument
                foreach (Keyword keyword in keywords)
                {
                    var newElement = document.CreateElement("FITSKeyword", document.DocumentElement.NamespaceURI);
                    newElement.SetAttribute("name", keyword.Name);
                    newElement.SetAttribute("comment", keyword.Comment);
                    newElement.SetAttribute("value", keyword.Value.ToString());
                    item.AppendChild(newElement);
                }
            }
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private int BinaryFind(byte[] buffer, string findText)
        {
            byte[] xisfFind = Encoding.UTF8.GetBytes(findText);
            int i;
            int j;
            for (i = 0; i <= (buffer.Length - xisfFind.Length); i++)
            {
                if (buffer[i] == xisfFind[0])
                {
                    for (j = 1; j < xisfFind.Length && buffer[i + j] == xisfFind[j]; j++) ;
                    if (j == xisfFind.Length)
                        return i;
                }
            }

            return -1;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************


        // ##############################################################################################################################################
        // ##############################################################################################################################################

        private bool WriteBinaryFile(string fileName)
        {
            byte[] zero = { 0x00 };

            try
            {
                using (MemoryStream rawStream = new MemoryStream())
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter(rawStream))
                    {
                        foreach (Buffer buffer in mBufferList)
                        {
                            long position = rawStream.Position;

                            switch (buffer.Type)
                            {
                                case eBufferData.ASCII:
                                    binaryWriter.Write(Encoding.UTF8.GetBytes(buffer.AsciiData), 0, Encoding.UTF8.GetBytes(buffer.AsciiData).Length);
                                    break;

                                case eBufferData.BINARY:
                                    binaryWriter.Write(buffer.BinaryData, buffer.BinaryDataStart, buffer.BinaryByteLength);
                                    break;

                                case eBufferData.ZEROS:
                                    for (int i = (int)position; i < buffer.BinaryByteLength + position; i++)
                                    {
                                        binaryWriter.Write(zero, 0, 1);
                                    }
                                    break;

                                case eBufferData.POSITION:
                                    if ((int)position > buffer.ToPosition)
                                    {
                                        string title = "WriteBinaryFile(string fileName) POSITION Error";
                                        string message = "\n\nThe length of xml xisfString is after the start of image data:\n" +
                                            "    File:                            " + fileName + "\n" +
                                            "    Current Write Position:          " + position.ToString() + "\n" +
                                            "    Image Attachment Start Position: " + buffer.ToPosition.ToString() + "\n\nAborting.";

                                        MessageBox.Show(message, title, MessageBoxButtons.OK);
                                        return false;
                                    }

                                    for (long i = position; i < buffer.ToPosition; i++)
                                    {
                                        binaryWriter.Write(zero, 0, 1);
                                    }
                                    break;
                            }
                        }
                    }

                    int binaryDataLength = rawStream.ToArray().Length;
                    byte[] binaryData = new byte[binaryDataLength];

                    binaryData = rawStream.ToArray();

                    using (Stream fileStream = new FileStream(fileName, FileMode.Create))
                    {
                        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                        {
                            fileStream.Write(binaryData, 0, binaryDataLength);
                            binaryWriter.Flush();
                            binaryWriter.Close();
                        }
                    }
                    binaryData = null;
                }
                return true;
            }
            catch
            {
                MessageBox.Show(fileName, "WriteBinaryFile Failed");
                return false;
            }

            // ##############################################################################################################################################
            // ##############################################################################################################################################

        }
        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
