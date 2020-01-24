using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace XisfRename
{
    class XisfFile
    {
        private string AmbTemp { get; set; } = string.Empty;
        private string Angle { get; set; } = string.Empty;
        private string Camera { get; set; } = string.Empty;
        private string CcdTemp { get; set; } = string.Empty;
        private string DateLoc { get; set; } = string.Empty;
        private string Exposure { get; set; } = string.Empty;
        private string Filter { get; set; } = string.Empty;
        private string FocPos { get; set; } = string.Empty;
        private string FocTemp { get; set; } = string.Empty;
        private string FocalLen { get; set; } = string.Empty;
        private string Gain { get; set; } = string.Empty;
        private string OffSet { get; set; } = string.Empty;
        private string Profile { get; set; } = string.Empty;
        private string SSWEIGHT { get; set; } = string.Empty;
        private string SiteName { get; set; } = string.Empty;
        private string Software { get; set; } = string.Empty;
        private string Target { get; set; } = string.Empty;
        private string Type { get; set; } = string.Empty;
        private string Xbin { get; set; } = string.Empty;
        public DateTime CaptureDateTime { get; set; }
        public DirectoryInfo SourceDirectoryInfo;
        public bool Unique { get; set; } = false;
        public string SourceFileName { get; set; }


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
                AmbTemp = x.Substring(x.IndexOf("\"") + 1);
                AmbTemp = AmbTemp.Substring(0, AmbTemp.IndexOf("\""));
            }
        }
        public string AmbientTemperature()
        {
            return AmbTemp;
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
                CcdTemp = x.Substring(x.IndexOf("\"") + 1);
                CcdTemp = CcdTemp.Substring(0, CcdTemp.IndexOf("\""));
            }
        }
        public string SensorTemperature()
        {
            return Convert.ToDouble(CcdTemp).ToString("F1");
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

                    DateLoc = x.Replace(":", "-");
                    CaptureDateTime = DateTime.ParseExact(DateLoc, "yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture);
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
        public void FocalPosition(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("FOCPOS"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                FocPos = x.Substring(x.IndexOf("\"") + 1);
                FocPos = FocPos.Substring(0, FocPos.IndexOf("\""));
                FocPos = FocPos.Replace(".", "");
                FocPos = FocPos.Trim();

            }
        }
        public string FocalPosition()
        {
            return FocPos;
        }

        // *****************************************************************
        public void FocalTemperature(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("FOCTEMP"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                FocTemp= x.Substring(x.IndexOf("\"") + 1);
                FocTemp = FocTemp.Substring(0, FocTemp.IndexOf("\""));
            }

            if (attribute.ToString().Contains("CCD-TEMP"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();
                FocTemp = x.Substring(x.IndexOf("\"") + 1);
                FocTemp = FocTemp.Substring(0, FocTemp.IndexOf("\""));
            }
        }
        public string FocalTemperature()
        {
            double FocuserTemperature;
            FocuserTemperature = Convert.ToDouble(FocTemp);

            FocuserTemperature = (FocuserTemperature > -0.1 && (FocuserTemperature <= 0.0)) ? 0 : FocuserTemperature;
            string fmt = "{0:+0.0;-0.0;+0.0}";
            string value = string.Format(fmt, FocuserTemperature);

            return value;
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

    }
}
