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
        enum rejectionType { NULL, LINEAR, STUDENT, WINSOR, SIGMA }

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
        // Various programs appear to screw this up - fix it
        public void SetEGain()
        {
            double egain = -1.0;
            double gain = Gain(); ;
            string camera = Camera();

            if (camera == "Z183")
            {
                egain = 3.6059 * Math.Exp(-0.011 * gain);
            }

            if (camera == "Z533")
            {
                egain = (-7e-13 * Math.Pow(gain, 5)) + (1e-9 * Math.Pow(gain, 4)) - (6e-7 * Math.Pow(gain, 3)) + (0.0002 * Math.Pow(gain, 2)) - (0.0356 * gain) + 3.1338;
            }

            if (camera == "Q178")
            {
                if (gain < 4.0)
                {
                    egain = 2.6;
                }
                else
                {
                    egain = 3.8018 * Math.Exp(-0.0117 * gain);
                }
            }

            if (camera == "A144")
            {
                egain = 0.37;
            }

            AddKeyword("EGAIN", egain, "Calculated electrons per ADU");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of TSX did not write OFFSET
        public void RepairOffset()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "OFFSET");
            if (node != null)
            {
                return;
            }

            if (Camera() == "Z183")
            {
                AddKeyword("OFFSET", 10);
                return;
            }

            if (Camera() == "Z533")
            {
                AddKeyword("OFFSET", 50);
                return;
            }

            if (Camera() == "Q178")
            {
                AddKeyword("OFFSET", 10);
                return;
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLatitude()
        {
            string value;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SITELAT");
            if (node == null) return;

            value = node.Value;

            if (value.Contains("N"))
            {
                value = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");
            }

            node.Type = Keyword.EType.STRING;
            node.Value = value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLongitude()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SITELONG");
            if (node == null) return;

            value = node.Value;

            if (value.Contains("W"))
            {
                value = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                Regex regReplace = new Regex("'");

                value = regReplace.Replace(value, "'-", 1);
            }

            node.Type = Keyword.EType.STRING;
            node.Value = value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void RepairTargetName(string targetName)
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "OBJECT");
            node.Type = Keyword.EType.STRING;
            node.Value = targetName;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void RepairTelescope()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "IMAGETYP");
            if (node.Value.Contains("Master")) return;

            Telescope();
            FocalLength();
        }

        // #########################################################################################################
        // #########################################################################################################
        public double AirMass()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "AIRMASS");
            if (node == null)
            {
                AddKeyword("AIRMASS", 0.0);
            }

            node = KeywordList.Find(i => i.Name == "AIRMASS");

            value = node.Value;
            node.Type = Keyword.EType.FLOAT;

            return Convert.ToDouble(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void IntegrationParamaters()
        {
            rejectionType rejectionValue = rejectionType.NULL;
            string value = string.Empty;

            foreach (Keyword node in KeywordList)
            {
                if (node.Comment.Contains("pixelRejection"))
                {
                    if (node.Comment.Contains("linear"))
                        rejectionValue = rejectionType.LINEAR;

                    if (node.Comment.Contains("Student"))
                        rejectionValue = rejectionType.STUDENT;

                    if (node.Comment.Contains("sigma"))
                        rejectionValue = rejectionType.SIGMA;

                    if (node.Comment.Contains("winsor"))
                        rejectionValue = rejectionType.WINSOR;
                }

                if (node.Comment.Contains("numberOfImages"))
                {
                    value = Regex.Match(node.Comment, @"\d+").Value;
                    break;
                }
            }

            switch (rejectionValue)
            {
                case rejectionType.LINEAR:
                    AddKeyword("Rejection", "LFC", "XISF File Manager");
                    break;
                case rejectionType.STUDENT:
                    AddKeyword("Rejection", "ESD", "XISF File Manager");
                    break;
                case rejectionType.SIGMA:
                    AddKeyword("Rejection", "SC", "XISF File Manager");
                    break;
                case rejectionType.WINSOR:
                    AddKeyword("Rejection", "WSC", "XISF File Manager");
                    break;
                default:
                    break;
            }

            if (value != string.Empty)
                AddKeyword("Images", value, "XISF File Manager");
        }

        public string TotalImages()
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "Images");

            if (node == null)
                return string.Empty;

            return Int32.Parse(node.Value).ToString("D3");
        }

        public string Rejection()
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "Rejection");

            if (node == null)
                return string.Empty;

            return node.Value.Replace("'", "");
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public string AmbientTemperature()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "AOCAMBT");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "TEMPERAT");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "AMB-TEMP");


            if (node == null)
            {
                AddKeyword("AOCAMBT", FocuserTemperature());
                node = KeywordList.Find(i => i.Name == "AOCAMBT");
                node.Type = Keyword.EType.FLOAT;
            }

            node.Type = Keyword.EType.FLOAT;
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
        public string FileName()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FILE");
            if (node != null)
            {
                return node.Comment;
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Binning()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "XBINNING");
            if (node != null)
            {
                return Convert.ToInt32(node.Value);
            }

            UserInputFormData formData = new UserInputFormData();
            formData.mFormName = "Cammera Binning";
            formData.mFormText = "Camera Binning Not Set";
            formData.mFormEntryText = "Enter Bins: 1, 2, 3 or 4";
            formData.mFileName = FileName();

            UserInputFormData FormValue = OpenUIForm(formData);
            int bin = Convert.ToInt32(FormValue.mTextBox);

            AddKeyword("XBINNING", bin);
            AddKeyword("YBINNING", bin);

            return bin;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Camera(bool findMissingKeywords = false)
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "INSTRUME");

            if (node != null)
            {
                return node.Value;
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Capture Camera";
                formData.mFormText = "Capture Camera Not Set";
                formData.mFormEntryText = "Enter Z533, Z183, Q178 or A144:";
                formData.mFileName = FileName();

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
            int index;
            bool status;
            //bool utc = false;

            DateTime parsedDateTime;


            node = KeywordList.Find(i => i.Name == "LOCALTIM");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "DATE-LOC");

            if (node == null)
                // DATE-OBS may is in UTC vs local time
                node = KeywordList.Find(i => i.Name == "DATE-OBS");

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.STRING;

            if (value.Contains("AM"))
            {
                value = value.Remove(value.IndexOf('.') + 4) + " AM";

                DateTime dt;
                status = DateTime.TryParseExact(value, "M/d/yyyy hh:mm:ss.fff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                return dt;
            }

            if (value.Contains("PM"))
            {
                value = value.Remove(value.IndexOf('.') + 4) + " PM";

                DateTime dt;
                status = DateTime.TryParseExact(value, "M/d/yyyy hh:mm:ss.fff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                return dt;
            }


            if ((index = value.IndexOf(".")) > 0)
                value = value.Replace("T", "  ").Replace("'", "").Remove(value.IndexOf('.')).Replace(".", "");
            else
                value = value.Replace("T", "  ").Replace("'", "");

            //value = value.Replace("/", "-");

            status = DateTime.TryParse(value, out parsedDateTime);

            if (status)
            {
                return parsedDateTime;
            }


            return DateTime.ParseExact(value, "yyyy-MM-dd  HH:mm:ss.f", CultureInfo.InvariantCulture);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CaptureSoftware(bool FindMissingKeywords = false)
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "CREATOR");

            if (node != null)
            {
                if (node.Value.Contains("Sequence") || node.Value.Contains("SGP"))
                {
                    AddKeyword("CREATOR", "SGP", node.Value);
                    return "SGP";
                }

                if (node.Value.Contains("VOYAGER") || node.Value.Contains("VOY"))
                {
                    AddKeyword("CREATOR", "VOY", node.Value);
                    return "VOY";
                }

                if (node.Value.Contains("Sky") || node.Value.Contains("TSX"))
                {
                    AddKeyword("CREATOR", "TSX", node.Value);
                    return "TSX";
                }

                if (node.Value.Contains("Sharp") || node.Value.Contains("SCP"))
                {
                    AddKeyword("CREATOR", "SCP", node.Value);
                    return "SCP";
                }
            }

            while (FindMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Capture Software";
                formData.mFormText = "Capture Software Not Set";
                formData.mFormEntryText = "Enter SGP, TSX, VOY or SCP:";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("SGP") || FormValue.mTextBox.Equals("TSX") || FormValue.mTextBox.Equals("VOY") || FormValue.mTextBox.Equals("SCP"))
                {
                    AddKeyword("CREATOR", FormValue.mTextBox, "XISF File Manager");
                    return FormValue.mTextBox;
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
            double seconds;
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
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Exposure Time";
                formData.mFormText = "Exposure Time Not Set";
                formData.mFormEntryText = "Enter Exposure Time in Seconds (double):";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                status = double.TryParse(formData.mTextBox, out seconds);
                if (status)
                {
                    AddKeyword("EXPTIME", seconds, "Camera Exposure Time in Seconds");
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

                if (value.ToLower().Contains("ha")) value = "Ha";
                if (value.ToLower().Contains("oiii")) value = "O3";
                if (value.ToLower().Contains("o3")) value = "O3";
                if (value.ToLower().Contains("sii")) value = "S2";
                if (value.ToLower().Contains("s2")) value = "S2";
                if (value.ToLower().Contains("red")) value = "Red";
                if (value.ToLower().Contains("green")) value = "Green";
                if (value.ToLower().Contains("blue")) value = "Blue";
                if (value.ToLower().Contains("shutter")) value = "Shutter";

                if (value == "Shutter")
                {
                    AddKeyword("FILTER", value, "Dark");
                }
                else
                {
                    AddKeyword("FILTER", value, "Astrodon E-Series");
                }
                return value;
            }

            while (FindMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Filter Type";
                formData.mFormText = "Filter Type Not Set";
                formData.mFormEntryText = "Enter Filter (L, R, G, B, Ha, O3, S2 or S):";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("L") || FormValue.mTextBox.Equals("R") || FormValue.mTextBox.Equals("G") || FormValue.mTextBox.Equals("B") ||
                    FormValue.mTextBox.Equals("Ha") || FormValue.mTextBox.Equals("O3") || FormValue.mTextBox.Equals("S2") || FormValue.mTextBox.Equals("S"))
                {
                    AddKeyword("FILTER", FormValue.mTextBox, "XISF File Manager");
                    return FormValue.mTextBox;
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
                return value;
            }

            while (FindMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Frame Type";
                formData.mFormText = "Frame Type Not Set";
                formData.mFormEntryText = "Enter Frame Type (L, D, F or B):";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("L") || FormValue.mTextBox.Equals("D") || FormValue.mTextBox.Equals("F") || FormValue.mTextBox.Equals("B"))
                {
                    if (FormValue.Equals("L")) value = "Light";
                    if (FormValue.Equals("D")) value = "Dark";
                    if (FormValue.Equals("F")) value = "Flat";
                    if (FormValue.Equals("B")) value = "Bias";

                    AddKeyword("IMAGETYP", FormValue.mTextBox, "XISF File Manager");
                    return FormValue.mTextBox;
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int FocalLength(bool findMissingKeywords = false)
        {
            int focalLength;
            bool status;
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FOCALLEN");
            if (node != null)
            {
                return (Convert.ToInt32(node.Value));
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Focal Length";
                formData.mFormText = "Focal Length Not Set";
                formData.mFormEntryText = "Enter Focal Length in millimeters:";
                formData.mFileName = FileName();

                UserInputFormData formValue = OpenUIForm(formData);

                status = int.TryParse(formValue.mTextBox, out focalLength);
                if (status)
                {
                    AddKeyword("FOCALLEN", focalLength, "Focal Length");
                    return focalLength;
                }
            }

            return -1;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocuserPosition()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FOCPOS");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "FOCUSPOS");

            if (node == null) return string.Empty;

            node.Type = Keyword.EType.INTEGER;

            value = node.Value.Replace(".", "");


            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocuserTemperature()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FOCTEMP");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "FOCUSTEM");

            if (node == null) return string.Empty;

            node.Type = Keyword.EType.FLOAT;
            value = node.Value;

            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Gain(bool findMissingKeywords = false)
        {
            string value = string.Empty;
            Keyword node = new Keyword();
            bool status;
            int gain;

            node = KeywordList.Find(i => i.Name == "GAIN");
            if (node != null)
            {
                value = node.Value.Replace("'", "").Replace(".", "").Replace(" ", "");
                status = Int32.TryParse(value, out gain);
                if (status)
                {
                    return gain;
                }
                else
                {
                    return -1;
                }
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Cammera Gain";
                formData.mFormText = "Camera Gain Not Set";
                formData.mFormEntryText = "Enter Camera Gain: ";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                status = Int32.TryParse(value, out gain);
                if (status)
                {
                    AddKeyword("GAIN", gain, "XISF File Manager");
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
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "POSANGLE");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "ROTATANG");

            if (node == null) return string.Empty;

            value = node.Value;
            node.Type = Keyword.EType.FLOAT;

            return String.Format("{0:000.0}", Convert.ToDouble(value));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Index()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "INDEX");
            value = node.Value.Replace("'", "").Replace(".", "").Replace(" ", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Offset(bool findMissingKeywords = false)
        {
            bool status;
            int offset;
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "OFFSET");
            if (node != null)
            {
                value = node.Value.Replace("'", "").Replace(".", "").Replace(" ", "");
                return Convert.ToInt32(value);
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Camera Offset";
                formData.mFormText = "Camera Offset Not Set";
                formData.mFormEntryText = "Enter Camera Offset:";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                status = Int32.TryParse(value, out offset);
                if (status)
                {
                    AddKeyword("OFFSET", offset);
                    return offset;
                }
            }

            return -1;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double PixelSize()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "XPIXSZ");
            if (node != null)
            {
                return Convert.ToDouble(node.Value);
            }


            UserInputFormData formData = new UserInputFormData();
            formData.mFormName = "Cammera Pixel Size";
            formData.mFormText = "Camera Pixel Size Not Set (Assumes Square Pixels)";
            formData.mFormEntryText = "Enter Camera Pixel Size (2.4, 3.76, 6.45):";
            formData.mFileName = FileName();


            UserInputFormData FormValue = OpenUIForm(formData);
            AddKeyword("XPIXSZ", Convert.ToDouble(FormValue.mTextBox));
            AddKeyword("YPIXSZ", Convert.ToDouble(FormValue.mTextBox));

            return Convert.ToDouble(FormValue.mTextBox);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Profile()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "Profile");
            if (node == null)
            {
                AddKeyword("Profile", "No Profile");
            }

            node = KeywordList.Find(i => i.Name == "Profile");

            node.Type = Keyword.EType.STRING;

            return node.Value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double Scale()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SCALE");
            if (node != null)
            {
                return Convert.ToDouble(node.Value);
            }

            double scale = (PixelSize() * 206.265) / FocalLength();

            AddKeyword("SCALE", scale);

            return scale;
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public int SensorHeight()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "NAXIS2");

            if (node == null)
            {
                if (Camera() == "Z183")
                {
                    if (Binning() == 1)
                    {
                        AddKeyword("NAXIS2", 3672);
                    }

                    if (Binning() == 2)
                    {
                        AddKeyword("NAXIS2", 3672 / 2);
                    }
                }

                if (Camera() == "Z533")
                {
                    if (Binning() == 1)
                    {
                        AddKeyword("NAXIS2", 3008);
                    }

                    if (Binning() == 2)
                    {
                        AddKeyword("NAXIS2", 3008 / 2);
                    }
                }

                if (Camera() == "Q178")
                {
                    if (Binning() == 1)
                    {
                        AddKeyword("NAXIS2", 2048);
                    }

                    if (Binning() == 2)
                    {
                        AddKeyword("NAXIS2", 2048 / 2);
                    }
                }

                if (Camera() == "A144")
                {
                    if (Binning() == 1)
                    {
                        AddKeyword("NAXIS2", 1040);
                    }

                    if (Binning() == 2)
                    {
                        AddKeyword("NAXIS2", 1040 / 2);
                    }
                }

                node = KeywordList.Find(i => i.Name == "NAXIS2");

                if (node == null)
                {
                    return 0;
                }
            }
            value = node.Value.Replace("'", "").Replace(".", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SensorSetPointTemperature()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SET-TEMP");
            if (node != null)
            {
                return FormatTemperatureString(value);
            }

            UserInputFormData formData = new UserInputFormData();
            formData.mFormName = "Cammera Temperature";
            formData.mFormText = "Camera Temperature Not Set";
            formData.mFormEntryText = "Enter Camera Temperature Setpoint:";
            formData.mFileName = FileName();

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
            Keyword node = new Keyword();
            double temperature;

            node = KeywordList.Find(i => i.Name == "CCD-TEMP");
            if (node != null)
            {
                return FormatTemperatureString(node.Value);
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Camera Sensor Temperature";
                formData.mFormText = "Camera Sensor Temperature Not Set";
                formData.mFormEntryText = "Enter Actual Sensor Temperature:";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                status = double.TryParse(value, out temperature);
                if (status)
                {
                    AddKeyword("CCD-TEMP", double.Parse(FormValue.mTextBox));
                    return temperature.ToString();
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int SensorWidth()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "NAXIS1");

            if (node == null)
            {
                if (Camera() == "Z183")
                {
                    AddKeyword("NAXIS1", 5496);
                }

                if (Camera() == "Z533")
                {
                    AddKeyword("NAXIS1", 3008);
                }

                if (Camera() == "Q178")
                {
                    AddKeyword("NAXIS1", 3072);
                }

                if (Camera() == "A144")
                {
                    AddKeyword("NAXIS1", 1392);
                }

                node = KeywordList.Find(i => i.Name == "NAXIS1");

                if (node == null)
                {
                    return 0;
                }
            }

            value = node.Value.Replace("'", "").Replace(".", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SiteLocation()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SITENAME");
            value = node.Value;
            node.Type = Keyword.EType.STRING;

            return value.Replace("'", "");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double SSWeight()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SSWEIGHT");

            if (node == null) return Double.NaN;
            double SSWeight = Convert.ToDouble(node.GetValue());

            if (Double.IsNaN(SSWeight)) return Double.NaN;

            return Convert.ToDouble(Math.Round(Convert.ToDecimal(SSWeight), 1, MidpointRounding.AwayFromZero));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string TargetName(bool findMissingKeywords = false)
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "OBJECT");
            if (node != null)
            {
                value = node.Value.Replace("'", "").Replace(" ", "").Replace("/", "");
                return value;
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Target Name";
                formData.mFormText = "Target Name Not Set";
                formData.mFormEntryText = "Enter Target Name (ID or Master):";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                AddKeyword("OBJECT", formData.mTextBox, "XISF File Manager");
                return formData.mTextBox;
            }

            return string.Empty;
        }

        // #########################################################################################################
        // #########################################################################################################
        public string Telescope(bool findMissingKeywords = false)
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "TELESCOP");
            if (node != null)
            {
                node.Value = node.Value.Replace(".", "").Replace("'", "");
                return node.Value;
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Telescope";
                formData.mFormText = "Telescope Not Set";
                formData.mFormEntryText = "Enter APM(R), EVO(R) or NWT(R):";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                if (FormValue.mTextBox.Equals("APM") || FormValue.mTextBox.Equals("EVO") || FormValue.mTextBox.Equals("NWT"))
                {
                    if (FormValue.mTextBox == "APM")
                    {
                        AddKeyword("TELESCOP", "APM107R", "w/Riccardi 0.75 Reducer");
                    }

                    if (FormValue.mTextBox == "EVO")
                    {
                        AddKeyword("TELESCOP", "EVO150R", "w/Riccardi 0.75 Reducer");
                    }

                    if (FormValue.mTextBox == "NWT")
                    {
                        AddKeyword("TELESCOP", "EVO150R", "w/Riccardi 0.75 Reducer");
                    }
                }
                else
                {
                    AddKeyword("TELESCOP", FormValue.mTextBox, "XISF File Manager");
                }
                return FormValue.mTextBox;
            }


            AddKeyword("TELESCOP", "APM107R", "w/Riccardi 0.75 Reducer");

            return "APM107R";
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double Weight()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "SSWEIGHT");

            if (node == null) return Double.NaN;
            double SSWeight = Convert.ToDouble(node.GetValue());

            if (Double.IsNaN(SSWeight)) return Double.NaN;

            return Convert.ToDouble(Math.Round(Convert.ToDecimal(SSWeight), 0, MidpointRounding.AwayFromZero));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int IntegrationCount(bool findMissingKeywords = false)
        {
            bool status;
            int integrationNumber;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "INTCOUNT");
            if (node != null)
            {
                node.Value = node.Value.Replace(".", "").Replace("'", "");
                return Convert.ToInt32(node.Value);
            }

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData();
                formData.mFormName = "Master Frame Integration Count";
                formData.mFormText = "Integration Count Not Set";
                formData.mFormEntryText = "Enter Number of frames used during Intgration:";
                formData.mFileName = FileName();

                UserInputFormData FormValue = OpenUIForm(formData);

                status = int.TryParse(FormValue.mTextBox, out integrationNumber);

                if (status)
                {
                    AddKeyword("INTCOUNT", integrationNumber, "Number of SubFrames used during Integrartion");
                    return integrationNumber;
                }
            }

            return -1;
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, string value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value;
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.STRING;
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, double value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString("F6");
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.FLOAT;
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, int value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString();
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.INTEGER;
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, bool value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString();
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.BOOL;
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(XElement element)
        {
            Keyword keyword = new Keyword();
            keyword.Name = element.Attribute("name").Value;
            keyword.Value = element.Attribute("value").Value;
            keyword.Comment = element.Attribute("comment").Value;
            keyword.Type = Keyword.EType.COPY;
            KeywordList.Add(keyword);
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
                if (seconds < 0.0001)
                {
                    return "0000";
                }

                return seconds.ToString("0.0000");
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

    public class GoogleTimeZone
    {
        public double dstOffset { get; set; }
        public double rawOffset { get; set; }
        public string status { get; set; }
        public string timeZoneId { get; set; }
        public string timeZoneName { get; set; }
    }
}
