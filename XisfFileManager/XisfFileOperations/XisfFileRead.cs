using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XisfFileManager.Keywords;

namespace XisfFileManager.XisfFileOperations
{
    public static class XisfFileRead
    {
        private static char[] mBuffer;
        private static string mXmlString;
        private static XDocument mXDoc;

        public static bool ReadXisfFile(XisfFile.XisfFile xFile)
        {
            using (StreamReader reader = new StreamReader(xFile.SourceFileName))
            {
                mBuffer = new char[0x3000];
                reader.Read(mBuffer, 0, mBuffer.Length);

                mXmlString = new string(mBuffer);
                mXmlString = mXmlString.Substring(mXmlString.IndexOf("<?xml"));
                mXmlString = mXmlString.Substring(0, mXmlString.LastIndexOf(@"</xisf>") + 7);

                try
                {
                    mXDoc = XDocument.Parse(mXmlString);
                }
                catch
                {
                    return false;
                }

                XElement root = mXDoc.Root;
                XNamespace ns = root.GetDefaultNamespace();

                IEnumerable<XElement> image = from c in mXDoc.Descendants(ns + "Image") select c;
                foreach (XElement element in image)
                {
                    xFile.ImageAttachment(element);
                }


                IEnumerable<XElement> thumbnail = from c in mXDoc.Descendants(ns + "Thumbnail") select c;
                foreach (XElement element in thumbnail)
                {
                    xFile.ThumbnailAttachment(element);
                }

                IEnumerable<XElement> elements = from c in mXDoc.Descendants(ns + "FITSKeyword") select c;

                // Find each relevent keyword and add it to mFile
                foreach (XElement element in elements)
                {
                    xFile.KeywordData.AddKeyword(element);
                }

                xFile.KeywordData.RepairSiteLatitude();
                xFile.KeywordData.RepairSiteLongitude();

                return true;
            }
        }
    

        
    }
}
