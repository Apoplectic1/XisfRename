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

namespace XisfFileManager.FileOperations
{
    public static class XisfFileUpdate
    {
        private static Buffer mBuffer;
        private static List<Buffer> mBufferList;

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

        private static XmlDocument ValidateXml(XmlDocument xmlDoc)
        {
            try
            {
                // Create a new XmlDocument to hold the valid content
                XmlDocument validXmlDoc = new XmlDocument();
                validXmlDoc.AppendChild(validXmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null));

                // Copy valid nodes to the new document
                XmlNode root = validXmlDoc.ImportNode(xmlDoc.DocumentElement, true);
                validXmlDoc.AppendChild(root);

                return validXmlDoc;
            }
            catch
            {
                return null; // Return null in case of an exception
            }
        }


        // ##############################################################################################################################################
        // ##############################################################################################################################################

        public static bool UpdateFile(XisfFile xFile)
        {
            // Return if KeywordList and the original xml are identical 
            //if (KeywordsMatchXml(xFile))
            //    return true;

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

                    xisfStart = BinaryFind(binaryFileData, "<xisf"); // returns the position of '<'
                    // NOTE: This value will change AFTER Keyword Replacement a few lines below
                    xisfEnd = BinaryFind(binaryFileData, "/xisf>") + "/xisf>".Length;  // returns the position immediately after '>'                

                    // convert from and including <xisf to /xisf> to a string and then parse string as xml into a new doc
                    string xisfString = Encoding.UTF8.GetString(binaryFileData, xisfStart, xisfEnd);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // The xisfString does not include the comment section if present
                    XmlDocument rawXmlDoc = new XmlDocument();
                    rawXmlDoc.LoadXml(xisfString);

                    // This will add an xml version if not present
                    XmlDocument xmlDoc = ValidateXml(rawXmlDoc);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Replace all existing FITSKeywords with FITSKeywords from our list (mFile.KeywordList)
                    ReplaceAllFitsKeywords(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // We need to set the start address and length of the image and thumbnail attachements due to changes in the <xisf> to </xisf> length
                    // Both image and thumbnail attachements require being padded with 0's prior to the image to BlockAlign them  
                    SetAttachmentPadding(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Set the new length of the <xisf to /xisf> section
                    int xmlLength = xmlDoc.OuterXml.Length;

                    binaryFileData[9] = (byte)((xmlLength >> 8) & 0xFF); // Most significant byte
                    binaryFileData[8] = (byte)(xmlLength & 0xFF); // Least significant byte

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    mBufferList.Clear();

                    // XISF Signature and length
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.BINARY,
                        BinaryDataStart = 0,
                        BinaryByteLength = 16,
                        BinaryData = binaryFileData
                    };
                    mBufferList.Add(mBuffer);

                    // Copy "<?xml" to  "/Xisf>"
                    int length = xmlDoc.OuterXml.Length;

                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ASCII,
                        AsciiData = xmlDoc.OuterXml
                    };
                    mBufferList.Add(mBuffer);

                    // Main Image
                    // In OUTPUT file, pad from current position to the start of image data
                    int padding = xFile.TargetAttachmentPadding;

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

                    // Thumbnail Image
                    if (xFile.ThumbnailAttachmentPadding > 0)
                    {
                        // Pad from current position to the start of the thumbnail image data
                        mBuffer = new Buffer
                        {
                            Type = eBufferData.ZEROS,
                            BinaryByteLength = xFile.ThumbnailAttachmentPadding,
                        };
                        mBufferList.Add(mBuffer);

                        // Add the binary thumbnail image data from rawFileData after image data and padding
                        mBuffer = new Buffer
                        {
                            Type = eBufferData.BINARY,
                            BinaryDataStart = xFile.ThumbnailAttachmentStart,
                            BinaryByteLength = xFile.ThumbnailAttachmentLength,
                            BinaryData = binaryFileData
                        };
                        mBufferList.Add(mBuffer);
                    }

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

        private static int FindStringInByteArray(byte[] bByteArray, string sString, int nMaxSearchLength)
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
        private static int GetPadding(int length, int alignment)
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
        static void UpdateImageLocation(XElement imageNode, int newStartingAddress)
        {
            var locationAttribute = imageNode.Attribute("location");
            if (locationAttribute != null)
            {
                string[] locationParts = locationAttribute.Value.Split(':');
                locationParts[1] = newStartingAddress.ToString();
                locationAttribute.Value = string.Join(":", locationParts);
            }
        }

        private static void SetAttachmentPadding(XmlDocument document, XisfFile xFile)
        {
            int originalDocumentLength;
            int modifiedDocumentLength;

            XElement root = xFile.mXDoc.Root;
            XNamespace ns = root.GetDefaultNamespace();


            XElement imageNode = xFile.mXDoc.Descendants(ns + "Image").FirstOrDefault(); // Assuming 'Image' is the tag

            if (imageNode != null)
            {
                originalDocumentLength = document.OuterXml.Length;

                int padding = GetPadding(document.ToString().Length + 16, xFile.BlockAlignmentSize);

                int newStartingAddress = document.ToString().Length + 16 + padding;
                UpdateImageLocation(imageNode, newStartingAddress);

                modifiedDocumentLength = document.OuterXml.Length;

                // Recalculate padding if needed
                xFile.TargetAttachmentPadding += modifiedDocumentLength - originalDocumentLength;

                // Other processing...
            }



            /*


            // Image, Icc Profile and Thumbnail start address and length are set when the input file is first read
            // We now need to compute how much padding is needed after Keyword replacement completes in the new output image

            // Xisf File Manager will take the first image in an Xisf File and throw out High and Low rejection images (or any other image) if they exist

            XmlNamespaceManager nsManager = new XmlNamespaceManager(document.NameTable);
            nsManager.AddNamespace("ns", "http://www.pixinsight.com/xisf"); // Add the correct namespace
            XmlNode imageNode = document.SelectSingleNode("//ns:Image", nsManager);

            // <Property id="XISF:BlockAlignmentSize" type="UInt16" value="4096" />
            // <Property id="XISF:MaxInlineBlockSize" type="UInt16" value="3072" />  This says any data block larger that 3072 needs to be included as an attachment
            XmlNode propertyNode = document.SelectSingleNode("//ns:Metadata/ns:Property[@id='XISF:BlockAlignmentSize']", nsManager);
            string valueAttribute = propertyNode.Attributes["value"].Value;
            int blockAlignmentSize = Convert.ToInt32(valueAttribute);

            // *****************************************************************************************************************
            // Target
            // *****************************************************************************************************************

            // Image PAD THE OUTPUT FILE after the xml section
            // document.OuterXml.Length includes any XmlVersion and XmlComment
            xFile.TargetAttachmentPadding = GetPadding(document.OuterXml.Length + 16, blockAlignmentSize);

            // Now we need to update the starting address for the attachment 
            // <Image geometry="3056:2048:1" sampleFormat="Float32" bounds="0:1" colorSpace="Gray" location="attachment:4096:25034752">

            string targetImageElement = imageNode.OuterXml;

            // The new starting address may require a different number of characters

            int originalElementLength = targetImageElement.IndexOf('>') + 1;
  
            string locationAttribute = imageNode.Attributes["location"].Value;
            string[] locationParts = locationAttribute.Split(':');

            int targetImageLength = Convert.ToInt32(locationParts[2]);
            int newTargetImageStartingAddress = (document.OuterXml.Length + 16) + xFile.TargetAttachmentPadding;

            // Modify the first field after "attachment" with the new integer value
            locationParts[1] = newTargetImageStartingAddress.ToString();
            string newLocationAttribute = string.Join(":", locationParts);

            // Update the attribute in the XML
            imageNode.Attributes["location"].Value = newLocationAttribute;

            targetImageElement = imageNode.OuterXml;

            int modifiedElementLength = targetImageElement.IndexOf('>') + 1;
           
            // Update based on new xml length
            xFile.TargetAttachmentPadding += modifiedElementLength - originalElementLength;


            // *****************************************************************************************************************
            // ICC Profile
            // *****************************************************************************************************************

            // *****************************************************************************************************************
            // Thumbnail
            // *****************************************************************************************************************

            // Thnumbnal PAD THE OUTPUT FILE after the main image 
            XmlNode thumbnailNode = imageNode.SelectSingleNode(".//ns:Thumbnail", nsManager);
            if (thumbnailNode != null)
                // For padding, all we need to know is the main image length since it starts on a modulo blockAlignmentSize boundary
                xFile.ThumbnailAttachmentPadding = (newTargetImageStartingAddress + xFile.TargetAttachmentLength) % blockAlignmentSize;
            else
                xFile.ThumbnailAttachmentPadding = 0;

            if (thumbnailNode != null)
            {
                // Now we need to update the starting address for the Thumbnail attachment inside <Image
                // <Thumbnail geometry="400:267:1" sampleFormat="UInt8" colorSpace="Gray" location="attachment:25042944:106800"/>

                string thumbnailImageElement = thumbnailNode.OuterXml;

                // The new starting address may require a different number of characters

                originalElementLength = thumbnailImageElement.IndexOf('>') + 1;
                
                locationAttribute = thumbnailNode.Attributes["location"].Value;
                locationParts = locationAttribute.Split(':');

                int newThumbnailImageStartingAddress = newTargetImageStartingAddress + targetImageLength + xFile.ThumbnailAttachmentPadding;

                // Modify the first field after "attachment" with the new integer value
                locationParts[1] = newThumbnailImageStartingAddress.ToString();
                newLocationAttribute = string.Join(":", locationParts);

                // Update the attribute in the XML
                thumbnailNode.Attributes["location"].Value = newLocationAttribute;

                thumbnailImageElement = thumbnailNode.OuterXml;

                modifiedElementLength = thumbnailImageElement.IndexOf('>') + 1;
                
                // Update based on new xml length
                xFile.ThumbnailAttachmentPadding += modifiedElementLength - originalElementLength;
            }
            */
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void ReplaceAllFitsKeywords(XmlDocument document, XisfFile mFile)
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
                // mFile.KeywordData.KeywordList contains the FITSKeywords from the original xisf file (minus explicit removals and changes)
                List<Keyword> keywords = mFile.KeywordList.mKeywordList.OrderBy(p => p.Name).ToList();

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

        private static int BinaryFind(byte[] buffer, string findText)
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

        private static bool WriteBinaryFile(string fileName)
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
        private static bool IsFileLocked(FileInfo file)
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
