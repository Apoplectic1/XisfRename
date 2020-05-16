using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XisfRename.Csv
{
    public static class ReadSubFrameCsv
    {
        private static string mCsvFile;
        private static SubFrameData mSubFrameData;

        public static bool ParseCsvFile(string path)
        {
            try
            {
                mCsvFile = System.IO.File.ReadAllText(path);

                ParseFields();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static bool ParseFields()
        {
            int index;
            int start;
            string fields;
            string[] lines;
            bool GetColumnIndexes;

            start = mCsvFile.IndexOf("Index,");
            mCsvFile = mCsvFile.Substring(start);

            int end = mCsvFile.IndexOf('\n');

            // Start mCsvFile at line containing Index,Approved,...
            fields = mCsvFile.Substring(0, end);
            fields = fields.Replace(" ", "");

            mSubFrameData = new SubFrameData();


            lines = mCsvFile.Split('\n');

            GetColumnIndexes = true;

            foreach (string line in lines)
            {
                index = 0;

                if (GetColumnIndexes)
                {
                    foreach (string field in fields.Split(','))
                    {
                        FindApprovedIndex(field, index);
                        FindFileIndex(field, index);
                        FindWeightIndex(field, index);
                        FindFwhmIndex(field, index);
                        FindEccentricityIndex(field, index);
                        FindFSnrWeightIndex(field, index);
                        FindMedianIndex(field, index);
                        FindMedianMeanDeviationIndex(field, index);
                        FindNoiseIndex(field, index);
                        index++;
                    }
                    GetColumnIndexes = false;
                    continue;
                }

                foreach (string field in line.Split(','))
                {
                    FindApproved(field, index);
                    FindFile(field, index);
                    FindWeight(field, index);
                    FindFwhm(field, index);
                    FindEccentricity(field, index);
                    FindFSnrWeight(field, index);
                    FindMedian(field, index);
                    FindMedianMeanDeviation(field, index);
                    FindNoise(field, index);
                    index++;
                }
            }

            return true;
        }


        private static void FindApprovedIndex(string field, int index)
        {
            if (field.Equals("Approved"))
            {
                mSubFrameData.ApprovedIndex = index;
            }
        }
        private static void FindApproved(string field, int index)
        {
            if (index == mSubFrameData.ApprovedIndex)
            {
                mSubFrameData.Approved.Add(field.ToUpper().Equals("TRUE") ? true : false);
            }
        }


        private static void FindFileIndex(string field, int index)
        {
            if (field.Equals("File"))
            {
                mSubFrameData.FileIndex = index;
            }
        }
        private static void FindFile(string field, int index)
        {
            if (index == mSubFrameData.FileIndex)
            {
                mSubFrameData.File.Add(field.Replace("\"", ""));
            }
        }


        private static void FindWeightIndex(string field, int index)
        {
            if (field.Equals("Weight"))
            {
                mSubFrameData.WeightIndex = index;
            }
        }
        private static void FindWeight(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.WeightIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Weight.Add((status == true) ? value : -1);
            }
        }


        private static void FindFwhmIndex(string field, int index)
        {
            if (field.Equals("FWHM"))
            {
                mSubFrameData.FwhmIndex = index;
            }
        }
        private static void FindFwhm(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.FwhmIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Fwhm.Add((status == true) ? value : -1);
            }
        }


        private static void FindEccentricityIndex(string field, int index)
        {
            if (field.Equals("Eccentricity"))
            {
                mSubFrameData.EccentricityIndex = index;
            }
        }
        private static void FindEccentricity(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.EccentricityIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Eccentricity.Add((status == true) ? value : -1);
            }
        }

        private static void FindFSnrWeightIndex(string field, int index)
        {
            if (field.Equals("SNRWeight"))
            {
                mSubFrameData.SnrWeightIndex = index;
            }
        }
        private static void FindFSnrWeight(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.SnrWeightIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.SnrWeight.Add((status == true) ? value : -1);
            }
        }

        private static void FindMedianIndex(string field, int index)
        {
            if (field.Equals("Median"))
            {
                mSubFrameData.MedianIndex = index;
            }
        }
        private static void FindMedian(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.MedianIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Median.Add((status == true) ? value : -1);
            }
        }


        private static void FindMedianMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("MedianMeanDeviation"))
            {
                mSubFrameData.MedianMeanDeviationIndex = index;
            }
        }
        private static void FindMedianMeanDeviation(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.MedianMeanDeviationIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.MedianMeanDeviation.Add((status == true) ? value : -1);
            }
        }


        private static void FindNoiseIndex(string field, int index)
        {
            if (field.Equals("Noise"))
            {
                mSubFrameData.NoiseIndex = index;
            }
        }
        private static void FindNoise(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.NoiseIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Noise.Add((status == true) ? value : -1);
            }
        }
    }
}
