using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public static class XisfFileRead
    {
        private static char[] mBuffer;
        private static string mXmlString;
        private static XDocument mXDoc;

        public static bool ReadXisfFile(XisfFile xFile)
        {
            using (StreamReader reader = new StreamReader(xFile.SourceFileName))
            {
                mBuffer = new char[0x300000];
                reader.Read(mBuffer, 0, mBuffer.Length);

                mXmlString = new string(mBuffer);
                // Skip first sixteen bytes that contain XISF0100xxxxxxxx 
                mXmlString = mXmlString.Substring(mXmlString.IndexOf("<?xml"));
                // find closing </xisf>. set mXmlString to the entire xml text. Note that the size of mBuffer can be too small and cause this to fail
                mXmlString = mXmlString.Substring(0, mXmlString.LastIndexOf("</xisf>") + "</xisf>".Length);

                try
                {
                    mXDoc = XDocument.Parse(mXmlString);
                }
                catch
                {
                    var selectedOption = MessageBox.Show("Could not parse xml in file:\n\n" + xFile.SourceFileName + "\n\nXisfRead.cs ReadXisfFile()", "Parse XISF File Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (selectedOption == DialogResult.Cancel)
                    {
                        Application.Exit();
                    }

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

                xFile.KeywordData.SetEGain();
                xFile.KeywordData.CaptureSoftware();
                xFile.KeywordData.RepairSiteLatitude();
                xFile.KeywordData.RepairSiteLongitude();
                xFile.KeywordData.RepairTelescope();

                return true;
            }
        }        
    }
}
