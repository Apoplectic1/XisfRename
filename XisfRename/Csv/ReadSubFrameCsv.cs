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
                        GetApprovedIndex(field, index);
                        GetEccentricityIndex(field, index);
                        GetEccentricityMeanDeviationIndex(field, index);
                        GetFileIndex(field, index);
                        GetFwhmIndex(field, index);
                        GetFwhmMeanDeviationIndex(field, index);
                        GetMedianIndex(field, index);
                        GetMedianMeanDeviationIndex(field, index);
                        GetNoiseIndex(field, index);
                        GetNoiseRatioIndex(field, index);
                        GetSnrWeightIndex(field, index);
                        GetStarResidualIndex(field, index);
                        GetStarResidualMeanDeviationIndex(field, index);
                        GetStarsIndex(field, index);
                        GetWeightIndex(field, index);

                        index++;
                    }

                    GetColumnIndexes = false;
                    continue;
                }

                foreach (string field in line.Split(','))
                {
                    AddApproved(field, index);
                    AddEccentricity(field, index);
                    AddEccentricityMeanDeviation(field, index);
                    AddFile(field, index);
                    AddFwhm(field, index);
                    AddFwhmMeanDeviation(field, index);
                    AddMedian(field, index);
                    AddMedianMeanDeviation(field, index);
                    AddNoise(field, index);
                    AddNoiseRatio(field, index);
                    AddSnrWeight(field, index);
                    AddStarResidual(field, index);
                    AddStarResidualMeanDeviation(field, index);
                    AddStars(field, index);
                    AddWeight(field, index);

                    index++;
                }
            }

            return true;
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetApprovedIndex(string field, int index)
        {
            if (field.Equals("Approved"))
            {
                mSubFrameData.ApprovedIndex = index;
            }
        }

        private static void AddApproved(string field, int index)
        {
            if (index == mSubFrameData.ApprovedIndex)
            {
                mSubFrameData.Approved.Add(field.ToUpper().Equals("TRUE") ? true : false);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetEccentricityIndex(string field, int index)
        {
            if (field.Equals("Eccentricity"))
            {
                mSubFrameData.EccentricityIndex = index;
            }
        }

        private static void AddEccentricity(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.EccentricityIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Eccentricity.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetEccentricityMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("EccentricityMeanDeviation"))
            {
                mSubFrameData.EccentricityMeanDeviationIndex = index;
            }
        }

        private static void AddEccentricityMeanDeviation(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.EccentricityMeanDeviationIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.EccentricityMeanDeviation.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetFileIndex(string field, int index)
        {
            if (field.Equals("File"))
            {
                mSubFrameData.FileIndex = index;
            }
        }

        private static void AddFile(string field, int index)
        {
            if (index == mSubFrameData.FileIndex)
            {
                mSubFrameData.File.Add(field.Replace("\"", ""));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetFwhmIndex(string field, int index)
        {
            if (field.Equals("FWHM"))
            {
                mSubFrameData.FwhmIndex = index;
            }
        }

        private static void AddFwhm(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.FwhmIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Fwhm.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetFwhmMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("FwhmMeanDeviation"))
            {
                mSubFrameData.FwhmMeanDeviationIndex = index;
            }
        }

        private static void AddFwhmMeanDeviation(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.FwhmMeanDeviationIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.FwhmMeanDeviation.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetMedianIndex(string field, int index)
        {
            if (field.Equals("Median"))
            {
                mSubFrameData.MedianIndex = index;
            }
        }

        private static void AddMedian(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.MedianIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Median.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetMedianMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("MedianMeanDeviation"))
            {
                mSubFrameData.MedianMeanDeviationIndex = index;
            }
        }

        private static void AddMedianMeanDeviation(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.MedianMeanDeviationIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.MedianMeanDeviation.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetNoiseIndex(string field, int index)
        {
            if (field.Equals("Noise"))
            {
                mSubFrameData.NoiseIndex = index;
            }
        }
        private static void AddNoise(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.NoiseIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Noise.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetNoiseRatioIndex(string field, int index)
        {
            if (field.Equals("NoiseRatio"))
            {
                mSubFrameData.NoiseRatioIndex = index;
            }
        }
        private static void AddNoiseRatio(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.NoiseRatioIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.NoiseRatio.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetSnrWeightIndex(string field, int index)
        {
            if (field.Equals("SNRWeight"))
            {
                mSubFrameData.SnrWeightIndex = index;
            }
        }
        private static void AddSnrWeight(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.SnrWeightIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.SnrWeight.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetStarsIndex(string field, int index)
        {
            if (field.Equals("Stars"))
            {
                mSubFrameData.StarsIndex = index;
            }
        }
        private static void AddStars(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.StarsIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Stars.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetStarResidualIndex(string field, int index)
        {
            if (field.Equals("StarResidual"))
            {
                mSubFrameData.StarResidualIndex = index;
            }
        }
        private static void AddStarResidual(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.StarResidualIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.StarResidual.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetStarResidualMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("StarResidual"))
            {
                mSubFrameData.StarResidualIndex = index;
            }
        }
        private static void AddStarResidualMeanDeviation(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.StarResidualMeanDeviationIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.StarResidualMeanDeviation.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetWeightIndex(string field, int index)
        {
            if (field.Equals("Weight"))
            {
                mSubFrameData.WeightIndex = index;
            }
        }
        private static void AddWeight(string field, int index)
        {
            bool status;
            double value;

            if (index == mSubFrameData.WeightIndex)
            {
                status = double.TryParse(field, out value);

                mSubFrameData.Weight.Add((status == true) ? value : -1);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

    }
}
