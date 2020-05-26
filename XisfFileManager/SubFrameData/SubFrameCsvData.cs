using System.Collections.Generic;

namespace XisfFileManager.SubFrameData
{
    public class SubFrameData
    {
        public List<XisfKeywords.Keyword> Approved { get; set; }
        public List<XisfKeywords.Keyword> Eccentricity { get; set; }
        public List<XisfKeywords.Keyword> EccentricityMeanDeviation { get; set; }
        public List<XisfKeywords.Keyword> Fwhm { get; set; }
        public List<XisfKeywords.Keyword> FwhmMeanDeviation { get; set; }
        public List<XisfKeywords.Keyword> Median { get; set; }
        public List<XisfKeywords.Keyword> MedianMeanDeviation { get; set; }
        public List<XisfKeywords.Keyword> Noise { get; set; }
        public List<XisfKeywords.Keyword> NoiseRatio { get; set; }
        public List<XisfKeywords.Keyword> SnrWeight { get; set; }
        public List<XisfKeywords.Keyword> StarResidual { get; set; }
        public List<XisfKeywords.Keyword> StarResidualMeanDeviation { get; set; }
        public List<XisfKeywords.Keyword> Stars { get; set; }
        public List<XisfKeywords.Keyword> SSWeight { get; set; }
        public List<XisfKeywords.Keyword> FileName { get; set; }

        public int ApprovedIndex = -1;
        public int EccentricityIndex = -1;
        public int EccentricityMeanDeviationIndex = -1;
        public int FileNameIndex = -1;
        public int FwhmIndex = -1;
        public int FwhmMeanDeviationIndex = -1;
        public int MedianIndex = -1;
        public int MedianMeanDeviationIndex = -1;
        public int NoiseIndex = -1;
        public int NoiseRatioIndex = -1;
        public int SnrWeightIndex = -1;
        public int StarResidualIndex = -1;
        public int StarResidualMeanDeviationIndex = -1;
        public int StarsIndex = -1;
        public int SSWeightIndex = -1;


        public SubFrameData()
        {
            Approved = new List<XisfKeywords.Keyword>();
            Eccentricity = new List<XisfKeywords.Keyword>();
            EccentricityMeanDeviation = new List<XisfKeywords.Keyword>();
            FileName = new List<XisfKeywords.Keyword>();
            Fwhm = new List<XisfKeywords.Keyword>();
            FwhmMeanDeviation = new List<XisfKeywords.Keyword>();
            Median = new List<XisfKeywords.Keyword>();
            MedianMeanDeviation = new List<XisfKeywords.Keyword>();
            Noise = new List<XisfKeywords.Keyword>();
            NoiseRatio = new List<XisfKeywords.Keyword>();
            SnrWeight = new List<XisfKeywords.Keyword>();
            Stars = new List<XisfKeywords.Keyword>();
            StarResidual = new List<XisfKeywords.Keyword>();
            StarResidualMeanDeviation = new List<XisfKeywords.Keyword>();
            SSWeight = new List<XisfKeywords.Keyword>();

        }
    }
}
