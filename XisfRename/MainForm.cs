using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

using LocalLib;

namespace XisfRename
{
    public partial class MainForm : Form
    {
        List<Parse.XisfFile> mFileList;
        Parse.XisfFile mFile;
        Parse.RenameXisfFile mRenameFile;
        Parse.UpdateXisfFile mUpdateFile;
        private OpenFolderDialog mFolder;
        private XDocument mXmlDoc;
       

        public MainForm()
        {
            InitializeComponent();
            Label_Task.Text = "";
            mFile = new Parse.XisfFile();
            mFileList = new List<Parse.XisfFile>();
            mRenameFile = new Parse.RenameXisfFile();
            mUpdateFile = new Parse.UpdateXisfFile();
        }

        private void Button_Browse_Click(object sender, EventArgs e)
        {
            ProgressBar_OverAll.Value = 0;

            mFolder = new OpenFolderDialog()
            {
                Title = "Select .xisf Folder",
                AutoUpgradeEnabled = true,
                CheckPathExists = false,
                InitialDirectory = @"E:\Photography\Astro Photography\Processing",
                Multiselect = true,
                RestoreDirectory = true
            };


            DialogResult result = mFolder.ShowDialog(IntPtr.Zero);

            if (result.Equals(DialogResult.OK))
            {
                FindXisf();

                Label_Task.Text = "Found " + mFileList.Count().ToString() + " Images";
            }

            mFile.TargetNameList = mFile.TargetNameList.Distinct().ToList();

            foreach (string targetName in mFile.TargetNameList)
            {
                ComboBox_TargetName.Items.Add(mFile.TargetName());
            }
            ComboBox_TargetName.SelectedIndex = 0;
            
            
            //ProgressBar_OverAll.Value = 0;
        }

        private void RadioButton_Chronological_CheckedChanged(object sender, EventArgs e)
        {
            mRenameFile.mBChronological = RadioButton_Chronological.Checked;
        }

        private void RadioButton_SSWEIGHT_CheckedChanged(object sender, EventArgs e)
        {
            mRenameFile.mBSsWeight = RadioButton_SSWEIGHT.Checked;
        }

        private void CheckBox_KeepIndex_CheckedChanged(object sender, EventArgs e)
        {
            mRenameFile.mKeepIndex = CheckBox_KeepIndex.Checked;
        }

        private void RadioButton_WeightFirst_CheckedChanged(object sender, EventArgs e)
        {
            mRenameFile.mWeightFirst = RadioButton_WeightFirst.Checked;
        }

        private void RadioButton_IndexFirst_CheckedChanged(object sender, EventArgs e)
        {
            mRenameFile.mIndexFirst = RadioButton_WeightFirst.Checked;
        }

        public bool FindXisf()
        {
            bool bFound;

            foreach (string folder in mFolder.SelectedPaths)
            {
                DirectoryInfo d;
                d = new DirectoryInfo(folder);


                FileInfo[] Files = d.GetFiles("*.xisf");

                ProgressBar_OverAll.Maximum = Files.Count();

                Label_Task.Text = "Reading Image File Data";

                mFileList.Clear();

                foreach (FileInfo file in Files)
                {
                    ProgressBar_OverAll.Value += 1;

                    mFile = new Parse.XisfFile();

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

                        IEnumerable<XElement> image = from c in mXmlDoc.Descendants(ns + "Image") select c;
                        foreach (XElement element in image)
                        {
                            mFile.Attachment(element);
                        }


                        IEnumerable<XElement> elements = from c in mXmlDoc.Descendants(ns + "FITSKeyword") select c;

                        // Advance through stream until we get to FITSKeyword elements
                        foreach (XElement element in elements)
                        {
                            bFound = mFile.CaptureSoftware(element);
                            if (bFound)
                                break;
                        }

                        // Find each relevent keyword and add it to mFile
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
                            mFile.FocusPosition(element);
                            mFile.FocusTemperature(element);
                            mFile.FrameType(element);
                            mFile.ImageAngle(element);
                            mFile.ImageDateTime(element);
                            mFile.ImageLocation(element);
                            mFile.SensorTemperature(element);
                            mFile.SgpProfile(element);
                            mFile.SubFrameSelectorWeight(element);
                            mFile.TargetName(element);
                            mFile.SiteLat(element);
                            mFile.SiteLon(element);
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
            }
            return true;
        }

        

        private void SelectTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mFolder = new OpenFolderDialog()
            {
                Title = "Select Template Folder",
                AutoUpgradeEnabled = true,
                CheckPathExists = false,
                InitialDirectory = @"E:\Photography\Astro Photography\Processing",
                Multiselect = true,
                RestoreDirectory = true
            };


            DialogResult result = mFolder.ShowDialog(IntPtr.Zero);

            if (result.Equals(DialogResult.OK))
            {
               
            }
        }

        private void Button_Rename_Click(object sender, EventArgs e)
        {
            Label_Task.Text = "Renaming Images";

            mRenameFile.RenameFiles(mFileList);

            Label_Task.Text = "Done.";
            ProgressBar_OverAll.Value = 0;
        }

        private void Button_Update_Click(object sender, EventArgs e)
        {
            mUpdateFile.NewTargetName = ComboBox_TargetName.Text;
            mUpdateFile.UpdateFiles(mFileList);
        }
    }
}
