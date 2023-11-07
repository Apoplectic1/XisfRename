using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using XisfFileManager.Enums;
using XisfFileManager.TargetScheduler.Tables;
using static System.Net.WebRequestMethods;

namespace XisfFileManager.FileOperations
{
    public class XisfFile
    {
        // Member Strutures
        public XDocument mXDoc { get; set; }
        public KeywordList KeywordList { get; set; }

        public string XmlString { get; set; }

        public XisfFile()
        {
            mXDoc = new XDocument();
            KeywordList = new KeywordList();
            ImageAttachmentLength = new List<int>();
            ImageAttachmentStart = new List<int>();
            ImageAttachmentStartPadding = new List<int>();
            ThumbnailAttachmentLength = new List<int>();
            ThumbnailAttachmentStart = new List<int>();
            ThumbnailAttachmentStartPadding = new List<int>();
        }

        // Properties
        public void Clear()
        {
            mXDoc = new XDocument();
            KeywordList.Clear();
            ImageAttachmentLength.Clear();
            ImageAttachmentStart.Clear();
            ImageAttachmentStartPadding.Clear();
            ThumbnailAttachmentLength.Clear();
            ThumbnailAttachmentStart.Clear();
            ThumbnailAttachmentStartPadding.Clear();
        }

        public string XmlVersionText { get; set; }
        public string XmlCommentText { get; set; }
        public void AddKeyword(string keyword, string value, string comment = "Xisf File Manager")
        {
            KeywordList.AddKeyword(keyword, value, comment);
        }

