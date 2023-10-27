using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using XisfFileManager.TargetScheduler.Tables;

namespace XisfFileManager.FileOperations
{
    public class XisfFileReader
    {
        private byte[] mBuffer;
        private int mBufferSize;
        private int bytesRead;
        private Match keywordBlock;
        private string modifiedString;
        private bool bIsPreamble;

        public async Task ReadXisfFile(XisfFile xFile)
        {
            await Task.Run(async () =>
            {
                using (FileStream xFileStream = new FileStream(xFile.FilePath, FileMode.Open, FileAccess.Read))
                {
                    
                    string xmlString = string.Empty;
                    mBufferSize = 25000;
                    mBuffer = new byte[mBufferSize];
                    keywordBlock = Match.Empty;
                    bytesRead = 0;
                    int nPreambleIndex = 0;

                    // If the xml section is larger than mBufferSize, repeatedly double the buffer size and read again
                    while (!keywordBlock.Success)
                    {
                        bytesRead = xFileStream.Read(mBuffer, bytesRead, mBufferSize - bytesRead);
                        
                        xmlString = Encoding.UTF8.GetString(new ArraySegment<byte>(mBuffer, nPreambleIndex, mBuffer.Length - nPreambleIndex));
                        nPreambleIndex = 0;

                        keywordBlock = Regex.Match(xmlString, @"<xisf.*?xisf>", RegexOptions.Singleline);

                        if (!keywordBlock.Success)
                        {
                            // We did not find the closing "xisf> in xmlString

                            // Expand the buffer size if it has been filled
                            mBufferSize += mBufferSize;
                            Array.Resize(ref mBuffer, mBufferSize);
                            bIsPreamble = true;
                        }
                    }
             
                    modifiedString = keywordBlock.ToString().Replace("'", "");

                    xFile.mXDoc = new XDocument();

                    try
                    {
                        xFile.mXDoc = XDocument.Parse(modifiedString);
                    }
                    catch (Exception ex)
                    {
                        _ = MessageBox.Show("Could not parse xml in file:\n\n" + xFile.FilePath +
                            "\n\nXisfRead.cs ReadXisfFile() ->\n\tmXDoc = XDocument.Parse(sXmlString)\n\n" + ex.Message,
                            "Parse XISF File",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Error);

                        Application.Exit();
                    }

                    XElement root = xFile.mXDoc.Root;
                    XNamespace ns = root.GetDefaultNamespace();

                    IEnumerable<XElement> image = xFile.mXDoc.Descendants(ns + "Image");
                    foreach (XElement element in image)
                    {
                        xFile.ImageAttachment(element);
                    }

                    IEnumerable<XElement> thumbnail = xFile.mXDoc.Descendants(ns + "Thumbnail");
                    foreach (XElement element in thumbnail)
                    {
                        xFile.ThumbnailAttachment(element);
                    }

                    // This will place all XML formated FITS Keyword Name, Value, Comment triples into 'elements' so that 'elements' has an IEnumerable list of each set of keyword triples
                    IEnumerable<XElement> elements = xFile.mXDoc.Descendants(ns + "FITSKeyword");

                    // Look at each XML FITS keyword triple and add it to this file's Keyword set 
                    foreach (XElement element in elements)
                    {
                        xFile.AddXMLKeyword(element);
                    }
                }
            });
        }
    }
}
