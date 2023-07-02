using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using XisfFileManager.Forms.UserInputForm;

namespace XisfFileManager
{
    public class FitsKeyword
    {
        public string Name { get; set; } = string.Empty;
        public object Value { get; set; } = null;
        public string Comment { get; set; } = string.Empty;
    }

    public class CalibrationTypeKeyword
    {
        public string CDARK { get; set; }
        public string CFLAT { get; set; }
        public string CBIAS { get; set; }

        public List<PanelKeyword> CPanel { get; set; }
        }

    public class PanelKeyword
    {
        public List<string> Panel { get; set; }

        public PanelKeyword()
        {
            Panel = new List<string>();
        }
    }

    public class KeywordLists
    {
        private static Regex onlyNumerics = new Regex(@"^[\+\-]?\d*\.?[Ee]?[\+\-]?\d*$", RegexOptions.Compiled);
        public List<FitsKeyword> KeywordList;

        public KeywordLists()
        {
            KeywordList = new List<FitsKeyword>();
        }

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

        // ----------------------------------------------------------------------------------------------------------

        private FitsKeyword NewKeyWord(string sName, object oValue, string sComment)
        {
            FitsKeyword newKeyword = new FitsKeyword
            {
                Name = sName,
                Value = oValue,
                Comment = sComment
            };

            return newKeyword;
        }

        // ----------------------------------------------------------------------------------------------------------

        public object GetKeywordValue(string sName)
        {
            FitsKeyword node = KeywordList.Find(i => i.Name == sName);
            if (node == null)
                return null;

            return node.Value;
        }
        public object GetKeywordComment(string sName)
        {
            FitsKeyword node = KeywordList.Find(i => i.Name == sName);
            if (node == null)
                return null;

            return node.Comment;
        }

        // ----------------------------------------------------------------------------------------------------------

        public void RemoveKeyword(string name)
        {
            KeywordList.RemoveAll(i => i.Name.Contains(name));
        }

        // ----------------------------------------------------------------------------------------------------------

        public void AddKeyword(string sName, object value, string sComment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == sName);

            FitsKeyword newKeyword = NewKeyWord(sName, value, sComment);

            KeywordList.Add(newKeyword);
        }

        // ----------------------------------------------------------------------------------------------------------

        public void AddXMLKeyword(XElement element)
        {
            bool bStatus;

            // First remove Keyword characteritics that interfere with later processing
            // Get rid of extra spaces and "'"
            string elementValue = element.Attribute("value").Value;
            elementValue = elementValue.Replace(" ", "").Replace("'", "");

            // Now get rid of an extra decimal point at the end of what should be integers
            elementValue = elementValue.TrimEnd('.');

            //int decimalIndex = elementValue.LastIndexOf('.') + 1;
            //if ((decimalIndex == elementValue.Length) && (decimalIndex != 0))
            //{
            //    elementValue = elementValue.Replace(".", "");
            // }

            // Now actually parse the keywords into bools, integers, doubles and finally strings
            bStatus = bool.TryParse(elementValue, out bool bBool);
            if (bStatus)
            {
                if (elementValue == "T")
                {
                    AddKeyword("true", bBool, element.Attribute("comment").Value);
                    return;
                }
                if (elementValue == "F")
                {
                    AddKeyword("false", bBool, element.Attribute("comment").Value);
                    return;
                }
            }

            if (element.Attribute("name").Value == "EXPTIME")
            {
                bStatus = double.TryParse(elementValue, out double seconds);
                if (bStatus)
                {
                    AddKeyword(element.Attribute("name").Value, seconds, element.Attribute("comment").Value);
                    return;
                }
            }

            bStatus = Int32.TryParse(elementValue, out int iInt32);
            if (bStatus)
            {
                AddKeyword(element.Attribute("name").Value, iInt32, element.Attribute("comment").Value);
                return;
            }

            bStatus = double.TryParse(elementValue, out double dDouble);
            if (bStatus)
            {
                AddKeyword(element.Attribute("name").Value, dDouble, element.Attribute("comment").Value);
                return;
            }

            // Pixinsight will add multiple Keywords using the same name
            // Keep string Keyword Duplicates
            AddKeywordKeepDuplicates(element.Attribute("name").Value, elementValue, element.Attribute("comment").Value);
        }

