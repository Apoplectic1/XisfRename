using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using XisfFileManager.Forms;

namespace XisfFileManager.Keywords
{
    public class KeywordLists
    {
        public List<Keyword> KeywordList;
        enum rejectionType {NULL, LINEAR, STUDENT, WINDSOR, SIGMA }

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
        public void RepairCamera()
        {
            double egain = -1.0;
            double gain;
            string camera;

            camera = Camera();
            SensorTemperature();
            ExposureSeconds();
            gain = Gain();
            Offset();

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
                        
                    if (node.Comment.Contains("student"))
                        rejectionValue = rejectionType.STUDENT;

                    if (node.Comment.Contains("sigma"))
                        rejectionValue = rejectionType.SIGMA;

                    if (node.Comment.Contains("windsor"))
                        rejectionValue = rejectionType.WINDSOR;
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
                case rejectionType.WINDSOR:
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

            return node.Value;
        }

        public string Rejection()
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "Rejection");

            if (node == null)
                return string.Empty;

            return node.Value;
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
        public int Binning()
        {
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "XBINNING");
            if (node != null)
            {
                return Convert.ToInt32(node.Value);
            }

            string FormValue = OpenUIForm("Camera Binning", "Camera Binning Not Set", "Enter Bins: 1, 2, 3 or 4");
            int bin = Convert.ToInt32(FormValue);

            AddKeyword("XBINNING", bin);
            AddKeyword("YBINNING", bin);

