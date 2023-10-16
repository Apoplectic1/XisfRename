﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using XisfFileManager.Forms.UserInputForm;
using static System.Windows.Forms.DataFormats;

using XisfFileManager.Enums;
using System.Diagnostics.Eventing.Reader;

/*
public class Keywords
{
    private static Keywords instance;

    public static Keywords Instance
    {
        get
        {
            if (instance == null)
                instance = new Keywords();
            return instance;
        }
    }

    public string Value1 { get; set; }
    public int Value2 { get; set; }
}

public class ClassA
{
    private Keywords keywords = Keywords.Instance;

    public void MethodA()
    {
        string value1 = keywords.Value1;
        int value2 = keywords.Value2;

        // Modify the values
        keywords.Value1 = "New Value";
        keywords.Value2 = 42;
    }
}

public class ClassB
{
    private Keywords keywords = Keywords.Instance;

    public void MethodB()
    {
        // Access the values
        string value1 = keywords.Value1;
        int value2 = keywords.Value2;
    }
}
*/
namespace XisfFileManager
{
    public class KeywordList
    {
        private Regex onlyNumerics = new Regex(@"^[\+\-]?\d*\.?[Ee]?[\+\-]?\d*$", RegexOptions.Compiled);
        public List<Keyword> mKeywordList;

        public KeywordList()
        {
            mKeywordList = new List<Keyword>();
        }

