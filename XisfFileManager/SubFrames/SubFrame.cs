using System.Collections.Generic;

namespace XisfFileManager.Keywords
{
    public class SubFrame
    {
        public enum eValidation { EMPTY, INVALD, VALID, MISMATCH }
        public List<Keyword> ApprovedList { get; set; }
        public List<Keyword> AirMassList { get; set; }
        public List<Keyword> Eccentricity { get; set; }
        public List<Keyword> EccentricityMeanDeviation { get; set; }
        public List<Keyword> FileNameList { get; set; }
        public List<Keyword> FwhmList { get; set; }
        public List<Keyword> FwhmMeanDeviationList { get; set; }
        public List<Keyword> MedianList { get; set; }
        public List<Keyword> MedianMeanDeviationList { get; set; }
        public List<Keyword> NoiseList { get; set; }
        public List<Keyword> NoiseRatioList { get; set; }
        public List<Keyword> SnrWeightList { get; set; }
        public List<Keyword> StarResidualList { get; set; }
        public List<Keyword> StarResidualMeanDeviationList { get; set; }
        public List<Keyword> StarsList { get; set; }
        public List<Keyword> WeightList { get; set; }

        public SubFrame()
        {
            ApprovedList = new List<Keyword>();
            AirMassList = new List<Keyword>();
            Eccentricity = new List<Keyword>();
            EccentricityMeanDeviation = new List<Keyword>();
            FileNameList = new List<Keyword>();
            FwhmList = new List<Keyword>();
            FwhmMeanDeviationList = new List<Keyword>();
            MedianList = new List<Keyword>();
            MedianMeanDeviationList = new List<Keyword>();
            NoiseList = new List<Keyword>();
            NoiseRatioList = new List<Keyword>();
            SnrWeightList = new List<Keyword>();
            StarResidualList = new List<Keyword>();
            StarResidualMeanDeviationList = new List<Keyword>();
            StarsList = new List<Keyword>();
            WeightList = new List<Keyword>();
        }

        public void Clear()
        {
            ApprovedList.Clear();
            AirMassList.Clear();
            Eccentricity.Clear();
            EccentricityMeanDeviation.Clear();
            FileNameList.Clear();
            FwhmList.Clear();
            FwhmMeanDeviationList.Clear();
            MedianList.Clear();
            MedianMeanDeviationList.Clear();
            NoiseList.Clear();
            NoiseRatioList.Clear();
            SnrWeightList.Clear();
            StarResidualList.Clear();
            StarResidualMeanDeviationList.Clear();
            StarsList.Clear();
            WeightList.Clear();
        }

        public eValidation Validate(int SubFrameCount)
        {
            bool bStatus = true;
            bool bZero = true;
            bool bFileExists = true;

            bStatus = ApprovedList.Count == SubFrameCount ? bStatus : false;
            bZero = ApprovedList.Count == 0 ? bZero : false;

            bStatus = AirMassList.Count == SubFrameCount ? bStatus : false;
            bZero = AirMassList.Count == 0 ? bZero : false;

            bStatus = Eccentricity.Count == SubFrameCount ? bStatus : false;
            bZero = Eccentricity.Count == 0 ? bZero : false;

            bStatus = EccentricityMeanDeviation.Count == SubFrameCount ? bStatus : false;
            bZero = EccentricityMeanDeviation.Count == 0 ? bZero : false;

            bStatus = FileNameList.Count == SubFrameCount ? bStatus : false;
            bZero = FileNameList.Count == 0 ? bZero : false;

            bStatus = FwhmList.Count == SubFrameCount ? bStatus : false;
            bZero = FwhmList.Count == 0 ? bZero : false;

            bStatus = FwhmMeanDeviationList.Count == SubFrameCount ? bStatus : false;
            bZero = EccentricityMeanDeviation.Count == 0 ? bZero : false;

            bStatus = MedianList.Count == SubFrameCount ? bStatus : false;
            bZero = MedianList.Count == 0 ? bZero : false;

            bStatus = MedianMeanDeviationList.Count == SubFrameCount ? bStatus : false;
            bZero = MedianMeanDeviationList.Count == 0 ? bZero : false;

            bStatus = NoiseList.Count == SubFrameCount ? bStatus : false;
            bZero = NoiseList.Count == 0 ? bZero : false;

            bStatus = NoiseRatioList.Count == SubFrameCount ? bStatus : false;
            bZero = NoiseRatioList.Count == 0 ? bZero : false;

            bStatus = SnrWeightList.Count == SubFrameCount ? bStatus : false;
            bZero = SnrWeightList.Count == 0 ? bZero : false;

            bStatus = StarResidualList.Count == SubFrameCount ? bStatus : false;
            bZero = StarResidualList.Count == 0 ? bZero : false;

            bStatus = StarResidualMeanDeviationList.Count == SubFrameCount ? bStatus : false;
            bZero = StarResidualMeanDeviationList.Count == 0 ? bZero : false;

            bStatus = StarsList.Count == SubFrameCount ? bStatus : false;
            bZero = StarsList.Count == 0 ? bZero : false;

            bStatus = WeightList.Count == SubFrameCount ? bStatus : false;
            bZero = WeightList.Count == 0 ? bZero : false;

            if (bZero == false)
            {
                foreach (Keyword filename in FileNameList)
                {
                    bFileExists = System.IO.File.Exists(filename.Name) ? bFileExists : false;
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
