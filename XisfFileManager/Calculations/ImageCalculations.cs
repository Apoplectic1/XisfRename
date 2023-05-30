using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
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

            if ((imageType == "Light") || (imageType == "L"))
            {
                int focusPosition = Keywords.FocuserPosition();
                double focusTemperature = Keywords.FocuserTemperature();
                double ambientTemperature = Keywords.AmbientTemperature();

                if ((focusPosition != int.MinValue) && (focusTemperature != -273) && (ambientTemperature != -273.0))
                {
                    FocuserPosition.Add(focusPosition);
                    FocuserTemperature.Add(focusTemperature);
                    AmbientTemperature.Add(ambientTemperature);
                }
                else
                {
                    FocuserPosition.Add(int.MinValue);
                    FocuserTemperature.Add(-273);
                    AmbientTemperature.Add(-273);
                }


                //FileSSWeight.Add(Convert.ToDouble(Keywords.AmbientTemperature()));
            }
        }

        public string CalculateOverhead(List<XisfFile> fileList)
        {
            int index;
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


            foreach (XisfFile file in fileList)
            {
                secondInterval = file.KeywordData.CaptureDateTime();
                double delta = secondInterval.Subtract(firstInterval).TotalSeconds;
                firstInterval = secondInterval;

                if (delta > (60 * 60))  // Don't add if over 1 hour between frames
                {
                    continue;
                }

                subFrameIntervalList.Add(delta);

                exposure = (double)file.KeywordData.GetKeyword("EXPTIME", Keywords.Keyword.eType.DOUBLE);

                subFrameExposureList.Add(exposure);
            }

            double sigma = subFrameIntervalList.StandardDeviation();

            if (sigma.Equals(double.NaN))
            {
                return "SubFrame Overhead: Calculation Error";
            }

            for (index = 0; index < subFrameIntervalList.Count; index++)
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

            return "SubFrame Overhead: " + overheadPercent.ToString("F0") + " % over " + overheadSeconds.ToString("F0") + " Seconds/Frame\n" +
                   "Average Exposure:     " + (totalExposureTime / exposureList.Count()).ToString("F0") + " Seconds" + " over " + exposureList.Count() + " Frames";
        }

        public string CalculateFocuserTemperatureCompensationCoefficient()
        {
            int index;

            if ((FocuserPosition.Count != FocuserTemperature.Count) || (FocuserPosition.Count < 3) || FocuserTemperature.Contains(-273)) 
            {
                return "No Focuser Position Data";
            }

            try
            {
                List<PositionTemperature> ptList = new List<PositionTemperature>();

                for (index = 0; index < FocuserPosition.Count(); index++)
                {
                    PositionTemperature pt = new PositionTemperature
                    {
                        Position = FocuserPosition[index],
                        Temperature = FocuserTemperature[index]
                    };
                    ptList.Add(pt);
                }

                ptList.Sort((x, y) => x.Position.CompareTo(y.Position));

                var DistinctItems = ptList.GroupBy(x => x.Position).Select(y => y.First());

                double[] position = new double[DistinctItems.Count()];
                double[] temperature = new double[DistinctItems.Count()];

                index = 0;
                foreach (var item in DistinctItems)
                {
                    position[index] = item.Position;
                    temperature[index] = item.Temperature;
                    index++;
                }

                Tuple<double, double> p = Fit.Line(temperature, position).ToTuple();

                double maxTemperature = temperature.Max();
                double minTemperature = temperature.Min();

                if (p.Item2 >= 0)
                {
                    return p.Item2.ToString("F0") + " Steps/Degree IN over " + position.Length + " positions from " + maxTemperature.ToString("F1") + " to " + minTemperature.ToString("F1") + " C";
                }
                else
                {
                    return p.Item2.ToString("F0") + " Steps/Degree OUT over " + position.Length + " positions from " + maxTemperature.ToString("F1") + " to " + minTemperature.ToString("F1") + " C";
                }
            }
            catch
            {
                return "No Focuser Position Data";
            }
        }
    }
}
