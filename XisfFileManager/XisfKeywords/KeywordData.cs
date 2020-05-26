using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace XisfFileManager.XisfKeywords
{
    public class KeywordData 
    {
        public List<XisfKeywords.Keyword> KeywordList;

        public KeywordData()
        {
            KeywordList = new List<Keyword>();
            KeywordList.Clear();
        }

        // #########################################################################################################
        // #########################################################################################################

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLatitude()
        {
            string value;

            Keyword node = KeywordList.Find(i => i.Name == "SITELAT");
            value = node.Value;

            if (value.Contains("N"))
            {
                value = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");
            }

            node.Type = Keyword.EType.STRING;
            node.Value = value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        // An older version of SGP caused PixInsight to complain - this has been fixed and this method is not needed
        public void RepairSiteLongitude()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "SITELONG");
            value = node.Value;

            if (value.Contains("W"))
            {
                value = Regex.Replace(value, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                Regex regReplace = new Regex("'");

                value = regReplace.Replace(value, "'-", 1);
            }

            node.Type = Keyword.EType.STRING;
            node.Value = value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void RepairTargetName(string targetName)
        {
            Keyword node = KeywordList.Find(i => i.Name == "OBJECT");
            node.Type = Keyword.EType.STRING;
            node.Value = targetName;
        }

        // #########################################################################################################
        // #########################################################################################################

        public string AmbientTemperature()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "AOCAMBT");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "TEMPERAT");

            if (node == null)
                node = KeywordList.Find(i => i.Name == "AMB-TEMP");

            node.Type = Keyword.EType.FLOAT;
            value = node.Value;

            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Binning()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "XBINNING");
            value = node.Value;
            node.Type = Keyword.EType.STRING;

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Camera()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "INSTRUME");
            value = node.Value;
            node.Type = Keyword.EType.STRING;

            if (value.Contains("183"))
            {
                return "Z183";
            }

            if (value.Contains("QHY"))
            {
                return "Q178";
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public DateTime CaptureDateTime()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "DATE-LOC");
            value = node.Value;
            node.Type = Keyword.EType.STRING;

            if (value.IndexOf(".") > 0)
                value = value.Replace("T", "  ").Replace("'", "").Remove(value.IndexOf('.')).Replace(".", "");
            else
                value = value.Replace("T", "  ").Replace("'", "");


            return DateTime.ParseExact(value, "yyyy-MM-dd  HH:mm:ss", CultureInfo.InvariantCulture);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CaptureSoftware()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "CREATOR");
            value = node.Value;
            node.Type = Keyword.EType.STRING;

            if (value.Contains("Sequence"))
            {
                return "SGP";
            }

            if (value.Contains("SkyX"))
            {
                return "TSX";
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ExposureSeconds()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "EXPOSURE");
            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.FLOAT;

            return Convert.ToInt32(value).ToString("D4");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FilterName()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "FILTER");
            node.Type = Keyword.EType.STRING;
            value = node.Value.Replace("'", "");

            if (value.Contains("OIII")) value = value.Replace("OIII", "O3");
            if (value.Contains("Oiii")) value = value.Replace("Oiii", "O3");
            if (value.Contains("SII")) value = value.Replace("SII", "S2");
            if (value.Contains("Sii")) value = value.Replace("Sii", "S2");
            if (value.Contains("HA")) value = value.Replace("HA", "Ha");
            if (value.Contains("Ha")) value = value.Replace("Ha", "Ha");

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocalLength()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "FOCALLEN");
            value = node.Value;
            node.Type = Keyword.EType.INTEGER;

            if (value == "525")
            {
                return "A107R@525";
            }

            if (value == "700")
            {
                return "A107@700";
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocusPosition()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "FOCPOS");
            node.Type = Keyword.EType.INTEGER;
            value = node.Value;


            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocusTemperature()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "FOCTEMP");
            node.Type = Keyword.EType.FLOAT;
            value = node.Value;

            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FrameType()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "IMAGETYP");
            node.Type = Keyword.EType.STRING;
            value = node.Value;


            if (value.IndexOf("Light", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "L";
            }

            if (value.IndexOf("Flat", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "F";
            }

            if (value.IndexOf("Dark", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "Dark";
            }

            if (value.IndexOf("Bias", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "B";
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Gain()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "GAIN");
            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ImageAngle()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "POSANGLE");
            value = node.Value;
            node.Type = Keyword.EType.FLOAT;

            return String.Format("{0:000.0}", Convert.ToDouble(value));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ImageLocation()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "SITENAME");
            value = node.Value;
            node.Type = Keyword.EType.STRING;

            return value.Replace("'", "");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ImageDateTime()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "DATE-LOC");
            node.Type = Keyword.EType.STRING;
            value = node.Value;


            if (value.IndexOf(".") > 0)
                value = value.Replace("T", "  ").Replace("'", "").Remove(value.IndexOf('.')).Replace(".", "").Replace(':', '.');
            else 
                value = value.Replace("T", "  ").Replace("'", "").Replace(':', '.');
            
            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Index()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "INDEX");
            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
        }
        
        // *********************************************************************************************************
        // *********************************************************************************************************
        public int Offset()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "OFFSET");
            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.INTEGER;

            return Convert.ToInt32(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SensorTemperature()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "CCD-TEMP");
            node.Type = Keyword.EType.FLOAT;
            value = node.Value;

            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public int SSWeight()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "SSWEIGHT");

            if (node == null) return int.MinValue;

            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.INTEGER;


            return Convert.ToInt32(Math.Round(Convert.ToDecimal(value), 0, MidpointRounding.AwayFromZero));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string TargetName()
        {
            string value = string.Empty;

            Keyword node = KeywordList.Find(i => i.Name == "OBJECT");
            value = node.Value.Replace("'", "");
            node.Type = Keyword.EType.STRING;

            return value;
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, string value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value;
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.STRING;
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, double value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString("F6");
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.FLOAT;
            KeywordList.Add(keyword);
        }
        // #########################################################################################################
        // #########################################################################################################
        public void AddKeyword(string name, int value, string comment = "XISF File Manager")
        {
            KeywordList.RemoveAll(i => i.Name == name);

            Keyword keyword = new Keyword();
            keyword.Name = name;
            keyword.Value = value.ToString();
            keyword.Comment = comment;
            keyword.Type = Keyword.EType.INTEGER;
            KeywordList.Add(keyword);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void AddKeyword(XElement element)
        {
            Keyword keyword = new Keyword();
            keyword.Name = element.Attribute("name").Value;
            keyword.Value = element.Attribute("value").Value;
            keyword.Comment = element.Attribute("comment").Value;
            keyword.Type = Keyword.EType.COPY;
            KeywordList.Add(keyword);
        }

        // #########################################################################################################
        // #########################################################################################################
        public void RemoveKeyword(string name)
        {
            KeywordList.RemoveAll(i => i.Name == name);
        }

        // #########################################################################################################
        // #########################################################################################################
        private string FormatTemperatureString(string temperatureString)
        {
            if (temperatureString == "") return "";

            double temperature;
            temperature = Convert.ToDouble(temperatureString);
            temperature = Math.Round(temperature, 1); // (temperature > -0.1 && (temperature <= 0.0)) ? 0 : temperature;

            string fmt = "{00:+00.0;-00.0;+00.0}";
            string value = string.Format(fmt, temperature);

            return value;
        }

        // #########################################################################################################
        // #########################################################################################################
    }
}
