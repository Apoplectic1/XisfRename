using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public class XisfFileReader
    {
        private char[] mBuffer;
        private string mXmlString;
        private XDocument mXDoc;
        private StreamReader streamReader;
        private int mBufferSize = 10000;

        public XisfFileReader()
        {
            mBuffer = new char[mBufferSize];
        }

        public bool ReadXisfFile(XisfFile xFile)
        {
            using (StreamReader streamReader = new StreamReader(xFile.SourceFileName))
            {
                string mXmlString;
                int totalBytesRead = 0;

                // Skip first sixteen bytes that contain XISF0100xxxxxxxx
                streamReader.BaseStream.Seek(16, SeekOrigin.Begin); // Seek to the 17th character
                int bytesRead = streamReader.Read(mBuffer, 0, mBufferSize);
                int stringLength = FindStringLength(mBuffer, bytesRead);
                totalBytesRead += bytesRead;

                bool bExpandedBufferSize = false;

                // If the xml section if larger than mBufferSize, add 10000 charaters and reread
                while (stringLength == bytesRead)
                {
                    mBufferSize += 10000;
                    Array.Resize(ref mBuffer, mBufferSize); // Resize the buffer
                    streamReader.BaseStream.Seek(16 + totalBytesRead, SeekOrigin.Begin); // Seek to the appropriate offset
                    bytesRead = streamReader.Read(mBuffer, totalBytesRead, mBufferSize - totalBytesRead);
                    stringLength = FindStringLength(mBuffer, bytesRead);
                    totalBytesRead += bytesRead;
                    bExpandedBufferSize = true;
                }

                // Let us know the buffer got expanded so we can potentailly change the default mBufferSize to match
                if (bExpandedBufferSize)
                {
                    MessageBox.Show("Expanded mBufferSize to :\n\n" + mBufferSize.ToString() +
                        " Bytes. Needed: " + stringLength.ToString() +
                        " Bytes.\n\nXisfRead.cs ReadXisfFile() Line 34",
                        "Parse XISF File",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }

                Console.WriteLine("Size: " + mBufferSize.ToString());
           
                mXmlString = new string(mBuffer, 0, stringLength);

                try
                {
                    mXDoc = XDocument.Parse(mXmlString);
                }
                catch ( Exception ex )
                {
                    MessageBox.Show("Could not parse xml in file:\n\n" + xFile.SourceFileName +
                        "\n\nXisfRead.cs ReadXisfFile() Line 34\n" + ex.Message,
                        "Parse XISF File",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Error);

                    return false;
                }

                XElement root = mXDoc.Root;
                XNamespace ns = root.GetDefaultNamespace();

                IEnumerable<XElement> image = mXDoc.Descendants(ns + "Image");
                foreach (XElement element in image)
                {
                    xFile.ImageAttachment(element);
                }


                IEnumerable<XElement> thumbnail = mXDoc.Descendants(ns + "Thumbnail");
                foreach (XElement element in thumbnail)
                {
                    xFile.ThumbnailAttachment(element);
                }

                IEnumerable<XElement> elements = mXDoc.Descendants(ns + "FITSKeyword");

                // Find each keyword and add it to xFile
                foreach (XElement element in elements)
                {
                    xFile.KeywordData.AddXMLKeyword(element);
                }

                xFile.SetRequiredKeywords();

                return true;
            }
        }

        private int FindStringLength(char[] buffer, int bytesRead)
        {
            for (int i = 0; i < bytesRead; i++)
            {
                if (buffer[i] == '\0')
                {
                    return i;
                }
            }

            return bytesRead;
        }
    }
}
