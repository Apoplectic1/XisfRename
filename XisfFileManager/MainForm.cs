using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;


using LocalLib;
using XisfFileManager.CsvFileOperations;
using XisfFileManager.Keywords;
using XisfFileManager.XisfFileOperations;
using XisfFileManager.XisfFile;
using System.Threading;

namespace XisfFileManager
{
    // ##########################################################################################################################
    // ##########################################################################################################################
    public partial class MainForm : Form
    {
        List<XisfFile.XisfFile> mFileList;
        XisfFile.XisfFile mFile;
        XisfFileRename mRenameFile;
        private DirectoryInfo d;
        private OpenFileDialog mFileCsv;
        private OpenFolderDialog mFolder;
        private double mEccentricityMeanDeviationPercent;
        private double mEccentricityMeanDeviationRangeHigh;
        private double mEccentricityMeanDeviationRangeLow;
        private double mEccentricityPercent;
        private double mEccentricityRangeHigh;
        private double mEccentricityRangeLow;
        private double mFwhmMeanDeviationPercent;
        private double mFwhmMeanDeviationRangeHigh;
        private double mFwhmMeanDeviationRangeLow;
        private double mFwhmPercent;
        private double mFwhmRangeHigh;
        private double mFwhmRangeLow;
        private double mMeanMedianDeviationPercent;
        private double mMeanMedianDeviationRangeHigh;
        private double mMeanMedianDeviationRangeLow;
        private double mMedianPercent;
        private double mMedianRangeHigh;
        private double mMedianRangeLow;
        private double mNoisePercent;
        private double mNoiseRangeHigh;
        private double mNoiseRangeLow;
        private double mNoiseRatioPercent;
        private double mNoiseRatioRangeHigh;
        private double mNoiseRatioRangeLow;
        private double mSnrPercent;
        private double mSnrRangeHigh;
        private double mSnrRangeLow;
        private double mStarResidualMeanDevationPercent;
        private double mStarResidualMeanDevationRangeHigh;
        private double mStarResidualMeanDevationRangeLow;
        private double mStarResidualPercent;
        private double mStarResidualRangeHigh;
        private double mStarResidualRangeLow;
        private double mStarsPercent;
        private double mStarsRangeHigh;
        private double mStarsRangeLow;
        private double mUpdateStatisticsRangeHigh;
        private double mUpdateStatisticsRangeLow;
        private string mFolderBrowseState;
        private string mFolderCsvBrowseState;
        public bool mAddCsvKeywods;
        public SubFrameKeywordLists FileSubFrameKeywordLists;
        public SubFrameKeywordLists CsvSubFrameKeywordLists;

