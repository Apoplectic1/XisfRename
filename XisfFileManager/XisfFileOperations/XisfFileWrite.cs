﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using XisfFileManager.XisfKeywords;

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

        public static bool UpdateFiles(XisfFile.XisfFile mFile, SubFrameData.SubFrameData csvKeywordLists)
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
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xisfString);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    mFile.KeywordData.RepairTargetName(TargetName);
                    mFile.KeywordData.RemoveKeyword("HISTORY");

                    // Replace all existing FITSKeywords with FITSKeywords from our list (mFile.KeywordList)
                    ReplaceAllFitsKeywords(doc, mFile, AddCsvKeywords, csvKeywordLists);

                    //RemoveSingleNumberSingleQuotes(doc);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************
                    // Begin setting up output XISF File
                    // Create a mBuffer to hold our different data types (Binary, Text, Binary Zero's, etc.
                    // Add each mBuffer to a List and when complete, sequentially write each List element over our XISF File

                    // Fixes for PixInsight Parser
                    xisfString = doc.OuterXml.Replace(" /", "/").Replace("'", "");

                    //xisfString = xisfString.Replace(" /", "/"); // PixInsight throws up with spaces before the '/'

                    // Add header (includes binary and string portions) up the start of "<xisf version"
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.ASCII;
                    mBuffer.AsciiData = "XISF0100";
                    mBufferList.Add(mBuffer);

                    // Write xml portion length to XISF Header - See PixInsight XISF Developer info 
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.BINARY;
                    byte[] headerLength = { Convert.ToByte((xisfString.Length >> 0) & 0xff), Convert.ToByte((xisfString.Length >> 8) & 0xff), Convert.ToByte((xisfString.Length >> 216) & 0xff), Convert.ToByte((xisfString.Length >> 24) & 0xff) };
                    mBuffer.BinaryData = headerLength;
                    mBuffer.BinaryByteLength = 4;
                    mBufferList.Add(mBuffer);

                    // Write reserved bytes as 0's
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.ZEROS;
                    mBuffer.BinaryByteLength = 4;
                    mBufferList.Add(mBuffer);


                    // Add the newly replaced XML ascii potortion to the buffer list 
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.ASCII;
                    mBuffer.AsciiData = xisfString;
                    mBufferList.Add(mBuffer);

                    // Pad zero's after </xisf> to the start of binary image data
                    mBuffer = new XisfFile.Buffer();
                    mBuffer.Type = XisfFile.Buffer.TypeEnum.ZEROS;
                    mBuffer.BinaryDataStart = 0;
                    mBuffer.BinaryByteLength = mFile.ImageAttachmentStart - xisfString.Length - xmlStart;
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
                        // Pad zero's after image data to the start of the thumbnail image
                        mBuffer = new XisfFile.Buffer();
                        mBuffer.Type = XisfFile.Buffer.TypeEnum.ZEROS;
                        mBuffer.BinaryDataStart = 0;
                        mBuffer.BinaryByteLength = mFile.ThumbnailAttachmentStart - (mFile.ImageAttachmentStart + mFile.ImageAttachmentLength);
                        mBufferList.Add(mBuffer);

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

        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void ReplaceAllFitsKeywords(XmlDocument document, XisfFile.XisfFile mFile, bool enable, SubFrameData.SubFrameData csvKeywordLists)
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
                    AddCsvKeywordList(enable, csvKeywordLists, mFile);

                // Add the FITSKeywords in alphabetical order - Not needed but WTF
                List<Keyword> keywords = mFile.KeywordData.KeywordList.OrderBy(p => p.Name).ToList();

                foreach (Keyword keyword in keywords)
                {
                    if (keyword.Type != XisfKeywords.Keyword.EType.NULL)
                    {
                        // Create a FITSKeyword under <Image
                        var newElement = document.CreateElement("FITSKeyword", document.DocumentElement.NamespaceURI);
                        newElement.SetAttribute("name", keyword.Name);
                        switch (keyword.Type)
                        {
                            case XisfKeywords.Keyword.EType.COPY:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case XisfKeywords.Keyword.EType.BOOL:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case XisfKeywords.Keyword.EType.FLOAT:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case XisfKeywords.Keyword.EType.INTEGER:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                            case XisfKeywords.Keyword.EType.STRING:
                                newElement.SetAttribute("value", keyword.Value);
                                break;
                        }
                        newElement.SetAttribute("comment", keyword.Comment);

                        item.AppendChild(newElement);
                    }
                }
            }
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private static void AddCsvKeywordList(bool enable, SubFrameData.SubFrameData csvKeywordLists, XisfFile.XisfFile mFile)
        {

            List<Keyword> fileNameList = csvKeywordLists.FileName;

            foreach (Keyword file in fileNameList)
            {
                int indexer = 0;
                string fileName = file.Value.Replace("\"", "").Replace("/", @"\");

                if (mFile.SourceFileName == fileName)
                {
                    mFile.KeywordData.AddKeyword(csvKeywordLists.Approved.ElementAt(indexer).Name, csvKeywordLists.Approved.ElementAt(indexer).Value, csvKeywordLists.Approved.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.Eccentricity.ElementAt(indexer).Name, csvKeywordLists.Eccentricity.ElementAt(indexer).Value, csvKeywordLists.Eccentricity.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.EccentricityMeanDeviation.ElementAt(indexer).Name, csvKeywordLists.EccentricityMeanDeviation.ElementAt(indexer).Value, csvKeywordLists.EccentricityMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.Fwhm.ElementAt(indexer).Name, csvKeywordLists.Fwhm.ElementAt(indexer).Value, csvKeywordLists.Fwhm.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.FwhmMeanDeviation.ElementAt(indexer).Name, csvKeywordLists.FwhmMeanDeviation.ElementAt(indexer).Value, csvKeywordLists.FwhmMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.Median.ElementAt(indexer).Name, csvKeywordLists.Median.ElementAt(indexer).Value, csvKeywordLists.Median.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.MedianMeanDeviation.ElementAt(indexer).Name, csvKeywordLists.MedianMeanDeviation.ElementAt(indexer).Value, csvKeywordLists.MedianMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.Noise.ElementAt(indexer).Name, csvKeywordLists.Noise.ElementAt(indexer).Value, csvKeywordLists.Noise.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.NoiseRatio.ElementAt(indexer).Name, csvKeywordLists.NoiseRatio.ElementAt(indexer).Value, csvKeywordLists.NoiseRatio.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.SnrWeight.ElementAt(indexer).Name, csvKeywordLists.SnrWeight.ElementAt(indexer).Value, csvKeywordLists.SnrWeight.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.StarResidual.ElementAt(indexer).Name, csvKeywordLists.StarResidual.ElementAt(indexer).Value, csvKeywordLists.StarResidual.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.StarResidualMeanDeviation.ElementAt(indexer).Name, csvKeywordLists.StarResidualMeanDeviation.ElementAt(indexer).Value, csvKeywordLists.StarResidualMeanDeviation.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.Stars.ElementAt(indexer).Name, csvKeywordLists.Stars.ElementAt(indexer).Value, csvKeywordLists.Stars.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.SSWeight.ElementAt(indexer).Name, csvKeywordLists.SSWeight.ElementAt(indexer).Value, csvKeywordLists.SSWeight.ElementAt(indexer).Comment);
                    mFile.KeywordData.AddKeyword(csvKeywordLists.FileName.ElementAt(indexer).Name, csvKeywordLists.FileName.ElementAt(indexer).Value, csvKeywordLists.FileName.ElementAt(indexer).Comment);
                }

                indexer++;
            }
        }

        private static void RemoveSingleNumberSingleQuotes(string xmlString)
        {

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
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(stream);

                foreach (XisfFile.Buffer buffer in mBufferList)
                {
                    switch (buffer.Type)
                    {
                        case XisfFile.Buffer.TypeEnum.ASCII:
                            bw.Write(Encoding.UTF8.GetBytes(buffer.AsciiData));
                            break;

                        case XisfFile.Buffer.TypeEnum.BINARY:
                            bw.Write(buffer.BinaryData, buffer.BinaryDataStart, buffer.BinaryByteLength);
                            break;

                        case XisfFile.Buffer.TypeEnum.ZEROS:
                            for (int i = 0; i < buffer.BinaryByteLength; i++)
                            {
                                bw.Write((byte)0x00);
                            }
                            break;
                    }
                }

                bw.Flush();
                bw.Close();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "WriteBinaryFile(" + fileName + ")");
                return false;
            }
        }

        // ##############################################################################################################################################
        // ##############################################################################################################################################

    }
}
