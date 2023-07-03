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
        private KeywordList mKeywordList;

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

        public void RemoveKeyword(string keyword)
        {
           mKeywordList.RemoveKeyword(keyword);
        }
        public double AmbientTemperature
        {
            get { return mKeywordList.AmbientTemperature; }
            set { mKeywordList.AmbientTemperature = value; }
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
        public string FileName { get; set; } = string.Empty;
        public string Filter
        {
            get { return mKeywordList.FilterName; }
            set { mKeywordList.AddKeyword("FILTER", value); }
        }
        public double FocalLength
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
        public int Index { get; set; } = 0;
        public bool Master { get; set; } = false;
        public bool NarrowBand { get; set; } = false;
        public int Offset
        {
            get { return mKeywordList.Offset; }
            set { mKeywordList.Offset = value; }
        }
        public bool RiccardiReducer { get; set; }
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
        public int ThumbnailAttachmentLength { get; set; } = 0;
        public int ThumbnailAttachmentStart { get; set; } = 0;
        public string TargetFilePath
        {
            get { return mKeywordList.TargetFilePath; }
            set { mKeywordList.TargetFilePath = value; }
        }
        public string TargetObjectName
        {
            get { return mKeywordList.TargetObjectName; }
            set { mKeywordList.TargetObjectName = value; }
        }
        public string Telescope
        {
            get { return mKeywordList.Telescope; }
            set { mKeywordList.Telescope = value; }
        }
        public bool Unique { get; set; } = false;

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

        public void SetRequiredKeywords()
        {
            FileName = mKeywordList.FileName;
            Master = mKeywordList.TargetObjectName.Contains("Master");
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