        public MainForm()
        {
            InitializeComponent();
            Label_Task.Text = "";
            mFileList = new List<XisfFile.XisfFile>();
            mRenameFile = new XisfFileRename();
            FileSubFrameKeywordLists = new SubFrameKeywordLists();
            CsvSubFrameKeywordLists = new SubFrameKeywordLists();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            mEccentricityMeanDeviationPercent = Properties.Settings.Default.Persist_EccentricityMeanDeviationPercentState;
            mEccentricityMeanDeviationRangeLow = Properties.Settings.Default.Persist_EccentricityMeanDeviationRangeLowState;
            mEccentricityMeanDeviationRangeHigh = Properties.Settings.Default.Persist_EccentricityMeanDeviationRangeHighState;
            mEccentricityPercent = Properties.Settings.Default.Persist_EccentricityPercentState;
            mEccentricityRangeHigh = Properties.Settings.Default.Persist_EccentricityRangeHighState;
            mEccentricityRangeLow = Properties.Settings.Default.Persist_EccentricityRangeLowState;
            mFolderBrowseState = Properties.Settings.Default.Persist_FolderBrowseState;
            mFolderCsvBrowseState = Properties.Settings.Default.Persist_FolderCsvBrowseState;
            mFwhmMeanDeviationPercent = Properties.Settings.Default.Persist_FwhmMeanDeviationPercentState;
            mFwhmMeanDeviationRangeHigh = Properties.Settings.Default.Persist_FwhmMeanDeviationRangeHighState;
            mFwhmMeanDeviationRangeLow = Properties.Settings.Default.Persist_FwhmMeanDeviationRangeLowState;
            mFwhmPercent = Properties.Settings.Default.Persist_FwhmPercentState;
            mFwhmRangeHigh = Properties.Settings.Default.Persist_FwhmRangeHighState;
            mFwhmRangeLow = Properties.Settings.Default.Persist_FwhmRangeLowState;
            mMeanMedianDeviationPercent = Properties.Settings.Default.Persist_MeanMedianDeviationPercentState;
            mMeanMedianDeviationRangeHigh = Properties.Settings.Default.Persist_MeanMedianDeviationRangeHighState;
            mMeanMedianDeviationRangeLow = Properties.Settings.Default.Persist_MeanMedianDeviationRangeLowState;
            mMedianPercent = Properties.Settings.Default.Persist_MedianPercentState;
            mMedianRangeHigh = Properties.Settings.Default.Persist_MedianRangeHighState;
            mMedianRangeLow = Properties.Settings.Default.Persist_MedianRangeLowState;
            mNoisePercent = Properties.Settings.Default.Persist_NoisePercentState;
            mNoiseRangeHigh = Properties.Settings.Default.Persist_NoiseRangeHighState;
            mNoiseRangeLow = Properties.Settings.Default.Persist_NoiseRangeLowState;
            mNoiseRatioPercent = Properties.Settings.Default.Persist_NoiseRatioPercentState;
            mNoiseRatioRangeHigh = Properties.Settings.Default.Persist_NoiseRatioRangeHighState;
            mNoiseRatioRangeLow = Properties.Settings.Default.Persist_NoiseRatioRangeLowState;
            mSnrPercent = Properties.Settings.Default.Persist_SnrPercentState;
            mSnrRangeHigh = Properties.Settings.Default.Persist_SnrRangeHighState;
            mSnrRangeLow = Properties.Settings.Default.Persist_SnrRangeLowState;
            mStarResidualMeanDevationPercent = Properties.Settings.Default.Persist_StarResidualMeanDevationPercentState;
            mStarResidualMeanDevationRangeHigh = Properties.Settings.Default.Persist_StarResidualMeanDevationRangeHighState;
            mStarResidualMeanDevationRangeLow = Properties.Settings.Default.Persist_StarResidualMeanDevationRangeLowState;
            mStarResidualPercent = Properties.Settings.Default.Persist_StarResidualPercentState;
            mStarResidualRangeHigh = Properties.Settings.Default.Persist_StarResidualRangeHighState;
            mStarResidualRangeLow = Properties.Settings.Default.Persist_StarResidualRangeLowState;
            mStarsPercent = Properties.Settings.Default.Persist_StarsPercentState;
            mStarsRangeHigh = Properties.Settings.Default.Persist_StarsRangeHighState;
            mStarsRangeLow = Properties.Settings.Default.Persist_StarsRangeLowState;
            mUpdateStatisticsRangeHigh = Properties.Settings.Default.Persist_UpdateStatisticsRangeHighState;
            mUpdateStatisticsRangeLow = Properties.Settings.Default.Persist_UpdateStatisticsRangeLowState;

            TextBox_EccentricityMeanDeviationPercent.Text = mEccentricityMeanDeviationPercent.ToString("F0");
            TextBox_EccentricityMeanDeviationRangeHigh.Text = mEccentricityMeanDeviationRangeHigh.ToString("F0");
            TextBox_EccentricityMeanDeviationRangeLow.Text = mEccentricityMeanDeviationRangeLow.ToString("F0");
            TextBox_EccentricityPercent.Text = mEccentricityPercent.ToString("F0");
            TextBox_EccentricityRangeHigh.Text = mEccentricityRangeHigh.ToString("F0");
            TextBox_EccentricityRangeLow.Text = mEccentricityRangeLow.ToString("F0");
            TextBox_FwhmMeanDeviationPercent.Text = mFwhmMeanDeviationPercent.ToString("F0");
            TextBox_FwhmMeanDeviationRangeHigh.Text = mFwhmMeanDeviationRangeHigh.ToString("F0");
            TextBox_FwhmMeanDeviationRangeLow.Text = mFwhmMeanDeviationRangeLow.ToString("F0");
            TextBox_FwhmPercent.Text = mFwhmPercent.ToString("F0");
            TextBox_FwhmRangeHigh.Text = mFwhmRangeHigh.ToString("F0");
            TextBox_FwhmRangeLow.Text = mFwhmRangeLow.ToString("F0");
            TextBox_MedianMeanDeviationPercent.Text = mMeanMedianDeviationPercent.ToString("F0");
            TextBox_MedianMeanDeviationRangeHigh.Text = mMeanMedianDeviationRangeHigh.ToString("F0");
            TextBox_MedianMeanDeviationRangeLow.Text = mMeanMedianDeviationRangeLow.ToString("F0");
            TextBox_MedianPercent.Text = mMedianPercent.ToString("F0");
            TextBox_MedianRangeHigh.Text = mMedianRangeHigh.ToString("F0");
            TextBox_MedianRangeLow.Text = mMedianRangeLow.ToString("F0");
            TextBox_NoisePercent.Text = mNoisePercent.ToString("F0");
            TextBox_NoiseRangeHigh.Text = mNoiseRangeHigh.ToString("F0");
            TextBox_NoiseRangeLow.Text = mNoiseRangeLow.ToString("F0");
            TextBox_NoiseRatioPercent.Text = mNoiseRatioPercent.ToString("F0");
            TextBox_NoiseRatioRangeHigh.Text = mNoiseRatioRangeHigh.ToString("F0");
            TextBox_NoiseRatioRangeLow.Text = mNoiseRatioRangeLow.ToString("F0");
            TextBox_SnrPercent.Text = mSnrPercent.ToString("F0");
            TextBox_SnrRangeHigh.Text = mSnrRangeHigh.ToString("F0");
            TextBox_SnrRangeLow.Text = mSnrRangeLow.ToString("F0");
            TextBox_StarResidualMeanDevationPercent.Text = mStarResidualMeanDevationPercent.ToString("F0");
            TextBox_StarResidualMeanDevationRangeHigh.Text = mStarResidualMeanDevationRangeHigh.ToString("F0");
            TextBox_StarResidualMeanDevationRangeLow.Text = mStarResidualMeanDevationRangeLow.ToString("F0");
            TextBox_StarResidualPercent.Text = mStarResidualPercent.ToString("F0");
            TextBox_StarResidualRangeHigh.Text = mStarResidualRangeHigh.ToString("F0");
            TextBox_StarResidualRangeLow.Text = mStarResidualRangeLow.ToString("F0");
            TextBox_StarPercent.Text = mStarsPercent.ToString("F0");
            TextBox_StarRangeHigh.Text = mStarsRangeHigh.ToString("F0");
            TextBox_StarRangeLow.Text = mStarsRangeLow.ToString("F0");
            TextBox_UpdateStatisticsRangeHigh.Text = mUpdateStatisticsRangeHigh.ToString("F0");
            TextBox_UpdateStatisticsRangeLow.Text = mUpdateStatisticsRangeLow.ToString("F0");
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            base.OnClosing(e);

            Properties.Settings.Default.Persist_EccentricityMeanDeviationPercentState = mEccentricityMeanDeviationPercent;
            Properties.Settings.Default.Persist_EccentricityMeanDeviationRangeHighState = mEccentricityMeanDeviationRangeHigh;
            Properties.Settings.Default.Persist_EccentricityMeanDeviationRangeLowState = mEccentricityMeanDeviationRangeLow;
            Properties.Settings.Default.Persist_EccentricityPercentState = mEccentricityPercent;
            Properties.Settings.Default.Persist_EccentricityRangeHighState = mEccentricityRangeHigh;
            Properties.Settings.Default.Persist_EccentricityRangeLowState = mEccentricityRangeLow;
            Properties.Settings.Default.Persist_FolderBrowseState = mFolderBrowseState;
            Properties.Settings.Default.Persist_FolderCsvBrowseState = mFolderCsvBrowseState;
            Properties.Settings.Default.Persist_FwhmMeanDeviationPercentState = mFwhmMeanDeviationPercent;
            Properties.Settings.Default.Persist_FwhmMeanDeviationRangeHighState = mFwhmMeanDeviationRangeHigh;
            Properties.Settings.Default.Persist_FwhmMeanDeviationRangeLowState = mFwhmMeanDeviationRangeLow;
            Properties.Settings.Default.Persist_FwhmPercentState = mFwhmPercent;
            Properties.Settings.Default.Persist_FwhmRangeHighState = mFwhmRangeHigh;
            Properties.Settings.Default.Persist_FwhmRangeLowState = mFwhmRangeLow;
            Properties.Settings.Default.Persist_MeanMedianDeviationPercentState = mMeanMedianDeviationPercent;
            Properties.Settings.Default.Persist_MeanMedianDeviationRangeHighState = mMeanMedianDeviationRangeHigh;
            Properties.Settings.Default.Persist_MeanMedianDeviationRangeLowState = mMeanMedianDeviationRangeLow;
            Properties.Settings.Default.Persist_MedianPercentState = mMedianPercent;
            Properties.Settings.Default.Persist_MedianRangeHighState = mMedianRangeHigh;
            Properties.Settings.Default.Persist_MedianRangeLowState = mMedianRangeLow;
            Properties.Settings.Default.Persist_NoisePercentState = mNoisePercent;
            Properties.Settings.Default.Persist_NoiseRangeHighState = mNoiseRangeHigh;
            Properties.Settings.Default.Persist_NoiseRangeLowState = mNoiseRangeLow;
            Properties.Settings.Default.Persist_NoiseRatioPercentState = mNoiseRatioPercent;
            Properties.Settings.Default.Persist_NoiseRatioRangeHighState = mNoiseRatioRangeHigh;
            Properties.Settings.Default.Persist_NoiseRatioRangeLowState = mNoiseRatioRangeLow;
            Properties.Settings.Default.Persist_SnrPercentState = mSnrPercent;
            Properties.Settings.Default.Persist_SnrRangeHighState = mSnrRangeHigh;
            Properties.Settings.Default.Persist_SnrRangeLowState = mSnrRangeLow;
            Properties.Settings.Default.Persist_StarResidualMeanDevationPercentState = mStarResidualMeanDevationPercent;
            Properties.Settings.Default.Persist_StarResidualMeanDevationRangeHighState = mStarResidualMeanDevationRangeHigh;
            Properties.Settings.Default.Persist_StarResidualMeanDevationRangeLowState = mStarResidualMeanDevationRangeLow;
            Properties.Settings.Default.Persist_StarResidualPercentState = mStarResidualPercent;
            Properties.Settings.Default.Persist_StarResidualRangeHighState = mStarResidualRangeHigh;
            Properties.Settings.Default.Persist_StarResidualRangeLowState = mStarResidualRangeLow;
            Properties.Settings.Default.Persist_StarsPercentState = mStarsPercent;
            Properties.Settings.Default.Persist_StarsRangeHighState = mStarsRangeHigh;
            Properties.Settings.Default.Persist_StarsRangeLowState = mStarsRangeLow;
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

                    if (Files.Count() < 1)
                    {
                        MessageBox.Show("No .xisf Files Found", "Select .xisf Folder");
                        return;
                    }

                    ProgressBar_OverAll.Maximum = Files.Count();


                    Label_Task.Text = "Reading Image File Data";

                    mFileList.Clear();

                    foreach (FileInfo file in Files)
                    {
                        bStatus = false;
                        ProgressBar_OverAll.Value += 1;

                        mFile = new XisfFile.XisfFile();
                        mFile.SourceFileName = file.FullName;

                        bStatus = XisfFileRead.ParseXisfFile(mFile, FileSubFrameKeywordLists);

                        if (bStatus)
                            mFileList.Add(mFile);
                    }
                }

