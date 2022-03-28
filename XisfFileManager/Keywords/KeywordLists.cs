using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using XisfFileManager.Forms.UserInputForm;
using XisfFileManager.Keywords;


namespace XisfFileManager
{
    public class KeywordLists
    {
        public List<Keyword> KeywordList;
        enum eRejectionType { NULL, LINEAR, STUDENT, WINSOR, SIGMA }

        public KeywordLists()
        {
            KeywordList = new List<Keyword>();
        }


        // #########################################################################################################
        // #########################################################################################################

        // *********************************************************************************************************
        // *********************************************************************************************************
        // Make sure FITS keyword DATE-LOC exists and is in local time (not UTC)
        public void RepairDateLocation()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "DATE-LOC");
            if (node != null)
            {
                // DATE-LOC exists (and should already be local time) so do nothing and return
                return;
            }

            return;

            /*
            double latitude;
            double longitude;
            string value;
            string[] array;
            int position;

            RepairSiteLatitude();
            RepairSiteLongitude();

            node = KeywordList.Find(i => i.Name == "SITELAT");
            value = node.Value;
            array = value.Split(' ');

            double[] divisor = { 1.0, 60.0, 3600.0 };

            latitude = 0.0;
            position = 0;
            foreach(string arrayValue in array)
            {
                latitude += Convert.ToDouble(arrayValue) / divisor[position++];
            }

            node = KeywordList.Find(i => i.Name == "SITELON");
            value = node.Value;
            array = value.Split(' ');

            longitude = 0.0;
            position = 0;
            foreach (string arrayValue in array)
            {
                longitude += Convert.ToDouble(arrayValue) / divisor[position++];
            }

            node = KeywordList.Find(i => i.Name == "DATE-LOC");

            DateTime utcDate = CaptureDateTime();
            string tz1 = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
            
            var timeZoneDbUseCases = new TimeZoneDbUseCases();
            var allTimeZones = timeZoneDbUseCases.GetAllTimeZones();
            var timeZone = timeZoneDbUseCases.GetTimeZoneWithIanaId(tz1);

            var timeZone1 = TimeZoneInfo.FindSystemTimeZoneById(timeZone.MicrosoftId);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone1);

            */

        }


        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CDARK()
        {
            Keyword node = KeywordList.Find(i => i.Name == "CDARK");
            if (node == null) return string.Empty;

            return node.Value.Replace("'", "");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CFLAT()
        {
            Keyword node = KeywordList.Find(i => i.Name == "CFLAT");
            if (node == null) return string.Empty;

            return node.Value.Replace("'", "");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CBIAS()
        {
            Keyword node = KeywordList.Find(i => i.Name == "CBIAS");
            if (node == null) return string.Empty;

            return node.Value.Replace("'", "");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // Who and Where
        public void SetObservationSite()
        {
            AddKeyword("SITENAME", "Penns Park, PA", "841 Durham Rd, Penns Park, PA 18943");
            AddKeyword("SITELONG", -74.997372, "Logitude of observation site - Degrees East");
            AddKeyword("SITELAT", 40.282852, "Latitude of observation site - Degrees North");
            AddKeyword("SITEELEV", 80.0, "Altitude of observation site - MSL Meters");
            RemoveKeyword("LONG-OBS");
            RemoveKeyword("LAT-OBS");
            RemoveKeyword("ALT-OBS");
            RemoveKeyword("OBSGEO-L");
            RemoveKeyword("OBSGEO-B");
            RemoveKeyword("OBSGEO-H");

            AddKeyword("OBSERVER", "Dan Stark", "P.O. Box 156, Penns Park, PA 18943 djstark@gmail.com (609) 575-5927");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // Various programs appear to screw this up - fix it
        public void SetEGain()
        {
            double egain = -1.0;
            double gain = Gain(); ;
            string camera = Camera();

            if (camera == "Z183")
                egain = 3.6059 * Math.Exp(-0.011 * gain);

            if (camera == "Z533")
                egain = (-7e-13 * Math.Pow(gain, 5)) + (1e-9 * Math.Pow(gain, 4)) - (6e-7 * Math.Pow(gain, 3)) + (0.0002 * Math.Pow(gain, 2)) - (0.0356 * gain) + 3.1338;

            if (camera == "Q178")
            {
                if (gain < 4.0)
                    egain = 2.6;
                else
                    egain = 3.8018 * Math.Exp(-0.0117 * gain);
            }

            if (camera == "A144")
                egain = 0.37;

            AddKeyword("EGAIN", egain, "Calculated electrons per ADU");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLatitude()
        {
            Keyword node = KeywordList.Find(i => i.Name == "SITELAT");
            if (node == null) return;

            if (node.Value.Contains("N"))
            {
                node.Value = Regex.Replace(node.Value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");
            }

            node.Value = node.Value.Replace("'", "");
            node.Type = Keyword.eType.STRING;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLongitude()
        {
            Keyword node = KeywordList.Find(i => i.Name == "SITELONG");
            if (node == null) return;

            if (node.Value.Contains("W"))
            {
                node.Value = Regex.Replace(node.Value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                Regex regReplace = new Regex("'");

                node.Value = regReplace.Replace(node.Value, "'-", 1);
            }

            node.Type = Keyword.eType.STRING;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void IntegrationParamaters()
        {
            foreach (Keyword node in KeywordList)
            {
                if (node.Comment.ToLower().Contains("numberofimages"))
                    AddKeyword("TOTALFRAMES", node.Value, "Number of Integrated SubFrames");

                if (node.Comment.ToLower().Contains("pixelrejection"))
                {
                    if (node.Comment.ToLower().Contains("linear"))
                        AddKeyword("REJECTION", "LFC", "PixInsight Linear Fit Clipping");

                    if (node.Comment.ToLower().Contains("student"))
                        AddKeyword("REJECTION", "ESD", "PixInsight Extreme Studentized Deviation Clipping");

                    if (node.Comment.ToLower().Contains("sigma"))
                        AddKeyword("REJECTION", "SC", "PixInsight Sigma Clipping");

                    if (node.Comment.ToLower().Contains("winsor"))
                        AddKeyword("REJECTION", "WSC", "PixInsight Winsorized Sigma Clipping");
                }
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int TotalFrames(bool findMissingKeywords = false)
        {
            bool status;

            Keyword node = KeywordList.Find(i => i.Name == "TOTALFRAMES");
            node.Type = Keyword.eType.INTEGER;

            if (node != null)
                return Int32.Parse(node.Value);

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Master Frame Integration",
                    mFormText = "Integrated SubFrames Not Set",
                    mFormEntryText = "Enter Total Integration Frames:",
                    mFileName = FileName()
                };

                UserInputFormData returnValue = OpenUIForm(formData);

                status = int.TryParse(returnValue.mTextBox, out int frames);
                if (status)
                {
                    AddKeyword("TOTALFRAMES", frames, "Total number of Integrated SubFrames");

                    if (returnValue.mGlobalCheckBox)
                        return -frames;
                    else
                        return frames;
                }
            }

            return -1;
        }

        public string Rejection(bool findMissingKeywords = false)
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "REJECTION");

            if (node != null)
            {
                return node.Value;
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Master Frame Rejection Method",
                    mFormText = "Master Frame Rejection Method Not Set",
                    mFormEntryText = "Enter PixInsight Rejection Method (SC, WSC, LFC or ESD):",
                    mFileName = FileName()
                };

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("SC") || FormValue.mTextBox.Equals("WSC") || FormValue.mTextBox.Equals("LFC") || FormValue.mTextBox.Equals("ESD"))
                {
                    AddKeyword("REJECTION", FormValue.mTextBox, "PixInsight Pixel Integration Rejection Method");
                    return FormValue.mTextBox;
                }

            }

            return string.Empty;
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public string AmbientTemperature()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "AOCAMBT");

            if (node == null)
            {
                node = KeywordList.Find(i => i.Name == "TEMPERAT");
                RemoveKeyword("TEMPERAT");
            }

            if (node == null)
            {
                node = KeywordList.Find(i => i.Name == "AMB-TEMP");
                RemoveKeyword("AMB-TEMP");
            }

            if (node == null)
            {
                node = KeywordList.Find(i => i.Name == "AMBTEMP");
                RemoveKeyword("AMBTEMP");
            }

            if (node == null)
            {
                AddKeyword("AOCAMBT", FocuserTemperature());
                node = KeywordList.Find(i => i.Name == "AOCAMBT");
                node.Type = Keyword.eType.DOUBLE;
            }

            node.Type = Keyword.eType.DOUBLE;
            value = node.Value;

            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public bool Approved()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "Approved");
            if (node == null)
            {
                AddKeyword("Approved", true);
            }

            node = KeywordList.Find(i => i.Name == "Approved");

            return (node.Value == "True") ? true : false;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // This is the name of the File itself - Does not contain the path
        public string FileName()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FILENAME");
            if (node != null)
            {
                return node.Comment;
            }

            return string.Empty;
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Binning(bool findMissingKeywords = false)
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "XBINNING");
            if (node != null)
            {
                node.Value = node.Value.Replace(".", "");
                return Convert.ToInt32(node.Value);
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Cammera Binning",
                    mFormText = "Camera Binning Not Set",
                    mFormEntryText = "Enter Binning: 1, 2, 3 or 4",
                    mFileName = FileName()
                };

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("1") || FormValue.mTextBox.Equals("2") || FormValue.mTextBox.Equals("3") || FormValue.mTextBox.Equals("4"))
                {
                    int bin = Convert.ToInt32(FormValue.mTextBox);

                    AddKeyword("XBINNING", bin);
                    AddKeyword("YBINNING", bin);
                    return bin;
                }
            }

            return -1;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Camera(bool findMissingKeywords = false)
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "INSTRUME");

            if (node != null)
            {
                return node.Value.Replace("'", "");
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Capture Camera",
                    mFormText = "Capture Camera Not Set",
                    mFormEntryText = "Enter Z533, Z183, Q178 or A144:",
                    mFileName = FileName()
                };

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("Z533") || FormValue.mTextBox.Equals("Z183") || FormValue.mTextBox.Equals("Q178") || FormValue.mTextBox.Equals("A144"))
                {
                    AddKeyword("INSTRUME", FormValue.mTextBox, "XISF File Manager");
                    return FormValue.mTextBox;
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public DateTime CaptureDateTime()
        {
            string value = string.Empty;
            Keyword node = new Keyword();
            bool status;



            node = KeywordList.Find(i => i.Name == "LOCALTIM");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "DATE-LOC");

            if (node == null)
                // DATE-OBS may is in UTC vs local time
                node = KeywordList.Find(i => i.Name == "DATE-OBS");

            if (node == null)
            {
                AddKeyword("DATE-OBS", "2019-01-01T12:00:00.0000000", "Missing DateTime XISF File Manager");

                node = KeywordList.Find(i => i.Name == "DATE-OBS");
            }

            value = node.Value.Replace("'", "");
            node.Type = Keyword.eType.STRING;

            if (value.Contains("AM"))
            {
                value = value.Remove(value.IndexOf('.') + 4) + " AM";

                status = DateTime.TryParseExact(value, "M/d/yyyy hh:mm:ss.fffffff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out DateTime dt);

                if (status) return dt;

                status = DateTime.TryParseExact(value, "M/d/yyyy hh:mm:ss.fff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                return dt;
            }

            if (value.Contains("PM"))
            {
                value = value.Remove(value.IndexOf('.') + 4) + " PM";

                status = DateTime.TryParseExact(value, "M/d/yyyy hh:mm:ss.fffffff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out DateTime dt);

                if (status) return dt;

                status = DateTime.TryParseExact(value, "M/d/yyyy hh:mm:ss.fff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                return dt;
            }

            value = value.Replace("T", "  ").Replace("'", "");

            status = DateTime.TryParse(value, out DateTime parsedDateTime);

            if (status)
            {
                return parsedDateTime;
            }


            return DateTime.ParseExact(value, "yyyy-MM-dd  HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CaptureSoftware(bool FindMissingKeywords = false)
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "CREATOR");

            if (node != null)
            {
                node.Value = node.Value.Replace("'", "");

                if (node.Value.Contains("Sequence"))
                {
                    AddKeyword("CREATOR", "SGP", node.Value);
                    return "SGP";
                }

                if (node.Value.Contains("VOYAGER"))
                {
                    AddKeyword("CREATOR", "VOY", node.Value);
                    return "VOY";
                }

                if (node.Value.Contains("Sky"))
                {
                    AddKeyword("CREATOR", "TSX", node.Value);
                    return "TSX";
                }

                if (node.Value.Contains("N.I.N.A."))
                {
                    AddKeyword("CREATOR", "NINA", node.Value);
                    return "NINA";
                }

                if (node.Value.Contains("Sharp"))
                {
                    AddKeyword("CREATOR", "SCP", node.Value);
                    return "SCP";
                }

                if (node.Value.Equals("SGP") || node.Value.Equals("VOY") || node.Value.Equals("TSX") || node.Value.Equals("SCP") || node.Value.Equals("NINA"))
                    return (node.Value);
            }

            while (FindMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Capture Software",
                    mFormText = "Capture Software Not Set",
                    mFormEntryText = "Enter NINA, SGP, TSX, VOY or SCP:",
                    mFileName = FileName()
                };

                UserInputFormData returnData = OpenUIForm(formData);

                if (returnData.mTextBox.Equals("SGP") || returnData.mTextBox.Equals("NINA") || returnData.mTextBox.Equals("TSX") || returnData.mTextBox.Equals("VOY") || returnData.mTextBox.Equals("SCP"))
                {
                    AddKeyword("CREATOR", returnData.mTextBox, "XISF File Manager");

                    if (returnData.mGlobalCheckBox)
                        return "Global_" + returnData.mTextBox;
                    else
                        return returnData.mTextBox;
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ExposureSeconds(bool FindMissingKeywords = false)
        {
            double Seconds;
            bool status;
            Keyword node = new Keyword();


            node = KeywordList.Find(i => i.Name == "EXPTIME");
            if (node != null)
            {
                RemoveKeyword("EXPOSURE");
                Seconds = Convert.ToDouble(node.Value);
                return FormatExposureSeconds(Seconds);
            }

            node = KeywordList.Find(i => i.Name == "EXPOSURE");
            if (node != null)
            {
                RemoveKeyword("EXPOSURE");
                Seconds = Convert.ToDouble(node.Value);
                AddKeyword("EXPTIME", Seconds);
                return FormatExposureSeconds(Seconds);
            }

            while (FindMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Exposure Time",
                    mFormText = "Exposure Time Not Set",
                    mFormEntryText = "Enter Exposure Time in Seconds (double):",
                    mFileName = FileName()
                };

                UserInputFormData returnData = OpenUIForm(formData);

                status = double.TryParse(returnData.mTextBox, out double seconds);
                if (status)
                {
                    AddKeyword("EXPTIME", seconds, "Camera Exposure Time in Seconds");

                    if (returnData.mGlobalCheckBox)
                        return "Global_" + FormatExposureSeconds(seconds);
                    else
                        return FormatExposureSeconds(seconds);
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FilterName(bool FindMissingKeywords = false)
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FILTER");

            if (node != null)
            {
                value = node.Value.Replace("'", "").Replace(" ", "");

                if (value.ToUpper().StartsWith("L")) value = "Luma";
                if (value.ToUpper().StartsWith("H")) value = "Ha";
                if (value.ToUpper().StartsWith("O")) value = "O3";
                if (value.ToUpper().StartsWith("S")) value = "S2";
                if (value.ToUpper().StartsWith("R")) value = "Red";
                if (value.ToUpper().StartsWith("G")) value = "Green";
                if (value.ToUpper().StartsWith("B")) value = "Blue";
                if (value.ToUpper().StartsWith("SH")) value = "Shutter";

                if (value == "Luma")
                    AddKeyword("FILTER", "Luma", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (value == "Red")
                    AddKeyword("FILTER", "Red", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (value == "Green")
                    AddKeyword("FILTER", "Green", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (value == "Blue")
                    AddKeyword("FILTER", "Blue", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (value == "Ha")
                    AddKeyword("FILTER", "Ha", "Astrodon E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (value == "O3")
                    AddKeyword("FILTER", "O3", "Astrodon E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (value == "S2")
                    AddKeyword("FILTER", "S2", "Astrodon E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (value == "Shutter")
                    AddKeyword("FILTER", "Shutter", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");

                return value;
            }

            while (FindMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Filter Type",
                    mFormText = "Filter Type Not Set",
                    mFormEntryText = "Enter Filter (L, R, G, B, Ha, O3, S2 or S):",
                    mFileName = FileName()
                };

                UserInputFormData returnData = OpenUIForm(formData);

                if (returnData.mTextBox.Equals("L") || returnData.mTextBox.Equals("R") || returnData.mTextBox.Equals("G") || returnData.mTextBox.Equals("B") ||
                    returnData.mTextBox.Equals("Ha") || returnData.mTextBox.Equals("O3") || returnData.mTextBox.Equals("S2") || returnData.mTextBox.Equals("S"))
                {
                    AddKeyword("FILTER", returnData.mTextBox, "XISF File Manager");

                    if (returnData.mGlobalCheckBox)
                        return "Global_" + returnData.mTextBox;
                    else
                        return returnData.mTextBox;
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FrameType(bool FindMissingKeywords = false)
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "IMAGETYP");

            if (node != null)
            {
                value = node.Value.Replace("'", "").Replace(" ", "");

                if (value.StartsWith("L")) value = "Light";
                if (value.StartsWith("D")) value = "Dark";
                if (value.StartsWith("F")) value = "Flat";
                if (value.StartsWith("B")) value = "Bias";

                AddKeyword("IMAGETYP", value, node.Comment);

                return value;
            }

            while (FindMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Frame Type",
                    mFormText = "Frame Type Not Set",
                    mFormEntryText = "Enter Frame Type (L, D, F or B):",
                    mFileName = FileName()
                };

                UserInputFormData returnData = OpenUIForm(formData);

                if (returnData.mTextBox.Equals("L") || returnData.mTextBox.Equals("D") || returnData.mTextBox.Equals("F") || returnData.mTextBox.Equals("B"))
                {
                    if (returnData.mTextBox.Equals("L")) value = "Light";
                    if (returnData.mTextBox.Equals("D")) value = "Dark";
                    if (returnData.mTextBox.Equals("F")) value = "Flat";
                    if (returnData.mTextBox.Equals("B")) value = "Bias";

                    AddKeyword("IMAGETYP", returnData.mTextBox, "XISF File Manager");

                    if (returnData.mGlobalCheckBox)
                        return "Global_" + returnData.mTextBox;
                    else
                        return returnData.mTextBox;
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int FocalLength(bool findMissingKeywords = false)
        {
            bool status;
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FOCALLEN");
            if (node != null)
            {
                node.Value = node.Value.Replace(".", "");
                return (Convert.ToInt32(node.Value));
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Focal Length",
                    mFormText = "Focal Length Not Set",
                    mFormEntryText = "Enter Focal Length in millimeters:",
                    mFileName = FileName()
                };

                UserInputFormData formValue = OpenUIForm(formData);

                status = int.TryParse(formValue.mTextBox, out int focalLength);
                if (status)
                {
                    AddKeyword("FOCALLEN", focalLength, "Focal Length");

                    if (formValue.mGlobalCheckBox)
                        return -focalLength;
                    else
                        return focalLength;
                }
            }

            return -1;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public object FocuserPosition(Keyword.eType eType)
        {
            object value = GetKeyword("FOCPOS", eType);
            if (value != null)
                return value;

            value = GetKeyword("FOCUSPOS", eType);
            if (value == null) return null;

            RemoveKeyword("FOCUSPOS");
            AddKeyword("FOCPOS", (int)value);

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocuserTemperature()
        {
            string value = (string)GetKeyword("FOCTEMP", Keyword.eType.STRING);
            if (value != null)
                return FormatTemperatureString(value);

            double? dValue = (double)GetKeyword("FOCUSTEM", Keyword.eType.DOUBLE);
            if (dValue == null) return string.Empty;

            RemoveKeyword("FOCUSTEM");
            AddKeyword("FOCTEMP", (double)dValue);

            value = (string)GetKeyword("FOCTEMP");
            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Gain(bool findMissingKeywords = false)
        {
            int? value = (int?)GetKeyword("GAIN");
            if (value != null)
                return (int)value;

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Cammera Gain",
                    mFormText = "Camera Gain Not Set",
                    mFormEntryText = "Enter Camera Gain: ",
                    mFileName = FileName()
                };

                UserInputFormData returnValue = OpenUIForm(formData);

                int gain;
                bool bStatus = Int32.TryParse(returnValue.mTextBox, out gain);
                if (bStatus)
                {
                    AddKeyword("GAIN", gain);

                    if (returnValue.mGlobalCheckBox)
                        return -gain;
                    else
                        return gain;
                }
            }

            return -1;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ImageAngle()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "POSANGLE");
            if (node == null)
            {
                node = KeywordList.Find(i => i.Name == "ROTATANG");
                if (node != null)
                {
                    RemoveKeyword("ROTATANG");
                    AddKeyword("POSANGLE", node.Value, node.Comment);
                }
            }

            if (node == null) return string.Empty;
            node.Type = Keyword.eType.DOUBLE;

            return String.Format("{0:000.0}", Convert.ToDouble(node.Value));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Offset(bool findMissingKeywords = false)
        {
            int? value = (int?)GetKeyword("OFFSET");
            if (value != null)
                return (int)value;


            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Camera Offset",
                    mFormText = "Camera Offset Not Set",
                    mFormEntryText = "Enter Camera Offset:",
                    mFileName = FileName()
                };

                UserInputFormData returnValue = OpenUIForm(formData);

                bool status = Int32.TryParse(returnValue.mTextBox, out int offset);
                if (status)
                {
                    AddKeyword("OFFSET", offset);

                    if (returnValue.mGlobalCheckBox)
                        return -offset;
                    else
                        return offset;
                }
            }

            return -1;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double PixelSize()
        {
            double? value = (double?)GetKeyword("XPIXSZ");
            if (value != null)
                return (double)value;


            UserInputFormData formData = new UserInputFormData
            {
                mFormName = "Cammera Pixel Size",
                mFormText = "Camera Pixel Size Not Set (Assumes Square Pixels)",
                mFormEntryText = "Enter Camera Pixel Size (2.4, 3.76, 6.45):",
                mFileName = FileName()
            };


            UserInputFormData FormValue = OpenUIForm(formData);
            AddKeyword("XPIXSZ", Convert.ToDouble(FormValue.mTextBox));
            AddKeyword("YPIXSZ", Convert.ToDouble(FormValue.mTextBox));

            return Convert.ToDouble(FormValue.mTextBox);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SensorSetPointTemperature()
        {
            string value = (string)GetKeyword("SET-TEMP");
            if (value != null)
                return (string)value;

            Keyword node = KeywordList.Find(i => i.Name == "SET-TEMP");
            if (node != null)
            {
                return FormatTemperatureString(node.Value);
            }

            UserInputFormData formData = new UserInputFormData
            {
                mFormName = "Cammera Temperature",
                mFormText = "Camera Temperature Not Set",
                mFormEntryText = "Enter Camera Temperature Setpoint:",
                mFileName = FileName()
            };

            UserInputFormData FormValue = OpenUIForm(formData);
            AddKeyword("SET-TEMP", FormValue.mTextBox);

            return FormatTemperatureString(FormValue.mTextBox);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SensorTemperature(bool findMissingKeywords = false)
        {
            string value = string.Empty;
            bool status;

            Keyword node = KeywordList.Find(i => i.Name == "CCD-TEMP");
            if (node != null)
            {
                return FormatTemperatureString(node.Value);
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Camera Sensor Temperature",
                    mFormText = "Camera Sensor Temperature Not Set",
                    mFormEntryText = "Enter Actual Sensor Temperature:",
                    mFileName = FileName()
                };

                UserInputFormData returnData = OpenUIForm(formData);

                status = double.TryParse(returnData.mTextBox, out double temperature);
                if (status)
                {
                    AddKeyword("CCD-TEMP", temperature);

                    if (returnData.mGlobalCheckBox)
                        return "Global_" + FormatTemperatureString(temperature.ToString());
                    else
                        return FormatTemperatureString(temperature.ToString());
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SiteLocation()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "SITENAME");
            value = node.Value;
            node.Type = Keyword.eType.STRING;

            return value.Replace("'", "");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double SSWeight()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "SSWEIGHT");

            if (node == null) return Double.NaN;
            double SSWeight = Convert.ToDouble(node.GetKeyword());

            if (Double.IsNaN(SSWeight)) return Double.NaN;

            return Convert.ToDouble(Math.Round(Convert.ToDecimal(SSWeight), 3, MidpointRounding.AwayFromZero));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string TargetName(bool findMissingKeywords = false)
        {
            string value = (string)GetKeyword("OBJECT");
            if (value != null)
                return value;


            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Target Name",
                    mFormText = "Target Name Not Set",
                    mFormEntryText = "Enter Target Name (ID or Master):",
                    mFileName = FileName()
                };

                UserInputFormData returnData = OpenUIForm(formData);

                AddKeyword("OBJECT", returnData.mTextBox, "XISF File Manager");

                if (returnData.mGlobalCheckBox)
                    return "Global_" + returnData.mTextBox;
                else
                    return returnData.mTextBox;
            }

            return string.Empty;
        }

        // #########################################################################################################
        // #########################################################################################################
        public string Telescope(bool findMissingKeywords = false)
        {
            Keyword node = KeywordList.Find(i => i.Name == "TELESCOP");
            if (node != null)
            {
                node.Value = node.Value.Replace(".", "").Replace("'", "");
                return node.Value;
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Telescope",
                    mFormText = "Telescope Not Set",
                    mFormEntryText = "Enter APM(R), EVO(R) or NWT(R):",
                    mFileName = FileName()
                };

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("APM") || FormValue.mTextBox.Equals("EVO") || FormValue.mTextBox.Equals("NWT") ||
                    FormValue.mTextBox.Equals("APMR") || FormValue.mTextBox.Equals("EVOR") || FormValue.mTextBox.Equals("NWTR"))
                {
                    if (FormValue.mTextBox.Contains("APM"))
                        if (FormValue.mTextBox.EndsWith("R"))
                            AddKeyword("TELESCOP", "APM107R", "APM107 Super ED w/Riccardi 0.75 Reducer");
                        else
                            AddKeyword("TELESCOP", "APM107", "APM107 Super ED wo/Reducer");

                    if (FormValue.mTextBox.Contains("EVO"))
                        if (FormValue.mTextBox.EndsWith("R"))
                            AddKeyword("TELESCOP", "EVO150R", "Skyhawtcher EvoStar w/Riccardi 0.75 Reducer");
                        else
                            AddKeyword("TELESCOP", "EVO150", "SkyWatcher EvoStar wo/Reducer");

                    if (FormValue.mTextBox.Contains("NWT"))
                        if (FormValue.mTextBox.EndsWith("R"))
                            AddKeyword("TELESCOP", "NWT254R", "10 Inch Custom w/Riccardi 0.75 Reducer");
                        else
                            AddKeyword("TELESCOP", "NWT254", "10 Inch Custom Newtonian wo/Reducer");

                    if (FormValue.mGlobalCheckBox)
                        return "Global_" + FormValue.mTextBox;
                    else
                        return FormValue.mTextBox;
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string WeightKeyword(bool findMissingKeywords = false)
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SSWEIGHT");
            if (node != null)
            {
                return node.Name;
            }

            node = KeywordList.Find(i => i.Name == "NWEIGHT");
            if (node != null)
            {
                return node.Name;
            }

            return string.Empty;
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, string value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value,
                Comment = comment,
                Type = Keyword.eType.STRING
            };

            keyword.SetKeyword(name, value, comment);
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, double value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value.ToString("F6"),
                Comment = comment,
                Type = Keyword.eType.DOUBLE
            };


            keyword.SetKeyword(name, value, comment);
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, int value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value.ToString(),
                Comment = comment,
                Type = Keyword.eType.INTEGER
            };


            keyword.SetKeyword(name, value, comment);
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, bool value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword
            {
                Name = name,
                Value = value.ToString(),
                Comment = comment,
                Type = Keyword.eType.BOOL
            };

            keyword.SetKeyword(name, value, comment);

            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(XElement element)
        {
            bool bStatus;

            bool bBool;
            bStatus = bool.TryParse(element.Attribute("value").Value, out bBool);
            if (bStatus)
            {
                AddKeyword(element.Attribute("name").Value, bBool, element.Attribute("comment").Value);
                return;
            }

            int iInt32;
            bStatus = Int32.TryParse(element.Attribute("value").Value, out iInt32);
            if (bStatus)
            {
                AddKeyword(element.Attribute("name").Value, iInt32, element.Attribute("comment").Value);
                return;
            }

            double dDouble;
            bStatus = double.TryParse(element.Attribute("value").Value, out dDouble);
            if (bStatus)
            {
                AddKeyword(element.Attribute("name").Value, dDouble, element.Attribute("comment").Value);
                return;
            }

            AddKeyword(element.Attribute("name").Value, (string)element.Attribute("value").Value, element.Attribute("comment").Value);

        }

        // #########################################################################################################
        // #########################################################################################################
        public object GetKeyword(string sName)
        {
            Keyword node = KeywordList.Find(i => i.Name == sName);
            if (node == null)
                return null;

            return node.GetKeyword();
        }

        // #########################################################################################################
        // #########################################################################################################
        public object GetKeyword(string sName, Keyword.eType eType)
        {
            Keyword node = KeywordList.Find(i => i.Name == sName);
            if (node == null)
                return null;

            return node.GetKeyword(eType);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void RemoveKeyword(string name)
        {
            KeywordList.RemoveAll(i => i.Name.Contains(name));
        }

        // #########################################################################################################
        // #########################################################################################################
        private string FormatTemperatureString(string temperatureString)
        {
            if (temperatureString == "") return "";

            double temperature;
            temperature = Convert.ToDouble(temperatureString.Replace("'", ""));
            temperature = Math.Round(temperature, 1);

            string fmt = "{00:+00.0;-00.0;+00.0}";
            string value = string.Format(fmt, temperature);

            return value;
        }

        // #########################################################################################################
        // #########################################################################################################
        public string FormatExposureSeconds(double seconds)
        {
            if (seconds < 1.0)
            {
                if (seconds <= 0.0001)
                {
                    return "0000";
                }

                return seconds.ToString("0.000");
            }
            else
            {
                return seconds.ToString("0000");
            }
        }

        // #########################################################################################################
        // #########################################################################################################
        private Forms.UserInputForm.UserInputFormData OpenUIForm(UserInputFormData formData)
        {
            UserInputFormData nullFormData = new UserInputFormData();

            using (var UIForm = new UserInputForm())
            {
                UIForm.Name = formData.mFormName;
                UIForm.Text = formData.mFormText;
                UIForm.Label_EntryText.Text = formData.mFormEntryText;
                UIForm.Label_FileName.Text = formData.mFileName;

                UIForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                UIForm.StartPosition = FormStartPosition.CenterScreen;


                var result = UIForm.ShowDialog();
                UIForm.TextBox_Text.Focus();

                if (result == DialogResult.OK)
                {
                    formData.mGlobalCheckBox = UIForm.CheckBox_Global.Checked;
                    return UIForm.mData;
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            nullFormData.mTextBox = string.Empty;
            nullFormData.mGlobalCheckBox = false;

            return nullFormData;
        }
    }
}
