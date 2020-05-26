using System;
using System.Windows.Forms;
using XisfFileManager.XisfKeywords;

namespace XisfFileManager.SubFrameData
{
    public static class ReadSubFrameCsv
    {
        private static string mCsvFile;
        public static SubFrameData SubFrameKeywordLists;

        public static bool ParseSubFrameSelectorCsvFile(string path)
        {
            try
            {
                mCsvFile = System.IO.File.ReadAllText(path);

                ParseFields();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ParseSubFrameSelectorCsvFile(" + path + "))");
                return false;
            }
        }

        private static void ClearSubFrameLists()
        {
            SubFrameKeywordLists.Approved.Clear();
            SubFrameKeywordLists.Eccentricity.Clear();
            SubFrameKeywordLists.EccentricityMeanDeviation.Clear();
            SubFrameKeywordLists.FileName.Clear();
            SubFrameKeywordLists.Fwhm.Clear();
            SubFrameKeywordLists.FwhmMeanDeviation.Clear();
            SubFrameKeywordLists.Median.Clear();
            SubFrameKeywordLists.MedianMeanDeviation.Clear();
            SubFrameKeywordLists.Noise.Clear();
            SubFrameKeywordLists.NoiseRatio.Clear();
            SubFrameKeywordLists.SnrWeight.Clear();
            SubFrameKeywordLists.StarResidual.Clear();
            SubFrameKeywordLists.StarResidualMeanDeviation.Clear();
            SubFrameKeywordLists.Stars.Clear();
            SubFrameKeywordLists.SSWeight.Clear();
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

            SubFrameKeywordLists = new SubFrameData();

            lines = mCsvFile.Split('\n');

            GetColumnIndexes = true;

            ClearSubFrameLists();

            foreach (string line in lines)
            {
                index = 0;

                if (GetColumnIndexes)
                {
                    foreach (string field in fields.Split(','))
                    {
                        GetCsvApprovedIndex(field, index);
                        GetCsvEccentricityIndex(field, index);
                        GetCsvEccentricityMeanDeviationIndex(field, index);
                        GetCsvFileIndex(field, index);
                        GetCsvFwhmIndex(field, index);
                        GetCsvFwhmMeanDeviationIndex(field, index);
                        GetCsvMedianIndex(field, index);
                        GetCsvMedianMeanDeviationIndex(field, index);
                        GetCsvNoiseIndex(field, index);
                        GetCsvNoiseRatioIndex(field, index);
                        GetCsvSnrWeightIndex(field, index);
                        GetCsvStarResidualIndex(field, index);
                        GetCsvStarResidualMeanDeviationIndex(field, index);
                        GetCsvStarsIndex(field, index);
                        GetCsvSSWeightIndex(field, index);

                        index++;
                    }

                    GetColumnIndexes = false;
                    continue;
                }

                foreach (string field in line.Split(','))
                {
                    AddCsvApproved(field, index);
                    AddCsvEccentricity(field, index);
                    AddCsvEccentricityMeanDeviation(field, index);
                    AddCsvFileName(field, index);
                    AddCsvFwhm(field, index);
                    AddCsvFwhmMeanDeviation(field, index);
                    AddCsvMedian(field, index);
                    AddCsvMedianMeanDeviation(field, index);
                    AddCsvNoise(field, index);
                    AddCsvNoiseRatio(field, index);
                    AddCsvSnrWeight(field, index);
                    AddCsvStarResidual(field, index);
                    AddCsvStarResidualMeanDeviation(field, index);
                    AddCsvStars(field, index);
                    AddCsvSSWeight(field, index);

                    index++;
                }
            }

            return true;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, string value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value;
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.STRING;
            return keyword;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, double value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString("F6");
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.FLOAT;
            return keyword;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, int value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString();
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.INTEGER;
            return keyword;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, bool value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString();
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.BOOL;
            return keyword;
        }
        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvApprovedIndex(string field, int index)
        {
            if (field.Equals("Approved"))
            {
                SubFrameKeywordLists.ApprovedIndex = index;
            }
        }

