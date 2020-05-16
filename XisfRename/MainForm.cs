using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

        private double mFwhmPercent;
        private double mFwhmRangeHigh;
        private double mFwhmRangeLow;

        private double mEccentricityPercent;
        private double mEccentricityRangeHigh;
        private double mEccentricityRangeLow;

        private double mSnrPercent;
        private double mSnrRangeHigh;
        private double mSnrRangeLow;

        private double mMedianPercent;
        private double mMedianRangeHigh;
        private double mMedianRangeLow;

        private double mMeanMedianDeviationPercent;
        private double mMeanMedianDeviationRangeHigh;
        private double mMeanMedianDeviationRangeLow;

        private double mNoisePercent;
        private double mNoiseRangeHigh;
        private double mNoiseRangeLow;

        private double mUpdateStatisticsRangeHigh;
        private double mUpdateStatisticsRangeLow;

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


            mFwhmPercent = Properties.Settings.Default.Persist_FwhmPercentState;
            mFwhmRangeHigh = Properties.Settings.Default.Persist_FwhmRangeHighState;
            mFwhmRangeLow = Properties.Settings.Default.Persist_FwhmRangeLowState;

            TextBox_FwhmPercent.Text = mFwhmPercent.ToString("F0");
            TextBox_FwhmRangeHigh.Text = mFwhmRangeHigh.ToString("F0");
            TextBox_FwhmRangeLow.Text = mFwhmRangeLow.ToString("F0");


            mEccentricityPercent = Properties.Settings.Default.Persist_EccentricityPercentState;
            mEccentricityRangeHigh = Properties.Settings.Default.Persist_EccentricityRangeHighState;
            mEccentricityRangeLow = Properties.Settings.Default.Persist_EccentricityRangeLowState;

            TextBox_EccentricityPercent.Text = mEccentricityPercent.ToString("F0");
            TextBox_EccentricityRangeHigh.Text = mEccentricityRangeHigh.ToString("F0");
            TextBox_EccentricityRangeLow.Text = mEccentricityRangeLow.ToString("F0");


            mSnrPercent = Properties.Settings.Default.Persist_SnrPercentState;
            mSnrRangeHigh = Properties.Settings.Default.Persist_SnrRangeHighState;
            mSnrRangeLow = Properties.Settings.Default.Persist_SnrRangeLowState;

            TextBox_SnrPercent.Text = mSnrPercent.ToString("F0");
            TextBox_SnrRangeHigh.Text = mSnrRangeHigh.ToString("F0");
            TextBox_SnrRangeLow.Text = mSnrRangeLow.ToString("F0");


            mMedianPercent = Properties.Settings.Default.Persist_MedianPercentState;
            mMedianRangeHigh = Properties.Settings.Default.Persist_MedianRangeHighState;
            mMedianRangeLow = Properties.Settings.Default.Persist_MedianRangeLowState;

            TextBox_MedianPercent.Text = mMedianPercent.ToString("F0");
            TextBox_MedianRangeHigh.Text = mMedianRangeHigh.ToString("F0");
            TextBox_MedianRangeLow.Text = mMedianRangeLow.ToString("F0");


            mMeanMedianDeviationPercent = Properties.Settings.Default.Persist_MeanMedianDeviationPercentState;
            mMeanMedianDeviationRangeHigh = Properties.Settings.Default.Persist_MeanMedianDeviationRangeHighState;
            mMeanMedianDeviationRangeLow = Properties.Settings.Default.Persist_MeanMedianDeviationRangeLowState;

            TextBox_MeanMedianDeviationPercent.Text = mMeanMedianDeviationPercent.ToString("F0");
            TextBox_MeanMedianDeviationRangeHigh.Text = mMeanMedianDeviationRangeHigh.ToString("F0");
            TextBox_MeanMedianDeviationRangeLow.Text = mMeanMedianDeviationRangeLow.ToString("F0");


            mNoisePercent = Properties.Settings.Default.Persist_NoisePercentState;
            mNoiseRangeHigh = Properties.Settings.Default.Persist_NoiseRangeHighState;
            mNoiseRangeLow = Properties.Settings.Default.Persist_NoiseRangeLowState;

            TextBox_NoisePercent.Text = mNoisePercent.ToString("F0");
            TextBox_NoiseRangeHigh.Text = mNoiseRangeHigh.ToString("F0");
            TextBox_NoiseRangeLow.Text = mNoiseRangeLow.ToString("F0");

            mUpdateStatisticsRangeHigh = Properties.Settings.Default.Persist_UpdateStatisticsRangeHighState;
            TextBox_UpdateStatisticsRangeHigh.Text = mUpdateStatisticsRangeHigh.ToString("F0");

            mUpdateStatisticsRangeLow = Properties.Settings.Default.Persist_UpdateStatisticsRangeLowState;
            TextBox_UpdateStatisticsRangeLow.Text = mUpdateStatisticsRangeLow.ToString("F0");
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            base.OnClosing(e);

            Properties.Settings.Default.Persist_FolderBrowseState = mFolderBrowseState;
            Properties.Settings.Default.Persist_FolderCsvBrowseState = mFolderCsvBrowseState;

            Properties.Settings.Default.Persist_FwhmPercentState = mFwhmPercent;
            Properties.Settings.Default.Persist_FwhmRangeHighState = mFwhmRangeHigh;
            Properties.Settings.Default.Persist_FwhmRangeLowState = mFwhmRangeLow;

            Properties.Settings.Default.Persist_EccentricityPercentState = mEccentricityPercent;
            Properties.Settings.Default.Persist_EccentricityRangeHighState = mEccentricityRangeHigh;
            Properties.Settings.Default.Persist_EccentricityRangeLowState = mEccentricityRangeLow;

            Properties.Settings.Default.Persist_SnrPercentState = mSnrPercent;
            Properties.Settings.Default.Persist_SnrRangeHighState = mSnrRangeHigh;
            Properties.Settings.Default.Persist_SnrRangeLowState = mSnrRangeLow;

            Properties.Settings.Default.Persist_MedianPercentState = mMedianPercent;
            Properties.Settings.Default.Persist_MedianRangeHighState = mMedianRangeHigh;
            Properties.Settings.Default.Persist_MedianRangeLowState = mMedianRangeLow;

            Properties.Settings.Default.Persist_MeanMedianDeviationPercentState = mMeanMedianDeviationPercent;
            Properties.Settings.Default.Persist_MeanMedianDeviationRangeHighState = mMeanMedianDeviationRangeHigh;
            Properties.Settings.Default.Persist_MeanMedianDeviationRangeLowState = mMeanMedianDeviationRangeLow;

            Properties.Settings.Default.Persist_NoisePercentState = mNoisePercent;
            Properties.Settings.Default.Persist_NoiseRangeHighState = mNoiseRangeHigh;
            Properties.Settings.Default.Persist_NoiseRangeLowState = mNoiseRangeLow;

            Properties.Settings.Default.Persist_UpdateStatisticsRangeHighState = mUpdateStatisticsRangeHigh;
            Properties.Settings.Default.Persist_UpdateStatisticsRangeLowState = mUpdateStatisticsRangeLow;

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

        private void CheckBox_IncludeWeightsAndStatistics_CheckedChanged(object sender, EventArgs e)
        {
            GroupBox_WeightsAndStatistics.Enabled = CheckBox_IncludeWeightsAndStatistics.Checked;
        }

        private void TextBox_FwhmPercent_TextChanged(object sender, EventArgs e)
        {
            mFwhmPercent = ValidateRangeValue(TextBox_FwhmPercent);
        }

        private void TextBox_FwhmRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mFwhmRangeHigh = ValidateRangeValue(TextBox_FwhmRangeHigh);
        }

        private void TextBox_FwhmRangeLow_TextChanged(object sender, EventArgs e)
        {
            mFwhmRangeLow = ValidateRangeValue(TextBox_FwhmRangeLow);
        }

        private void TextBox_EccentricityPercent_TextChanged(object sender, EventArgs e)
        {
            mEccentricityPercent = ValidateRangeValue(TextBox_EccentricityPercent);
        }

        private void TextBox_EccentricityRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mEccentricityRangeHigh = ValidateRangeValue(TextBox_EccentricityRangeHigh);
        }

        private void TextBox_EccentricityRangeLow_TextChanged(object sender, EventArgs e)
        {
            mEccentricityRangeLow = ValidateRangeValue(TextBox_EccentricityRangeLow);
        }

        private void TextBox_SnrPercent_TextChanged(object sender, EventArgs e)
        {
            mSnrPercent = ValidateRangeValue(TextBox_SnrPercent);
        }

        private void TextBox_SnrRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mSnrRangeHigh = ValidateRangeValue(TextBox_SnrRangeHigh);
        }

        private void TextBox_SnrRangeLow_TextChanged(object sender, EventArgs e)
        {
            mSnrRangeLow = ValidateRangeValue(TextBox_SnrRangeLow);
        }

        private void TextBox_MedianPercent_TextChanged(object sender, EventArgs e)
        {
            mMedianPercent = ValidateRangeValue(TextBox_MedianPercent);
        }

        private void TextBox_MedianRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mMedianRangeHigh = ValidateRangeValue(TextBox_MedianRangeHigh);
        }

        private void TextBox_MedianRangeLow_TextChanged(object sender, EventArgs e)
        {
            mMedianRangeLow = ValidateRangeValue(TextBox_MedianRangeLow);
        }

        private void TextBox_MeanMedianDeviationPercent_TextChanged(object sender, EventArgs e)
        {
            mMeanMedianDeviationPercent = ValidateRangeValue(TextBox_MeanMedianDeviationPercent);
        }

        private void TextBox_MeanMedianDeviationRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mMeanMedianDeviationRangeHigh = ValidateRangeValue(TextBox_MeanMedianDeviationRangeHigh);
        }

        private void TextBox_MeanMedianDeviationRangeLow_TextChanged(object sender, EventArgs e)
        {
            mMeanMedianDeviationRangeLow = ValidateRangeValue(TextBox_MeanMedianDeviationRangeLow);
        }

        private void TextBox_NoisePercent_TextChanged(object sender, EventArgs e)
        {
            mNoisePercent = ValidateRangeValue(TextBox_NoisePercent);
        }

        private void TextBox_NoiseRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mNoiseRangeHigh = ValidateRangeValue(TextBox_NoiseRangeHigh);
        }

        private void TextBox_NoiseRangeLow_TextChanged(object sender, EventArgs e)
        {
            mNoiseRangeLow = ValidateRangeValue(TextBox_NoiseRangeLow);
        }

        private void TextBox_UpdateStatisticsRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mUpdateStatisticsRangeHigh = ValidateRangeValue(TextBox_UpdateStatisticsRangeHigh);
        }

        private void TextBox_UpdateStatisticsRangeLow_TextChanged(object sender, EventArgs e)
        {
            mUpdateStatisticsRangeLow = ValidateRangeValue(TextBox_UpdateStatisticsRangeLow);
        }

        private double ValidateRangeValue (TextBox textBox)
        {
            bool status;
            double value;
            double rangedValue;

            textBox.Text = Regex.Match(textBox.Text, @"^[-+]?\d+$").Value;

            status = double.TryParse(textBox.Text, out value);

            rangedValue = status ? value : 0.0;

            rangedValue = rangedValue > 500 ? 500 : rangedValue;
            rangedValue = rangedValue < -500 ? -500 : rangedValue;

            textBox.Text = rangedValue.ToString("F0");

            return rangedValue;
        }
    }
}
