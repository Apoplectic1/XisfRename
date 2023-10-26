using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace XisfFileManager.FileOperations
{
    public class XisfFileReader
    {
        private byte[] mBuffer;
        private int mBufferSize;
        private int bytesRead;
        private Match keywordBlock;
        private string keywordMatch = @"<xisf.*?</xisf>";
        private string modifiedString;
       
        public Task ReadXisfFile(XisfFile xFile)
        {
            using (FileStream fileStream = new FileStream(xFile.FilePath, FileMode.Open, FileAccess.Read))
            {
                mBufferSize = 10000;
                mBuffer = new byte[mBufferSize];
                keywordBlock = Match.Empty;
                bytesRead = 0;


                // Skip first sixteen bytes that contain XISF0100xxxxxxxx with keywordMatch = @"<xisf.*?</xisf>";

                // If the xml section is larger than mBufferSize, repeatedly double the buffer size and reread
                while (!keywordBlock.Success)
                {
                    bytesRead = fileStream.Read(mBuffer, bytesRead, mBufferSize - bytesRead);
                    keywordBlock = Regex.Match(Encoding.UTF8.GetString(mBuffer), keywordMatch, RegexOptions.Singleline);

                    if (!keywordBlock.Success)
                    {
                        if (bytesRead == mBufferSize)
                        {
                            // Expand the buffer size if it has been filled
                            mBufferSize += mBufferSize;
                            Array.Resize(ref mBuffer, mBufferSize);
                        }
                        else
                        {
                            // Process the bytes read and exit the loop
                            modifiedString = Encoding.UTF8.GetString(mBuffer, 0, bytesRead);
                        }
                    }
                }

                modifiedString = keywordBlock.ToString().Replace("'", "");

                xFile.mXDoc = new XDocument();

                try
                {
                   // MatchCollection matches = Regex.Matches(modifiedString, "<xisf(.*?)xisf>");

                    //modifiedString = string.Empty;
                    //foreach (Match match in matches)
                   //{
                    //    modifiedString += match.Value;
                   // }

                        xFile.mXDoc = XDocument.Parse(modifiedString);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show("Could not parse xml in file:\n\n" + xFile.FilePath +
                        "\n\nXisfRead.cs ReadXisfFile() ->\n\tmXDoc = XDocument.Parse(sXmlString)\n\n" + ex.Message,
                        "Parse XISF File",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Error);

                    Application.Exit();
                    return Task.CompletedTask;
                }

                XElement root = xFile.mXDoc.Root;
                XNamespace ns = root.GetDefaultNamespace();

                IEnumerable < XElement> image = xFile.mXDoc.Descendants(ns + "Image");
                foreach (XElement element in image)
                {
                    xFile.ImageAttachment(element);
                }

                IEnumerable<XElement> thumbnail = xFile.mXDoc.Descendants(ns + "Thumbnail");
                foreach (XElement element in thumbnail)
                {
                    xFile.ThumbnailAttachment(element);
                }

                // This will place all XML formated FITS Keyword Name, Value, Comment triples into 'elements' so that 'elements' has an IEnumerable list of each set of keyword triples
                IEnumerable<XElement> elements = xFile.mXDoc.Descendants(ns + "FITSKeyword");

                // Look at each XML FITS keyword triple and add it to this file's Keyword set 
                foreach (XElement element in elements)
                {
                    xFile.AddXMLKeyword(element);
                }

                //SetKeywordsFromFileName(xFile);
            }

            return Task.CompletedTask;
        }

        public void SetKeywordsFromFileName(XisfFile xFile)
        {
            string sFileName = Path.GetFileName(xFile.FilePath);

            if (sFileName != null)
            {
                // Split the string into fields using whitespace as the delimiter
                string[] fields = sFileName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Assign Master, FrameType, Filter and Date without hours, minutes, seconds
                string Master = fields[0];

                if (Master != "Master")
                    return;

                string FrameType = fields[1];

                if (FrameType == "Flat")
                {
                    string Filter = fields[2];
                    DateTime Date = DateTime.ParseExact(fields[3], "yyyy-MM-dd", null);

                    // Split fields[4] into Exposure, Binning, and Frames using 'x' as the separator
                    string[] exposureParts = fields[4].Split(new char[] { 'x' });
                    double Exposure = double.Parse(exposureParts[0]);
                    int Binning = int.Parse(exposureParts[1]);
                    int Frames = int.Parse(exposureParts[2]);

                    // Extract Camera, Gain, Offset, and SensorTemp from fields[5]
                    string Camera = fields[5].Substring(0, 4);
                    int gainStartIndex = fields[5].IndexOf('G') + 1;
                    int gainEndIndex = fields[5].IndexOf('O');
                    int Gain = int.Parse(fields[5].Substring(gainStartIndex, gainEndIndex - gainStartIndex));
                    int offsetStartIndex = fields[5].IndexOf('O') + 1;
                    int offsetEndIndex = fields[5].IndexOf('@');
                    int Offset = int.Parse(fields[5].Substring(offsetStartIndex, offsetEndIndex - offsetStartIndex));
                    int sensorTempStartIndex = fields[5].IndexOf('@') + 1;
                    int sensorTempEndIndex = fields[5].IndexOf('C');
                    double SensorTemp = double.Parse(fields[5].Substring(sensorTempStartIndex, sensorTempEndIndex - sensorTempStartIndex));

                    // Extract Telescope and FocalLength from fields[6]
                    int telescopeStartIndex = fields[6].IndexOf('@') + 1;
                    string Telescope = fields[6].Substring(0, telescopeStartIndex - 1);
                    int FocalLength = int.Parse(fields[6].Substring(telescopeStartIndex));

                    // Extract Algorithm and Software from the remaining fields
                    string Algorithm = fields[7].Replace("(", "");
                    string Software = fields[8].Substring(0, fields[8].IndexOf(')'));

                    xFile.AddKeyword("OBJECT", Master, "Master Integration Frame");
                    xFile.AddKeyword("IMAGETYP", FrameType, "Type of Master Frame");
                    xFile.AddKeyword("FILTER", Filter, "Filter used");
                    //xFile.AddKeyword("DATE-END", Date, "Local time of capture");
                    //xFile.AddKeyword("DATE-LOC", Date, "Local time of capture");
                    //xFile.AddKeyword("DATE-OBS", Date.ToUniversalTime(), "UTC time of capture");
                    xFile.AddKeyword("EXPTIME", Exposure, "Exposure time in seconds");
                    xFile.AddKeyword("XBINNING", Binning, "Horizontal Binning");
                    xFile.AddKeyword("YBINNING", Binning, "Vertical Binning");
                    xFile.AddKeyword("NUM-FRMS", Frames, "Number of integrated subframes");
                    xFile.AddKeyword("INSTRUME", Camera, "Camera used");
                    xFile.AddKeyword("GAIN", Gain, "Camera gain setting");
                    xFile.AddKeyword("OFFSET", Offset, "Camera offset setting");
                    xFile.AddKeyword("CCD-TEMP", -20.0, "Actual Sensor Temperature");
                    xFile.AddKeyword("TELESCOP", Telescope, "APM107 Super ED with Riccardi 0.75 Reducer");
                    xFile.AddKeyword("FOCALLEN", FocalLength, "APM107 Super ED with Riccardi 0.75 Reducer");
                    xFile.AddKeyword("RJCT-ALG", Algorithm, "PixInsight Statistical Rejection Algorithm used");
                    xFile.AddKeyword("CREATOR", Software, "Software Capture package used to capture subframes");
                }
                if ((FrameType == "Dark") || (FrameType == "Bias"))
                {
                    string Filter = "Shutter";
                    DateTime Date = DateTime.ParseExact(fields[2], "yyyy-MM-dd", null);

                    // Split fields[4] into Exposure, Binning, and Frames using 'x' as the separator
                    string[] exposureParts = fields[3].Split(new char[] { 'x' });
                    double Exposure = double.Parse(exposureParts[0]);
                    int Binning = int.Parse(exposureParts[1]);
                    int Frames = int.Parse(exposureParts[2]);

                    // Extract Camera, Gain, Offset, and SensorTemp from fields[5]
                    string Camera = fields[4].Substring(0, 4);
                    int gainStartIndex = fields[4].IndexOf('G') + 1;
                    int gainEndIndex = fields[4].IndexOf('O');
                    int Gain = int.Parse(fields[4].Substring(gainStartIndex, gainEndIndex - gainStartIndex));
                    int offsetStartIndex = fields[4].IndexOf('O') + 1;
                    int offsetEndIndex = fields[4].IndexOf('@');
                    int Offset = int.Parse(fields[4].Substring(offsetStartIndex, offsetEndIndex - offsetStartIndex));
                    int sensorTempStartIndex = fields[4].IndexOf('@') + 1;
                    int sensorTempEndIndex = fields[4].IndexOf('C');
                    double SensorTemp = double.Parse(fields[4].Substring(sensorTempStartIndex, sensorTempEndIndex - sensorTempStartIndex));

                    string Telescope = "APM107R";
                    int FocalLength = 531;

                    // Extract Algorithm and Software from the remaining fields
                    string Algorithm = fields[5].Replace("(", "");
                    string Software = fields[6].Substring(0, fields[6].IndexOf(')'));

                    xFile.AddKeyword("OBJECT", Master, "Master Integration Frame");
                    xFile.AddKeyword("IMAGETYP", FrameType, "Type of Master Frame");
                    xFile.AddKeyword("FILTER", Filter, "Filter used");
                    //xFile.AddKeyword("DATE-END", Date, "Local time of capture");
                    //xFile.AddKeyword("DATE-LOC", Date, "Local time of capture");
                    //xFile.AddKeyword("DATE-OBS", Date.ToUniversalTime(), "UTC time of capture");
                    xFile.AddKeyword("EXPTIME", Exposure, "Exposure time in seconds");
                    xFile.AddKeyword("XBINNING", Binning, "Horizontal Binning");
                    xFile.AddKeyword("YBINNING", Binning, "Vertical Binning");
                    xFile.AddKeyword("NUM-FRMS", Frames, "Number of integrated subframes");
                    xFile.AddKeyword("INSTRUME", Camera, "Camera used");
                    xFile.AddKeyword("GAIN", Gain, "Camera gain setting");
                    xFile.AddKeyword("OFFSET", Offset, "Camera offset setting");
                    xFile.AddKeyword("CCD-TEMP", -20.0, "Actual Sensor Temperature");
                    xFile.AddKeyword("TELESCOP", Telescope, "APM107 Super ED with Riccardi 0.75 Reducer");
                    xFile.AddKeyword("FOCALLEN", FocalLength, "APM107 Super ED with Riccardi 0.75 Reducer");
                    xFile.AddKeyword("RJCT-ALG", Algorithm, "PixInsight Statistical Rejection Algorithm used");
                    xFile.AddKeyword("CREATOR", Software, "Software Capture package used to capture subframes");
                }
            }
        }
    }
}
