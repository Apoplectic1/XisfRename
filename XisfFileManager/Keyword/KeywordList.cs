﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XisfFileManager.Forms.UserInputForm;

using XisfFileManager.Enums;
using static System.Windows.Forms.DataFormats;


namespace XisfFileManager
{
    public class KeywordList
    {
        private Regex onlyNumerics = new Regex(@"^[\+\-]?\d*\.?[Ee]?[\+\-]?\d*$", RegexOptions.Compiled);
        public List<Keyword> mKeywordList { get; set; }

        // ----------------------------------------------------------------------------------------------------------

        public KeywordList()
        {
            mKeywordList = new List<Keyword>();
        }

        // ----------------------------------------------------------------------------------------------------------

        public void Clear()
        {
            mKeywordList.Clear();
        }

        // ----------------------------------------------------------------------------------------------------------

        public static Keyword NewKeyword(string sName, string oValue, string sComment)
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

        public string GetKeywordValue(string sName)
        {
            Keyword node = mKeywordList.Find(i => i.Name == sName);
            if (node == null)
                return string.Empty;

            return node.Value;
        }

        // ----------------------------------------------------------------------------------------------------------

        public string GetKeywordComment(string sName)
        {
            Keyword node = mKeywordList.Find(i => i.Name == sName);
            if (node == null)
                return string.Empty;

            return node.Comment;
        }

        // ----------------------------------------------------------------------------------------------------------

        public void RemoveKeyword(string sName)
        {
            mKeywordList.RemoveAll(i => i.Name.Equals(sName));
        }

        // ----------------------------------------------------------------------------------------------------------

        public void RemoveKeyword(string sName, object oValue)
        {
            mKeywordList.RemoveAll(i => i.Name.Equals(sName) && i.Value.Equals(oValue));
        }

        // ----------------------------------------------------------------------------------------------------------

        public void AddKeyword(string sName, string oValue, string sComment = "XISF File Manager")
        {
            mKeywordList.RemoveAll(i => i.Name.Equals(sName));

            Keyword newKeyword = NewKeyword(sName, oValue, sComment);

            mKeywordList.Add(newKeyword);
        }

        // ----------------------------------------------------------------------------------------------------------

        public void AddKeywordKeepDuplicates(Keyword newKeyword)
        {
            mKeywordList.Add(newKeyword);
        }

        // ----------------------------------------------------------------------------------------------------------

        public void AddKeywordKeepDuplicates(string sName, string oValue, string sComment = "XISF File Manager")
        {
            Keyword newKeyword = NewKeyword(sName, oValue, sComment);

            mKeywordList.Add(newKeyword);
        }

        // *********************************************************************************************************

        public double Airmass
        {
            get
            {
                string value = GetKeywordValue("AIRMASS");
                if (value == string.Empty)
                    return -1.0;

                return Convert.ToDouble(value);
            }
            set { AddKeyword("AIRMASS", value.ToString("F3"), "[#] Line-of-sight Atmospheres"); }
        }

        // *********************************************************************************************************
 
        public double AmbientTemperature
        {
            get
            {
                string value = GetKeywordValue("AMB-TEMP");
                if (value == string.Empty)
                    return -273.0;

                return Convert.ToDouble(value);
            }
            set { AddKeyword("AMB-TEMP", value.ToString("F1"), "[deg C] Local Temerature from Open Weather API"); }
        }

        // *********************************************************************************************************

        public int Binning
        {
            get
            {
                string value = GetKeywordValue("XBINNING");
                if (value == string.Empty)
                    return -1;

                return Convert.ToInt32(value);
            }
            set
            {
                AddKeyword("XBINNING", value.ToString(), "[#] Camera Binning");
                AddKeyword("YBINNING", value.ToString(), "[#] Camera Binning");
            }
        }

        // *********************************************************************************************************

        public string Camera
        {
            get { return GetKeywordValue("INSTRUME"); }
            set { AddKeyword("INSTRUME", value, "[name] Imaging Camera"); }
        }

        // *********************************************************************************************************

