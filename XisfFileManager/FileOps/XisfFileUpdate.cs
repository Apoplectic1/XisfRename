using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XisfFileManager.Calculations;
using XisfFileManager.Keywords;

namespace XisfFileManager.FileOperations
{
    public static class XisfFileUpdate
    {
        public enum OperationEnum { NEW_WEIGHTS, KEEP_WEIGHTS, RESCALE_WEIGHTS, CALCULATED_WEIGHTS }
        public static OperationEnum Operation { get; set; } = OperationEnum.KEEP_WEIGHTS;
        public static Buffer mBuffer;
        public static List<Buffer> mBufferList;
        public static string TargetName { get; set; }
       
        // ##############################################################################################################################################
        // ##############################################################################################################################################

        public static bool UpdateFile(XisfFile mFile, SubFrameLists SubFrameKeywordLists)
        {
            int xmlStart;
            int xisfStart;
            int xisfEnd;
            byte[] rawFileData = new byte[(int)1e9];
            mBufferList = new List<Buffer>();
            string sourceFilePath;
            try
            {
                using (Stream stream = new FileStream(mFile.SourceFileName, FileMode.Open))
                {
                    mBufferList.Clear();

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Read entire XISF file (up to 1 GB) into rawFileData and create an xml document
                    BinaryReader br = new BinaryReader(stream);
                    rawFileData = br.ReadBytes((int)1e9);
                    br.Close();

                    // Set up some pointers to xml start and stop positions
                    xmlStart = BinaryFind(rawFileData, "<?xml version"); // returns the position of '<'
                    xisfStart = BinaryFind(rawFileData, "<xisf version"); // returns the position of '<'
                    xisfEnd = BinaryFind(rawFileData, "</xisf>") + "</xisf>".Length;  // returns the position immediately after '>'                

                    // convert (including) from <xisf to </xisf> to a string and then parse string as xml into a new doc
                    string xisfString = Encoding.UTF8.GetString(rawFileData, xmlStart, xisfEnd);
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
                    if (mFile.Master == false)
                    {
                        mFile.KeywordData.RemoveKeyword("COMMENT");
                        mFile.KeywordData.RemoveKeyword("HISTORY");
                    }
                    else
                    {
                        mFile.KeywordData.RemoveKeyword("COMMENT");
                        mFile.KeywordData.RemoveKeyword("HISTORY");
                    }

                    mFile.KeywordData.RemoveKeyword("ALIGNH");    
                    mFile.KeywordData.RemoveKeyword("NOISE");
                    mFile.KeywordData.RemoveKeyword("Bandwidth setting");
                    mFile.KeywordData.RemoveKeyword("BTP");
                    mFile.KeywordData.RemoveKeyword("SBUUID");
                    mFile.KeywordData.RemoveKeyword("READOUTM");
                    mFile.KeywordData.RemoveKeyword("CDS");
                    mFile.KeywordData.RemoveKeyword("DISPINCR");
                    mFile.KeywordData.RemoveKeyword("EXTEND");
                    mFile.KeywordData.RemoveKeyword("FOCTMPSC");
                    mFile.KeywordData.RemoveKeyword("PICTTYPE");

                    // Replace all existing FITSKeywords with FITSKeywords from our list (mFile.KeywordList)
                    ReplaceAllFitsKeywords(xmlDoc, mFile, SubFrameKeywordLists);


                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    int imageStart = UpdateImageAttachmentLocationsXml(xmlDoc,
                        xmlDoc.OuterXml.Length + 16,
                        mFile.ImageAttachmentLength,
                        xmlDoc.OuterXml.Length + 16 + mFile.ImageAttachmentLength,
                        mFile.ThumbnailAttachmentLength);
                    
                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Begin setting up output XISF File
                    // Create a mBuffer to hold our different data types (Binary, Text, Binary Zero's, etc.)
                    // Add each mBuffer to a List and when complete, sequentially write each List element over our XISF File

                    xisfString = xmlDoc.OuterXml;

                    // Add header (includes binary and string portions) up the start of "<xisf version"
                    mBuffer = new Buffer
                    {
                        Type = Buffer.TypeEnum.ASCII,
                        AsciiData = "XISF0100"
                    };
                    mBufferList.Add(mBuffer);

                    // Write length (filled out later) and reserved bytes as 0's
                    mBuffer = new Buffer
                    {
                        Type = Buffer.TypeEnum.ZEROS,
                        BinaryByteLength = 8
                    };
                    mBufferList.Add(mBuffer);

                    // Add the newly replaced XML ascii potortion to the buffer list 
                    mBuffer = new Buffer
                    {
                        Type = Buffer.TypeEnum.ASCII,
                        AsciiData = xisfString
                    };
                    mBufferList.Add(mBuffer);

                    // Pad from current position (which is the end of xisfString xml) to the start of image data
                    // This is here because it is difficult to determine where the end position is of xisfString. 
                    mBuffer = new Buffer
                    {
                        Type = Buffer.TypeEnum.POSITION,
                        ToPosition = imageStart
                    };
                    mBufferList.Add(mBuffer);

                    // Add the binary image data from rawFileData after padding
                    mBuffer = new Buffer
                    {
                        Type = Buffer.TypeEnum.BINARY,
                        BinaryDataStart = mFile.ImageAttachmentStart,
                        BinaryByteLength = mFile.ImageAttachmentLength,
                        BinaryData = rawFileData
                    };
                    mBufferList.Add(mBuffer);

                    if (mFile.ThumbnailAttachmentLength > 0)
                    {
                        // Add the binary thumbnail image data from rawFileData after image data and padding
                        mBuffer = new Buffer
                        {
                            Type = Buffer.TypeEnum.BINARY,
                            BinaryDataStart = mFile.ThumbnailAttachmentStart,
                            BinaryByteLength = mFile.ThumbnailAttachmentLength,
                            BinaryData = rawFileData
                        };
                        mBufferList.Add(mBuffer);
                    }

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Now that the mBuffer List is done, write the XISF File
                    bool bStatus = WriteBinaryFile(mFile.SourceFileName);
                    if (bStatus == false)
                        return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "UpdateFiles(XisfFile.XisfFile " + mFile.SourceFileName + ")");
                return false;
            }


            // After Keyword update, move rejected file (Approved = false) to the "Rejected" subdirectory
            if (mFile.KeywordData.Approved() == false)
            {
                sourceFilePath = Path.GetDirectoryName(mFile.SourceFileName);
                Directory.CreateDirectory(sourceFilePath + "\\Rejected");
                File.Move(mFile.SourceFileName, sourceFilePath + "\\Rejected\\" + Path.GetFileName(mFile.SourceFileName));
             }

            return true;
        }
        // ##############################################################################################################################################
        // ##############################################################################################################################################

