using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;
using XisfFileManager.Calculations;
using XisfFileManager.Keywords;

using XisfFileManager.Enums;

namespace XisfFileManager.FileOperations
{
    public static class XisfFileUpdate
    {
        
        public static eOperation Operation { get; set; } = eOperation.KEEP_WEIGHTS;
        private static Buffer mBuffer;
        private static List<Buffer> mBufferList;

        // ##############################################################################################################################################
        // ##############################################################################################################################################

        public static bool UpdateFile(XisfFile xFile, SubFrameLists SubFrameKeywordLists, bool bPreserveProtectedFiles)
        {
            int xisfStart;
            int xisfEnd;
            byte[] rawFileData = new byte[(int)1e9];
            mBufferList = new List<Buffer>();
            string sourceFilePath;

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
                    mBufferList.Clear();

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Read entire XISF file (up to 1 GB) into rawFileData and create an xml document
                    BinaryReader br = new BinaryReader(stream);
                    rawFileData = br.ReadBytes((int)1e9);
                    br.Close();

                    // Set up pointers to xisf start and stop positions
                    xisfStart = BinaryFind(rawFileData, "<xisf version"); // returns the position of '<'
                    xisfEnd = BinaryFind(rawFileData, "</xisf>") + "</xisf>".Length;  // returns the position immediately after '>'                

                    // convert (including) from <xisf to </xisf> to a string and then parse string as xml into a new doc
                    string xisfString = Encoding.UTF8.GetString(rawFileData, xisfStart, xisfEnd);
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xisfString);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Keywords are added in this section
                    // First, all the Keywords in mFile KeywordData are adjusted - add, remove, or otherwise changed.
                    // Then all FITSKeywords are removed from the xmlDoc
                    // Keywords are then added in this order:
                    //    1. All keywords found in AddCsvKeywordList
                    //       This is all the keywords found in the original file
                    //    2. Add keywords from CSV File

                    // Remove keywords from KeywordData associated with each mFile. Note that this mFile instance's KeyWordData will be added in ReplaceAllFitsKeywords
                    xFile.RemoveKeyword("COMMENT");
                    xFile.RemoveKeyword("HISTORY");
                    xFile.RemoveKeyword("ALIGNH");
                    xFile.RemoveKeyword("Bandwidth setting");
                    xFile.RemoveKeyword("BTP");
                    xFile.RemoveKeyword("SBUUID");
                    xFile.RemoveKeyword("READOUTM");
                    xFile.RemoveKeyword("CDS");
                    xFile.RemoveKeyword("DISPINCR");
                    xFile.RemoveKeyword("EXTEND");
                    xFile.RemoveKeyword("FOCTMPSC");
                    xFile.RemoveKeyword("PICTTYPE");

                    // Replace all existing FITSKeywords with FITSKeywords from our list (mFile.KeywordList)
                    ReplaceAllFitsKeywords(xmlDoc, xFile, SubFrameKeywordLists);


                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // We need to set the start address and length of the image and thumbnail attachements due to changes in the <xisf> to </xisf> length
                    // Both image and thumbnail attachements require being padded with 0's prior to the image to BlockAlign them  
                    _ = UpdateImageAttachmentLocationsXml(xmlDoc, xFile);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Begin setting up output XISF File
                    // Create a mBuffer to hold our different data types (Binary, Text, Binary Zero's, etc.)
                    // Add each mBuffer to a List and when complete, sequentially write each List element over our XISF File

                    xisfString = xmlDoc.OuterXml;

                    // Add header (includes binary and string portions) up the start of "<xisf version"
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ASCII,
                        AsciiData = "XISF0100"
                    };
                    mBufferList.Add(mBuffer);

