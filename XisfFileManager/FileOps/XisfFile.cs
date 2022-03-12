﻿using System;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public class XisfFile
    {
        public enum eFrameType { LIGHT, DARK, FLAT, BIAS }
        public enum eFilterType { LUMA, RED, GREEN, BLUE, HA, O3, S2, SHUTTER }

        public int ImageAttachmentLength { get; set; } = 0;
        public int ImageAttachmentStart { get; set; } = 0;
        public int ThumbnailAttachmentLength { get; set; } = 0;
        public int ThumbnailAttachmentStart { get; set; } = 0;
        public string SourceFileName { get; set; }
        public KeywordLists KeywordData { get; set; }
        public bool Unique { get; set; } = false;
        public int Index { get; set; } = 0;
        

        // List of required keywords for EVERY frame
        // Some of these Keywords will add subsets of additional keywords
        public string Target { get; set; }
        public string Telescope { get; set; }
        public bool RiccardiReducer { get; set; }
        public string CaptureSoftware { get; set; }
        public int FocalLength { get; set; }
        public string Camera { get; set; }
        public string Exposure { get; set; }
        public bool NarrowBand { get; set; }
        public int Gain { get; set; }
        public int Offset { get; set; }
        public string Temperature { get; set; } = string.Empty;
        public int Binning { get; set; }
        public string Filter { get; set; }
        public string FileName { get; set; }
        public string FrameType { get; set; }
        public bool Master { get; set; } = false;
        public DateTime CaptureTime { get; set; }
        public string CDARK { get; set; }
        public string CFLAT { get; set; }
        public string CBIAS { get; set; }
        public bool Protected { get; set; } = false;


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

        public static Comparison<XisfFile> CaptureTimeComparison = delegate (XisfFile object1, XisfFile object2)
        {
            if (object1 == null) return 1;
            if (object2 == null) return 1;
            if (object1.KeywordData.CaptureDateTime() > object2.KeywordData.CaptureDateTime()) return 1;
            if (object1.KeywordData.CaptureDateTime() < object2.KeywordData.CaptureDateTime()) return -1;
            return 0;
        };

        public void ParseRequiredKeywords()
        {
            Binning = KeywordData.Binning();
            Camera = KeywordData.Camera();
            CaptureSoftware = KeywordData.CaptureSoftware();
            CaptureTime = KeywordData.CaptureDateTime();
            Exposure = KeywordData.ExposureSeconds();
            Filter = KeywordData.FilterName();
            FocalLength = KeywordData.FocalLength();
            FrameType = KeywordData.FrameType();
            Gain = KeywordData.Gain();
            Target = KeywordData.TargetName();
            Master = Target.Contains("Master");
            NarrowBand = Filter.Contains("Ha") || Filter.Contains("O3") || Filter.Contains("S2");
            Offset = KeywordData.Offset();
            FileName = KeywordData.FileName();
            Telescope = KeywordData.Telescope();
            RiccardiReducer = Telescope.EndsWith("R");
            Temperature = KeywordData.SensorTemperature();
            Protected = KeywordData.Protected();
        }
    }
}

