using System;
using System.Collections.Generic;
using System.Linq;
using XisfFileManager.Keywords;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;

namespace XisfFileManager.Calculations
{
    public class ImageCalculations
    {
        public List<double> FocuserPosition { get; set; }
        public List<double> FocuserTemperature { get; set; }
        public List<double> AmbientTemperature { get; set; }
        public List<double> FileSSWeight { get; set; }


        public ImageCalculations()
        {
            FocuserPosition = new List<double>();
            FocuserTemperature = new List<double>();
            AmbientTemperature = new List<double>();
            FileSSWeight = new List<double>();
        }

        public void Clear()
        {
            FocuserPosition.Clear();
            FocuserTemperature.Clear();
            AmbientTemperature.Clear();
            FileSSWeight.Clear();
        }

        public double ReScaleFileSSWeight(double weight, double WeightRangeMin, double WeightRangeMax)
        {
            double WeightScaled;

            WeightScaled = NumericWeightLists.Scale(weight, FileSSWeight.Min(), FileSSWeight.Max(), WeightRangeMin, WeightRangeMax);

            return WeightScaled;
        }

        public void UpdateSSWeight(bool bUpdateWight, bool bUseCsvWeightList, KeywordLists Keywords, NumericWeightLists CsvSubFrameKeywordLists, NumericWeightLists FileSubFrameKeywordLists)
        {

        }


        public void BuildImageParameterValueLists(KeywordLists Keywords)
        {
            FocuserPosition.Add(Convert.ToDouble(Keywords.FocuserPosition()));
            FocuserTemperature.Add(Convert.ToDouble(Keywords.FocuserTemperature()));
            AmbientTemperature.Add(Convert.ToDouble(Keywords.AmbientTemperature()));

            FileSSWeight.Add(Convert.ToDouble(Keywords.AmbientTemperature()));
        }


        public string ComputeFocuserTemperatureCompensationCoefficient()
        {
            int index;
            PositionTemperature pt;

            if (FocuserPosition.Count != FocuserTemperature.Count || FocuserPosition.Count == 0 || FocuserTemperature.Count == 0)
            {
                return "Invalid Focuser Data";
            }

            List<PositionTemperature> ptList = new List<PositionTemperature>();

            for (index = 0; index < FocuserPosition.Count(); index++)
            {
                pt = new PositionTemperature();

                pt.Position = FocuserPosition[index];
                pt.Temperature = Math.Round(FocuserTemperature[index], 1);
                ptList.Add(pt);
            }

            var positionAverageTemperature =
                from position in ptList
                group position by position.Position into positionGroup
                select new
                {
                    Position = positionGroup.Key,
                    AverageTemperature = positionGroup.Distinct().Average(x => x.Temperature),
                };

            double[] positionArray = new double[positionAverageTemperature.Count()];
            double[] temperatureArray = new double[positionAverageTemperature.Count()];

            index = 0;
            foreach (var item in positionAverageTemperature)
            {
                positionArray[index] = item.Position;
                temperatureArray[index] = item.AverageTemperature;
                index++;
            }

            if (index == 1)
            {
                return "Single Focus Point";
            }
            Array.Sort(temperatureArray, positionArray);

            Tuple<double, double> p = Fit.Line(temperatureArray, positionArray);

            double maxTemperature = FocuserTemperature.Max();
            double minTemperature = FocuserTemperature.Min();

            return p.Item2.ToString("F1") + " Steps/Degree over " + positionArray.Length + " positions from " + minTemperature.ToString("F1") + " to " + maxTemperature.ToString("F1") + " C";
        }

        public int RejectSubFrames(NumericWeightLists weightLists)
        {
            return 0;
        }


        public double FindMean(List<double> valueList)
        {
            return valueList.Mean();
        }
    }
}
