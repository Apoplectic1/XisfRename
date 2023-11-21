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

                    xFile.XmlVersionText = xmlVersionBlockMatch.ToString().Clone() as string;
                    xFile.XmlCommentText = xmlCommentBlockMatch.ToString().Clone() as string;

                    xmlString = xmlKeywordBlockMatch.ToString();

                    ValidateXisfXml(xmlString);

                    // Make an isolated copy
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

                    XElement root = xFile.mXDoc.Root;
                    XNamespace ns = root.GetDefaultNamespace();

                    // ***********************************************************************************

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

        public static void ValidateXisfXml(string xmlString)
        {
            // Remove anthing after </xisf > in xmlString
            xmlString = xmlString.Substring(0, xmlString.IndexOf("</xisf>") + "</xisf>".Length);

            // Remove all non-ASCII characters
            xmlString = Regex.Replace(xmlString, @"[^\x00-\x7F]", "");

            // Some XISF files have single quotes inside FITS Keywords - Remove them.
            xmlString = Regex.Replace(xmlString, "'", "");

            // Remove Processing History Property if it exists
            string pattern = Regex.Escape("<Property") + @"(.*?)" + Regex.Escape(";</Property>");
            xmlString = Regex.Replace(xmlString, pattern, "");
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


        public static bool ContainsNonAsciiOrInvalidChars(string input, out char firstInvalidChar)
        {
            // Check for non-ASCII characters
            bool containsNonAscii = Regex.IsMatch(input, @"[^\x00-\x7F]");

            // Check for characters other than the allowed set
            Match match = Regex.Match(input, @"[^A-Za-z0-9+\-./:()_ .<>="",*%]");
            if (match.Success)
            {
                firstInvalidChar = match.Value[0];
                return true;
            }

            firstInvalidChar = '\0';

            int quoteCount = 0;

            foreach (char c in input)
            {
                if (c == '"')
                {
                    quoteCount++;
                }
            }

            bool quotesMatch = quoteCount % 2 == 0;


            return containsNonAscii;
        }

        public static string RemoveNonEvenPairs(string input, string openString, string closeString)
        {
            string pattern = $@"{Regex.Escape(openString)}[^{Regex.Escape(openString + closeString)}]*{Regex.Escape(closeString)}";
            MatchCollection matches = Regex.Matches(input, pattern);

            string result = "";

            foreach (Match match in matches)
            {
                if (HasEvenPairs(match.Value, openString, closeString))
                {
                    result += match.Value;
                }
            }

            return result;
        }

        public static bool HasEvenPairs(string input, string openString, string closeString)
        {
            int openCount = Regex.Matches(input, Regex.Escape(openString)).Count;
            int closeCount = Regex.Matches(input, Regex.Escape(closeString)).Count;

            return openCount == closeCount;
        }
    }
}