                    // Write length (filled out later) and reserved bytes as 0's
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ZEROS,
                        BinaryByteLength = 8
                    };
                    mBufferList.Add(mBuffer);

                    // Add the newly replaced XML ascii potortion to the buffer list 
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ASCII,
                        AsciiData = xisfString
                    };
                    mBufferList.Add(mBuffer);

                    // Pad from current position to the start of image data
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.ZEROS,
                        BinaryByteLength = xFile.ImageAttachmentStartPadding
                    };
                    mBufferList.Add(mBuffer);

                    // Add the binary image data from rawFileData after padding
                    mBuffer = new Buffer
                    {
                        Type = eBufferData.BINARY,
                        BinaryDataStart = xFile.ImageAttachmentStart,
                        BinaryByteLength = xFile.ImageAttachmentLength,
                        BinaryData = rawFileData
                    };
                    mBufferList.Add(mBuffer);

                    if (xFile.ThumbnailAttachmentStartPadding > 0)
                    {
                        // Pad from current position to the start of the thumbnail image data
                        mBuffer = new Buffer
                        {
                            Type = eBufferData.ZEROS,
                            BinaryByteLength = xFile.ThumbnailAttachmentStartPadding
                        };
                        mBufferList.Add(mBuffer);

                        // Add the binary thumbnail image data from rawFileData after image data and padding
                        mBuffer = new Buffer
                        {
                            Type = eBufferData.BINARY,
                            BinaryDataStart = xFile.ThumbnailAttachmentStart,
                            BinaryByteLength = xFile.ThumbnailAttachmentLength,
                            BinaryData = rawFileData
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


            // After Keyword update, move rejected file (Approved = false) to the "Rejected" subdirectory
            if (xFile.Approved == false)
            {
                sourceFilePath = Path.GetDirectoryName(xFile.FilePath);
                _ = Directory.CreateDirectory(sourceFilePath + "\\Rejected");
                File.Move(xFile.FilePath, sourceFilePath + "\\Rejected\\" + Path.GetFileName(xFile.FilePath));
            }

            return true;
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

            // <Image geometry="5496:3672:1" sampleFormat="Float32" bounds="0:1" colorSpace="Gray" location="attachment:8192:80725248">
            string[] imageLocationAttribute = imageNode.Attributes["location"].Value.Split(':');
            int imageStart = document.OuterXml.Length + 16;
            int imageLength = Convert.ToInt32(imageLocationAttribute[2]);

            XmlNode propertyNode = document.SelectSingleNode("//ns:Metadata/ns:Property[@id='XISF:BlockAlignmentSize']", nsManager);
            string valueAttribute = propertyNode.Attributes["value"].Value;
            int blockAlignmentSize = Convert.ToInt32(valueAttribute);

            xFile.ImageAttachmentStartPadding = blockAlignmentSize - (imageStart % blockAlignmentSize);
            xFile.ImageAttachmentStart = imageStart + xFile.ImageAttachmentStartPadding;
            xFile.ImageAttachmentLength = imageLength;

            // <Thumbnail geometry="400:267:1" sampleFormat="UInt8" colorSpace="Gray" location="attachment:80736256:106800"/>
            XmlNode thumbnailNode = imageNode.SelectSingleNode(".//ns:Thumbnail", nsManager);
            if (thumbnailNode != null)
            {
                string[] thumbnailLocationAttribute = thumbnailNode.Attributes["location"].Value.Split(':');
                imageStart = xFile.ImageAttachmentStart + xFile.ImageAttachmentLength;
                imageLength = Convert.ToInt32(thumbnailLocationAttribute[2]);

                xFile.ThumbnailAttachmentStartPadding = blockAlignmentSize - (imageStart % blockAlignmentSize);
                xFile.ThumbnailAttachmentStart = imageStart + xFile.ThumbnailAttachmentStartPadding;
                xFile.ThumbnailAttachmentLength = imageLength;
            }
            else
            {
                xFile.ThumbnailAttachmentStartPadding = 0;
                xFile.ThumbnailAttachmentStart = 0;
                xFile.ThumbnailAttachmentLength = 0;
            }

            return true;
        }


        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void ReplaceAllFitsKeywords(XmlDocument document, XisfFile mFile, SubFrameLists SubFrameSelectorKeywordLists)
        {
            // First remove all FITSKeywords (nodes) from the xml document
            XmlNodeList nodeList = document.GetElementsByTagName("FITSKeyword");
            for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                _ = nodeList[i].ParentNode.RemoveChild(nodeList[i]);
            }

            // At this point, our xmlDoc only contains header boilerplate

            // Find the "<Image" tag. We are going to add the entire contents of the KeywordData keyword list as 
            // individual child elements under it (and selectively includeing SubFrame Selector keywords)
            nodeList = document.GetElementsByTagName("Image");

            foreach (XmlNode item in nodeList)
            {
                // If Operation != OperationEnum.KEEP_WEIGHTS, remove (if existing) and then add (i.e. replace) the keywords from PixInsight's SubFrame Selector.
                // The "replaced" keywords are the orignal xisf file keywords and are replaced with the keywords stored in mFile.KeywordData.KeywordList.
                // Existing Weights are kept by NOT replacing or adding new ones.
                if (Operation != eOperation.KEEP_WEIGHTS)
                {
                    ReplaceSubFrameSelectorKeywords(SubFrameSelectorKeywordLists, mFile);
                }

                // Deal with SSWEIGHT and WEIGHT
                // SSWEIGHT may already be part of mFile or not
                // We want to selectively update an existing SSWEIGHT value, add a new one or leave it alone (leaving it alone inludes a non-existent SSWEIGHT by default)
                // For adding and updating, we need to consider SSWEIGHT vs WEIGHT
                // WEIGHT is produced by this program while SSWEIGHT is produced by PixInsight

                // Alphabetize the KeywordData FITSKeywords
                // mFile.KeywordData.KeywordList contains the FITSKeywords from the original xisf file (minus explicit removals and changes)
                List<Keyword> keywords = mFile.mKeywordList.mKeywordList.OrderBy(p => p.Name).ToList();

                // Now add all FITSKeywords found in KeywordData to the xmlDocument
                foreach (Keyword keyword in keywords)
                {
                    var newElement = document.CreateElement("FITSKeyword", document.DocumentElement.NamespaceURI);
                    newElement.SetAttribute("name", keyword.Name);
                    newElement.SetAttribute("comment", keyword.Comment);
                    newElement.SetAttribute("value", keyword.Value.ToString());
                    _ = item.AppendChild(newElement);
                }
            }
        }

        public static void UpdateCsvWeightList(SubFrameNumericLists WeightLists, SubFrameLists CsvWeightLists)
        {
            int index = 0;

            foreach (double value in WeightLists.Weight)
            {
                CsvWeightLists.SubFrameList.WeightList[index].Value = WeightLists.Weight[index].ToString();
                index++;
            }
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void ReplaceSubFrameSelectorKeywords(SubFrameLists CsvWeightLists, XisfFile mFile)
        {
            int indexer;
            List<Keyword> fileNameList = CsvWeightLists.SubFrameList.FileNameList;

            indexer = 0;
            foreach (Keyword file in fileNameList)
            {
                string fileName = file.Value.ToString().Replace("\"", "").Replace("/", @"\");

                // This equality check makes sure the correct data is used for the current file.
                if (mFile.FilePath == fileName)
                {
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.ApprovedList[indexer].Name, CsvWeightLists.SubFrameList.ApprovedList[indexer].Value, CsvWeightLists.SubFrameList.ApprovedList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.Eccentricity.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.Eccentricity.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.Eccentricity.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.EccentricityMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.EccentricityMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.EccentricityMeanDeviation.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.FwhmList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.FwhmList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.FwhmList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.FwhmMeanDeviationList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.FwhmMeanDeviationList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.FwhmMeanDeviationList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.MedianList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.MedianList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.MedianList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.MedianMeanDeviationList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.MedianMeanDeviationList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.MedianMeanDeviationList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.NoiseList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.NoiseList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.NoiseList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.NoiseRatioList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.NoiseRatioList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.NoiseRatioList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.SnrWeightList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.SnrWeightList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.SnrWeightList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.StarResidualList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.StarResidualList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.StarResidualList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.StarResidualMeanDeviationList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.StarResidualMeanDeviationList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.StarResidualMeanDeviationList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.StarsList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.StarsList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.StarsList.ElementAt(indexer).Comment);
                    mFile.AddKeyword(CsvWeightLists.SubFrameList.WeightList.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.WeightList.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.WeightList.ElementAt(indexer).Comment);
                }

                indexer++;
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

                                        _ = MessageBox.Show(message, title, MessageBoxButtons.OK);
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

                    // Fix Header length
                    int binaryDataLength = rawStream.ToArray().Length;
                    byte[] binaryData = new byte[binaryDataLength];

                    binaryData = rawStream.ToArray();

                    int xisfStart = BinaryFind(binaryData, "<?xml"); // returns the position of '<'
                    int xisfEnd = BinaryFind(binaryData, "</xisf>") + "</xisf>".Length;  // returns the position immediately after '>'             
                    int xisfLength = xisfEnd - xisfStart;

                    // Write '<'?xml to </xisf'>' length to XISF Header length portion - See PixInsight XISF Developer info 
                    binaryData[8] = Convert.ToByte((xisfLength >> 0) & 0xff);
                    binaryData[9] = Convert.ToByte((xisfLength >> 8) & 0xff);
                    binaryData[10] = Convert.ToByte((xisfLength >> 16) & 0xff);
                    binaryData[11] = Convert.ToByte((xisfLength >> 24) & 0xff);

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
                _ = MessageBox.Show(fileName, "WriteBinaryFile Failed");
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