                // Sort Image File List by Capture Time
                mFileList.Sort(XisfFile.XisfFile.CaptureTimeComparison);


                Label_Task.Text = "Found " + mFileList.Count().ToString() + " Images";

                List<string> TargetNames = new List<string>();
                foreach (XisfFile.XisfFile file in mFileList)
                {
                    TargetNames.Add(file.KeywordData.TargetName());
                }

                TargetNames = TargetNames.Distinct().ToList();

                if (TargetNames.Count > 1)
                {
                    Label_TagetName.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    Label_TagetName.ForeColor = System.Drawing.Color.Black;
                }

                TargetNames = TargetNames.OrderBy(q => q).ToList();

                foreach (string item in TargetNames)
                {
                    ComboBox_TargetName.Items.Add(item);
                }

                ComboBox_TargetName.SelectedIndex = 0;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Button_Browse_Click(object sender, EventArgs e)");
                return;
            }
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
            int index = 1;
            int indexIncrement;
            Label_Task.Text = "Renaming Images";

            ProgressBar_XisfFile.Maximum = mFileList.Count();
            ProgressBar_XisfFile.Value = 0;

            mRenameFile.MarkDuplicates(mFileList);


            foreach (XisfFile.XisfFile file in mFileList)
            {
                ProgressBar_XisfFile.Value += 1;
                indexIncrement = mRenameFile.RenameFiles(index, file);
                if (indexIncrement < 0)
                    break;
                index += indexIncrement;
            }
            ProgressBar_XisfFile.Value = ProgressBar_XisfFile.Maximum;
            mFileList.Clear();
            Label_Task.Text = "Done.";
           
