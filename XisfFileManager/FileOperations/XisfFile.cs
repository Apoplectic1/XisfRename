using System;
using System.Xml.Linq;
using XisfFileManager.Keywords;

namespace XisfFileManager.FileOperations
{
    public class XisfFile
    {
        public int ImageAttachmentLength { get; set; } = 0;
        public int ImageAttachmentStart { get; set; } = 0;
        public int ThumbnailAttachmentLength { get; set; } = 0;
        public int ThumbnailAttachmentStart { get; set; } = 0;
        public string SourceFileName { get; set; }
        public KeywordLists KeywordData { get; set; }
        public bool Unique;

        public XisfFile()
        {
            KeywordData = new KeywordLists();
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

        public static Comparison<XisfFile> CaptureTimeComparison = delegate (XisfFile object1, XisfFile object2)
        {
            if (object1 == null) return 1;
            if (object2 == null) return 1;
            if (object1.KeywordData.CaptureDateTime() > object2.KeywordData.CaptureDateTime()) return 1;
            if (object1.KeywordData.CaptureDateTime() < object2.KeywordData.CaptureDateTime()) return -1;
            return 0;
        };
    }
}

