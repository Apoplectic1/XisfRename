using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Deployment.Application;

using LocalLib;
using XisfFileManager.FileOperations;
using XisfFileManager.Keywords;
using System.Drawing;
using XisfFileManager.Calculations;
using static XisfFileManager.Calculations.SubFrameNumericLists;

namespace XisfFileManager
{
    // ##########################################################################################################################
    // ##########################################################################################################################
    public partial class MainForm : Form
    {
        private ImageCalculations ImageParameterLists;
        private List<XisfFile> mFileList;
        private OpenFileDialog mFileCsv;
        private OpenFolderDialog mFolder;
        private SubFrameLists SubFrameLists;
        private SubFrameNumericLists SubFrameNumericLists;
        private SubFrameNumericListsValidEnum eSubFrameValidListsValid;
        private XisfFile mFile;
        private XisfFileRename mRenameFile;
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
        private bool mUpdateFilter;
        public MainForm()
        {
            InitializeComponent();
            Label_Task.Text = "";
            mFileList = new List<XisfFile>();
            mRenameFile = new XisfFileRename();
            mRenameFile.RenameOrder = XisfFileRename.OrderType.INDEXWEIGHT;

            // This set of lists conatin the data that was read from PixInsight's SubFrameSelector or from an image file that was updated with SubframeSlector Data.
            // Data is stored as Keyword XML nodes - strings
            SubFrameLists = new SubFrameLists();

            // This set of lists contains only numeric values (not XML node strings) to be used for weight calculations  
            SubFrameNumericLists = new SubFrameNumericLists();

            // This set of a much smaller number of numeric lists contains per image data used for Focuser Temperature compensation coefficient calculation and SSWEIGHTs
            ImageParameterLists = new ImageCalculations();

            Label_Task.Text = "No Images Selected";
            Label_TempratureCompensation.Text = "Temerature Coefficient: N/A";


            // Version Number
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                Version version = ad.CurrentVersion;
                Text = "XISF File Manager - Version: " + version.ToString();
            }
            else
            {
                Text = "XISF File Manager - Version: " + File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyyy.MM.dd - h:mm tt");
            }
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
            TextBox_StarResidualMeanDeviationPercent.Text = mStarResidualMeanDevationPercent.ToString("F0");
            TextBox_StarResidualMeanDevationRangeHigh.Text = mStarResidualMeanDevationRangeHigh.ToString("F0");
            TextBox_StarResidualMeanDevationRangeLow.Text = mStarResidualMeanDevationRangeLow.ToString("F0");
            TextBox_StarResidualPercent.Text = mStarResidualPercent.ToString("F0");
            TextBox_StarResidualRangeHigh.Text = mStarResidualRangeHigh.ToString("F0");
            TextBox_StarResidualRangeLow.Text = mStarResidualRangeLow.ToString("F0");
            TextBox_StarsPercent.Text = mStarsPercent.ToString("F0");
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
            DirectoryInfo d;

            ProgressBar_OverAll.Value = 0;
            ProgressBar_XisfFile.Value = 0;

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

            // Clear all lists - we are reading or re-reading what will become a new xisf file data set that will invalidate any existing data.         
            mFileList.Clear();
            SubFrameLists.Clear();
            SubFrameNumericLists.Clear();
            ImageParameterLists.Clear();
            mUpdateFilter = false;

