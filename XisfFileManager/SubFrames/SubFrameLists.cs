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

        public void AddKeywordApproved(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "Approved");
            if (node == null) return;

            SubFrameList.ApprovedList.Add(node);
            keywords.KeywordList.Remove(node);
        }

        public void AddKeywordAirMass(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "AIRMASS");
            if (node == null) return;

            SubFrameList.AirMassList.Add(node);
            keywords.KeywordList.Remove(node);
        }

        public void AddKeywordEccentricity(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "Eccentricity");
            if (node == null) return;

            SubFrameList.Eccentricity.Add(node);
            keywords.KeywordList.Remove(node);
        }

        public void AddKeywordEccentricityMeanDeviation(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "EccentricityMeanDeviation");
            if (node == null) return;

            SubFrameList.EccentricityMeanDeviation.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordFwhm(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "Fwhm");
            if (node == null) return;

            SubFrameList.FwhmList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordFwhmMeanDeviation(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "FwhmMeanDeviation");
            if (node == null) return;

            SubFrameList.FwhmMeanDeviationList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordMedian(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "Median");
            if (node == null) return;

            SubFrameList.MedianList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordMedianMeanDeviation(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "MedianMeanDeviation");
            if (node == null) return;

            SubFrameList.MedianMeanDeviationList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordNoise(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "Noise");
            if (node == null) return;

            SubFrameList.NoiseList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordNoiseRatio(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "NoiseRatio");
            if (node == null) return;

            SubFrameList.NoiseRatioList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordSnrWeight(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "SNRWeight");
            if (node == null) return;

            SubFrameList.SnrWeightList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordStarResidual(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "StarResidual");
            if (node == null) return;

            SubFrameList.StarResidualList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordStarResidualMeanDeviation(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "StarResidualMeanDeviation");
            if (node == null) return;

            SubFrameList.StarResidualMeanDeviationList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordStars(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "Stars");
            if (node == null) return;

            SubFrameList.StarsList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordWeight(KeywordLists keywords)
        {
            FitsKeyword node = new FitsKeyword();
            node = keywords.KeywordList.Find(i => i.Name == "WEIGHT");
            if (node == null) return;

            SubFrameList.WeightList.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordFileName(string fileName)
        {
            FitsKeyword node = new FitsKeyword
            {
                Name = "FileName",
                Value = fileName,
                Comment = "XISF File Manager"
            };

            SubFrameList.FileNameList.Add(node);
        }
    }
}
