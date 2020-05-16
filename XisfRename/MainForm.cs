using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


using LocalLib;
using XisfRename.Csv;

namespace XisfRename
{
    // ##########################################################################################################################
    // ##########################################################################################################################
    public partial class MainForm : Form
    {
        List<Parse.XisfFile> mFileList;
        Parse.XisfFile mFile;
        Parse.RenameXisfFile mRenameFile;
        Parse.UpdateXisfFile mUpdateFile;
        private OpenFolderDialog mFolder;
        private OpenFileDialog mFileCsv;
        private string mFolderBrowseState;
        private string mFolderCsvBrowseState;
        private DirectoryInfo d;

        public MainForm()
        {
            InitializeComponent();
            Label_Task.Text = "";
            mFile = new Parse.XisfFile();
            mFileList = new List<Parse.XisfFile>();
            mRenameFile = new Parse.RenameXisfFile();
            mUpdateFile = new Parse.UpdateXisfFile();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            mFolderBrowseState = Properties.Settings.Default.Persist_FolderBrowseState;
            mFolderCsvBrowseState = Properties.Settings.Default.Persist_FolderCsvBrowseState;
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            base.OnClosing(e);

            Properties.Settings.Default.Persist_FolderBrowseState = mFolderBrowseState;
            Properties.Settings.Default.Persist_FolderCsvBrowseState = mFolderCsvBrowseState;

            Properties.Settings.Default.Save();
        }

        // ##########################################################################################################################
        // ##########################################################################################################################

        private void Button_Browse_Click(object sender, EventArgs e)
        {
            try
            {
                ProgressBar_OverAll.Value = 0;

                mFolder = new OpenFolderDialog()
                {
                    Title = "Select .xisf Folder",
                    AutoUpgradeEnabled = true,
                    CheckPathExists = false,
                    InitialDirectory = mFolderBrowseState, // @"E:\Photography\Astro Photography\Processing",
                    Multiselect = false,
                    RestoreDirectory = true
                };

                DialogResult result = mFolder.ShowDialog(IntPtr.Zero);

                if (result.Equals(DialogResult.OK) == false)
                {
                    return;
                }

                mFolderBrowseState = mFolder.SelectedPaths[0];

                foreach (string folder in mFolder.SelectedPaths)
                {
                    bool bStatus;

                    d = new DirectoryInfo(folder);

                    FileInfo[] Files = d.GetFiles("*.xisf");

                    ProgressBar_OverAll.Maximum = Files.Count();

                    Label_Task.Text = "Reading Image File Data";

                    mFileList.Clear();

                    foreach (FileInfo file in Files)
                    {
                        bStatus = false;
                        ProgressBar_OverAll.Value += 1;

                        mFile = new Parse.XisfFile();
                        mFile.SourceFileName = file.FullName;

                        bStatus = mFile.Parse();

                        if (bStatus)
                            mFileList.Add(mFile);
                    }
                }

                    // Sort Image File List by Capture Time
                    mFileList.Sort((x, y) => DateTime.Compare(x.CaptureDateTime, y.CaptureDateTime));

                    Label_Task.Text = "Found " + mFileList.Count().ToString() + " Images";

                    mFile.TargetNameList = mFile.TargetNameList.Distinct().ToList();

                    foreach (string targetName in mFile.TargetNameList)
                    {
                        ComboBox_TargetName.Items.Add(mFile.TargetName());
                    }
                
                ComboBox_TargetName.SelectedIndex = 0;
            }
            catch
            {
                return;
            }
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

        private void Button_ReadCSV_Click(object sender, EventArgs e)
        {
            mFileCsv = new OpenFileDialog()
            {
                Title = "Select Calibarated SubFrameSelected CSV File",
                AutoUpgradeEnabled = true,
                CheckPathExists = false,
                Filter = "CSV Files (*.csv) | *.csv",
                InitialDirectory = mFolderCsvBrowseState, // @"E:\Photography\Astro Photography\Processing",
                Multiselect = false,
                RestoreDirectory = true
            };

            DialogResult result = mFileCsv.ShowDialog();

            if (result.Equals(DialogResult.OK) == false)
            {
                return;
            }

            ReadSubFrameCsv.ParseCsvFile(mFileCsv.FileName);

        }
    }
}