        public void Clear()
        {
            mKeywordList.Clear();
        }
        // ***********************************************************************************************************************************
        public static Forms.UserInputForm.UserInputFormData OpenUIForm(UserInputFormData formData)
        {
            UserInputFormData nullFormData = new UserInputFormData();

            using (var UIForm = new UserInputForm())
            {
                UIForm.Name = formData.FormName;
                UIForm.Text = formData.FormText;
                UIForm.Label_EntryText.Text = formData.FormEntryText;
                UIForm.Label_FileName.Text = formData.FileName;

                UIForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                UIForm.StartPosition = FormStartPosition.CenterScreen;


                var result = UIForm.ShowDialog();
                _ = UIForm.TextBox_Text.Focus();

                if (result == DialogResult.OK)
                {
                    formData.GlobalCheckBox = UIForm.CheckBox_Global.Checked;
                    return UIForm.mData;
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            nullFormData.TextBox = string.Empty;
            nullFormData.GlobalCheckBox = false;

            return nullFormData;
        }

        // ----------------------------------------------------------------------------------------------------------

        public Keyword NewKeyword(string sName, object oValue, string sComment)
        {
            Keyword newKeyword = new Keyword
            {
                Name = sName,
                Value = oValue,
                Comment = sComment
            };

            return newKeyword;
        }

        // ----------------------------------------------------------------------------------------------------------

        public Keyword GetKeyword(string sName)
        {
            Keyword node = mKeywordList.Find(i => i.Name == sName);
            if (node == null)
                return null;

            return node;
        }

        // ----------------------------------------------------------------------------------------------------------

        public object GetKeywordValue(string sName)
        {
            Keyword node = mKeywordList.Find(i => i.Name == sName);
            if (node == null)
                return null;

            return node.Value;
        }
        public object GetKeywordComment(string sName)
        {
            Keyword node = mKeywordList.Find(i => i.Name == sName);
            if (node == null)
                return null;

            return node.Comment;
        }

        // ----------------------------------------------------------------------------------------------------------

        public void RemoveKeyword(string sName)
        {
            _ = mKeywordList.RemoveAll(i => i.Name.Equals(sName));
        }

        // ----------------------------------------------------------------------------------------------------------
        public void RemoveKeyword(string sName, object oValue)
        {
            _ = mKeywordList.RemoveAll(i => i.Name.Equals(sName) && i.Value.Equals(oValue));
        }

        // ----------------------------------------------------------------------------------------------------------


        public void AddKeyword(string sName, object oValue, string sComment = "XISF File Manager")
        {
            _ = mKeywordList.RemoveAll(i => i.Name == sName);

            Keyword newKeyword = NewKeyword(sName, oValue, sComment);

            mKeywordList.Add(newKeyword);
        }

        // ----------------------------------------------------------------------------------------------------------
        public void AddKeywordKeepDuplicates(Keyword newKeyword)
        {
            mKeywordList.Add(newKeyword);
        }

        // ----------------------------------------------------------------------------------------------------------
        public void AddKeywordKeepDuplicates(string sName, object oValue, string sComment = "XISF File Manager")
        {
            Keyword newKeyword = NewKeyword(sName, oValue, sComment);

            mKeywordList.Add(newKeyword);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double Airmass
        {
            get
            {
                object Object = GetKeywordValue("AIRMASS");
                if (Object != null)
                    return (double)Object;
                return -1;
            }
            set
            {
                AddKeyword("AIRMASS", value, "Number of atmospheres this image is looking through");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // Find the ambient temerature as reported by a local weather station
        public double AmbientTemperature
        {
            get
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

                return -273.0;
            }

            set
            {
                AddKeyword("AMB-TEMP", value, "Local Temerature from Open Weather API");
            }
            // Did not find any air temeprature keywords so ask user to enter one
            /*
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
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public bool Approved
        {
            get
            {
                object Object = GetKeywordValue("Approved");
                if (Object != null)
                    return (bool)Object;
                return true;
            }
            set
            {
                AddKeyword("Approved", value, "This file has been approved for subframe calculations");
            }
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Binning
        {
            get
            {
                object Object = GetKeywordValue("XBINNING");
                if (Object != null)
                    return Convert.ToInt32(Object);
                return -1;
            }
            set
            {
                AddKeyword("XBINNING", value, "Camera Binning Mode 1-4 - Square modes only");
                AddKeyword("YBINNING", value, "Camera Binning Mode 1-4 - Square modes only");
            }
            /*
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
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Camera
        {
            get
            {
                object Object = GetKeywordValue("INSTRUME");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                AddKeyword("INSTRUME", value, "Camera used to take the exposure");
            }
            /* 
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
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public DateTime CaptureDateTime
        {
            get
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

                DateTime Local;
                DateTime UTC;

                if (CaptureSoftware == "TSX")
                {
                    // "7/18/202010:19:28.000PMDST";

                    // Remove the "DST" part from the end of the string
                    string dateString = Object.ToString().Replace("DST", "").Trim();

                    // Find the positions of slashes and the last space
                    int firstSlash = dateString.IndexOf('/');
                    int secondSlash = dateString.IndexOf('/', firstSlash + 1);
                    int dpoint = dateString.LastIndexOf('.');

                    if (firstSlash > 0)
                    {
                        // Extract date components
                        string monthStr = dateString.Substring(0, firstSlash);
                        string dayStr = dateString.Substring(firstSlash + 1, secondSlash - firstSlash - 1);
                        string yearStr = dateString.Substring(secondSlash + 1, 4);

                        // Extract time components
                        string timeStr = dateString.Substring(secondSlash + 5, 12);

                        // Convert PM time to 24-hour format
                        if (dateString.Contains("PM"))
                            timeStr = string.Concat((int.Parse(timeStr.Substring(0, 2)) + 12).ToString(), timeStr.AsSpan(2));

                        // Fix a fix of mine: Hours sometimes has three digits - take the time from the file name
                        timeStr = Regex.Replace(timeStr, @"^\d{3}:", m => string.Concat(m.Value.AsSpan(0, 2), ":"));

                        // Combine date and time parts for parsing
                        string dateTimeStr = $"{monthStr}/{dayStr}/{yearStr} {timeStr}";

                        Local = DateTime.Parse(dateTimeStr);
                    }
                    else
                    {
                        Local = DateTime.Parse(dateString);
                    }

                    AddKeyword("DATE-LOC", Local.ToString("yyyy-MM-ddTHH:mm:ss.fff"), "Local Time of observation");
                    UTC = Local.ToUniversalTime();
                }
                else
                {
                    // if '6/8/2020 01:22:44.119 AM DST'
                    string result = Regex.Replace(Object.ToString(), @"(\.\d+)[^.]*$", "$1");

                    // Build LOC from OBS or made up Object
                    Local = DateTime.Parse(result);
                    AddKeyword("DATE-LOC", Local.ToString("yyyy-MM-ddTHH:mm:ss.fff"), "Local Time of observation");
                    UTC = Local.ToUniversalTime();
                }

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

            set
            {
                AddKeyword("DATE-LOC", value, "Local capture time");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CaptureSoftware
        {
            get
            {
                object Object = GetKeywordValue("SWCREATE");
                string software = (string)Object;

                // Nina 3 Format
                if (Object != null)
                    if (software.Contains("N.I.N.A"))
                        return "NINA";


                Object = GetKeywordValue("CREATOR");
                software = (string)Object;

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
                return string.Empty;
            }
            set
            {
                AddKeyword("CREATOR", value, "Software that captured this image");
            }
            /*
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
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CBIAS
        {
            get
            {
                object Object = GetKeywordValue("CBIAS");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                AddKeyword("CBIAS", value, "WBPP: Match this file with other CBIAS" + CBIAS + " files");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CDARK
        {
            get
            {
                object Object = GetKeywordValue("CDARK");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                AddKeyword("CDARK", value, "WBPP: Match this file with other CDARK" + CDARK + " files");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CFLAT
        {
            get
            {
                object Object = GetKeywordValue("CFLAT");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                AddKeyword("CFLAT", value, "WBPP: Match this file with other CFLAT" + CFLAT + " files");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CLIGHT
        {
            get
            {
                object Object = GetKeywordValue("CLIGHT");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                AddKeyword("CLIGHT", value, "WBPP: Match this file with other CLIGHT" + CLIGHT + " files");
            }
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CPANEL
        {
            get
            {
                object Object = GetKeywordValue("CPANEL");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                AddKeyword("CPANEL", value, "Match this file with other CPANEL" + CPANEL + " files");
            }
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CSTARS
        {
            get
            {
                object Object = GetKeywordValue("CSTARS");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                AddKeyword("CSTARS", value, "Match this file with other CSTARS" + CSTARS + " files");
            }
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public double Eccentricity
        {
            get
            {
                object Object = GetKeywordValue("AIRMASS");
                if (Object != null)
                    return (double)Object;
                return -1;
            }
            set
            {
                AddKeyword("ECCENTRICITY", value, "Number of atmospheres this image is looking through");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double ExposureSeconds
        {
            get
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
                return -1;
            }
            set
            {
                AddKeyword("EXPTIME", value, "Frame Exposure Time in Seconds");
            }
            /*


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
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // This is the name of the File itself - Does not contain the path
        public string FileName
        {
            get
            {
                object Object = GetKeywordValue("FILENAME");
                if (Object != null)
                    return (string)GetKeywordComment("FILENAME");
                return string.Empty;
            }
            set
            {
                AddKeyword("FILENAME", value, "Original Filename");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FilterName
        {
            get
            {
                object Object = GetKeywordValue("FILTER");
                if (Object != null)
                {
                    return (string)Object;
                }

                return string.Empty;
            }

            set
            {
                string filterName = string.Empty;
                if (value.ToUpper().Equals("LUMA")) filterName = "Luma";
                if (value.ToUpper().Equals("HA")) filterName = "Ha";
                if (value.ToUpper().Equals("O3")) filterName = "O3";
                if (value.ToUpper().Equals("S2")) filterName = "S2";
                if (value.ToUpper().Equals("RED")) filterName = "Red";
                if (value.ToUpper().Equals("GREEN")) filterName = "Green";
                if (value.ToUpper().Equals("BLUE")) filterName = "Blue";
                if (value.ToUpper().Equals("SHUTTER")) filterName = "Shutter";

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
            }
        }
        /*

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
        */

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int FocalLength
        {
            get
            {
                object Object = GetKeywordValue("FOCALLEN");
                if (Object != null)
                    return Convert.ToInt32(Object);
                return -1;
            }
            set
            {
                AddKeyword("FOCALLEN", value, Telescope + " Focal length in mm");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int FocuserPosition
        {
            get
            {
                object Object = GetKeywordValue("FOCPOS");
                if (Object != null)
                    return (int)Object;
                return -1;
            }

            set
            {
                AddKeyword("FOCPOS", value, "MoonLite Focuser Posistion");
            }
            /*
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

            }
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double FocuserTemperature
        {
            get
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
                    AddKeyword("FOCTEMP", Convert.ToDouble(Object));

                    // Remove any other keyword synonyms
                    RemoveKeyword("FOCUSTEM");

                    return Convert.ToDouble(Object);
                }

                return -273.0;
            }

            set
            {
                AddKeyword("FOCTEMP", value, "MoonLite NightCrawler Focuser Temperature");
            }
            /*
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
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public eFrame FrameType
        {
            get
            {
                object Object = GetKeywordValue("IMAGETYP");
                if (Object != null)
                {
                    if (Object.ToString().ToLower().Contains("light")) return eFrame.LIGHT;
                    if (Object.ToString().ToLower().Contains("dark")) return eFrame.DARK;
                    if (Object.ToString().ToLower().Contains("flat")) return eFrame.FLAT;
                    if (Object.ToString().ToLower().Contains("bias")) return eFrame.BIAS;
                }
                return eFrame.EMPTY;
            }

            set
            {
                AddKeyword("IMAGETYP", value, "Type of frame capture");
            }
            /*
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
        }
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double FWHM
        {
            get
            {
                object Object = GetKeywordValue("FWHM");
                if (Object != null)
                    return (double)Object;
                return -1;
            }
            set
            {
                AddKeyword("FWHM", value, "Average FWHM in this image");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Gain
        {
            get
            {
                object Object = GetKeywordValue("GAIN");
                if (Object != null)
                    return (int)Object;
                return -1;
            }
            set
            {
                AddKeyword("GAIN", value, "Camera Gain");
                SetEGain();
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Offset
        {
            get
            {
                object Object = GetKeywordValue("OFFSET");
                if (Object != null)
                    return (int)Object;

                return -1;
            }

            set
            {
                if (Camera.Contains('Z'))
                    AddKeyword("OFFSET", value, "Actual Camera Offset is this value times 10");
                else
                    AddKeyword("OFFSET", value, "Camera Offset");
            }

            /*
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
            */
        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        public bool Protect
        {
            get
            {
                object Object = GetKeywordValue("PROTECT");
                if (Object != null)
                    return (bool)Object;

                return false;
            }

            set
            {
                AddKeyword("PROTECT", value, "Xisf File Manager Protected File Status");
            }
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
                FormName = "Cammera Pixel Size",
                FormText = "Camera Pixel Size Not Set (Assumes Square Pixels)",
                FormEntryText = "Enter Camera Pixel Size (2.4, 3.76, 6.45):",
                FileName = FileName
            };


            UserInputFormData FormValue = OpenUIForm(formData);
            AddKeyword("XPIXSZ", Convert.ToDouble(FormValue.TextBox));
            AddKeyword("YPIXSZ", Convert.ToDouble(FormValue.TextBox));

            return Convert.ToDouble(FormValue.TextBox);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Rejection
        {
            get
            {
                // Temp
                Keyword keyWord = GetKeyword("REJECTION");
                if (keyWord != null)
                {
                    AddKeyword("RJCT-ALG", keyWord.Value, keyWord.Comment);
                    RemoveKeyword("REJECTION");
                    RemoveKeyword("REJECTIO");
                }
                // Temp

                object Object = GetKeywordValue("RJCT-ALG");
                if (Object != null)
                    return (string)Object;
                return string.Empty;
            }
            set
            {
                SetIntegrationParamaters();
                //AddKeyword("RJCT-ALG", value, "Rejection Integration Method");
            }

            /*
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
                     AddKeyword("RJCT-ALG", FormValue.mTextBox, "PixInsight Pixel Integration Rejection Method");
                     return FormValue.mTextBox;
                 }
             }

             return string.Empty;
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLatitude()
        {
            string latitude = (string)GetKeywordValue("SITELAT");

            if (latitude == null) return;

            if (latitude.Contains('N'))
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

            if (longitude.Contains('W'))
            {
                longitude = Regex.Replace(longitude, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                Regex regReplace = new Regex("'");

                longitude = regReplace.Replace(longitude, "'-", 1);
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double RotatorAngle
        {
            get
            {
                object Object = GetKeywordValue("POSANGLE");
                if (Object == null)
                {
                    Object = GetKeywordValue("ROTATANG");
                    if (Object != null)
                    {
                        RemoveKeyword("ROTATANG");
                        AddKeyword("POSANGLE", (double)Object, "MoonLite NightCrawler 360 Degree Rotator Mechanical Angle");
                    }
                    else
                        return double.MinValue;
                }

                return Convert.ToDouble(Object);
            }

            set
            {
                AddKeyword("POSANGLE", value, "MoonLite NightCrawler Rotator Posistion");
            }

        }
        // *********************************************************************************************************
        // *********************************************************************************************************
        // Various programs appear to screw this up - fix it
        private void SetEGain()
        {
            // Use graphs found on manufacturer website

            double egain = -1.0;
            double gain = Gain;
            string camera = Camera;

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
        public void SetIntegrationParamaters()
        {
            List<Keyword> keys = new List<Keyword>(mKeywordList);

            foreach (Keyword node in keys)
            {
                if (node.Comment.ToLower().Contains("numberofimages"))
                {
                    var totalFrames = Regex.Match(node.Comment, @"\d+(?!\D*\d)").Value;

                    AddKeyword("NUM-FRMS", totalFrames, "Number of Integrated SubFrames");
                }

                if (node.Comment.Contains("pixelrejection", StringComparison.OrdinalIgnoreCase))
                {
                    if (node.Comment.ToLower().Contains("linear"))
                    {
                        AddKeyword("RJCT-ALG", "LFC", "PixInsight Linear Fit Clipping");
                        break;
                    }

                    if (node.Comment.ToLower().Contains("student"))
                    {
                        AddKeyword("RJCT-ALG", "ESD", "PixInsight Extreme Studentized Deviation Clipping");
                        break;
                    }

                    if (node.Comment.ToLower().Contains("winsor"))
                    {
                        AddKeyword("RJCT-ALG", "WSC", "PixInsight Winsorized Sigma Clipping");
                        break;
                    }

                    if (node.Comment.ToLower().Contains("sigma"))
                    {
                        AddKeyword("RJCT-ALG", "SC", "PixInsight Sigma Clipping");
                        break;
                    }
                }
            }
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
                    FormName = "Cammera Temperature",
                    FormText = "Camera Temperature Not Set",
                    FormEntryText = "Enter Camera Temperature Setpoint:",
                    FileName = FileName
                };

                UserInputFormData returnData = OpenUIForm(formData);

                bool status = double.TryParse(returnData.TextBox, out double temperature);
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
        public double SensorTemperature
        {
            get
            {
                object Object = GetKeywordValue("CCD-TEMP");
                if (Object != null)
                    return Convert.ToDouble(Object);
                return -273.0;
            }
            set
            {
                AddKeyword("CCD-TEMP", value, "Actual sensor temperature");
            }
            /*
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
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SiteName
        {
            get
            {
                object Object = GetKeywordValue("SITENAME");
                if (Object != (object)null)
                    return (string)Object;
                return "Penns Park, PA";
            }
            set
            {
                AddKeyword("SITENAME", value, "Location name of observation site");
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public double SSWeight
        {
            get
            {
                object Obj = GetKeywordValue("SSWEIGHT");
                if (Obj == (object)null)
                    return (double)0.0;
                return -1;
            }
            set
            {
                AddKeyword("SSWEIGHT", value, "");
            }

            //return Math.Round(Convert.ToDouble(Obj), 3, MidpointRounding.AwayFromZero);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string TargetName
        {
            get
            {
                object Object = GetKeywordValue("OBJECT");
                if (Object != null)
                    return Object.ToString();

                return string.Empty;
            }
            set
            {
                if (value.Equals("Master"))
                {
                    AddKeyword("OBJECT", value, "Master Calibration Frame");
                    return;
                }

                string targetName;

                targetName = Regex.Replace(value, @"['""]+", ""); // Remove single and double quotes
                targetName = Regex.Replace(targetName, @"\s+", " "); // Replace multiple spaces with a single space

                // Replace a TargetName containing the word "Panel" with "P"  
                // followed by one or more digits at the end of the string with the same string but with a space inserted before the "P".
                // Return original string if replacement fails.
                targetName = Regex.Replace(targetName, @"\bPanel\b", "P");
                targetName = Regex.Replace(targetName, @"(?<=[A-Za-z0-9\s-])\s*P(\d+)$", " P$1");

                AddKeyword("OBJECT", targetName, "Target Object Name");
            }
        }

        // #########################################################################################################
        // #########################################################################################################
        public string Telescope
        {
            get
            {
                object Object = GetKeywordValue("TELESCOP");
                if (Object != null)
                    return (string)Object;

                return string.Empty;
            }

            set
            {
                AddKeyword("TELESCOP", value, "Telescope");
            }

            /*

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
            }
            */

        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int TotalFrames
        {
            get
            {
                // Temp
                Keyword keyWord = GetKeyword("TOTALFRAMES");
                if (keyWord != null)
                {
                    AddKeyword("NUM-FRMS", keyWord.Value, keyWord.Comment);
                    RemoveKeyword("TOTALFRAMES");
                }
                // Temp

                object Object = GetKeywordValue("NUM-FRMS");
                if (Object != null)
                    return Convert.ToInt32(Object);

                return 0;
            }
            set
            {
                SetIntegrationParamaters();
            }
            /*
                

            object Object = GetKeywordValue("FRAMES");
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
                        AddKeyword("NUM-FRMS", frames, "Total number of Integrated SubFrames");

                        if (returnValue.mGlobalCheckBox)
                            return -frames;
                        else
                            return frames;
                    }
                }
                return -1;
            }

            return (int)Object;
            */
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public List<string> WeightKeyword
        {
            get
            {
                List<string> wList = new List<string>();

                object Obj = GetKeywordValue("SSWEIGHT");
                if (Obj != null)
                {
                    wList.Add("SSWEIGHT");
                }

                Obj = GetKeywordValue("NWEIGHT");
                if (Obj != null)
                {
                    wList.Add("NWEIGHT");
                }

                Obj = GetKeywordValue("W_SNR");
                if (Obj != null)
                {
                    wList.Add("W_SNR");
                }

                Obj = GetKeywordValue("W_FWHM");
                if (Obj != null)
                {
                    wList.Add("W_FWHM");
                }

                Obj = GetKeywordValue("W_ECC");
                if (Obj != null)
                {
                    wList.Add("W_ECC");
                }

                Obj = GetKeywordValue("W_PSFSNR");
                if (Obj != null)
                {
                    wList.Add("W_PSFSNR");
                }

                Obj = GetKeywordValue("W_PSFS");
                if (Obj != null)
                {
                    wList.Add("W_PSFS");
                }

                return wList;
            }
        }
        // *********************************************************************************************************
        // *********************************************************************************************************

        // #########################################################################################################
        // #########################################################################################################

    }
}
