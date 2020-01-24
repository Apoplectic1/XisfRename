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
    public partial class Main : Form
    {
        List<XisfFile> mFileList = new List<XisfFile>();
        XisfFile mFile;
        private FolderBrowserDialog mFolderBrowserDialog1;
        private string mFolderPath = string.Empty;
        int mDupIndex = 1;
        private XDocument mXmlDoc;
        bool mBChronological = false;
        bool mBSsWeight = true;
        bool mKeepIndex = true;
        bool mWeightFirst = true;
        bool mIndexFirst = false;

        public Main()
        {
            InitializeComponent();

            mFolderBrowserDialog1 = new FolderBrowserDialog
            {
                SelectedPath = "E:\\Photography\\Astro Photography\\Processing"
            };
        }

        private void Button_Browse_Click(object sender, EventArgs e)
        {
            ProgressBar_OverAll.Value = 0;

            FindXisf();

            Label_Task.Text = "Found " + mFileList.Count().ToString() + " Images";
            //ProgressBar_OverAll.Value = 0;
        }

        private void Button_Rename_Click(object sender, EventArgs e)
        {
            RenameFiles();

            Label_Task.Text = "Done.";
            ProgressBar_OverAll.Value = 0;
        }

        private void RadioButton_Chronological_CheckedChanged(object sender, EventArgs e)
        {
            mBChronological = RadioButton_Chronological.Checked;
        }

        private void RadioButton_SSWEIGHT_CheckedChanged(object sender, EventArgs e)
        {
            mBSsWeight = RadioButton_SSWEIGHT.Checked;
        }

        private void CheckBox_KeepIndex_CheckedChanged(object sender, EventArgs e)
        {
            mKeepIndex = CheckBox_KeepIndex.Checked;
        }

        private void RadioButton_WeightFirst_CheckedChanged(object sender, EventArgs e)
        {
            mWeightFirst = RadioButton_WeightFirst.Checked;
        }

        private void RadioButton_IndexFirst_CheckedChanged(object sender, EventArgs e)
        {
            mIndexFirst = RadioButton_WeightFirst.Checked;
        }

        public bool FindXisf()
        {
            bool bFound;

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

            DirectoryInfo d;
            d = new DirectoryInfo(mFolderPath);


            FileInfo[] Files = d.GetFiles("*.xisf");

            ProgressBar_OverAll.Maximum = Files.Count();

            Label_Task.Text = "Reading Image File Data";

            mFileList.Clear();

            Settings.Default.PersistDirectory = mFolderPath;
            Settings.Default.Save();

            foreach (FileInfo file in Files)
            {
                ProgressBar_OverAll.Value += 1;

                mFile = new XisfFile();

                try
                {
                    char[] buffer = new char[20000];

                    mFile.SourceFileName = file.FullName;
                    mFile.SourceDirectoryInfo = new DirectoryInfo(mFile.SourceFileName);

                    using (StreamReader reader = new StreamReader(file.FullName))
                    {
                        reader.Read(buffer, 0, buffer.Length);
                    }

                    string s = new string(buffer);

                    s = s.Substring(s.IndexOf("<?xml"));
                    s = s.Substring(0, s.LastIndexOf(@"</xisf>") + 7);

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
                    foreach (XElement element in elements)
                    {
                        bFound = mFile.CaptureSoftware(element);
                        if (bFound)
                            break;
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
            }

            // Sort Image File List by Capture Time
            mFileList.Sort((x, y) => DateTime.Compare(x.CaptureDateTime, y.CaptureDateTime));

            return true;
        }

        private void RenameFiles()
        {
            Label_Task.Text = "Renaming Images";
            string newFileName;
            int index = 1;
            mDupIndex = 1;

            MarkDuplicates();

            string sourceFilePath;

            foreach (XisfFile file in mFileList)
            {
                sourceFilePath = Path.GetDirectoryName(file.SourceFileName);

                newFileName = BuildFileName(index, file);

                newFileName += ".xisf";

                // Actually rename the file
                if (file.Unique == true)
                {
                    if (File.Exists(sourceFilePath + "\\" + newFileName))
                    {
                        //File.Move(file.SourceFileName, sourceFilePath + "\\Renun " + newFileName);
                    }
                    else
                    {
                        File.Move(file.SourceFileName, sourceFilePath + "\\" + newFileName);
                    }
                    index++;
                }
                else
                {
                    Directory.CreateDirectory(sourceFilePath + "\\Duplicates");

                    int lastParen = newFileName.LastIndexOf(')');
                    newFileName = newFileName.Insert(lastParen + 1, "-" + mDupIndex.ToString());

                    File.Move(file.SourceFileName, sourceFilePath + "\\Duplicates\\" + newFileName);
                    mDupIndex++;
                }
            }

            Label_Task.Text = "Done";
        }

        private void MarkDuplicates()
        {
 
            // Duplicates are files with identical image capture times
 
            DateTime entryDateTime = DateTime.Now;

            //var results = mFileList.GroupBy(x => x.CaptureDateTime).Select(g => g.First()).ToList();

            foreach (var entry in mFileList)
            {
                // Only mark a DateTime once
                if (entry.CaptureDateTime == entryDateTime)
                {
                    continue;
                }

                entry.Unique = true;
                entryDateTime = entry.CaptureDateTime;
            }
        }

        private string BuildFileName(int index, XisfFile imageFile)
        {
            string newName;

            if ((imageFile.SubFrameSelectorWeight() == string.Empty) || (mBChronological == true))
            {
                newName = index.ToString("D3") + " ";
            }
            else
            {
                string fileName = Path.GetFileName(imageFile.SourceFileName);

                bool hasIndex = fileName.Substring(0, 2).All(char.IsDigit);

                if (mKeepIndex && hasIndex)
                {
                    if (mWeightFirst)
                    {
                        newName = imageFile.SubFrameSelectorWeight() + " " + fileName.Substring(0, 4); 
                    }
                    else
                    {
                        newName = fileName.Substring(0, 4) + imageFile.SubFrameSelectorWeight() + " ";
                    }
                }
                
                else
                {
                    newName = imageFile.SubFrameSelectorWeight() + " ";
                }
                
            }

            newName += imageFile.TargetName() + " ";
            newName += imageFile.FocalLength() + " ";
            if (imageFile.FrameType() == "Dark")
            {
                newName += imageFile.FrameType() + " ";
            }
            else
            {
                newName += imageFile.FrameType() + "-" + imageFile.FilterName() + " ";
            }
            newName += imageFile.ExposureSeconds() + "Sx" + imageFile.Binning() + " ";
            newName += imageFile.CameraModel() + "G" + imageFile.CameraGain() + "O" + imageFile.CameraOffset();
            if (imageFile.FrameType() == "Dark")
            {
                newName += "@" + imageFile.SensorTemperature() + "C ";
            }
            else
            {
                newName += "@" + imageFile.SensorTemperature() + "C" + " F" + imageFile.FocalPosition() + "@" + imageFile.FocalTemperature() + "C";
                if (imageFile.ImageAngle() != string.Empty)
                {
                    newName += " R" + imageFile.ImageAngle();
                }
                else
                {
                    newName += " ";
                }
            }
            

            newName += " (" + imageFile.ImageDateTime() + " ";

            if (imageFile.CaptureSoftware() == "SGP")
            {
                newName += "SGP";// + file.SgpProfile();
            }

            if (imageFile.CaptureSoftware() == "TheSkyX")
            {
                newName += "TSX";
            }

            newName += ")";

            return newName;
        }
        private void MoveDuplicates(XisfFile currentFile, string sourceFilePath, string newFileName)
        {
            mDupIndex = 1;

            foreach (var entry in mFileList)
            {
                mDupIndex++;

                Directory.CreateDirectory(sourceFilePath + "\\" + "Duplicates");

                int last = currentFile.SourceFileName.LastIndexOf(@"\");

                // Remove the index or SSWEIGHT from the file name (the first four characters) and postfix duplicate index
                string duplicateFileName = newFileName.Remove(0, 4).Insert(0, mDupIndex.ToString("D3") + " ");

                File.Move(entry.SourceFileName, sourceFilePath + "\\" + "Duplicates" + "\\" + duplicateFileName);
                mFileList.Remove(entry);
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

    }
}