        private static void AddCsvApproved(string field, int index)
        {
            if (index == SubFrameKeywordLists.ApprovedIndex)
            {
                bool value = (field.ToLower().Equals("true")) ? true : false;
                SubFrameKeywordLists.Approved.Add(BuildKeyword("Approved", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvEccentricityIndex(string field, int index)
        {
            if (field.Equals("Eccentricity"))
            {
                SubFrameKeywordLists.EccentricityIndex = index;
            }
        }

        private static void AddCsvEccentricity(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.EccentricityIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.Eccentricity.Add(BuildKeyword("Eccentricity", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvEccentricityMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("EccentricityMeanDeviation"))
            {
                SubFrameKeywordLists.EccentricityMeanDeviationIndex = index;
            }
        }

        private static void AddCsvEccentricityMeanDeviation(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.EccentricityMeanDeviationIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.EccentricityMeanDeviation.Add(BuildKeyword("EccentricityMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvFileIndex(string field, int index)
        {
            if (field.Equals("File"))
            {
                SubFrameKeywordLists.FileNameIndex = index;
            }
        }

        private static void AddCsvFileName(string field, int index)
        {
            if (index == SubFrameKeywordLists.FileNameIndex)
            {
                SubFrameKeywordLists.FileName.Add(BuildKeyword("FileName", field));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvFwhmIndex(string field, int index)
        {
            if (field.Equals("FWHM"))
            {
                SubFrameKeywordLists.FwhmIndex = index;
            }
        }

        private static void AddCsvFwhm(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.FwhmIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.Fwhm.Add(BuildKeyword("Fwhm", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvFwhmMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("FWHMMeanDeviation"))
            {
                SubFrameKeywordLists.FwhmMeanDeviationIndex = index;
            }
        }

        private static void AddCsvFwhmMeanDeviation(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.FwhmMeanDeviationIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.FwhmMeanDeviation.Add(BuildKeyword("FwhmMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvMedianIndex(string field, int index)
        {
            if (field.Equals("Median"))
            {
                SubFrameKeywordLists.MedianIndex = index;
            }
        }

        private static void AddCsvMedian(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.MedianIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.Median.Add(BuildKeyword("Median", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvMedianMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("MedianMeanDeviation"))
            {
                SubFrameKeywordLists.MedianMeanDeviationIndex = index;
            }
        }

        private static void AddCsvMedianMeanDeviation(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.MedianMeanDeviationIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.MedianMeanDeviation.Add(BuildKeyword("MedianMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvNoiseIndex(string field, int index)
        {
            if (field.Equals("Noise"))
            {
                SubFrameKeywordLists.NoiseIndex = index;
            }
        }
        private static void AddCsvNoise(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.NoiseIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.Noise.Add(BuildKeyword("Noise", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvNoiseRatioIndex(string field, int index)
        {
            if (field.Equals("NoiseRatio"))
            {
                SubFrameKeywordLists.NoiseRatioIndex = index;
            }
        }
        private static void AddCsvNoiseRatio(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.NoiseRatioIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.NoiseRatio.Add(BuildKeyword("NoiseRatio", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvSnrWeightIndex(string field, int index)
        {
            if (field.Equals("SNRWeight"))
            {
                SubFrameKeywordLists.SnrWeightIndex = index;
            }
        }
        private static void AddCsvSnrWeight(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.SnrWeightIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.SnrWeight.Add(BuildKeyword("SNRWeight", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvStarsIndex(string field, int index)
        {
            if (field.Equals("Stars"))
            {
                SubFrameKeywordLists.StarsIndex = index;
            }
        }
        private static void AddCsvStars(string field, int index)
        {
            int value = int.MinValue;

            if (index == SubFrameKeywordLists.StarsIndex)
            {
                int.TryParse(field, out value);
                SubFrameKeywordLists.Stars.Add(BuildKeyword("Stars", value)); ;
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvStarResidualIndex(string field, int index)
        {
            if (field.Equals("StarResidual"))
            {
                SubFrameKeywordLists.StarResidualIndex = index;
            }
        }
        private static void AddCsvStarResidual(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.StarResidualIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.StarResidual.Add(BuildKeyword("StarResidual", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvStarResidualMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("StarResidualMeanDeviation"))
            {
                SubFrameKeywordLists.StarResidualMeanDeviationIndex = index;
            }
        }
        private static void AddCsvStarResidualMeanDeviation(string field, int index)
        {
            double value = Double.NaN;

            if (index == SubFrameKeywordLists.StarResidualMeanDeviationIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.StarResidualMeanDeviation.Add(BuildKeyword("StarResidualMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvSSWeightIndex(string field, int index)
        {
            if (field.Equals("Weight"))
            {
                SubFrameKeywordLists.SSWeightIndex = index;
            }
        }
        private static void AddCsvSSWeight(string field, int index)
        {
            double value = 1.0;

            if (index == SubFrameKeywordLists.SSWeightIndex)
            {
                double.TryParse(field, out value);
                SubFrameKeywordLists.SSWeight.Add(BuildKeyword("SSWEIGHT", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

    }
}