        public void AddXMLKeyword(string keyword, string value, string comment = "Xisf File Manager")
        {
            KeywordList.AddKeyword(keyword, value, comment);
        }
        public void RemoveKeyword(string keyword)
        {
            KeywordList.RemoveKeyword(keyword);
        }
        public void RemoveKeyword(string keyword, object oValue)
        {
            KeywordList.RemoveKeyword(keyword, oValue);
        }
        public Keyword GetKeyword(string keyword)
        {
            return KeywordList.GetKeyword(keyword);
        }
        public object GetKeywordValue(string keyword)
        {
            return KeywordList.GetKeywordValue(keyword);
        }
        public object GetKeywordComment(string keyword)
        {
            return KeywordList.GetKeywordComment(keyword);
        }
        public double AmbientTemperature
        {
            get { return KeywordList.AmbientTemperature; }
            set { KeywordList.AmbientTemperature = value; }
        }
        public int Binning
        {
            get { return KeywordList.Binning; }
            set { KeywordList.Binning = value; }
        }
        public string Camera
        {
            get { return KeywordList.Camera; }
            set { KeywordList.Camera = value; }
        }
        public DateTime CaptureDateTime
        {
            get { return KeywordList.CaptureTime; }
            set { KeywordList.CaptureTime = value; }
        }
        public string CaptureSoftware
        {
            get { return KeywordList.CaptureSoftware; }
            set { KeywordList.CaptureSoftware = value; }
        }
        public string CBIAS
        {
            get { return KeywordList.CBIAS; }
            set { KeywordList.CBIAS = value; }
        }
        public string CDARK
        {
            get { return KeywordList.CDARK; }
            set { KeywordList.CDARK = value; }
        }
        public string CFLAT
        {
            get { return KeywordList.CFLAT; }
            set { KeywordList.CFLAT = value; }
        }
        public string CPANEL
        {
            get { return KeywordList.CPANEL; }
            set { KeywordList.CPANEL = value; }
        }
        public double ExposureSeconds
        {
            get { return KeywordList.ExposureSeconds; }
            set { KeywordList.ExposureSeconds = value; }
        }
        public string FilePath { get; set; } = string.Empty;
        public string FilterName
        {
            get { return KeywordList.FilterName; }
            set { KeywordList.AddKeyword("FILTER", value); }
        }
        public double FocalLength
        {
            get { return KeywordList.FocalLength; }
            set { KeywordList.FocalLength = value; }
        }
        public int FocuserPosition
        {
            get { return KeywordList.FocuserPosition; }
            set { KeywordList.FocuserPosition = value; }
        }
        public double FocuserTemperature
        {
            get { return KeywordList.FocuserTemperature; }
            set { KeywordList.FocuserTemperature = value; }
        }
        public eFrame FrameType
        {
            get { return KeywordList.FrameType; }
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
        public int Gain
        {
            get { return KeywordList.Gain; }
            set { KeywordList.Gain = value; }
        }
        public List<int> ImageAttachmentLength { get; set; }
        public List<int> ImageAttachmentStart { get; set; }
        public List<int> ImageAttachmentStartPadding { get; set; }
        public int FileNameNumberIndex { get; set; }
        public int Offset
        {
            get { return KeywordList.Offset; }
            set { KeywordList.Offset = value; }
        }
        public string Rejection
        {
            get { return KeywordList.Rejection; }
            set { KeywordList.Rejection = value; }
        }
        public string RotationAngle
        {
            get
            {
                double angle = RotatorSkyAngle;
                if (angle != double.MinValue)
                    return "S" + angle.FormatRotationAngle();

                angle = RotatorMechanicalAngle;
                if (angle != double.MinValue)
                    return "M" + angle.FormatRotationAngle();

                return string.Empty;
            }
        }

        public double RotatorMechanicalAngle
        {
            get { return KeywordList.RotatorMechanicalAngle; }
            set { KeywordList.RotatorMechanicalAngle = value; }
        }
        public double RotatorSkyAngle
        {
            get { return KeywordList.RotatorSkyAngle; }
            set { KeywordList.RotatorSkyAngle = value; }
        }
        public double SensorTemperature
        {
            get { return KeywordList.SensorTemperature; }
            set { KeywordList.SensorTemperature = value; }
        }
        public double SSWeight { get; set; }
        public List<int> ThumbnailAttachmentStartPadding { get; set; }
        public List<int> ThumbnailAttachmentLength { get; set; }
        public List<int> ThumbnailAttachmentStart { get; set; }
        public bool KeepPanel { get; set; }
        /// <summary>
        /// Updates an Xisf File Target Name.
        /// Appends " Stars" if the Xisf File is contained in a "Stars Filter" Directory.
        /// Sets CPANEL to the returned string value. Set CSTARS when containing directory name contains "Stars ".
        /// </summary>
        /// <returns>The TargetName string</returns>
        public string TargetName
        {
            get { return KeywordList.TargetName; }
            set
            {
                string targetName = value;

                targetName = Regex.Replace(targetName, @"\s+", " "); // Replace multiple spaces with a single space

                // First match Stars
                if (Path.GetDirectoryName(FilePath).Contains("Stars "))
                {
                    KeywordList.CSTARS = "Stars " + KeywordList.FilterName;
                    // Make sure files in "Stars" directories are properly named: "Target Stars"
                    if (!targetName.Contains("Stars"))
                        targetName = targetName + " Stars";
                }
                else
                    KeywordList.CSTARS = "*"; // Wildcard by CPANEL

                // After Stars, replace a TargetName containing the word "Panel" with "P" followed by one or more digits
                // Return original string if replacement fails.
                targetName = Regex.Replace(targetName, @"\bPanel\b", "P");
                targetName = Regex.Replace(targetName, @"(?<=[A-Za-z0-9\s-])\s*P(\d+)$", " P$1");

                if (!KeepPanel)
                    KeywordList.CPANEL = targetName.Replace(" Stars", ""); // Remove " Stars" so we Register and Intergrate by CPanel

                KeywordList.TargetName = targetName;
            }
        }
        public string Telescope
        {
            get { return KeywordList.Telescope; }
            set { KeywordList.Telescope = value; }
        }
        public int TotalFrames
        {
            get { return KeywordList.TotalFrames; }
            set { KeywordList.TotalFrames = value; }
        }
        public bool Unique { get; set; }
        public List<string> WeightKeyword
        {
            get { return KeywordList.WeightKeyword; }
        }

        // ***********************************************************************************************************************************

        // Access these properties from anything that instances the XifsFile class

        // ************************************************************************************************
        // ************************************************************************************************

        public void ImageAttachment(XElement element)
        {
            XAttribute attribute = element.Attribute("location");

            if (attribute != null)
            {
                string attachment = attribute.Value;

                string[] values = attachment.Split(':');

                ImageAttachmentStart.Add(Convert.ToInt32(values[1]));
                ImageAttachmentLength.Add(Convert.ToInt32(values[2]));
            }
        }

        public void ThumbnailAttachment(XElement element)
        {
            XAttribute attribute = element.Attribute("location");

            if (attribute != null)
            {
                string attachment = attribute.Value;

                string[] values = attachment.Split(':');

                ThumbnailAttachmentStart.Add(Convert.ToInt32(values[1]));
                ThumbnailAttachmentLength.Add(Convert.ToInt32(values[2]));
            }
        }

        public Keyword NewKeyWord(string sName, string oValue, string sComment)
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
        public void AddKeywordKeepDuplicates(string sName, string oValue, string sComment = "XISF File Manager")
        {
            Keyword newKeyword = NewKeyWord(sName, oValue, sComment);

            KeywordList.AddKeywordKeepDuplicates(newKeyword);
        }

        public void AddXMLKeyword(XElement element)
        {
            // It looks like NINA 3 may have added a fourth XML Keyword string field called "xmlns=xxx'. We should ignore this field
            // Example: "{<FITSKeyword name="IMAGETYP" value="LIGHT" comment="Type of exposure" xmlns="http://www.pixinsight.com/xisf" />}"

            string elementName = string.Empty;
            string elementValue = string.Empty;
            string elementComment = string.Empty;

            try
            {
                // Split the XML Keyword triple into elementName, elementValue, elementComment
                elementName = element.Attribute("name").Value;
                elementValue = element.Attribute("value").Value;
                elementComment = element.Attribute("comment").Value;

                // Get rid of an extra decimal point at the end of what should be integers
                elementValue = elementValue.TrimEnd('.');

                AddKeywordKeepDuplicates(elementName, elementValue, elementComment);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nXML to Keyword Exception Thrown:\n" +
                    "\tKeyword Name:" + elementName.ToString() +
                    "\tKeyword Value:" + elementValue.ToString() +
                    "\tKeyword Comment:" + elementComment.ToString() +
                    "\n\n" + ex.Message);
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // Set Who and Where
        public void SetObservationSite()
        {
            AddKeyword("SITENAME", "Penns Park, PA", "841 Durham Rd, Penns Park, PA 18943");
            AddKeyword("SITELONG", "-74.997372", "Logitude of observation site - Degrees East");
            AddKeyword("SITELAT", "40.282852", "Latitude of observation site - Degrees North");
            AddKeyword("SITEELEV", "80.0", "Altitude of observation site - MSL Meters");
            RemoveKeyword("LONG-OBS");
            RemoveKeyword("LAT-OBS");
            RemoveKeyword("ALT-OBS");
            RemoveKeyword("OBSGEO-L");
            RemoveKeyword("OBSGEO-B");
            RemoveKeyword("OBSGEO-H");

            AddKeyword("OBSERVER", "Dan Stark", "P.O. Box 156, Penns Park, PA 18943 djstark@gmail.com (609) 575-5927");
        }

        public static Comparison<XisfFile> CaptureTimeComparison = delegate (XisfFile object1, XisfFile object2)
        {
            if (object1 == null) return 1;
            if (object2 == null) return 1;
            if (object1.KeywordList.CaptureTime > object2.KeywordList.CaptureTime) return 1;
            if (object1.KeywordList.CaptureTime < object2.KeywordList.CaptureTime) return -1;
            return 0;
        };
    }

    public static class XisfFileExtensions
    {
        public static string FormatTemperature(this double temperatureValue)
        {
            string fmt = "{00:+00.0;-00.0;+00.0}";
            return string.Format(fmt, Math.Round(temperatureValue, 1));
        }

        public static string FormatRotationAngle(this double rotationAngle)
        {
            return rotationAngle.ToString("000.0");
        }

        public static string FormatExposureTime(this double seconds)
        {
            if (seconds < 10)
            {
                if (seconds < 0.00001)
                    return "0.000";

                if (seconds < 1)
                    return ((decimal)seconds / 1.000000000000000000000000000000000m).ToString("0.000");

                return seconds.ToString("0.000");
            }
            else
            {
                return seconds.ToString("0000");
            }
        }
    }
}