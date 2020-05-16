using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfRename.Csv
{
    class SubFrameData
    {
        public List<bool> Approved { get; set; }
        public int ApprovedIndex = -1;
        public List<string> File { get; set; }
        public int FileIndex = -1;
        public List<double> Weight { get; set; }
        public int WeightIndex = -1;
        public List<double> Fwhm { get; set; }
        public int FwhmIndex = -1;
        public List<double> Eccentricity { get; set; }
        public int EccentricityIndex = -1;
        public List<double> SnrWeight { get; set; }
        public int SnrWeightIndex = -1;
        public List<double> Median { get; set; }
        public int MedianIndex = -1;
        public List<double> MedianMeanDeviation { get; set; }
        public int MedianMeanDeviationIndex = -1;
        public List<double> Noise { get; set; }
        public int NoiseIndex = -1;

        public SubFrameData()
        {
            Approved = new List<bool>();
            File = new List<string>();
            Weight = new List<double>();
            Fwhm = new List<double>();
            Eccentricity = new List<double>();
            SnrWeight = new List<double>();
            Median = new List<double>();
            MedianMeanDeviation = new List<double>();
            Noise = new List<double>();
        }
    }
}
