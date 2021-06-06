﻿using System;
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
using XisfFileManager.Forms;

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

            this.Size = new Size(989, 655);
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


        public void UserInputForm_DataAvailable(object sender, EventArgs e)
        {
            UserInputForm UIForm = sender as UserInputForm;
            if (UIForm != null)
            {
                string FormName = UIForm.Name;
                string FormText = UIForm.TextBox_Text.Text;
            }
        }

        public void UserInputForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }


        // ##########################################################################################################################
        // ##########################################################################################################################
        private void Button_Browse_Click(object sender, EventArgs e)
        {
            DirectoryInfo d;

            ProgressBar_OverAll.Value = 0;
            ProgressBar_Keyword_XisfFile.Value = 0;

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
                        mFile.KeywordData.AddKeyword("File", "Name", Path.GetFileName(file.FullName));

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
            // **********************************************************************


            // Need to add calculations for average capture duration/overhead
            // **********************************************************************

            SetUISubFrameGroupBoxState();

            // **********************************************************************
            FindCaptureSoftware();
            FindFrameType();
            FindTelescope();
            FindCamera();
            // **********************************************************************

            if (ComboBox_TargetName.Items.Count != 0)
            {
                ComboBox_TargetName.SelectedIndex = 0;
            }
        }


        private void Button_Rename_Click(object sender, EventArgs e)
        {
            int index = 1;
            int indexIncrement;
            Label_Task.Text = "Renaming " + mFileList.Count().ToString() + " Images";

            ProgressBar_Keyword_XisfFile.Maximum = mFileList.Count();
            ProgressBar_Keyword_XisfFile.Value = 0;

            mRenameFile.MarkDuplicates(mFileList);


            foreach (XisfFile file in mFileList)
            {
                file.Master = CheckBox_Master.Checked;

                Tuple<int, string> renameTuple;
                ProgressBar_Keyword_XisfFile.Value += 1;
                Label_BrowseFileName.Text = file.SourceFileName;

                renameTuple = mRenameFile.RenameFiles(index, file);
                indexIncrement = renameTuple.Item1;
                Label_Keyword_UpdateFileName.Text = Path.GetDirectoryName(renameTuple.Item2) + "\n" + Path.GetFileName(renameTuple.Item2);
                Application.DoEvents();


                if (indexIncrement < 0)
                    break;
                index += indexIncrement;
            }
            ProgressBar_Keyword_XisfFile.Value = ProgressBar_Keyword_XisfFile.Maximum;

            Label_Task.Text = mFileList.Count().ToString() + " Images Renamed";

            mFileList.Clear();

            ProgressBar_OverAll.Value = 0;
        }

        private void Button_Update_Click(object sender, EventArgs e)
        {
            bool bStatus;
            GroupBox_DirectorySelection.Enabled = false;
            GroupBox_RenameOrder.Enabled = false;

            Label_Task.Text = "Updating " + mFileList.Count().ToString() + " File Keywords";
            ProgressBar_Keyword_XisfFile.Maximum = mFileList.Count();
            ProgressBar_Keyword_XisfFile.Value = 0;

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
                file.Master = CheckBox_Master.Checked;

                ProgressBar_Keyword_XisfFile.Value += 1;
                bStatus = XisfFileUpdate.UpdateFile(file, SubFrameLists);
                Label_Keyword_UpdateFileName.Text = Label_Keyword_UpdateFileName.Text = Path.GetDirectoryName(file.SourceFileName) + "\n" + Path.GetFileName(file.SourceFileName);
                Application.DoEvents();

                if (bStatus == false)
                {
                    Label_Task.Text = "File Write Error";

                    var result = MessageBox.Show(
                        "File Update Failed.\n\n" + Label_Keyword_UpdateFileName.Text,
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

            if (RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.Checked == true)
            {
                this.Size = new Size(989, 1147);
            }
            else
            {
                this.Size = new Size(989, 655);
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


        private void FindCaptureSoftware()
        {
            RadioButton_KeywordSoftware_TSX.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_SGP.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_VOY.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_SCP.ForeColor = Color.Black;

            RadioButton_KeywordSoftware_TSX.Checked = false;
            RadioButton_KeywordSoftware_SGP.Checked = false;
            RadioButton_KeywordSoftware_VOY.Checked = false;
            RadioButton_KeywordSoftware_SCP.Checked = false;

            Button_KeywordSoftware_SetAll.ForeColor = Color.Black;
            Button_KeywordSoftware_SetByFile.ForeColor = Color.Black;

            // Now check each and every source file for different or the same capture software
            // If identical, do nothing. If different, make all found UI software labels red 
            bool foundTSX = false;
            bool foundSGP = false;
            bool foundVOY = false;
            bool foundSCP = false;

            int count = 0;
            foreach (XisfFile file in mFileList)
            {
                string program;

                program = file.KeywordData.CaptureSoftware();

                if (program == "TSX")
                {
                    foundTSX = true;
                    count++;
                }

                if (program == "SGP")
                {
                    foundSGP = true;
                    count++;
                }

                if (program == "VOY")
                {
                    foundVOY = true;
                    count++;
                }

                if (program == "SCP")
                {
                    foundSCP = true;
                    count++;
                }
            }

            if (foundTSX)
            {
                if (foundSGP | foundVOY | foundSCP)
                {
                    RadioButton_KeywordSoftware_TSX.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_TSX.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_TSX.ForeColor = Color.Black;
                    RadioButton_KeywordSoftware_TSX.Checked = true;
                }
            }

            if (foundSGP)
            {
                if (foundTSX | foundVOY | foundSCP)
                {
                    RadioButton_KeywordSoftware_SGP.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_SGP.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_SGP.ForeColor = Color.Black;
                    RadioButton_KeywordSoftware_SGP.Checked = true;
                }
            }

            if (foundVOY)
            {
                if (foundTSX | foundSGP | foundSCP)
                {
                    RadioButton_KeywordSoftware_VOY.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_VOY.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_VOY.ForeColor = Color.Black;
                    RadioButton_KeywordSoftware_VOY.Checked = true;
                }
            }

            if (foundSCP)
            {
                if (foundTSX | foundSGP | foundVOY)
                {
                    RadioButton_KeywordSoftware_SCP.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_SCP.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_SCP.ForeColor = Color.Black;
                    RadioButton_KeywordSoftware_SCP.Checked = true;
                }
            }

            if (foundTSX ^ foundSGP ^ foundVOY ^ foundSCP)
            {
                // Set "SetAll" to black if only a single software program was found
                Button_KeywordSoftware_SetAll.ForeColor = Color.Black;
            }
            else
            {
                // More that one software program - set "SetByFile" to red
                Button_KeywordSoftware_SetAll.ForeColor = Color.Red;
            }

            if (count != mFileList.Count)
            {
                // The number of source files didn't equal the number of files with a known software program
                // Set "SetByFile" to red
                Button_KeywordSoftware_SetByFile.ForeColor = Color.Red;
            }
        }

        private void Button_CaptureSoftware_SetAll_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordSoftware_TSX.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "TSX");
                }

                if (RadioButton_KeywordSoftware_SGP.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "SGP");
                }

                if (RadioButton_KeywordSoftware_VOY.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "VOY");
                }

                if (RadioButton_KeywordSoftware_SCP.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "SCP");
                }
            }

            if (count == 0)
            {
                return;
            }

            RadioButton_KeywordSoftware_TSX.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_SGP.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_VOY.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_SCP.ForeColor = Color.Black;

            Button_KeywordSoftware_SetAll.ForeColor = Color.Black;
            Button_KeywordSoftware_SetByFile.ForeColor = Color.Black;

            FindCaptureSoftware();
        }


        private void Button_CaptureSoftware_SetByFile_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                file.KeywordData.CaptureSoftware(true);
            }

            FindCaptureSoftware();
        }


        private void FindTelescope()
        {
            string telescope;
            string firstTelescope;
            bool multipleTelescopes = false;
            bool multipleReducers = false;

            if (mFileList.Count == 0)
                return;

            firstTelescope = mFileList[0].KeywordData.Telescope(false);

            if (firstTelescope.EndsWith("R"))
            {
                CheckBox_KeywordTelescope_Riccardi.Checked = true;
                multipleReducers = false;
            }
            else
            {
                CheckBox_KeywordTelescope_Riccardi.Checked = false;
                multipleReducers = false;
            }

            if (firstTelescope.Contains("APM107"))
            {
                RadioButton_KeywordTelescope_APM107.Checked = true;
            }

            if (firstTelescope.Contains("EVO150"))
            {
                RadioButton_KeywordTelescope_EVO150.Checked = true;
            }

            if (firstTelescope.Contains("NWT254"))
            {
                RadioButton_KeywordTelescope_NWT254.Checked = true;
            }

            RadioButton_KeywordTelescope_APM107.ForeColor = Color.Black;
            RadioButton_KeywordTelescope_EVO150.ForeColor = Color.Black;
            RadioButton_KeywordTelescope_NWT254.ForeColor = Color.Black;
            CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Black;


            int count = 0;
            foreach (XisfFile file in mFileList)
            {
                telescope = file.KeywordData.Telescope(false);
                if (telescope != firstTelescope)
                {
                    if (telescope.EndsWith("R"))
                    {
                        if (multipleReducers == false)
                        {
                            multipleReducers = true;
                            CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        if (multipleReducers == true)
                        {
                            multipleReducers = true;
                            CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Red;
                        }
                    }

                    if (telescope.Contains("APM107"))
                    {
                        count++;
                        RadioButton_KeywordTelescope_APM107.ForeColor = Color.Red;
                        multipleTelescopes = true;
                    }

                    if (telescope.Contains("EVO150"))
                    {
                        count++;
                        RadioButton_KeywordTelescope_EVO150.ForeColor = Color.Red;
                        multipleTelescopes = true;
                    }

                    if (telescope.Contains("NWT254"))
                    {
                        count++;
                        RadioButton_KeywordTelescope_NWT254.ForeColor = Color.Red;
                        multipleTelescopes = true;
                    }
                }

                if (count == 0)
                {
                    Button_KeywordTelescope_SetByFile.ForeColor = Color.Black;
                }
                else
                {
                    if (count != mFileList.Count)
                    {
                        Button_KeywordTelescope_SetByFile.ForeColor = Color.Red;
                    }
                }

                if (multipleReducers)
                {
                    CheckBox_KeywordTelescope_Riccardi.Checked = false;
                    CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Red;
                }
                else
                {
                    CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Black;
                }

                if (multipleTelescopes)
                {
                    RadioButton_KeywordTelescope_APM107.Checked = false;
                    RadioButton_KeywordTelescope_EVO150.Checked = false;
                    RadioButton_KeywordTelescope_NWT254.Checked = false;
                    CheckBox_KeywordTelescope_Riccardi.Checked = false;

                    if (firstTelescope.Contains("APM107"))
                    {
                        RadioButton_KeywordTelescope_APM107.ForeColor = Color.Red;
                    }

                    if (firstTelescope.Contains("EVO150"))
                    {
                        RadioButton_KeywordTelescope_EVO150.ForeColor = Color.Red;
                    }

                    if (firstTelescope.Contains("NWT254"))
                    {
                        RadioButton_KeywordTelescope_NWT254.ForeColor = Color.Red;
                    }
                }
                else
                {
                    if (firstTelescope.Contains("APM107"))
                    {
                        RadioButton_KeywordTelescope_APM107.Checked = true;
                    }

                    if (firstTelescope.Contains("EVO150"))
                    {
                        RadioButton_KeywordTelescope_EVO150.Checked = true;
                    }

                    if (firstTelescope.Contains("NWT254"))
                    {
                        RadioButton_KeywordTelescope_NWT254.Checked = true;
                    }

                    RadioButton_KeywordTelescope_APM107.ForeColor = Color.Black;
                    RadioButton_KeywordTelescope_EVO150.ForeColor = Color.Black;
                    RadioButton_KeywordTelescope_NWT254.ForeColor = Color.Black;
                }
            }
        }

        private void Button_Telescope_SetAll_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordTelescope_APM107.Checked)
                {
                    count++;
                    if (CheckBox_KeywordTelescope_Riccardi.Checked)
                    {
                        file.KeywordData.AddKeyword("TELESCOP", "APM107R", "w/Riccardi 0.75 Reducer");
                        file.KeywordData.AddKeyword("FOCALLEN", 525, "w/Riccardi 0.75 Reducer");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("TELESCOP", "APM107", "No Reducer");
                        file.KeywordData.AddKeyword("FOCALLEN", 700, "No Reducer");
                    }
                }

                if (RadioButton_KeywordTelescope_EVO150.Checked)
                {
                    count++;
                    if (CheckBox_KeywordTelescope_Riccardi.Checked)
                    {
                        file.KeywordData.AddKeyword("TELESCOP", "EVO150R", "w/Riccardi 0.75 Reducer");
                        file.KeywordData.AddKeyword("FOCALLEN", 750, "w/Riccardi 0.75 Reducer");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("TELESCOP", "EVO150", "No Reducer");
                        file.KeywordData.AddKeyword("FOCALLEN", 1000, "No Reducer");
                    }
                }

                if (RadioButton_KeywordTelescope_NWT254.Checked)
                {
                    count++;
                    if (CheckBox_KeywordTelescope_Riccardi.Checked)
                    {
                        file.KeywordData.AddKeyword("TELESCOP", "NWT254R", "w/Riccardi 0.75 Reducer");
                        file.KeywordData.AddKeyword("FOCALLEN", 825, "w/Riccardi 0.75 Reducer");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("TELESCOP", "NWT254", "No Reducer");
                        file.KeywordData.AddKeyword("FOCALLEN", 1100, "No Reducer");
                    }
                }
            }

            if (count == 0)
            {
                return;
            }

            RadioButton_KeywordTelescope_APM107.ForeColor = Color.Black;
            RadioButton_KeywordTelescope_EVO150.ForeColor = Color.Black;
            RadioButton_KeywordTelescope_NWT254.ForeColor = Color.Black;
            CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Black;

            Button_KeywordTelescope_SetByFile.ForeColor = Color.Black;

            FindTelescope();
        }

        private void Button_Telescope_SetByFile_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                file.KeywordData.Telescope(true);
            }

            FindTelescope();
        }

        private void CheckBox_CameraNarrowBand_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_CameraNarrowBand.Checked)
            {
                TextBox_CameraZ533Gain.Text = "100";
                TextBox_CameraZ533Offset.Text = "50";

                TextBox_CameraZ183Gain.Text = "111";
                TextBox_CameraZ183Offset.Text = "10";

                TextBox_CameraQ178Gain.Text = "40";
                TextBox_CameraQ178Offset.Text = "15";
            }
            else
            {
                TextBox_CameraZ533Gain.Text = "100";
                TextBox_CameraZ533Offset.Text = "50";

                TextBox_CameraZ183Gain.Text = "53";
                TextBox_CameraZ183Offset.Text = "10";

                TextBox_CameraQ178Gain.Text = "40";
                TextBox_CameraQ178Offset.Text = "15";
            }
        }

        private void Button_KeywordImageTypeFrame_SetByFile_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                file.KeywordData.FrameType(true);
                file.KeywordData.FilterName(true);
            }

            FindFrameType();
        }

        private void Button_KeywordImageTypeFrame_SetAll_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordImageFrame_Light.Checked)
                    file.KeywordData.AddKeyword("IMAGETYP", "Light");

                if (RadioButton_KeywordImageFrame_Dark.Checked)
                {
                    if (CheckBox_KeywordImageFrame_Master.Checked)
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Master Dark");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Dark");
                    }
                }

                if (RadioButton_KeywordImageFrame_Flat.Checked)
                {
                    if (CheckBox_KeywordImageFrame_Master.Checked)
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Master Flat");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Flat");
                    }
                }

                if (RadioButton_KeywordImageFrame_Bias.Checked)
                {
                    if (CheckBox_KeywordImageFrame_Master.Checked)
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Master Bias");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Bias");
                    }
                }
            }

            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordImageFilter_Luma.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Luma");

                if (RadioButton_KeywordImageFilter_Red.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Red");

                if (RadioButton_KeywordImageFilter_Green.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Green");

                if (RadioButton_KeywordImageFilter_Blue.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Blue");

                if (RadioButton_KeywordImageFilter_Ha.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Ha");

                if (RadioButton_KeywordImageFilter_O3.Checked)
                    file.KeywordData.AddKeyword("FILTER", "O3");

                if (RadioButton_KeywordImageFilter_S2.Checked)
                    file.KeywordData.AddKeyword("FILTER", "S2");

                if (RadioButton_KeywordImageFilter_Shutter.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Shutter");
            }

            FindFrameType();
        }

        public void FindFrameType()
        {
            string filter;
            int filterCount;

            bool foundLuma = false;
            bool foundRed = false;
            bool foundGreen = false;
            bool foundBlue = false;
            bool foundHa = false;
            bool foundO3 = false;
            bool foundS2 = false;
            bool foundShutter = false;

            RadioButton_KeywordImageFilter_Luma.ForeColor = Color.Black;
            RadioButton_KeywordImageFilter_Red.ForeColor = Color.Black;
            RadioButton_KeywordImageFilter_Green.ForeColor = Color.Black;
            RadioButton_KeywordImageFilter_Blue.ForeColor = Color.Black;
            RadioButton_KeywordImageFilter_Ha.ForeColor = Color.Black;
            RadioButton_KeywordImageFilter_O3.ForeColor = Color.Black;
            RadioButton_KeywordImageFilter_S2.ForeColor = Color.Black;
            RadioButton_KeywordImageFilter_Shutter.ForeColor = Color.Black;

            RadioButton_KeywordImageFilter_Luma.Checked = false;
            RadioButton_KeywordImageFilter_Red.Checked = false;
            RadioButton_KeywordImageFilter_Green.Checked = false;
            RadioButton_KeywordImageFilter_Blue.Checked = false;
            RadioButton_KeywordImageFilter_Ha.Checked = false;
            RadioButton_KeywordImageFilter_O3.Checked = false;
            RadioButton_KeywordImageFilter_S2.Checked = false;
            RadioButton_KeywordImageFilter_Shutter.Checked = false;

            if (mFileList.Count == 0)
            {
                Button_KeywordImage_SetAll.ForeColor = Color.Black;
                Button_KeywordImage_SetByFile.ForeColor = Color.Black;
                return;
            }

            filterCount = 0;
            foreach (XisfFile file in mFileList)
            {
                filter = file.KeywordData.FilterName();
                if (filter == "Luma")
                {
                    foundLuma = true;
                    filterCount++;
                }

                if (filter == "Red")
                {
                    foundRed = true;
                    filterCount++;
                }

                if (filter == "Green")
                {
                    foundGreen = true;
                    filterCount++;
                }

                if (filter == "Blue")
                {
                   foundBlue = true;
                    filterCount++;
                }

                if (filter == "Ha")
                {
                    foundHa = true;
                    filterCount++;
                }

                if (filter == "O3")
                {
                   foundO3 = true;
                    filterCount++;
                }

                if (filter == "S2")
                {
                   foundS2 = true;
                    filterCount++;
                }

                if (filter == "Shutter")
                {
                    foundShutter = true;
                    filterCount++;
                }
            }

            if (foundLuma)
            {
                if (foundRed ^ foundGreen ^ foundBlue ^ foundHa ^ foundO3 ^ foundS2 ^ foundShutter)
                {
                    RadioButton_KeywordImageFilter_Luma.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_Luma.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_Luma.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_Luma.Checked = true;
                }
            }

            if (foundRed)
            {
                if (foundLuma ^ foundGreen ^ foundBlue ^ foundHa ^ foundO3 ^ foundS2 ^ foundShutter)
                {
                    RadioButton_KeywordImageFilter_Red.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_Red.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_Red.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_Red.Checked = true;
                }
            }

            if (foundGreen)
            {
                if (foundLuma ^ foundRed ^ foundBlue ^ foundHa ^ foundO3 ^ foundS2 ^ foundShutter)
                {
                    RadioButton_KeywordImageFilter_Green.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_Green.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_Green.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_Green.Checked = true;
                }
            }

            if (foundBlue)
            {
                if (foundLuma ^ foundRed ^ foundGreen ^ foundHa ^ foundO3 ^ foundS2 ^ foundShutter)
                {
                    RadioButton_KeywordImageFilter_Blue.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_Blue.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_Blue.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_Blue.Checked = true;
                }
            }

            if (foundHa)
            {
                if (foundLuma ^ foundRed ^ foundGreen ^ foundBlue ^ foundO3 ^ foundS2 ^ foundShutter)
                {
                    RadioButton_KeywordImageFilter_Ha.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_Ha.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_Ha.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_Ha.Checked = true;
                }
            }

            if (foundO3)
            {
                if (foundLuma ^ foundRed ^ foundGreen ^ foundBlue ^ foundHa ^ foundS2 ^ foundShutter)
                {
                    RadioButton_KeywordImageFilter_O3.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_O3.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_O3.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_O3.Checked = true;
                }
            }

            if (foundS2)
            {
                if (foundLuma ^ foundRed ^ foundGreen ^ foundBlue ^ foundHa ^ foundO3 ^ foundShutter)
                {
                    RadioButton_KeywordImageFilter_S2.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_S2.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_S2.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_S2.Checked = true;
                }
            }

            if (foundShutter)
            {
                if (foundLuma ^ foundRed ^ foundGreen ^ foundBlue ^ foundHa ^ foundO3 ^ foundS2)
                {
                    RadioButton_KeywordImageFilter_Shutter.ForeColor = Color.Red;
                    RadioButton_KeywordImageFilter_Shutter.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFilter_Shutter.ForeColor = Color.Black;
                    RadioButton_KeywordImageFilter_Shutter.Checked = true;
                }
            }

            RadioButton_KeywordImageFrame_Light.ForeColor = Color.Black;
            RadioButton_KeywordImageFrame_Dark.ForeColor = Color.Black;
            RadioButton_KeywordImageFrame_Flat.ForeColor = Color.Black;
            RadioButton_KeywordImageFrame_Bias.ForeColor = Color.Black;

            RadioButton_KeywordImageFrame_Light.Checked = false;
            RadioButton_KeywordImageFrame_Dark.Checked = false;
            RadioButton_KeywordImageFrame_Flat.Checked = false;
            RadioButton_KeywordImageFrame_Bias.Checked = false;

            CheckBox_KeywordImageFrame_Master.Checked = false;
            CheckBox_KeywordImageFrame_Master.ForeColor = Color.Black;
    

            Button_KeywordImage_SetAll.ForeColor = Color.Black;
            Button_KeywordImage_SetByFile.ForeColor = Color.Black;

            // Now check each and every source file for different or the same frame type
            // If identical, do nothing. If different, make all found UI labels red 
            bool foundLight = false;
            bool foundDark = false;
            bool foundFlat = false;
            bool foundBias = false;
            int  masterCount;
            int typeCount;
            
            masterCount = 0;
            typeCount = 0;
            foreach (XisfFile file in mFileList)
            {
                string frameType;

                frameType = file.KeywordData.FrameType();

                if (frameType.Contains("Light"))
                {
                    foundLight = true;
                    typeCount++;
                }

                if (frameType.Contains("Dark"))
                {
                    foundDark = true;
                    typeCount++;
                }

                if (frameType.Contains("Flat"))
                {
                    foundFlat = true;
                    typeCount++;
                }

                if (frameType.Contains("Bias"))
                {
                    foundBias = true;
                    typeCount++;
                }

                if (frameType.Contains("Master"))
                {
                    masterCount++;
                }
            }

            if (foundLight)
            {
                if (foundDark || foundFlat || foundBias)
                {
                    RadioButton_KeywordImageFrame_Light.ForeColor = Color.Red;
                    RadioButton_KeywordImageFrame_Light.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFrame_Light.ForeColor = Color.Black;
                    RadioButton_KeywordImageFrame_Light.Checked = true;
                }
            }

            if (foundDark)
            {
                if (foundLight || foundFlat || foundBias)
                {
                    RadioButton_KeywordImageFrame_Dark.ForeColor = Color.Red;
                    RadioButton_KeywordImageFrame_Dark.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFrame_Dark.ForeColor = Color.Black;
                    RadioButton_KeywordImageFrame_Dark.Checked = true;
                }
            }

            if (foundFlat)
            {
                if (foundLight || foundDark || foundBias)
                {
                    RadioButton_KeywordImageFrame_Flat.ForeColor = Color.Red;
                    RadioButton_KeywordImageFrame_Flat.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFrame_Flat.ForeColor = Color.Black;
                    RadioButton_KeywordImageFrame_Flat.Checked = true;
                }
            }

            if (foundBias)
            {
                if (foundLight || foundDark || foundFlat)
                {
                    RadioButton_KeywordImageFrame_Bias.ForeColor = Color.Red;
                    RadioButton_KeywordImageFrame_Bias.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageFrame_Bias.ForeColor = Color.Black;
                    RadioButton_KeywordImageFrame_Bias.Checked = true;
                }
            }


            if ((foundLight ^ foundDark ^ foundFlat ^ foundBias) & (foundLuma ^ foundRed ^ foundGreen ^ foundBlue ^ foundHa ^ foundO3 ^ foundS2 ^ foundShutter))
            {
                // Set "SetAll" to black if only a single filter and a single frame type was found
                Button_KeywordImage_SetAll.ForeColor = Color.Black;
            }
            else
            {
                // More that one software program - set "SetByFile" to red
                Button_KeywordImage_SetAll.ForeColor = Color.Red;
            }

            if (masterCount != mFileList.Count)
            {
                CheckBox_KeywordImageFrame_Master.ForeColor = Color.Red;
                Button_KeywordImage_SetByFile.ForeColor = Color.Red;
            }

            if ((filterCount != mFileList.Count) || (typeCount != mFileList.Count))
            {
                // The number of source files didn't equal the number of files with a known filter
                // Set "SetByFile" to red
                Button_KeywordImage_SetByFile.ForeColor = Color.Red;
            }
        }

        public void FindCamera()
        {
            if (mFileList.Count == 0)
            {
                return;
            }

            RadioButton_KeywordCamera_Z533.Checked = false;
            RadioButton_KeywordCamera_Z533.ForeColor = Color.Black;

            RadioButton_KeywordCamera_Z183.Checked = false;
            RadioButton_KeywordCamera_Z183.ForeColor = Color.Black;

            RadioButton_KeywordCamera_Q178.Checked = false;
            RadioButton_KeywordCamera_Q178.ForeColor = Color.Black;

            RadioButton_KeywordCamera_A144.Checked = false;
            RadioButton_KeywordCamera_A144.ForeColor = Color.Black;


            int cameraCount = 0;
            bool foundZ533 = false;
            bool foundZ183 = false;
            bool foundQ178 = false;
            bool foundA144 = false;
            string camera;

            foreach (XisfFile file in mFileList)
            {
                camera = file.KeywordData.Camera();

                if (camera.Equals("Z533"))
                {
                    cameraCount++;
                    foundZ533 = true;
                }

                if (camera.Equals("Z183"))
                {
                    cameraCount++;
                    foundZ183 = true;
                }

                if (camera.Equals("Q178"))
                {
                    cameraCount++;
                    foundQ178 = true;
                }

                if (camera.Equals("A144"))
                {
                    cameraCount++;
                    foundA144 = true;
                }
            }

            if (foundZ533)
            {
                if (foundZ183 | foundQ178 | foundA144)
                {
                    RadioButton_KeywordCamera_Z533.Checked = false;
                    RadioButton_KeywordCamera_Z533.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordCamera_Z533.Checked = true;
                    RadioButton_KeywordCamera_Z533.ForeColor = Color.Black;
                }
            }

            if (foundZ183)
            {
                if (foundZ533 | foundQ178 | foundA144)
                {
                    RadioButton_KeywordCamera_Z183.Checked = false;
                    RadioButton_KeywordCamera_Z183.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordCamera_Z183.Checked = true;
                    RadioButton_KeywordCamera_Z183.ForeColor = Color.Black;
                }
            }

            if (foundQ178)
            {
                if (foundZ533 | foundZ183 | foundA144)
                {
                    RadioButton_KeywordCamera_Q178.Checked = false;
                    RadioButton_KeywordCamera_Q178.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordCamera_Q178.Checked = true;
                    RadioButton_KeywordCamera_Q178.ForeColor = Color.Black;
                }
            }

            if (foundA144)
            {
                if (foundZ533 | foundZ183 | foundQ178)
                {
                    RadioButton_KeywordCamera_A144.Checked = false;
                    RadioButton_KeywordCamera_A144.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordCamera_A144.Checked = true;
                    RadioButton_KeywordCamera_A144.ForeColor = Color.Black;
                }
            }

            // ************************************************************************************************

            int gain;
            int offset;
            int gainCount = 0;
            int offsetCount = 0;
            bool foundGain = false;
            bool foundOffset = false;

            foreach (XisfFile file in mFileList)
            {
                gain = file.KeywordData.Gain();

                if (gain != -1)
                {
                    gainCount++;
                    foundGain = true;


                    if (file.KeywordData.Camera().Equals("Z533"))
                    {
                        TextBox_CameraZ533Gain.Text = gain.ToString();
                    }

                    if (file.KeywordData.Camera().Equals("Z183"))
                    {
                        TextBox_CameraZ183Gain.Text = gain.ToString();
                    }

                    if (file.KeywordData.Camera().Equals("Q178"))
                    {
                        TextBox_CameraQ178Gain.Text = gain.ToString();
                    }
                }

                if (gainCount != mFileList.Count)
                {
                    Label_CameraGain.ForeColor = Color.Red;
                }

                // ****************************************************************

                offset = file.KeywordData.Offset();

                if (offset != -1)
                {
                    offsetCount++;
                    foundOffset = true;


                    if (file.KeywordData.Camera().Equals("Z533"))
                    {
                        TextBox_CameraZ533Offset.Text = offset.ToString();
                    }

                    if (file.KeywordData.Camera().Equals("Z183"))
                    {
                        TextBox_CameraZ183Offset.Text = offset.ToString();
                    }

                    if (file.KeywordData.Camera().Equals("Q178"))
                    {
                        TextBox_CameraQ178Offset.Text = offset.ToString();
                    }
                }

                if (offsetCount != mFileList.Count)
                {
                    Label_CameraOffset.ForeColor = Color.Red;
                }
            }


            // ************************************************************************************************
            // ************************************************************************************************

            if ((foundZ533 ^ foundZ183 ^ foundQ178 ^ foundA144) & foundGain & foundOffset)
            {
                Button_KeywordCamera_SetAll.ForeColor = Color.Black;
            }
            else
            {
                Button_KeywordCamera_SetAll.ForeColor = Color.Red;
            }

            if ((cameraCount != mFileList.Count) || (gainCount != mFileList.Count) || (offsetCount != mFileList.Count))
            {
                Button_KeywordCamera_SetByFile.ForeColor = Color.Red;
            }
        }

        private void Button_KeywordCamera_SetAll_Click(object sender, EventArgs e)
        {
            if (mFileList.Count == 0)
            {
                return;
            }

            foreach (XisfFile file in mFileList)
            {
                file.KeywordData.RemoveKeyword("NAXIS3");

                file.KeywordData.AddKeyword("BITPIX", 16, "Bits Per Pixel");
                file.KeywordData.AddKeyword("BSCALE", 1, "Multiply Raw Values by BSCALE");
                file.KeywordData.AddKeyword("BZERO", 32768, "Add value to scale to 65536 (16 bit) values");
                file.KeywordData.AddKeyword("CCD-TEMP", TextBox_CameraSensorTemperature.Text, "Actual Sensor Temperature");
                file.KeywordData.AddKeyword("NAXIS", 2, "XISF File Manager");
                file.KeywordData.AddKeyword("XBINNING", 1, "Horizontal Binning");
                file.KeywordData.AddKeyword("YBINNING", 1, "Vertical Bining");

                if (RadioButton_KeywordCamera_Z533.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z533", "ASIZ533MC Pro");
                    file.KeywordData.AddKeyword("NAXIS1", 3008, "XISF File Manager");
                    file.KeywordData.AddKeyword("NAXIS2", 3008, "XISF File Manager");
                    file.KeywordData.AddKeyword("XPIXSZ", 3.76, "XISF File Manager");
                    file.KeywordData.AddKeyword("YPIXSZ", 3.76, "XISF File Manager");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");
                    file.KeywordData.AddKeyword("GAIN", Int32.Parse(TextBox_CameraZ533Gain.Text), "Camera Gain");
                    file.KeywordData.AddKeyword("OFFSET", Int32.Parse(TextBox_CameraZ533Offset.Text), "Camera Offset");
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordCamera_Z183.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z183", "ASIZ183M Pro");
                    file.KeywordData.AddKeyword("NAXIS1", 5496, "XISF File Manager");
                    file.KeywordData.AddKeyword("NAXIS2", 3672, "XISF File Manager");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "XISF File Manager");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "XISF File Manager");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");
                    file.KeywordData.AddKeyword("GAIN", Int32.Parse(TextBox_CameraZ183Gain.Text), "Camera Gain");
                    file.KeywordData.AddKeyword("OFFSET", Int32.Parse(TextBox_CameraZ183Offset.Text), "Camera Offset");
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordCamera_Q178.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Q178", "ASIZ183M Pro");
                    file.KeywordData.AddKeyword("NAXIS1", 3072, "XISF File Manager");
                    file.KeywordData.AddKeyword("NAXIS2", 2048, "XISF File Manager");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "XISF File Manager");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "XISF File Manager");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");
                    file.KeywordData.AddKeyword("GAIN", Int32.Parse(TextBox_CameraQ178Gain.Text), "Camera Gain");
                    file.KeywordData.AddKeyword("OFFSET", Int32.Parse(TextBox_CameraQ178Offset.Text), "Camera Offset");
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordCamera_A144.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "A144", "Atik Infinity");
                    file.KeywordData.AddKeyword("NAXIS1", 1392, "XISF File Manager");
                    file.KeywordData.AddKeyword("NAXIS2", 1040, "XISF File Manager");
                    file.KeywordData.AddKeyword("XPIXSZ", 6.45, "XISF File Manager");
                    file.KeywordData.AddKeyword("YPIXSZ", 6.45, "XISF File Manager");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");
                    file.KeywordData.RemoveKeyword("GAIN");
                    file.KeywordData.RemoveKeyword("OFFSET");
                    file.KeywordData.SetEGain();
                }
            }
        }

        private void Button_KeywordCamera_SetByFile_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox_KeywordImageFrame_Master_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_KeywordImageFrame_Master.Checked)
            {
                ComboBox_TargetName.Text = "Master";
                CheckBox_SubFrameKeywords_UpdateTargetName.Checked = true;
            }
        }
    }
}
