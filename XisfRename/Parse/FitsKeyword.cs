using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XisfRename.Parse
{
    public class FitsKeyword
    {
        // #################################################################################################
        // #################################################################################################
        public string Fwhm(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Equals("FWHM"))
            {
                attribute = element.Attribute("value");

                return attribute.ToString();
            }
            return string.Empty;
        }
        public XElement Fwhm(double value, string comment)
        {
            XElement FitsKeyword = new XElement("FITSKeyword",
                new XElement("name", "FWHM"),
                new XElement("value", value.ToString("F5")),
                new XElement("comment", comment));

            return FitsKeyword;
        }
        public XElement Fwhm(string value, string comment)
        {
            XElement FitsKeyword = new XElement("FITSKeyword",
                new XElement("name", "FWHM"),
                new XElement("value", value),
                new XElement("comment", comment));

            return FitsKeyword;
        }
        /*public void ReplaceFwhm(string xmlString, string value, string comment)
        {
            XElement newFwhm = Fwhm(value, comment);
            var xml = 
            var existingFwhm = xmlString.Elements("Genre")
                   .Where(e => e.Attribute("Name").Value == "Rock")
                   .Elements("Artist")
                   .Single(e => e.Attribute("Name").Value == "Pink Floyd");
        }
        */
        // #################################################################################################
        // #################################################################################################
    }
}