        public DateTime CaptureTime
        {
            get
            {
                string value = GetKeywordValue("DATE-LOC");
                if (value == string.Empty)
                {
                    value = GetKeywordValue("DATE-OBS");
                    if (value != string.Empty)
                    {
                        // Convert UTC to Local Time
                        DateTime dateTimeUtc = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);

                        // Ensure that the DateTime is in UTC by setting its Kind property
                        dateTimeUtc = DateTime.SpecifyKind(dateTimeUtc, DateTimeKind.Utc);

                        // Specify the target time zone
                        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

                        // Convert UTC time to local time
                        DateTime dateTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, localTimeZone);

                        AddKeyword("DATE-LOC", dateTimeLocal.ToString("yyyy-MM-ddTHH:mm:ss.fff"), "Local capture time");
                        return DateTime.Parse(value);
                    }
                    else
                        return DateTime.MinValue;
                }
                return DateTime.Parse(value);
            }
            set { AddKeyword("DATE-LOC", value.ToString("yyyy-MM-ddTHH:mm:ss.fff"), "Local capture time"); }
        }

        // *********************************************************************************************************

        public string CaptureSoftware
        {
            get { return GetKeywordValue("SWCREATE"); }
            set { AddKeyword("SWCREATE", value.ToString(), "[name] Equipment Control and Automation Application"); }
        }

        // *********************************************************************************************************

        public string CBIAS
        {
            get { return GetKeywordValue("CBIAS"); }
            set { AddKeyword("CBIAS", value.ToString(), "[#] PixInsight WBPP PreProcessing Group Keyword"); }
        }

        // *********************************************************************************************************

        public string CDARK
        {
            get { return GetKeywordValue("CDARK"); }
            set { AddKeyword("CDARK", value.ToString(), "[#] PixInsight WBPP PreProcessing Group Keyword"); }
        }

        // *********************************************************************************************************

        public string CFLAT
        {
            get { return GetKeywordValue("CFLAT"); }
            set { AddKeyword("CFLAT", value.ToString(), "[#] PixInsight WBPP PreProcessing Group Keyword"); }
        }

        // *********************************************************************************************************

        public string CPANEL
        {
            get { return GetKeywordValue("CPANEL"); }
            set { AddKeyword("CPANEL", value.ToString(), "[name] PixInsight WBPP PostProcessing Group Keyword"); }
        }

        // *********************************************************************************************************

        public string CSTARS
        {
            get { return GetKeywordValue("CSTARS"); }
            set { AddKeyword("CSTARS", value.ToString(), "[name] PixInsight WBPP PostProcessing Group Keyword"); }
        }

        // *********************************************************************************************************

        public double ExposureSeconds
        {
            get 
            {
                double exposure;
                bool bStatus;
                string value = GetKeywordValue("EXPOSURE");
                if (value != string.Empty)
                {
                    bStatus = double.TryParse(value, out exposure);
                    if (bStatus)
                    {
                        RemoveKeyword("EXPTIME");
                        return exposure;
                    }
                }
                
                value = GetKeywordValue("EXPTIME");
                if (value != string.Empty)
                {
                    bStatus = double.TryParse(value, out exposure);
                    if (bStatus)
                    {
                        AddKeyword("EXPOSURE", exposure.ToString(), "[seconds] Imaging Camera Exposure Time");
                        RemoveKeyword("EXPTIME");
                        return exposure;
                    }
                }
                AddKeyword("EXPOSURE", "0.0", "[seconds] Imaging Camera Exposure Time");
                return 0.0;
            }
            set { AddKeyword("EXPOSURE", value.ToString(), "[seconds] Imaging Camera Exposure Time"); }
        }

        // *********************************************************************************************************

        public string FilterName
        {
            get { return GetKeywordValue("FILTER"); }
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

        // *********************************************************************************************************

        public double FocalLength
        {
            get
            {
                string value = GetKeywordValue("FOCALLEN");
                if (value == string.Empty)
                    return -1.0;
    
                double length = Convert.ToDouble(value);
                return length;
            }
            set { AddKeyword("FOCALLEN", value.ToString("F1"), "[mm] OTA Focal length"); }
        }

        // *********************************************************************************************************

        public int FocuserPosition
        {
            get
            {
                string value = GetKeywordValue("FOCPOS");
                if (value == string.Empty)
                    return -1;
                
                return Convert.ToInt32(value);
            }
            set { AddKeyword("FOCPOS", value.ToString(), "[um] NiteCrawler Position - 94580 Steps, 0.2667 um/Step"); }
        }

        // *********************************************************************************************************

        public double FocuserTemperature
        {
            get
            {
                string value = GetKeywordValue("FOCTEMP");
                if (value == string.Empty) return -273.0;
                
                return Convert.ToDouble(value);
            }
            set { AddKeyword("FOCTEMP", value.ToString(), "[degC] NightCrawler Focuser Temperature"); }
        }

        // *********************************************************************************************************
 
        public eFrame FrameType
        {
            get
            {
                string value = GetKeywordValue("IMAGETYP");
                if (value == string.Empty) 
                    return eFrame.EMPTY;

                if (value.ToLower().Contains("light")) return eFrame.LIGHT;
                if (value.ToLower().Contains("dark")) return eFrame.DARK;
                if (value.ToLower().Contains("flat")) return eFrame.FLAT;
                if (value.ToLower().Contains("bias")) return eFrame.BIAS;

                return eFrame.EMPTY;
            }
            set
            {
                switch (value)
                {
                    case eFrame.LIGHT:
                        AddKeyword("IMAGETYP", "Light", "Type of frame capture");
                        break;
                    case eFrame.DARK:
                        AddKeyword("IMAGETYP", "Dark", "Type of frame capture");
                        break;
                    case eFrame.FLAT:
                        AddKeyword("IMAGETYP", "Flat", "Type of frame capture");
                        break;
                    case eFrame.BIAS:
                        AddKeyword("IMAGETYP", "Bias", "Type of frame capture");
                        break;
                    default:
                        AddKeyword("IMAGETYP", "", "Type of frame capture");
                        break;
                }
            }
        }

        // *********************************************************************************************************

        public int Gain
        {
            get
            {
                string value = GetKeywordValue("GAIN");
                if (value == string.Empty)
                    return -1;

                int gain = Convert.ToInt32(value);
                return gain;
            }
            set
            {
                AddKeyword("GAIN", value.ToString(), "[#] Imaging Camera Gain");
                SetEGain();
            }
        }

        // *********************************************************************************************************

        public string MSTRALG
        {
            get { return GetKeywordValue("MSTRALG"); }
            set { AddKeyword("MSTRALG", value, "[name] Master Frame Rejection Algorithm"); }
        }

        // *********************************************************************************************************

        public int MSTRFRMS
        {
            get 
            { 
                string value = GetKeywordValue("MSTRFRMS");
                if (value == string.Empty)
                    return - 1;

                return Convert.ToInt32(value);
            }
            set { AddKeyword("MSTRFRMS", value.ToString(), "[#] Master Frame Integration Total"); }
        }

        // *********************************************************************************************************

        public int Offset
        {
            get
            {
                string value = GetKeywordValue("OFFSET");
                if (value == string.Empty)
                    return -1;

                int offset = Convert.ToInt32(value);
                return offset;
            }
            set
            {
                if (Camera.Contains("183"))
                {
                    AddKeyword("OFFSET", value.ToString(), "[#] ADU Offset divided by 5");
                    return;
                }

                if (Camera.Contains("533"))
                {
                    AddKeyword("OFFSET", value.ToString(), "[#] ADU Offset divided by 40");
                    return;
                }

                if (Camera.Contains("178"))
                {
                    AddKeyword("OFFSET", value.ToString(), "[#] ADU Offset divided by 18.33");
                    return;
                }

                if (Camera.Contains("144"))
                {
                    RemoveKeyword("OFFSET");
                    return;
                }
            }
        }

        // *********************************************************************************************************

        public double PixelSize
        {
            get
            {
                string value = GetKeywordValue("XPIXSZ");
                if (value == string.Empty) 
                    return -1;

                return Convert.ToDouble(value);
            }
            set
            {
                AddKeyword("XPIXSZ", value.ToString(), "[um] Sensor Photosite Width Microns");
                AddKeyword("YPIXSZ", value.ToString(), "[um] Sensor Photosite Height Microns");
            }
        }

        // *********************************************************************************************************

        public double RotatorPosition
        {
            get
            {
                string value = GetKeywordValue("POSANGLE");
                if (value == string.Empty)
                    return double.MinValue;

                return Convert.ToDouble(value);
            }
            set { AddKeyword("POSANGLE", value.ToString(), "[degrees] NightCrawler Mechanical Position 0.001 deg/Step"); }
        }

        // *********************************************************************************************************
      
        public double RotatorSkyAngle
        {
            get
            {
                string value = GetKeywordValue("OBJCTROT");
                if (value == string.Empty)
                    return double.MinValue;

                return Convert.ToDouble(value);
            }
            set { AddKeyword("OBJCTROT", value.ToString(), "[degrees] Image Sky Angle at Frame Center"); }
        }

        // *********************************************************************************************************

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

            AddKeyword("EGAIN", egain.ToString(), "[#] Electrons per ADU per manufacturer graphs");
        }

        // *********************************************************************************************************

        public double SensorTemperature
        {
            get
            {
                string value = GetKeywordValue("CCD-TEMP");
                if (value == string.Empty) 
                    return -273;
                
                return Convert.ToDouble(value);
            }
            set { AddKeyword("CCD-TEMP", value.ToString("F1"), "[degC] Imaging Camera Sensor Temperature"); }
        }

        // *********************************************************************************************************

        public double SensorTemperatureSetPoint
        {
            get
            {
                string value = GetKeywordValue("SET-TEMP");
                if (value == string.Empty)
                    return -273.0;

                double temperature = Convert.ToDouble(value);
                return temperature;
            }
            set { AddKeyword("SET-TEMP", value.ToString("F1"), "[degC] Imaging Camera Temperature Set Point"); }
        }

        // *********************************************************************************************************

        public string TargetName
        {
            get { return GetKeywordValue("OBJECT"); }
            set
            {
                if (value.Equals("Master"))
                    AddKeyword("OBJECT", "Master", "[name] Master Calibration Frame");
                else
                    AddKeyword("OBJECT", value, "[name] Target Object Name");
            }
        }

        // *********************************************************************************************************

        public string Telescope
        {
            get { return GetKeywordValue("TELESCOP"); }
            set { AddKeyword("TELESCOP", value.ToString(), "[name] Imaging OTA"); }
        }

        // *********************************************************************************************************
        
        public List<string> WeightKeyword
        {
            get
            {
                List<string> wList = new List<string>();

                string value = GetKeywordValue("SSWEIGHT");
                if (value != string.Empty)
                    wList.Add("SSWEIGHT");

                value = GetKeywordValue("NWEIGHT");
                if (value != string.Empty)
                    wList.Add("NWEIGHT");

                value = GetKeywordValue("W_SNR");
                if (value != string.Empty)
                    wList.Add("W_SNR");

                value = GetKeywordValue("W_FWHM");
                if (value != string.Empty)
                    wList.Add("W_FWHM");

                value = GetKeywordValue("W_ECC");
                if (value != string.Empty)
                    wList.Add("W_ECC");

                value = GetKeywordValue("W_PSFSNR");
                if (value != string.Empty)
                    wList.Add("W_PSFSNR");

                value = GetKeywordValue("W_PSFS");
                if (value != string.Empty)
                    wList.Add("W_PSFS");

                return wList;
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************

        public void SetMasterFrameKeywords()
        {
            List<Keyword> keys = new List<Keyword>(mKeywordList);

            foreach (Keyword node in keys)
            {
                if (node.Comment.ToLower().Contains("numberofimages"))
                {
                    var totalFrames = Regex.Match(node.Comment, @"\d+(?!\D*\d)").Value;

                    AddKeyword("MSTRFRMS", totalFrames, "Number of Integrated SubFrames");
                }

                if (node.Comment.Contains("pixelrejection", StringComparison.OrdinalIgnoreCase))
                {
                    if (node.Comment.ToLower().Contains("linear"))
                    {
                        AddKeyword("MSTRALG", "LFC", "PixInsight Linear Fit Clipping");
                        break;
                    }

                    if (node.Comment.ToLower().Contains("student"))
                    {
                        AddKeyword("MSTRALG", "ESD", "PixInsight Extreme Studentized Deviation Clipping");
                        break;
                    }

                    if (node.Comment.ToLower().Contains("winsor"))
                    {
                        AddKeyword("MSTRALG", "WSC", "PixInsight Winsorized Sigma Clipping");
                        break;
                    }

                    if (node.Comment.ToLower().Contains("sigma"))
                    {
                        AddKeyword("MSTRALG", "SC", "PixInsight Sigma Clipping");
                        break;
                    }
                }
            }


            Keyword keyword;
            Keyword algorithm;
            keyword = GetKeyword("MSTRALG");
            if (keyword == null)
            {
                algorithm = GetKeyword("REJECTION");
                if (algorithm != null)
                    AddKeyword("MSTRALG", algorithm.Value, algorithm.Comment);

                algorithm = GetKeyword("Rejection");
                if (algorithm != null)
                    AddKeyword("MSTRALG", algorithm.Value, algorithm.Comment);

                algorithm = GetKeyword("REJECTIO");
                if (algorithm != null)
                    AddKeyword("MSTRALG", algorithm.Value, algorithm.Comment);

                // Make sure at least one rejection keyword was found
                string value = GetKeywordValue("MSTRALG");
                if (value == string.Empty)
                    AddKeyword("MSTRALG", "ESD", "PixInsight Extreme Studentized Deviation Clipping");
            }

            string numFrames;
            keyword = GetKeyword("MSTRFRMS");
            if (keyword == null)
            {
                numFrames = GetKeywordValue("TOTALFRAMES");
                if (numFrames != string.Empty && numFrames != "1")
                    AddKeyword("MSTRFRMS", numFrames, "Number of Integrated SubFrames");

                numFrames = GetKeywordValue("NUM-FRMS");
                if (numFrames != string.Empty && numFrames != "1")
                    AddKeyword("MSTRFRMS", numFrames, "Number of Integrated SubFrames");

                // Make sure at least one number of frames keyword was found
                string value = GetKeywordValue("MSTRFRMS");
                if (value == string.Empty)
                    AddKeyword("MSTRFRMS", "32", "Number of Integrated SubFrames");

            }

            RemoveKeyword("ALT-OBS");
            RemoveKeyword("AOCAMBT");
            RemoveKeyword("CBIAS");
            RemoveKeyword("CDARK");
            RemoveKeyword("CFLAT");
            RemoveKeyword("COMMENT");
            RemoveKeyword("Camera");
            RemoveKeyword("CPANEL");
            RemoveKeyword("CSTARS");
            RemoveKeyword("Camera");
            RemoveKeyword("DEC");
            RemoveKeyword("FILENAME");
            RemoveKeyword("HISTORYTORY");
            RemoveKeyword("LAT-OBS");
            RemoveKeyword("LONG-OBS");
            RemoveKeyword("HISTORY");
            RemoveKeyword("NOISE00");
            RemoveKeyword("NOISEA00");
            RemoveKeyword("NOISEH00");
            RemoveKeyword("NOISEL00");
            RemoveKeyword("NUM-FRMS");
            RemoveKeyword("OBJCTDEC");
            RemoveKeyword("OBJCTRA");
            RemoveKeyword("OBSGEO-B");
            RemoveKeyword("OBSGEO-H");
            RemoveKeyword("OBSGEO-L");
            RemoveKeyword("PROTECT");
            RemoveKeyword("PSFFLP00");
            RemoveKeyword("PSFFLX00");
            RemoveKeyword("PSFMFL00");
            RemoveKeyword("PSFMFP00");
            RemoveKeyword("PSFMST00");
            RemoveKeyword("PSFNST00");
            RemoveKeyword("PSFSGL00");
            RemoveKeyword("PSFSGN00");
            RemoveKeyword("PSFSGP00");
            RemoveKeyword("PSFSGTYP");
            RemoveKeyword("Protected");
            RemoveKeyword("RA");
            RemoveKeyword("RADESYS");
            RemoveKeyword("REJECTIO");
            RemoveKeyword("REJECTION");
            RemoveKeyword("Rejection");
            RemoveKeyword("RESOUNIT");
            RemoveKeyword("RJCT-ALG");
            RemoveKeyword("TOTALFRA");
            RemoveKeyword("TOTALFRAMES");
            RemoveKeyword("XBAYROFF");
            RemoveKeyword("XORGSUBF");
            RemoveKeyword("YBAYROFF");
            RemoveKeyword("YORGSUBF");
        }

        // #########################################################################################################
        // #########################################################################################################

    }
}
