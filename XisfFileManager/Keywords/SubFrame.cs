using System.Collections.Generic;

namespace XisfFileManager.Keywords
{
    public class SubFrame
    {
        public enum eValidation { EMPTY, INVALD, VALID, MISMATCH }
        public List<Keyword> Approved { get; set; }
        public List<Keyword> AirMass { get; set; }
        public List<Keyword> Eccentricity { get; set; }
        public List<Keyword> EccentricityMeanDeviation { get; set; }
        public List<Keyword> FileName { get; set; }
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
        public List<Keyword> Weight { get; set; }

        public SubFrame()
        {
            Approved = new List<Keyword>();
            AirMass = new List<Keyword>();
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
            StarResidual = new List<Keyword>();
            StarResidualMeanDeviation = new List<Keyword>();
            Stars = new List<Keyword>();
            Weight = new List<Keyword>();
        }

        public void Clear()
        {
            Approved.Clear();
            AirMass.Clear();
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
            Weight.Clear();
        }

        public eValidation Validate(int SubFrameCount)
        {
            bool bStatus = true;
            bool bZero = true;
            bool bFileExists = true;

            bStatus = Approved.Count == SubFrameCount ? bStatus : false;
            bZero = Approved.Count == 0 ? bZero : false;

            bStatus = AirMass.Count == SubFrameCount ? bStatus : false;
            bZero = AirMass.Count == 0 ? bZero : false;

            bStatus = Eccentricity.Count == SubFrameCount ? bStatus : false;
            bZero = Eccentricity.Count == 0 ? bZero : false;

            bStatus = EccentricityMeanDeviation.Count == SubFrameCount ? bStatus : false;
            bZero = EccentricityMeanDeviation.Count == 0 ? bZero : false;

            bStatus = FileName.Count == SubFrameCount ? bStatus : false;
            bZero = FileName.Count == 0 ? bZero : false;

            bStatus = Fwhm.Count == SubFrameCount ? bStatus : false;
            bZero = Fwhm.Count == 0 ? bZero : false;

            bStatus = FwhmMeanDeviation.Count == SubFrameCount ? bStatus : false;
            bZero = EccentricityMeanDeviation.Count == 0 ? bZero : false;

            bStatus = Median.Count == SubFrameCount ? bStatus : false;
            bZero = Median.Count == 0 ? bZero : false;

            bStatus = MedianMeanDeviation.Count == SubFrameCount ? bStatus : false;
            bZero = MedianMeanDeviation.Count == 0 ? bZero : false;

            bStatus = Noise.Count == SubFrameCount ? bStatus : false;
            bZero = Noise.Count == 0 ? bZero : false;

            bStatus = NoiseRatio.Count == SubFrameCount ? bStatus : false;
            bZero = NoiseRatio.Count == 0 ? bZero : false;

            bStatus = SnrWeight.Count == SubFrameCount ? bStatus : false;
            bZero = SnrWeight.Count == 0 ? bZero : false;

            bStatus = StarResidual.Count == SubFrameCount ? bStatus : false;
            bZero = StarResidual.Count == 0 ? bZero : false;

            bStatus = StarResidualMeanDeviation.Count == SubFrameCount ? bStatus : false;
            bZero = StarResidualMeanDeviation.Count == 0 ? bZero : false;

            bStatus = Stars.Count == SubFrameCount ? bStatus : false;
            bZero = Stars.Count == 0 ? bZero : false;

            bStatus = Weight.Count == SubFrameCount ? bStatus : false;
            bZero = Weight.Count == 0 ? bZero : false;

            if (bZero == false)
            {
                foreach (Keyword filename in FileName)
                {
                    bFileExists = System.IO.File.Exists(filename.Value) ? bFileExists : false;
                }
            }

            if (!bFileExists)
            {
                return eValidation.MISMATCH;
            }

            if (bZero)
            {
                return eValidation.EMPTY;
            }
            else
            {
                if (bStatus)
                    return eValidation.VALID;
                else
                    return eValidation.INVALD;
            }
        }
    }
}
