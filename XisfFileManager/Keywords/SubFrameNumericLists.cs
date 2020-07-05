﻿using System;
using System.Collections.Generic;
using System.Linq;
using XisfFileManager.Keywords;

namespace XisfFileManager.Calculations
{
    public class SubFrameNumericLists
    {
        public enum SubFrameNumericListsValidEnum { EMPTY, INVALD, VALID, MISMATCH }

        public List<bool> Approved { get; set; }

        public List<double> Eccentricity { get; set; }
        public double EccentricityScaled { get; private set; }
        public double EccentricityValue { private get; set; } = 1.0;
        public double EccentricityRangeMin { private get; set; } = 0.0;
        public double EccentricityRangeMax { private get; set; } = 1.0;
        public double EccentricityPercent { private get; set; } = 0.0;


        public List<double> EccentricityMeanDeviation { get; set; }
        public double EccentricityMeanDeviationScaled { get; private set; }
        public double EccentricityMeanDeviationValue { private get; set; } = 1.0;
        public double EccentricityMeanDeviationRangeMin { private get; set; } = 0.0;
        public double EccentricityMeanDeviationRangeMax { private get; set; } = 1.0;
        public double EccentricityMeanDeviationPercent { private get; set; } = 0.0;


        public List<double> Fwhm { get; set; }
        public double FwhmScaled { get; private set; }
        public double FwhmValue { private get; set; } = 1.0;
        public double FwhmRangeMin { private get; set; } = 0.0;
        public double FwhmRangeMax { private get; set; } = 1.0;
        public double FwhmPercent { private get; set; } = 0.0;


        public List<double> FwhmMeanDeviation { get; set; }
        public double FwhmMeanDeviationScaled { get; private set; }
        public double FwhmMeanDeviationValue { private get; set; } = 1.0;
        public double FwhmMeanDeviationRangeMin { private get; set; } = 0.0;
        public double FwhmMeanDeviationRangeMax { private get; set; } = 1.0;
        public double FwhmMeanDeviationPercent { private get; set; } = 0.0;


        public List<double> Median { get; set; }
        public double MedianScaled { get; private set; }
        public double MedianValue { private get; set; } = 1.0;
        public double MedianRangeMin { private get; set; } = 0.0;
        public double MedianRangeMax { private get; set; } = 1.0;
        public double MedianPercent { private get; set; } = 0.0;


        public List<double> MedianMeanDeviation { get; set; }
        public double MedianMeanDeviationScaled { get; private set; }
        public double MedianMeanDeviationValue { private get; set; } = 1.0;
        public double MedianMeanDeviationRangeMin { private get; set; } = 0.0;
        public double MedianMeanDeviationRangeMax { private get; set; } = 1.0;
        public double MedianMeanDeviationPercent { private get; set; } = 0.0;


        public List<double> Noise { get; set; }
        public double NoiseScaled { get; private set; }
        public double NoiseValue { private get; set; } = 1.0;
        public double NoiseRangeMin { private get; set; } = 0.0;
        public double NoiseRangeMax { private get; set; } = 1.0;
        public double NoisePercent { private get; set; } = 0.0;


        public List<double> NoiseRatio { get; set; }
        public double NoiseRatioScaled { get; private set; }
        public double NoiseRatioValue { private get; set; } = 1.0;
        public double NoiseRatioRangeMin { private get; set; } = 0.0;
        public double NoiseRatioRangeMax { private get; set; } = 1.0;
        public double NoiseRatioPercent { private get; set; } = 0.0;


        public List<double> SnrWeight { get; set; }
        public double SnrWeightScaled { get; private set; }
        public double SnrWeightValue { private get; set; } = 1.0;
        public double SnrWeightRangeMin { private get; set; } = 0.0;
        public double SnrWeightRangeMax { private get; set; } = 1.0;
        public double SnrWeightPercent { private get; set; } = 0.0;


        public List<double> StarResidual { get; set; }
        public double StarResidualScaled { get; private set; }
        public double StarResidualValue { private get; set; } = 1.0;
        public double StarResidualRangeMin { private get; set; } = 0.0;
        public double StarResidualRangeMax { private get; set; } = 1.0;
        public double StarResidualPercent { private get; set; } = 0.0;


        public List<double> StarResidualMeanDeviation { get; set; }
        public double StarResidualMeanDeviationScaled { get; private set; }
        public double StarResidualMeanDeviationValue { private get; set; } = 1.0;
        public double StarResidualMeanDeviationRangeMin { private get; set; } = 0.0;
        public double StarResidualMeanDeviationRangeMax { private get; set; } = 1.0;
        public double StarResidualMeanDeviationPercent { private get; set; } = 0.0;