        // ----------------------------------------------------------------------------------------------------------

        public void AddKeywordKeepDuplicates(string sName, object oValue, string sComment = "XISF File Manager")
        {
            FitsKeyword newKeyword = NewKeyWord(sName, oValue, sComment);

            KeywordList.Add(newKeyword);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************

        public string CBIAS()
        {
            object Object = GetKeywordValue("CBIAS");
            if (Object != null)
                return (string)Object;

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CDARK()
        {
            object Object = GetKeywordValue("CDARK");
            if (Object != null)
                return (string)Object;

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CFLAT()
        {
            object Object = GetKeywordValue("CFLAT");
            if (Object != null)
                return (string)Object;

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Gain(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("GAIN");
            if (Object != null)
            {
                return (int)Object;
            }

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
            // Use graphs found on manufacturer website

            double egain = -1.0;
            double gain = Gain();
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

            AddKeyword("EGAIN", egain, "Electrons per ADU calculated using manufacturer graphs");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLatitude()
        {
            string latitude = (string)GetKeywordValue("SITELAT");

            if (latitude == null) return;

            if (latitude.Contains("N"))
            {
                latitude = Regex.Replace(latitude, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLongitude()
        {
            string longitude = (string)GetKeywordValue("SITELONG");

            if (longitude == null) return;

            if (longitude.Contains("W"))
            {
                longitude = Regex.Replace(longitude, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                Regex regReplace = new Regex("'");

                longitude = regReplace.Replace(longitude, "'-", 1);
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void SetIntegrationParamaters()
        {
            List<FitsKeyword> keys = new List<FitsKeyword>(KeywordList);

            foreach (FitsKeyword node in keys)
            {
                if (node.Comment.ToLower().Contains("numberofimages"))
                {
                    var totalFrames = Regex.Match(node.Comment, @"\d+(?!\D*\d)").Value;
                    AddKeyword("TOTALFRAMES", totalFrames, "Number of Integrated SubFrames");
                }

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
            SetIntegrationParamaters();

            object Object = GetKeywordValue("TOTALFRAMES");
            if (Object == null)
            {
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

                    bool bStatus = int.TryParse(returnValue.mTextBox, out int frames);
                    if (bStatus)
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

            return (int)Object;
        }

        public string Rejection(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("REJECTION");
            if (Object != null)
                return (string)Object;


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
        // Find the ambient temerature as reported by a local weather station
        public double AmbientTemperature(bool findMissingKeywords = false)
        {
            object Object;

            Object = GetKeywordValue("AMB-TEMP");
            if (Object != null)
            {
                double ambientTemperture = Convert.ToDouble(Object);
                AddKeyword("AMB-TEMP", ambientTemperture, "Local Temerature from Open Weather API");

                RemoveKeyword("AMBTEMP");
                RemoveKeyword("TEMPERAT");
                RemoveKeyword("AOCAMBT");

                return ambientTemperture;
            }

            Object = GetKeywordValue("AOCAMBT");
            if (Object != null)
            {
                double ambientTemperture = Convert.ToDouble(Object);
                AddKeyword("AMB-TEMP", ambientTemperture, "Local Temerature from Open Weather API");

                RemoveKeyword("AMBTEMP");
                RemoveKeyword("TEMPERAT");
                RemoveKeyword("AOCAMBT");

                return ambientTemperture;
            }

            Object = GetKeywordValue("AMBTEMP");
            if (Object != null)
            {
                double ambientTemperture = Convert.ToDouble(Object);
                AddKeyword("AMB-TEMP", ambientTemperture, "Local Temerature from Open Weather API");

                RemoveKeyword("AMBTEMP");
                RemoveKeyword("TEMPERAT");
                RemoveKeyword("AOCAMBT");

                return ambientTemperture;
            }

            // Did not find the prefered air temeprature keyword so look for other keyword synonyms
            Object = GetKeywordValue("TEMPERAT");
            if (Object != null)
            {
                double ambientTemperture = Convert.ToDouble(Object);
                AddKeyword("AMB-TEMP", ambientTemperture, "Local Temerature from Open Weather API");

                RemoveKeyword("AMBTEMP");
                RemoveKeyword("TEMPERAT");
                RemoveKeyword("AOCAMBT");

                return ambientTemperture;
            }


            // Did not find any air temeprature keywords so ask user to enter one

            while (findMissingKeywords)
            {
                // Loop until user enters a numeric value then return

                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Ambient Temperature",
                    mFormText = "Ambient Temperature Not Set",
                    mFormEntryText = "Enter Ambient Temperature: ",
                    mFileName = FileName()
                };

                UserInputFormData FormValue = OpenUIForm(formData);

                FormValue.mTextBox = FormValue.mTextBox.Trim();

                // Make sure user entered a valid temerature
                if (onlyNumerics.Match(FormValue.mTextBox).Success)
                {
                    AddKeyword("AMB-TEMP", Convert.ToDouble(FormValue.mTextBox), "Local Temerature from Open Weather API");
                    return Convert.ToDouble(FormValue.mTextBox);
                }
            }

            // Did not ask user to enter missing ambient temerature and did not find a valid keyword so default to absolute zero

            AddKeyword("AMB-TEMP", -273.0, "Missing Value");
            return -273.0;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public bool Approved()
        {
            object Object = GetKeywordValue("Approved");
            if (Object != null)
                return (bool)Object;

            AddKeyword("Approved", true);
            return true;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // This is the name of the File itself - Does not contain the path
        public string FileName()
        {
            object Object = GetKeywordValue("FILENAME");
            if (Object != null)
                return (string)GetKeywordComment("FILENAME");

            return string.Empty;
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Binning(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("XBINNING");
            if (Object != null)
                return Convert.ToInt32(Object);

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
            object Object = GetKeywordValue("INSTRUME");
            if (Object != null)
                return (string)Object;

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
            object Object = GetKeywordValue("DATE-LOC");
            if (Object == null)
            {
                // DATE-LOC does not exist
                Object = GetKeywordValue("LOCALTIM");
                if (Object == null)
                {
                    // Local Time does not exist
                    Object = GetKeywordValue("DATE-OBS");
                    if (Object == null)
                    {
                        // No LOC, No OBS and No LOCALTIME - Make a time up
                        Object = "2000-01-01T12:00:00.000";
                    }
                }
            }

            // Build LOC from OBS or made up Object
            DateTime Local = DateTime.Parse(Object.ToString());
            AddKeyword("DATE-LOC", Local.ToString("yyyy-MM-ddTHH:mm:ss.fff"), "Local Time of observation");
            DateTime UTC = DateTime.Parse(Object.ToString()).ToUniversalTime(); //, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture).ToUniversalTime();
            AddKeyword("DATE-OBS", UTC.ToString("yyyy-MM-ddTHH:mm:ss.fff"), "UTC Time of observation");
            RemoveKeyword("LOCALTIM");

            // Keywords are now correct
            // Format returned date and time to yyyy-MM-dd HH:mm:ss.fff

            string localTime = (string)Object;
            localTime = localTime.Replace("'", "").Replace("T", " ");

            bool status;
            DateTime dt;

            if (localTime.Contains("AM"))
            {
                localTime = localTime.Remove(localTime.IndexOf('.') + 4) + " AM";

                status = DateTime.TryParseExact(localTime, "M/d/yyyy hh:mm:ss.fffffff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);

                if (status) return dt;

                status = DateTime.TryParseExact(localTime, "M/d/yyyy hh:mm:ss.fff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                return dt;
            }

            if (localTime.Contains("PM"))
            {
                localTime = localTime.Remove(localTime.IndexOf('.') + 4) + " PM";

                status = DateTime.TryParseExact(localTime, "M/d/yyyy hh:mm:ss.fffffff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                if (status) return dt;

                status = DateTime.TryParseExact(localTime, "M/d/yyyy hh:mm:ss.fff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                if (status) return dt;

                status = DateTime.TryParseExact(localTime, "M/d/yyyyhh:mm:ss.fff tt",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
                return dt;
            }

            status = DateTime.TryParseExact(localTime, "yyyy-MM-dd HH:mm:ss.fffffff",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None, out dt);
            if (status)
            {
                return DateTime.ParseExact(dt.ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            }

            Local = DateTime.Parse(Object.ToString());
            return DateTime.ParseExact(Local.ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CaptureSoftware(bool FindMissingKeywords = false)
        {
            object Object = GetKeywordValue("CREATOR");
            string software = (string)Object;

            if (Object != null)
            {
                if (software.Contains("Sequence"))
                {
                    AddKeyword("CREATOR", "SGP", software);
                    return "SGP";
                }

                if (software.Contains("VOYAGER"))
                {
                    AddKeyword("CREATOR", "VOY", software);
                    return "VOY";
                }

                if (software.Contains("Sky"))
                {
                    AddKeyword("CREATOR", "TSX", software);
                    return "TSX";
                }

                if (software.Contains("N.I.N.A."))
                {
                    AddKeyword("CREATOR", "NINA", software);
                    return "NINA";
                }

                if (software.Contains("Sharp"))
                {
                    AddKeyword("CREATOR", "SCP", software);
                    return "SCP";
                }

                if (software.Equals("SGP") || software.Equals("VOY") || software.Equals("TSX") || software.Equals("SCP") || software.Equals("NINA"))
                    return (software);
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
        public double ExposureTime(bool FindMissingKeywords = false)
        {
            object Object = GetKeywordValue("EXPTIME");
            if (Object != null)
            {
                RemoveKeyword("EXPOSURE");
                return Convert.ToDouble(Object);
            }

            Object = GetKeywordValue("EXPOSURE");
            if (Object != null)
            {
                RemoveKeyword("EXPOSURE");
                AddKeyword("EXPTIME", (double)Object, "Exposure Time in Seconds");
                return Convert.ToDouble(Object);
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

                bool status = double.TryParse(returnData.mTextBox, out double seconds);
                if (status)
                {
                    AddKeyword("EXPTIME", seconds, "Exposure Time in Seconds");
                    return seconds;
                }
            }

            return double.MinValue;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FilterName(bool FindMissingKeywords = false)
        {
            object Object = GetKeywordValue("FILTER");
            string filterName = (string)Object;

            if (Object != null)
            {
                filterName = filterName.Replace("'", "").Replace(" ", "");

                if (filterName.ToUpper().Equals("LUMA")) filterName = "Luma";
                if (filterName.ToUpper().Equals("HA")) filterName = "Ha";
                if (filterName.ToUpper().Equals("O3")) filterName = "O3";
                if (filterName.ToUpper().Equals("S2")) filterName = "S2";
                if (filterName.ToUpper().Equals("RED")) filterName = "Red";
                if (filterName.ToUpper().Equals("GREEN")) filterName = "Green";
                if (filterName.ToUpper().Equals("BLUE")) filterName = "Blue";
                if (filterName.ToUpper().Equals("SHUTTER")) filterName = "Shutter";

                if (filterName == "Luma")
                    AddKeyword("FILTER", "Luma", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (filterName == "Red")
                    AddKeyword("FILTER", "Red", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (filterName == "Green")
                    AddKeyword("FILTER", "Green", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (filterName == "Blue")
                    AddKeyword("FILTER", "Blue", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (filterName == "Ha")
                    AddKeyword("FILTER", "Ha", "Astrodon E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (filterName == "O3")
                    AddKeyword("FILTER", "O3", "Astrodon E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (filterName == "S2")
                    AddKeyword("FILTER", "S2", "Astrodon E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (filterName == "Shutter")
                    AddKeyword("FILTER", "Shutter", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");

                return filterName;
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
            object Object = GetKeywordValue("IMAGETYP");
            string frameType = (string)Object;

            if (Object != null)
            {
                frameType = frameType.Replace("'", "").Replace(" ", "");

                if (frameType.ToLower().Contains("light")) frameType = "Light";
                if (frameType.ToLower().Contains("dark")) frameType = "Dark";
                if (frameType.ToLower().Contains("flat")) frameType = "Flat";
                if (frameType.ToLower().Contains("bias")) frameType = "Bias";

                AddKeyword("IMAGETYP", frameType);

                return frameType;
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
                    if (returnData.mTextBox.Equals("L")) frameType = "Light";
                    if (returnData.mTextBox.Equals("D")) frameType = "Dark";
                    if (returnData.mTextBox.Equals("F")) frameType = "Flat";
                    if (returnData.mTextBox.Equals("B")) frameType = "Bias";

                    AddKeyword("IMAGETYP", frameType, "XISF File Manager");

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
        public double FocalLength(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("FOCALLEN");

            if (Object != null)
            {
                return Convert.ToDouble(Object);
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

                bool status = double.TryParse(formValue.mTextBox, out double focalLength);
                if (status)
                {
                    AddKeyword("FOCALLEN", focalLength, "Focal Length");

                    if (formValue.mGlobalCheckBox)
                        return -focalLength;
                    else
                        return focalLength;
                }
            }

            return -1.0;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int FocuserPosition(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("FOCPOS");
            if (Object != null)
            {
                return (int)Object;
            }

            Object = GetKeywordValue("FOCUSPOS");
            if (Object != null)
            {
                RemoveKeyword("FOCUSPOS");
                AddKeyword("FOCPOS", (int)Object);
                return (int)Object;
            }

            while (findMissingKeywords)
            {
                // Loop until user enters a numeric value then return

                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Focuser Position",
                    mFormText = "Focuser Position Not Set",
                    mFormEntryText = "Enter Focuser Position: ",
                    mFileName = FileName()
                };

                UserInputFormData returnValue = OpenUIForm(formData);

                int value;
                // Make sure user entered a valid temerature
                bool bStatus = Int32.TryParse(returnValue.mTextBox, out value);
                if (bStatus)
                {
                    AddKeyword("FOCPOS", value);
                    return value;
                }
            }

            return -1;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double FocuserTemperature(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("FOCTEMP");
            if (Object != null)
            {
                // Remove any other keyword synonyms                
                RemoveKeyword("FOCUSTEM");

                return Convert.ToDouble(Object);
            }

            // Did not find the prefered focuser temeprature keyword so look for other keyword synonyms

            Object = GetKeywordValue("FOCUSTEM");
            if (Object != null)
            {
                AddKeyword("FOCTEMP", (double)Object);

                // Remove any other keyword synonyms
                RemoveKeyword("FOCUSTEM");

                return Convert.ToDouble(Object);
            }

            // Did not find any air temeprature keywords so ask user to enter one

            while (findMissingKeywords)
            {
                // Loop until user enters a numeric value then return

                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Focuser Temperature",
                    mFormText = "Focuser Temperature Not Set",
                    mFormEntryText = "Enter Focuser Temperature: ",
                    mFileName = FileName()
                };

                UserInputFormData returnValue = OpenUIForm(formData);

                // Make sure user entered a valid temerature
                double value;
                // Make sure user entered a valid temerature
                bool bStatus = double.TryParse(returnValue.mTextBox, out value);
                if (bStatus)
                    AddKeyword("FOCTEMP", value);

                return value;
            }

            // Did not ask user to enter missing focuser temerature and did not find a valid keyword so default to absolute zero
            AddKeyword("FOCTEMP", -273.0);
            return -273.0;
        }


        // *********************************************************************************************************
        // *********************************************************************************************************
        public double RotatorAngle()
        {
            object Object = GetKeywordValue("POSANGLE");
            if (Object == null)
            {
                Object = GetKeywordValue("ROTATANG");
                if (Object != null)
                {
                    RemoveKeyword("ROTATANG");
                    AddKeyword("POSANGLE", (double)Object, "360 Degree Rotator Mechanical Angle");
                }
                else
                    return double.MinValue;
            }

            return Convert.ToDouble(Object);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Offset(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("OFFSET");
            if (Object != null)
                return (int)Object;

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
            object Object = GetKeywordValue("XPIXSZ");
            if (Object != null)
                return Convert.ToDouble(Object);

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
        public double SensorSetPointTemperature(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("SET-TEMP");
            if (Object != null)
                return Convert.ToDouble(Object);

            while (findMissingKeywords)
            {
                UserInputFormData formData = new UserInputFormData
                {
                    mFormName = "Cammera Temperature",
                    mFormText = "Camera Temperature Not Set",
                    mFormEntryText = "Enter Camera Temperature Setpoint:",
                    mFileName = FileName()
                };

                UserInputFormData returnData = OpenUIForm(formData);

                bool status = double.TryParse(returnData.mTextBox, out double temperature);
                if (status)
                {
                    AddKeyword("SET-TEMP", temperature);

                    return temperature;
                }
            }

            return -273.0;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double SensorTemperature(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("CCD-TEMP");
            if (Object != null)
                return Convert.ToDouble(Object);

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

                bool status = double.TryParse(returnData.mTextBox, out double temperature);
                if (status)
                {
                    AddKeyword("CCD-TEMP", temperature);

                    return temperature;
                }
            }

            return -273.0;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SiteName()
        {
            object Object = GetKeywordValue("SITENAME");
            if (Object != (object)null)
                return (string)Object;

            AddKeyword("SITENAME", "Penns Park, PA");
            return "Penns Park, PA";
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double SSWeight()
        {
            object Obj = GetKeywordValue("SSWEIGHT");
            if (Obj == (object)null)
                return (double)0.0;

            return Math.Round(Convert.ToDouble(Obj), 3, MidpointRounding.AwayFromZero);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string TargetName(bool findMissingKeywords = false)
        {
            object Object = GetKeywordValue("OBJECT");
            string targetName = (string)Object;

            if (Object != null)
            {
                // Replace a TargetName containing "Panel" with "P" in prep for the next Regex
                targetName = targetName.Replace("Panel", "P");

                // Replace a TargetName containing one or more letters, numbers, spaces, or a dash followed by "P" and
                // followed by one or more digits at the end of the string with the same string but with a space inserted before the "P".
                // Return original string if replacement fails.
                string pattern = @"([A-Za-z0-9\s-]+)P(\d+)$";
                string replacement = "$1 P$2";
                string newTargetName = Regex.Replace(targetName, pattern, replacement);

                return newTargetName;
            }


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
            object Object = GetKeywordValue("TELESCOP");
            if (Object != null)
                return (string)Object;

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
        public double WeightKeyword(bool findMissingKeywords = false)
        {
            object Obj = GetKeywordValue("SSWEIGHT");
            if (Obj != null)
            {
                return (double)Obj;
            }

            Obj = GetKeywordValue("NWEIGHT");
            if (Obj != null)
            {
                return (double)Obj;
            }

            Obj = GetKeywordValue("W_SNR");
            if (Obj != null)
            {
                return (double)Obj;
            }

            Obj = GetKeywordValue("W_FWHM");
            if (Obj != null)
            {
                return (double)Obj;
            }

            Obj = GetKeywordValue("W_ECC");
            if (Obj != null)
            {
                return (double)Obj;
            }

            Obj = GetKeywordValue("W_PSFSNR");
            if (Obj != null)
            {
                return (double)Obj;
            }

            Obj = GetKeywordValue("W_PSF");
            if (Obj != null)
            {
                return (double)Obj;
            }

            return -1.0;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        
        // #########################################################################################################
        // #########################################################################################################

    }
}
