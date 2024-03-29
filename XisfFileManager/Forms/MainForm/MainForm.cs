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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace XisfFileManager
{

    public delegate void DataReceivedEventHandler(CalibrationTabPageValues data);


    // ##########################################################################################################################
    // ##########################################################################################################################
    public partial class MainForm : Form
    {
        private DirectoryOps.FileType mFileType = DirectoryOps.FileType.NO_MASTERS;
        private List<XisfFile> mFileList;
        private OpenFileDialog mFileCsv;
        private OpenFolderDialog mFolder;
        private XisfFile mFile;
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
        private eSubFrameNumericListsValid eSubFrameValidListsValid;
        private Calibration mCalibration;
        private DirectoryOps mDirectoryOps;
        private readonly ImageCalculations ImageParameterLists;
        private readonly SubFrameLists SubFrameLists;
        private readonly SubFrameNumericLists SubFrameNumericLists;
        private readonly XisfFileReader mFileReader;
        private readonly XisfFileRename mRenameFile;
        private string mFolderBrowseState;
        private string mFolderCsvBrowseState;



        public MainForm()
        {
            InitializeComponent();
            CalibrationTabPageEvent.CalibrationTabPage_InvokeEvent += EventHandler_UpdateCalibrationPageForm;

            mDirectoryOps = new DirectoryOps();
            mCalibration = new Calibration();
            mFileReader = new XisfFileReader();

            Label_FileSelection_Statistics_Task.Text = "";
            mFileList = new List<XisfFile>();
            mRenameFile = new XisfFileRename
            {
                RenameOrder = XisfFileRename.OrderType.INDEX
            };

            // This set of lists conatin the data that was read from PixInsight's SubFrameSelector or from an image file that was updated with SubframeSlector Data.
            // Data is stored as Keyword XML nodes - strings
            SubFrameLists = new SubFrameLists();

            // This set of lists contains only numeric values (not XML node strings) to be used for weight calculations  
            SubFrameNumericLists = new SubFrameNumericLists();

            // This set of a much smaller number of numeric lists contains per image data used for Focuser Temperature compensation coefficient calculation and SSWEIGHTs
            ImageParameterLists = new ImageCalculations();

            Label_FileSelection_Statistics_Task.Text = "No Images Selected";
            Label_FileSelection_Statistics_TempratureCompensation.Text = "Temperature Coefficient: Not Computed";


            // Version Number
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                Version version = ad.CurrentVersion;
                Text = "XISF File Manager - Version: " + version.ToString();
            }
            else
            {
                Text = "XISF File Manager - Version: " + System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyyy.MM.dd - h:mm tt");
            }

            Utility.ToolTips.AddToolTip(RadioButton_FileSelection_Index_ByFilter, "Orders Files by Capture Time per Filter", "\"By Target\" orders each filter's files consecutively.\r\n\"By Night\" orders each filter's files consecutively by night.");
            Utility.ToolTips.AddToolTip(RadioButton_FileSelection_Index_ByTime, "Orders Files by Capture Time", "\"By Target\" orders all files consecutively.\r\n\"By Night\" orders all files consecutively by night.");
        }

        // ****************************************************************************************************************
        // ************************ Event Handlers ************************************************************************
        // ****************************************************************************************************************
        private void EventHandler_UpdateCalibrationPageForm(CalibrationTabPageValues data)
        {
            ProgressBar_CalibrationTab.Maximum = data.ProgressMax;
            ProgressBar_CalibrationTab.Value = data.Progress;
            Label_CalibrationTab_ReadFileName.Text = data.FileName;
            Label_CalibrationTab_TotalFiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Label_CalibrationTab_TotalFiles.Text = "Found " + data.TotalFiles.ToString() + " Calibration Library Files";

            switch (data.MessageMode)
            {
                case CalibrationTabPageValues.eMessageMode.CLEAR:
                    TextBox_CalibrationTab_Messgaes.Clear();
                    break;

                case CalibrationTabPageValues.eMessageMode.APPEND:
                    TextBox_CalibrationTab_Messgaes.AppendText(data.MatchCalibrationMessage);
                    break;

                case CalibrationTabPageValues.eMessageMode.NEW:
                    TextBox_CalibrationTab_Messgaes.Clear();
                    TextBox_CalibrationTab_Messgaes.AppendText(data.MatchCalibrationMessage);
                    break;

                default:
                    break;

            }
            data.MessageMode = CalibrationTabPageValues.eMessageMode.KEEP;


            Label_CalibrationTab_TotalMatchedFiles.Text = "Matched " + data.TotalMatchedCalibrationFiles.ToString() + " Calibration Files: " +
                data.TotalUniqueDarkCalibrationFiles.ToString() + " Unique Darks, " +
                data.TotalUniqueFlatCalibrationFiles.ToString() + " Unique Flats and " +
                data.TotalUniqueBiasCalibrationFiles.ToString() + " Unique Bias Files " +
                "from " + mFileList.Count.ToString() + " Target Frames";


            TextBox_CalibrationTab_MatchingTolerance_Exposure.Text = mCalibration.ExposureTolerance.ToString();
            TextBox_CalibrationTab_MatchingTolerance_Gain.Text = mCalibration.GainTolerance.ToString();
            TextBox_CalibrationTab_MatchingTolerance_Offset.Text = mCalibration.OffsetTolerance.ToString();
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Text = mCalibration.TemperatureTolerance.ToString();

            TabPage_Calibration.Update();
        }

        // ****************************************************************************************************************
        // ****************************************************************************************************************

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
        private async void Button_Browse_Click(object sender, EventArgs e)
        {
            XisfFileReader fileReader = new XisfFileReader();
            // Clear all lists - we are reading or re-reading what will become a new xisf file data set that will invalidate any existing data.         
            mFileList.Clear();
            SubFrameLists.Clear();
            SubFrameNumericLists.Clear();
            ImageParameterLists.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text = "";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Items.Clear();
            ClearForm();

            ProgressBar_FileSelection_ReadProgress.Value = 0;
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;
            TabControl_Update.Enabled = false;

            mFolder = new OpenFolderDialog()
            {
                Title = "Select .xisf Folder",
                //AutoUpgradeEnabled = true,
                CheckPathExists = false,

                InitialDirectory = mFolderBrowseState,
                // InitialDirectory = @"E:\Temp\maste", // @"E:\Photography\Astro Photography\Processing",

                Multiselect = true,
                RestoreDirectory = true
            };

            if (mFolder.ShowDialog(IntPtr.Zero).Equals(DialogResult.OK) == false)
            {
                return;
            }

            mFolderBrowseState = mFolder.SelectedPaths[0];
            DirectoryInfo diDirectoryTree = new DirectoryInfo(mFolder.SelectedPaths[0]);

            mDirectoryOps.ClearFileList();
            mDirectoryOps.Filter = DirectoryOps.FilterType.ALL;
            mDirectoryOps.File = mFileType;
            mDirectoryOps.Camera = DirectoryOps.CameraType.ALL;
            mDirectoryOps.Frame = DirectoryOps.FrameType.ALL;
            mDirectoryOps.Recurse = CheckBox_FileSelection_DirectorySelection_Recurse.Checked;

            mDirectoryOps.RecuseDirectories(diDirectoryTree);

            Label_FileSelection_Statistics_Task.Text = "Reading " + mDirectoryOps.Files.Count.ToString() + " Image Files";
            Label_FileSelection_Statistics_TempratureCompensation.Text = "Temperature Coefficient: Not Computed";
            Label_FileSelection_Statistics_SubFrameOverhead.Text = "SubFrame Overhead: Not Computed";

            ProgressBar_FileSelection_ReadProgress.Value = 0;

            if (mDirectoryOps.Files.Count == 0)
            {
                MessageBox.Show("No .xisf Files Found\nIs this a 'Master' Directory?", "Select .xisf Folder");
                return;
            }

            // Upate the UI with data from the .xisf recursive directory search
            ProgressBar_FileSelection_ReadProgress.Maximum = mDirectoryOps.Files.Count;
            Application.DoEvents();

            foreach (var file in mDirectoryOps.Files)
            {
                Label_FileSelection_BrowseFileName.Text = file.DirectoryName + "\n" + file.Name;
                ProgressBar_FileSelection_ReadProgress.Value += 1;

                // Create a new xisf file instance
                mFile = new XisfFile
                {
                    SourceFileName = file.FullName
                };

                await Task.Run(async () =>
                {
                    await fileReader.ReadXisfFile(mFile);
                });

                mFileList.Add(mFile);
            }

            /*
            foreach (var xFile in mFileList)
            {
                XElement root = xFile.mXDoc.Root;
                XNamespace ns = root.GetDefaultNamespace();

                IEnumerable<XElement> elements = xFile.mXDoc.Descendants(ns + "FITSKeyword");

                // Find each keyword and add it to xFile
                foreach (XElement element in elements)
                {
                    xFile.KeywordData.AddXMLKeyword(element);
                }

                xFile.SetRequiredKeywords();
            }
            */

            // Sort Image File Lists by Capture Time
            // Careful - make sure this doesn't screw up the SubFrameKeywordLists order later when writing back SubFrameKeyword data.
            // When updating actual xisf files, the update method for SubFrameKeyword data must use the SubFrameKeyword data FileName field to make sure the correct data gets written to the currect file.
            //mFileList.Sort(XisfFile.CaptureTimeComparison);
            mFileList.Sort((a, b) => a.CaptureDateTime.CompareTo(b.CaptureDateTime)); // Faster?


            /*
                 
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

                Label_FileSelection_Statistics_Task.Text = "Browse Aborted";
                return;
            }

            Label_FileSelection_Statistics_Task.Text = "Found " + mFileList.Count().ToString() + " Images";

            // Again, if the above Keywords exist in the source XISF file, add the keyword value to mNumericWeightLists
            // Build a set of numeric lists from FileSubFrameKeywordLists with any .csv keyword actually data found in the set of .xisf files 
            // we just read and parsed. mWeightLists will be used to mathaamatically generate actual weightings (SSWEIGHT) for PixInsight once they are written with the "Update" button.
            SubFrameNumericLists.BuildNumericSubFrameDataKeywordLists(SubFrameLists);

            eSubFrameValidListsValid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
            if (eSubFrameValidListsValid == eSubFrameNumericListsValid.VALID)
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


            */

            // **********************************************************************
            // Get TargetName and and Weights to populate ComboBoxes

            // First get a list of all the target names found in the source files, then find unique names and sort.
            // Place culled list in the target name combobox
            List<string> TargetNames = new List<string>();
            List<double> WeightKeywords = new List<double>();

            foreach (XisfFile file in mFileList)
            {
                TargetNames.Add(file.KeywordData.TargetName());
            }

            if (TargetNames.Count > 0)
            {
                TargetNames = TargetNames.Distinct().ToList();
                TargetNames = TargetNames.OrderBy(q => q).ToList();

                if (TargetNames.Count > 1)
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_TagetName.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_TagetName.ForeColor = Color.Black;
                }

                foreach (string item in TargetNames)
                {
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Items.Add(item);
                }

                ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.SelectedIndex = 0;
            }
            else
            {
                ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Items.Clear();
                Label_KeywordUpdateTab_SubFrameKeywords_TagetName.ForeColor = Color.DarkViolet;
            }


            // Now find a list of any present weight keywords (not values). Find unique Keyords, sort and populate Weight combobox
            foreach (XisfFile file in mFileList)
            {
                WeightKeywords.Add(file.KeywordData.WeightKeyword());
                //WeightKeywords.Add(file.KeywordData.WBPPKeyword());
            }

            if (WeightKeywords.Count > 0)
            {
                WeightKeywords = WeightKeywords.Distinct().ToList();
                WeightKeywords = WeightKeywords.OrderBy(q => q).ToList();

                foreach (var item in WeightKeywords)
                {
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Add(item);
                }

                if (WeightKeywords.Count > 1)
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Black;
                }

                ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.SelectedIndex = 0;
            }
            else
            {
                ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();
                Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Black;
            }



            // Now make a list of all Keywords found in all files. Sort and populate combobox
            Keyword node = new Keyword();
            List<string> keywordNamelist = new List<string>();

            foreach (XisfFile file in mFileList)
            {
                foreach (var keywordName in file.KeywordData.KeywordList)
                {
                    keywordNamelist.Add(keywordName.Name);
                }
            }

            keywordNamelist.Sort();
            keywordNamelist = keywordNamelist.Distinct().ToList();

            foreach (var name in keywordNamelist)
            {
                ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Items.Add(name);
            }

            // **********************************************************************





            // **********************************************************************
            // Calculate Image paramters for UI
            foreach (XisfFile file in mFileList)
            {
                if (file.KeywordData.FileName() == string.Empty)
                    file.KeywordData.AddKeyword("FILENAME", "Original Name", Path.GetFileName(file.SourceFileName));

                ImageParameterLists.BuildImageParameterValueLists(file.KeywordData);
            }
            if (mDirectoryOps.Files.Count == mFileList.Count)
                Label_FileSelection_Statistics_Task.Text = "Read all " + mFileList.Count.ToString() + " Image Files";
            else
                Label_FileSelection_Statistics_Task.Text = "Read " + mFileList.Count.ToString() + " out of " + mDirectoryOps.Files.Count + " Image Files";
            Label_FileSelection_Statistics_SubFrameOverhead.Text = ImageParameterLists.CalculateOverhead(mFileList);
            string stepsPerDegree = ImageParameterLists.CalculateFocuserTemperatureCompensationCoefficient();
            Label_FileSelection_Statistics_TempratureCompensation.Text = "Temperature Coefficient: " + stepsPerDegree;
            // **********************************************************************

            SetUISubFrameGroupBoxState();

            // **********************************************************************
            FindCaptureSoftware();
            FindFrameType();
            FindTelescope();
            FindCamera();
            // **********************************************************************
            TabControl_Update.Enabled = true;
        }

        public void SetFileIndex(bool bTarget, bool bNight, bool bFilter, bool bTime, List<XisfFile> fileList)
        {
            if (bNight)
            {
                List<string> nightlist = new List<string>();

                foreach (XisfFile file in fileList)
                {
                    string fileName = Directory.GetParent(file.SourceFileName).ToString();

                    nightlist.Add(fileName.Substring(fileName.LastIndexOf('\\') + 1));
                }

                nightlist.Distinct().ToList();


                foreach (string night in nightlist)
                {
                    // Take a look at the Unique test. Files should already be unique?
                    int index = 0;
                    int lumaIndex = 0;
                    int redIndex = 0;
                    int greenIndex = 0;
                    int blueIndex = 0;
                    int haIndex = 0;
                    int o3Index = 0;
                    int s2Index = 0;
                    int shutterIndex = 0;

                    foreach (XisfFile file in fileList)
                    {
                        if (!file.SourceFileName.Contains(night))
                            continue;

                        if (bFilter)
                        {
                            int fileIndex = file.Index;

                            if (file.Filter.Equals("Luma"))
                                file.Index = (file.Unique) ? ++lumaIndex : lumaIndex++;

                            if (file.Filter.Equals("Red"))
                                file.Index = (file.Unique) ? ++redIndex : redIndex++;

                            if (file.Filter.Equals("Green"))
                                file.Index = (file.Unique) ? ++greenIndex : greenIndex++;

                            if (file.Filter.Equals("Blue"))
                                file.Index = (file.Unique) ? ++blueIndex : blueIndex++;

                            if (file.Filter.Equals("Ha"))
                                file.Index = (file.Unique) ? ++haIndex : haIndex++;

                            if (file.Filter.Equals("O3"))
                                file.Index = (file.Unique) ? ++o3Index : o3Index++;

                            if (file.Filter.Equals("S2"))
                                file.Index = (file.Unique) ? ++s2Index : s2Index++;

                            if (file.Filter.Equals("Shutter"))
                                file.Index = (file.Unique) ? ++shutterIndex : shutterIndex++;

                            if (fileIndex == file.Index)
                            {
                                DialogResult result = MessageBox.Show(
                                "No Filter in source file:\n" + file.SourceFileName +
                                "\n\nMainForm.cs\nSetFileIndex(bool bTarget, bool bNight, bool bFilter, bool bTime, List<XisfFile> fileList)",
                                "File Update Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                                Environment.Exit(-1);
                            }
                        }

                        if (bTime)
                        {
                            // This works because fileList is already sorted by Time
                            file.Index = (file.Unique) ? ++index : index++;
                        }
                    }
                }
            }

            if (bTarget)
            {
                // Take a look at the Unique test. Files should already be unique?
                int index = 0;
                int lumaIndex = 0;
                int redIndex = 0;
                int greenIndex = 0;
                int blueIndex = 0;
                int haIndex = 0;
                int o3Index = 0;
                int s2Index = 0;
                int shutterIndex = 0;

                foreach (XisfFile file in fileList)
                {
                    if (file.SourceFileName.Contains("Duplicates"))
                        continue;

                    if (bFilter)
                    {
                        int fileIndex = file.Index;

                        if (file.Filter.Equals("Luma"))
                            file.Index = (file.Unique) ? ++lumaIndex : lumaIndex++;

                        if (file.Filter.Equals("Red"))
                            file.Index = (file.Unique) ? ++redIndex : redIndex++;

                        if (file.Filter.Equals("Green"))
                            file.Index = (file.Unique) ? ++greenIndex : greenIndex++;

                        if (file.Filter.Equals("Blue"))
                            file.Index = (file.Unique) ? ++blueIndex : blueIndex++;

                        if (file.Filter.Equals("Ha"))
                            file.Index = (file.Unique) ? ++haIndex : haIndex++;

                        if (file.Filter.Equals("O3"))
                            file.Index = (file.Unique) ? ++o3Index : o3Index++;

                        if (file.Filter.Equals("S2"))
                            file.Index = (file.Unique) ? ++s2Index : s2Index++;

                        if (file.Filter.Equals("Shutter"))
                            file.Index = (file.Unique) ? ++shutterIndex : shutterIndex++;

                        if (fileIndex == file.Index)
                        {
                            DialogResult result = MessageBox.Show(
                                "No Filter in source file:\n" + file.SourceFileName +
                                "\n\nMainForm.cs\nSetFileIndex(bool bTarget, bool bNight, bool bFilter, bool bTime, List<XisfFile> fileList)",
                                "File Update Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            Environment.Exit(-1);
                        }
                    }

                    if (bTime)
                    {
                        // This works because fileList is already sorted by Time
                        file.Index = (file.Unique) ? ++index : index++;
                    }
                }
            }
        }

        private void Button_Rename_Click(object sender, EventArgs e)
        {
            int duplicates = 0;
            bool byTarget = RadioButton_FileSelection_Order_ByTarget.Checked;
            bool byNight = RadioButton_FileSelection_Order_ByNight.Checked;
            bool byFilter = RadioButton_FileSelection_Index_ByFilter.Checked;
            bool byTime = RadioButton_FileSelection_Index_ByTime.Checked;

            Label_FileSelection_Statistics_Task.Text = "Renaming " + mFileList.Count().ToString() + " Images";

            ProgressBar_KeywordUpdateTab_WriteProgress.Maximum = mFileList.Count();
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;

            mRenameFile.MarkDuplicates(mFileList);

            SetFileIndex(byTarget, byNight, byFilter, byTime, mFileList);

            foreach (XisfFile file in mFileList)
            {
                ProgressBar_KeywordUpdateTab_WriteProgress.Value += 1;
                Label_FileSelection_BrowseFileName.Text = Path.GetDirectoryName(file.SourceFileName) + "\n" + Path.GetFileName(file.SourceFileName);

                file.Master = CheckBox_FileSelection_DirectorySelection_Master.Checked;

                Tuple<int, string> renameTuple = mRenameFile.RenameFile(file.Index, file);

                duplicates += (renameTuple.Item1 == 0) ? 1 : 0;

                Label_KeywordUpdateTab_FileName.Text = Path.GetDirectoryName(renameTuple.Item2) + "\n" + Path.GetFileName(renameTuple.Item2);

                Application.DoEvents();
            }

            ProgressBar_KeywordUpdateTab_WriteProgress.Value = ProgressBar_KeywordUpdateTab_WriteProgress.Maximum;

            if (duplicates == 1)
                Label_FileSelection_Statistics_Task.Text = (mFileList.Count() - duplicates).ToString() + " Images Renamed\n" + duplicates.ToString() + " Duplicate";
            else
                Label_FileSelection_Statistics_Task.Text = (mFileList.Count() - duplicates).ToString() + " Images Renamed\n" + duplicates.ToString() + " Duplicates";

            mFileList.Clear();

            ProgressBar_FileSelection_ReadProgress.Value = 0;
        }

        private void Button_KeywordSubFrame_UpdateXisfFiles_Click(object sender, EventArgs e)
        {
            bool bStatus;
            GroupBox_FileSelection.Enabled = false;
            TabControl_Update.Enabled = false;
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;
            ProgressBar_KeywordUpdateTab_WriteProgress.Maximum = mFileList.Count;

            // Only fill out the weight lists if in fact, we are actually updating them
            if (XisfFileUpdate.Operation == XisfFileUpdate.eOperation.CALCULATED_WEIGHTS)
            {
                // If the data in at least one of the read source XISF files is bad or inconsistent, do not allow that data to be updated without
                // re-reading a new PixInsight Subframe Selector generated CSV file (to either initially fill out or to correct bogus data)
                eSubFrameValidListsValid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
                if (eSubFrameValidListsValid == eSubFrameNumericListsValid.INVALD)
                {
                    var result = MessageBox.Show(
                        "SubFrame Numerical Weight List is Invalid.\n\nThere is a difference between the number of files contained in mFileList (" + mFileList.Count.ToString() + ")  " +
                        "compared to the number files in at least one Numeric Weight list.\n\nExample:\n    mWeightLists.Approved.Count = " + SubFrameNumericLists.Approved.Count() + "",
                        "\nMainForm.cs Button_Update_Click()",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    Label_FileSelection_Statistics_Task.Text = "Update Aborted";
                    GroupBox_FileSelection.Enabled = true;
                    TabControl_Update.Enabled = true;
                    return;
                }

                // If we've got good consistent file and SubFrame data, then update the numerical lists 
                if (eSubFrameValidListsValid == eSubFrameNumericListsValid.VALID)
                {
                    // Fisrt calculate a new WEIGHT Keyword (not SSWEIGHT yet) based on the current state of the UI and file/list contents
                    SubFrameNumericLists.CalculateNewSubFrameWeights(mFileList.Count);

                    // Now copy the newly calculated WEIGHTs to the SubFrame Lists (the data structure that will be used to update file contents)
                    XisfFileUpdate.UpdateCsvWeightList(SubFrameNumericLists, SubFrameLists);
                }
            }

            XisfFileUpdate.TargetName = ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text.Replace("'", "").Replace("\"", "");

            // File Protection
            // We get here under two conditions: 1. Protect is not checked or 2. We are updating only unprotected files
            int count = 0;
            foreach (XisfFile file in mFileList)
            {
                // Don't update existing files that have the Protected Keyword set unless the UI overrides this setting
                if (CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked)
                {
                    //if (file.KeywordData.Protected() == true)
                    // An unprotected file wil1: 1. Not have a Proteted Keyword; 2. The Keyword is false  
                    continue;
                }

                file.KeywordData.SetObservationSite();

                if (CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Checked)
                    file.KeywordData.AddKeyword("OBJECT", ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text.Replace("'", "").Replace("\"", ""), "Imaging Target");

                file.Master = CheckBox_FileSelection_DirectorySelection_Master.Checked;

                if (file.Master)
                    file.KeywordData.AddKeyword("OBJECT", "Master", "Master Integration Frame");


                // For each file: Add PROTECTED Keyword if CheckBox is checked or remove all PROTECTED Keyword
                if (CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked)
                    file.KeywordData.AddKeyword("PROTECTED", true);

                ProgressBar_KeywordUpdateTab_WriteProgress.Value += 1;
                bStatus = XisfFileUpdate.UpdateFile(file, SubFrameLists, CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked);
                Label_KeywordUpdateTab_FileName.Text = Label_KeywordUpdateTab_FileName.Text = Path.GetDirectoryName(file.SourceFileName) + "\n" + Path.GetFileName(file.SourceFileName);
                Application.DoEvents();

                if (bStatus == false)
                {
                    Label_FileSelection_Statistics_Task.Text = "File Write Error";

                    var result = MessageBox.Show(
                        "File Update Failed - Protected or I/O Error.\n\n" + Label_KeywordUpdateTab_FileName.Text,
                        "\nMainForm.cs Button_Update_Click()",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    GroupBox_FileSelection.Enabled = true;
                    TabControl_Update.Enabled = true;
                    return;
                }

                count++;
            }

            Label_FileSelection_Statistics_Task.Text = count.ToString() + " Images Updated";
            GroupBox_FileSelection.Enabled = true;
            TabControl_Update.Enabled = true;
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
                case eSubFrameNumericListsValid.EMPTY:
                    MessageBox.Show("Numeric weight lists contain zero items.\n\n", mFileCsv.FileName);
                    return;

                case eSubFrameNumericListsValid.MISMATCH:
                    MessageBox.Show("Numeric weight list file names do not match read file names.\n\n" + mFileCsv.FileName + "\n\nRerun PixInsight SubFrame Selector.", "CSV File Error");
                    return;

                case eSubFrameNumericListsValid.INVALD:
                    MessageBox.Show("Numeric weight lists do not each contain " + mFileList.Count.ToString() + " items.\n\n", mFileCsv.FileName);
                    return;
            }

            if (eSubFrameValidListsValid == eSubFrameNumericListsValid.VALID)
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

            XisfFileUpdate.Operation = XisfFileUpdate.eOperation.NEW_WEIGHTS;
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

        private double ValidateRangeValue(System.Windows.Forms.TextBox textBox)
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
            if (RadioButton_FileSelection_SequenceNumbering_WeightIndex.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.WEIGHTINDEX;
            }
        }

        private void RadioButton_Index_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_FileSelection_SequenceNumbering_IndexOnly.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.INDEX;
            }
        }

        private void RadioButton_Weight_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_FileSelection_SequenceNumbering_WeightOnly.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.WEIGHT;
            }
        }

        private void RadioButton_IndexWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_FileSelection_SequenceNumbering_IndexWeight.Checked)
            {
                mRenameFile.RenameOrder = XisfFileRename.OrderType.INDEXWEIGHT;
            }
        }

        private void SetUISubFrameGroupBoxState()
        {
            if (SubFrameNumericLists.ValidatenumericLists(mFileList.Count) == eSubFrameNumericListsValid.VALID)
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

            if (eSubFrameValidListsValid != eSubFrameNumericListsValid.VALID)
            {
                GroupBox_InitialRejectionCriteria.Enabled = false;
                GroupBox_WeightCalculations.Enabled = false;
            }

            if (eSubFrameValidListsValid == eSubFrameNumericListsValid.VALID)
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


            // Dan GroupBox_WeightsAndStatistics.Enabled = !RadioButton_SubFrameKeywords_AlphabetizeKeywords.Checked;

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
                if (eSubFrameValidListsValid == eSubFrameNumericListsValid.VALID)
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
                XisfFileUpdate.Operation = XisfFileUpdate.eOperation.KEEP_WEIGHTS;
                SetUISubFrameGroupBoxState();
            }
        }

        private void RadioButton_SetImageStatistics_RescaleWeights_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_SetImageStatistics_RescaleWeights.Checked)
            {
                XisfFileUpdate.Operation = XisfFileUpdate.eOperation.RESCALE_WEIGHTS;
                SetUISubFrameGroupBoxState();
            }
        }

        private void RadioButton_SetImageStatistics_CalculateWeights_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_SetImageStatistics_CalculateWeights.Checked)
            {
                XisfFileUpdate.Operation = XisfFileUpdate.eOperation.CALCULATED_WEIGHTS;
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
            eSubFrameNumericListsValid valid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
            if (valid != eSubFrameNumericListsValid.VALID)
            {
                return;
            }

            int index = 0;
            foreach (XisfFile file in mFileList)
            {
                // Add keyword will remove all instances of the keyword to be added and then add it
                SubFrameLists.SubFrameList.ApprovedList[index].Value = SubFrameNumericLists.Approved[index].ToString();

                index++;
            }
        }

        private void UpdateWeightCalculations()
        {
            eSubFrameNumericListsValid valid = SubFrameNumericLists.ValidatenumericLists(mFileList.Count);
            if (valid != eSubFrameNumericListsValid.VALID)
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
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked = false;
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Checked = false;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked = false;
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked = false;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked = false;

            Button_KeywordUpdateTab_CaptureSoftware_SetAll.ForeColor = Color.Black;
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.ForeColor = Color.Black;

            // Now check each and every source file for different or the same capture software
            // If identical, do nothing. If different, make all found UI software labels red 
            bool foundTSX = false;
            bool foundSGP = false;
            bool foundNINA = false;
            bool foundVOY = false;
            bool foundSCP = false;

            int count = 0;
            foreach (XisfFile file in mFileList)
            {
                string program;

                program = file.CaptureSoftware;

                if (program.Contains("TSX"))
                {
                    foundTSX = true;
                    count++;
                }

                if (program.Contains("NINA"))
                {
                    foundNINA = true;
                    count++;
                }

                if (program.Contains("SGP"))
                {
                    foundSGP = true;
                    count++;
                }

                if (program.Contains("VOY"))
                {
                    foundVOY = true;
                    count++;
                }

                if (program.Contains("SCP"))
                {
                    foundSCP = true;
                    count++;
                }
            }

            if (foundTSX)
            {
                if (foundNINA | foundSGP | foundVOY | foundSCP)
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked = true;
                }
            }

            if (foundNINA)
            {
                if (foundTSX | foundSGP | foundVOY | foundSCP)
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Checked = true;
                }
            }

            if (foundSGP)
            {
                if (foundTSX | foundNINA | foundVOY | foundSCP)
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked = true;
                }
            }

            if (foundVOY)
            {
                if (foundTSX | foundNINA | foundSGP | foundSCP)
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked = true;
                }
            }

            if (foundSCP)
            {
                if (foundTSX | foundNINA | foundSGP | foundVOY)
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked = true;
                }
            }

            if (!foundTSX && !foundNINA && !foundSGP && !foundVOY && !foundSCP)
            {
                RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.ForeColor = Color.DarkViolet;
            }

            if (foundTSX ^ foundNINA ^ foundSGP ^ foundVOY ^ foundSCP)
            {
                // Set "SetAll" to black if only a single software program was found
                Button_KeywordUpdateTab_CaptureSoftware_SetAll.ForeColor = Color.Black;
            }
            else
            {
                // More that one software program - set "SetByFile" to red
                Button_KeywordUpdateTab_CaptureSoftware_SetAll.ForeColor = Color.Red;
            }

            if (count != mFileList.Count)
            {
                // The number of source files didn't equal the number of files with a known software program
                // Set "SetByFile" to red
                Button_KeywordUpdateTab_CaptureSoftware_SetByFile.ForeColor = Color.Red;
            }
        }

        private void Button_CaptureSoftware_SetAll_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "TSX");
                }

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "NINA");
                }

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "SGP");
                }

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "VOY");
                }

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "SCP");
                }

                file.SetRequiredKeywords();
            }

            if (count == 0)
            {
                return;
            }


            FindCaptureSoftware();
        }

        private void Button_CaptureSoftware_SetByFile_Click(object sender, EventArgs e)
        {
            bool global = false;
            string captureSoftware = string.Empty;

            foreach (XisfFile file in mFileList)
            {
                if (global)
                {
                    if (file.KeywordData.CaptureSoftware() == string.Empty)
                        file.KeywordData.AddKeyword("CREATOR", captureSoftware, "XISF File Manager");
                }
                else
                {
                    captureSoftware = file.KeywordData.CaptureSoftware(true);
                    if (captureSoftware.Contains("Global_"))
                    {
                        global = true;
                        captureSoftware = captureSoftware.Replace("Global_", "");
                    }
                }

                file.CaptureSoftware = captureSoftware;

            }

            FindCaptureSoftware();
        }


        private void RadioButton_KeywordTelescope_APM107_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
            {
                TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "531";
            }
            else
            {
                TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "700";
            }
        }

        private void RadioButton_KeywordTelescope_EVO150_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
            {
                TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "750";
            }
            else
            {
                TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "1000";
            }
        }

        private void RadioButton_KeywordTelescope_NWT254_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
            {
                TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "825";
            }
            else
            {
                TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "1100";
            }
        }

        private void CheckBox_KeywordTelescope_Riccardi_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_KeywordUpdateTab_Telescope_APM107.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                    TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "531";
                else
                    TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "700";
            }

            if (RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                    TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "750";
                else
                    TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "1000";
            }

            if (RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                    TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "825";
                else
                    TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "1100";
            }
        }

        private void FindTelescope()
        {
            string telescope;
            double focalLength;
            int telescopeCount = 0;
            int riccardiCount = 0;
            int focalCount = 0;
            bool foundAPM = false;
            bool foundEVO = false;
            bool foundNWT = false;
            bool foundRiccardi = false;
            bool multipleFocalLengths = false;
            bool foundFocalLength = false;

            RadioButton_KeywordUpdateTab_Telescope_APM107.Checked = false;
            RadioButton_KeywordUpdateTab_Telescope_APM107.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Checked = false;
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Checked = false;
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.ForeColor = Color.Black;

            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked = false;
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.ForeColor = Color.Black;

            TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = string.Empty;
            Label_KeywordUpdateTab_Telescope_FocalLength.ForeColor = Color.Black;

            Button_KeywordUpdateTab_Telescope_SetAll.ForeColor = Color.Black;
            Button_KeywordUpdateTab_Telescope_SetByFile.ForeColor = Color.Black;

            if (mFileList.Count == 0)
                return;

            focalLength = mFileList[0].KeywordData.FocalLength();

            foreach (XisfFile file in mFileList)
            {
                telescope = file.KeywordData.Telescope();
                if (telescope == string.Empty)
                    continue;

                if (telescope.EndsWith("R"))
                {
                    riccardiCount++;
                    foundRiccardi = true;
                }

                if (telescope.Contains("APM"))
                {
                    telescopeCount++;
                    foundAPM = true;
                }

                if (telescope.Contains("EVO"))
                {
                    telescopeCount++;
                    foundEVO = true;
                }

                if (telescope.Contains("NWT"))
                {
                    telescopeCount++;
                    foundNWT = true;
                }

                if (focalLength != file.KeywordData.FocalLength())
                {
                    multipleFocalLengths = true;
                }

                focalLength = file.KeywordData.FocalLength();
                if (focalLength != -1)
                {
                    focalCount++;
                    foundFocalLength = true;
                }
            }

            if ((riccardiCount != mFileList.Count) && (riccardiCount != 0))
            {
                CheckBox_KeywordUpdateTab_Telescope_Riccardi.ForeColor = Color.Red;
            }
            else
            {
                CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked = true;
            }

            if ((focalCount != mFileList.Count) || !foundFocalLength || multipleFocalLengths)
            {
                Label_KeywordUpdateTab_Telescope_FocalLength.ForeColor = Color.Red;
            }


            if (foundAPM)
            {
                if (foundEVO || foundNWT)
                {
                    RadioButton_KeywordUpdateTab_Telescope_APM107.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_Telescope_APM107.Checked = true;

                    if (foundRiccardi)
                        TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "531";
                    else
                        TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "700";
                }
            }

            if (foundEVO)
            {
                if (foundAPM || foundNWT)
                {
                    RadioButton_KeywordUpdateTab_Telescope_EvoStar150.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Checked = true;

                    if (foundRiccardi)
                        TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "750";
                    else
                        TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "1000";
                }
            }

            if (foundNWT)
            {
                if (foundAPM || foundEVO)
                {
                    RadioButton_KeywordUpdateTab_Telescope_Newtonian254.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Checked = true;

                    if (foundRiccardi)
                        TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "825";
                    else
                        TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "1100";
                }
            }

            if (!foundAPM && !foundEVO & !foundNWT)
            {
                RadioButton_KeywordUpdateTab_Telescope_APM107.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_Telescope_EvoStar150.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_Telescope_Newtonian254.ForeColor = Color.DarkViolet;
                Label_KeywordUpdateTab_Telescope_FocalLength.ForeColor = Color.DarkViolet;
                CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked = false;
                CheckBox_KeywordUpdateTab_Telescope_Riccardi.ForeColor = Color.DarkViolet;
                Button_KeywordUpdateTab_Telescope_SetAll.ForeColor = Color.Red;
                Button_KeywordUpdateTab_Telescope_SetByFile.ForeColor = Color.Red;
                return;
            }

            // Set SetAll button to black if only a single telescope has been found or a signle focal lenght has been found
            if ((foundAPM ^ foundEVO ^ foundNWT) && (focalCount == mFileList.Count))
            {
                // Set "SetAll" to black if only a single filter and a single frame type was found
                Button_KeywordUpdateTab_Telescope_SetAll.ForeColor = Color.Black;
            }
            else
            {
                // More that one software program - set "SetByFile" to red
                Button_KeywordUpdateTab_Telescope_SetAll.ForeColor = Color.Red;
            }

            if ((telescopeCount < mFileList.Count) || (riccardiCount < mFileList.Count) || (focalCount < mFileList.Count))
            {
                Button_KeywordUpdateTab_Telescope_SetByFile.ForeColor = Color.Red;
            }
        }

        private void SetTelescopeUI(XisfFile file)
        {
            if (RadioButton_KeywordUpdateTab_Telescope_APM107.Checked)
            {
                file.KeywordData.AddKeyword("APTDIA", 107.0, "Aperture Diameter in mm");
                file.KeywordData.AddKeyword("APTAREA", 8992.02, "Aperture area in square mm minus obstructions");

                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.KeywordData.AddKeyword("TELESCOP", "APM107R", "APM107 Super ED with Riccardi 0.75 Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 531, "APM107 Super ED with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.KeywordData.AddKeyword("TELESCOP", "APM107", "APM107 Super ED without Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 700, "APM107 Super ED without Reducer");
                }
            }

            if (RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.KeywordData.AddKeyword("TELESCOP", "EVO150R", "EvoStar 150 with Riccardi 0.75 Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 750, "EvoStar 150 with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.KeywordData.AddKeyword("TELESCOP", "EVO150", "EvoStar 150 without Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 1000, "EvoStar 150 without Reducer");
                }
            }

            if (RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.KeywordData.AddKeyword("TELESCOP", "NWT254R", "10 Inch Newtownian with Riccardi 0.75 Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 825, "10 inch Newtonian with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.KeywordData.AddKeyword("TELESCOP", "NWT254", "10 Inch Newtonian without Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 1100, "10 Inch Newtonian without Reducer");
                }
            }
        }

        private void Button_Telescope_SetAll_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                SetTelescopeUI(file);

                file.SetRequiredKeywords();
            }

            FindTelescope();
        }


        private void Button_Telescope_SetByFile_Click(object sender, EventArgs e)
        {
            bool globalTelescope = false;
            foreach (XisfFile file in mFileList)
            {
                if (globalTelescope)
                {
                    if (file.KeywordData.Telescope() == string.Empty)
                    {
                        SetTelescopeUI(file);
                    }
                }
                else
                {
                    string telescope = file.KeywordData.Telescope(true);
                    if (telescope.Contains("Global_"))
                    {
                        globalTelescope = true;
                        telescope = telescope.Replace("Global_", "");

                        if (telescope.EndsWith("R"))
                            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked = true;
                        else
                            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked = false;

                        // Checking the radio button for the found telescope with also set focal length and Riccardi checkbox
                        RadioButton_KeywordUpdateTab_Telescope_APM107.Checked = telescope.Contains("APM") ? true : false;
                        RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Checked = telescope.Contains("EVO") ? true : false;
                        RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Checked = telescope.Contains("NWT") ? true : false;

                        SetTelescopeUI(file);
                    }
                }

                file.SetRequiredKeywords();
            }

            FindTelescope();
        }


        private void Button_KeywordImageTypeFrame_SetByFile_Click(object sender, EventArgs e)
        {
            bool globalFrameType = false;
            string frameTypeText = string.Empty;

            bool globalFilter = false;
            string globalFilterText = string.Empty;

            foreach (XisfFile file in mFileList)
            {
                if (globalFrameType)
                {
                    if (file.KeywordData.FrameType() == string.Empty)
                        file.KeywordData.AddKeyword("IMAGETYP", frameTypeText, "XISF File Manager");
                }
                else
                {
                    frameTypeText = file.KeywordData.FrameType(true);
                    if (frameTypeText.Contains("Global_"))
                    {
                        globalFrameType = true;
                        frameTypeText = frameTypeText.Replace("Global_", "");

                    }
                }

                file.KeywordData.AddKeyword("IMAGETYP", frameTypeText, "XISF File Manager");
                if (frameTypeText.Equals("Dark") || frameTypeText.Equals("Bias"))
                {
                    file.KeywordData.AddKeyword("FILTER", "Shutter", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");
                }

                file.SetRequiredKeywords();
            }



            foreach (XisfFile file in mFileList)
            {
                if (globalFilter)
                {
                    if (file.KeywordData.FilterName() == string.Empty)
                        file.KeywordData.AddKeyword("FILTER", globalFilterText, "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");
                }
                else
                {
                    globalFilterText = file.KeywordData.FilterName(true);
                    if (globalFilterText.Contains("Global_"))
                    {
                        globalFilter = true;
                        globalFilterText = globalFilterText.Replace("Global_", "");
                    }
                }

                file.KeywordData.AddKeyword("FILTER", globalFilterText, "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                file.SetRequiredKeywords();
            }

            FindFrameType();
        }

        private void Button_KeywordImageTypeFrame_SetAll_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Light", "Integration Master");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Light", "Sub Frame");
                    }
                }

                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.KeywordData.RemoveKeyword("ALT-OBS");
                        file.KeywordData.RemoveKeyword("DATE-END");
                        file.KeywordData.RemoveKeyword("LAT-OBS");
                        file.KeywordData.RemoveKeyword("LONG-OBS");
                        file.KeywordData.RemoveKeyword("OBSGEO-B");
                        file.KeywordData.RemoveKeyword("OBSGEO-H");
                        file.KeywordData.RemoveKeyword("OBSGEO-L");
                        file.KeywordData.AddKeyword("IMAGETYP", "Dark", "Integration Master");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Dark", "Sub Frame");
                    }
                }

                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Flat", "Integration Master");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Flat", "Sub Frame");
                    }
                }

                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.KeywordData.RemoveKeyword("ALT-OBS");
                        file.KeywordData.RemoveKeyword("DATE-END");
                        file.KeywordData.RemoveKeyword("LAT-OBS");
                        file.KeywordData.RemoveKeyword("LONG-OBS");
                        file.KeywordData.RemoveKeyword("OBSGEO-B");
                        file.KeywordData.RemoveKeyword("OBSGEO-H");
                        file.KeywordData.RemoveKeyword("OBSGEO-L");
                        file.KeywordData.AddKeyword("IMAGETYP", "Bias", "Integration Master");
                    }
                    else
                    {
                        file.KeywordData.AddKeyword("IMAGETYP", "Bias", "Sub Frame");
                    }

                }

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Luma", "Astrodon Luma 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Red", "Astrodon Red 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filterr_Green.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Green", "Astrodon Green 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Blue", "Astrodon Blue 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Ha", "Astrodon Ha E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Checked)
                    file.KeywordData.AddKeyword("FILTER", "O3", "Astrodon O3 E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Checked)
                    file.KeywordData.AddKeyword("FILTER", "S2", "Astrodon S2 E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Shutter", "Opaque 1.25 or placeholder via Starlight Xpress USB 7 Position Wheel");

                file.SetRequiredKeywords();
            }

            FindFrameType();
        }

        public void FindFrameType()
        {
            string filter;
            int filterCount;
            int masterCount;

            bool foundLuma = false;
            bool foundRed = false;
            bool foundGreen = false;
            bool foundBlue = false;
            bool foundHa = false;
            bool foundO3 = false;
            bool foundS2 = false;
            bool foundShutter = false;
            bool foundMaster = false;

            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filterr_Green.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filterr_Green.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Checked = false;

            CheckBox_FileSelection_DirectorySelection_Master.ForeColor = Color.Black;
            CheckBox_FileSelection_DirectorySelection_Master.Checked = false;

            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.ForeColor = Color.Black;
            Button_KeywordUpdateTab_ImageType_SetAll.ForeColor = Color.Black;
            Button_KeywordUpdateTab_ImageType_SetByFile.ForeColor = Color.Black;


            if (mFileList.Count == 0)
            {
                return;
            }

            // *****************************************************************************

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

            if (filterCount != mFileList.Count)
            {
                RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Filter_Red.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Filterr_Green.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Filter_O3.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Filter_S2.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.ForeColor = Color.DarkViolet;
            }

            if (foundLuma)
            {
                if (foundRed || foundGreen || foundBlue || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Checked = true;
                }
            }

            if (foundRed)
            {
                if (foundLuma || foundGreen || foundBlue || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Red.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Checked = true;
                }
            }

            if (foundGreen)
            {
                if (foundLuma || foundRed || foundBlue || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filterr_Green.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filterr_Green.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filterr_Green.Checked = true;
                }
            }

            if (foundBlue)
            {
                if (foundLuma || foundRed || foundGreen || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Checked = true;
                }
            }

            if (foundHa)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Checked = true;
                }
            }

            if (foundO3)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundHa || foundS2 || foundShutter)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_O3.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Checked = true;
                }
            }

            if (foundS2)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundHa || foundO3 || foundShutter)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_S2.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Checked = true;
                }
            }

            if (foundShutter)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundHa || foundO3 || foundS2)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Checked = true;
                }
            }



            // Now check each and every source file for different or the same frame type
            // If identical, do nothing. If different, make all found UI labels red 
            bool foundLight = false;
            bool foundDark = false;
            bool foundFlat = false;
            bool foundBias = false;
            int frameTypeCount;

            masterCount = 0;
            frameTypeCount = 0;
            foreach (XisfFile file in mFileList)
            {
                string frameType = file.FrameType;

                if (frameType.Equals("Light"))
                {
                    foundLight = true;
                    frameTypeCount++;
                }

                if (frameType.Equals("Dark"))
                {
                    foundDark = true;
                    frameTypeCount++;
                }

                if (frameType.Equals("Flat"))
                {
                    foundFlat = true;
                    frameTypeCount++;
                }

                if (frameType.Equals("Bias"))
                {
                    foundBias = true;
                    frameTypeCount++;
                }

                if (file.KeywordData.TargetName().Equals("Master"))
                {
                    foundMaster = true;
                    masterCount++;
                }
            }

            if (foundLight)
            {
                if (foundDark || foundFlat || foundBias)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Light.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Checked = true;
                }
            }

            if (foundDark)
            {
                if (foundLight || foundFlat || foundBias)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Checked = true;
                }
            }

            if (foundFlat)
            {
                if (foundLight || foundDark || foundBias)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Checked = true;
                }
            }

            if (foundBias)
            {
                if (foundLight || foundDark || foundFlat)
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.ForeColor = Color.Red;
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Checked = false;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Checked = true;
                }
            }

            if (!foundLight && !foundDark && !foundFlat && !foundBias)
            {
                RadioButton_KeywordUpdateTab_ImageType_Frame_Light.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.ForeColor = Color.DarkViolet;
                RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.ForeColor = Color.DarkViolet;

                return;
            }

            if (foundMaster)
            {
                if ((masterCount != mFileList.Count) && (masterCount > 0))
                {
                    CheckBox_FileSelection_DirectorySelection_Master.ForeColor = Color.Red;
                    Button_KeywordUpdateTab_ImageType_Frame_SetMaster.ForeColor = Color.Red;
                }
                else
                {
                    CheckBox_FileSelection_DirectorySelection_Master.Checked = true;
                }
            }


            // *****************************************************************************


            if ((foundLight || foundDark || foundFlat || foundBias) & (foundLuma || foundRed || foundGreen || foundBlue || foundHa || foundO3 || foundS2 || foundShutter))
            {
                // Set "SetAll" to black if only a single filter and a single frame type was found
                Button_KeywordUpdateTab_ImageType_SetAll.ForeColor = Color.Black;
            }
            else
            {
                // More that one software program - set "SetByFile" to red
                Button_KeywordUpdateTab_ImageType_SetAll.ForeColor = Color.Red;
            }

            if ((masterCount != mFileList.Count) && (masterCount != 0))
            {
                CheckBox_FileSelection_DirectorySelection_Master.ForeColor = Color.Red;
                Button_KeywordUpdateTab_ImageType_SetByFile.ForeColor = Color.Red;
            }

            if ((filterCount != mFileList.Count) || (frameTypeCount != mFileList.Count))
            {
                // The number of source files didn't equal the number of files with a known filter
                // Set "SetByFile" to red
                Button_KeywordUpdateTab_ImageType_SetByFile.ForeColor = Color.Red;
            }
        }

        private void ClearForm()
        {
            Label_KeywordUpdateTab_Camera_Camera.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_Camera_Z533.Checked = false;
            RadioButton_KeywordUpdateTab_Camera_Z533.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_Camera_Z183.Checked = false;
            RadioButton_KeywordUpdateTab_Camera_Z183.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_Camera_Q178.Checked = false;
            RadioButton_KeywordUpdateTab_Camera_Q178.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_Camera_A144.Checked = false;
            RadioButton_KeywordUpdateTab_Camera_A144.ForeColor = Color.Black;

            Label_KeywordUpdateTab_Camera_SensorTemperature.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_Gain.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_Offset.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_Binning.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_ExposureSeconds.ForeColor = Color.Black;

            Button_KeywordUpdateTab_Camera_SetAll.ForeColor = Color.Black;
            Button_KeywordUpdateTab_Camera_SetByFile.ForeColor = Color.Black;

            // Clear Form Camera Text Boxes
            TextBox_KeywordUpdateTab_Camera_Z533Gain.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_Z533Offset.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_Z183Gain.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_Z183Offset.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_Q178Gain.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_Q178Offset.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_SensorTemperature.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_ExposureSeconds.Text = string.Empty;
            TextBox_KeywordUpdateTab_Camera_Binning.Text = string.Empty;
        }

        public void FindCamera()
        {

            ClearForm();

            // Color Key - Valid is item specific
            // All items valid and unique are colored Black
            // All items valid but not unique are colored DarkViolet
            // At least one item is missing is colored Red



            // If no files, just return
            if (mFileList.Count == 0) return;

            bool foundZ533 = mFileList.Where(i => i.Camera.Equals("Z533")).Count() > 0;
            bool foundZ183 = mFileList.Where(i => i.Camera.Equals("Z183")).Count() > 0;
            bool foundQ178 = mFileList.Where(i => i.Camera.Equals("Q178")).Count() > 0;
            bool foundA144 = mFileList.Where(i => i.Camera.Equals("A144")).Count() > 0;

            if (foundZ533)
            {
                if (foundZ183 | foundQ178 | foundA144)
                {
                    RadioButton_KeywordUpdateTab_Camera_Z533.Checked = false;
                    RadioButton_KeywordUpdateTab_Camera_Z533.ForeColor = Color.DarkViolet;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_Camera_Z533.Checked = true;
                }
            }

            if (foundZ183)
            {
                if (foundZ533 | foundQ178 | foundA144)
                {
                    RadioButton_KeywordUpdateTab_Camera_Z183.Checked = false;
                    RadioButton_KeywordUpdateTab_Camera_Z183.ForeColor = Color.DarkViolet;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_Camera_Z183.Checked = true;
                }
            }

            if (foundQ178)
            {
                if (foundZ533 | foundZ183 | foundA144)
                {
                    RadioButton_KeywordUpdateTab_Camera_Q178.Checked = false;
                    RadioButton_KeywordUpdateTab_Camera_Q178.ForeColor = Color.DarkViolet;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_Camera_Q178.Checked = true;
                }
            }

            if (foundA144)
            {
                if (foundZ533 | foundZ183 | foundQ178)
                {
                    RadioButton_KeywordUpdateTab_Camera_A144.Checked = false;
                    RadioButton_KeywordUpdateTab_Camera_A144.ForeColor = Color.DarkViolet;
                }
                else
                {
                    RadioButton_KeywordUpdateTab_Camera_A144.Checked = true;
                }
            }

            if (!foundA144 && !foundQ178 && !foundZ183 && !foundZ533)
            {
                RadioButton_KeywordUpdateTab_Camera_A144.ForeColor = Color.Red;
                RadioButton_KeywordUpdateTab_Camera_Q178.ForeColor = Color.Red;
                RadioButton_KeywordUpdateTab_Camera_Z183.ForeColor = Color.Red;
                RadioButton_KeywordUpdateTab_Camera_Z533.ForeColor = Color.Red;
                Label_KeywordUpdateTab_Camera_Gain.ForeColor = Color.Red;
                Label_KeywordUpdateTab_Camera_Offset.ForeColor = Color.Red;
                Label_KeywordUpdateTab_Camera_SensorTemperature.ForeColor = Color.Red;
                Label_KeywordUpdateTab_Camera_ExposureSeconds.ForeColor = Color.Red;
                Label_KeywordUpdateTab_Camera_Binning.ForeColor = Color.Red;

                Button_KeywordUpdateTab_Camera_SetAll.ForeColor = Color.Red;
                Button_KeywordUpdateTab_Camera_SetByFile.ForeColor = Color.Red;
            }

            // ****************************************************************

            bool missingGain = mFileList.Exists(i => i.Gain == -1);
            bool uniqueGain = mFileList.Select(i => i.Gain).Distinct().Count() == 1;

            if (missingGain)
                Label_KeywordUpdateTab_Camera_Gain.ForeColor = Color.Red;
            else if (!uniqueGain)
                Label_KeywordUpdateTab_Camera_Gain.ForeColor = Color.DarkViolet;

            if (!missingGain && uniqueGain)
            {
                // All valid and unique so just pick the first one to display
                if (foundZ533)
                    TextBox_KeywordUpdateTab_Camera_Z533Gain.Text = mFileList[0].Gain.ToString();
                if (foundZ183)
                    TextBox_KeywordUpdateTab_Camera_Z183Gain.Text = mFileList[0].Gain.ToString();
                if (foundQ178)
                    TextBox_KeywordUpdateTab_Camera_Q178Gain.Text = mFileList[0].Gain.ToString();
            }

            // ****************************************************************

            bool missingOffset = mFileList.Exists(i => i.Offset == -1);
            bool uniqueOffset = mFileList.Select(i => i.Offset).Distinct().Count() == 1;

            if (missingOffset)
                Label_KeywordUpdateTab_Camera_Offset.ForeColor = Color.Red;
            else if (!uniqueOffset)
                Label_KeywordUpdateTab_Camera_Offset.ForeColor = Color.DarkViolet;

            if (!missingOffset && uniqueOffset)
            {
                // All valid and unique so just pick the first one to display
                if (foundZ533)
                    TextBox_KeywordUpdateTab_Camera_Z533Offset.Text = mFileList[0].Offset.ToString();
                if (foundZ183)
                    TextBox_KeywordUpdateTab_Camera_Z183Offset.Text = mFileList[0].Offset.ToString();
                if (foundQ178)
                    TextBox_KeywordUpdateTab_Camera_Q178Offset.Text = mFileList[0].Offset.ToString();
            }

            // ****************************************************************

            bool missingTemperature = mFileList.Exists(i => i.SensorTemperature == -273.0);
            bool uniqueTemperature = mFileList.Select(i => i.SensorTemperature).Distinct().Count() == 1;

            if (missingTemperature)
                Label_KeywordUpdateTab_Camera_SensorTemperature.ForeColor = Color.Red;
            else if (!uniqueTemperature)
                Label_KeywordUpdateTab_Camera_SensorTemperature.ForeColor = Color.DarkViolet;

            if (!missingTemperature && uniqueTemperature)
            {
                // All valid and unique so just pick the first one to display
                TextBox_KeywordUpdateTab_Camera_SensorTemperature.Text = Convert.ToDouble(mFileList[0].SensorTemperature).ToString("F1");
            }

            // ***************************************************************

            bool missingBinning = mFileList.Exists(i => i.Binning == -1);
            bool uniqueBinning = mFileList.Select(i => i.Binning).Distinct().Count() == 1;

            if (missingBinning)
                Label_KeywordUpdateTab_Camera_Binning.ForeColor = Color.Red;
            else if (!uniqueBinning)
                Label_KeywordUpdateTab_Camera_Binning.ForeColor = Color.DarkViolet;

            if (!missingBinning && uniqueBinning)
            {
                TextBox_KeywordUpdateTab_Camera_Binning.Text = mFileList[0].Binning.ToString();
            }

            // ****************************************************************

            bool missingExposure = mFileList.Exists(i => i.Exposure == double.MinValue);
            bool uniqueExposure = mFileList.Select(i => i.Exposure).Distinct().Count() == 1;

            if (missingExposure)
                Label_KeywordUpdateTab_Camera_ExposureSeconds.ForeColor = Color.Red;
            else if (!uniqueExposure)
                Label_KeywordUpdateTab_Camera_ExposureSeconds.ForeColor = Color.DarkViolet;

            if (!missingExposure && uniqueExposure)
            {
                // All valid and unique so just pick the first one to display and remove leading zeros from integers
                TextBox_KeywordUpdateTab_Camera_ExposureSeconds.Text = Regex.Replace(mFileList[0].Exposure.FormatExposureTime(), @"\b0+(\d+)", match => match.Groups[1].Value);
            }

            // ****************************************************************
            // ****************************************************************

            // Now set button colors
            if (missingGain || missingOffset || missingTemperature || missingExposure || missingBinning)
            {
                Button_KeywordUpdateTab_Camera_SetAll.ForeColor = Color.Red;
                Button_KeywordUpdateTab_Camera_SetByFile.ForeColor = Color.Red;
            }
            else
            {
                if (!uniqueGain || !uniqueOffset || !uniqueTemperature || !uniqueExposure || !uniqueBinning)
                {
                    Button_KeywordUpdateTab_Camera_SetAll.ForeColor = Color.DarkViolet;
                    Button_KeywordUpdateTab_Camera_SetByFile.ForeColor = Color.DarkViolet;
                }
            }

            // ****************************************************************
        }

        private void Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB_Click(object sender, EventArgs e)
        {
            if (RadioButton_KeywordUpdateTab_Camera_Z533.Checked)
            {
                if (Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text == "NB Preset")
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "BB Preset";
                    TextBox_KeywordUpdateTab_Camera_Z533Gain.Text = "100";
                    TextBox_KeywordUpdateTab_Camera_Z533Offset.Text = "50";
                }
                else
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "NB Preset";
                    TextBox_KeywordUpdateTab_Camera_Z533Gain.Text = "100";
                    TextBox_KeywordUpdateTab_Camera_Z533Offset.Text = "50";
                }
            }

            if (RadioButton_KeywordUpdateTab_Camera_Z183.Checked)
            {
                if (Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text == "NB Preset")
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "BB Preset";
                    TextBox_KeywordUpdateTab_Camera_Z183Gain.Text = "53";
                    TextBox_KeywordUpdateTab_Camera_Z183Offset.Text = "10";
                }
                else
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "NB Preset";
                    TextBox_KeywordUpdateTab_Camera_Z183Gain.Text = "111";
                    TextBox_KeywordUpdateTab_Camera_Z183Offset.Text = "10";
                }
            }

            if (RadioButton_KeywordUpdateTab_Camera_Q178.Checked)
            {
                if (Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text == "NB Preset")
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "NB Preset";
                    TextBox_KeywordUpdateTab_Camera_Q178Gain.Text = "40";
                    TextBox_KeywordUpdateTab_Camera_Q178Offset.Text = "15";
                }
                else
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "BB Preset";
                    TextBox_KeywordUpdateTab_Camera_Q178Gain.Text = "40";
                    TextBox_KeywordUpdateTab_Camera_Q178Offset.Text = "15";
                }
            }
        }

        private void Button_KeywordCamera_SetAll_Click(object sender, EventArgs e)
        {
            double value;
            int parseInt;

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

                bool status = double.TryParse(TextBox_KeywordUpdateTab_Camera_SensorTemperature.Text, out value);
                if (status)
                {
                    file.KeywordData.AddKeyword("CCD-TEMP", value, "Actual Sensor Temperature");
                }

                file.KeywordData.AddKeyword("NAXIS", 2, "XISF File Manager");

                status = double.TryParse(TextBox_KeywordUpdateTab_Camera_ExposureSeconds.Text, out value);
                if (status)
                {
                    file.KeywordData.AddKeyword("EXPTIME", value, "Exposure Time in Seconds");
                    file.Exposure = value;
                }

                if (RadioButton_KeywordUpdateTab_Camera_Z533.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    file.KeywordData.AddKeyword("NAXIS1", 3008, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3008, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 3.76, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 3.76, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");
                    file.KeywordData.AddKeyword("COLORSPC", "Color", "Color Image");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z533Gain.Text, out parseInt);
                    if (status)
                        file.KeywordData.AddKeyword("GAIN", parseInt, "Camera Gain");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z533Offset.Text, out parseInt);
                    if (status)
                        file.KeywordData.AddKeyword("OFFSET", parseInt, "Camera Offset");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Binning.Text, out parseInt);
                    if (status)
                    {
                        file.KeywordData.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.KeywordData.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordUpdateTab_Camera_Z183.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    file.KeywordData.AddKeyword("NAXIS1", 5496, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3672, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z183Gain.Text, out parseInt);
                    if (status)
                        file.KeywordData.AddKeyword("GAIN", parseInt, "Camera Gain");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z183Offset.Text, out parseInt);
                    if (status)
                        file.KeywordData.AddKeyword("OFFSET", parseInt, "Camera Offset");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Binning.Text, out parseInt);
                    if (status)
                    {
                        file.KeywordData.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.KeywordData.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }

                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordUpdateTab_Camera_Q178.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    file.KeywordData.AddKeyword("NAXIS1", 3072, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 2048, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Q178Gain.Text, out parseInt);
                    if (status)
                        file.KeywordData.AddKeyword("GAIN", parseInt, "Camera Gain");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Q178Offset.Text, out parseInt);
                    if (status)
                        file.KeywordData.AddKeyword("OFFSET", parseInt, "Camera Offset");
                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Binning.Text, out parseInt);
                    if (status)
                    {
                        file.KeywordData.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.KeywordData.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordUpdateTab_Camera_A144.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "A144", "Atik Infinity Camera (2018)");
                    file.KeywordData.AddKeyword("NAXIS1", 1392, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 1040, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 6.45, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 6.45, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");
                    file.KeywordData.AddKeyword("COLORSPC", "Color", "Color Image");
                    file.KeywordData.AddKeyword("GAIN", 0.37);
                    file.KeywordData.RemoveKeyword("OFFSET");
                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Binning.Text, out parseInt);
                    if (status)
                    {
                        file.KeywordData.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.KeywordData.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }

                    file.KeywordData.SetEGain();
                }

                file.SetRequiredKeywords();
            }

            FindCamera();
        }


        private void Button_KeywordCamera_SetByFile_Click(object sender, EventArgs e)
        {
            bool status;


            bool globalTemperature = false;
            string globalTemperatureText = string.Empty;
            bool globalSeconds = false;
            string globalSecondsText = string.Empty;
            bool globalGain = false;
            int globalGainValue = -1;

            bool globalOffset = false;
            int globalOffsetValue = -1;


            if (mFileList.Count == 0)
            {
                return;
            }

            foreach (XisfFile file in mFileList)
            {
                file.KeywordData.RemoveKeyword("NAXIS3");
                file.KeywordData.RemoveKeyword("EXPOSURE");

                file.KeywordData.AddKeyword("BITPIX", 16, "Bits Per Pixel");
                file.KeywordData.AddKeyword("BSCALE", 1, "Multiply Raw Values by BSCALE");
                file.KeywordData.AddKeyword("BZERO", 32768, "Add value to scale to 65536 (16 bit) values");
                string temperatureTextUI = TextBox_KeywordUpdateTab_Camera_SensorTemperature.Text;

                string temperatureText;
                if (globalTemperature)
                {
                    temperatureText = file.SensorTemperature.ToString();
                    if (temperatureText == string.Empty)
                    {
                        temperatureText = globalTemperatureText;
                    }
                }
                else
                {
                    if (temperatureTextUI == string.Empty)
                    {
                        globalTemperatureText = file.SensorTemperature.ToString();
                        if (globalTemperatureText.Contains("Global_"))
                        {
                            globalTemperature = true;
                            globalTemperatureText = globalTemperatureText.Replace("Global_", "");
                        }

                        temperatureText = globalTemperatureText;
                    }
                    else
                    {
                        temperatureText = file.SensorTemperature.ToString();
                        if (temperatureText == string.Empty)
                        {
                            temperatureText = temperatureTextUI;
                        }
                    }
                }

                double temperature;
                status = double.TryParse(temperatureText, out temperature);
                file.KeywordData.AddKeyword("CCD-TEMP", temperature, "Actual Sensor Temperature");

                file.KeywordData.AddKeyword("NAXIS", 2, "XISF File Manager");
                file.KeywordData.AddKeyword("XBINNING", Int32.Parse(TextBox_KeywordUpdateTab_Camera_Binning.Text), "Horizontal Binning");
                file.KeywordData.AddKeyword("YBINNING", Int32.Parse(TextBox_KeywordUpdateTab_Camera_Binning.Text), "Vertical Binning");
                string secondsTextUI = TextBox_KeywordUpdateTab_Camera_ExposureSeconds.Text;

                string secondsText;
                if (globalSeconds)
                {
                    secondsText = file.KeywordData.ExposureTime().FormatExposureTime();
                    if (secondsText == string.Empty)
                    {
                        secondsText = globalSecondsText;
                    }
                }
                else
                {
                    if (secondsTextUI == string.Empty)
                    {
                        globalSecondsText = file.KeywordData.ExposureTime(true).FormatExposureTime();
                        if (globalSecondsText.Contains("Global_"))
                        {
                            globalSeconds = true;
                            globalSecondsText = globalSecondsText.Replace("Global_", "");
                        }

                        secondsText = globalSecondsText;
                    }
                    else
                    {
                        secondsText = file.KeywordData.ExposureTime().FormatExposureTime();
                        if (secondsText == string.Empty)
                        {
                            secondsText = secondsTextUI;
                        }
                    }
                }

                double seconds;
                status = double.TryParse(secondsText, out seconds);
                file.KeywordData.AddKeyword("EXPTIME", seconds, "Exposure Time in Seconds");




                int gainValue;
                int gainValueUI;
                int offsetValue;
                int offsetValueUI;
                if (RadioButton_KeywordUpdateTab_Camera_Z533.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    file.KeywordData.AddKeyword("NAXIS1", 3008, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3008, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 3.76, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 3.76, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z533Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = file.KeywordData.Gain();
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = file.KeywordData.Gain(true);
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = file.KeywordData.Gain();
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    file.KeywordData.AddKeyword("GAIN", gainValue, "Camera Gain");
                    file.KeywordData.SetEGain();


                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z533Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = file.KeywordData.Offset();
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = file.KeywordData.Offset(true);
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = file.KeywordData.Offset();
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    file.KeywordData.AddKeyword("OFFSET", (int)offsetValue, "Camera Offset");
                }

                if (RadioButton_KeywordUpdateTab_Camera_Z183.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    file.KeywordData.AddKeyword("NAXIS1", 5496, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3672, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z183Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = file.KeywordData.Gain();
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = file.KeywordData.Gain(true);
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = file.KeywordData.Gain();
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    file.KeywordData.AddKeyword("GAIN", gainValue, "Camera Gain");
                    file.KeywordData.SetEGain();


                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Z183Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = file.KeywordData.Offset();
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = file.KeywordData.Offset(true);
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = file.KeywordData.Offset();
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    file.KeywordData.AddKeyword("OFFSET", offsetValue, "Camera Offset");
                }




                if (RadioButton_KeywordUpdateTab_Camera_Q178.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    file.KeywordData.AddKeyword("NAXIS1", 3072, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 2048, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Q178Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = file.KeywordData.Gain();
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = file.KeywordData.Gain(true);
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = file.KeywordData.Gain();
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    file.KeywordData.AddKeyword("GAIN", gainValue, "Camera Gain");
                    file.KeywordData.SetEGain();


                    status = int.TryParse(TextBox_KeywordUpdateTab_Camera_Q178Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = file.KeywordData.Offset();
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = file.KeywordData.Offset(true);
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = file.KeywordData.Offset();
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    file.KeywordData.AddKeyword("OFFSET", (int)offsetValue, "Camera Offset");
                }

                if (RadioButton_KeywordUpdateTab_Camera_A144.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "A144", "Atik Infinity Camera (2018)");
                    file.KeywordData.AddKeyword("NAXIS1", 1392, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 1040, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 6.45, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 6.45, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");
                    file.KeywordData.RemoveKeyword("GAIN");
                    file.KeywordData.RemoveKeyword("OFFSET");
                    file.KeywordData.SetEGain();
                }

                file.SetRequiredKeywords();
            }

            FindCamera();
        }


        private void Button_KeywordImageTypeFrame_SetMaster_Click(object sender, EventArgs e)
        {
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text = "Master";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Checked = true;
            CheckBox_FileSelection_DirectorySelection_Master.Checked = true;

            bool globalTotalFrames = false;
            int globalFrames = -1;

            foreach (XisfFile file in mFileList)
            {

                int frames;
                if (globalTotalFrames)
                {

                    frames = file.KeywordData.TotalFrames();
                    if (frames < 0)
                    {
                        frames = globalFrames;
                    }

                }
                else
                {
                    frames = file.KeywordData.TotalFrames(true);
                    if (frames < 0)
                    {
                        globalTotalFrames = true;
                        globalFrames = -frames;
                        frames = globalFrames;
                    }
                }

                file.KeywordData.AddKeyword("TOTALFRAMES", frames, "Number of Integrated SubFrames");
            }
        }

        private void CheckBox_Master_CheckedChanged(object sender, EventArgs e)
        {
            bool foundNumberOfImages = false;
            bool foundRejection = false;

            int numberOfImages = 0;
            string rejection = string.Empty;
            string comment = string.Empty;

            if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
            {
                foreach (XisfFile file in mFileList)
                {
                    foreach (Keyword node in file.KeywordData.KeywordList)
                    {
                        if (node.Comment.ToLower().Contains("numberofimages"))
                        {
                            numberOfImages = Convert.ToInt32(Regex.Match(node.Comment, @"\d+").Value);
                            foundNumberOfImages = true;

                            if (foundRejection)
                                break;
                        }

                        if (node.Comment.ToLower().Contains("pixelrejection"))
                        {
                            if (node.Comment.ToLower().Contains("linear"))
                            {
                                rejection = "LFC";
                                comment = "PixInsight Linear Fit Clipping";
                                foundRejection = true;

                                if (foundNumberOfImages)
                                    break;
                            }

                            if (node.Comment.ToLower().Contains("student"))
                            {
                                rejection = "ESD";
                                comment = "PixInsight Extreme Studentized Deviation Clipping";
                                foundRejection = true;

                                if (foundNumberOfImages)
                                    break;
                            }

                            if (node.Comment.ToLower().Contains("sigma"))
                            {
                                rejection = "SC";
                                comment = "PixInsight Sigma Clipping";
                                foundRejection = true;

                                if (foundNumberOfImages)
                                    break;
                            }

                            if (node.Comment.ToLower().Contains("winsor"))
                            {
                                rejection = "WSC";
                                comment = "PixInsight Winsorized Sigma Clipping";
                                foundRejection = true;

                                if (foundNumberOfImages)
                                    break;
                            }
                        }
                    }

                    if (foundNumberOfImages)
                        file.KeywordData.AddKeyword("TOTALFRAMES", numberOfImages, "Number of Integrated SubFrames");

                    if (foundRejection)
                        file.KeywordData.AddKeyword("REJECTION", rejection, comment);
                }
            }
        }

        private void Button_KeywordSubFrameWeight_Remove_Click(object sender, EventArgs e)
        {
            List<string> WeightKeywords = new List<string>();

            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();

            // Repopulate the list of any present weight keywords (not values). Find unique Keyords, sort and populate Weight combobox
            foreach (XisfFile file in mFileList)
            {
                WeightKeywords.Add(file.KeywordData.WeightKeyword().ToString());
            }

            if (WeightKeywords.Count > 0)
            {
                WeightKeywords = WeightKeywords.Distinct().ToList();
                WeightKeywords = WeightKeywords.OrderBy(q => q).ToList();

                foreach (var item in WeightKeywords)
                {
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Add(item).ToString();
                }

                if (WeightKeywords.Count > 1)
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Black;
                }
            }
            else
            {
                ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();
                Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Black;
                return;
            }

            // Remove ALL WEIGHT items
            if (RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.Checked)
            {
                foreach (string item in ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items)
                {
                    foreach (XisfFile file in mFileList)
                    {
                        file.KeywordData.RemoveKeyword(item);
                    }
                }

                Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Black;
                ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();
                ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Text = "";
                return;
            }

            // Only Remove selected item
            if (RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.Checked)
            {
                foreach (XisfFile file in mFileList)
                {
                    file.KeywordData.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Text);
                }

                WeightKeywords.Remove(ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Text);
                ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Remove(ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Text);
                ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Text = "";

                if (WeightKeywords.Count > 1)
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Black;
                }

                if (WeightKeywords.Count > 0)
                {
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.SelectedIndex = 0;
                }
                return;
            }

            return;
        }

        private void RadioButton_DirectorySelection_AllFiles_CheckedChanged(object sender, EventArgs e)
        {
            mFileType = DirectoryOps.FileType.ALL;
        }

        private void RadioButton_DirectorySelection_ExcludeMasters_CheckedChanged(object sender, EventArgs e)
        {
            mFileType = DirectoryOps.FileType.NO_MASTERS;
        }

        private void RadioButton_DirectorySelection_MastersOnly_CheckedChanged(object sender, EventArgs e)
        {
            mFileType = DirectoryOps.FileType.MASTERS;
        }

        private void CalibrationTab_FindCalibrationFrames_Click(object sender, EventArgs e)
        {
            bool bMatchedAllFiles = false;

            TextBox_CalibrationTab_Messgaes.Clear();
            mCalibration.Frame = DirectoryOps.FrameType.ALL;

            if (!bMatchedAllFiles)
                mCalibration.FindLibraryCalibrationFrames(mFileList);
        }

        private void CalibrationTab_ReMatchCalibrationFrames_Click(object sender, EventArgs e)
        {
            TextBox_CalibrationTab_Messgaes.Clear();
            mCalibration.MatchCalibrationLibraryFrames(mFileList);
        }

        private void CalibrationTab_CreateCalibrationDirectory_Click(object sender, EventArgs e)
        {
            if (CheckBox_CalibrationTab_CreateNew.Checked == true)
            {
                string targetCalibrationDirectory = mCalibration.SetTargetCalibrationFileDirectories(mFileList[0].SourceFileName);

                if (Directory.Exists(targetCalibrationDirectory))
                    Directory.Delete(targetCalibrationDirectory, true);

                Directory.CreateDirectory(targetCalibrationDirectory);
            }

            mCalibration.CreateTargetCalibrationDirectory(mFileList, SubFrameLists);
        }

        private void TextBox_CalibrationTab_ExposureTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Exposure.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Exposure.Text = "10";
                return;
            }

            mCalibration.ExposureTolerance = value;
        }

        private void TextBox_CalibrationTab_GainTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Gain.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Gain.Text = "10";
                return;
            }

            mCalibration.GainTolerance = value;
        }

        private void TextBox_CalibrationTab_OffsetTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Offset.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Offset.Text = "10";
                return;
            }

            mCalibration.OffsetTolerance = value;

        }

        private void TextBox_CalibrationTab_TemperatureTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Temperature.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Temperature.Text = "25";
                return;
            }

            mCalibration.TemperatureTolerance = value;
        }

        private void CheckBox_KeywordUpdate_SubFrameKeywords_Protect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.ForeColor = Color.Black;

            if (CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked)
            {
                RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.Enabled = true;
                RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Enabled = true;
            }
            else
            {
                RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.Enabled = false;
                RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Enabled = false;
                Label_FileSelection_Statistics_Task.Text = "Updates are enabled.";
            }
        }

        private void RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.ForeColor = Color.Black;
        }

        private void Button_SubFrameKeywords_CalibrationFiles_ClearAll_Click(object sender, EventArgs e)
        {
            if (mFileList.Count == 0) return;


            string directoryName = Path.GetDirectoryName(mFileList[0].SourceFileName);
            if (directoryName.Contains(@"Captures\"))
                directoryName = directoryName.Substring(0, directoryName.IndexOf("Captures")) + @"Captures\Calibration";
            else
                directoryName = Path.GetFullPath(Path.Combine(directoryName, @"..") + @"\Calibration");

            if (Directory.Exists(directoryName))
            {
                Directory.Delete(directoryName, true);
            }

            foreach (var file in mFileList)
            {
                file.KeywordData.RemoveKeyword("CDARK");
                file.CDARK = string.Empty;

                file.KeywordData.RemoveKeyword("CFLAT");
                file.CFLAT = string.Empty;

                file.KeywordData.RemoveKeyword("CBIAS");
                file.CBIAS = string.Empty;
            }

            mCalibration.ResetAll();
            TextBox_CalibrationTab_MatchingTolerance_Exposure.Text = mCalibration.ExposureTolerance.ToString();
            TextBox_CalibrationTab_MatchingTolerance_Gain.Text = mCalibration.GainTolerance.ToString();
            TextBox_CalibrationTab_MatchingTolerance_Offset.Text = mCalibration.OffsetTolerance.ToString();
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Text = mCalibration.TemperatureTolerance.ToString();

            Label_CalibrationTab_TotalMatchedFiles.Text = "No Macthed Calibration Frames";

        }

        private void ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName_SelectedIndexChanged(object sender, EventArgs e)
        {
            Keyword node = new Keyword();
            List<string> keywordValuelist = new List<string>();

            if ((string.IsNullOrEmpty(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text) || ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text == "Keyword"))
                return;

            foreach (XisfFile file in mFileList)
            {
                foreach (var keyword in file.KeywordData.KeywordList)
                {
                    //if (keyword.Name.Equals(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.SelectedItem))
                    // keywordValuelist.Add(GetKeywordValue());
                }
            }

            keywordValuelist.Sort();
            keywordValuelist = keywordValuelist.Distinct().ToList();

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Clear();
            foreach (var value in keywordValuelist)
            {
                ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Add(value);
            }
        }

        private void Button_KeywordUpdateTab_SubFrameKeywords_Delete_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                file.KeywordData.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text);
            }

            if (ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.SelectedIndex >= 0)
                ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Items.RemoveAt(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.SelectedIndex);
        }

        private void Button_KeywordUpdateTab_SubFrameKeywords_AddReplace_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                Keyword item = file.KeywordData.KeywordList.Find(i => i.Name == ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text);
                if (item != null)
                {
                    if (item.Value.ToString() == ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text)
                        file.KeywordData.AddKeyword(item.Name, item.Value);
                }
                //                file.KeywordData.AddKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text);
            }
        }
    }
}
