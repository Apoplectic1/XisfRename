using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

using XisfFileManager.Enums;
using XisfFileManager.FileOperations;

namespace XisfFileManager.Calculations
{
    public class ImageCalculations
    {
        public List<double> mFocuserPositionList { get; set; }
        public List<double> mFocuserTemperatureList { get; set; }
        public List<double> mAmbientTemperatureList { get; set; }
        public List<double> mFileSSWeightList { get; set; }


        public ImageCalculations()
        {
            mFocuserPositionList = new List<double>();
            mFocuserTemperatureList = new List<double>();
            mAmbientTemperatureList = new List<double>();
            mFileSSWeightList = new List<double>();
        }

        public void Clear()
        {
            mFocuserPositionList.Clear();
            mFocuserTemperatureList.Clear();
            mAmbientTemperatureList.Clear();
            mFileSSWeightList.Clear();
        }

        public double ReScaleFileSSWeight(double weight, double WeightRangeMin, double WeightRangeMax)
        {
            double WeightScaled;

            WeightScaled = SubFrameNumericLists.Scale(weight, mFileSSWeightList.Min(), mFileSSWeightList.Max(), WeightRangeMin, WeightRangeMax);

            return WeightScaled;
        }

        public void BuildImageParameterValueLists(XisfFile xFile)
        {

            eFrame imageType = xFile.FrameType;

            if (imageType == eFrame.LIGHT)
            {
                int focusPosition = xFile.FocuserPosition;
                double focusTemperature = xFile.FocuserTemperature;
                double ambientTemperature = xFile.AmbientTemperature;

                if ((focusPosition != int.MinValue) && (focusTemperature != -273.0) && (ambientTemperature != -273.0))
                {
                    mFocuserPositionList.Add(focusPosition);
                    mFocuserTemperatureList.Add(focusTemperature);
                    mAmbientTemperatureList.Add(ambientTemperature);
                }
                else
                {
                    mFocuserPositionList.Add(-1);
                    mFocuserTemperatureList.Add(-273);
                    mAmbientTemperatureList.Add(-273);
                }


                //FileSSWeight.Add(Convert.ToDouble(Keywords.AmbientTemperature()));
            }
        }

        public string CalculateOverhead(List<XisfFile> fileList)
        {
            int index;
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
                secondInterval = file.CaptureDateTime;
                double delta = secondInterval.Subtract(firstInterval).TotalSeconds;
                firstInterval = secondInterval;

                if (delta > (60 * 60))  // Don't add if over 1 hour between frames
                {
                    continue;
                }

                subFrameIntervalList.Add(delta);

                subFrameExposureList.Add(file.ExposureSeconds);
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
            double overheadSeconds = (totalIntervalTime - totalExposureTime) / exposureList.Count;

            return "SubFrame Overhead: " + overheadPercent.ToString("F0") + " % over " + overheadSeconds.ToString("F0") + " Seconds/Frame\n" +
                   "Average Exposure:     " + (totalExposureTime / exposureList.Count).ToString("F0") + " Seconds" + " over " + exposureList.Count + " Frames";
        }

        public string CalculateFocuserTemperatureCompensationCoefficient()
        {
            int index;

            if ((mFocuserPositionList.Count != mFocuserTemperatureList.Count) || (mFocuserPositionList.Count < 3) || mFocuserTemperatureList.Contains(-273))
            {
                return "No Focuser Position Data";
            }

            try
            {
                List<PositionTemperature> ptList = new List<PositionTemperature>();

                for (index = 0; index < mFocuserPositionList.Count; index++)
                {
                    PositionTemperature pt = new PositionTemperature
                    {
                        Position = mFocuserPositionList[index],
                        Temperature = mFocuserTemperatureList[index]
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
