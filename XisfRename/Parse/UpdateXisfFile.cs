using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using LocalLib;

namespace XisfRename.Parse
{
    public class UpdateXisfFile
    {
        private Buffer mBuffer;
        private List<Buffer> mBufferList;

        enum keywordType { TEXT, INT, FLOAT, BOOL }

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

                    // convert (including) from <xisf to </xisf> to string and then parse xml into a new doc
                    string xisfString = Encoding.UTF8.GetString(rawFileData, xmlStart, xisfEnd);

                    int position = xisfString.IndexOf("<FITSKeyword");  // Find first FITSKeyword


                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xisfString);

                    UpdateFitsKeyword(doc, keywordType.FLOAT, "FWHM", "2.1828", true, "#################################");
                    UpdateFitsKeyword(doc, keywordType.TEXT, "OBJECT", NewTargetName, true);
                    ReplaceSiteLatitude(doc, mFile.SITELAT);
                    ReplaceSiteLongitude(doc, mFile.SITELON);

                    // *******************************************************************************************************************************
                    // *******************************************************************************************************************************

                    // Fixes for PixInsight Parser
                    xisfString = doc.OuterXml.Replace(" /", "/");

                    //xisfString = xisfString.Replace(" /", "/"); // PixInsight throws up with spaces before the '/'

                    // Add header (includes binary and string portions) up the start of "<xisf version"
                    mBuffer = new Buffer();
                    mBuffer.Type = Buffer.TypeEnum.ASCII;
                    mBuffer.AsciiData = "XISF0100";
                    mBufferList.Add(mBuffer);

                    mBuffer = new Buffer();
                    mBuffer.Type = Buffer.TypeEnum.BINARY;
                    byte[] headerLength = { Convert.ToByte((xisfString.Length >> 0) & 0xff), Convert.ToByte((xisfString.Length >> 8) & 0xff), Convert.ToByte((xisfString.Length >> 216) & 0xff), Convert.ToByte((xisfString.Length >> 24) & 0xff) };
                    mBuffer.BinaryData = headerLength;
                    mBuffer.BinaryByteLength = 4;
                    mBufferList.Add(mBuffer);

                    mBuffer = new Buffer();
                    mBuffer.Type = Buffer.TypeEnum.ZEROS;
                    mBuffer.BinaryByteLength = 4;
                    mBufferList.Add(mBuffer);


                    // Add the Complete XML ascii potortion to the buffer list - after OBJECT has been replaced in the returned ObjectName string
                    mBuffer = new Buffer();
                    mBuffer.Type = Buffer.TypeEnum.ASCII;
                    mBuffer.AsciiData = xisfString;
                    mBufferList.Add(mBuffer);

                    // Pad zero's after </xisf> to start of image
                    mBuffer = new Buffer();
                    mBuffer.Type = Buffer.TypeEnum.ZEROS;
                    mBuffer.BinaryDataStart = 0;
                    mBuffer.BinaryByteLength = mFile.ImageAttachmentStart - xisfString.Length - xmlStart;
                    mBufferList.Add(mBuffer);

                    // Add the binary image data after rawFileData "</xisf>" - not the new one
                    mBuffer = new Buffer();
                    mBuffer.Type = Buffer.TypeEnum.BINARY;
                    mBuffer.BinaryDataStart = mFile.ImageAttachmentStart;
                    mBuffer.BinaryByteLength = mFile.ImageAttachmentLength;
                    mBuffer.BinaryData = rawFileData;
                    mBufferList.Add(mBuffer);

                    if (mFile.ThumbnailAttachmentLength > 0)
                    {
                        // Pad zero's after </xisf> to start of image
                        mBuffer = new Buffer();
                        mBuffer.Type = Buffer.TypeEnum.ZEROS;
                        mBuffer.BinaryDataStart = 0;
                        mBuffer.BinaryByteLength = mFile.ThumbnailAttachmentStart - (mFile.ImageAttachmentStart + mFile.ImageAttachmentLength);
                        mBufferList.Add(mBuffer);

                        // Add the binary image data after rawFileData "</xisf>" - not the new one
                        mBuffer = new Buffer();
                        mBuffer.Type = Buffer.TypeEnum.BINARY;
                        mBuffer.BinaryDataStart = mFile.ThumbnailAttachmentStart;
                        mBuffer.BinaryByteLength = mFile.ThumbnailAttachmentLength;
                        mBuffer.BinaryData = rawFileData;
                        mBufferList.Add(mBuffer);
                    }