        private static int UpdateImageAttachmentLocationsXml(XmlDocument document, int imageStart, int imageLength, int thumbnailStart, int thumbnailLength)
        {
            XmlNodeList nodeList;
            int remainder;

            remainder = imageStart % 256;

            imageStart += (256 - remainder);

            nodeList = document.GetElementsByTagName("Image", document.DocumentElement.NamespaceURI);

            foreach (XmlNode node in nodeList)
            {
                if (node.Name.Equals("Image"))
                {
                    node.Attributes["location"].Value = "attachment:" + imageStart.ToString() + ":" + imageLength.ToString();

                    XmlNodeList childNodeList = node.ChildNodes;

                    foreach (XmlNode childNode in childNodeList)
                    {
                        if (childNode.Name.Equals("Thumbnail"))
                        {
                            childNode.Attributes["location"].Value = "attachment:" + thumbnailStart.ToString() + ":" + thumbnailLength.ToString();
                        }
                    }
                }
            }

            return imageStart;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void ReplaceAllFitsKeywords(XmlDocument document, XisfFile mFile, SubFrameLists SubFrameSelectorKeywordLists)
        {
            // First remove all FITSKeywords (nodes) from the xml document
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
                // If Operation != OperationEnum.KEEP_WEIGHTS, remove (if existing) and then add (i.e. replace) the keywords from PixInsight's SubFrame Selector.
                // The "replaced" keywords are the orignal xisf file keywords and are replaced with the keywords stored in mFile.KeywordData.KeywordList.
                // Existing Weights are kept by NOT replacing or adding new ones.
                if (Operation != OperationEnum.KEEP_WEIGHTS)
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
                List<Keyword> keywords = mFile.KeywordData.KeywordList.OrderBy(p => p.Name).ToList();

                // Now add all FITSKeywords found in KeywordData to the xmlDocument
                foreach (Keyword keyword in keywords)
                {
                    if (keyword.Type != Keyword.EType.NULL)
                    {
                        // Create a FITSKeyword under <Image
                        var newElement = document.CreateElement("FITSKeyword", document.DocumentElement.NamespaceURI);
                        newElement.SetAttribute("name", keyword.Name);
                        switch (keyword.Type)
                        {
                            case Keyword.EType.COPY:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case Keyword.EType.BOOL:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case Keyword.EType.FLOAT:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case Keyword.EType.INTEGER:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case Keyword.EType.STRING:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                        }
                        newElement.SetAttribute("comment", keyword.Comment);

                        item.AppendChild(newElement);
                    }
                }
            }
        }

