﻿using System;
using System.Windows.Forms;
using XisfFileManager.Keywords;

namespace XisfFileManager.FileOperations
{
    public static class ReadSubFrameCsvData
    {
        private static string mCsvFile;
        public static int ApprovedCsvIndex = -1;
        public static int EccentricityCsvIndex = -1;
        public static int EccentricityMeanDeviationCsvIndex = -1;
        public static int FileNameCsvIndex = -1;
        public static int FwhmCsvIndex = -1;
        public static int FwhmMeanDeviationCsvIndex = -1;
        public static int MedianCsvIndex = -1;
        public static int MedianMeanDeviationCsvIndex = -1;
        public static int NoiseCsvIndex = -1;
        public static int NoiseRatioCsvIndex = -1;
        public static int SnrWeightCsvIndex = -1;
        public static int StarResidualIndex = -1;
        public static int StarResidualMeanDeviationCsvIndex = -1;
        public static int StarsCsvIndex = -1;
        public static int SSWeightcsvIndex = -1;

        public static bool ReadCsvFile(string path, SubFrameLists CsvSubFrameKeywordLists)
        {
            try
            {
                mCsvFile = System.IO.File.ReadAllText(path);

                ParseFields(CsvSubFrameKeywordLists);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ParseSubFrameSelectorCsvFile(" + path + "))");
                return false;
            }
        }

       

        private static bool ParseFields(SubFrameLists CsvSubFrameKeywordLists)
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

            lines = mCsvFile.Split('\n');

            GetColumnIndexes = true;

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
                        GetCsvWeightIndex(field, index);
                        GetCsvStarsIndex(field, index);

