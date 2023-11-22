using MathNet.Numerics;
using Microsoft.AspNetCore.Http;
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
using System.Xml.Schema;
using XisfFileManager.TargetScheduler.Tables;
using XisfFileManager.XML;

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

        //private Xml mXml = new XisfFileManager.XML.Xml();


        public async Task ReadXisfFileHeaderKeywords(XisfFile xFile)
        {
            await Task.Run(async () =>
            {
                using (FileStream xFileStream = new FileStream(xFile.FilePath, FileMode.Open, FileAccess.Read))
                {
                    // Try to read the minium amount of data from each Xisf File.
                    // mBuffer size has been set read most Xisf files xml section in a single read pass.
                    // We MUST read enough to include the first "<xisf" delimiter (<xisf is after comment section)
                    // If we don't read the entire <xisf to xisf> section in one pass, double mBuffer size and re-read from start (start: being lazy; should use stream position)

                    mBufferSize = 10000;
                    mBuffer = new byte[mBufferSize];

                    xmlVersionBlockMatch = Match.Empty;
                    xmlCommentBlockMatch = Match.Empty;
                    xmlKeywordBlockMatch = Match.Empty;

                    bytesRead = 0;
                    int nXisfSignatureBlockSize = 16;
                    string xmlString;

                    while (!xmlKeywordBlockMatch.Success)
                    {
                        bytesRead = xFileStream.Read(mBuffer, bytesRead, mBufferSize - bytesRead);

                        xmlString = Encoding.UTF8.GetString(mBuffer.Skip(nXisfSignatureBlockSize).ToArray());

                        xmlVersionBlockMatch = Regex.Match(xmlString, @"<\?xml[\s\S]*?\?>");
                        xmlCommentBlockMatch = Regex.Match(xmlString, @"<!--[\s\S]*?-->");
                        xmlKeywordBlockMatch = Regex.Match(xmlString, @"<xisf[\s\S]*?xisf>");

                        if (!xmlKeywordBlockMatch.Success)
                        {
                            // We did not find the closing "xisf> due to insuficient mBuffer size. Expand mBuffer and try again
                            mBufferSize += mBufferSize;
                            //Console.WriteLine("Final Buffersize: " + mBufferSize.ToString() + " " + Path.GetFileName(xFile.FilePath));
                            Array.Resize(ref mBuffer, mBufferSize);
                        }

                        if (mBufferSize > 2E17)
                        {
                            // Abort - Xml section is limited to 2E16 characters (because of my code that sets xml length in XmlUpdate)
                            MessageBox.Show("Xml section of file:\n\n" + xFile.FilePath + "\n\nis larger than 2E16 characters. Aborted",
                                             "XISF File Read Size Error",
                                              MessageBoxButtons.OKCancel,
                                              MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // return <xisf>...</xisf> section
                    xmlString = xmlKeywordBlockMatch.ToString();

                    // Remove any blatent garbage from xmlString
                    xmlString = Xml.FixXisfXml(xmlString);

                    // Remove any malformed xml from xmlString
                    xmlString = Xml.ValidateXisfXml(xmlString);

                    // Make an isolated copies
                    xFile.XmlVersionText = xmlVersionBlockMatch.ToString().Clone() as string;
                    xFile.XmlCommentText = xmlCommentBlockMatch.ToString().Clone() as string;
                    xFile.XmlString = xmlString.Clone() as string;

                    xFile.mXDoc = new XDocument();

                    try
                    {
                        xFile.mXDoc = XDocument.Parse(xmlString);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not Parse xml in file:\n\n" + xFile.FilePath + "\n\nin:\n\nReadXisfFileHeaderKeywords(XisfFile xFile)",
                            "Parse XISF File Error",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Error);

                        return;
                    }

                    // ***********************************************************************************

                    XElement root = xFile.mXDoc.Root;
                    XNamespace ns = root.GetDefaultNamespace();

                    FindXisfAttachments(xFile, ns);

                    FindXisIccProfiles(xFile, ns);

                    FindXisfThumbnails(xFile, ns);

                    FindXisfFitsKeywords(xFile, ns);

                    FindXisfProperties(xFile, ns);

                    // ***********************************************************************************
                }
            });
        }

        // ***********************************************************************************

        public static void FindXisfAttachments(XisfFile xFile, XNamespace ns)
        {
            IEnumerable<XElement> imageElements = xFile.mXDoc.Descendants(ns + "Image")
                               .Where(element =>
                                   (element.Attribute("id") == null) ||
                                   (element.Attribute("id") != null && (string)element.Attribute("id") == "integration") ||
                                   (element.Attribute("id") != null && (string)element.Attribute("id") == "rejection_high") ||
                                   (element.Attribute("id") != null && (string)element.Attribute("id") == "rejection_low"));

            foreach (XElement element in imageElements)
            {
                XAttribute idAttribute = element.Attribute("id");
                if (idAttribute == null || idAttribute.Value == "integration")
                {
                    // Both are main and should be first image
                    xFile.ImageAttachment(element);
                    continue;
                }
                else if (idAttribute != null && idAttribute.Value == "rejection_high")
                {
                    xFile.ImageRejectionHighAttachment(element);
                    continue;
                }
                else if (idAttribute != null && idAttribute.Value == "rejection_low")
                {
                    xFile.ImageRejectionLowAttachment(element);
                    continue;
                }
            }

        }

        // ***********************************************************************************

        public static void FindXisIccProfiles(XisfFile xFile, XNamespace ns)
        {
            IEnumerable<XElement> iccprofile = xFile.mXDoc.Descendants(ns + "ICCProfile");
            foreach (XElement element in iccprofile)
            {
                xFile.IccAttachment(element);
            }
        }

        // ***********************************************************************************

        public static void FindXisfThumbnails(XisfFile xFile, XNamespace ns)
        {
            IEnumerable<XElement> thumbnail = xFile.mXDoc.Descendants(ns + "Thumbnail");
            foreach (XElement element in thumbnail)
            {
                xFile.ThumbnailAttachment(element);
            }
        }

        // ***********************************************************************************

        public static void FindXisfFitsKeywords(XisfFile xFile, XNamespace ns)
        {
            // Place all XML formated FITS Keyword Name, Value, Comment triples into 'elements'
            IEnumerable<XElement> elements = xFile.mXDoc.Descendants(ns + "FITSKeyword");
            foreach (XElement element in elements)
            {
                xFile.AddXMLKeyword(element);
            }
        }

        // ***********************************************************************************

        public static void FindXisfProperties(XisfFile xFile, XNamespace ns)
        {
            // Extract various property values from the XISF file
            IEnumerable<XElement> properties = xFile.mXDoc.Descendants(ns + "Property");
            foreach (XElement property in properties)
            {
                xFile.ParseProperties(property);
            }
        }

        // ***********************************************************************************
        // ***********************************************************************************
    }
}
