using System.Collections.Generic;

namespace XisfFileManager.Keywords
{
    public class SubFrame
    {
        public enum eValidation { EMPTY, INVALD, VALID, MISMATCH }
        public List<FitsKeyword> ApprovedList { get; set; }
        public List<FitsKeyword> AirMassList { get; set; }
        public List<FitsKeyword> Eccentricity { get; set; }
        public List<FitsKeyword> EccentricityMeanDeviation { get; set; }
        public List<FitsKeyword> FileNameList { get; set; }
        public List<FitsKeyword> FwhmList { get; set; }
        public List<FitsKeyword> FwhmMeanDeviationList { get; set; }
        public List<FitsKeyword> MedianList { get; set; }
        public List<FitsKeyword> MedianMeanDeviationList { get; set; }
        public List<FitsKeyword> NoiseList { get; set; }
        public List<FitsKeyword> NoiseRatioList { get; set; }
        public List<FitsKeyword> SnrWeightList { get; set; }
        public List<FitsKeyword> StarResidualList { get; set; }
        public List<FitsKeyword> StarResidualMeanDeviationList { get; set; }
        public List<FitsKeyword> StarsList { get; set; }
        public List<FitsKeyword> WeightList { get; set; }

        public SubFrame()
        {
            ApprovedList = new List<FitsKeyword>();
            AirMassList = new List<FitsKeyword>();
            Eccentricity = new List<FitsKeyword>();
            EccentricityMeanDeviation = new List<FitsKeyword>();
            FileNameList = new List<FitsKeyword>();
            FwhmList = new List<FitsKeyword>();
            FwhmMeanDeviationList = new List<FitsKeyword>();
            MedianList = new List<FitsKeyword>();
            MedianMeanDeviationList = new List<FitsKeyword>();
            NoiseList = new List<FitsKeyword>();
            NoiseRatioList = new List<FitsKeyword>();
            SnrWeightList = new List<FitsKeyword>();
            StarResidualList = new List<FitsKeyword>();
            StarResidualMeanDeviationList = new List<FitsKeyword>();
            StarsList = new List<FitsKeyword>();
            WeightList = new List<FitsKeyword>();
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
                foreach (FitsKeyword filename in FileNameList)
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
