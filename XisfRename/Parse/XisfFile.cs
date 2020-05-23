using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XisfRename.Parse
{
    public class XisfFile
    {
        private XDocument mXmlDoc;
        private char[] mBuffer;

        public List<FitsKeyword> KeywordList;

        public string AmbientTemp { get; set; } = string.Empty;
        public string Angle { get; set; } = string.Empty;
        public string Camera { get; set; } = string.Empty;
        public string DateLoc { get; set; } = string.Empty;
        public string Exposure { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string FocalLen { get; set; } = string.Empty;
        public string FocusPos { get; set; } = string.Empty;
        public string FocusTemp { get; set; } = string.Empty;
        public string Gain { get; set; } = string.Empty;
        public string OffSet { get; set; } = string.Empty;
        public string Profile { get; set; } = string.Empty;
        public string SSWEIGHT { get; set; } = string.Empty;
        public string SensorTemp { get; set; } = string.Empty;
        public string SiteName { get; set; } = string.Empty;
        public string Software { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Xbin { get; set; } = string.Empty;
        public string mXmlString;
        public DateTime CaptureDateTime { get; set; }
        public List<string> TargetNameList = new List<string>();
        public bool Unique { get; set; } = false;
        public bool ValidFile { get; set; } = false;
        public int ImageAttachmentStart = 0;
        public int ImageAttachmentLength = 0;
        public int ThumbnailAttachmentStart = 0;
        public int ThumbnailAttachmentLength = 0;
        public string SITELAT { get; set; } = string.Empty;
        public string SITELONG { get; set; } = string.Empty;
        public string SourceFileName { get; set; }


        public XisfFile()
        {
            TargetNameList.Clear();
            mBuffer = new char[30000];
            KeywordList = new List<FitsKeyword>();
        }

        public string FormatTemperatureString(string temperatureString)
        {
            if (temperatureString == "") return "";

            double temperature;
            temperature = Convert.ToDouble(temperatureString);
            temperature = Math.Round(temperature, 1); // (temperature > -0.1 && (temperature <= 0.0)) ? 0 : temperature;

            string fmt = "{00:+00.0;-00.0;+00.0}";
            string value = string.Format(fmt, temperature);

            return value;
        }

        // *****************************************************************
        public bool CaptureSoftware(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("SWCREATE"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();

                if (x.Contains("TheSkyX"))
                {
                    Software = "TheSkyX";
                    return true;
                }

                if (x.Contains("Sequence"))
                {
                    Software = "SGP";
                    return true;
                }
            }

            if (attribute.ToString().Contains("CREATOR"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();

                if (x.Contains("TheSkyX"))
                {
                    Software = "TheSkyX";
                    return true;
                }

                if (x.Contains("Sequence"))
                {
                    Software = "SGP";
                    return true;
                }
            }

            if (attribute.ToString().Contains("PROGRAM"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();

                if (x.Contains("TheSkyX"))
                {
                    Software = "TheSkyX";
                    return true;
                }

                if (x.Contains("Sequence"))
                {
                    Software = "SGP";
                    return true;
                }

                if (x.Contains("PixInsight"))
                {
                    Software = "PI";
                    //return true;
                }
            }

            Software = "SGP";
            return false;
        }
        public string CaptureSoftware()
        {
            return Software;
        }

        // *****************************************************************
        public void AmbientTemperature(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("AMB-TEMP"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                AmbientTemp = x.Substring(x.IndexOf("\"") + 1);
                AmbientTemp = AmbientTemp.Substring(0, AmbientTemp.IndexOf("\""));
            }

            if (attribute.ToString().Contains("AOCAMBT"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                AmbientTemp = x.Substring(x.IndexOf("\"") + 1);
                AmbientTemp = AmbientTemp.Substring(0, AmbientTemp.IndexOf("\""));
            }
        }
        public string AmbientTemperature()
        {
            return FormatTemperatureString(AmbientTemp);
        }

        // *****************************************************************
        public void ImageAngle(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("ANGLE") && !attribute.ToString().Contains("POSANGLE"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                Angle = x.Substring(x.IndexOf("\"") + 1);
                Angle = Angle.Substring(0, Angle.IndexOf("\""));

            }
        }
        public string ImageAngle()
        {
            if (Angle == string.Empty)
                return string.Empty;

            double tmps;
            tmps = Convert.ToDouble(Angle);
            return String.Format("{0:000.0}", tmps);
        }

        // *****************************************************************
        public void SensorTemperature(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("CCD-TEMP"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                SensorTemp = x.Substring(x.IndexOf("\"") + 1);
                SensorTemp = SensorTemp.Substring(0, SensorTemp.IndexOf("\""));
            }
        }
        public string SensorTemperature()
        {
            return FormatTemperatureString(SensorTemp);
        }

        // *****************************************************************
        public void ImageDateTime(XElement element)
        {
            XAttribute attribute = element.Attribute("name");
            string x;

            if (Software == "SGP")
            {
                if (attribute.ToString().Contains("DATE-LOC"))
                {
                    attribute = element.Attribute("value");
                    x = attribute.ToString();
                    x = x.Replace("'", "");
                    x = x.Replace("\"", "");
                    x = x.Replace("T", " ");
                    x = x.Replace("value=", "");

                    if (x.IndexOf('.') > 0)
                        DateLoc = x.Remove(x.IndexOf('.'));
                    else
                        DateLoc = x;

                    CaptureDateTime = DateTime.ParseExact(DateLoc, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }

            if (Software == "TheSkyX")
            {
                if (attribute.ToString().Contains("DATE-OBS"))
                {
                    attribute = element.Attribute("value");
                    x = attribute.ToString();
                    x = x.Replace("'", "");
                    x = x.Replace("\"", "");
                    x = x.Replace("T", " ");
                    x = x.Replace("value=", "");
                    x = x.Substring(0, x.IndexOf(".")); // TheSkyX inlcudes milliseconds

                    DateLoc = x.Replace(":", "-");
                    CaptureDateTime = DateTime.ParseExact(DateLoc, "yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture);
                }
            }
        }
        public string ImageDateTime()
        {
            return DateLoc.Replace(':', '-');
        }

        // *****************************************************************
        public void ExposureSeconds(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("EXPOSURE") || attribute.ToString().Contains("EXPTIME"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                Exposure = x.Substring(x.IndexOf("\"") + 1);
                Exposure = Exposure.Substring(0, Exposure.IndexOf("\""));
                Exposure = Exposure.Replace(".", "");
            }
        }
        public string ExposureSeconds()
        {
            return Convert.ToInt32(Exposure).ToString("D4");
        }

        // *****************************************************************
        public void FilterName(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("FILTER"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();

                if (x.Contains("OIII")) x = x.Replace("OIII", "O3");
                if (x.Contains("Oiii")) x = x.Replace("Oiii", "O3");
                if (x.Contains("SII")) x = x.Replace("SII", "S2");
                if (x.Contains("Sii")) x = x.Replace("Sii", "S2");
                if (x.Contains("HA")) x = x.Replace("HA", "Ha");
                if (x.Contains("Ha")) x = x.Replace("Ha", "Ha");

                Filter = x.Substring(x.IndexOf("'") + 1);
                Filter = Filter.Substring(0, Filter.IndexOf("'"));
                Filter = Filter.Trim();
            }
        }
        public string FilterName()
        {
            return Filter;
        }

        // *****************************************************************
        public void FocalLength(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("FOCALLEN"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                FocalLen = x.Substring(x.IndexOf("\"") + 1);
                FocalLen = FocalLen.Substring(0, FocalLen.IndexOf("\""));
                FocalLen = FocalLen.Replace(".", "");
            }
        }
        public string FocalLength()
        {
            if (FocalLen == "525")
            {
                return "A107R@525 ";
            }

            if (FocalLen == "700")
            {
                return "A107@700 ";
            }

            return string.Empty;
        }

        // *****************************************************************
        public void FocusPosition(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("FOCPOS"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                FocusPos = x.Substring(x.IndexOf("\"") + 1);
                FocusPos = FocusPos.Substring(0, FocusPos.IndexOf("\""));
                FocusPos = FocusPos.Replace(".", "");
                FocusPos = FocusPos.Trim();

            }
        }
        public string FocusPosition()
        {
            return FocusPos;
        }


        // *****************************************************************
        public void FocusTemperature(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("FOCTEMP"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                FocusTemp = x.Substring(x.IndexOf("\"") + 1);
                FocusTemp = FocusTemp.Substring(0, FocusTemp.IndexOf("\""));
            }
        }
        public string FocusTemperature()
        {
            return FormatTemperatureString(FocusTemp);
        }


        // *****************************************************************
        public void CameraModel(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (Software == "SGP")
            {
                if (attribute.ToString().Contains("INSTRU"))
                {
                    attribute = element.Attribute("value");

                    if (attribute.ToString().Contains("183") || attribute.ToString().Contains("ASI"))
                    {
                        Camera = "Z183";
                    }

                    if (attribute.ToString().Contains("QHY"))
                    {
                        Camera = "Q178";
                    }
                }
            }

            if (Software == "TheSkyX")
            {
                if (attribute.ToString().Contains("ASI183"))
                {
                    Camera = "Z183";
                }

                if (attribute.ToString().Contains("QHY"))
                {
                    Camera = "Q178";
                }
            }
        }
        public string CameraModel()
        {
            return Camera;
        }

        // *****************************************************************
        public void CameraGain(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (Software == "SGP")
            {
                if (attribute.ToString().Contains("GAIN") && !attribute.ToString().Contains("EGAIN"))
                {
                    attribute = element.Attribute("value");

                    string x = attribute.ToString();
                    Gain = x.Substring(x.IndexOf("\"") + 1);
                    Gain = Gain.Substring(0, Gain.IndexOf("\""));
                    Gain = Gain.Replace(".", "");
                    Gain = Gain.Trim();
                }
            }

            if (Software == "TheSkyX")
            {
                if (attribute.ToString().Contains("GAINRAW"))
                {
                    attribute = element.Attribute("value");

                    string x = attribute.ToString();
                    Gain = x.Substring(x.IndexOf("\"") + 1);
                    Gain = Gain.Substring(0, Gain.IndexOf("\""));
                    Gain = Gain.Replace(".", "");
                    Gain = Gain.Trim();
                }
            }
        }
        public string CameraGain()
        {
            return Gain;
        }

        // *****************************************************************
        public void TargetName(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("OBJECT"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                Target = x.Substring(x.IndexOf("'") + 1);
                Target = Target.Substring(0, Target.IndexOf("'"));
                Target = Target.Replace('/', '-');
                Target = Target.Replace("flats", "Flat");
                Target = Target.Trim();

                TargetNameList.Add(Target);
            }
        }

        public string TargetName()
        {
            return Target;
        }

        // *****************************************************************
        public void CameraOffset(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("OFFSET"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                OffSet = x.Substring(x.IndexOf("\"") + 1);
                OffSet = OffSet.Substring(0, OffSet.IndexOf("\""));
                OffSet = OffSet.Replace(".", "");
                OffSet = OffSet.Trim();
            }
        }
        public string CameraOffset()
        {
            return OffSet;
        }

        // *****************************************************************
        public void SgpProfile(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("PROFILE"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                Profile = x.Substring(x.IndexOf("'") + 1);
                Profile = Profile.Substring(0, Profile.IndexOf("'"));
            }
        }
        public string SgpProfile()
        {
            return Profile;
        }

        public void SubFrameSelectorWeight(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("SSWEIGH"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                SSWEIGHT = x.Substring(x.IndexOf("\"") + 1);
                SSWEIGHT = SSWEIGHT.Substring(0, SSWEIGHT.IndexOf("\""));

                double weight = Convert.ToDouble(SSWEIGHT) * 10;
                SSWEIGHT = weight.ToString("F0");
            }
        }
        public string SubFrameSelectorWeight()
        {
            return SSWEIGHT;
        }


        public void ImageLocation(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("SITENAME"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                SiteName = x.Substring(x.IndexOf("'") + 1);
                SiteName = SiteName.Substring(0, SiteName.IndexOf("'"));
            }
        }
        public string ImageLocation()
        {
            return SiteName;
        }


        public void Binning(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("XBINNING"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                Xbin = x.Substring(x.IndexOf("\"") + 1);
                Xbin = Xbin.Substring(0, Xbin.IndexOf("\""));
                Xbin = Xbin.Replace(".", "");
                Xbin = Xbin.Trim();
            }
        }
        public string Binning()
        {
            return Xbin;
        }



        public void FrameType(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("IMAGETYP"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                Type = x.Substring(x.IndexOf("'") + 1);
                Type = Type.Substring(0, Type.IndexOf("'"));
                Type = Type.Trim();
            }
        }
        public string FrameType()
        {
            if (Type.IndexOf("Light", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "L";
            }

            if (Type.IndexOf("Flat", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "F";
            }

            if (Type.IndexOf("Dark", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Dark";
            }

            if (Type.IndexOf("Bias", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "B";
            }

            return string.Empty;
        }

        public void AddSiteLat(XElement element)
        {
            if (element.Attribute("name").Value.Equals("SITELAT"))
            {
                FitsKeyword keyword = new FitsKeyword();
                keyword.Type = FitsKeyword.KeywordType.STRING;
                keyword.Name = keyword.Name = element.Attribute("name").Value;

                string SITELAT = element.Attribute("value").Value;

                if (SITELAT.Contains("N"))
                {
                    SITELAT = Regex.Replace(SITELAT, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");
                }

                keyword.SetValue = SITELAT;

                keyword.Comment = element.Attribute("comment").Value;
                KeywordList.Add(keyword);
            }
        }


        public void AddSiteLong(XElement element)
        {
            if (element.Attribute("name").Value.Equals("SITELONG"))
            {
                FitsKeyword keyword = new FitsKeyword();
                keyword.Type = FitsKeyword.KeywordType.STRING;
                keyword.Name = keyword.Name = element.Attribute("name").Value;

                string SITELONG = element.Attribute("value").Value;

                if (SITELONG.Contains("W"))
                {
                    SITELONG = Regex.Replace(SITELONG, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                    Regex regReplace = new Regex("'");

                    SITELONG = regReplace.Replace(SITELONG, "'-", 1);
                }

                keyword.SetValue = SITELONG;

                keyword.Comment = element.Attribute("comment").Value;
                KeywordList.Add(keyword);
            }
        }


        // ************************************************************************************************
        // ************************************************************************************************
        public void AddFWHM(XElement element)
        {
            if (element.Attribute("name").Value.Equals("FWHM"))
            {
                FitsKeyword keyword = new FitsKeyword();
                keyword.Type = FitsKeyword.KeywordType.FLOAT;
                keyword.Name = element.Attribute("name").Value;
                keyword.SetValue = element.Attribute("value").Value;
                keyword.Comment = element.Attribute("comment").Value;
                KeywordList.Add(keyword);
            }
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

        public bool Parse()
        {
            bool bFound;

            using (StreamReader reader = new StreamReader(SourceFileName))
            {
                reader.Read(mBuffer, 0, mBuffer.Length);
            }

            mXmlString = new string(mBuffer);

            mXmlString = mXmlString.Substring(mXmlString.IndexOf("<?xml"));
            mXmlString = mXmlString.Substring(0, mXmlString.LastIndexOf(@"</xisf>") + 7);

            try
            {
                mXmlDoc = XDocument.Parse(mXmlString);
            }
            catch
            {
                ValidFile = false;
                return false;
            }

            XElement root = mXmlDoc.Root;
            XNamespace ns = root.GetDefaultNamespace();

            IEnumerable<XElement> image = from c in mXmlDoc.Descendants(ns + "Image") select c;
            foreach (XElement element in image)
            {
                ImageAttachment(element);
            }


            IEnumerable<XElement> thumbnail = from c in mXmlDoc.Descendants(ns + "Thumbnail") select c;
            foreach (XElement element in thumbnail)
            {
                ThumbnailAttachment(element);
            }

            IEnumerable<XElement> elements = from c in mXmlDoc.Descendants(ns + "FITSKeyword") select c;

            // Advance through stream until we get to FITSKeyword elements
            foreach (XElement element in elements)
            {
                bFound = CaptureSoftware(element);
                if (bFound)
                    break;
            }

            // Find each relevent keyword and add it to mFile
            foreach (XElement element in elements)
            {
                AddFITSKeyword(element);
            }

            ValidFile = true;
            return true;
        }

        public void AddFITSKeyword(XElement element)
        {
            FitsKeyword keyword = new FitsKeyword();
            keyword.Type = FitsKeyword.KeywordType.COPY;
            keyword.Name = element.Attribute("name").Value;
            keyword.SetValue = element.Attribute("value").Value;
            keyword.Comment = element.Attribute("comment").Value;
            KeywordList.Add(keyword);
        }
    }
}