        public List<double> Stars { get; set; }
        public double StarsScaled { get; private set; }
        public double StarsValue { private get; set; } = 1.0;
        public double StarsRangeMin { private get; set; } = 0.0;
        public double StarsRangeMax { private get; set; } = 1.0;
        public double StarsPercent { private get; set; } = 0.0;


        public List<double> Weight { get; set; }
        public double WeightRangeMin { private get; set; } = 50.0;
        public double WeightRangeMax { private get; set; } = 100.0;

        public List<string> FileName { get; private set; }


        public SubFrameNumericLists()
        {
            Approved = new List<bool>();
            Eccentricity = new List<double>();
            EccentricityMeanDeviation = new List<double>();
            FileName = new List<string>();
            Fwhm = new List<double>();
            FwhmMeanDeviation = new List<double>();
            Median = new List<double>();
            MedianMeanDeviation = new List<double>();
            Noise = new List<double>();
            NoiseRatio = new List<double>();
            SnrWeight = new List<double>();
            StarResidual = new List<double>();
            StarResidualMeanDeviation = new List<double>();
            Stars = new List<double>();
            Weight = new List<double>();
        }

        public void Clear()
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
            Weight.Clear();
        }

        public SubFrameNumericListsValidEnum ValidatenumericLists(int SubFrameCount)
        {
            bool bStatus = true;
            bool bZero = true;
            bool bFileExists = true;

            bStatus = Approved.Count == SubFrameCount ? bStatus : false;
            bZero = Approved.Count == 0 ? bZero : false;

            bStatus = Eccentricity.Count == SubFrameCount ? bStatus : false;
            bZero = Eccentricity.Count == 0 ? bZero : false;

            bStatus = EccentricityMeanDeviation.Count == SubFrameCount ? bStatus : false;
            bZero = EccentricityMeanDeviation.Count == 0 ? bZero : false;

            bStatus = FileName.Count == SubFrameCount ? bStatus : false;
            bZero = FileName.Count == 0 ? bZero : false;

            bStatus = Fwhm.Count == SubFrameCount ? bStatus : false;
            bZero = Fwhm.Count == 0 ? bZero : false;

            bStatus = FwhmMeanDeviation.Count == SubFrameCount ? bStatus : false;
            bZero = FwhmMeanDeviation.Count == 0 ? bZero : false;

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
                foreach (string filename in FileName)
                {
                    bFileExists = System.IO.File.Exists(filename) ? bFileExists : false;
                }
            }

            if (!bFileExists)
            {
                return SubFrameNumericListsValidEnum.MISMATCH;
            }

