using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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


                        string s = Encoding.UTF8.GetString(buffer, 0, 20000);

                        s = s.Substring(s.IndexOf("<?xml"));
                        s = s.Substring(0, s.LastIndexOf(@"</xisf>") + 7);

                       

                        ReplaceObjectFITSKeyword(buffer, NewTarget, s.Length);

                        /*
                        try
                        {
                            mXmlDoc = XDocument.Parse(s);
                        }
                        catch
                        {
                            continue;
                        }

                        XElement root = mXmlDoc.Root;
                        XNamespace ns = root.GetDefaultNamespace();
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
                            TargetName(element);
                            SiteLat(element);
                            SiteLon(element);
                        }

                        bytes = s.Length;
                        */
                        WriteBinaryFile(file.FullName, buffer);
                    }
                    catch
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
        public void TargetName(XElement element)
        {
            if (NewTarget == string.Empty) return;

            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("OBJECT"))
            {
                //element.Attribute("value").Remove();
                element.Attribute("value").SetValue(NewTarget);
            }
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

        // ****************************************************************************************************
        // ****************************************************************************************************

        public bool CaptureSoftware(XElement element)
        {
            XAttribute attribute = element.Attribute("name");

            if (attribute.ToString().Contains("SWCREATE"))
            {
                attribute = element.Attribute("value");

                string x = attribute.ToString();

                if (x.Contains("TheSkyX"))
                {
                    return true;
                }

                if (x.Contains("Sequence"))
                {
                    return true;
                }
            }
            return false;
        }

        // ****************************************************************************************************
        // ****************************************************************************************************

        private void ReplaceObjectFITSKeyword(byte[] buffer, string value, int length)
        {
            string keywordString = @"OBJECT"" value=""'";
            byte[] keywordBytes = Encoding.UTF8.GetBytes(keywordString);
            int bufferIndex;
            int keywordIndex;
            int startInsert;
            int lastQuoteIndex = 0;

            for (bufferIndex = 0; bufferIndex < length; bufferIndex++)
            {
                for (keywordIndex = 0; keywordIndex < Buffer.ByteLength(keywordBytes); keywordIndex++)
                {
                    if (keywordBytes[keywordIndex] != buffer[bufferIndex]) break;

                    if (keywordIndex == keywordString.Length - 1)
                    {
                        // we get here only if we found the keywordString

                        bufferIndex++;
                        // now bufferIndex is pointing to the character AFTER the end of the keywordByte buffer

                        // Now remove the old object name and replace it with the new one starting at the current bufferIndex value

                        startInsert = bufferIndex;

                        // First, find index of closing "'" in buffer. Our new name fits inside 's like: 'NGC754 ' - the space is PixInsight
                        for (int index = bufferIndex; index < bufferIndex + 1000; index++)
                        {
                            if (buffer[index] == 0x27) // A single '
                            {
                                lastQuoteIndex = index;
                                break;
                            }
                        }

                        // How long is our new name?
                        byte[] newName = Encoding.UTF8.GetBytes(NewTarget + " ");
                        int newNameLength = Buffer.ByteLength(newName);

                        // Will our new name fit?
                        if ((lastQuoteIndex - startInsert) >= newNameLength)
                        {
                            // Yes it will
                            for (int index = 0; index < (lastQuoteIndex - startInsert); index++)
                            {
                                if (index < newNameLength)
                                {
                                    // Replace the name
                                    buffer[bufferIndex] = newName[index];
                                }
                                else
                                {
                                    // The new name was short; fill with spaces
                                    buffer[bufferIndex] = 0x20;
                                }
                                bufferIndex++;
                            }

                            return;
                        }
                    }

                    bufferIndex++;
                }
            }
        }

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
