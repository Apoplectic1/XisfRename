using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XisfFileManager.Calculations;
using XisfFileManager.Keywords;

namespace XisfFileManager.XisfFileOperations
{
    public static class XisfFileWrite
    {
        public static XisfFile.Buffer mBuffer;
        public static List<XisfFile.Buffer> mBufferList;
        public static string TargetName { get; set; }
        public static bool AddCsvKeywords { get; set; } = false;


        // ##############################################################################################################################################
        // ##############################################################################################################################################

        public static bool UpdateFiles(XisfFile.XisfFile mFile, SubFrameKeywordLists SubFrameKeywordLists)
        {
            int xmlStart;
            int xisfStart;
            int xisfEnd;
            byte[] rawFileData = new byte[(int)1e9];
            mBufferList = new List<XisfFile.Buffer>();

            try
            {
                using (Stream stream = new FileStream(mFile.SourceFileName, FileMode.Open))
                {
                    mBufferList.Clear();

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Read entire XISF file (up to 1 GB) into rawFileData and create an xml document

                    //Stream stream = new FileStream(mFile.SourceFileName, FileMode.Open);
                    BinaryReader bw = new BinaryReader(stream);
                    rawFileData = bw.ReadBytes((int)1e9);
                    bw.Close();

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

                    mFile.KeywordData.RepairTargetName(TargetName);
                    mFile.KeywordData.RemoveKeyword("HISTORY");

                    // Replace all existing FITSKeywords with FITSKeywords from our list (mFile.KeywordList)
                    ReplaceAllFitsKeywords(xmlDoc, mFile, AddCsvKeywords, SubFrameKeywordLists);

                    int imageStart;
                    //Compute new Image Attachment Start Locations
                    imageStart = UpdateImageAttachmentLocationsXml(xmlDoc,
                        xmlDoc.OuterXml.Replace(" /", "/").Replace("'", "").Length + 16,
                        mFile.ImageAttachmentLength,
                        xmlDoc.OuterXml.Replace(" /", "/").Replace("'", "").Length + 16 + mFile.ImageAttachmentLength,
                        mFile.ThumbnailAttachmentLength);
                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Begin setting up output XISF File
                    // Create a mBuffer to hold our different data types (Binary, Text, Binary Zero's, etc.)
                    // Add each mBuffer to a List and when complete, sequentially write each List element over our XISF File

                    xisfString = xmlDoc.OuterXml;

                    // Add header (includes binary and string portions) up the start of "<xisf version"
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.ASCII;
                    mBuffer.AsciiData = "XISF0100";
                    mBufferList.Add(mBuffer);

                    // Write length (filled out later) and reserved bytes as 0's
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.ZEROS;
                    mBuffer.BinaryByteLength = 8;
                    mBufferList.Add(mBuffer);

                    // Add the newly replaced XML ascii potortion to the buffer list 
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.ASCII;
                    mBuffer.AsciiData = xisfString;
                    mBufferList.Add(mBuffer);

                    // Pad from current position (which is the end of xisfString xml) to the start of image data
                    // This is here because it is difficult to consistently determine where the end position is of xisfString. This may be a programming error.
                    // This "error" may also cause problems for the header lenght field above.
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.POSITION;
                    mBuffer.ToPosition = imageStart;
                    mBufferList.Add(mBuffer);

                    // Add the binary image data from rawFileData after padding
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.BINARY;
                    mBuffer.BinaryDataStart = mFile.ImageAttachmentStart;
                    mBuffer.BinaryByteLength = mFile.ImageAttachmentLength;
                    mBuffer.BinaryData = rawFileData;
                    mBufferList.Add(mBuffer);

                    if (mFile.ThumbnailAttachmentLength > 0)
                    {
                        // Add the binary thumbnail image data from rawFileData after image data and padding
                        mBuffer = new XisfFile.Buffer();
                        mBuffer.Type = XisfFile.Buffer.TypeEnum.BINARY;
                        mBuffer.BinaryDataStart = mFile.ThumbnailAttachmentStart;
                        mBuffer.BinaryByteLength = mFile.ThumbnailAttachmentLength;
                        mBuffer.BinaryData = rawFileData;
                        mBufferList.Add(mBuffer);
                    }

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Now that the mBuffer List is done, write the XISF File
                    WriteBinaryFile(mFile.SourceFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "UpdateFiles(XisfFile.XisfFile " + mFile.SourceFileName + ")");
                return false;
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

        private static void ReplaceAllFitsKeywords(XmlDocument document, XisfFile.XisfFile mFile, bool enable, SubFrameKeywordLists CsvSubFrameKeywordLists)
        {
            // First Clean Up by removing all FITSKeywords
            XmlNodeList nodeList = document.GetElementsByTagName("FITSKeyword");
            for (int i = nodeList.Count - 1; i >= 0; i--)
            {
                nodeList[i].ParentNode.RemoveChild(nodeList[i]);
            }


            // Find the <Image tag - We are going to add the updated FITSKeyword list under here
            nodeList = document.GetElementsByTagName("Image");

            foreach (XmlNode item in nodeList)
            {
                if (enable)
                    AddCsvKeywordList(enable, CsvSubFrameKeywordLists, mFile);

                // Add the FITSKeywords in alphabetical order - Not needed but WTF
                List<Keyword> keywords = mFile.KeywordData.KeywordList.OrderBy(p => p.Name).ToList();

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

        public static void UpdateCsvSSWeightList(SubFrameWeights WeightLists, SubFrameKeywordLists CsvWeightLists)
        {
            int index = 0;

            foreach (double value in WeightLists.SSWeight)
            {
                CsvWeightLists.SSWeight[index].Value = WeightLists.SSWeight[index].ToString();
                index++;
            }
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void AddCsvKeywordList(bool enable, SubFrameKeywordLists CsvWeightLists, XisfFile.XisfFile mFile)
        {

            List<Keyword> fileNameList = CsvWeightLists.FileName;

            foreach (Keyword file in fileNameList)
            {
                int indexer = 0;
                string fileName = file.Value.Replace("\"", "").Replace("/", @"\");

                if (mFile.SourceFileName == fileName)
                {
                    mFile.KeywordData.AddKeyword(CsvWeightLists.Approved[indexer].Name, CsvWeightLists.Approved[indexer].Value, CsvWeightLists.Approved.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.Eccentricity.ElementAt(indexer).Name, CsvWeightLists.Eccentricity.ElementAt(indexer).Value, CsvWeightLists.Eccentricity.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.EccentricityMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.EccentricityMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.EccentricityMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.Fwhm.ElementAt(indexer).Name, CsvWeightLists.Fwhm.ElementAt(indexer).Value, CsvWeightLists.Fwhm.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.FwhmMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.FwhmMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.FwhmMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.Median.ElementAt(indexer).Name, CsvWeightLists.Median.ElementAt(indexer).Value, CsvWeightLists.Median.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.MedianMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.MedianMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.MedianMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.Noise.ElementAt(indexer).Name, CsvWeightLists.Noise.ElementAt(indexer).Value, CsvWeightLists.Noise.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.NoiseRatio.ElementAt(indexer).Name, CsvWeightLists.NoiseRatio.ElementAt(indexer).Value, CsvWeightLists.NoiseRatio.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SnrWeight.ElementAt(indexer).Name, CsvWeightLists.SnrWeight.ElementAt(indexer).Value, CsvWeightLists.SnrWeight.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.StarResidual.ElementAt(indexer).Name, CsvWeightLists.StarResidual.ElementAt(indexer).Value, CsvWeightLists.StarResidual.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.StarResidualMeanDeviation.ElementAt(indexer).Name, CsvWeightLists.StarResidualMeanDeviation.ElementAt(indexer).Value, CsvWeightLists.StarResidualMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.Stars.ElementAt(indexer).Name, CsvWeightLists.Stars.ElementAt(indexer).Value, CsvWeightLists.Stars.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(CsvWeightLists.SSWeight.ElementAt(indexer).Name, CsvWeightLists.SSWeight.ElementAt(indexer).Value, CsvWeightLists.SSWeight.ElementAt(indexer).Comment);
                    //mFile.KeywordData.AddKeyword(CsvWeightLists.FileName.ElementAt(indexer).Name, CsvWeightLists.FileName.ElementAt(indexer).Value, CsvWeightLists.FileName.ElementAt(indexer).Comment);
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
                    using (BinaryWriter rawWriter = new BinaryWriter(rawStream))
                    {
                        foreach (XisfFile.Buffer buffer in mBufferList)
                        {
                            long position = rawStream.Position;

                            switch (buffer.Type)
                            {
                                case XisfFile.Buffer.TypeEnum.ASCII:
                                    rawWriter.Write(Encoding.UTF8.GetBytes(buffer.AsciiData), 0, Encoding.UTF8.GetBytes(buffer.AsciiData).Length);
                                    break;

                                case XisfFile.Buffer.TypeEnum.BINARY:
                                    rawWriter.Write(buffer.BinaryData, buffer.BinaryDataStart, buffer.BinaryByteLength);
                                    break;

                                case XisfFile.Buffer.TypeEnum.ZEROS:
                                    for (int i = (int)position; i < buffer.BinaryByteLength + position; i++)
                                    {
                                        rawWriter.Write(zero, 0, 1);
                                    }
                                    break;

                                case XisfFile.Buffer.TypeEnum.POSITION:
                                    if ((int)position > buffer.ToPosition)
                                    {
                                        MessageBox.Show("The length of xml xisfString is after the start of image data:\n" +
                                            "Current Position:     " + position.ToString() + "\n" +
                                            "Image Start Position: " + buffer.ToPosition.ToString(),
                                            "MainForm.cs WriteBinaryFile(" + fileName + ")");
                                        return false;
                                    }

                                    for (long i = position; i < buffer.ToPosition; i++)
                                    {
                                        rawWriter.Write(zero, 0, 1);
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
                        using (BinaryWriter rawWriter = new BinaryWriter(fileStream))
                        {
                            fileStream.Write(binaryData, 0, binaryDataLength);
                            rawWriter.Flush();
                            rawWriter.Close();
                        }
                    }
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