            if (bZero)
                return SubFrameNumericListsValidEnum.EMPTY;
            else
            {
                if (bStatus)
                    return SubFrameNumericListsValidEnum.VALID;
                else
                    return SubFrameNumericListsValidEnum.INVALD;
            }
        }


        public static double Scale(double value, double vMin, double vMax, double rMin, double rMax)
        {
            double scale = (rMax - rMin) / (vMax - vMin);
            double scaled = rMin + ((value - vMin) * scale);
            return scaled;
        }

        public double ReScaleSSWeight(double weight, double WeightRangeMin, double WeightRangeMax)
        {
            double WeightScaled = 1.0;

            //WeightScaled = SubFrameWeightLists.Scale(weight, FileSSWeight.Min(), FileSSWeight.Max(), WeightRangeMin, WeightRangeMax);

            return WeightScaled;
        }

        public int SetRejectedSubFrames(decimal FwhmMax, decimal EccentricityMax, decimal MedianMax)
        {
            int index;

            for(index = 0; index < Approved.Count; index++)
            {
                Approved[index] = true;
            }

            index = 0;
            foreach (double fwhm in Fwhm)
            {
                if (fwhm > Convert.ToDouble(FwhmMax))
                {
                    Approved[index] = false;
                }
                index++;
            }

            index = 0;
            foreach (double ecccentricity in Eccentricity)
            {
                if (ecccentricity > Convert.ToDouble(EccentricityMax))
                {
                    Approved[index] = false;
                }
                index++;
            }

            index = 0;
            foreach (double median in Median)
            {
                if (median > Convert.ToDouble(MedianMax))
                {
                    Approved[index] = false;
                }
                index++;
            }

            index = 0;
            foreach (bool approved in Approved)
            {
                if (approved == false)
                    index++;
            }

            return index;
        }

        public void WeightSubFrameValue(int SubFrameCount)
        {
            int SubFrameIndex = 0;

            List<double> weightList = new List<double>();
            double weight;

            while (SubFrameIndex < SubFrameCount)
            {
                EccentricityScaled = Scale(Eccentricity[SubFrameIndex], Eccentricity.Min(), Eccentricity.Max(), EccentricityRangeMin, EccentricityRangeMax);
                EccentricityMeanDeviationScaled = Scale(EccentricityMeanDeviation[SubFrameIndex], EccentricityMeanDeviation.Min(), EccentricityMeanDeviation.Max(), EccentricityMeanDeviationRangeMin, EccentricityMeanDeviationRangeMax);
                FwhmScaled = Scale(Fwhm[SubFrameIndex], Fwhm.Min(), Fwhm.Max(), FwhmRangeMin, FwhmRangeMax);
                FwhmMeanDeviationScaled = Scale(FwhmMeanDeviation[SubFrameIndex], FwhmMeanDeviation.Min(), FwhmMeanDeviation.Max(), FwhmMeanDeviationRangeMin, FwhmMeanDeviationRangeMax);
                MedianScaled = Scale(Median[SubFrameIndex], Median.Min(), Median.Max(), MedianRangeMin, MedianRangeMax);
                MedianMeanDeviationScaled = Scale(MedianMeanDeviation[SubFrameIndex], MedianMeanDeviation.Min(), MedianMeanDeviation.Max(), MedianMeanDeviationRangeMin, MedianMeanDeviationRangeMax);
                NoiseScaled = Scale(Noise[SubFrameIndex], Noise.Min(), Noise.Max(), NoiseRangeMin, NoiseRangeMax);
                NoiseRatioScaled = Scale(NoiseRatio[SubFrameIndex], NoiseRatio.Min(), NoiseRatio.Max(), NoiseRatioRangeMin, NoiseRatioRangeMax);
                SnrWeightScaled = Scale(SnrWeight[SubFrameIndex], SnrWeight.Min(), SnrWeight.Max(), SnrWeightRangeMin, SnrWeightRangeMax);
                StarResidualScaled = Scale(StarResidual[SubFrameIndex], StarResidual.Min(), StarResidual.Max(), StarResidualRangeMin, StarResidualRangeMax);
                StarResidualMeanDeviationScaled = Scale(StarResidualMeanDeviation[SubFrameIndex], StarResidualMeanDeviation.Min(), StarResidualMeanDeviation.Max(), StarResidualMeanDeviationRangeMin, StarResidualMeanDeviationRangeMax);
                StarsScaled = Scale(Stars[SubFrameIndex], Stars.Min(), Stars.Max(), StarsRangeMin, StarsRangeMax);

                weight = EccentricityScaled * EccentricityPercent + EccentricityMeanDeviationScaled * EccentricityMeanDeviationPercent +
                            FwhmScaled + FwhmPercent + FwhmMeanDeviationScaled * FwhmMeanDeviationPercent + MedianScaled * MedianPercent +
                            MedianMeanDeviationScaled * MedianMeanDeviationPercent + NoiseScaled * NoisePercent + NoiseRatioScaled * NoiseRatioPercent +
                            SnrWeightScaled * SnrWeightPercent + StarResidualScaled * StarResidualPercent + StarResidualMeanDeviationScaled * StarResidualMeanDeviationPercent +
                            StarsScaled * StarsPercent;

                weightList.Add(weight);

                SubFrameIndex++;
            }

            SubFrameIndex = 0;
            while (SubFrameIndex < SubFrameCount)
            {
                Weight[SubFrameIndex] = Scale(weightList[SubFrameIndex], weightList.Min(), weightList.Max(), WeightRangeMin, WeightRangeMax);
                SubFrameIndex++;
            }
        }

        public void BuildNumericSubFrameDataKeywordLists(SubFrameLists KeywordLists)
        {
            foreach (Keyword keyword in KeywordLists.SubFrameList.Approved)
            {
                Approved.Add((bool)Convert.ChangeType(keyword.GetValue(), typeof(bool)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.Eccentricity)
            {
                Eccentricity.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.EccentricityMeanDeviation)
            {
                EccentricityMeanDeviation.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.Fwhm)
            {
                Fwhm.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.FwhmMeanDeviation)
            {
                FwhmMeanDeviation.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.Median)
            {
                Median.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.MedianMeanDeviation)
            {
                MedianMeanDeviation.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.Noise)
            {
                Noise.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.NoiseRatio)
            {
                NoiseRatio.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.SnrWeight)
            {
                SnrWeight.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.StarResidual)
            {
                StarResidual.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.StarResidualMeanDeviation)
            {
                StarResidualMeanDeviation.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.Stars)
            {
                Stars.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.Weight)
            {
                Weight.Add((double)Convert.ChangeType(keyword.GetValue(), typeof(double)));
            }

            foreach (Keyword keyword in KeywordLists.SubFrameList.FileName)
            {
                FileName.Add((string)Convert.ChangeType(keyword.GetValue(), typeof(string)));
            }
        }
    }

    public static class Extend
    {
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
    }
}