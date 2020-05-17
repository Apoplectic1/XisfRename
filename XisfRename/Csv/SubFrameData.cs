using System.Collections.Generic;

namespace XisfRename.Csv
{
    class SubFrameData
    {
        public List<bool> Approved { get; set; }
        public List<double> Eccentricity { get; set; }
        public List<double> EccentricityMeanDeviation { get; set; }
        public List<double> Fwhm { get; set; }
        public List<double> FwhmMeanDeviation { get; set; }
        public List<double> Median { get; set; }
        public List<double> MedianMeanDeviation { get; set; }
        public List<double> Noise { get; set; }
        public List<double> NoiseRatio { get; set; }
        public List<double> SnrWeight { get; set; }
        public List<double> StarResidual { get; set; }
        public List<double> StarResidualMeanDeviation { get; set; }
        public List<double> Stars { get; set; }
        public List<double> Weight { get; set; }
        public List<string> File { get; set; }

        public int ApprovedIndex = -1;
        public int EccentricityIndex = -1;
        public int EccentricityMeanDeviationIndex = -1;
        public int FileIndex = -1;
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
        public int WeightIndex = -1;


        public SubFrameData()
        {
            Approved = new List<bool>();
            Eccentricity = new List<double>();
            EccentricityMeanDeviation = new List<double>();
            File = new List<string>();
            Fwhm = new List<double>();
            FwhmMeanDeviation = new List<double>();
            Median = new List<double>();
            MedianMeanDeviation = new List<double>();
            Noise = new List<double>();
            NoiseRatio = new List<double>();
            SnrWeight = new List<double>();
            Stars = new List<double>();
            StarResidual = new List<double>();
            StarResidualMeanDeviation = new List<double>();
            Weight = new List<double>();

        }
    }
}
