using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public class XisfFile
    {
        // Member Strutures
        public XDocument mXDoc;
        public KeywordList mKeywordList;

        public XisfFile()
        {
            mXDoc = new XDocument();    
            mKeywordList = new KeywordList();
        }

        // Properties
        public void Clear()
        {
            mXDoc = new XDocument();
            mKeywordList.Clear();
        }

        public void AddKeyword(string keyword, object value, string comment = "Xisf File Manager")
        {
            mKeywordList.AddKeyword(keyword, value, comment);
        }

        public void AddXMLKeyword(string keyword, object value, string comment = "Xisf File Manager")
        {
            mKeywordList.AddKeyword(keyword, value, comment);
        }
        public void RemoveKeyword(string keyword)
        {
           mKeywordList.RemoveKeyword(keyword);
        }
        public void RemoveKeyword(string keyword, object oValue)
        {
            mKeywordList.RemoveKeyword(keyword, oValue);
        }
        public Keyword GetKeyword(string keyword)
        {
            return mKeywordList.GetKeyword(keyword);
        }
        public object GetKeywordValue(string keyword)
        {
            return mKeywordList.GetKeywordValue(keyword);
        }
        public object GetKeywordComment(string keyword)
        {
            return mKeywordList.GetKeywordComment(keyword);
        }
        public double AmbientTemperature
        {
            get { return mKeywordList.AmbientTemperature; }
            set { mKeywordList.AmbientTemperature = value; }
        }
        public bool Approved
        {
            get { return mKeywordList.Approved; }
            set { mKeywordList.Approved = value; }
        }
        public int Binning
        {
            get { return mKeywordList.Binning; }
            set { mKeywordList.Binning = value; }
        }
        public string Camera
        {
            get { return mKeywordList.Camera; }
            set { mKeywordList.Camera = value; }
        }
        public DateTime CaptureDateTime
        {
            get { return mKeywordList.CaptureDateTime; }
            set { mKeywordList.CaptureDateTime = value; }
        }
        public string CaptureSoftware
        {
            get { return mKeywordList.CaptureSoftware; }
            set { mKeywordList.CaptureSoftware = value; }
        }
        public string CBIAS
        {
            get { return mKeywordList.CBIAS; }
            set { mKeywordList.CBIAS = value; }
        }
        public string CDARK
        {
            get { return mKeywordList.CDARK; }
            set { mKeywordList.CDARK = value; }
        }
        public string CFLAT
        {
            get { return mKeywordList.CFLAT; }
            set { mKeywordList.CFLAT = value; }
        }
        public string CLIGHT
        {
            get { return mKeywordList.CLIGHT; }
            set { mKeywordList.CLIGHT = value; }
        }
        public string CPANEL
        {
            get { return mKeywordList.CPANEL; }
            set { mKeywordList.CPANEL = value; }
        }
        public double ExposureSeconds
        {
            get { return mKeywordList.ExposureSeconds; }
            set { mKeywordList.ExposureSeconds = value; }
        }
        public string FilePath { get; set; } = string.Empty;
        public string FilterName
        {
            get { return mKeywordList.FilterName; }
            set { mKeywordList.AddKeyword("FILTER", value); }
        }
        public int FocalLength
        {
            get { return mKeywordList.FocalLength; }
            set { mKeywordList.FocalLength = value; }
        }
        public int FocuserPosition
        {
            get { return mKeywordList.FocuserPosition; }
            set { mKeywordList.FocuserPosition = value; }
        }
        public double FocuserTemperature
        {
            get { return mKeywordList.FocuserTemperature; }
            set { mKeywordList.FocuserTemperature = value; }
        }
        public eFrameType FrameType
        {
            get { return mKeywordList.FrameType; }
            set { mKeywordList.AddKeyword("IMAGETYP", value); }
        }
        public int Gain
        {
            get { return mKeywordList.Gain; }
            set { mKeywordList.Gain = value; }
        }
        public int ImageAttachmentLength { get; set; } = 0;
        public int ImageAttachmentStart { get; set; } = 0;
        public int ImageAttachmentStartPadding { get; set; } = 0;
        public int Index { get; set; } = 0;
        public bool Master { get; set; } = false;
        public bool NarrowBand { get; set; } = false;
        public int Offset
        {
            get { return mKeywordList.Offset; }
            set { mKeywordList.Offset = value; }
        }
        public bool Protect
        {
            get { return mKeywordList.Protect; }
            set { mKeywordList.Protect = value; }
        }
        public bool RiccardiReducer { get; set; }
        public string Rejection
        {
            get { return mKeywordList.Rejection; }
            set { mKeywordList.Rejection = value; }
        }
        public double RotatorAngle
        {
            get { return mKeywordList.RotatorAngle; }
            set { mKeywordList.RotatorAngle = value; }
        }
        public double SensorTemperature
        {
            get { return mKeywordList.SensorTemperature; }
            set { mKeywordList.SensorTemperature = value; }
        }
        public double SSWeight { get; set; } = 0.0;
        public int ThumbnailAttachmentStartPadding { get; set; } = 0;
        public int ThumbnailAttachmentLength { get; set; } = 0;
        public int ThumbnailAttachmentStart { get; set; } = 0;
        public string TargetObjectName
        {
            get { return mKeywordList.TargetName; }
            set { mKeywordList.TargetName = value; }
        }
        public string Telescope
        {
            get { return mKeywordList.Telescope; }
            set { mKeywordList.Telescope = value; }
        }
        public int TotalFrames
        {
            get { return mKeywordList.TotalFrames; }
            set { mKeywordList.TotalFrames = value; }
        }
        public bool Unique { get; set; } = false;
        public List<string> WeightKeyword
        {
            get { return mKeywordList.WeightKeyword; }
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

                ImageAttachmentStart = Convert.ToInt32(values[1]);
                ImageAttachmentLength = Convert.ToInt32(values[2]);
            }
        }

        public void ThumbnailAttachment(XElement element)
        {
            XAttribute attribute = element.Attribute("location");

            if (attribute != null)
            {
                string attachment = attribute.Value;

                string[] values = attachment.Split(':');

                ThumbnailAttachmentStart = Convert.ToInt32(values[1]);
                ThumbnailAttachmentLength = Convert.ToInt32(values[2]);
            }
        }

        public Keyword NewKeyWord(string sName, object oValue, string sComment)
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
        public void AddKeywordKeepDuplicates(string sName, object oValue, string sComment = "XISF File Manager")
        {
            Keyword newKeyword = NewKeyWord(sName, oValue, sComment);

            mKeywordList.AddKeywordKeepDuplicates(newKeyword);
        }

        public void AddXMLKeyword(XElement element)
        {
            bool bStatus;

            // First remove Keyword characteritics that interfere with later processing
            // Get rid of "'"
            string elementValue = element.Attribute("value").Value;
            //elementValue = elementValue.Replace(" ", "").Replace("'", "");
            elementValue = elementValue.Replace("'", "");

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

        // *********************************************************************************************************
        // *********************************************************************************************************
        // Set Who and Where
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

        public void SetRequiredKeywords()
        {
            Master = mKeywordList.TargetName.Contains("Master");
            NarrowBand = mKeywordList.FilterName.Contains("Ha") || mKeywordList.FilterName.Contains("O3") || mKeywordList.FilterName.Contains("S2");
            RiccardiReducer = mKeywordList.Telescope.EndsWith("R");
         }

        public static Comparison<XisfFile> CaptureTimeComparison = delegate (XisfFile object1, XisfFile object2)
        {
                if (object1 == null) return 1;
                if (object2 == null) return 1;
                if (object1.mKeywordList.CaptureDateTime > object2.mKeywordList.CaptureDateTime) return 1;
                if (object1.mKeywordList.CaptureDateTime < object2.mKeywordList.CaptureDateTime) return -1;
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