            try
            {
                Label_Task.Text = "Reading Image File Data";

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

                    foreach (FileInfo file in Files)
                    {
                        bStatus = false;
                        ProgressBar_OverAll.Value += 1;
                        Application.DoEvents();

                        // Create a new xisf file instance
                        mFile = new XisfFile();
                        mFile.SourceFileName = file.FullName;
                        Label_BrowseFileName.Text = file.FullName;

                        // Get the keyword data contained found within the current file
                        // The keyword data is copied to and fills out the Keyword Class. The Keyword Class is an instance in mFile and specific to that file.
                        //
                        // FileSubFrameKeywordLists
                        // This set of lists will conatin the data initially supplied by PixInsight's SubFrame Selector in the form of an exported .csv file.
                        // This set of lists is not in mFile; rather it is global since it has data for each of the .xisf files that are read.
                        // Once an exported subframe Selector .csv file is read, the file specific data will be added to the mFile keywords and saved by clicking the "Update" button.
                        // If we are reading an .xisf file that already has these .csv keyords, add this files's csv specific data to each of FileSubFrameKeywordLists lists.
                        // This list addition happens in read order. Assignement to the correct mFile is based in the FileName list element in FileSubFrameKeywordLists.
                        // If this .csv data doesn't already exist in the current mFile, we will manually add it later by reading a selected .csv file from the UI.
                        //
                        // Note that each list in FileSubFrameKeywordLists contains a Keyword Class element that can be directly used to write keyword data back into an xisf file.
                        // What I mean by this is that FileSubFrameKeywordLists is basically string data and is not in a form easily used for calculations (a major point of this program).


                        bStatus = XisfFileRead.ReadXisfFile(mFile);

                        // If data was able to be properly read from our current .xisf file, add the current mFile instance to our master list mFileList.
                        if (bStatus)
                        {
                            mFileList.Add(mFile);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                         "An exception occured during file Browse/Read.\n\n" + ex.ToString(),
                         "\nMainForm.cs Button_Browse_Click()",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);

                Label_Task.Text = "Browse Aborted";
                return;
            }

            // Sort Image File List by Capture Time
            // Careful - make sure this doesn't screw up the SubFrameKeywordLists order later when writing back SubFrameKeyword data.
            // When updating actual xisf files, the update method for SubFrameKeyword data must use the SubFrameKeyword data FileName field to make sure the correct data gets written to the currect file.
            mFileList.Sort(XisfFile.CaptureTimeComparison);

            // If the following Keywords exist in the source XISF file, add the keyword value to FileSubFrameKeywordLists
            try
            {
                foreach (XisfFile file in mFileList)
                {
                    SubFrameLists.AddKeywordApproved(file.KeywordData);
                    SubFrameLists.AddKeywordEccentricity(file.KeywordData);
                    SubFrameLists.AddKeywordEccentricityMeanDeviation(file.KeywordData);
                    SubFrameLists.AddKeywordFileName(file.SourceFileName);
                    SubFrameLists.AddKeywordFwhm(file.KeywordData);
                    SubFrameLists.AddKeywordFwhmMeanDeviation(file.KeywordData);
                    SubFrameLists.AddKeywordMedian(file.KeywordData);
                    SubFrameLists.AddKeywordMedianMeanDeviation(file.KeywordData);
                    SubFrameLists.AddKeywordNoise(file.KeywordData);
                    SubFrameLists.AddKeywordNoiseRatio(file.KeywordData);
                    SubFrameLists.AddKeywordSnrWeight(file.KeywordData);
                    SubFrameLists.AddKeywordStarResidual(file.KeywordData);
                    SubFrameLists.AddKeywordStarResidualMeanDeviation(file.KeywordData);
                    SubFrameLists.AddKeywordStars(file.KeywordData);
                    SubFrameLists.AddKeywordWeight(file.KeywordData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                         "An exception occured during KeyWord assignments from source files.\n\n" + ex.ToString(),
                         "\nMainForm.cs Button_Browse_Click()",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Error);

                Label_Task.Text = "Browse Aborted";
                return;
            }

            Label_Task.Text = "Found " + mFileList.Count().ToString() + " Images";

            // Again, if the above Keywords exist in the source XISF file, add the keyword value to mNumericWeightLists
            // Build a set of numeric lists from FileSubFrameKeywordLists with any .csv keyword actually data found in the set of .xisf files 
            // we just read and parsed. mWeightLists will be used to mathaamatically generate actual weightings (SSWEIGHT) for PixInsight once they are written with the "Update" button.
            SubFrameNumericLists.BuildNumericSubFrameDataKeywordLists(SubFrameLists);

            // **********************************************************************
            // Get TargetName and populate ComboBox
            List<string> TargetNames = new List<string>();

            foreach (XisfFile file in mFileList)
            {
                TargetNames.Add(file.KeywordData.TargetName());
            }

            TargetNames = TargetNames.Distinct().ToList();
            TargetNames = TargetNames.OrderBy(q => q).ToList();

            if (TargetNames.Count > 1)
            {
                Label_TagetName.ForeColor = Color.Red;
            }
            else
            {
                Label_TagetName.ForeColor = Color.Black;
            }

            foreach (string item in TargetNames)
            {
                ComboBox_TargetName.Items.Add(item);
            }
            // **********************************************************************

            // **********************************************************************
            // Calculate Image paramters for UI
            foreach (XisfFile file in mFileList)
            {
                ImageParameterLists.BuildImageParameterValueLists(file.KeywordData);
            }

            string stepsPerDegree = ImageParameterLists.ComputeFocuserTemperatureCompensationCoefficient();
            Label_TempratureCompensation.Text = "Temperature Coefficient: " + stepsPerDegree;

            // Need to add calculations for average capture duration/overhead
            // **********************************************************************

            SetUISubFrameGroupBoxState();
            FindMultipleFilters();

            ComboBox_TargetName.SelectedIndex = 0;
            GroupBox_XisfFileUpdate.Enabled = true;
        }


        private void Button_Rename_Click(object sender, EventArgs e)
        {
            int index = 1;
            int indexIncrement;
            Label_Task.Text = "Renaming " + mFileList.Count().ToString() + " Images";

            ProgressBar_XisfFile.Maximum = mFileList.Count();
            ProgressBar_XisfFile.Value = 0;

            mRenameFile.MarkDuplicates(mFileList);


            foreach (XisfFile file in mFileList)
            {
                Tuple<int, string> renameTuple;
                ProgressBar_XisfFile.Value += 1;
                Label_BrowseFileName.Text = file.SourceFileName;

                renameTuple = mRenameFile.RenameFiles(index, file);
                indexIncrement = renameTuple.Item1;
                Label_UpdateFileName.Text = renameTuple.Item2;
                Application.DoEvents();

                
                if (indexIncrement < 0)
                    break;
                index += indexIncrement;
            }
            ProgressBar_XisfFile.Value = ProgressBar_XisfFile.Maximum;

            Label_Task.Text = mFileList.Count().ToString() + " Images Renamed";

            mFileList.Clear();

            ProgressBar_OverAll.Value = 0;

            GroupBox_XisfFileUpdate.Enabled = false;
        }

        private void Button_Update_Click(object sender, EventArgs e)
        {
            bool bStatus;

            Label_Task.Text = "Updating " + mFileList.Count().ToString() + " File Keywords";
            ProgressBar_XisfFile.Maximum = mFileList.Count();
            ProgressBar_XisfFile.Value = 0;

            // Only fill out the weight lists if in fact, we are actually updating them
            if (XisfFileUpdate.Operation == XisfFileUpdate.OperationEnum.CALCULATED_WEIGHTS)
            {
                // If the data in at least one of the read source XISF files is bad or inconsistent, do not allow that data to be updated without
                // re-reading a new PixInsight Subframe Selector generated CSV file (to either initially fill out or to correct bogus data)
                eSubFrameValidListsValid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
                if (eSubFrameValidListsValid == SubFrameNumericListsValidEnum.INVALD)
                {
                    var result = MessageBox.Show(
                        "SubFrame Numerical Weight List is Invalid.\n\nThere is a difference between the number of files contained in mFileList (" + mFileList.Count.ToString() + ")  " +
                        "compared to the number files in at least one Numeric Weight list.\n\nExample:\n    mWeightLists.Approved.Count = " + SubFrameNumericLists.Approved.Count() + "",
                        "\nMainForm.cs Button_Update_Click()",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    Label_Task.Text = "Update Aborted";
                    return;
                }

                // If we've got good consistent file and SubFrame data, then update the numerical lists 
                if (eSubFrameValidListsValid == SubFrameNumericListsValidEnum.VALID)
                {
                    // Fisrt calculate a new WEIGHT Keyword (not SSWEIGHT yet) based on the current state of the UI and file/list contents
                    SubFrameNumericLists.CalculateNewSubFrameWeights(mFileList.Count);

                    // Now copy the newly calculated WEIGHTs to the SubFrame Lists (the data structure that will be used to update file contents)
                    XisfFileUpdate.UpdateCsvWeightList(SubFrameNumericLists, SubFrameLists);
                }
            }

            XisfFileUpdate.TargetName = ComboBox_TargetName.Text.Replace("'", "").Replace("\"", "");

            foreach (XisfFile file in mFileList)
            {
                ProgressBar_XisfFile.Value += 1;
                bStatus = XisfFileUpdate.UpdateFile(file, SubFrameLists);
                Label_UpdateFileName.Text = file.SourceFileName;
                Application.DoEvents();

                if (bStatus == false)
                {
                    Label_Task.Text = "File Write Error";

                    var result = MessageBox.Show(
                        "File Update Failed.\n\n" + file.SourceFileName,
                        "\nMainForm.cs Button_Update_Click()",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }
            }

            Label_Task.Text = mFileList.Count().ToString() + " Images Updated";
        }

        private void Button_ReadCSV_Click(object sender, EventArgs e)
        {
            bool bStatus;

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

            SubFrameLists.Clear();
            SubFrameNumericLists.Clear();

            bStatus = ReadSubFrameCsvData.ReadCsvFile(mFileCsv.FileName, SubFrameLists);

            if (bStatus == false)
            {
                MessageBox.Show(mFileCsv.FileName, "CSV file data did not read and/or parse properly.");
                return;
            }

            SubFrameNumericLists.BuildNumericSubFrameDataKeywordLists(SubFrameLists);

            eSubFrameValidListsValid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);

            switch (eSubFrameValidListsValid)
            {
                case SubFrameNumericListsValidEnum.EMPTY:
                    MessageBox.Show("Numeric weight lists contain zero items.\n\n", mFileCsv.FileName);
                    return;

                case SubFrameNumericListsValidEnum.MISMATCH:
                    MessageBox.Show("Numeric weight list file names do not match read file names.\n\n" + mFileCsv.FileName + "\n\nRerun PixInsight SubFrame Selector.", "CSV File Error");
                    return;

                case SubFrameNumericListsValidEnum.INVALD:
                    MessageBox.Show("Numeric weight lists do not each contain " + mFileList.Count.ToString() + " items.\n\n", mFileCsv.FileName);
                    return;
            }

            if (eSubFrameValidListsValid == SubFrameNumericListsValidEnum.VALID)
            {
                // SubFrame data is valid
                NumericUpDown_Rejection_FWHM.Value = Convert.ToDecimal(SubFrameNumericLists.Fwhm.Max());
                NumericUpDown_Rejection_Eccentricity.Value = Convert.ToDecimal(SubFrameNumericLists.Eccentricity.Max());
                NumericUpDown_Rejection_Median.Value = Convert.ToDecimal(SubFrameNumericLists.Median.Max());
            }


            SetUISubFrameGroupBoxState();
        }

        private void TextBox_FwhmPercent_TextChanged(object sender, EventArgs e)
        {
            mFwhmPercent = ValidateRangeValue(TextBox_FwhmPercent);
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
        }

        private void TextBox_MedianPercent_TextChanged(object sender, EventArgs e)
        {
            mMedianPercent = ValidateRangeValue(TextBox_MedianPercent);
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
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
            mStarsPercent = ValidateRangeValue(TextBox_StarsPercent);
            SetUISubFrameGroupBoxState();
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
            SetUISubFrameGroupBoxState();
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
            mStarResidualMeanDevationPercent = ValidateRangeValue(TextBox_StarResidualMeanDeviationPercent);
            SetUISubFrameGroupBoxState();
        }

        private void TextBox_StarResidualMeanDevationRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mStarResidualMeanDevationRangeHigh = ValidateRangeValue(TextBox_StarResidualMeanDevationRangeHigh);
        }

        private void TextBox_StarResidualMeanDevationRangeLow_TextChanged(object sender, EventArgs e)
        {
            mStarResidualMeanDevationRangeLow = ValidateRangeValue(TextBox_StarResidualMeanDevationRangeLow);
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

        private void RadioButton_WeightIndex_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_RenameOrder_WeightIndex.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.WEIGHTINDEX;
            }
        }