                        index++;
                    }

                    GetColumnIndexes = false;
                    continue;
                }

                foreach (string field in line.Split(','))
                {
                    AddCsvApproved(field, index, CsvSubFrameKeywordLists);
                    AddCsvEccentricity(field, index, CsvSubFrameKeywordLists);
                    AddCsvEccentricityMeanDeviation(field, index, CsvSubFrameKeywordLists);
                    AddCsvFileName(field, index, CsvSubFrameKeywordLists);
                    AddCsvFwhm(field, index, CsvSubFrameKeywordLists);
                    AddCsvFwhmMeanDeviation(field, index, CsvSubFrameKeywordLists);
                    AddCsvMedian(field, index, CsvSubFrameKeywordLists);
                    AddCsvMedianMeanDeviation(field, index, CsvSubFrameKeywordLists);
                    AddCsvNoise(field, index, CsvSubFrameKeywordLists);
                    AddCsvNoiseRatio(field, index, CsvSubFrameKeywordLists);
                    AddCsvSnrWeight(field, index, CsvSubFrameKeywordLists);
                    AddCsvStarResidual(field, index, CsvSubFrameKeywordLists);
                    AddCsvStarResidualMeanDeviation(field, index, CsvSubFrameKeywordLists);
                    AddCsvStars(field, index, CsvSubFrameKeywordLists);
                    AddCsvWeight(field, index, CsvSubFrameKeywordLists);

                    index++;
                }
            }

            return true;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, string value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value,
                Comment = comment,
            };
            return keyword;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, double value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value.ToString("F6"),
                Comment = comment,
            };
            return keyword;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, int value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value.ToString(),
                Comment = comment,
            };
            return keyword;
        }

        // #########################################################################################################
        // #########################################################################################################
        public static Keyword BuildKeyword(string name, bool value, string comment = "XISF File Manager - CSV")
        {
            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value.ToString(),
                Comment = comment,
            };
            return keyword;
        }
        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvApprovedIndex(string field, int index)
        {
            if (field.Equals("Approved"))
            {
                ApprovedCsvIndex = index;
            }
        }

        private static void AddCsvApproved(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == ApprovedCsvIndex)
            {
                bool value = (field.ToLower().Equals("true")) ? true : false;
                CsvSubFrameKeywordLists.SubFrameList.ApprovedList.Add(BuildKeyword("Approved", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvEccentricityIndex(string field, int index)
        {
            if (field.Equals("Eccentricity"))
            {
                EccentricityCsvIndex = index;
            }
        }

        private static void AddCsvEccentricity(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == EccentricityCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.Eccentricity.Add(BuildKeyword("Eccentricity", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvEccentricityMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("EccentricityMeanDeviation"))
            {
                EccentricityMeanDeviationCsvIndex = index;
            }
        }

        private static void AddCsvEccentricityMeanDeviation(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == EccentricityMeanDeviationCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.EccentricityMeanDeviation.Add(BuildKeyword("EccentricityMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvFileIndex(string field, int index)
        {
            if (field.Equals("File"))
            {
                FileNameCsvIndex = index;
            }
        }

        private static void AddCsvFileName(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == FileNameCsvIndex)
            {
                CsvSubFrameKeywordLists.SubFrameList.FileNameList.Add(BuildKeyword("FileName", field));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvFwhmIndex(string field, int index)
        {
            if (field.Equals("FWHM"))
            {
                FwhmCsvIndex = index;
            }
        }

        private static void AddCsvFwhm(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == FwhmCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.FwhmList.Add(BuildKeyword("Fwhm", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvFwhmMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("FWHMMeanDeviation"))
            {
                FwhmMeanDeviationCsvIndex = index;
            }
        }

        private static void AddCsvFwhmMeanDeviation(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == FwhmMeanDeviationCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.FwhmMeanDeviationList.Add(BuildKeyword("FwhmMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvMedianIndex(string field, int index)
        {
            if (field.Equals("Median"))
            {
                MedianCsvIndex = index;
            }
        }

        private static void AddCsvMedian(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == MedianCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.MedianList.Add(BuildKeyword("Median", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvMedianMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("MedianMeanDeviation"))
            {
                MedianMeanDeviationCsvIndex = index;
            }
        }

        private static void AddCsvMedianMeanDeviation(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == MedianMeanDeviationCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.MedianMeanDeviationList.Add(BuildKeyword("MedianMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvNoiseIndex(string field, int index)
        {
            if (field.Equals("Noise"))
            {
                NoiseCsvIndex = index;
            }
        }
        private static void AddCsvNoise(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == NoiseCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.NoiseList.Add(BuildKeyword("Noise", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvNoiseRatioIndex(string field, int index)
        {
            if (field.Equals("NoiseRatio"))
            {
                NoiseRatioCsvIndex = index;
            }
        }
        private static void AddCsvNoiseRatio(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == NoiseRatioCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.NoiseRatioList.Add(BuildKeyword("NoiseRatio", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvSnrWeightIndex(string field, int index)
        {
            if (field.Equals("SNRWeight"))
            {
                SnrWeightCsvIndex = index;
            }
        }
        private static void AddCsvSnrWeight(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == SnrWeightCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.SnrWeightList.Add(BuildKeyword("SNRWeight", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvStarsIndex(string field, int index)
        {
            if (field.Equals("Stars"))
            {
                StarsCsvIndex = index;
            }
        }
        private static void AddCsvStars(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == StarsCsvIndex)
            {
                int value;
                int.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.StarsList.Add(BuildKeyword("Stars", value)); ;
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvStarResidualIndex(string field, int index)
        {
            if (field.Equals("StarResidual"))
            {
                StarResidualIndex = index;
            }
        }
        private static void AddCsvStarResidual(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == StarResidualIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.StarResidualList.Add(BuildKeyword("StarResidual", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvStarResidualMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("StarResidualMeanDeviation"))
            {
                StarResidualMeanDeviationCsvIndex = index;
            }
        }
        private static void AddCsvStarResidualMeanDeviation(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == StarResidualMeanDeviationCsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.StarResidualMeanDeviationList.Add(BuildKeyword("StarResidualMeanDeviation", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvWeightIndex(string field, int index)
        {
            if (field.Equals("Weight"))
            {
                SSWeightcsvIndex = index;
            }
        }
        private static void AddCsvWeight(string field, int index, SubFrameLists CsvSubFrameKeywordLists)
        {
            if (index == SSWeightcsvIndex)
            {
                double value;
                double.TryParse(field, out value);
                CsvSubFrameKeywordLists.SubFrameList.WeightList.Add(BuildKeyword("WEIGHT", value));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

    }
}
