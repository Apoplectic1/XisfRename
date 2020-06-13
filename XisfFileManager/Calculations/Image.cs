using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XisfFileManager.Keywords;
using MathNet.Numerics.LinearRegression;
using MathNet.Numerics;

namespace XisfFileManager.Calculations
{
    public class Image
    {
        public List<double> FocuserPosition { get; set; }
        public List<double> FocuserTemperature { get; set; }
        public List<double> AmbientTemperature { get; set; }


        public Image()
        {
            FocuserPosition = new List<double>();
            FocuserTemperature = new List<double>();
            AmbientTemperature = new List<double>();
        }

        public void ClearImageLists()
        {
            FocuserPosition.Clear();
            FocuserTemperature.Clear();
            AmbientTemperature.Clear();
        }


        public void BuildNumericImageKeywordLists(KeywordData Keywords)
        {
            FocuserPosition.Add(Convert.ToDouble(Keywords.FocuserPosition()));
            FocuserTemperature.Add(Convert.ToDouble(Keywords.FocuserTemperature()));
            AmbientTemperature.Add(Convert.ToDouble(Keywords.AmbientTemperature()));
        }


        public string ComputeFocuserTemperatureCompensationCoefficient()
        {
            int index;
            positionTemperature pt;

            if (FocuserPosition.Count != FocuserTemperature.Count || FocuserPosition.Count == 0 || FocuserTemperature.Count == 0)
            {
                return "Invalid Focuser Data";
            }

            List<positionTemperature> ptList = new List<positionTemperature>();

            for (index = 0; index < FocuserPosition.Count(); index++)
            {
                pt = new positionTemperature();

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
    }

    class positionTemperature
    {
        public double Position;
        public double Temperature;
    }
}