        private void RadioButton_Index_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_RenameOrder_Index.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.INDEX;
            }
        }

        private void RadioButton_Weight_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_RenameOrder_Weight.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.WEIGHT;
            }
        }

        private void RadioButton_IndexWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_RenameOrder_IndexWeight.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.INDEXWEIGHT;
            }
        }

        private void SetUISubFrameGroupBoxState()
        {
            eSubFrameValidListsValid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);

            if (eSubFrameValidListsValid != SubFrameNumericListsValidEnum.VALID)
            {
                GroupBox_InitialRejectionCriteria.Enabled = false;
                GroupBox_WeightCalculations.Enabled = false;
            }

            if (eSubFrameValidListsValid == SubFrameNumericListsValidEnum.VALID)
            {
                // SubFrame data is valid
                TextBox_Rejection_Total.Text = SubFrameNumericLists.SetRejectedSubFrames(
                NumericUpDown_Rejection_FWHM.Value,
                NumericUpDown_Rejection_Eccentricity.Value,
                NumericUpDown_Rejection_Median.Value).ToString();
            }


            GroupBox_WeightsAndStatistics.Enabled = RadioButton_SubFrameKeywords_Alphabetize.Checked ? false : true;

            if (RadioButton_SetImageStatistics_KeepWeights.Checked)
            {
                GroupBox_WeightCalculations.Enabled = false;
                Label_UpdateStatistics.Enabled = false;
                Label_UpdateStatisticsRangeHigh.Enabled = false;
                TextBox_UpdateStatisticsRangeHigh.Enabled = false;
                Label_UpdateStatisticsRangeLow.Enabled = false;
                TextBox_UpdateStatisticsRangeLow.Enabled = false;
            }
            else
            {
                GroupBox_WeightCalculations.Enabled = RadioButton_SetImageStatistics_CalculateWeights.Checked;
                Label_UpdateStatistics.Enabled = true;
                Label_UpdateStatisticsRangeHigh.Enabled = true;
                TextBox_UpdateStatisticsRangeHigh.Enabled = true;
                Label_UpdateStatisticsRangeLow.Enabled = true;
                TextBox_UpdateStatisticsRangeLow.Enabled = true;
            }

            if (mFileList.Count != 0)
            {
                if (eSubFrameValidListsValid == SubFrameNumericListsValidEnum.VALID)
                {
                    GroupBox_InitialRejectionCriteria.Enabled = true;

                    GroupBox_EccentricityMeanDeviationWeight.Enabled = true;
                    if (TextBox_EccentricityMeanDeviationPercent.Text == "0") { GroupBox_EccentricityMeanDeviationWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_EccentricityMeanDeviationWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_FwhmWeight.Enabled = true;
                    if (TextBox_FwhmPercent.Text == "0") { GroupBox_FwhmWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_FwhmWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_EccentricityWeight.Enabled = true;
                    if (TextBox_EccentricityPercent.Text == "0") { GroupBox_EccentricityWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_EccentricityWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_FwhmMeanDeviationWeight.Enabled = true;
                    if (TextBox_FwhmMeanDeviationPercent.Text == "0") { GroupBox_FwhmMeanDeviationWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_FwhmMeanDeviationWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_FwhmWeight.Enabled = true;
                    if (TextBox_FwhmPercent.Text == "0") { GroupBox_FwhmWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_FwhmWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_MedianMeanDeviationWeight.Enabled = true;
                    if (TextBox_MedianMeanDeviationPercent.Text == "0") { GroupBox_MedianMeanDeviationWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_MedianMeanDeviationWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_MedianWeight.Enabled = true;
                    if (TextBox_MedianPercent.Text == "0") { GroupBox_MedianWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_MedianWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_NoiseRatioWeight.Enabled = true;
                    if (TextBox_NoiseRatioPercent.Text == "0") { GroupBox_NoiseRatioWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_NoiseRatioWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_NoiseWeight.Enabled = true;
                    if (TextBox_NoisePercent.Text == "0") { GroupBox_NoiseWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_NoiseWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_SnrWeight.Enabled = true;
                    if (TextBox_SnrPercent.Text == "0") { GroupBox_SnrWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_SnrWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_StarResidual.Enabled = true;
                    if (TextBox_StarResidualPercent.Text == "0") { GroupBox_StarResidual.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_StarResidual.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_StarResidualMeanDeviation.Enabled = true;
                    if (TextBox_StarResidualMeanDeviationPercent.Text == "0") { GroupBox_StarResidualMeanDeviation.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_StarResidualMeanDeviation.BackColor = Color.FromArgb(255, 240, 240, 240); }

                    GroupBox_StarsWeight.Enabled = true;
                    if (TextBox_StarsPercent.Text == "0") { GroupBox_StarsWeight.BackColor = Color.FromArgb(255, 200, 200, 200); } else { GroupBox_StarsWeight.BackColor = Color.FromArgb(255, 240, 240, 240); }
                }
                else
                {
                    GroupBox_InitialRejectionCriteria.Enabled = false;

                    GroupBox_EccentricityMeanDeviationWeight.Enabled = false;
                    GroupBox_EccentricityWeight.Enabled = false;
                    GroupBox_FwhmMeanDeviationWeight.Enabled = false;
                    GroupBox_FwhmWeight.Enabled = false;
                    GroupBox_MedianMeanDeviationWeight.Enabled = false;
                    GroupBox_MedianWeight.Enabled = false;
                    GroupBox_NoiseRatioWeight.Enabled = false;
                    GroupBox_NoiseWeight.Enabled = false;
                    GroupBox_SnrWeight.Enabled = false;
                    GroupBox_StarResidual.Enabled = false;
                    GroupBox_StarResidualMeanDeviation.Enabled = false;
                    GroupBox_StarsWeight.Enabled = false;
                }
            }
        }

        private void RadioButton_SetImageStatistics_KeepWeights_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_SetImageStatistics_KeepWeights.Checked)
            {
                XisfFileUpdate.Operation = XisfFileUpdate.OperationEnum.KEEP_WEIGHTS;
                SetUISubFrameGroupBoxState();
            }
        }

        private void RadioButton_SetImageStatistics_RescaleWeights_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_SetImageStatistics_RescaleWeights.Checked)
            {
                XisfFileUpdate.Operation = XisfFileUpdate.OperationEnum.RESCALE_WEIGHTS;
                SetUISubFrameGroupBoxState();
            }
        }

        private void RadioButton_SetImageStatistics_CalculateWeights_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_SetImageStatistics_CalculateWeights.Checked)
            {
                XisfFileUpdate.Operation = XisfFileUpdate.OperationEnum.RESCALE_WEIGHTS;
                SetUISubFrameGroupBoxState();
            }
        }

        private void RadioButton_SubFrameKeywords_Alphabetize_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_SubFrameKeywords_Alphabetize.Checked)
            {
                SetUISubFrameGroupBoxState();
            }
        }

        private void RadioButton_SubFrameKeyWords_SubFrameWeightCalculations_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.Checked)
            {
                SetUISubFrameGroupBoxState();
            }
        }

        private void NumericUpDown_Rejection_FWHM_ValueChanged(object sender, EventArgs e)
        {
            SetUISubFrameGroupBoxState();
        }

        private void NumericUpDown_Rejection_Eccentricity_ValueChanged(object sender, EventArgs e)
        {
            SetUISubFrameGroupBoxState();
        }

        private void NumericUpDown_Rejection_Median_ValueChanged(object sender, EventArgs e)
        {
            SetUISubFrameGroupBoxState();
        }

        private void Button_Rejection_RejectionSet_Click(object sender, EventArgs e)
        {
            SubFrameNumericListsValidEnum valid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
            if (valid != SubFrameNumericListsValidEnum.VALID)
            {
                return;
            }

            int index = 0;
            foreach (XisfFile file in mFileList)
            {
                // Add keyword will remove all instances of the keyword to be added and then add it
                SubFrameLists.SubFrameList.Approved[index].Value = SubFrameNumericLists.Approved[index].ToString();

                index++;
            }
        }

        private void UpdateWeightCalculations()
        {
            SubFrameNumericListsValidEnum valid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
            if (valid != SubFrameNumericListsValidEnum.VALID)
            {
                return;
            }


            Label_FwhmMean.Text = "Mean: " + SubFrameNumericLists.Fwhm.Average().ToString("F2");
            Label_EccentricityMean.Text = "Mean: " + SubFrameNumericLists.Eccentricity.Average().ToString("F2");
            Label_MedianMean.Text = "Mean: " + SubFrameNumericLists.Median.Average().ToString("F0");
            Label_FwhmMeanDeviationMean.Text = "Mean: " + SubFrameNumericLists.FwhmMeanDeviation.Average().ToString("F2");
            Label_EccentricityMeanDeviationMean.Text = "Mean: " + SubFrameNumericLists.EccentricityMeanDeviation.Average().ToString("F2");
            Label_MedianMeanDeviationMean.Text = "Mean: " + SubFrameNumericLists.MedianMeanDeviation.Average().ToString("F2");
            Label_NoiseMean.Text = "Mean: " + SubFrameNumericLists.Noise.Average().ToString("F2");
            Label_NoiseRatioMean.Text = "Mean: " + SubFrameNumericLists.NoiseRatio.Average().ToString("F2");
            Label_SnrMean.Text = "Mean: " + SubFrameNumericLists.SnrWeight.Average().ToString("F2");
            Label_StarsMean.Text = "Mean: " + SubFrameNumericLists.Stars.Average().ToString("F0");
            Label_StarResidualMean.Text = "Mean: " + SubFrameNumericLists.StarResidual.Average().ToString("F2");
            Label_StarResidualMeanDevationMean.Text = "Mean: " + SubFrameNumericLists.StarResidualMeanDeviation.Average().ToString("F2");

            Label_FwhmStdDev.Text = "StdDev: " + SubFrameNumericLists.Fwhm.StandardDeviation().ToString("F2");
            Label_EccentricityStdDev.Text = "StdDev: " + SubFrameNumericLists.Eccentricity.StandardDeviation().ToString("F2");
            Label_MedianStdDev.Text = "StdDev: " + SubFrameNumericLists.Median.StandardDeviation().ToString("F2");
            Label_FwhmMeanDeviationStdDev.Text = "StdDev: " + SubFrameNumericLists.FwhmMeanDeviation.StandardDeviation().ToString("F2");
            Label_EccentricityMeanDeviationStdDev.Text = "StdDev: " + SubFrameNumericLists.EccentricityMeanDeviation.StandardDeviation().ToString("F2");
            Label_MedianMeanDeviationStdDev.Text = "StdDev: " + SubFrameNumericLists.MedianMeanDeviation.StandardDeviation().ToString("F2");
            Label_NoiseStdDev.Text = "StdDev: " + SubFrameNumericLists.Noise.StandardDeviation().ToString("F2");
            Label_NoiseRatioStdDev.Text = "StdDev: " + SubFrameNumericLists.NoiseRatio.StandardDeviation().ToString("F2");
            Label_SnrStdDev.Text = "StdDev: " + SubFrameNumericLists.SnrWeight.StandardDeviation().ToString("F2");
            Label_StarsStdDev.Text = "StdDev: " + SubFrameNumericLists.Stars.StandardDeviation().ToString("F2");
            Label_StarResidualStdDev.Text = "StdDev: " + SubFrameNumericLists.StarResidual.StandardDeviation().ToString("F2");
            Label_StarResidualMeanDevationStdDev.Text = "StdDev: " + SubFrameNumericLists.StarResidualMeanDeviation.StandardDeviation().ToString("F2");
        }

        private void CheckBox_Filter_SetFilter_CheckedChanged(object sender, EventArgs e)
        {
            mUpdateFilter = CheckBox_Filter_SetFilter.Checked;

            if (mUpdateFilter)
            {
                foreach (XisfFile file in mFileList)
                {
                    if (RadioButton_Filter_Luma.Checked)
                        file.KeywordData.AddKeyword("FILTER", "Luma");

                    if (RadioButton_Filter_Red.Checked)
                        file.KeywordData.AddKeyword("FILTER", "Red");

                    if (RadioButton_Filter_Green.Checked)
                        file.KeywordData.AddKeyword("FILTER", "Green");

                    if (RadioButton_Filter_Blue.Checked)
                        file.KeywordData.AddKeyword("FILTER", "Blue");

                    if (RadioButton_Filter_Ha.Checked)
                        file.KeywordData.AddKeyword("FILTER", "Ha");

                    if (RadioButton_Filter_O3.Checked)
                        file.KeywordData.AddKeyword("FILTER", "O3");

                    if (RadioButton_Filter_S2.Checked)
                        file.KeywordData.AddKeyword("FILTER", "S2");
                }
            }
           
            FindMultipleFilters();
        }
        private void FindMultipleFilters()
        {
            string filter;
            string firstFilter;
            bool multipleFilters = false;

            if (mFileList.Count == 0)
                return;

            firstFilter = mFileList[0].KeywordData.FilterName();


            foreach (XisfFile file in mFileList)
            {
                filter = file.KeywordData.FilterName();
                if (filter != firstFilter)
                {
                    if (filter == "Luma")
                    {
                        RadioButton_Filter_Luma.ForeColor = Color.Red;
                        multipleFilters = true;
                    }

                    if (filter == "Red")
                    {
                        RadioButton_Filter_Red.ForeColor = Color.Red;
                        multipleFilters = true;
                    }

                    if (filter == "Green")
                    {
                        RadioButton_Filter_Green.ForeColor = Color.Red;
                        multipleFilters = true;
                    }

                    if (filter == "Blue")
                    {
                        RadioButton_Filter_Blue.ForeColor = Color.Red;
                        multipleFilters = true;
                    }

                    if (filter == "Ha")
                    {
                        RadioButton_Filter_Ha.ForeColor = Color.Red;
                        multipleFilters = true;
                    }

                    if (filter == "O3")
                    {
                        RadioButton_Filter_O3.ForeColor = Color.Red;
                        multipleFilters = true;
                    }

                    if (filter == "S2")
                    {
                        RadioButton_Filter_S2.ForeColor = Color.Red;
                        multipleFilters = true;
                    }
                }

                if (multipleFilters)
                {
                    if (firstFilter == "Luma")
                    {
                        RadioButton_Filter_Luma.ForeColor = Color.Red;
                    }

                    if (firstFilter == "Red")
                    {
                        RadioButton_Filter_Red.ForeColor = Color.Red;
                    }

                    if (firstFilter == "Green")
                    {
                        RadioButton_Filter_Green.ForeColor = Color.Red;
                    }

                    if (firstFilter == "Blue")
                    {
                        RadioButton_Filter_Blue.ForeColor = Color.Red;
                    }

                    if (firstFilter == "Ha")
                    {
                        RadioButton_Filter_Ha.ForeColor = Color.Red;
                    }

                    if (firstFilter == "O3")
                    {
                        RadioButton_Filter_O3.ForeColor = Color.Red;
                    }

                    if (firstFilter == "S2")
                    {
                        RadioButton_Filter_S2.ForeColor = Color.Red;
                    }
                }
                else
                {
                    if (firstFilter == "Luma")
                    {
                        RadioButton_Filter_Luma.Checked = true;
                    }

                    if (firstFilter == "Red")
                    {
                        RadioButton_Filter_Red.Checked = true;
                    }

                    if (firstFilter == "Green")
                    {
                        RadioButton_Filter_Green.Checked = true;
                    }

                    if (firstFilter == "Blue")
                    {
                        RadioButton_Filter_Blue.Checked = true;
                    }

                    if (firstFilter == "Ha")
                    {
                        RadioButton_Filter_Ha.Checked = true;
                    }

                    if (firstFilter == "O3")
                    {
                        RadioButton_Filter_O3.Checked = true;
                    }

                    if (firstFilter == "S2")
                    {
                        RadioButton_Filter_S2.Checked = true;
                    }

                    RadioButton_Filter_Luma.ForeColor = Color.Black;
                    RadioButton_Filter_Red.ForeColor = Color.Black;
                    RadioButton_Filter_Green.ForeColor = Color.Black;
                    RadioButton_Filter_Blue.ForeColor = Color.Black;
                    RadioButton_Filter_Ha.ForeColor = Color.Black;
                    RadioButton_Filter_O3.ForeColor = Color.Black;
                    RadioButton_Filter_S2.ForeColor = Color.Black;
                }
            }
        }
    }
}
