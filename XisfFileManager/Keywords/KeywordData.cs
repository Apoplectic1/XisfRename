using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace XisfFileManager.Keywords
{
    class KeywordData
    {
        public List<Keyword> KeywordList;

        public KeywordData()
        {
            KeywordList = new List<Keyword>();
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void RepairSiteLatitude()
        {
            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "SITELAT");

            foreach (Keyword node in nodeList)
            {
                string SITELAT = node.GetValue<string>();

                if (SITELAT.Contains("N"))
                {
                    SITELAT = Regex.Replace(SITELAT, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");
                }

                node.Type = Keyword.KeywordType.STRING;
                node.SetValue = SITELAT;
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void RepairSiteLongitude()
        {
            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "SITELONG");

            foreach (Keyword node in nodeList)
            {
                string SITELONG = node.GetValue<string>();

                if (SITELONG.Contains("W"))
                {
                    SITELONG = Regex.Replace(SITELONG, "([a-zA-Z,_ ]+|(?<=[a-zA-Z ])[/-])", " ");

                    Regex regReplace = new Regex("'");

                    SITELONG = regReplace.Replace(SITELONG, "'-", 1);
                }

                node.Type = Keyword.KeywordType.STRING;
                node.SetValue = SITELONG;
            }
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public void RepairTargetName(string targetName)
        {
            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "OBJECT");

            foreach (Keyword node in nodeList)
            {
                node.Type = Keyword.KeywordType.STRING;
                node.SetValue = targetName;
            }
        }

        // #########################################################################################################
        // #########################################################################################################
        public string ImageAngle()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "POSANGLE");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return String.Format("{0:000.0}", Convert.ToDouble(value));
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string CaptureSoftware()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "CREATOR");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

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
        public string LocalImageDateTime()
        {
            string value = string.Empty;

            if (CaptureSoftware() == "SGP")
            {
                List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "DATE-LOC");

                foreach (Keyword node in nodeList)
                {
                    value = node.GetValue<string>();
                }
            }

            return DateTime.ParseExact(value.Replace('T', ' '), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString().Replace(':', '-');
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ExposureSeconds()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "EXPOSURE");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return Convert.ToInt32(value).ToString("D4");
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FilterName()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "FILTER");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

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

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "FOCALLEN");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            if (value == "525")
            {
                return "A107R@525 ";
            }

            if (value == "700")
            {
                return "A107@700 ";
            }

            return string.Empty;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocusPosition()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "FOCPOS");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
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

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FocusTemperature()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "FOCPOS");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return FormatTemperatureString(value);
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Camera()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "INSTRUME");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

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
        public string Gain()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "GAIN");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string TargetName()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "OBJECT");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Offset()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "OFFSET");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string SSWeight()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "SSWEIGHT");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string ImageLocation()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "SITENAME");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string Binning()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "XBINNING");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

            return value;
        }

        // *********************************************************************************************************
        // *********************************************************************************************************
        public string FrameType()
        {
            string value = string.Empty;

            List<Keyword> nodeList = (List<Keyword>)KeywordList.Where(i => i.Name == "IMAGETYP");

            foreach (Keyword node in nodeList)
            {
                value = node.GetValue<string>();
            }

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
        public void AddFWHM(string fwhm)
        {
            KeywordList.RemoveAll(i => i.Name == "FWHM");

            Keyword keyword = new Keyword();
            keyword.Name = "FWHM";
            keyword.SetValue = fwhm;
            keyword.Type = Keyword.KeywordType.FLOAT;
            keyword.Comment = "SubFrameSelector Value";

            KeywordList.Add(keyword);
        }
    }
}
