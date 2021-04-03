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
using MathNet.Numerics.Statistics;

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
        private double mEccentricityRangeHigh;
        private double mEccentricityRangeLow;
        private double mFwhmPercent;
        private double mFwhmRangeHigh;
        private double mFwhmRangeLow;
        private double mMedianRangeHigh;
        private double mMedianRangeLow;
        private double mNoiseRangeHigh;
        private double mNoiseRangeLow;
        private double mNoiseRatioRangeHigh;
        private double mNoiseRatioRangeLow;
        private double mSnrPercent;
        private double mSnrRangeHigh;
        private double mSnrRangeLow;
        private double mStarResidualRangeHigh;
        private double mStarResidualRangeLow;
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

            mEccentricityRangeHigh = Properties.Settings.Default.Persist_EccentricityRangeHighState;
            mEccentricityRangeLow = Properties.Settings.Default.Persist_EccentricityRangeLowState;
            mFolderBrowseState = Properties.Settings.Default.Persist_FolderBrowseState;
            mFolderCsvBrowseState = Properties.Settings.Default.Persist_FolderCsvBrowseState;
            mFwhmPercent = Properties.Settings.Default.Persist_FwhmPercentState;
            mFwhmRangeHigh = Properties.Settings.Default.Persist_FwhmRangeHighState;
            mFwhmRangeLow = Properties.Settings.Default.Persist_FwhmRangeLowState;
            mMedianRangeHigh = Properties.Settings.Default.Persist_MedianRangeHighState;
            mMedianRangeLow = Properties.Settings.Default.Persist_MedianRangeLowState;
            mNoiseRangeHigh = Properties.Settings.Default.Persist_NoiseRangeHighState;
            mNoiseRangeLow = Properties.Settings.Default.Persist_NoiseRangeLowState;
            mNoiseRatioRangeHigh = Properties.Settings.Default.Persist_NoiseRatioRangeHighState;
            mNoiseRatioRangeLow = Properties.Settings.Default.Persist_NoiseRatioRangeLowState;
            mSnrPercent = Properties.Settings.Default.Persist_SnrPercentState;
            mSnrRangeHigh = Properties.Settings.Default.Persist_SnrRangeHighState;
            mSnrRangeLow = Properties.Settings.Default.Persist_SnrRangeLowState;
            mStarResidualRangeHigh = Properties.Settings.Default.Persist_StarResidualRangeHighState;
            mStarResidualRangeLow = Properties.Settings.Default.Persist_StarResidualRangeLowState;
            mStarsRangeHigh = Properties.Settings.Default.Persist_StarsRangeHighState;
            mStarsRangeLow = Properties.Settings.Default.Persist_StarsRangeLowState;
            mUpdateStatisticsRangeHigh = Properties.Settings.Default.Persist_UpdateStatisticsRangeHighState;
            mUpdateStatisticsRangeLow = Properties.Settings.Default.Persist_UpdateStatisticsRangeLowState;

            TextBox_EccentricityRangeHigh.Text = mEccentricityRangeHigh.ToString("F0");
            TextBox_EccentricityRangeLow.Text = mEccentricityRangeLow.ToString("F0");
            TextBox_FwhmRangeHigh.Text = mFwhmRangeHigh.ToString("F0");
            TextBox_FwhmRangeLow.Text = mFwhmRangeLow.ToString("F0");
            TextBox_MedianRangeHigh.Text = mMedianRangeHigh.ToString("F0");
            TextBox_MedianRangeLow.Text = mMedianRangeLow.ToString("F0");
            TextBox_NoiseRangeHigh.Text = mNoiseRangeHigh.ToString("F0");
            TextBox_NoiseRangeLow.Text = mNoiseRangeLow.ToString("F0");
            TextBox_SnrRangeHigh.Text = mSnrRangeHigh.ToString("F0");
            TextBox_SnrRangeLow.Text = mSnrRangeLow.ToString("F0");
            TextBox_StarResidualRangeHigh.Text = mStarResidualRangeHigh.ToString("F0");
            TextBox_StarResidualRangeLow.Text = mStarResidualRangeLow.ToString("F0");
            TextBox_StarRangeHigh.Text = mStarsRangeHigh.ToString("F0");
            TextBox_StarRangeLow.Text = mStarsRangeLow.ToString("F0");
            TextBox_UpdateStatisticsRangeHigh.Text = mUpdateStatisticsRangeHigh.ToString("F0");
            TextBox_UpdateStatisticsRangeLow.Text = mUpdateStatisticsRangeLow.ToString("F0");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Properties.Settings.Default.Persist_EccentricityRangeHighState = mEccentricityRangeHigh;
            Properties.Settings.Default.Persist_EccentricityRangeLowState = mEccentricityRangeLow;
            Properties.Settings.Default.Persist_FolderBrowseState = mFolderBrowseState;
            Properties.Settings.Default.Persist_FolderCsvBrowseState = mFolderCsvBrowseState;
            Properties.Settings.Default.Persist_FwhmPercentState = mFwhmPercent;
            Properties.Settings.Default.Persist_FwhmRangeHighState = mFwhmRangeHigh;
            Properties.Settings.Default.Persist_FwhmRangeLowState = mFwhmRangeLow;
            Properties.Settings.Default.Persist_MedianRangeHighState = mMedianRangeHigh;
            Properties.Settings.Default.Persist_MedianRangeLowState = mMedianRangeLow;
            Properties.Settings.Default.Persist_NoiseRangeHighState = mNoiseRangeHigh;
            Properties.Settings.Default.Persist_NoiseRangeLowState = mNoiseRangeLow;
            Properties.Settings.Default.Persist_NoiseRatioRangeHighState = mNoiseRatioRangeHigh;
            Properties.Settings.Default.Persist_NoiseRatioRangeLowState = mNoiseRatioRangeLow;
            Properties.Settings.Default.Persist_SnrPercentState = mSnrPercent;
            Properties.Settings.Default.Persist_SnrRangeHighState = mSnrRangeHigh;
            Properties.Settings.Default.Persist_SnrRangeLowState = mSnrRangeLow;
            Properties.Settings.Default.Persist_StarResidualRangeHighState = mStarResidualRangeHigh;
            Properties.Settings.Default.Persist_StarResidualRangeLowState = mStarResidualRangeLow;
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
            ComboBox_TargetName.Text = "";
            ComboBox_TargetName.Items.Clear();
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
                        Label_BrowseFileName.Text = Path.GetDirectoryName(file.FullName) + "\n" + Path.GetFileName(file.FullName);

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
                    SubFrameLists.AddKeywordAirMass(file.KeywordData);
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

            eSubFrameValidListsValid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
            if (eSubFrameValidListsValid == SubFrameNumericListsValidEnum.VALID)
            {
                // SubFrame data is valid
                NumericUpDown_Rejection_FWHM.Value = Convert.ToDecimal(SubFrameNumericLists.Fwhm.Max());
                NumericUpDown_Rejection_Eccentricity.Value = Convert.ToDecimal(SubFrameNumericLists.Eccentricity.Max());
                NumericUpDown_Rejection_Median.Value = Convert.ToDecimal(SubFrameNumericLists.Median.Max());
                NumericUpDown_Rejection_Noise.Value = Convert.ToDecimal(SubFrameNumericLists.Noise.Max());
                NumericUpDown_Rejection_AirMass.Value = Convert.ToDecimal(SubFrameNumericLists.AirMass.Max());
                NumericUpDown_Rejection_Stars.Value = Convert.ToDecimal(SubFrameNumericLists.Stars.Max());
                NumericUpDown_Rejection_StarResidual.Value = Convert.ToDecimal(SubFrameNumericLists.StarResidual.Max());
                NumericUpDown_Rejection_Snr.Value = Convert.ToDecimal(SubFrameNumericLists.Snr.Max());
            }
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

            Label_File_Selection_SubFrameOverhead.Text = ImageParameterLists.CalculateOverhead(mFileList);
            string stepsPerDegree = ImageParameterLists.CalculateFocuserTemperatureCompensationCoefficient();
            Label_TempratureCompensation.Text = "Temperature Coefficient: " + stepsPerDegree;

            // Need to add calculations for average capture duration/overhead
            // **********************************************************************

            SetUISubFrameGroupBoxState();
            FindMultipleFilters();

            if (ComboBox_TargetName.Items.Count != 0)
            {
                ComboBox_TargetName.SelectedIndex = 0;
            }
            
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
                Label_UpdateFileName.Text = Path.GetDirectoryName(renameTuple.Item2) + "\n" + Path.GetFileName(renameTuple.Item2);
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
            GroupBox_DirectorySelection.Enabled = false;
            GroupBox_RenameOrder.Enabled = false;

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
                    GroupBox_DirectorySelection.Enabled = true;
                    GroupBox_RenameOrder.Enabled = true;
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
                Label_UpdateFileName.Text = Label_UpdateFileName.Text = Path.GetDirectoryName(file.SourceFileName) + "\n" + Path.GetFileName(file.SourceFileName);
                Application.DoEvents();

                if (bStatus == false)
                {
                    Label_Task.Text = "File Write Error";

                    var result = MessageBox.Show(
                        "File Update Failed.\n\n" + Label_UpdateFileName.Text,
                        "\nMainForm.cs Button_Update_Click()",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    GroupBox_DirectorySelection.Enabled = true;
                    GroupBox_RenameOrder.Enabled = true;
                    return;
                }
            }

            Label_Task.Text = mFileList.Count().ToString() + " Images Updated";
            GroupBox_DirectorySelection.Enabled = true;
            GroupBox_RenameOrder.Enabled = true;
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
                NumericUpDown_Rejection_Noise.Value = Convert.ToDecimal(SubFrameNumericLists.Noise.Max());
                NumericUpDown_Rejection_AirMass.Value = Convert.ToDecimal(SubFrameNumericLists.AirMass.Max());
                NumericUpDown_Rejection_Stars.Value = Convert.ToDecimal(SubFrameNumericLists.Stars.Max());
                NumericUpDown_Rejection_StarResidual.Value = Convert.ToDecimal(SubFrameNumericLists.StarResidual.Max());
                NumericUpDown_Rejection_Snr.Value = Convert.ToDecimal(SubFrameNumericLists.Snr.Max());
            }

            SetUISubFrameGroupBoxState();

            XisfFileUpdate.Operation = XisfFileUpdate.OperationEnum.NEW_WEIGHTS;
        }

        private void TextBox_FwhmRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mFwhmRangeHigh = ValidateRangeValue(TextBox_FwhmRangeHigh);
        }

        private void TextBox_FwhmRangeLow_TextChanged(object sender, EventArgs e)
        {
            mFwhmRangeLow = ValidateRangeValue(TextBox_FwhmRangeLow);
        }

        private void TextBox_EccentricityRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mEccentricityRangeHigh = ValidateRangeValue(TextBox_EccentricityRangeHigh);
        }

        private void TextBox_EccentricityRangeLow_TextChanged(object sender, EventArgs e)
        {
            mEccentricityRangeLow = ValidateRangeValue(TextBox_EccentricityRangeLow);
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

        private void TextBox_MedianRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mMedianRangeHigh = ValidateRangeValue(TextBox_MedianRangeHigh);
        }

        private void TextBox_MedianRangeLow_TextChanged(object sender, EventArgs e)
        {
            mMedianRangeLow = ValidateRangeValue(TextBox_MedianRangeLow);
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

        private void TextBox_StarsRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mStarsRangeHigh = ValidateRangeValue(TextBox_StarRangeHigh);
        }

        private void TextBox_StarsRangeLow_TextChanged(object sender, EventArgs e)
        {
            mStarsRangeLow = ValidateRangeValue(TextBox_StarRangeLow);
        }

        private void TextBox_StarResidualRangeHigh_TextChanged(object sender, EventArgs e)
        {
            mStarResidualRangeHigh = ValidateRangeValue(TextBox_StarResidualRangeHigh);
        }

        private void TextBox_StarResidualRangeLow_TextChanged(object sender, EventArgs e)
        {
            mStarResidualRangeLow = ValidateRangeValue(TextBox_StarResidualRangeLow);
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
            if (SubFrameNumericLists.ValidatenumericLists(mFileList.Count) == SubFrameNumericListsValidEnum.VALID)
            {
                RadioButton_SetImageStatistics_KeepWeights.Text = "Keep Existing Weights";
                RadioButton_SetImageStatistics_RescaleWeights.Enabled = true;
                RadioButton_SetImageStatistics_CalculateWeights.Enabled = true;

                UpdateWeightCalculations();
            }
            else
            {
                RadioButton_SetImageStatistics_KeepWeights.Text = "Read CSV File";
                RadioButton_SetImageStatistics_RescaleWeights.Enabled = false;
                RadioButton_SetImageStatistics_CalculateWeights.Enabled = false;
            }

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
                 NumericUpDown_Rejection_Median.Value,
                 NumericUpDown_Rejection_Noise.Value,
                 NumericUpDown_Rejection_AirMass.Value,
                 NumericUpDown_Rejection_Stars.Value,
                 NumericUpDown_Rejection_StarResidual.Value,
                 NumericUpDown_Rejection_Snr.Value).ToString();
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

                }
                else
                {
                    GroupBox_InitialRejectionCriteria.Enabled = false;


                    GroupBox_EccentricityWeight.Enabled = false;
                    GroupBox_AirMassWeight.Enabled = false;
                    GroupBox_FwhmWeight.Enabled = false;

                    GroupBox_MedianWeight.Enabled = false;

                    GroupBox_NoiseWeight.Enabled = false;
                    GroupBox_SnrWeight.Enabled = false;
                    GroupBox_StarResidual.Enabled = false;

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
                XisfFileUpdate.Operation = XisfFileUpdate.OperationEnum.CALCULATED_WEIGHTS;
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

        private void NumericUpDown_Rejection_Noise_ValueChanged(object sender, EventArgs e)
        {
            SetUISubFrameGroupBoxState();
        }

        private void NumericUpDown_Rejection_AirMass_ValueChanged(object sender, EventArgs e)
        {
            SetUISubFrameGroupBoxState();
        }

        private void NumericUpDown_Rejection_Stars_ValueChanged(object sender, EventArgs e)
        {
            SetUISubFrameGroupBoxState();
        }

        private void NumericUpDown_Rejection_StarResidual_ValueChanged(object sender, EventArgs e)
        {
            SetUISubFrameGroupBoxState();
        }

        private void NumericUpDown_Rejection_Snr_ValueChanged(object sender, EventArgs e)
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


            Label_FwhmMeanValue.Text = SubFrameNumericLists.Fwhm.Average().ToString("F2");
            Label_FwhmMedianValue.Text = SubFrameNumericLists.Fwhm.Median().ToString("F2");
            Label_FwhmMinValue.Text = SubFrameNumericLists.Fwhm.Min().ToString("F2");
            Label_FwhmMaxValue.Text = SubFrameNumericLists.Fwhm.Max().ToString("F2");
            Label_FwhmSigmaValue.Text = SubFrameNumericLists.Fwhm.StandardDeviation().ToString("F2");

            Label_EccentricityMeanValue.Text = SubFrameNumericLists.Eccentricity.Average().ToString("F2");
            Label_EccentricityMedianValue.Text = SubFrameNumericLists.Eccentricity.Median().ToString("F2");
            Label_EccentricityMinValue.Text = SubFrameNumericLists.Eccentricity.Min().ToString("F2");
            Label_EccentricityMaxValue.Text = SubFrameNumericLists.Eccentricity.Max().ToString("F2");
            Label_EccentricitySigmaValue.Text = SubFrameNumericLists.Eccentricity.StandardDeviation().ToString("F2");

            Label_MedianMeanValue.Text = SubFrameNumericLists.Median.Average().ToString("F0");
            Label_MedianMedianValue.Text = SubFrameNumericLists.Median.Median().ToString("F0");
            Label_MedianMinValue.Text = SubFrameNumericLists.Median.Min().ToString("F0");
            Label_MedianMaxValue.Text = SubFrameNumericLists.Median.Max().ToString("F0");
            Label_MedianSigmaValue.Text = SubFrameNumericLists.Median.StandardDeviation().ToString("F2");

            Label_NoiseMeanValue.Text = SubFrameNumericLists.Noise.Average().ToString("F2");
            Label_NoiseMedianValue.Text = SubFrameNumericLists.Noise.Median().ToString("F2");
            Label_NoiseMinValue.Text = SubFrameNumericLists.Noise.Min().ToString("F2");
            Label_NoiseMaxValue.Text = SubFrameNumericLists.Noise.Max().ToString("F2");
            Label_NoiseSigmaValue.Text = SubFrameNumericLists.Noise.StandardDeviation().ToString("F2");

            Label_AirMassMeanValue.Text = SubFrameNumericLists.AirMass.Average().ToString("F2");
            Label_AirMassMedianValue.Text = SubFrameNumericLists.AirMass.Median().ToString("F2");
            Label_AirMassMinValue.Text = SubFrameNumericLists.AirMass.Min().ToString("F2");
            Label_AirMassMaxValue.Text = SubFrameNumericLists.AirMass.Max().ToString("F2");
            Label_AirMassSigmaValue.Text = SubFrameNumericLists.AirMass.StandardDeviation().ToString("F2");

            Label_StarsMeanValue.Text = SubFrameNumericLists.Stars.Average().ToString("F0");
            Label_StarsMedianValue.Text = SubFrameNumericLists.Stars.Median().ToString("F0");
            Label_StarsMinValue.Text = SubFrameNumericLists.Stars.Min().ToString("F0");
            Label_StarsMaxValue.Text = SubFrameNumericLists.Stars.Max().ToString("F0");
            Label_StarsSigmaValue.Text = SubFrameNumericLists.Stars.StandardDeviation().ToString("F2");

            Label_StarResidualMeanValue.Text = SubFrameNumericLists.StarResidual.Average().ToString("F2");
            Label_StarResidualMedianValue.Text = SubFrameNumericLists.StarResidual.Median().ToString("F2");
            Label_StarResidualMinValue.Text = SubFrameNumericLists.StarResidual.Min().ToString("F2");
            Label_StarResidualMaxValue.Text = SubFrameNumericLists.StarResidual.Max().ToString("F2");
            Label_StarResidualSigmaValue.Text = SubFrameNumericLists.StarResidual.StandardDeviation().ToString("F2");

            Label_SnrMeanValue.Text = SubFrameNumericLists.Snr.Average().ToString("F2");
            Label_SnrMedianValue.Text = SubFrameNumericLists.Snr.Median().ToString("F2");
            Label_SnrMinValue.Text = SubFrameNumericLists.Snr.Min().ToString("F2");
            Label_SnrMaxValue.Text = SubFrameNumericLists.Snr.Max().ToString("F2");
            Label_SnrSigmaValue.Text = SubFrameNumericLists.Snr.StandardDeviation().ToString("F2");

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

                    if (RadioButton_Filter_Shutter.Checked)
                        file.KeywordData.AddKeyword("FILTER", "Shutter");
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

                    if (filter == "Shutter")
                    {
                        RadioButton_Filter_Shutter.ForeColor = Color.Red;
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

                    if (firstFilter == "Shutter")
                    {
                        RadioButton_Filter_Shutter.ForeColor = Color.Red;
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

                    if (firstFilter == "Shutter")
                    {
                        RadioButton_Filter_Shutter.Checked = true;
                    }

                    RadioButton_Filter_Luma.ForeColor = Color.Black;
                    RadioButton_Filter_Red.ForeColor = Color.Black;
                    RadioButton_Filter_Green.ForeColor = Color.Black;
                    RadioButton_Filter_Blue.ForeColor = Color.Black;
                    RadioButton_Filter_Ha.ForeColor = Color.Black;
                    RadioButton_Filter_O3.ForeColor = Color.Black;
                    RadioButton_Filter_S2.ForeColor = Color.Black;
                    RadioButton_Filter_Shutter.ForeColor = Color.Black;
                }
            }
        }
    }
}
