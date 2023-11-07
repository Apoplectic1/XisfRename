using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private Match xmlVersionMatch;
        private Match xmlCommentMatch;
        private Match keywordBlockMatch;

        public async Task ReadXisfFileHeaderKeywords(XisfFile xFile)
        {
            await Task.Run(async () =>
            {
                using (FileStream xFileStream = new FileStream(xFile.FilePath, FileMode.Open, FileAccess.Read))
                {
                    // Try to read the minium amount of data from each Xisf File.
                    // mBuffer size has been set read most Xisf files xml section in a single read pass. We MUST read enough to include the first "<xisf" delimiter (<xisf is after comment section)
                    // If we don't read the entire <xisf to xisf> section in one pass, double mBuffer size and re-read from start (start: being lazy; should use stream position)
                    mBufferSize = 10000;
                    mBuffer = new byte[mBufferSize];

                    xmlVersionMatch = Match.Empty;
                    xmlCommentMatch = Match.Empty;
                    keywordBlockMatch = Match.Empty;

                    bytesRead = 0;
                    int nXisfSignatureBlockSize = 16;
                    string xmlString;

                    // If the xml section is larger than mBufferSize, repeatedly double the buffer size and read again
                    while (!keywordBlockMatch.Success)
                    {
                        bytesRead = xFileStream.Read(mBuffer, bytesRead, mBufferSize - bytesRead);

                        xmlString = Encoding.UTF8.GetString(mBuffer.Skip(nXisfSignatureBlockSize).ToArray());

                        xmlVersionMatch = Regex.Match(xmlString, @"<\?xml[\s\S]*?\?>");
                        xmlCommentMatch = Regex.Match(xmlString, @"<!--[\s\S]*?-->");
                        keywordBlockMatch = Regex.Match(xmlString, @"<xisf[\s\S]*?xisf>");
                        if (!keywordBlockMatch.Success)
                        {
                            // We did not find the closing "xisf>. Expand mBuffer and try again
                            mBufferSize += mBufferSize;
                            //Console.WriteLine("Final Buffersize: " + mBufferSize.ToString() + " " + Path.GetFileName(xFile.FilePath));
                            Array.Resize(ref mBuffer, mBufferSize);
                        }
                    }

                    xFileStream.Close();

                    xFile.XmlVersionText = xmlVersionMatch.ToString();
                    xFile.XmlCommentText = xmlCommentMatch.ToString();
                    xmlString = keywordBlockMatch.ToString().Replace("'", "");

                    // Make an isolated copy
                    xFile.XmlString = xmlString.Clone() as string;

                    xFile.mXDoc = new XDocument();

                    try
                    {
                        xFile.mXDoc = XDocument.Parse(xmlString);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not parse xml in file:\n\n" + xFile.FilePath +
                            "\n\nXisfRead.cs ReadXisfFile() ->\n\tmXDoc = XDocument.Parse(sXmlString)\n\n" + ex.Message,
                            "Parse XISF File",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Error);

                        return;
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
                    
                    // Place all XML formated FITS Keyword Name, Value, Comment triples into 'elements'
                    IEnumerable<XElement> elements = xFile.mXDoc.Descendants(ns + "FITSKeyword");

                    // Look at each XML FITS keyword triple and add it xFile.KeywordList 
                    foreach (XElement element in elements)
                    {
                        xFile.AddXMLKeyword(element);
                    }
                }
            });
        }
    }
}
