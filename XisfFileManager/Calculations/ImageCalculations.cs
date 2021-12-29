using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using XisfFileManager.FileOperations;

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

            WeightScaled = SubFrameNumericLists.Scale(weight, FileSSWeight.Min(), FileSSWeight.Max(), WeightRangeMin, WeightRangeMax);

            return WeightScaled;
        }

        public void UpdateSSWeight(bool bUpdateWight, bool bUseCsvWeightList, KeywordLists Keywords, SubFrameNumericLists CsvSubFrameKeywordLists, SubFrameNumericLists FileSubFrameKeywordLists)
        {

        }


        public void BuildImageParameterValueLists(KeywordLists Keywords)
        {
            string imageType;

            imageType = Keywords.FrameType();

            if (imageType == "L")
            {
                FocuserPosition.Add(Convert.ToDouble(Keywords.FocuserPosition()));
                FocuserTemperature.Add(Convert.ToDouble(Keywords.FocuserTemperature()));
                AmbientTemperature.Add(Convert.ToDouble(Keywords.AmbientTemperature()));

                FileSSWeight.Add(Convert.ToDouble(Keywords.AmbientTemperature()));
            }
        }

        public string CalculateOverhead(List<XisfFile> fileList)
        {
            int index;
            bool status;
            double exposure;
            List<double> subFrameIntervalList;
            List<double> subFrameExposureList;
            List<double> exposureList;
            List<double> intervalList;

            subFrameIntervalList = new List<double>();
            subFrameExposureList = new List<double>();

            exposureList = new List<double>();
            intervalList = new List<double>();

            if (fileList.Count <= 1)
            {
                return "SubFrame Overhead: Not Calculated";
            }

            XisfFile fistFile = fileList[0];

            DateTime firstInterval = new DateTime(2000, 1, 1, 0, 0, 0);
            DateTime secondInterval = new DateTime(2000, 1, 1, 0, 0, 0);


            foreach ( XisfFile file in fileList)
            {
                secondInterval = file.KeywordData.CaptureDateTime();
                double delta = secondInterval.Subtract(firstInterval).TotalSeconds;
                firstInterval = secondInterval;

                if (delta > (60 * 60))  // Don't add if over 1 hour between frames
                {
                    continue;
                }

                subFrameIntervalList.Add(delta);
 
                status = Double.TryParse(file.KeywordData.ExposureSeconds(), out exposure);
                if (status == false)
                {
                    return "SubFrame Overhead: Calculation Error";
                }
                subFrameExposureList.Add(exposure);
            }

            double sigma = subFrameIntervalList.StandardDeviation();

            if (sigma.Equals(double.NaN))
            {
                return "SubFrame Overhead: Calculation Error";
            }

            for(index = 0; index < subFrameIntervalList.Count; index++)
            {
                double interval = Math.Abs(subFrameIntervalList[index]);

                if ((interval < ((sigma * 4) + subFrameIntervalList.Mean())) && (interval > 0))
                {
                    intervalList.Add(subFrameIntervalList[index]);
                    exposureList.Add(subFrameExposureList[index]);
                }
            }

            double totalIntervalTime = intervalList.Sum();
            double totalExposureTime = exposureList.Sum();

            double overheadPercent = ((totalIntervalTime - totalExposureTime) / totalExposureTime) * 100.0;
            double overheadSeconds = (totalIntervalTime - totalExposureTime) / exposureList.Count();
         
            return "SubFrame Overhead: " + overheadPercent.ToString("F0") + " % with " + overheadSeconds.ToString("F0") + 
                   " Seconds/Frame Average Overhead\n                                   SubFrame Exposure: " + (totalExposureTime / exposureList.Count()).ToString("F0") + " Seconds" +
                   " over " + exposureList.Count() + " Frames";
        }

        public string CalculateFocuserTemperatureCompensationCoefficient()
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
                pt = new PositionTemperature
                {
                    Position = FocuserPosition[index],
                    Temperature = Math.Round(FocuserTemperature[index], 1)
                };
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

            return p.Item2.ToString("F0") + " Steps/Degree over " + positionArray.Length + " positions from " + minTemperature.ToString("F1") + " to " + maxTemperature.ToString("F1") + " C";
        }
    }
}
