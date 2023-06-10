using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public class XisfFileReader
    {
        private char[] mBuffer;
        private string mXmlString;
        private XDocument mXDoc;

        public bool ReadXisfFile(XisfFile xFile)
        {
            using (StreamReader reader = new StreamReader(xFile.SourceFileName))
            {
                mBuffer = new char[0x300000];
                reader.Read(mBuffer, 0, mBuffer.Length);

                mXmlString = new string(mBuffer);
                // Skip first sixteen bytes that contain XISF0100xxxxxxxx 
                // I've found at least two standards for the xml block surround: one is <?xml and the other is <xisf from N.I.N.A
                // N.I.N.A also adds a ^M as xml line endings (nice in emacs but different than PixInsight
                mXmlString = mXmlString.Substring(mXmlString.IndexOf("<?xml"));


                // find closing </xisf>. set mXmlString to the entire xml text. Note that the size of mBuffer can be too small and cause this to fail
                mXmlString = mXmlString.Substring(0, mXmlString.LastIndexOf("</xisf>") + "</xisf>".Length);

                try
                {
                    mXDoc = XDocument.Parse(mXmlString);
                }
                catch
                {
                    var selectedOption = MessageBox.Show("Could not parse xml in file:\n\n" + xFile.SourceFileName + "\n\nXisfRead.cs ReadXisfFile() Line 34", "Parse XISF File Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
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

                // Find each keyword and add it to xFile
                foreach (XElement element in elements)
                {
                    xFile.KeywordData.AddXMLKeyword(element);
                }

                xFile.SetRequiredKeywords();

                return true;
            }
        }        
    }
}
