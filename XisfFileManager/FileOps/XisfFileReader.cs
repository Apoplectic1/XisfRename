using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public class XisfFileReader
    {
        private byte[] mBuffer;
        private int mBufferSize;
        private int bytesRead;
        private Match keywordBlock;
        private string keywordMatch = @"<xisf.*?</xisf>";
        private string modifiedString;

        public async Task ReadXisfFile(XisfFile xFile)
        {
            using (FileStream fileStream = new FileStream(xFile.SourceFileName, FileMode.Open, FileAccess.Read))
            {
                mBufferSize = 10000;
                mBuffer = new byte[mBufferSize];
                keywordBlock = Match.Empty;
                bytesRead = 0;


                // Skip first sixteen bytes that contain XISF0100xxxxxxxx

                // If the xml section is larger than mBufferSize, repeatedly double the buffer size and reread
                while (!keywordBlock.Success)
                {
                    bytesRead = fileStream.Read(mBuffer, bytesRead, mBufferSize - bytesRead);
                    keywordBlock = Regex.Match(Encoding.UTF8.GetString(mBuffer), keywordMatch, RegexOptions.Singleline);

                    if (!keywordBlock.Success)
                    {
                        if (bytesRead == mBufferSize)
                        {
                            // Expand the buffer size if it has been filled
                            mBufferSize += mBufferSize;
                            Array.Resize(ref mBuffer, mBufferSize);
                        }
                        else
                        {
                            // Process the bytes read and exit the loop
                            modifiedString = Encoding.UTF8.GetString(mBuffer, 0, bytesRead);
                        }
                    }
                }

                modifiedString = keywordBlock.ToString();

                /*
                string startTag = "<Property";
                string stopTag = "/>";
                string pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
                modifiedString = Regex.Replace(modifiedString, pattern, "");

                startTag = "<Property";
                stopTag = "/Property>";
                pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
                modifiedString = Regex.Replace(modifiedString.ToString(), pattern, "");

                startTag = "<Image";
                stopTag = "</Image>";
                int first = modifiedString.IndexOf(startTag);
                int last = modifiedString.IndexOf(stopTag);
                if (last <= first)
                {
                    startTag = "/><Display";
                    stopTag = "/xisf>";
                    pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
                    modifiedString = Regex.Replace(modifiedString.ToString(), pattern, "/></Image></xisf>");
                }
                */
                //PruneXisfFile();


                xFile.mXDoc = new XDocument();

                try
                {
                    xFile.mXDoc = XDocument.Parse(modifiedString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not parse xml in file:\n\n" + xFile.SourceFileName +
                        "\n\nXisfRead.cs ReadXisfFile() ->\n\tmXDoc = XDocument.Parse(sXmlString)\n\n" + ex.Message,
                        "Parse XISF File",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Error);

                    Application.Exit();
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

                IEnumerable<XElement> elements = xFile.mXDoc.Descendants(ns + "FITSKeyword");

                // Find each keyword and add it to xFile
                foreach (XElement element in elements)
                {
                    xFile.KeywordData.AddXMLKeyword(element);
                }

                xFile.SetRequiredKeywords();
            }
        }

        private void PruneXisfFile()
        {
            string startTag;
            string stopTag;
            string pattern;
            string mSearch;


            mSearch = "BlockAlignmentSize";
            pattern = $"<([^<>]*?{mSearch}[^<>]*?)>";
            MatchCollection BlockAlignmentSize = Regex.Matches(modifiedString, pattern);

            mSearch = "MaxInlineBlockSize";
            pattern = $"<([^<>]*?{mSearch}[^<>]*?)>";
            MatchCollection MaxInlineBlockSize = Regex.Matches(modifiedString, pattern);

            startTag = "<Property";
            stopTag = "/>";
            pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
            modifiedString = Regex.Replace(modifiedString, pattern, "");

            startTag = "<Property";
            stopTag = "/Property>";
            pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
            modifiedString = Regex.Replace(modifiedString.ToString(), pattern, "");

            startTag = "<ICCProfile";
            stopTag = "</ICCProfile>";
            pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
            modifiedString = Regex.Replace(modifiedString, pattern, "");

            startTag = "<FITSKeyword name=\"HISTORY\" val";
            stopTag = "/>";
            pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
            modifiedString = Regex.Replace(modifiedString, pattern, "");

            startTag = "<FITSKeyword name=\"COMMENT\" val";
            stopTag = "/>";
            pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
            modifiedString = Regex.Replace(modifiedString, pattern, "");

            startTag = "<Image";
            stopTag = "</Image>";
            int first = modifiedString.IndexOf(startTag);
            int last  = modifiedString.IndexOf(stopTag);
            if (last < first)
            {
                ;
            }

            pattern = $"{Regex.Escape(startTag)}.*?{Regex.Escape(stopTag)}";
            modifiedString = Regex.Replace(modifiedString, pattern, "");

            //Add <Property id="XISF:BlockAlignmentSize" type="UInt16" value="4096"/>
            startTag = "</Metadata>";
            int startIndex = modifiedString.IndexOf(startTag);
            int stopIndex = startIndex + startTag.Length;

            if (startIndex != -1 && stopIndex != -1)
            {
                modifiedString = modifiedString.Remove(startIndex, startTag.Length);
                modifiedString = modifiedString.Insert(startIndex, BlockAlignmentSize[0].ToString() + "</Metadata>");
            }
            else
                modifiedString += $"<Property id=\"XISF:BlockAlignmentSize\" type=\"UInt16\" value=\"4096\"/></Metadata>";


            //Add <Property id="XISF:MaxInlineBlockSize" type="UInt16" value="3072"/>
            startTag = "</Metadata>";
            startIndex = modifiedString.IndexOf(startTag);
            stopIndex = startIndex + startTag.Length;

            if (startIndex != -1 && stopIndex != -1)
            {
                modifiedString = modifiedString.Remove(startIndex, startTag.Length);
                modifiedString = modifiedString.Insert(startIndex, MaxInlineBlockSize[0].ToString() + "</Metadata>");
            }
            else
                modifiedString = Regex.Replace(modifiedString, pattern, "<Property id=\"XISF:MaxInlineBlockSize\" type=\"UInt16\" value=\"3072\"/></Metadata>");
        }
    }
}
