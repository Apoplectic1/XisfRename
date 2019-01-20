using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using XisfRename.Properties;

namespace XisfRename
{
    public partial class Form1 : Form
    {
        List<XisfFile> mFileList = new List<XisfFile>();
        XisfFile mFile;
        private FolderBrowserDialog mFolderBrowserDialog1;
        private string mFolderPath = string.Empty;

        public Form1()
        {
            InitializeComponent();

            mFolderBrowserDialog1 = new FolderBrowserDialog();
        }

        public bool FindXisf()
        {
            if ((Settings.Default.PersistDirectory != null) && (Settings.Default.PersistDirectory != string.Empty))
            {
                mFolderBrowserDialog1.SelectedPath = Settings.Default.PersistDirectory;
            }

            if (mFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                mFolderPath = mFolderBrowserDialog1.SelectedPath;
            }
            else
            {
                return false;
            }

            DirectoryInfo d = new DirectoryInfo(mFolderPath);
            FileInfo[] Files = d.GetFiles("*.xisf");

            ProgressBar_OverAll.Maximum = Files.Count();


            foreach (FileInfo file in Files)
            {
                ProgressBar_OverAll.Value += 1;

                mFile = new XisfFile();

                try
                {
                    char[] buffer = new char[20000];

                    mFile.SourceFileName = file.FullName;

                    using (StreamReader reader = new StreamReader(file.FullName))
                    {
                        reader.Read(buffer, 0, buffer.Length);
                    }

                    string s = new string(buffer);

                    s = s.Substring(s.IndexOf("<?xml"));
                    s = s.Substring(0, s.LastIndexOf(@"</xisf>") + 7);

                    XDocument doc = XDocument.Parse(s);
                    XElement root = doc.Root;
                    XNamespace ns = root.GetDefaultNamespace();
                    IEnumerable<XElement> elements = from c in doc.Descendants(ns + "FITSKeyword") select c;
                    foreach (XElement element in elements)
                    {
                        mFile.CaptureSoftware(element);
                    }

                    foreach (XElement element in elements)
                    {
                        mFile.AmbientTemperature(element);
                        mFile.Binning(element);
                        mFile.CameraModel(element);
                        mFile.CameraGain(element);
                        mFile.CameraOffset(element);
                        mFile.ExposureSeconds(element);
                        mFile.FilterName(element);
                        mFile.FocalLength(element);
                        mFile.FocalPosition(element);
                        mFile.FocalTemperature(element);
                        mFile.FrameType(element);
                        mFile.ImageAngle(element);
                        mFile.ImageDateTime(element);
                        mFile.ImageLocation(element);
                        mFile.SensorTemperature(element);
                        mFile.SgpProfile(element);
                        mFile.SubFrameSelectorWeight(element);
                        mFile.TargetName(element);
                    }
                }
                catch
                {
                    return false;
                }

                mFileList.Add(mFile);

                myList.Sort((x, y) => DateTime.Compare(x.Created, y.Created));
            }

            return true;
        }

        private void Button_Browse_Click(object sender, EventArgs e)
        {
            ProgressBar_OverAll.Value = 0;
            FindXisf();
            ProgressBar_OverAll.Maximum = mFileList.Count();
        }

        private void RenameFiles()
        {
            string newFileName;
            int index = 1;

            string sourceFilePath;

            foreach (XisfFile file in mFileList)
            {
                sourceFilePath = Path.GetDirectoryName(file.SourceFileName);

                if (file.SubFrameSelectorWeight() != string.Empty)
                {
                    int weight = Convert.ToInt32(Convert.ToDouble(file.SubFrameSelectorWeight()) * 10);

                    newFileName = weight.ToString("D3") + " " + file.TargetName() + " " + file.FrameType() + file.FilterName() + " " + file.ExposureSeconds() + "S-" + file.Binning() + "x" + file.Binning() + " G" +
                        file.CameraGain() + "O" + file.CameraOffset() + "@" + file.SensorTemperature() + "C" + " F" + file.FocalPosition() + "@" + file.FocalTemperature() + "C R" + file.ImageAngle() +
                        " (" + file.ImageDateTime() + " " + file.SgpProfile() + ")" + ".xisf";
                }

                else
                {
                    newFileName = index.ToString("D3") + " ";


                    newFileName += file.TargetName() + " ";

                    newFileName += file.FocalLength() + " ";

                    newFileName += file.FrameType() + "-" + file.FilterName() + " ";

                    newFileName += file.ExposureSeconds() + "Sx" + file.Binning() + " ";

                    newFileName += file.CameraModel() + "G" + file.CameraGain() + "O" + file.CameraOffset();

                    newFileName += "@" + file.SensorTemperature() + "C" + " F" + file.FocalPosition() + "@" + file.FocalTemperature() + "C";

                    if (file.ImageAngle() != string.Empty)
                    {
                        newFileName += " R" + file.ImageAngle();
                    }
                    else
                    {
                        newFileName += " ";
                    }

                    newFileName += " (" + file.ImageDateTime() + " ";// + " FL" + file.FocalLength();

                    if (file.CaptureSoftware() == "SGP")
                    {
                        newFileName += "SGP";// + file.SgpProfile();
                    }

                    if (file.CaptureSoftware() == "TheSkyX")
                    {
                        newFileName += "TheSkyX";
                    }

                    newFileName += ")" + ".xisf";
                }
                
                if (IsFileLocked(newFileName))
                {
                    File.Move(file.SourceFileName, sourceFilePath + "\\" + newFileName + "-Duplicate");
                }
                else
                {
                    File.Move(file.SourceFileName, sourceFilePath + "\\" + newFileName);
                }

                index++;
            }
        }

        private void SavePersistedStates(object sender, FormClosingEventArgs e)
        {
            Settings.Default.PersistDirectory = mFolderPath;
            Settings.Default.Save();
        }

        private bool IsFileLocked(string path)
        {
            FileInfo file = new FileInfo(path);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        private void Button_Rename_Click(object sender, EventArgs e)
        {
            RenameFiles();
        }
    }
}
