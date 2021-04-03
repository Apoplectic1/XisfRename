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
        private UserInputForm UIForm;


        public KeywordLists()
        {
            KeywordList = new List<Keyword>();
            UIForm = new Forms.UserInputForm();
            UIForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            UIForm.StartPosition = FormStartPosition.CenterScreen;
            UIForm.TextBox_Text.Focus();
            UIForm.FormClosed += new FormClosedEventHandler(UserInputForm_FormClosed);
            UIForm.DataAvailable += new EventHandler(UserInputForm_DataAvailable);
        }


        public void UserInputForm_DataAvailable(object sender, EventArgs e)
        {
            UserInputForm UIForm = sender as UserInputForm;
            if (UIForm != null)
            {
                string KeywordName = UIForm.Name;
                string KeywordValue = UIForm.TextBox_Text.Text;

                if (KeywordName == "Camera")
                {
                    AddKeyword("Camera", KeywordValue);

                    AddKeyword("NAXIS", 2);
                    AddKeyword("BITPIX", 16);

                    if (KeywordValue == "Z533")
                    {
                        AddKeyword("NAXIS1", 3008);
                        AddKeyword("NAXIS2", 3008);
                    }

                    if (KeywordValue == "Z183")
                    {
                        AddKeyword("NAXIS1", 5496);
                        AddKeyword("NAXIS2", 3672);
                    }

                    if (KeywordValue == "Q178")
                    {
                        AddKeyword("NAXIS1", 3072);
                        AddKeyword("NAXIS2", 2048);
                    }

                    if (KeywordValue == "A144")
                    {
                        AddKeyword("NAXIS1", 1392);
                        AddKeyword("NAXIS2", 1040);
                    }
                }

                if (KeywordName == "Gain")
                {
                    AddKeyword("GAIN", Convert.ToInt32(KeywordValue));
                }

                if (KeywordName == "Offset")
                {
                    AddKeyword("OFFSET", Convert.ToInt32(KeywordValue));
                }

                if (KeywordName == "Exposure")
                {
                    AddKeyword("EXPTIME", Convert.ToDouble(KeywordValue));
                }


                if (KeywordName == "SensorTemp")
                {
                    AddKeyword("CCD-TEMP", Convert.ToDouble(KeywordValue));
                }

                if (KeywordName == "Software")
                {
                    if (KeywordValue == "SGP")
                    {
                        AddKeyword("CREATOR", "Sequence Generator Pro");
                    }

                    if (KeywordValue == "VGR")
                    {
                        AddKeyword("CREATOR", "VOYAGER");
                    }

                    if (KeywordValue == "TSX")
                    {
                        AddKeyword("CREATOR", "TheSkyX");
                    }

                    if (KeywordValue == "SCP")
                    {
                        AddKeyword("CREATOR", "SharpCap Pro");
                    }
                }

                UIForm.TextBox_Text.Text = string.Empty;
                UIForm.TextBox_Text.Focus();
            }
        }

        public void UserInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UIForm.Close();
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
            if (Camera().Equals("Z183")) return;
            if (Camera().Equals("Z533")) return;
            if (Camera().Equals("Q178")) return;
            if (Camera().Equals("A144")) return;

            string camera = string.Empty;

            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "INSTRUME");
            value = node.Value;


            double? gain = Gain();
            if (!gain.HasValue) return;

            if (camera == string.Empty) return;

            double egain = -1.0;

            if (camera == "Z183")
            {
                egain = 3.6059 * Math.Exp(-0.011 * gain.Value);
            }
            else
            {
                if (Camera() == "Q178")
                {
                    if (gain < 4.0)
                    {
                        egain = 2.6;
                    }
                    else
                    {
                        egain = 3.8018 * Math.Exp(-0.0117 * gain.Value);
                    }
                }
            }

            if (egain == -1.0) return;

            AddKeyword("EGAIN", egain, "XSIF File Manager - Calculated electrons per ADU");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // Various programs appear to screw this up - fix it
        public void RepairEGain()
        {
            double? gain = Gain();
            if (!gain.HasValue) return;

            string camera = Camera();
            if (camera == string.Empty) return;

            double egain = -1.0;

            if (camera == "Z183")
            {
                egain = 3.6059 * Math.Exp(-0.011 * gain.Value);
            }
            else
            {
                if (Camera() == "Q178")
                {
                    if (gain < 4.0)
                    {
                        egain = 2.6;
                    }
                    else
                    {
                        egain = 3.8018 * Math.Exp(-0.0117 * gain.Value);
                    }
                }
            }

            if (egain == -1.0) return;

            AddKeyword("EGAIN", egain, "XSIF File Manager - Calculated electrons per ADU");
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
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "XBINNING");
            value = node.Value;
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Camera()
        {
            Keyword naxis1 = new Keyword();
            Keyword naxis2 = new Keyword();

            naxis1.Type = Keyword.EType.INTEGER;
            naxis2.Type = Keyword.EType.INTEGER;

            naxis1 = KeywordList.Find(i => i.Name == "NAXIS1");
            naxis2 = KeywordList.Find(i => i.Name == "NAXIS2");


            if ((naxis1 == null) || (naxis2 == null))
            {
                UIForm.Name = "Camera";
                UIForm.Text = "Unknown Camera";
                UIForm.Label_Text.Text = "Enter Camera: Z183, Z533, Q178 or A144";

                UIForm.ShowDialog();

                // Show testDialog as a modal dialog and determine if DialogResult = OK.
                if (UIForm.DialogResult != DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }

            naxis1 = KeywordList.Find(i => i.Name == "NAXIS1");
            naxis2 = KeywordList.Find(i => i.Name == "NAXIS2");

            if (naxis1.Value.Equals("5496") && naxis2.Value.Equals("3672")) return "Z183";
            if (naxis1.Value.Equals("3008") && naxis2.Value.Equals("3008")) return "Z533";
            if (naxis1.Value.Equals("3072") && naxis2.Value.Equals("2048")) return "Q178";
            if (naxis1.Value.Equals("1392") && naxis2.Value.Equals("1040")) return "A144";

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
        public string CaptureSoftware()
        {
            string value = string.Empty;
            Keyword node = new Keyword();


            node = KeywordList.Find(i => i.Name == "CREATOR");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "NOTES");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "SWCREATE");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "TELESCOP");

            if (node == null)
            {
                UIForm.Name = "Software";
                UIForm.Text = "Unknown Capture Software";
                UIForm.Label_Text.Text = "Enter Capture Software: SGP, TSX, VGR or SCP";

                UIForm.ShowDialog();

                // Show testDialog as a modal dialog and determine if DialogResult = OK.
                if (UIForm.DialogResult != DialogResult.OK)
                {
                    Environment.Exit(0);
                }

                node = KeywordList.Find(i => i.Name == "CREATOR");
            }

            value = node.Value;
            node.Type = Keyword.EType.STRING;

            if (value.Contains("Sequence"))
            {
                return "SGP";
            }

            if (value.Contains("VOYAGER"))
            {
                return "VGR";
            }

            if (value.Contains("SkyX") || value.Contains("TheSky"))
            {
                return "TSX";
            }

            if (value.Contains("SharpCap"))
            {
                return "SCP";
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ExposureSeconds()
        {
            double exposureSeconds;

            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "EXPOSURE");

            if (node == null)
            {
                node = KeywordList.Find(i => i.Name == "EXPTIME");
            }

            if (node == null)
            {
                UIForm.Name = "Exposure";
                UIForm.Text = "Exposure Time Not Set";
                UIForm.Label_Text.Text = "Enter Exposure Time";

                UIForm.ShowDialog();

                // Show testDialog as a modal dialog and determine if DialogResult = OK.
                if (UIForm.DialogResult != DialogResult.OK)
                {
                    Environment.Exit(0);
                }

                node = KeywordList.Find(i => i.Name == "EXPTIME");
            }

            value = node.Value.Replace("'", "").Replace(" ", "");
            node.Type = Keyword.EType.FLOAT;

            exposureSeconds = (double)Convert.ChangeType(node.GetValue(), typeof(double));

            if (exposureSeconds < 10.0)
            {
                return exposureSeconds.ToString("0.0000");
            }
            else
            {
                return exposureSeconds.ToString("0000");
            }
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
                    AddKeyword("FILTER", "Shutter");
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

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocalLength()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "FOCALLEN");

            if (node == null)
            {
                AddKeyword("FOCALLEN", 525);
                node = KeywordList.Find(i => i.Name == "FOCALLEN");
                node.Type = Keyword.EType.INTEGER;
            }

            value = node.Value;

            if (value == "525")
            {
                return "A107R@525";
            }

            if (value == "700")
            {
                return "A107@700";
            }

            return value;
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
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "IMAGETYP");
            node.Type = Keyword.EType.STRING;
            value = node.Value;


            if (value.IndexOf("Light", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "L";
            }

            if (value.IndexOf("Flat", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "F";
            }

            if (value.IndexOf("Dark", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "D";
            }

            if (value.IndexOf("Bias", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "B";
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int? Gain()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "GAIN");

            if (node == null)
            {
                node = KeywordList.Find(i => i.Name == "GAINRAW");

                if (node != null)
                {
                    RemoveKeyword("GAINRAW");
                    AddKeyword("GAIN", node.Value);
                }
            }

            if (node == null)
            {
                UIForm.Name = "Gain";
                UIForm.Text = "Camera Gain Not Set";
                UIForm.Label_Text.Text = "Enter Gain";

                UIForm.ShowDialog();

                // Show testDialog as a modal dialog and determine if DialogResult = OK.
                if (UIForm.DialogResult != DialogResult.OK)
                {
                    Environment.Exit(0);
                }

                node = KeywordList.Find(i => i.Name == "GAIN");
            }

            value = node.Value.Replace("'", "").Replace(".", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
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
            value = node.Value.Replace("'", "").Replace(".", "");
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
            if (node == null)
            {
                AddKeyword("OFFSET", 10);
            }

            node = KeywordList.Find(i => i.Name == "OFFSET");

            value = node.Value.Replace("'", "").Replace(".", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
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
            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.STRING;

            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SensorTemperature()
        {
            string value = string.Empty;
            Keyword node = new Keyword();

            node = KeywordList.Find(i => i.Name == "CCD-TEMP");


            if (node == null)
            {
                UIForm.Name = "SensorTemp";
                UIForm.Text = "CCD Temperature Not Set";
                UIForm.Label_Text.Text = "Enter Sensor Temperature";

                UIForm.ShowDialog();

                // Show testDialog as a modal dialog and determine if DialogResult = OK.
                if (UIForm.DialogResult != DialogResult.OK)
                {
                    Environment.Exit(0);
                }

                node = KeywordList.Find(i => i.Name == "CCD-TEMP");
            }

            node.Type = Keyword.EType.FLOAT;
            value = node.Value;

            return FormatTemperatureString(value);
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
            temperature = Convert.ToDouble(temperatureString);
            temperature = Math.Round(temperature, 1);

            string fmt = "{00:+00.0;-00.0;+00.0}";
            string value = string.Format(fmt, temperature);

            return value;
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
