using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XisfFileManager.TargetScheduler.Tables;

namespace XisfFileManager.FileOperations
{
    public class XisfFileReader
    {
        private byte[] mBuffer;
        private int mBufferSize;
        private int bytesRead;

        private Match xmlVersionBlockMatch;
        private Match xmlCommentBlockMatch;
        private Match xmlKeywordBlockMatch;

        static bool IsWellFormedXml(string xml)
        {
            // Implement your custom XML validation logic here
            // You can use libraries like HtmlAgilityPack for more robust validation

            // For simplicity, this example checks if the XML starts and ends with proper tags
            return xml.StartsWith('<') && xml.EndsWith('>');
        }

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

                    xmlVersionBlockMatch = Match.Empty;
                    xmlCommentBlockMatch = Match.Empty;
                    xmlKeywordBlockMatch = Match.Empty;

                    bytesRead = 0;
                    int nXisfSignatureBlockSize = 16;
                    string xmlString;

                    // If the xml section is larger than mBufferSize, repeatedly double the buffer size and read again
                    while (!xmlKeywordBlockMatch.Success)
                    {
                        bytesRead = xFileStream.Read(mBuffer, bytesRead, mBufferSize - bytesRead);

                        xmlString = Encoding.UTF8.GetString(mBuffer.Skip(nXisfSignatureBlockSize).ToArray());

                        xmlVersionBlockMatch = Regex.Match(xmlString, @"<\?xml[\s\S]*?\?>");
                        xmlCommentBlockMatch = Regex.Match(xmlString, @"<!--[\s\S]*?-->");
                        xmlKeywordBlockMatch = Regex.Match(xmlString, @"<xisf[\s\S]*?xisf>");
                        if (!xmlKeywordBlockMatch.Success)
                        {
                            // We did not find the closing "xisf>. Expand mBuffer and try again
                            mBufferSize += mBufferSize;
                            //Console.WriteLine("Final Buffersize: " + mBufferSize.ToString() + " " + Path.GetFileName(xFile.FilePath));
                            Array.Resize(ref mBuffer, mBufferSize);
                        }
                    }

                    xFileStream.Close();

                    xFile.XmlVersionText = xmlVersionBlockMatch.ToString().Clone() as string;
                    xFile.XmlCommentText = xmlCommentBlockMatch.ToString().Clone() as string;
                    xmlString = xmlKeywordBlockMatch.ToString().Replace("'", "");


                    //xmlString = xmlString.Replace("com\" ", "");


                    // Make an isolated copy
                    xFile.XmlString = xmlString.Clone() as string;

                    xFile.mXDoc = new XDocument();

                    try
                    {
                        xFile.mXDoc = XDocument.Parse(xmlString);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not Read xml in file:\n\n" + xFile.FilePath + "\n\nin:\n\nReadXisfFileHeaderKeywords(XisfFile xFile)",
                            "Parse XISF File Error",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Error);

                        return;
                    }

                    XElement root = xFile.mXDoc.Root;
                    XNamespace ns = root.GetDefaultNamespace();

                    // ***********************************************************************************

                    IEnumerable<XElement> image = xFile.mXDoc.Descendants(ns + "Image");
                    foreach (XElement element in image)
                    {
                        xFile.ImageAttachment(element);
                    }

                    IEnumerable<XElement> iccprofile = xFile.mXDoc.Descendants(ns + "ICCProfile");
                    foreach (XElement element in iccprofile)
                    {
                        xFile.IccAttachment(element);
                    }

                    IEnumerable<XElement> thumbnail = xFile.mXDoc.Descendants(ns + "Thumbnail");
                    foreach (XElement element in thumbnail)
                    {
                        xFile.ThumbnailAttachment(element);
                    }

                    // Place all XML formated FITS Keyword Name, Value, Comment triples into 'elements'
                    IEnumerable<XElement> elements = xFile.mXDoc.Descendants(ns + "FITSKeyword");
                    foreach (XElement element in elements)
                    {
                        xFile.AddXMLKeyword(element);
                    }

                    // Place Property triples into 'properties'
                    IEnumerable<XElement> properties = xFile.mXDoc.Descendants(ns + "Property");
                    foreach (XElement property in properties)
                    {
                        xFile.ParseProperties(property);
                    }

                    // ***********************************************************************************
                }
            });
        }
    }
}
