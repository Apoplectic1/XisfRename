using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using LocalLib;

namespace XisfRename.Parse
{
    public class UpateXisfFile
    {
        public string NewTarget { get; set; } = string.Empty;
        public string NewSITELAT { get; set; } = string.Empty;
        public string NewSITELON { get; set; } = string.Empty;


        private XDocument mXmlDoc;

        public UpateXisfFile()
        {
        }

        public bool ReWriteXisf(OpenFolderDialog mFolder)
        {
            int numberRead;
            bool bFound;
            byte[] buffer = new byte[(int)1e9];

            foreach (string folder in mFolder.SelectedPaths)
            {
                DirectoryInfo d;
                d = new DirectoryInfo(folder);

                FileInfo[] Files = d.GetFiles("*.xisf");

                foreach (FileInfo file in Files)
                {
                    try
                    {

                        Stream stream = new FileStream(file.FullName, FileMode.Open);
                        BinaryReader bw = new BinaryReader(stream);
                        buffer = bw.ReadBytes((int)1e9);
                        bw.Close();


                        string xmlString = Encoding.UTF8.GetString(buffer, 0, 20000);

                        int length = xmlString.Length;

                        xmlString = xmlString.Substring(xmlString.IndexOf("<?xml");
                        xmlString = xmlString.Substring(0, xmlString.IndexOf(@"</xisf>") + 7);
                        length = xmlString.Length;

                        string newTargetString = TargetName(xmlString);






                        WriteBinaryFile(file.FullName, buffer);
                    }
                    catch(Exception e)
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        // <FITSKeyword name="OBJECT" value="'NGC5457 '" comment="Object name"/>
        public string TargetName(string xmlString)
        {
            if (NewTarget == string.Empty) return xmlString;


            int objectIndex = xmlString.IndexOf("OBJECT");
            var pattern = "'(.*)?'";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            string temp = xmlString.Substring(objectIndex + 8);
            temp = temp.Substring(0, temp.IndexOf(" comment=\"Object "));
            string replacement = regex.Replace(temp, "'" + NewTarget + " '");

            string xmlHead = xmlString.Substring(0, objectIndex + 7) + " " + replacement + " comment=\"Object name\"/>";

            string xmlTail = xmlString.Substring(objectIndex, xmlString.Length - objectIndex);

            int nextElement = xmlTail.IndexOf(@"<");
            
            string xmlEnd = xmlString.Substring(objectIndex + nextElement, xmlString.IndexOf(@"</xisf>") + 7 - (objectIndex + nextElement));

            return xmlHead + xmlEnd;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************



        // ****************************************************************************************************
        // ****************************************************************************************************

        public void SiteLat(XElement element)
        {
            if (NewSITELAT == string.Empty) return;

            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("SITELAT"))
            {
                //element.Attribute("value").Remove();
                element.Attribute("value").SetValue(NewSITELAT);
            }
        }

        // ****************************************************************************************************
        // ****************************************************************************************************



        // ****************************************************************************************************
        // ****************************************************************************************************

        public void SiteLon(XElement element)
        {
            if (NewSITELON == string.Empty) return;

            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("SITELON"))
            {
                //element.Attribute("value").Remove();
                element.Attribute("value").SetValue(NewSITELON);
            }
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        
       
        // ##############################################################################################################################################
        // ##############################################################################################################################################

        private bool WriteBinaryFile(string fileName, byte[] buffer)
        {
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(stream);

                foreach (var b in buffer)
                {
                    bw.Write(b);
                }

                bw.Flush();
                bw.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
