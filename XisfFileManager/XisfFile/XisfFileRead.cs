using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using XisfFileManager.Keywords;

namespace XisfFileManager.XisfFile
{
    public class XisfFileRead
    {
        public KeywordData mKeywordData;
        private XDocument mXDoc;
        private char[] mBuffer;
        public List<Keyword> mKeywordList;
        public bool ValidFile { get; set; } = false;
        public int ImageAttachmentLength = 0;
        public int ImageAttachmentStart = 0;
        public int ThumbnailAttachmentLength = 0;
        public int ThumbnailAttachmentStart = 0;
        public string SourceFileName { get; set; }
        public string mXmlString;


        public XisfFileRead()
        {
            mBuffer = new char[0x3000];
            mKeywordList = new List<Keyword>();
            mKeywordData = new KeywordData();
        }

        // ************************************************************************************************
        // ************************************************************************************************

        public void ImageAttachment(XElement element)
        {
            XAttribute attribute = element.Attribute("location");

            if (attribute != null)
            {
                string attachment = attribute.Value;

                string[] values = attachment.Split(':');

                ImageAttachmentStart = Convert.ToInt32(values[1]);
                ImageAttachmentLength = Convert.ToInt32(values[2]);
            }
        }

        public void ThumbnailAttachment(XElement element)
        {
            XAttribute attribute = element.Attribute("location");

            if (attribute != null)
            {
                string attachment = attribute.Value;

                string[] values = attachment.Split(':');

                ThumbnailAttachmentStart = Convert.ToInt32(values[1]);
                ThumbnailAttachmentLength = Convert.ToInt32(values[2]);
            }
        }

        public void AddFITSKeyword(XElement element)
        {
            Keyword keyword = new Keyword();
            keyword.Type = Keyword.KeywordType.COPY;
            keyword.Name = element.Attribute("name").Value;
            keyword.SetValue = element.Attribute("value").Value;
            keyword.Comment = element.Attribute("comment").Value;
            mKeywordList.Add(keyword);
        }

        public bool ParseXisfFile()
        {
            using (StreamReader reader = new StreamReader(SourceFileName))
            {
                reader.Read(mBuffer, 0, mBuffer.Length);
            }

            mXmlString = new string(mBuffer);

            mXmlString = mXmlString.Substring(mXmlString.IndexOf("<?xml"));
            mXmlString = mXmlString.Substring(0, mXmlString.LastIndexOf(@"</xisf>") + 7);

            try
            {
                mXDoc = XDocument.Parse(mXmlString);
            }
            catch
            {
                ValidFile = false;
                return false;
            }

            XElement root = mXDoc.Root;
            XNamespace ns = root.GetDefaultNamespace();

            IEnumerable<XElement> image = from c in mXDoc.Descendants(ns + "Image") select c;
            foreach (XElement element in image)
            {
                ImageAttachment(element);
            }


            IEnumerable<XElement> thumbnail = from c in mXDoc.Descendants(ns + "Thumbnail") select c;
            foreach (XElement element in thumbnail)
            {
                ThumbnailAttachment(element);
            }

            IEnumerable<XElement> elements = from c in mXDoc.Descendants(ns + "FITSKeyword") select c;

            // Find each relevent keyword and add it to mFile
            foreach (XElement element in elements)
            {
                AddFITSKeyword(element);
            }

            mKeywordData.RepairSiteLatitude(mKeywordList);
            mKeywordData.RepairSiteLongitude(mKeywordList);

            ValidFile = true;
            return true;
        }
    }
}

