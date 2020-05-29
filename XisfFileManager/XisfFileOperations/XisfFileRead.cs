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

        public static bool ParseXisfFile(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
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

                FindKeywordApproved(xFile, FileSubFrameKeywordLists);
                FindKeywordEccentricity(xFile, FileSubFrameKeywordLists);
                FindKeywordEccentricityMeanDeviation(xFile, FileSubFrameKeywordLists);
                FindKeywordFwhm(xFile, FileSubFrameKeywordLists);
                FindKeywordFwhmMeanDeviation(xFile, FileSubFrameKeywordLists);
                FindKeywordMedian(xFile, FileSubFrameKeywordLists);
                FindKeywordMedianMeanDeviation(xFile, FileSubFrameKeywordLists);
                FindKeywordNoise(xFile, FileSubFrameKeywordLists);
                FindKeywordNoiseRatio(xFile, FileSubFrameKeywordLists);
                FindKeywordSnrWeight(xFile, FileSubFrameKeywordLists);
                FindKeywordStarResidual(xFile, FileSubFrameKeywordLists);
                FindKeywordStarResidualMeanDeviation(xFile, FileSubFrameKeywordLists);
                FindKeywordStars(xFile, FileSubFrameKeywordLists);
                FindKeywordSSWeight(xFile, FileSubFrameKeywordLists);
                FindKeywordFileName(xFile, FileSubFrameKeywordLists);

                xFile.KeywordData.RepairSiteLatitude();
                xFile.KeywordData.RepairSiteLongitude();

                return true;
            }
        }
    

        // #########################################################################################################
        // #########################################################################################################

        // Find SubFrame keywords Added by this program

        // #########################################################################################################
        // #########################################################################################################

        private static void FindKeywordApproved(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "Approved");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            FileSubFrameKeywordLists.Approved.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }

        private static void FindKeywordEccentricity(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "Eccentricity");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.Eccentricity.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordEccentricityMeanDeviation(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "EccentricityMeanDeviation");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.EccentricityMeanDeviation.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordFwhm(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "Fwhm");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.Fwhm.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordFwhmMeanDeviation(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "FwhmMeanDeviation");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.FwhmMeanDeviation.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordMedian(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "Median");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.Median.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordMedianMeanDeviation(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "MedianMeanDeviation");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.MedianMeanDeviation.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordNoise(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "Noise");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.Noise.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordNoiseRatio(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "NoiseRatio");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            FileSubFrameKeywordLists.NoiseRatio.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordSnrWeight(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "SnrWeight");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            FileSubFrameKeywordLists.SnrWeight.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordStarResidual(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "StarResidual");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            FileSubFrameKeywordLists.StarResidual.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordStarResidualMeanDeviation(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "StarResidualMeanDeviation");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            FileSubFrameKeywordLists.StarResidualMeanDeviation.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordStars(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "Stars");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            FileSubFrameKeywordLists.Stars.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordSSWeight(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "SSWeight");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            FileSubFrameKeywordLists.SSWeight.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
        private static void FindKeywordFileName(XisfFile.XisfFile xFile, SubFrameKeywordLists FileSubFrameKeywordLists)
        {
            string value = string.Empty;

            Keyword node = xFile.KeywordData.KeywordList.Find(i => i.Name == "FileName");
            if (node == null) return;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            FileSubFrameKeywordLists.FileName.Add(node);
            xFile.KeywordData.KeywordList.Remove(node);
        }
    }
}
