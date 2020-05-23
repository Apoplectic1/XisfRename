namespace XisfRename.CsvData
{
    public static class ReadSubFrameCsv
    {
        private static string mCsvFile;
        private static SubFrameData mSubFrameData;

        public static bool ParseSubFrameSelectorCsvFile(string path)
        {
            try
            {
                mCsvFile = System.IO.File.ReadAllText(path);

                ParseFields();

                return true;
            }
            catch
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
                        GetCsvWeightIndex(field, index);

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
                    AddCsvWeight(field, index);

                    index++;
                }
            }

            return true;
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvApprovedIndex(string field, int index)
        {
            if (field.Equals("Approved"))
            {
                mSubFrameData.ApprovedIndex = index;
            }
        }

        private static void AddCsvApproved(string field, int index)
        {
            if (index == mSubFrameData.ApprovedIndex)
            {
                mSubFrameData.Approved.Add(field.ToUpper().Equals("TRUE") ? true : false);
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvEccentricityIndex(string field, int index)
        {
            if (field.Equals("Eccentricity"))
            {
                mSubFrameData.EccentricityIndex = index;
            }
        }

        private static void AddCsvEccentricity(string field, int index)
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

        private static void GetCsvEccentricityMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("EccentricityMeanDeviation"))
            {
                mSubFrameData.EccentricityMeanDeviationIndex = index;
            }
        }

        private static void AddCsvEccentricityMeanDeviation(string field, int index)
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

        private static void GetCsvFileIndex(string field, int index)
        {
            if (field.Equals("File"))
            {
                mSubFrameData.FileIndex = index;
            }
        }

        private static void AddCsvFileName(string field, int index)
        {
            if (index == mSubFrameData.FileIndex)
            {
                mSubFrameData.File.Add(field.Replace("\"", ""));
            }
        }

        // **************************************************************************************************************
        // **************************************************************************************************************

        private static void GetCsvFwhmIndex(string field, int index)
        {
            if (field.Equals("FWHM"))
            {
                mSubFrameData.FwhmIndex = index;
            }
        }

        private static void AddCsvFwhm(string field, int index)
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

        private static void GetCsvFwhmMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("FwhmMeanDeviation"))
            {
                mSubFrameData.FwhmMeanDeviationIndex = index;
            }
        }

        private static void AddCsvFwhmMeanDeviation(string field, int index)
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

        private static void GetCsvMedianIndex(string field, int index)
        {
            if (field.Equals("Median"))
            {
                mSubFrameData.MedianIndex = index;
            }
        }

        private static void AddCsvMedian(string field, int index)
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

        private static void GetCsvMedianMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("MedianMeanDeviation"))
            {
                mSubFrameData.MedianMeanDeviationIndex = index;
            }
        }

        private static void AddCsvMedianMeanDeviation(string field, int index)
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

        private static void GetCsvNoiseIndex(string field, int index)
        {
            if (field.Equals("Noise"))
            {
                mSubFrameData.NoiseIndex = index;
            }
        }
        private static void AddCsvNoise(string field, int index)
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

        private static void GetCsvNoiseRatioIndex(string field, int index)
        {
            if (field.Equals("NoiseRatio"))
            {
                mSubFrameData.NoiseRatioIndex = index;
            }
        }
        private static void AddCsvNoiseRatio(string field, int index)
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

        private static void GetCsvSnrWeightIndex(string field, int index)
        {
            if (field.Equals("SNRWeight"))
            {
                mSubFrameData.SnrWeightIndex = index;
            }
        }
        private static void AddCsvSnrWeight(string field, int index)
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

        private static void GetCsvStarsIndex(string field, int index)
        {
            if (field.Equals("Stars"))
            {
                mSubFrameData.StarsIndex = index;
            }
        }
        private static void AddCsvStars(string field, int index)
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

        private static void GetCsvStarResidualIndex(string field, int index)
        {
            if (field.Equals("StarResidual"))
            {
                mSubFrameData.StarResidualIndex = index;
            }
        }
        private static void AddCsvStarResidual(string field, int index)
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

        private static void GetCsvStarResidualMeanDeviationIndex(string field, int index)
        {
            if (field.Equals("StarResidual"))
            {
                mSubFrameData.StarResidualIndex = index;
            }
        }
        private static void AddCsvStarResidualMeanDeviation(string field, int index)
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

        private static void GetCsvWeightIndex(string field, int index)
        {
            if (field.Equals("Weight"))
            {
                mSubFrameData.WeightIndex = index;
            }
        }
        private static void AddCsvWeight(string field, int index)
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