        public static void UpdateCsvWeightList(SubFrameNumericLists WeightLists, SubFrameLists CsvWeightLists)
        {
            int index = 0;

            foreach (double value in WeightLists.Weight)
            {
                CsvWeightLists.SubFrameList.Weight[index].Value = WeightLists.Weight[index].ToString();
                index++;
            }
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void ReplaceSubFrameSelectorKeywords(SubFrameLists CsvWeightLists, XisfFile mFile)
        {
            int indexer;
            List<Keyword> fileNameList = CsvWeightLists.SubFrameList.FileName;

            indexer = 0;
            foreach (Keyword file in fileNameList)
            {
                string fileName = file.Value.Replace("\"", "").Replace("/", @"\");

                // This equality check makes sure the correct data is used for the current file.
                if (mFile.SourceFileName == fileName)
                {
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.Approved[indexer].Name, CsvWeightLists.SubFrameList.Approved[indexer].Value, CsvWeightLists.SubFrameList.Approved.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.Eccentricity.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.Eccentricity.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.Eccentricity.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.EccentricityMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.EccentricityMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.EccentricityMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.Fwhm.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.Fwhm.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.Fwhm.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.FwhmMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.FwhmMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.FwhmMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.Median.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.Median.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.Median.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.MedianMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.MedianMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.MedianMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.Noise.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.Noise.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.Noise.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.NoiseRatio.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.NoiseRatio.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.NoiseRatio.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.SnrWeight.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.SnrWeight.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.SnrWeight.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.StarResidual.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.StarResidual.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.StarResidual.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.StarResidualMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.StarResidualMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.StarResidualMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.Stars.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.Stars.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.Stars.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SubFrameList.Weight.ElementAt(indexer).Name, CsvWeightLists.SubFrameList.Weight.ElementAt(indexer).Value, CsvWeightLists.SubFrameList.Weight.ElementAt(indexer).Comment);
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
                                case Buffer.TypeEnum.ASCII:
                                    binaryWriter.Write(Encoding.UTF8.GetBytes(buffer.AsciiData), 0, Encoding.UTF8.GetBytes(buffer.AsciiData).Length);
                                    break;

                                case Buffer.TypeEnum.BINARY:
                                    binaryWriter.Write(buffer.BinaryData, buffer.BinaryDataStart, buffer.BinaryByteLength);
                                    break;

                                case Buffer.TypeEnum.ZEROS:
                                    for (int i = (int)position; i < buffer.BinaryByteLength + position; i++)
                                    {
                                        binaryWriter.Write(zero, 0, 1);
                                    }
                                    break;

                                case Buffer.TypeEnum.POSITION:
                                    if ((int)position > buffer.ToPosition)
                                    {
                                        MessageBox.Show(fileName + "\n\nThe length of xml xisfString is after the start of image data:\n" +
                                            "    Current Write Position:     " + position.ToString() + "\n" +
                                            "    Image Attachment Start Position: " + buffer.ToPosition.ToString() + "\n\nAborting.",
                                            "MainForm.cs WriteBinaryFile()",
                                            MessageBoxButtons.OK);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "WriteBinaryFile(" + fileName + ")");
                return false;
            }

            // ##############################################################################################################################################
            // ##############################################################################################################################################

        }
    }
}
