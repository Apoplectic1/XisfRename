using XisfFileManager.FileOperations;

namespace XisfFileManager.Keywords
{
    public class SubFrameLists
    {
        public SubFrame SubFrameList;


        public SubFrameLists()
        {
            SubFrameList = new SubFrame();
        }

        public void Clear()
        {
            SubFrameList.Clear();
        }

        // #########################################################################################################
        // #########################################################################################################

        // Find SubFrame keywords Added by this program

        // #########################################################################################################
        // #########################################################################################################

        public void AddApproved(XisfFile xfile)
        {
            SubFrameList.ApprovedList.Add(xfile.Approved);
        }

        public void AddEccentricity(XisfFile xfile)
        {
            
        }

        public void AddKeywordEccentricityMeanDeviation(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "EccentricityMeanDeviation");
            if (node == null) return;

            SubFrameList.EccentricityMeanDeviation.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordFwhm(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "Fwhm");
            if (node == null) return;

            SubFrameList.FwhmList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordFwhmMeanDeviation(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "FwhmMeanDeviation");
            if (node == null) return;

            SubFrameList.FwhmMeanDeviationList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordMedian(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "Median");
            if (node == null) return;

            SubFrameList.MedianList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordMedianMeanDeviation(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "MedianMeanDeviation");
            if (node == null) return;

            SubFrameList.MedianMeanDeviationList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordNoise(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "Noise");
            if (node == null) return;

            SubFrameList.NoiseList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordNoiseRatio(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "NoiseRatio");
            if (node == null) return;

            SubFrameList.NoiseRatioList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordSnrWeight(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "SNRWeight");
            if (node == null) return;

            SubFrameList.SnrWeightList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordStarResidual(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "StarResidual");
            if (node == null) return;

            SubFrameList.StarResidualList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordStarResidualMeanDeviation(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "StarResidualMeanDeviation");
            if (node == null) return;

            SubFrameList.StarResidualMeanDeviationList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordStars(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "Stars");
            if (node == null) return;

            SubFrameList.StarsList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordWeight(KeywordList keywords)
        {
            Keyword node = new Keyword();
            node = keywords.mKeywordList.Find(i => i.Name == "WEIGHT");
            if (node == null) return;

            SubFrameList.WeightList.Add(node);
            keywords.mKeywordList.Remove(node);
        }
        public void AddKeywordFileName(string fileName)
        {
            Keyword node = new Keyword
            {
                Name = "FileName",
                Value = fileName,
                Comment = "XISF File Manager"
            };

            SubFrameList.FileNameList.Add(node);
        }
    }
}
