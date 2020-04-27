using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using LocalLib;

namespace XisfRename.Parse
{
    public class UpdateXisfFile
    {
        private Buffer mBuffer;
        private List<Buffer> mBufferList;

        public string NewTargetName { get; set; }

        public UpdateXisfFile()
        {
            mBufferList = new List<Buffer>();
        }

        // ****************************************************************************************************
        // ****************************************************************************************************
        public bool UpdateFiles(List<Parse.XisfFile> mFileList)
        {
            int xmlStart; 
            int xisfStart;
            int xisfEnd;
           

            byte[] rawFileData = new byte[(int)1e9];

            foreach (XisfFile mFile in mFileList)
            {
                try
                {
                        mBufferList.Clear();

                        Stream stream = new FileStream(mFile.SourceFileName, FileMode.Open);
                        BinaryReader bw = new BinaryReader(stream);
                        rawFileData = bw.ReadBytes((int)1e9);
                        bw.Close();

                        xmlStart = BinaryFind(rawFileData, "<?xml version"); // returns the position of '<'
                        xisfStart = BinaryFind(rawFileData, "<xisf version"); // returns the position of '<'
                        xisfEnd = BinaryFind(rawFileData, "</xisf>") + "</xisf>".Length;  // returns the position immediately after '>'                

                        // Add header (includes binary and string portions) up the start of "<xisf version"
                        mBuffer = new Buffer();
                        mBuffer.Type = Buffer.TypeEnum.BINARY;
                        mBuffer.BinaryStart = 0;
                        mBuffer.BinaryLength = xmlStart;
                        mBuffer.Binary = rawFileData;
                        mBufferList.Add(mBuffer);

                        // convert (including) from <xisf to </xisf> to string and then parse xml into a new doc
                        string xisfString = Encoding.UTF8.GetString(rawFileData, xmlStart, xisfEnd);
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xisfString);

                        ReplaceObjectName(doc, NewTargetName);
                        ReplaceSiteLatitude(doc, mFile.SITELAT);
                        ReplaceSiteLongitude(doc, mFile.SITELON);

                        // Fixes for PixInsight Parser
                        string newXisfString = doc.OuterXml;

                        newXisfString = newXisfString.Replace(" /", "/"); // PixInsight throws up with spaces before the '/'

                        // Add the Complete XML ascii potortion to the buffer list - after OBJECT has been replaced in the returned ObjectName string
                        mBuffer = new Buffer();
                        mBuffer.Type = Buffer.TypeEnum.ASCII;
                        mBuffer.ASCII = newXisfString;
                        mBufferList.Add(mBuffer);

                        // Pad zero's after </xisf> to start of image
                        mBuffer = new Buffer();
                        mBuffer.Type = Buffer.TypeEnum.ZEROS;
                        mBuffer.BinaryStart = 0;
                        mBuffer.BinaryLength = mFile.AttachmentStart - newXisfString.Length - xmlStart;
                        mBufferList.Add(mBuffer);

                        // Add the binary image data after rawFileData "</xisf>" - not the new one
                        mBuffer = new Buffer();
                        mBuffer.Type = Buffer.TypeEnum.BINARY;
                        mBuffer.BinaryStart = mFile.AttachmentStart;
                        mBuffer.BinaryLength = mFile.AttachmentLength;
                        mBuffer.Binary = rawFileData;
                        mBufferList.Add(mBuffer);


                        WriteBinaryFile(mFile.SourceFileName);

                        
                    }
                    catch(Exception e)
                    {
                        return false;
                    }
            }
            return true;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************
        private void ReplaceObjectName(XmlDocument document, string newObjectName)
        {
            XmlNodeList items = document.GetElementsByTagName("FITSKeyword");

            foreach (XmlNode xItem in items)
            {
                if (xItem.OuterXml.Contains("OBJECT"))
                {
                    xItem.Attributes["value"].Value = "\'" + newObjectName + " \'";
                    return;
                }
            }
        }

        private void ReplaceSiteLatitude(XmlDocument document, string newSiteLatitude)
        {
            XmlNodeList items = document.GetElementsByTagName("FITSKeyword");

            foreach (XmlNode xItem in items)
            {
                if (xItem.OuterXml.Contains("SITELAT"))
                {
                    string value = xItem.Attributes["value"].Value;

                    if (value.Contains("N"))
                    {
                        string replaced = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                        xItem.Attributes["value"].Value = replaced;
                    }
                }
            }
        }

        private void ReplaceSiteLongitude(XmlDocument document, string newSiteLongitude)
        {
            XmlNodeList items = document.GetElementsByTagName("FITSKeyword");

            foreach (XmlNode xItem in items)
            {
                if (xItem.OuterXml.Contains("SITELONG"))
                {
                    string value = xItem.Attributes["value"].Value;

                    if (value.Contains("W"))
                    {
                        string replaced = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                        Regex regReplace = new Regex("'");
                        replaced = regReplace.Replace(replaced, "'-", 1);

                        xItem.Attributes["value"].Value = replaced;
                        return;
                    }

                }
            }
        }

        public int BinaryFind(byte[] buffer, string findText)
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
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(stream);

                foreach (Buffer buffer in mBufferList)
                {
                    switch (buffer.Type)
                    {
                        case Buffer.TypeEnum.ASCII:
                            bw.Write(Encoding.UTF8.GetBytes(buffer.ASCII));
                            break;

                        case Buffer.TypeEnum.BINARY:
                            bw.Write(buffer.Binary, buffer.BinaryStart, buffer.BinaryLength);
                            break;

                        case Buffer.TypeEnum.ZEROS:
                            for (int i = 0; i < buffer.BinaryLength; i++)
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
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
