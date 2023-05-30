using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public class XisfFile
    {
        public enum eFrameType { LIGHT, DARK, FLAT, BIAS }
        public enum eFilterType { LUMA, RED, GREEN, BLUE, HA, O3, S2, SHUTTER }


        public DateTime CaptureTime { get; set; }
        public KeywordLists KeywordData { get; set; }
        public bool Master { get; set; } = false;
        public bool NarrowBand { get; set; }
        public bool RiccardiReducer { get; set; }
        public bool Unique { get; set; } = false;
        public double FocuserTemperature { get; set; }
        public int Binning { get; set; }
        public int FocalLength { get; set; }
        public int FocuserPosition { get; set; } = int.MinValue;
        public int Gain { get; set; }
        public int ImageAttachmentLength { get; set; } = 0;
        public int ImageAttachmentStart { get; set; } = 0;
        public int Index { get; set; } = 0;
        public int Offset { get; set; }
        public int ThumbnailAttachmentLength { get; set; } = 0;
        public int ThumbnailAttachmentStart { get; set; } = 0;
        public string CBIAS { get; set; } = string.Empty;
        public string CDARK { get; set; } = string.Empty;
        public string CFLAT { get; set; } = string.Empty;
        public string Camera { get; set; }
        public string CaptureSoftware { get; set; }
        public string Exposure { get; set; }
        public string FileName { get; set; }
        public string Filter { get; set; }
        public string FrameType { get; set; }
        public double RotatorAngle { get; set; } = double.MinValue;
        public string SourceFileName { get; set; }
        public string Target { get; set; }
        public string Telescope { get; set; }
        public double SensorTemperature { get; set; } = -273.0;
        public double AmbientTemperature { get; set; } = -273.0;


        public XisfFile()
        {
            KeywordData = new KeywordLists();
        }

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
            Binning = KeywordData.Binning();
            CBIAS = KeywordData.CBIAS();
            CDARK = KeywordData.CDARK();
            CFLAT = KeywordData.CFLAT();
            Camera = KeywordData.Camera();
            CaptureSoftware = KeywordData.CaptureSoftware();
            CaptureTime = KeywordData.CaptureDateTime();
            Exposure = KeywordData.ExposureSeconds();
            FileName = KeywordData.FileName();
            Filter = KeywordData.FilterName();
            FocalLength = KeywordData.FocalLength();
            FocuserPosition = KeywordData.FocuserPosition();
            FocuserTemperature = KeywordData.FocuserTemperature();
            FrameType = KeywordData.FrameType();
            Gain = KeywordData.Gain();
            Master = KeywordData.TargetName().Contains("Master");
            NarrowBand = KeywordData.FilterName().Contains("Ha") || KeywordData.FilterName().Contains("O3") || KeywordData.FilterName().Contains("S2");
            Offset = KeywordData.Offset();
            RiccardiReducer = KeywordData.Telescope().EndsWith("R");
            RotatorAngle = KeywordData.RotatorAngle();
            Target = KeywordData.TargetName();
            Telescope = KeywordData.Telescope();
            SensorTemperature = KeywordData.SensorTemperature();
            AmbientTemperature = KeywordData.AmbientTemperature();
         }

        public static Comparison<XisfFile> CaptureTimeComparison = delegate (XisfFile object1, XisfFile object2)
        {
                if (object1 == null) return 1;
                if (object2 == null) return 1;
                if (object1.KeywordData.CaptureDateTime() > object2.KeywordData.CaptureDateTime()) return 1;
                if (object1.KeywordData.CaptureDateTime() < object2.KeywordData.CaptureDateTime()) return -1;
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
            return rotationAngle.ToString("+#000.0;-#000.0");
        }
    }
}