                    WriteBinaryFile(mFile.SourceFileName);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private void ReplaceSiteLatitude(XmlDocument document, string newSiteLatitude)
        {
            // Fix old SGPro SITELAT format error - this method should not be needed in the future 
            XmlNodeList items = document.GetElementsByTagName("FITSKeyword");

            foreach (XmlNode xItem in items)
            {
                if (xItem.Attributes[0].Value.Equals("SITELAT"))
                {
                    string value = xItem.Attributes[1].Value;

                    if (value.Contains("N"))
                    {
                        string replaced = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                        xItem.Attributes[1].Value = replaced;
                    }
                }
            }
        }

        private void ReplaceSiteLongitude(XmlDocument document, string newSiteLongitude)
        {
            // Fix old SGPro SITELONG format error - this method should not be needed in the future 
            XmlNodeList items = document.GetElementsByTagName("FITSKeyword");

            foreach (XmlNode xItem in items)
            {
                if (xItem.Attributes[0].Value.Equals("SITELONG"))
                {
                    string value = xItem.Attributes[1].Value;

                    if (value.Contains("W"))
                    {
                        string replaced = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                        Regex regReplace = new Regex("'");
                        replaced = regReplace.Replace(replaced, "'-", 1);

                        xItem.Attributes[1].Value = replaced;
                        return;
                    }

                }
            }
        }

        private void UpdateFitsKeyword(XmlDocument document, keywordType type, string fitsKeyword, string value, bool update = false, string comment = "")
        {
            if (update)
            {
                XmlNodeList items = document.GetElementsByTagName("FITSKeyword");

                foreach (XmlNode xItem in items)
                {
                    if (xItem.Attributes[0].Value.Equals(fitsKeyword))
                    {
                        switch (type)
                        {
                            case keywordType.BOOL:
                                xItem.Attributes[1].Value = value;
                                break;
                            case keywordType.FLOAT:
                                xItem.Attributes[1].Value = value;
                                break;
                            case keywordType.INT:
                                xItem.Attributes[1].Value = value;
                                break;
                            case keywordType.TEXT:
                                xItem.Attributes[1].Value = "\'" + value + " \'";
                                break;
                        }
                        xItem.Attributes[1].Value = "\'" + value + " \'";
                        xItem.Attributes[2].Value = (comment == "") ? xItem.Attributes[2].Value : comment;
                        return;
                    }
                }

                items = document.GetElementsByTagName("Image");

                // Didn't find the 'fitsSKeyword' - So create and add it at the beginning of FITSKeywords (as first child of Image)

                var newElement = document.CreateElement("FITSKeyword", document.DocumentElement.NamespaceURI);
                newElement.SetAttribute("name", fitsKeyword);
                switch (type)
                {
                    case keywordType.BOOL:
                        newElement.SetAttribute("value", value);
                        break;
                    case keywordType.FLOAT:
                        newElement.SetAttribute("value", value);
                        break;
                    case keywordType.INT:
                        newElement.SetAttribute("value", value);
                        break;
                    case keywordType.TEXT:
                        newElement.SetAttribute("value", "\'" + value + " \'");
                        break;
                }
                newElement.SetAttribute("comment", comment);

                foreach (XmlNode xItem in items)
                {
                    xItem. PrependChild(newElement);
                    return;
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
                            bw.Write(Encoding.UTF8.GetBytes(buffer.AsciiData));
                            break;

                        case Buffer.TypeEnum.BINARY:
                            bw.Write(buffer.BinaryData, buffer.BinaryDataStart, buffer.BinaryByteLength);
                            break;

                        case Buffer.TypeEnum.ZEROS:
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
            catch
            {
                return false;
            }

        }
    }
}