            return bin;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Camera()
        {
            string FormValue;

            RemoveKeyword("Camera"); // Legacy - not used

            Keyword instrument = new Keyword();
            instrument = KeywordList.Find(i => i.Name == "INSTRUME");

            if (instrument != null)
            {
                // Return if known
                if (instrument.Value.Equals("Z183")) return "Z183";
                if (instrument.Value.Equals("Z533")) return "Z533";
                if (instrument.Value.Equals("Q178")) return "Q178";
                if (instrument.Value.Equals("A144")) return "A144";

                // No Camera found - First try to determine from image size

                Keyword naxis1 = new Keyword();
                naxis1 = KeywordList.Find(i => i.Name == "NAXIS1");

                if (naxis1 != null)
                {
                    naxis1.Value = naxis1.Value.Replace("'", "").Replace(" ", "");

                    if (naxis1.Value.Equals("5496"))
                    {
                        AddKeyword("INSTRUME", "Z183", instrument.Value);
                        return "Z183";
                    }

                    if (naxis1.Value.Equals("3008"))
                    {
                        AddKeyword("INSTRUME", "Z533", instrument.Value);
                        return "Z533";
                    }

                    if (naxis1.Value.Equals("3072"))
                    {
                        AddKeyword("INSTRUME", "Q178", instrument.Value);
                        return "Q178";
                    }

                    if (naxis1.Value.Equals("1392"))
                    {
                        AddKeyword("INSTRUME", "A144", instrument.Value);
                        return "A144";
                    }
                }
            }

            // Did not find any of my cameras; add one

            while (true)
            {
                FormValue = OpenUIForm("Camera", "Camera Not Set", "Enter Z183, Z533, Q178 or A144");
                AddKeyword("INSTRUME", FormValue, "XISF File Manager");

                if (FormValue.Equals("Z183"))
                {
                    AddKeyword("INSTRUME", "Z183", "XISF File Manager");
                    AddKeyword("NAXIS", 2, "XISF File Manager");
                    AddKeyword("NAXIS1", 5496, "XISF File Manager");
                    AddKeyword("NAXIS2", 3672, "XISF File Manager");
                    AddKeyword("XPIXSZ", 2.4, "XISF File Manager");
                    AddKeyword("YPIXSZ", 2.4, "XISF File Manager");
                    RemoveKeyword("NAXIS3");
                    return "Z183";
                }

                if (FormValue.Equals("Z533"))
                {
                    AddKeyword("INSTRUME", "Z533", "XISF File Manager");
                    AddKeyword("NAXIS", 2, "XISF File Manager");
                    AddKeyword("NAXIS1", 3008, "XISF File Manager");
                    AddKeyword("NAXIS2", 3008, "XISF File Manager");
                    AddKeyword("XPIXSZ", 3.76, "XISF File Manager");
                    AddKeyword("YPIXSZ", 3.76, "XISF File Manager");
                    RemoveKeyword("NAXIS3");
                    return "Z533";
                }

                if (FormValue.Equals("Q178"))
                {
                    AddKeyword("INSTRUME", "Q178", "XISF File Manager");
                    AddKeyword("NAXIS", 2, "XISF File Manager");
                    AddKeyword("NAXIS1", 3072, "XISF File Manager");
                    AddKeyword("NAXIS2", 2048, "XISF File Manager");
                    AddKeyword("XPIXSZ", 2.4, "XISF File Manager");
                    AddKeyword("YPIXSZ", 2.4, "XISF File Manager");
                    RemoveKeyword("NAXIS3");
                    return "Q178";
                }

                if (FormValue.Equals("A144"))
                {
                    AddKeyword("INSTRUME", "A144", "XISF File Manager");
                    AddKeyword("NAXIS", 2, "XISF File Manager");
                    AddKeyword("NAXIS1", 1392, "XISF File Manager");
                    AddKeyword("NAXIS2", 1040, "XISF File Manager");
                    AddKeyword("XPIXSZ", 6.45, "XISF File Manager");
                    AddKeyword("YPIXSZ", 6.45, "XISF File Manager");
                    RemoveKeyword("NAXIS3");
                    return "A144";
                }
            }
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
        public string CaptureSoftware()
        {
            Keyword node = new Keyword();
            node = KeywordList.Find(i => i.Name == "CREATOR");

            if (node != null)
            {
                if (node.Value.Contains("Sequence") || node.Value.Contains("SGP"))
                {
                    return "SGP";
                }

                if (node.Value.Contains("VOYAGER") || node.Value.Contains("VOY"))
                {
                    return "VOY";
                }

                if (node.Value.Contains("Sky") || node.Value.Contains("TSX"))
                {
                    return "TSX";
                }

                if (node.Value.Contains("Sharp") || node.Value.Contains("SCP"))
                {
                    return "SCP";
                }
            }

            while (true)
            {
                string FormValue = OpenUIForm("Capture Software", "Capture Software Not Set", "Enter SGP, TSX, VOY or SCP");

                if (FormValue.Equals("SGP") || FormValue.Equals("TSX") || FormValue.Equals("VOY") || FormValue.Equals("SCP"))
                {
                    AddKeyword("CREATOR", FormValue, "XISF File Manager");
                    return FormValue;
                }
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ExposureSeconds()
        {
            double Seconds;
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

            string FormValue = OpenUIForm("Exposure Time", "Exposure Time Not Set", "Enter Exposure Time (double): ");
            Seconds = Convert.ToDouble(FormValue);

            AddKeyword("EXPTIME", Seconds);

            return FormatExposureSeconds(Seconds);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FilterName()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FILTER");

            if (node == null)
            {
                if (FrameType() == "D")
                {
                    AddKeyword("FILTER", "Shutter", "XISF File Manager");
                    return "Shutter";
                }
            }

            node.Type = Keyword.EType.STRING;
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

            AddKeyword("FILTER", value, "XISF File Manager");

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int FocalLength()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FOCALLEN");
            if (node != null)
            {
                return (Convert.ToInt32(node.Value));
            }

            string FormValue = OpenUIForm("Focal Length", "Focal Length Not Set", "Enter mm: ");
            int focalLength = Convert.ToInt32(FormValue);

            AddKeyword("FOCALLEN", focalLength, "XISF File Manager");
            return focalLength;
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
        public string FrameType()
        {
            string value;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "IMAGETYP");

            value = node.Value.Replace("'", "").Replace(" ", "");

            if (node != null)
            {
                if (value.ToUpper().Equals("LIGHT"))
                {
                    AddKeyword("IMAGETYP", "Light");
                    return "L";
                }

                if (value.ToUpper().Equals("DARK"))
                {
                    AddKeyword("IMAGETYP", "Dark");
                    return "D";
                }

                if (value.ToUpper().Equals("FLAT"))
                {
                    AddKeyword("IMAGETYP", "Flat");
                    return "F";
                }

                if (value.ToUpper().Equals("BIAS"))
                {
                    AddKeyword("IMAGETYP", "Bias");
                    return "B";
                }

                if (value.ToUpper().Contains("MASTER"))
                {
                    if (value.ToUpper().Contains("DARK"))
                    {
                        AddKeyword("IMAGETYP", "MasterDark");
                        return "MD";
                    }

                    if (value.ToUpper().Contains("FLAT"))
                    {
                        AddKeyword("IMAGETYP", "MasterFlat");
                        return "MF";
                    }

                    if (value.ToUpper().Contains("BIAS"))
                    {
                        AddKeyword("IMAGETYP", "MasterBias");
                        return "MB";
                    }
                }
            }

            string FormValue = OpenUIForm("Frame Type", "Frame Type Not Set", "Enter Frame Type (Light, Dark, Flat, Bias, MasterDark, MasterFlat, MasterBias): ");

            if (FormValue.ToUpper().Equals("LIGHT"))
            {
                AddKeyword("IMAGETYP", "Light");
                return "L";
            }

            if (FormValue.ToUpper().Equals("DARK"))
            {
                AddKeyword("IMAGETYP", "Dark");
                return "D";
            }

            if (FormValue.ToUpper().Equals("FLAT"))
            {
                AddKeyword("IMAGETYP", "Flat");
                return "F";
            }

            if (FormValue.ToUpper().Equals("BIAS"))
            {
                AddKeyword("IMAGETYP", "Bias");
                return "B";
            }

            if (FormValue.ToUpper().Contains("MASTER"))
            {
                if (FormValue.ToUpper().Contains("DARK"))
                {
                    AddKeyword("IMAGETYP", "MasterDark");
                    return "MD";
                }


                if (FormValue.ToUpper().Contains("FLAT"))
                {
                    AddKeyword("IMAGETYP", "MasterFlat");
                    return "MF";
                }

                if (FormValue.ToUpper().Contains("BIAS"))
                {
                    AddKeyword("IMAGETYP", "MasterBias");
                    return "MB";
                }
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Gain()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "GAIN");
            if (node != null)
            {
                value = node.Value.Replace("'", "").Replace(".", "").Replace(" ", "");
                return Convert.ToInt32(value);
            }

            node = KeywordList.Find(i => i.Name == "GAINRAW");

            if (node != null)
            {
                RemoveKeyword("GAINRAW");
                value = node.Value.Replace("'", "").Replace(".", "").Replace(" ", "");
                AddKeyword("GAIN", Convert.ToInt32(value));
                return Convert.ToInt32(value);
            }

            string FormValue = OpenUIForm("Camera Gain", "Camera Gain Not Set", "Enter Camera Gain: ");
            AddKeyword("GAIN", Convert.ToInt32(FormValue));

            Offset();

            return Convert.ToInt32(FormValue);
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
        public int Offset()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "OFFSET");
            if (node != null)
            {
                value = node.Value.Replace("'", "").Replace(".", "").Replace(" ", "");
                return Convert.ToInt32(value);
            }

            string FormValue = OpenUIForm("Camera Offset", "Camera Offset Not Set", "Enter Camera Offset: ");
            AddKeyword("OFFSET", Convert.ToInt32(FormValue));

            return Convert.ToInt32(FormValue);
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

            string FormValue = OpenUIForm("Camera Pixel Size", "Camera Pixel Size Not Set (Assumes Square Pixels)", "Enter Camera Pixel Size (2.4, 3.76, 6.45): ");
            AddKeyword("XPIXSZ", Convert.ToDouble(FormValue));
            AddKeyword("YPIXSZ", Convert.ToDouble(FormValue));

            return Convert.ToDouble(FormValue);
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

            string FormValue = OpenUIForm("Camera Temperature", "Camera Temperature Setpoint Not Set", "Enter Camera Temperature Setpoint: ");
            AddKeyword("SET-TEMP", FormValue);

            return FormatTemperatureString(FormValue);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SensorTemperature()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "CCD-TEMP");
            if (node != null)
            {
                return FormatTemperatureString(node.Value);
            }

            string FormValue = OpenUIForm("Camera Sensor Temperature", "Camera Sensor Temperature Not Set", "Enter Actual Sensor Temperature: ");
            AddKeyword("CCD-TEMP", FormValue);

            return FormatTemperatureString(FormValue);
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
        public string TargetName()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "OBJECT");
            if (node == null)
            {
                AddKeyword("OBJECT", "NoObject");
                node = KeywordList.Find(i => i.Name == "OBJECT");
                return "NoObject";
            }

            value = node.Value.Replace("'", "").Replace(" ", "").Replace("/", "");
            node.Type = Keyword.EType.STRING;

            return value;
        }

        // #########################################################################################################
        // #########################################################################################################
        public string Telescope()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "TELESCOP");
            if (node != null)
            {
                node.Value = node.Value.Replace(".", "").Replace("'", "");

                if (node.Value.Contains("APM107"))
                {
                    return node.Value;
                }

                if (FocalLength() < 700)
                {
                    AddKeyword("TELESCOP", "APM107R", node.Value);
                    return "APM107R";
                }
                else
                {
                    AddKeyword("TELESCOP", "APM107", node.Value);
                    return "APM107";
                }

            }

            AddKeyword("TELESCOP", "APM107R");

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
        private string FormatExposureSeconds(double seconds)
        {
            if (seconds < 10.0)
            {
                if (seconds < 0.0001)
                {
                    return "0.0000";
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
        private string OpenUIForm(string FormName, string FormText, string LabelText)
        {
            using (var UIForm = new UserInputForm())
            {
                UIForm.Name = FormName;
                UIForm.Text = FormText;
                UIForm.Label_Text.Text = LabelText;
                UIForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                UIForm.StartPosition = FormStartPosition.CenterScreen;
                UIForm.TextBox_Text.Focus();

                var result = UIForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return UIForm.FormReturnValue;
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            return string.Empty;
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
