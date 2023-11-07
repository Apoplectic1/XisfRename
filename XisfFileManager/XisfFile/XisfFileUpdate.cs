using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private static void AddXmlVersion(XmlDocument xmlDoc)
        {
            // Check if the declaration element already exists
            bool declarationExists = xmlDoc.SelectSingleNode("/*[local-name()='declaration']") != null;

            // Create an XmlElement for the XML declaration if it doesn't exist
            if (!declarationExists)
            {
                // Create an XmlElement for the declaration
                XmlElement declarationElement = xmlDoc.CreateElement("declaration");

                // Set the inner text of the declaration element to your XML declaration string
                declarationElement.InnerText = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

                // Insert the declaration element as the first child of the root element
                xmlDoc.DocumentElement.InsertBefore(declarationElement, xmlDoc.DocumentElement.FirstChild);
            }
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
            if (KeywordsMatchXml(xFile))
                return true;
   
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
                    xisfEnd = BinaryFind(binaryFileData, "/xisf>") + "/xisf>".Length;  // returns the position immediately after '>'                

                    // convert from and including <xisf to /xisf> to a string and then parse string as xml into a new doc
                    string xisfString = Encoding.UTF8.GetString(binaryFileData, xisfStart, xisfEnd);

                    XmlDocument rawXmlDoc = new XmlDocument();
                    rawXmlDoc.LoadXml(xisfString);

                    XmlDocument xmlDoc = ValidateXml(rawXmlDoc);

                    // Replace all existing FITSKeywords with FITSKeywords from our list (mFile.KeywordList)
                    ReplaceAllFitsKeywords(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // We need to set the start address and length of the image and thumbnail attachements due to changes in the <xisf> to </xisf> length
                    // Both image and thumbnail attachements require being padded with 0's prior to the image to BlockAlign them  
                    UpdateImageAttachmentLocationsXml(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Set the new length of the <xisf to /xisf> section
                    int xmlLength = xmlDoc.OuterXml.Length + 16;

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
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ASCII,
                        AsciiData = xmlDoc.OuterXml
                    };
                    mBufferList.Add(mBuffer);

                    // Pad from current position to the start of image data
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ZEROS,
                        BinaryByteLength = xFile.ImageAttachmentStartPadding.First()
                    };
                    mBufferList.Add(mBuffer);

                    // Add the binary image data from rawFileData after padding
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.BINARY,
                        BinaryDataStart = xFile.ImageAttachmentStart.First(),
                        BinaryByteLength = xFile.ImageAttachmentLength.First(),
                        BinaryData = binaryFileData
                    };
                    mBufferList.Add(mBuffer);

                    if (xFile.ThumbnailAttachmentStartPadding.First() > 0)
                    {
                        // Pad from current position to the start of the thumbnail image data
                        mBuffer = new Buffer
                        {
                            Type = eBufferData.ZEROS,
                            BinaryByteLength = xFile.ThumbnailAttachmentStartPadding.First(),
                        };
                        mBufferList.Add(mBuffer);

                        // Add the binary thumbnail image data from rawFileData after image data and padding
                        mBuffer = new Buffer
                        {
                            Type = eBufferData.BINARY,
                            BinaryDataStart = xFile.ThumbnailAttachmentStart.First(),
                            BinaryByteLength = xFile.ThumbnailAttachmentLength.First(),
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

        private static bool UpdateImageAttachmentLocationsXml(XmlDocument document, XisfFile xFile)
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(document.NameTable);
            nsManager.AddNamespace("ns", "http://www.pixinsight.com/xisf"); // Add the correct namespace
            XmlNode imageNode = document.SelectSingleNode("//ns:Image", nsManager);

            if (imageNode == null)
                return false;

            // <Image geometry="5496:3672:1" sampleFormat="UInt16" colorSpace="Gray" location="attachment:8192:40362624"><Resolution horizontal="72" vertical="72" unit="inch" />
            string[] imageLocationAttribute = imageNode.Attributes["location"].Value.Split(':');
            // imageStart temporarily begins at the end of the modified xml section
            int imageStart = document.OuterXml.Length + 16;
            int imageLength = Convert.ToInt32(imageLocationAttribute[2]);


            // <Property id="XISF:BlockAlignmentSize" type="UInt16" value="4096" />
            // <Property id="XISF:MaxInlineBlockSize" type="UInt16" value="3072" />    What is this?
            XmlNode propertyNode = document.SelectSingleNode("//ns:Metadata/ns:Property[@id='XISF:BlockAlignmentSize']", nsManager);
            string valueAttribute = propertyNode.Attributes["value"].Value;
            int blockAlignmentSize = Convert.ToInt32(valueAttribute);

            xFile.ImageAttachmentStartPadding.Add(blockAlignmentSize - (imageStart % blockAlignmentSize));
            xFile.ImageAttachmentStart[0] = imageStart + xFile.ImageAttachmentStartPadding.First();
            xFile.ImageAttachmentLength[0] = imageLength;

            // <Thumbnail geometry="400:267:1" sampleFormat="UInt8" colorSpace="Gray" location="attachment:80736256:106800"/>
            XmlNode thumbnailNode = imageNode.SelectSingleNode(".//ns:Thumbnail", nsManager);
            if (thumbnailNode != null)
            {
                string[] thumbnailLocationAttribute = thumbnailNode.Attributes["location"].Value.Split(':');
                imageStart = xFile.ThumbnailAttachmentStart[0] + xFile.ThumbnailAttachmentLength.First();
                imageLength = Convert.ToInt32(thumbnailLocationAttribute[2]);

                xFile.ThumbnailAttachmentStartPadding.Add(blockAlignmentSize - (imageStart % blockAlignmentSize));
                xFile.ThumbnailAttachmentStart[0] = imageStart + xFile.ThumbnailAttachmentStartPadding.First();
                xFile.ThumbnailAttachmentLength[0] = imageLength;
            }
            else
            {
                xFile.ThumbnailAttachmentStartPadding.Add(0);
                xFile.ThumbnailAttachmentStart.Add(-1);
                xFile.ThumbnailAttachmentLength.Add(-1);
            }

            return true;
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
