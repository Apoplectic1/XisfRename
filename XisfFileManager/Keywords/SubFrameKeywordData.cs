using System.Collections.Generic;

namespace XisfFileManager.Keywords
{
    public class SubFrameKeywordData
    {
        public List<Keyword> Approved { get; set; }
        public List<Keyword> Eccentricity { get; set; }
        public List<Keyword> EccentricityMeanDeviation { get; set; }
        public List<Keyword> Fwhm { get; set; }
        public List<Keyword> FwhmMeanDeviation { get; set; }
        public List<Keyword> Median { get; set; }
        public List<Keyword> MedianMeanDeviation { get; set; }
        public List<Keyword> Noise { get; set; }
        public List<Keyword> NoiseRatio { get; set; }
        public List<Keyword> SnrWeight { get; set; }
        public List<Keyword> StarResidual { get; set; }
        public List<Keyword> StarResidualMeanDeviation { get; set; }
        public List<Keyword> Stars { get; set; }
        public List<Keyword> SSWeight { get; set; }
        public List<Keyword> FileName { get; set; }

        public SubFrameKeywordData()
        {
            Approved = new List<Keyword>();
            Eccentricity = new List<Keyword>();
            EccentricityMeanDeviation = new List<Keyword>();
            FileName = new List<Keyword>();
            Fwhm = new List<Keyword>();
            FwhmMeanDeviation = new List<Keyword>();
            Median = new List<Keyword>();
            MedianMeanDeviation = new List<Keyword>();
            Noise = new List<Keyword>();
            NoiseRatio = new List<Keyword>();
            SnrWeight = new List<Keyword>();
            Stars = new List<Keyword>();
            StarResidual = new List<Keyword>();
            StarResidualMeanDeviation = new List<Keyword>();
            SSWeight = new List<Keyword>();

        }

        public void ClearSubFrameLists()
        {
            Approved.Clear();
            Eccentricity.Clear();
            EccentricityMeanDeviation.Clear();
            FileName.Clear();
            Fwhm.Clear();
            FwhmMeanDeviation.Clear();
            Median.Clear();
            MedianMeanDeviation.Clear();
            Noise.Clear();
            NoiseRatio.Clear();
            SnrWeight.Clear();
            StarResidual.Clear();
            StarResidualMeanDeviation.Clear();
            Stars.Clear();
            SSWeight.Clear();
        }
    }
}
