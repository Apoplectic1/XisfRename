﻿namespace XisfFileManager.Keywords
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
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "Approved");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.BOOL;

            SubFrameList.Approved.Add(node);
            keywords.KeywordList.Remove(node);
        }

        public void AddKeywordEccentricity(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "Eccentricity");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.Eccentricity.Add(node);
            keywords.KeywordList.Remove(node);
        }

        public void AddKeywordEccentricityMeanDeviation(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "EccentricityMeanDeviation");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.EccentricityMeanDeviation.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordFwhm(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "Fwhm");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.Fwhm.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordFwhmMeanDeviation(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "FwhmMeanDeviation");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.FwhmMeanDeviation.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordMedian(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "Median");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.Median.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordMedianMeanDeviation(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "MedianMeanDeviation");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.MedianMeanDeviation.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordNoise(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "Noise");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.Noise.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordNoiseRatio(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "NoiseRatio");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.NoiseRatio.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordSnrWeight(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "SNRWeight");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.SnrWeight.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordStarResidual(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "StarResidual");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.StarResidual.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordStarResidualMeanDeviation(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "StarResidualMeanDeviation");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.StarResidualMeanDeviation.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordStars(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "Stars");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.INTEGER;

            SubFrameList.Stars.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordWeight(KeywordLists keywords)
        {
            Keyword node = new Keyword();
            node = keywords.KeywordList.Find(i => i.Name == "WEIGHT");
            if (node == null) return;

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            SubFrameList.Weight.Add(node);
            keywords.KeywordList.Remove(node);
        }
        public void AddKeywordFileName(string fileName)
        {
            Keyword node = new Keyword();
            node.Name = "FileName";
            node.Value = fileName;
            node.Comment = "XISF File Manager";

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.STRING;

            SubFrameList.FileName.Add(node);
        }
    }
}