            ProgressBar_OverAll.Value = 0;
        }

        private void Button_Update_Click(object sender, EventArgs e)
        {
            Label_Task.Text = "Updating Image Keywords";

            ProgressBar_XisfFile.Maximum = mFileList.Count();
            ProgressBar_XisfFile.Value = 0;


            foreach (XisfFile.XisfFile file in mFileList)
            {
                ProgressBar_XisfFile.Value += 1;

                XisfFileWrite.TargetName = ComboBox_TargetName.Text.Replace("'","").Replace("\"","");
                XisfFileWrite.AddCsvKeywords = mAddCsvKeywods;
                XisfFileWrite.UpdateFiles(file, CsvSubFrameKeywordLists);
            }
            ProgressBar_XisfFile.Value = ProgressBar_XisfFile.Maximum;
            Label_Task.Text = "Done.";
        }

        private void Button_ReadCSV_Click(object sender, EventArgs e)
        {
            bool status;

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

            status = ReadSubFrameCsvData.ParseSubFrameSelectorCsvFile(mFileCsv.FileName, CsvSubFrameKeywordLists);

            if (status)
                mAddCsvKeywods = true;

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
            mMeanMedianDeviationPercent = ValidateRangeValue(TextBox_MedianMeanDeviationPercent);
        }

        private void TextBox_MeanMedianDeviationRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mMeanMedianDeviationRangeHigh = ValidateRangeValue(TextBox_MedianMeanDeviationRangeHigh);
        }

        private void TextBox_MeanMedianDeviationRangeLow_TextChanged(object sender, EventArgs e)
        {
            mMeanMedianDeviationRangeLow = ValidateRangeValue(TextBox_MedianMeanDeviationRangeLow);
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

        private void TextBox_NoiseRatioPercent_TextChanged(object sender, EventArgs e)
        {
            mNoiseRatioPercent = ValidateRangeValue(TextBox_NoiseRatioPercent);
        }

        private void TextBox_NoiseRatioRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mNoiseRatioRangeHigh = ValidateRangeValue(TextBox_NoiseRatioRangeHigh);
        }

        private void TextBox_NoiseRationRangeLow_TextChanged(object sender, EventArgs e)
        {
            mNoiseRatioRangeLow = ValidateRangeValue(TextBox_NoiseRatioRangeLow);
        }

        private void TextBox_EccentricityMeanDeviationPercent_TextChanged(object sender, EventArgs e)
        {
            mEccentricityMeanDeviationPercent = ValidateRangeValue(TextBox_EccentricityMeanDeviationPercent);
        }

        private void TextBox_EccentricityMeanDeviationRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mEccentricityMeanDeviationRangeHigh = ValidateRangeValue(TextBox_EccentricityMeanDeviationRangeHigh);
        }

        private void TextBox_EccentricityMeanDeviationRangeLow_TextChanged(object sender, EventArgs e)
        {
            mEccentricityMeanDeviationRangeLow = ValidateRangeValue(TextBox_EccentricityMeanDeviationRangeLow);
        }

        private void TextBox_FwhmMeanDeviationPercent_TextChanged(object sender, EventArgs e)
        {
            mFwhmMeanDeviationPercent = ValidateRangeValue(TextBox_FwhmMeanDeviationPercent);
        }

        private void TextBox_FwhmMeanDeviationRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mFwhmMeanDeviationRangeHigh = ValidateRangeValue(TextBox_FwhmMeanDeviationRangeHigh);
        }

        private void TextBox_FwhmMeanDeviationRangeLow_TextChanged(object sender, EventArgs e)
        {
            mFwhmMeanDeviationRangeLow = ValidateRangeValue(TextBox_FwhmMeanDeviationRangeLow);
        }

        private void TextBox_StarsPercent_TextChanged(object sender, EventArgs e)
        {
            mStarsPercent = ValidateRangeValue(TextBox_StarPercent);
        }

        private void TextBox_StarsRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mStarsRangeHigh = ValidateRangeValue(TextBox_StarRangeHigh);
        }

        private void TextBox_StarsRangeLow_TextChanged(object sender, EventArgs e)
        {
            mStarsRangeLow = ValidateRangeValue(TextBox_StarRangeLow);
        }

        private void TextBox_StarResidualPercent_TextChanged(object sender, EventArgs e)
        {
            mStarResidualPercent = ValidateRangeValue(TextBox_StarResidualPercent);
        }

        private void TextBox_StarResidualRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mStarResidualRangeHigh = ValidateRangeValue(TextBox_StarResidualRangeHigh);
        }

        private void TextBox_StarResidualRangeLow_TextChanged(object sender, EventArgs e)
        {
            mStarResidualRangeLow = ValidateRangeValue(TextBox_StarResidualRangeLow);
        }

        private void TextBox_StarResidualMeanDevationPercent_TextChanged(object sender, EventArgs e)
        {
            mStarResidualMeanDevationPercent = ValidateRangeValue(TextBox_StarResidualMeanDevationPercent);
        }

        private void TextBox_StarResidualMeanDevationRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mStarResidualMeanDevationRangeHigh = ValidateRangeValue(TextBox_StarResidualMeanDevationRangeHigh);
        }

        private void TextBox_StarResidualMeanDevationRangeLow_TextChanged(object sender, EventArgs e)
        {
            mStarResidualMeanDevationRangeLow = ValidateRangeValue(TextBox_StarResidualMeanDevationRangeLow);
        }

        private double ValidateRangeValue(TextBox textBox)
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
