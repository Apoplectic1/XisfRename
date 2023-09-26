using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//using System.Deployment.Application;
//using LocalLib;
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
using System.Diagnostics.Eventing.Reader;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using XisfFileManager.Forms.MainForm;

namespace XisfFileManager
{
    public delegate void DataReceivedEventHandler(CalibrationTabPageValues data);

    public enum eFrameType { LIGHT, DARK, FLAT, BIAS, EMPTY }
    public enum eFilterType { LUMA, RED, GREEN, BLUE, HA, O3, S2, SHUTTER, EMPTY }

    // ##########################################################################################################################
    // ##########################################################################################################################
    public partial class MainForm : Form
    {
        private DirectoryOps.FileType mFileType = DirectoryOps.FileType.NO_MASTERS;
        private List<XisfFile> mFileList;
        private OpenFileDialog mFileCsv;
        //private OpenFolderDialog mFolder;
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

            /*
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
            */
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
            // Clear all lists - we are reading or re-reading what will become a new xisf file data set that will invalidate any existing data.         
            mFileList.Clear();
            SubFrameLists.Clear();
            SubFrameNumericLists.Clear();
            ImageParameterLists.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text = "Keyword";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text = "Value";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text = "";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Items.Clear();
            ClearCameraForm();

            ProgressBar_FileSelection_ReadProgress.Value = 0;
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;
            TabControl_Update.Enabled = false;


            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            string selectedFolder;

            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                    selectedFolder = folderBrowserDialog.SelectedPath;
                else
                    return;
            }
            /*
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
            */
            //mFolderBrowseState = mFolder.SelectedPaths;
            DirectoryInfo diDirectoryTree = new DirectoryInfo(selectedFolder);

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

            XisfFileReader fileReader = new XisfFileReader();

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
                    FilePath = file.FullName
                };

                await Task.Run(async () =>
                {
                    await fileReader.ReadXisfFile(mFile);
                });

                mFileList.Add(mFile);
            }

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
            List<string> WeightKeywords = new List<string>();

            foreach (XisfFile file in mFileList)
            {
                TargetNames.Add(file.TargetObjectName);
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
                WeightKeywords = file.WeightKeyword;
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



            // Now make a list of all Keywords found in ALL files. Sort and populate comboBox
            List<string> keywordNamelist = new List<string>();

            foreach (XisfFile xFile in mFileList)
            {
                foreach (var keywordName in xFile.mKeywordList.mKeywordList)
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
            //ClearImageParameterLists();
            foreach (XisfFile xFile in mFileList)
            {
                if (xFile.FilePath == string.Empty)
                    xFile.AddKeyword("FILENAME", "Original Name", Path.GetFileName(xFile.FilePath));

                ImageParameterLists.BuildImageParameterValueLists(xFile);
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
            FindFilterFrameType();
            FindTelescope();
            FindCamera();
            // **********************************************************************
            TabControl_Update.Enabled = true;
        }

        private string GetPanelDirectory(string directoryPath)
        {
            string[] directories = directoryPath.Split('\\');
            for (int i = directories.Length - 1; i >= 0; i--)
            {
                if (directories[i].Contains("Panel"))
                {
                    return string.Join("\\", directories.Take(i + 1));
                }
            }
            return directoryPath;
        }

        public void SetFileIndex(bool bTarget, bool bNight, bool bFilter, bool bTime, List<XisfFile> xFileList)
        {
            // Preset the index for each file in mFileList based on the bools for Target, Night (by existing subdirectory (typically yyyy-mm-dd)), Filter and Time (Date and Time)
            // Filters with different exposure times are not considered to be unique meaning a 600 second Blue filter uses the same index list as 60 second Blue filter
            // An exception to this is if the containing directory includes the word "Stars". Files in "Stars" directories have unique Filter indexes that are independent of exposure time. 
            // Ignore any found duplicates

            // Begin directory tree recursive search

            var panelsQuery = from xFile in xFileList
                              where Path.GetDirectoryName(xFile.FilePath).Contains("Panel")
                              group xFile by GetPanelDirectory(Path.GetDirectoryName(xFile.FilePath)) into panelGroup
                              select new
                              {
                                  PanelDirectory = panelGroup.Key,
                                  FilesInPanel = panelGroup.ToList()
                              };

            if (!panelsQuery.Any())
            {
                // Include all files if there are no panels.
                panelsQuery = from xFile in xFileList
                              group xFile by "" into allFilesGroup
                              select new
                              {
                                  PanelDirectory = "",
                                  FilesInPanel = allFilesGroup.ToList()
                              };
            }


            foreach (var panelGroup in panelsQuery)
            {
                // Process files within each individual panel.
                string panelDirectory = panelGroup.PanelDirectory;

                var starsQuery = from xFile in panelGroup.FilesInPanel
                                 where !Path.GetDirectoryName(xFile.FilePath).Contains("Duplicates") &&
                                        Path.GetDirectoryName(xFile.FilePath).Contains("Stars")
                                 select xFile;

                var lightsQuery = from xFile in panelGroup.FilesInPanel
                                  where !Path.GetDirectoryName(xFile.FilePath).Contains("Duplicates") &&
                                        !Path.GetDirectoryName(xFile.FilePath).Contains("Stars")
                                  select xFile;



                List<IEnumerable<XisfFile>> queryList = new List<IEnumerable<XisfFile>>();
                queryList.Add(lightsQuery);
                queryList.Add(starsQuery);

                if (bNight)
                {
                    foreach (var query in queryList)
                    {
                        // Number files using the existing subdirectory structure under TARGET/Capture starting at 1 for each night
                        List<string> nightlist = new List<string>();

                        foreach (XisfFile xFile in query)
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(xFile.FilePath);
                            nightlist.Add(fileNameWithoutExtension);
                        }

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

                            foreach (XisfFile xFile in query)
                            {
                                if (!xFile.FilePath.Contains(night))
                                    continue;

                                if (bFilter)
                                {
                                    int fileIndex = xFile.Index;

                                    if (xFile.FilterName.Equals("Luma"))
                                        xFile.Index = (xFile.Unique) ? ++lumaIndex : lumaIndex++;

                                    if (xFile.FilterName.Equals("Red"))
                                        xFile.Index = (xFile.Unique) ? ++redIndex : redIndex++;

                                    if (xFile.FilterName.Equals("Green"))
                                        xFile.Index = (xFile.Unique) ? ++greenIndex : greenIndex++;

                                    if (xFile.FilterName.Equals("Blue"))
                                        xFile.Index = (xFile.Unique) ? ++blueIndex : blueIndex++;

                                    if (xFile.FilterName.Equals("Ha"))
                                        xFile.Index = (xFile.Unique) ? ++haIndex : haIndex++;

                                    if (xFile.FilterName.Equals("O3"))
                                        xFile.Index = (xFile.Unique) ? ++o3Index : o3Index++;

                                    if (xFile.FilterName.Equals("S2"))
                                        xFile.Index = (xFile.Unique) ? ++s2Index : s2Index++;

                                    if (xFile.FilterName.Equals("Shutter"))
                                        xFile.Index = (xFile.Unique) ? ++shutterIndex : shutterIndex++;

                                    if (fileIndex == xFile.Index)
                                    {
                                        DialogResult result = MessageBox.Show(
                                        "No Filter in source file:\n" + xFile.FilePath +
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
                                    xFile.Index = (xFile.Unique) ? ++index : index++;
                                }
                            }
                        }
                    }
                }

                if (bTarget)
                {
                    foreach (var query in queryList)
                    {
                        // Take a look at the Unique test. Files should already be unique? They have to have unique names but could contain identicle Keywords? 
                        int lightsIndex = 0;
                        int lumaIndex = 0;
                        int redIndex = 0;
                        int greenIndex = 0;
                        int blueIndex = 0;
                        int haIndex = 0;
                        int o3Index = 0;
                        int s2Index = 0;
                        int shutterIndex = 0;

                        foreach (XisfFile xFile in query)
                        {
                            if (bFilter)
                            {
                                int fileIndex = xFile.Index;

                                if (xFile.FilterName.Equals("Luma"))
                                    xFile.Index = (xFile.Unique) ? ++lumaIndex : lumaIndex++;

                                if (xFile.FilterName.Equals("Red"))
                                    xFile.Index = (xFile.Unique) ? ++redIndex : redIndex++;

                                if (xFile.FilterName.Equals("Green"))
                                    xFile.Index = (xFile.Unique) ? ++greenIndex : greenIndex++;

                                if (xFile.FilterName.Equals("Blue"))
                                    xFile.Index = (xFile.Unique) ? ++blueIndex : blueIndex++;

                                if (xFile.FilterName.Equals("Ha"))
                                    xFile.Index = (xFile.Unique) ? ++haIndex : haIndex++;

                                if (xFile.FilterName.Equals("O3"))
                                    xFile.Index = (xFile.Unique) ? ++o3Index : o3Index++;

                                if (xFile.FilterName.Equals("S2"))
                                    xFile.Index = (xFile.Unique) ? ++s2Index : s2Index++;

                                if (xFile.FilterName.Equals("Shutter"))
                                    xFile.Index = (xFile.Unique) ? ++shutterIndex : shutterIndex++;

                                if (fileIndex == xFile.Index)
                                {
                                    DialogResult result = MessageBox.Show(
                                        "No Filter in source file:\n" + xFile.FilePath +
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
                                xFile.Index = (xFile.Unique) ? ++lightsIndex : lightsIndex++;
                            }
                        }
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

            // SetFileIndex will preset the index for each file in mFileList based on the bools for Target, Night (by existing subdirectory (typically yyyy-mm-dd)), Filter and Time (Date and Time)
            // Filters with different exposure times are not considered to be unique meaning a 600 second Blue filter uses the same index list as 60 second Blue filter
            // An exception to this is if the containing directory includes the word "Stars". Files in "Stars" directories have unique Filter indexes that are independent of exposure time. 
            // Any found Duplicates are handled inside the RenameFile method
            SetFileIndex(byTarget, byNight, byFilter, byTime, mFileList);

            foreach (XisfFile xFile in mFileList)
            {
                ProgressBar_KeywordUpdateTab_WriteProgress.Value += 1;
                Label_FileSelection_BrowseFileName.Text = Path.GetDirectoryName(xFile.FilePath) + "\n" + Path.GetFileName(xFile.FilePath);

                xFile.Master = CheckBox_FileSelection_DirectorySelection_Master.Checked;

                Tuple<int, string> renameTuple = mRenameFile.RenameFile(xFile);

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
                // For each file: Add PROTECT Keyword if CheckBox is checked or remove all PROTECT Keywords
                // Don't update existing files that have the Protected Keyword set unless the UI overrides this setting
                if (CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked)
                {
                    file.Protect = true;
                    continue;
                }
                else
                    file.Protect = false;

                file.SetObservationSite();

                if (CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Checked)
                    file.AddKeyword("OBJECT", ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text.Replace("'", "").Replace("\"", ""), "Imaging Target");

                file.Master = CheckBox_FileSelection_DirectorySelection_Master.Checked;

                if (file.Master)
                    file.AddKeyword("OBJECT", "Master", "Master Integration Frame");

                ProgressBar_KeywordUpdateTab_WriteProgress.Value += 1;
                bStatus = XisfFileUpdate.UpdateFile(file, SubFrameLists, CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked);
                Label_KeywordUpdateTab_FileName.Text = Label_KeywordUpdateTab_FileName.Text = Path.GetDirectoryName(file.FilePath) + "\n" + Path.GetFileName(file.FilePath);
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
            // Check each source file for different or the same capture software
            int foundTSX = 0;
            int foundSGP = 0;
            int foundNINA = 0;
            int foundVOY = 0;
            int foundSCP = 0;
            int count = 0;

            foreach (XisfFile file in mFileList)
            {
                string softwareCreator = file.CaptureSoftware;

                if (softwareCreator.Equals("NINA"))
                {
                    foundNINA++;
                    count++;
                    continue;
                }

                if (softwareCreator.Equals("SGP"))
                {
                    foundSGP++;
                    count++;
                    continue;
                }

                if (softwareCreator.Equals("TSX"))
                {
                    foundTSX++;
                    count++;
                    continue;
                }

                if (softwareCreator.Equals("VOY"))
                {
                    foundVOY++;
                    count++;
                    continue;
                }

                if (softwareCreator.Equals("SCP"))
                {
                    foundSCP++;
                    count++;
                    continue;
                }
            }

            if ((count != (foundNINA + foundSCP + foundSGP + foundTSX + foundVOY)) || (count == 0))
            {
                // Missing at least one. If we found any, make DarkViolet otherwise Red
                RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.ForeColor = (foundNINA == 0) ? Color.Red : Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.ForeColor = (foundSCP == 0) ? Color.Red : Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.ForeColor = (foundSGP == 0) ? Color.Red : Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.ForeColor = (foundTSX == 0) ? Color.Red : Color.DarkViolet;
                RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.ForeColor = (foundVOY == 0) ? Color.Red : Color.DarkViolet;

                // Missing at least on. Uncheck all
                RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked = false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Checked = false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked = false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked = false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked = false;

                // Missing at least one. Set SetAll and SetByFile Buttons to Red
                Button_KeywordUpdateTab_CaptureSoftware_SetAll.ForeColor = Color.Red;
                Button_KeywordUpdateTab_CaptureSoftware_SetByFile.ForeColor = Color.Red;
            }
            else
            {
                // All matched. Is there more that one SoftwareCreator? If so, DarkViolet otherwise Black
                RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.ForeColor = ((foundNINA == count) || (foundNINA == 0)) ? Color.Black : Color.DarkGreen;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.ForeColor = ((foundSCP == count) || (foundSCP == 0)) ? Color.Black : Color.DarkGreen;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.ForeColor = ((foundSGP == count) || (foundSGP == 0)) ? Color.Black : Color.DarkGreen;
                RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.ForeColor = ((foundTSX == count) || (foundTSX == 0)) ? Color.Black : Color.DarkGreen;
                RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.ForeColor = ((foundVOY == count) || (foundVOY == 0)) ? Color.Black : Color.DarkGreen;

                // All matched. Is there a single SoftwareCreator? If so, checked otherwise unchecked
                RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Checked = (foundNINA == count) ? true : false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked = (foundSCP == count) ? true : false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked = (foundSGP == count) ? true : false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked = (foundTSX == count) ? true : false;
                RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked = (foundVOY == count) ? true : false;

                // All matched. Set SetAll and SetByFile Buttons to Black
                Button_KeywordUpdateTab_CaptureSoftware_SetAll.ForeColor = Color.Black;
                Button_KeywordUpdateTab_CaptureSoftware_SetByFile.ForeColor = Color.Black;
            }
        }

        private void Button_CaptureSoftware_SetAll_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Checked)
                    file.AddKeyword("CREATOR", "NINA");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked)
                    file.AddKeyword("CREATOR", "TSX");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked)
                    file.AddKeyword("CREATOR", "SGP");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked)
                    file.AddKeyword("CREATOR", "VOY");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked)
                    file.AddKeyword("CREATOR", "SCP");
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
                    if (file.CaptureSoftware == string.Empty)
                        file.AddKeyword("CREATOR", captureSoftware, "XISF File Manager");
                }
                else
                {
                    captureSoftware = file.CaptureSoftware;
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

            foreach (XisfFile file in mFileList)
            {
                if (TextBox_KeywordUpdateTab_Telescope_FocalLength.Text != file.FocalLength.ToString())
                {
                    TextBox_KeywordUpdateTab_Telescope_FocalLength.Text = "";
                    Label_KeywordUpdateTab_Telescope_FocalLength.ForeColor = Color.Red;
                    break;
                }
            }

            Label_KeywordUpdateTab_Telescope_FocalLength.ForeColor = Color.Black;
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

            focalLength = mFileList[0].FocalLength;

            foreach (XisfFile file in mFileList)
            {
                telescope = file.Telescope;
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

                if (focalLength != file.FocalLength)
                {
                    multipleFocalLengths = true;
                }

                focalLength = file.FocalLength;
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
                file.AddKeyword("APTDIA", 107.0, "Aperture Diameter in mm");
                file.AddKeyword("APTAREA", 8992.02, "Aperture area in square mm minus obstructions");

                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.AddKeyword("TELESCOP", "APM107R", "APM107 Super ED with Riccardi 0.75 Reducer");
                    file.AddKeyword("FOCALLEN", 531, "APM107 Super ED with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.AddKeyword("TELESCOP", "APM107", "APM107 Super ED without Reducer");
                    file.AddKeyword("FOCALLEN", 700, "APM107 Super ED without Reducer");
                }
            }

            if (RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.AddKeyword("TELESCOP", "EVO150R", "EvoStar 150 with Riccardi 0.75 Reducer");
                    file.AddKeyword("FOCALLEN", 750, "EvoStar 150 with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.AddKeyword("TELESCOP", "EVO150", "EvoStar 150 without Reducer");
                    file.AddKeyword("FOCALLEN", 1000, "EvoStar 150 without Reducer");
                }
            }

            if (RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.AddKeyword("TELESCOP", "NWT254R", "10 Inch Newtownian with Riccardi 0.75 Reducer");
                    file.AddKeyword("FOCALLEN", 825, "10 inch Newtonian with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.AddKeyword("TELESCOP", "NWT254", "10 Inch Newtonian without Reducer");
                    file.AddKeyword("FOCALLEN", 1100, "10 Inch Newtonian without Reducer");
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
                    if (file.Telescope == string.Empty)
                    {
                        SetTelescopeUI(file);
                    }
                }
                else
                {
                    string telescope = file.Telescope;
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
                    if (file.FrameType == eFrameType.EMPTY)
                        file.AddKeyword("IMAGETYP", frameTypeText, "XISF File Manager");
                }
                else
                {
                    frameTypeText = string.Empty; // file.FrameType;
                    if (frameTypeText.Contains("Global_"))
                    {
                        globalFrameType = true;
                        frameTypeText = frameTypeText.Replace("Global_", "");

                    }
                }

                file.AddKeyword("IMAGETYP", frameTypeText, "XISF File Manager");
                if (frameTypeText.Equals("Dark") || frameTypeText.Equals("Bias"))
                {
                    file.AddKeyword("FILTER", "Shutter", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");
                }

                file.SetRequiredKeywords();
            }



            foreach (XisfFile file in mFileList)
            {
                if (globalFilter)
                {
                    if (file.FilterName == string.Empty)
                        file.AddKeyword("FILTER", globalFilterText, "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");
                }
                else
                {
                    globalFilterText = file.FilterName;
                    if (globalFilterText.Contains("Global_"))
                    {
                        globalFilter = true;
                        globalFilterText = globalFilterText.Replace("Global_", "");
                    }
                }

                file.AddKeyword("FILTER", globalFilterText, "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                file.SetRequiredKeywords();
            }

            FindFilterFrameType();
        }

        private void Button_KeywordImageTypeFrame_SetAll_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.AddKeyword("IMAGETYP", "Light", "Integration Master");
                    }
                    else
                    {
                        file.AddKeyword("IMAGETYP", "Light", "Sub Frame");
                    }
                }

                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.RemoveKeyword("ALT-OBS");
                        file.RemoveKeyword("DATE-END");
                        file.RemoveKeyword("LAT-OBS");
                        file.RemoveKeyword("LONG-OBS");
                        file.RemoveKeyword("OBSGEO-B");
                        file.RemoveKeyword("OBSGEO-H");
                        file.RemoveKeyword("OBSGEO-L");
                        file.AddKeyword("IMAGETYP", "Dark", "Integration Master");
                    }
                    else
                    {
                        file.AddKeyword("IMAGETYP", "Dark", "Sub Frame");
                    }
                }

                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.AddKeyword("IMAGETYP", "Flat", "Integration Master");
                    }
                    else
                    {
                        file.AddKeyword("IMAGETYP", "Flat", "Sub Frame");
                    }
                }

                if (RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Checked)
                {
                    if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
                    {
                        file.RemoveKeyword("ALT-OBS");
                        file.RemoveKeyword("DATE-END");
                        file.RemoveKeyword("LAT-OBS");
                        file.RemoveKeyword("LONG-OBS");
                        file.RemoveKeyword("OBSGEO-B");
                        file.RemoveKeyword("OBSGEO-H");
                        file.RemoveKeyword("OBSGEO-L");
                        file.AddKeyword("IMAGETYP", "Bias", "Integration Master");
                    }
                    else
                    {
                        file.AddKeyword("IMAGETYP", "Bias", "Sub Frame");
                    }

                }

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Checked)
                    file.AddKeyword("FILTER", "Luma", "Astrodon Luma 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Checked)
                    file.AddKeyword("FILTER", "Red", "Astrodon Red 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Checked)
                    file.AddKeyword("FILTER", "Green", "Astrodon Green 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Checked)
                    file.AddKeyword("FILTER", "Blue", "Astrodon Blue 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Checked)
                    file.AddKeyword("FILTER", "Ha", "Astrodon Ha E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Checked)
                    file.AddKeyword("FILTER", "O3", "Astrodon O3 E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Checked)
                    file.AddKeyword("FILTER", "S2", "Astrodon S2 E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Checked)
                    file.AddKeyword("FILTER", "Shutter", "Opaque 1.25 or placeholder via Starlight Xpress USB 7 Position Wheel");

                file.SetRequiredKeywords();
            }

            FindFilterFrameType();
        }

        public void FindFilterFrameType()
        {
            string filter;
            int filterCount;
            int masterCount;
            int lumaCount;
            int redCount;
            int greenCount;
            int blueCount;
            int haCount;
            int o3Count;
            int s2Count;
            int shutterCount;

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
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Checked = false;
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
            lumaCount = 0;
            redCount = 0;
            greenCount = 0;
            blueCount = 0;
            haCount = 0;
            o3Count = 0;
            s2Count = 0;
            shutterCount = 0;

            foreach (XisfFile file in mFileList)
            {
                filter = file.FilterName;

                if (filter == "Luma")
                {
                    foundLuma = true;
                    lumaCount++;
                    filterCount++;
                }

                if (filter == "Red")
                {
                    foundRed = true;
                    redCount++;
                    filterCount++;
                }

                if (filter == "Green")
                {
                    foundGreen = true;
                    greenCount++;
                    filterCount++;
                }

                if (filter == "Blue")
                {
                    foundBlue = true;
                    blueCount++;
                    filterCount++;
                }

                if (filter == "Ha")
                {
                    foundHa = true;
                    haCount++;
                    filterCount++;
                }

                if (filter == "O3")
                {
                    foundO3 = true;
                    o3Count++;
                    filterCount++;
                }

                if (filter == "S2")
                {
                    foundS2 = true;
                    s2Count++;
                    filterCount++;
                }

                if (filter == "Shutter")
                {
                    foundShutter = true;
                    shutterCount++;
                    filterCount++;
                }
            }

            if (filterCount == mFileList.Count)
            {
                // Every source file has a filter.

                // If one filter is used, check that filter's radio button and leave the radio button as black
                // if more than one filter is used, make a found filter's radio button unchecked and color DarkGreen
                // Do this for each filter

                if (foundLuma)
                {
                    if (lumaCount != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Checked = true;
                }
                if (foundRed)
                {
                    if (redCount != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Red.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Checked = true;
                }
                if (foundGreen)
                {
                    if (greenCount != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Green.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Checked = true;
                }
                if (foundBlue)
                {
                    if (blueCount != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Checked = true;
                }
                if (foundHa)
                {
                    if (haCount != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Checked = true;
                }
                if (foundO3)
                {
                    if (o3Count != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_O3.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Checked = true;
                }
                if (foundS2)
                {
                    if (s2Count != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_S2.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Checked = true;
                }
                if (foundShutter)
                {
                    if (shutterCount != filterCount)
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Checked = true;
                }
            }
            else
            {
                // Some source files are missing filters

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
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Green.ForeColor = Color.Red;
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Checked = false;
                    }
                    else
                    {
                        RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Checked = true;
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
            }


            // Now check each and every source file for a valid frame type

            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.ForeColor = Color.Black;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.ForeColor = Color.Black;

            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Checked = false;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Checked = false;

            bool foundLight = false;
            bool foundDark = false;
            bool foundFlat = false;
            bool foundBias = false;
            int lightCount = 0;
            int darkCount = 0;
            int flatCount = 0;
            int biasCount = 0;
            int frameTypeCount;

            masterCount = 0;
            frameTypeCount = 0;
            foreach (XisfFile file in mFileList)
            {
                if (file.FrameType == eFrameType.LIGHT)
                {
                    foundLight = true;
                    lightCount++;
                    frameTypeCount++;
                }

                if (file.FrameType == eFrameType.DARK)
                {
                    foundDark = true;
                    darkCount++;
                    frameTypeCount++;
                }

                if (file.FrameType == eFrameType.FLAT)
                {
                    foundFlat = true;
                    flatCount++;
                    frameTypeCount++;
                }

                if (file.FrameType == eFrameType.BIAS)
                {
                    foundBias = true;
                    biasCount++;
                    frameTypeCount++;
                }

                if (file.TargetObjectName.Equals("Master"))
                {
                    foundMaster = true;
                    masterCount++;
                }
            }



            if (frameTypeCount == mFileList.Count)
            {
                // Every source file has a frameType.

                // If one filter is used, check that filter's radio button and leave the radio button as black
                // if more than one filter is used, make a found filter's radio button unchecked and color DarkGreen
                // Do this for each filter

                if (foundLight)
                {
                    if (lightCount != frameTypeCount)
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Light.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Checked = true;
                }
                if (foundDark)
                {
                    if (darkCount != frameTypeCount)
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Checked = true;
                }
                if (foundFlat)
                {
                    if (flatCount != frameTypeCount)
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Checked = true;
                }
                if (foundBias)
                {
                    if (biasCount != frameTypeCount)
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.ForeColor = Color.DarkGreen;
                    else
                        RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Checked = true;
                }
            }
            else
            {
                /*
                if (frameTypeCount == mFileList.Count)
                {
                    // Every source file has a FrameType. Make each found FrameType radio button DarkGreen
                    if (foundLight) RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.ForeColor = Color.DarkGreen;
                }
                */
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

        private void ClearCameraForm()
        {
            Label_KeywordUpdateTab_Camera_Camera.ForeColor = Color.Black;

            CheckBox_KeywordUpdateTab_Camera_Z533.Checked = false;
            CheckBox_KeywordUpdateTab_Camera_Z533.ForeColor = Color.Black;

            CheckBox_KeywordUpdateTab_Camera_Z183.Checked = false;
            CheckBox_KeywordUpdateTab_Camera_Z183.ForeColor = Color.Black;

            CheckBox_KeywordUpdateTab_Camera_Q178.Checked = false;
            CheckBox_KeywordUpdateTab_Camera_Q178.ForeColor = Color.Black;

            CheckBox_KeywordUpdateTab_Camera_A144.Checked = false;
            CheckBox_KeywordUpdateTab_Camera_A144.ForeColor = Color.Black;

            Label_KeywordUpdateTab_Camera_SensorTemp.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_Gain.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_Offset.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_Binning.ForeColor = Color.Black;
            Label_KeywordUpdateTab_Camera_Seconds.ForeColor = Color.Black;

            Button_KeywordUpdateTab_Camera_SetAll.ForeColor = Color.Black;
            Button_KeywordUpdateTab_Camera_SetByFile.ForeColor = Color.Black;

            // Clear Form Camera Text Boxes
            ComboBox_KeywordUpdateTab_Camera_A144Binning.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_A144Binning.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_A144Seconds.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_A144Seconds.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.Text = string.Empty;

            ComboBox_KeywordUpdateTab_Camera_Q178Binning.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Q178Binning.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Q178Gain.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Q178Gain.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Q178Offset.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Q178Offset.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.Text = string.Empty;


            ComboBox_KeywordUpdateTab_Camera_Z183Binning.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z183Binning.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z183Gain.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z183Gain.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z183Offset.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z183Offset.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.Text = string.Empty;

            ComboBox_KeywordUpdateTab_Camera_Z533Binning.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z533Gain.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z533Gain.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z533Offset.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z533Offset.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Text = string.Empty;
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.DataSource = null;
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Text = string.Empty;

        }

        public void FindCamera()
        {
            ClearCameraForm();

            //Z183CustomComboBox.OutlineColor = Color.Red;

            // If no files, just return
            if (mFileList.Count == 0) return;

            // ****************************************************************

            // cameraList should contain an entry for each file
            List<string> CameraList = mFileList.Where(c => c.Camera == "A144" || c.Camera == "Q178" || c.Camera == "Z183" || c.Camera == "Z533").Select(c => c.Camera).ToList();
            bool bFoundZ183 = CameraList.Count(c => c == "Z183") != 0;
            bool bFoundZ533 = CameraList.Count(c => c == "Z533") != 0;
            bool bFoundQ178 = CameraList.Count(c => c == "Q178") != 0;
            bool bFoundA144 = CameraList.Count(c => c == "A144") != 0;

            bool bNoCameras = CameraList.Count() == 0;
            bool bMissingCameras = (CameraList.Count() != mFileList.Count()) && !bNoCameras;
            bool bDifferentCameras = ((bFoundZ183 ? 1 : 0) + (bFoundZ533 ? 1 : 0) + (bFoundQ178 ? 1 : 0) + (bFoundA144 ? 1 : 0) >= 2) && !bMissingCameras;
            bool bUniqueCamera = !bMissingCameras && !bDifferentCameras && !bNoCameras;
            //bool bUniqueCamera = !bMissingCameras && !bNoCameras;

            CheckBox_KeywordUpdateTab_Camera_A144.Checked = bFoundA144;
            CheckBox_KeywordUpdateTab_Camera_Q178.Checked = bFoundQ178;
            CheckBox_KeywordUpdateTab_Camera_Z183.Checked = bFoundZ183;
            CheckBox_KeywordUpdateTab_Camera_Z533.Checked = bFoundZ533;

            if (bNoCameras)
            {
                // All files are missing cameras
                CheckBox_KeywordUpdateTab_Camera_A144.ForeColor = Color.Red;
                CheckBox_KeywordUpdateTab_Camera_Q178.ForeColor = Color.Red;
                CheckBox_KeywordUpdateTab_Camera_Z183.ForeColor = Color.Red;
                CheckBox_KeywordUpdateTab_Camera_Z533.ForeColor = Color.Red;
            }

            if (bMissingCameras)
            {
                // Found at least one Camera but some files are missing Cameras 
                CheckBox_KeywordUpdateTab_Camera_A144.ForeColor = bFoundA144 ? Color.DarkViolet : Color.Red;
                CheckBox_KeywordUpdateTab_Camera_Q178.ForeColor = bFoundQ178 ? Color.DarkViolet : Color.Red;
                CheckBox_KeywordUpdateTab_Camera_Z183.ForeColor = bFoundZ183 ? Color.DarkViolet : Color.Red;
                CheckBox_KeywordUpdateTab_Camera_Z533.ForeColor = bFoundZ533 ? Color.DarkViolet : Color.Red;
            }

            if (bDifferentCameras)
            {
                // Found different Cameras and all files contain Cameras 
                CheckBox_KeywordUpdateTab_Camera_A144.ForeColor = CheckBox_KeywordUpdateTab_Camera_A144.Checked ? Color.Green : Color.Black;
                CheckBox_KeywordUpdateTab_Camera_Q178.ForeColor = CheckBox_KeywordUpdateTab_Camera_Q178.Checked ? Color.Green : Color.Black;
                CheckBox_KeywordUpdateTab_Camera_Z183.ForeColor = CheckBox_KeywordUpdateTab_Camera_Z183.Checked ? Color.Green : Color.Black;
                CheckBox_KeywordUpdateTab_Camera_Z533.ForeColor = CheckBox_KeywordUpdateTab_Camera_Z533.Checked ? Color.Green : Color.Black;
            }

            if (bUniqueCamera)
            {
                CheckBox_KeywordUpdateTab_Camera_A144.ForeColor = Color.Black;
                CheckBox_KeywordUpdateTab_Camera_Q178.ForeColor = Color.Black;
                CheckBox_KeywordUpdateTab_Camera_Z183.ForeColor = Color.Black;
                CheckBox_KeywordUpdateTab_Camera_Z533.ForeColor = Color.Black;
            }

            // ****************************************************************

            bool bNoSecondsZ533 = false;
            bool bMissingSecondsZ533 = false;
            bool bDifferentSecondsZ533 = false;
            bool bUniqueSecondsZ533 = false;

            if (bFoundZ533)
            {
                List<double> SecondsListZ533 = mFileList.Where(i => i.ExposureSeconds > 0 && i.Camera == "Z533").Select(i => i.ExposureSeconds).ToList();
                bNoSecondsZ533 = SecondsListZ533.Count() == 0;
                bMissingSecondsZ533 = SecondsListZ533.Count() != CameraList.Count(c => c == "Z533") && !bNoSecondsZ533;
                bDifferentSecondsZ533 = SecondsListZ533.Distinct().Count() > 1;
                bUniqueSecondsZ533 = !bMissingSecondsZ533 && !bDifferentSecondsZ533 && !bNoSecondsZ533;

                ComboBox_KeywordUpdateTab_Camera_Z533Seconds.DataSource = SecondsListZ533.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSecondsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Seconds.ForeColor = Color.Black;

                if (!bMissingSecondsZ533 && bDifferentSecondsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Seconds.ForeColor = Color.Green;

                if (bMissingSecondsZ533 && !bDifferentSecondsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Seconds.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z533Seconds.SelectedIndex = bUniqueSecondsZ533 || (bMissingSecondsZ533 && !bDifferentSecondsZ533) ? 0 : -1;
            }


            bool bNoSecondsZ183 = false;
            bool bMissingSecondsZ183 = false;
            bool bDifferentSecondsZ183 = false;
            bool bUniqueSecondsZ183 = false;

            if (bFoundZ183)
            {
                List<double> SecondsListZ183 = mFileList.Where(i => i.ExposureSeconds > 0 && i.Camera == "Z183").Select(i => i.ExposureSeconds).ToList();
                bNoSecondsZ183 = SecondsListZ183.Count() == 0;
                bMissingSecondsZ183 = SecondsListZ183.Count() != CameraList.Count(c => c == "Z183") && !bNoSecondsZ183;
                bDifferentSecondsZ183 = SecondsListZ183.Distinct().Count() > 1;
                bUniqueSecondsZ183 = !bMissingSecondsZ183 && !bDifferentSecondsZ183 && !bNoSecondsZ183;

                ComboBox_KeywordUpdateTab_Camera_Z183Seconds.DataSource = SecondsListZ183.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSecondsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Seconds.ForeColor = Color.Black;

                if (!bMissingSecondsZ183 && bDifferentSecondsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Seconds.ForeColor = Color.Green;

                if (bMissingSecondsZ183 && !bDifferentSecondsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Seconds.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z183Seconds.SelectedIndex = bUniqueSecondsZ183 || (bMissingSecondsZ183 && !bDifferentSecondsZ183) ? 0 : -1;
            }


            bool bNoSecondsQ178 = false;
            bool bMissingSecondsQ178 = false;
            bool bDifferentSecondsQ178 = false;
            bool bUniqueSecondsQ178 = false;

            if (bFoundQ178)
            {
                List<double> SecondsListQ178 = mFileList.Where(i => i.ExposureSeconds > 0 && i.Camera == "Q178").Select(i => i.ExposureSeconds).ToList();
                bNoSecondsQ178 = SecondsListQ178.Count() == 0;
                bMissingSecondsQ178 = SecondsListQ178.Count() != CameraList.Count(c => c == "Q178") && !bNoSecondsQ178;
                bDifferentSecondsQ178 = SecondsListQ178.Distinct().Count() > 1;
                bUniqueSecondsQ178 = !bMissingSecondsQ178 && !bDifferentSecondsQ178 && !bNoSecondsQ178;

                ComboBox_KeywordUpdateTab_Camera_Q178Seconds.DataSource = SecondsListQ178.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSecondsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Seconds.ForeColor = Color.Black;

                if (!bMissingSecondsQ178 && bDifferentSecondsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Seconds.ForeColor = Color.Green;

                if (bMissingSecondsQ178 && !bDifferentSecondsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Seconds.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Q178Seconds.SelectedIndex = bUniqueSecondsQ178 || (bMissingSecondsQ178 && !bDifferentSecondsQ178) ? 0 : -1;
            }

            bool bNoSecondsA144 = false;
            bool bMissingSecondsA144 = false;
            bool bDifferentSecondsA144 = false;
            bool bUniqueSecondsA144 = false;

            if (bFoundA144)
            {
                List<double> SecondsListA144 = mFileList.Where(i => i.ExposureSeconds > 0 && i.Camera == "A144").Select(i => i.ExposureSeconds).ToList();
                bNoSecondsA144 = SecondsListA144.Count() == 0;
                bMissingSecondsA144 = SecondsListA144.Count() != CameraList.Count(c => c == "A144") && !bNoSecondsA144;
                bDifferentSecondsA144 = SecondsListA144.Distinct().Count() > 1;
                bUniqueSecondsA144 = !bMissingSecondsA144 && !bDifferentSecondsA144 && !bNoSecondsA144;

                ComboBox_KeywordUpdateTab_Camera_A144Seconds.DataSource = SecondsListA144.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSecondsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144Seconds.ForeColor = Color.Black;

                if (!bMissingSecondsA144 && bDifferentSecondsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144Seconds.ForeColor = Color.Green;

                if (bMissingSecondsA144 && !bDifferentSecondsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144Seconds.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_A144Seconds.SelectedIndex = bUniqueSecondsA144 || (bMissingSecondsA144 && !bDifferentSecondsA144) ? 0 : -1;
            }

            if (bNoSecondsZ533 || bMissingSecondsZ533 || bNoSecondsZ183 || bMissingSecondsZ183 || bNoSecondsQ178 || bMissingSecondsQ178 || bNoSecondsA144 || bMissingSecondsA144)
                Label_KeywordUpdateTab_Camera_Seconds.ForeColor = Color.Red;
            else
                if (bDifferentSecondsZ533 || bDifferentSecondsZ183 || bDifferentSecondsQ178 || bDifferentSecondsA144)
                Label_KeywordUpdateTab_Camera_Seconds.ForeColor = Color.Green;

            // ****************************************************************

            bool bNoGainsZ533 = false;
            bool bMissingGainsZ533 = false;
            bool bDifferentGainsZ533 = false;
            bool bUniqueGainZ533 = false;

            if (bFoundZ533)
            {
                List<int> GainListZ533 = mFileList.Where(i => i.Gain > 0 && i.Camera == "Z533").Select(i => i.Gain).ToList();
                bNoGainsZ533 = GainListZ533.Count() == 0;
                bMissingGainsZ533 = GainListZ533.Count() != CameraList.Count(c => c == "Z533") && !bNoGainsZ533;
                bDifferentGainsZ533 = GainListZ533.Distinct().Count() > 1 && !bMissingGainsZ533;
                bUniqueGainZ533 = !bMissingGainsZ533 && !bDifferentGainsZ533 && !bNoGainsZ533;

                ComboBox_KeywordUpdateTab_Camera_Z533Gain.DataSource = GainListZ533.OrderBy(item => item).Distinct().ToList();

                if (bUniqueGainZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Gain.ForeColor = Color.Black;

                if (!bMissingGainsZ533 && bDifferentGainsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Gain.ForeColor = Color.Green;

                if (bMissingGainsZ533 && !bDifferentGainsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Gain.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z533Gain.SelectedIndex = bUniqueGainZ533 || (bMissingGainsZ533 && !bDifferentGainsZ533) ? 0 : -1;
            }


            bool bNoGainsZ183 = false;
            bool bMissingGainsZ183 = false;
            bool bDifferentGainsZ183 = false;
            bool bUniqueGainZ183 = false;

            if (bFoundZ183)
            {
                List<int> GainListZ183 = mFileList.Where(i => i.Gain > 0 && i.Camera == "Z183").Select(i => i.Gain).ToList();
                bNoGainsZ183 = GainListZ183.Count() == 0;
                bMissingGainsZ183 = GainListZ183.Count() != CameraList.Count(c => c == "Z183") && !bNoGainsZ183;
                bDifferentGainsZ183 = GainListZ183.Distinct().Count() > 1;
                bUniqueGainZ183 = !bMissingGainsZ183 && !bDifferentGainsZ183 && !bNoGainsZ183;

                ComboBox_KeywordUpdateTab_Camera_Z183Gain.DataSource = GainListZ183.OrderBy(item => item).Distinct().ToList();

                if (bUniqueGainZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Gain.ForeColor = Color.Black;

                if (!bMissingGainsZ183 && bDifferentGainsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Gain.ForeColor = Color.Green;

                if (bMissingGainsZ183 && !bDifferentGainsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Gain.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z183Gain.SelectedIndex = bUniqueGainZ183 || (bMissingGainsZ183 && !bDifferentGainsZ183) ? 0 : -1;
            }


            bool bNoGainsQ178 = false;
            bool bMissingGainsQ178 = false;
            bool bDifferentGainsQ178 = false;
            bool bUniqueGainQ178 = false;

            if (bFoundQ178)
            {
                List<int> GainListQ178 = mFileList.Where(i => i.Gain > 0 && i.Camera == "Q178").Select(i => i.Gain).ToList();
                bNoGainsQ178 = GainListQ178.Count() == 0;
                bMissingGainsQ178 = GainListQ178.Count() != CameraList.Count(c => c == "Q178") && !bNoGainsQ178;
                bDifferentGainsQ178 = GainListQ178.Distinct().Count() > 1 && !bMissingGainsQ178;
                bUniqueGainQ178 = !bMissingGainsQ178 && !bDifferentGainsQ178 && !bNoGainsQ178;

                ComboBox_KeywordUpdateTab_Camera_Q178Gain.DataSource = GainListQ178.OrderBy(item => item).Distinct().ToList();

                if (bUniqueGainQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Gain.ForeColor = Color.Black;

                if (!bMissingGainsQ178 && bDifferentGainsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Gain.ForeColor = Color.Green;

                if (bMissingGainsQ178 && !bDifferentGainsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Gain.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Q178Gain.SelectedIndex = bUniqueGainQ178 || (bMissingGainsQ178 && !bDifferentGainsQ178) ? 0 : -1;
            }


            if (bNoGainsZ533 || bMissingGainsZ533 || bNoGainsZ183 || bMissingGainsZ183 || bNoGainsQ178 || bMissingGainsQ178)
                Label_KeywordUpdateTab_Camera_Gain.ForeColor = Color.Red;
            else
                if (bDifferentGainsZ533 || bDifferentGainsZ183 || bDifferentGainsQ178)
                Label_KeywordUpdateTab_Camera_Gain.ForeColor = Color.Green;

            // ****************************************************************


            bool bNoOffsetsZ533 = false;
            bool bMissingOffsetsZ533 = false;
            bool bDifferentOffsetsZ533 = false;
            bool bUniqueOffsetZ533 = false;

            if (bFoundZ533)
            {
                List<int> OffsetListZ533 = mFileList.Where(i => i.Offset > 0 && i.Camera == "Z533").Select(i => i.Offset).ToList();
                bNoOffsetsZ533 = OffsetListZ533.Count() == 0;
                bMissingOffsetsZ533 = OffsetListZ533.Count() != CameraList.Count(c => c == "Z533") && !bNoOffsetsZ533;
                bDifferentOffsetsZ533 = OffsetListZ533.Distinct().Count() > 1 && !bMissingOffsetsZ533;
                bUniqueOffsetZ533 = !bMissingOffsetsZ533 && !bDifferentOffsetsZ533 && !bNoOffsetsZ533;

                ComboBox_KeywordUpdateTab_Camera_Z533Offset.DataSource = OffsetListZ533.OrderBy(item => item).Distinct().ToList();

                if (bUniqueOffsetZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Offset.ForeColor = Color.Black;

                if (!bMissingOffsetsZ533 && bDifferentOffsetsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Offset.ForeColor = Color.Green;

                if (bMissingOffsetsZ533 && !bDifferentOffsetsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Offset.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z533Offset.SelectedIndex = bUniqueOffsetZ533 || (bMissingOffsetsZ533 && !bDifferentOffsetsZ533) ? 0 : -1;
            }


            bool bNoOffsetsZ183 = false;
            bool bMissingOffsetsZ183 = false;
            bool bDifferentOffsetsZ183 = false;
            bool bUniqueOffsetZ183 = false;

            if (bFoundZ183)
            {
                List<int> OffsetListZ183 = mFileList.Where(i => i.Offset > 0 && i.Camera == "Z183").Select(i => i.Offset).ToList();
                bNoOffsetsZ183 = OffsetListZ183.Count() == 0;
                bMissingOffsetsZ183 = OffsetListZ183.Count() != CameraList.Count(c => c == "Z183") && !bNoOffsetsZ183;
                bDifferentOffsetsZ183 = OffsetListZ183.Distinct().Count() > 1;
                bUniqueOffsetZ183 = !bMissingOffsetsZ183 && !bDifferentOffsetsZ183 && !bNoOffsetsZ183;

                ComboBox_KeywordUpdateTab_Camera_Z183Offset.DataSource = OffsetListZ183.OrderBy(item => item).Distinct().ToList();

                if (bUniqueOffsetZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Offset.ForeColor = Color.Black;

                if (!bMissingOffsetsZ183 && bDifferentOffsetsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Offset.ForeColor = Color.Green;

                if (bMissingOffsetsZ183 && !bDifferentOffsetsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Offset.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z183Offset.SelectedIndex = bUniqueOffsetZ183 || (bMissingOffsetsZ183 && !bDifferentOffsetsZ183) ? 0 : -1;
            }

            bool bNoOffsetsQ178 = false;
            bool bMissingOffsetsQ178 = false;
            bool bDifferentOffsetsQ178 = false;
            bool bUniqueOffsetQ178 = false;

            if (bFoundQ178)
            {
                List<int> OffsetListQ178 = mFileList.Where(i => i.Offset > 0 && i.Camera == "Q178").Select(i => i.Offset).ToList();
                bNoOffsetsQ178 = OffsetListQ178.Count() == 0;
                bMissingOffsetsQ178 = OffsetListQ178.Count() != CameraList.Count(c => c == "Q178") && !bNoOffsetsQ178;
                bDifferentOffsetsQ178 = OffsetListQ178.Distinct().Count() > 1 && !bMissingOffsetsQ178;
                bUniqueOffsetQ178 = !bMissingOffsetsQ178 && !bDifferentOffsetsQ178 && !bNoOffsetsQ178;

                ComboBox_KeywordUpdateTab_Camera_Q178Offset.DataSource = OffsetListQ178.OrderBy(item => item).Distinct().ToList();

                if (bUniqueOffsetQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Offset.ForeColor = Color.Black;

                if (!bMissingOffsetsQ178 && bDifferentOffsetsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Offset.ForeColor = Color.Green;

                if (bMissingOffsetsQ178 && !bDifferentOffsetsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Offset.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Q178Offset.SelectedIndex = bUniqueOffsetQ178 || (bMissingOffsetsQ178 && !bDifferentOffsetsQ178) ? 0 : -1;
            }

            if (bNoOffsetsZ533 || bMissingOffsetsZ533 || bNoOffsetsZ183 || bMissingOffsetsZ183 || bNoOffsetsQ178 || bMissingOffsetsQ178)
                Label_KeywordUpdateTab_Camera_Offset.ForeColor = Color.Red;
            else
               if (bDifferentOffsetsZ533 || bDifferentOffsetsZ183 || bDifferentOffsetsQ178)
                Label_KeywordUpdateTab_Camera_Offset.ForeColor = Color.Green;


            // ****************************************************************

            bool bNoSensorTempsZ533 = false;
            bool bMissingSensorTempsZ533 = false;
            bool bDifferentSensorTempsZ533 = false;
            bool bUniqueSensorTempZ533 = false;

            if (bFoundZ533)
            {
                List<double> SensorTempListZ533 = mFileList.Where(i => i.SensorTemperature != -273 && i.Camera == "Z533").Select(i => i.SensorTemperature).ToList();
                bNoSensorTempsZ533 = SensorTempListZ533.Count() == 0;
                bMissingSensorTempsZ533 = SensorTempListZ533.Count() != CameraList.Count(c => c == "Z533") && !bNoSensorTempsZ533;
                bDifferentSensorTempsZ533 = SensorTempListZ533.Distinct().Count() > 1 && !bMissingSensorTempsZ533;
                bUniqueSensorTempZ533 = !bMissingSensorTempsZ533 && !bDifferentSensorTempsZ533 && !bNoSensorTempsZ533;

                ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.DataSource = SensorTempListZ533.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSensorTempZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.ForeColor = Color.Black;

                if (!bMissingSensorTempsZ533 && bDifferentSensorTempsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.ForeColor = Color.Green;

                if (bMissingSensorTempsZ533 && !bDifferentSensorTempsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.SelectedIndex = bUniqueSensorTempZ533 || (bMissingSensorTempsZ533 && !bDifferentSensorTempsZ533) ? 0 : -1;
            }

            bool bNoSensorTempsZ183 = false;
            bool bMissingSensorTempsZ183 = false;
            bool bDifferentSensorTempsZ183 = false;
            bool bUniqueSensorTempZ183 = false;

            if (bFoundZ183)
            {
                List<double> SensorTempListZ183 = mFileList.Where(i => i.SensorTemperature != -273 && i.Camera == "Z183").Select(i => i.SensorTemperature).ToList();
                bNoSensorTempsZ183 = SensorTempListZ183.Count() == 0;
                bMissingSensorTempsZ183 = SensorTempListZ183.Count() != CameraList.Count(c => c == "Z183") && !bNoSensorTempsZ183;
                bDifferentSensorTempsZ183 = SensorTempListZ183.Distinct().Count() > 1;
                bUniqueSensorTempZ183 = !bMissingSensorTempsZ183 && !bDifferentSensorTempsZ183 && !bNoSensorTempsZ183;

                ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.DataSource = SensorTempListZ183.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSensorTempZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.ForeColor = Color.Black;

                if (!bMissingSensorTempsZ183 && bDifferentSensorTempsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.ForeColor = Color.Green;

                if (bMissingSensorTempsZ183 && !bDifferentSensorTempsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.SelectedIndex = bUniqueSensorTempZ183 || (bMissingSensorTempsZ183 && !bDifferentSensorTempsZ183) ? 0 : -1;
            }

            bool bNoSensorTempsQ178 = false;
            bool bMissingSensorTempsQ178 = false;
            bool bDifferentSensorTempsQ178 = false;
            bool bUniqueSensorTempQ178 = false;

            if (bFoundQ178)
            {
                List<double> SensorTempListQ178 = mFileList.Where(i => i.FocuserTemperature != -273 && i.Camera == "Q178").Select(i => i.FocuserTemperature).ToList();
                bNoSensorTempsQ178 = SensorTempListQ178.Count() == 0;
                bMissingSensorTempsQ178 = SensorTempListQ178.Count() != CameraList.Count(c => c == "Q178") && !bNoSensorTempsQ178;
                bDifferentSensorTempsQ178 = SensorTempListQ178.Distinct().Count() > 1 && !bMissingSensorTempsQ178;
                bUniqueSensorTempQ178 = !bMissingSensorTempsQ178 && !bDifferentSensorTempsQ178 && !bNoSensorTempsQ178;

                ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.DataSource = SensorTempListQ178.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSensorTempQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.ForeColor = Color.Black;

                if (!bMissingSensorTempsQ178 && bDifferentSensorTempsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.ForeColor = Color.Green;

                if (bMissingSensorTempsQ178 && !bDifferentSensorTempsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.SelectedIndex = bUniqueSensorTempQ178 || (bMissingSensorTempsQ178 && !bDifferentSensorTempsQ178) ? 0 : -1;
            }

            bool bNoSensorTempsA144 = false;
            bool bMissingSensorTempsA144 = false;
            bool bDifferentSensorTempsA144 = false;
            bool bUniqueSensorTempA144 = false;

            if (bFoundA144)
            {
                List<double> SensorTempListA144 = mFileList.Where(i => i.FocuserTemperature != -273 && i.Camera == "A144").Select(i => i.FocuserTemperature).ToList();
                bNoSensorTempsA144 = SensorTempListA144.Count() == 0;
                bMissingSensorTempsA144 = SensorTempListA144.Count() != CameraList.Count(c => c == "A144") && !bNoSensorTempsA144;
                bDifferentSensorTempsA144 = SensorTempListA144.Distinct().Count() > 1 && !bMissingSensorTempsA144;
                bUniqueSensorTempA144 = !bMissingSensorTempsA144 && !bDifferentSensorTempsA144 && !bNoSensorTempsA144;

                ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.DataSource = SensorTempListA144.OrderBy(item => item).Distinct().ToList();

                if (bUniqueSensorTempA144)
                    ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.ForeColor = Color.Black;

                if (!bMissingSensorTempsA144 && bDifferentSensorTempsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.ForeColor = Color.Green;

                if (bMissingSensorTempsA144 && !bDifferentSensorTempsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.SelectedIndex = bUniqueSensorTempA144 || (bMissingSensorTempsA144 && !bDifferentSensorTempsA144) ? 0 : -1;
            }

            if (bNoSensorTempsZ533 || bMissingSensorTempsZ533 || bNoSensorTempsZ183 || bMissingSensorTempsZ183 || bNoSensorTempsQ178 || bMissingSensorTempsQ178 || bNoSensorTempsA144 || bMissingSensorTempsA144)
                Label_KeywordUpdateTab_Camera_SensorTemp.ForeColor = Color.Red;
            else
                if (bDifferentSensorTempsZ533 || bDifferentSensorTempsZ183 || bDifferentSensorTempsQ178 || bDifferentSensorTempsA144)
                Label_KeywordUpdateTab_Camera_SensorTemp.ForeColor = Color.Green;

            // ****************************************************************

            bool bNoBinningsZ533 = false;
            bool bMissingBinningsZ533 = false;
            bool bDifferentBinningsZ533 = false;
            bool bUniqueBinningZ533 = false;

            if (bFoundZ533)
            {
                List<int> BinningListZ533 = mFileList.Where(i => i.Binning > 0 && i.Camera == "Z533").Select(i => i.Binning).ToList();
                bNoBinningsZ533 = BinningListZ533.Count() == 0;
                bMissingBinningsZ533 = BinningListZ533.Count() != CameraList.Count(c => c == "Z533") && !bNoBinningsZ533;
                bDifferentBinningsZ533 = BinningListZ533.Distinct().Count() > 1;
                bUniqueBinningZ533 = !bMissingBinningsZ533 && !bDifferentBinningsZ533 && !bNoBinningsZ533;

                ComboBox_KeywordUpdateTab_Camera_Z533Binning.DataSource = BinningListZ533.OrderBy(item => item).Distinct().ToList();

                if (bUniqueBinningZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Binning.ForeColor = Color.Black;

                if (!bMissingBinningsZ533 && bDifferentBinningsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Binning.ForeColor = Color.Green;

                if (bMissingBinningsZ533 && !bDifferentBinningsZ533)
                    ComboBox_KeywordUpdateTab_Camera_Z533Binning.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z533Binning.SelectedIndex = bUniqueBinningZ533 || (bMissingBinningsZ533 && !bDifferentBinningsZ533) ? 0 : -1;
            }

            bool bNoBinningsZ183 = false;
            bool bMissingBinningsZ183 = false;
            bool bDifferentBinningsZ183 = false;
            bool bUniqueBinningZ183 = false;

            if (bFoundZ183)
            {
                List<int> BinningListZ183 = mFileList.Where(i => i.Binning > 0 && i.Camera == "Z183").Select(i => i.Binning).ToList();
                bNoBinningsZ183 = BinningListZ183.Count() == 0;
                bMissingBinningsZ183 = BinningListZ183.Count() != CameraList.Count(c => c == "Z183") && !bNoBinningsZ183;
                bDifferentBinningsZ183 = BinningListZ183.Distinct().Count() > 1;
                bUniqueBinningZ183 = !bMissingBinningsZ183 && !bDifferentBinningsZ183 && !bNoBinningsZ183;

                ComboBox_KeywordUpdateTab_Camera_Z183Binning.DataSource = BinningListZ183.OrderBy(item => item).Distinct().ToList();

                if (bUniqueBinningZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Binning.ForeColor = Color.Black;

                if (!bMissingBinningsZ183 && bDifferentBinningsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Binning.ForeColor = Color.Green;

                if (bMissingBinningsZ183 && !bDifferentBinningsZ183)
                    ComboBox_KeywordUpdateTab_Camera_Z183Binning.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Z183Binning.SelectedIndex = bUniqueBinningZ183 || (bMissingBinningsZ183 && !bDifferentBinningsZ183) ? 0 : -1;
            }

            bool bNoBinningsQ178 = false;
            bool bMissingBinningsQ178 = false;
            bool bDifferentBinningsQ178 = false;
            bool bUniqueBinningQ178 = false;

            if (bFoundQ178)
            {
                List<int> BinningListQ178 = mFileList.Where(i => i.Binning > 0 && i.Camera == "Q178").Select(i => i.Binning).ToList();
                bNoBinningsQ178 = BinningListQ178.Count() == 0;
                bMissingBinningsQ178 = BinningListQ178.Count() != CameraList.Count(c => c == "Q178") && !bNoBinningsQ178;
                bDifferentBinningsQ178 = BinningListQ178.Distinct().Count() > 1;
                bUniqueBinningQ178 = !bMissingBinningsQ178 && !bDifferentBinningsQ178 && !bNoBinningsQ178;

                ComboBox_KeywordUpdateTab_Camera_Q178Binning.DataSource = BinningListQ178.OrderBy(item => item).Distinct().ToList();

                if (bUniqueBinningQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Binning.ForeColor = Color.Black;

                if (!bMissingBinningsQ178 && bDifferentBinningsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Binning.ForeColor = Color.Green;

                if (bMissingBinningsQ178 && !bDifferentBinningsQ178)
                    ComboBox_KeywordUpdateTab_Camera_Q178Binning.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_Q178Binning.SelectedIndex = bUniqueBinningQ178 || (bMissingBinningsQ178 && !bDifferentBinningsQ178) ? 0 : -1;
            }

            bool bNoBinningsA144 = false;
            bool bMissingBinningsA144 = false;
            bool bDifferentBinningsA144 = false;
            bool bUniqueBinningsA144 = false;

            if (bFoundA144)
            {
                List<int> BinningsListA144 = mFileList.Where(i => i.Binning > 0 && i.Camera == "A144").Select(i => i.Binning).ToList();
                bNoBinningsA144 = BinningsListA144.Count() == 0;
                bMissingBinningsA144 = BinningsListA144.Count() != CameraList.Count(c => c == "A144") && !bNoBinningsA144;
                bDifferentBinningsA144 = BinningsListA144.Distinct().Count() > 1;
                bUniqueBinningsA144 = !bMissingBinningsA144 && !bDifferentBinningsA144 && !bNoBinningsA144;

                ComboBox_KeywordUpdateTab_Camera_A144Binning.DataSource = BinningsListA144.OrderBy(item => item).Distinct().ToList();

                if (bUniqueBinningsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144Binning.ForeColor = Color.Black;

                if (!bMissingBinningsA144 && bDifferentBinningsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144Binning.ForeColor = Color.Green;

                if (bMissingBinningsA144 && !bDifferentBinningsA144)
                    ComboBox_KeywordUpdateTab_Camera_A144Binning.ForeColor = Color.DarkViolet;

                ComboBox_KeywordUpdateTab_Camera_A144Binning.SelectedIndex = bUniqueBinningsA144 || (bMissingBinningsA144 && !bDifferentBinningsA144) ? 0 : -1;
            }

            if (bNoBinningsZ533 || bMissingBinningsZ533 || bNoBinningsZ183 || bMissingBinningsZ183 || bNoBinningsQ178 || bMissingBinningsQ178 || bNoBinningsA144 || bMissingBinningsA144)
                Label_KeywordUpdateTab_Camera_Binning.ForeColor = Color.Red;
            else
                if (bDifferentBinningsZ533 || bDifferentBinningsZ183 || bDifferentBinningsQ178 || bDifferentBinningsA144)
                Label_KeywordUpdateTab_Camera_Binning.ForeColor = Color.Green;

            // ****************************************************************
            // ****************************************************************
        }

        private void Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB_Click(object sender, EventArgs e)
        {
            if (CheckBox_KeywordUpdateTab_Camera_Z533.Checked)
            {
                if (Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text == "NB Preset")
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "BB Preset";
                    ComboBox_KeywordUpdateTab_Camera_Z533Gain.Text = "100";
                    ComboBox_KeywordUpdateTab_Camera_Z533Offset.Text = "50";
                }
                else
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "NB Preset";
                    ComboBox_KeywordUpdateTab_Camera_Z533Gain.Text = "100";
                    ComboBox_KeywordUpdateTab_Camera_Z533Offset.Text = "50";
                }
            }

            if (CheckBox_KeywordUpdateTab_Camera_Z183.Checked)
            {
                if (Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text == "NB Preset")
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "BB Preset";
                    ComboBox_KeywordUpdateTab_Camera_Z183Gain.Text = "53";
                    ComboBox_KeywordUpdateTab_Camera_Z183Offset.Text = "10";
                }
                else
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "NB Preset";
                    ComboBox_KeywordUpdateTab_Camera_Z183Gain.Text = "111";
                    ComboBox_KeywordUpdateTab_Camera_Z183Offset.Text = "10";
                }
            }

            if (CheckBox_KeywordUpdateTab_Camera_Q178.Checked)
            {
                if (Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text == "NB Preset")
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "NB Preset";
                    ComboBox_KeywordUpdateTab_Camera_Q178Gain.Text = "40";
                    ComboBox_KeywordUpdateTab_Camera_Q178Offset.Text = "15";
                }
                else
                {
                    Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "BB Preset";
                    ComboBox_KeywordUpdateTab_Camera_Q178Gain.Text = "40";
                    ComboBox_KeywordUpdateTab_Camera_Q178Offset.Text = "15";
                }
            }
        }

        private void Button_KeywordCamera_SetAll_Click(object sender, EventArgs e)
        {
            double value;
            int parseInt;
            int checkedCount = 0;

            if (mFileList.Count == 0)
                return;

            checkedCount += CheckBox_KeywordUpdateTab_Camera_A144.Checked ? 1 : 0;
            checkedCount += CheckBox_KeywordUpdateTab_Camera_Q178.Checked ? 1 : 0;
            checkedCount += CheckBox_KeywordUpdateTab_Camera_Z183.Checked ? 1 : 0;
            checkedCount += CheckBox_KeywordUpdateTab_Camera_Z533.Checked ? 1 : 0;

            if (checkedCount != 1)
                return;

            foreach (XisfFile file in mFileList)
            {
                file.RemoveKeyword("NAXIS3");

                file.AddKeyword("BITPIX", 16, "Bits Per Pixel");
                file.AddKeyword("BSCALE", 1, "Multiply Raw Values by BSCALE");
                file.AddKeyword("BZERO", 32768, "Add value to scale to 65536 (16 bit) values");

                bool status = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Text, out value);
                if (status)
                {
                    file.AddKeyword("CCD-TEMP", value, "Actual Sensor Temperature");
                }

                file.AddKeyword("NAXIS", 2, "XISF File Manager");

                status = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Text, out value);
                if (status)
                {
                    file.AddKeyword("EXPTIME", value, "Exposure Time in Seconds");
                    file.ExposureSeconds = value;
                }

                if (CheckBox_KeywordUpdateTab_Camera_Z533.Checked)
                {
                    file.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    file.AddKeyword("NAXIS1", 3008, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 3008, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 3.76, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 3.76, "Vertical Pixel Size in Microns");
                    file.AddKeyword("BAYERPAT", "RGGB");
                    file.AddKeyword("COLORSPC", "Color", "Color Image");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Gain.Text, out parseInt);
                    if (status)
                        file.Gain = parseInt;

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Offset.Text, out parseInt);
                    if (status)
                        file.Offset = parseInt;

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text, out parseInt);
                    if (status)
                    {
                        file.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }
                }

                if (CheckBox_KeywordUpdateTab_Camera_Z183.Checked)
                {
                    file.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    file.AddKeyword("NAXIS1", 5496, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 3672, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Gain.Text, out parseInt);
                    if (status)
                        file.Gain = parseInt;

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Offset.Text, out parseInt);
                    if (status)
                        file.Offset = parseInt;

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text, out parseInt);
                    if (status)
                    {
                        file.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }
                }

                if (CheckBox_KeywordUpdateTab_Camera_Q178.Checked)
                {
                    file.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    file.AddKeyword("NAXIS1", 3072, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 2048, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Gain.Text, out parseInt);
                    if (status)
                        file.Gain = parseInt;

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Offset.Text, out parseInt);
                    if (status)
                        file.Offset = parseInt;

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text, out parseInt);
                    if (status)
                    {
                        file.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }
                }

                if (CheckBox_KeywordUpdateTab_Camera_A144.Checked)
                {
                    file.AddKeyword("INSTRUME", "A144", "Atik Infinity Camera (2018)");
                    file.AddKeyword("NAXIS1", 1392, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 1040, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 6.45, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 6.45, "Vertical Pixel Size in Microns");
                    file.AddKeyword("BAYERPAT", "RGGB");
                    file.AddKeyword("COLORSPC", "Color", "Color Image");
                    file.AddKeyword("GAIN", 0.37);
                    file.RemoveKeyword("OFFSET");
                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text, out parseInt);
                    if (status)
                    {
                        file.AddKeyword("XBINNING", parseInt, "Horizontal Binning");
                        file.AddKeyword("YBINNING", parseInt, "Vertical Binning");
                    }
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
                file.RemoveKeyword("NAXIS3");
                file.RemoveKeyword("EXPOSURE");

                file.AddKeyword("BITPIX", 16, "Bits Per Pixel");
                file.AddKeyword("BSCALE", 1, "Multiply Raw Values by BSCALE");
                file.AddKeyword("BZERO", 32768, "Add value to scale to 65536 (16 bit) values");
                string temperatureTextUI = ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Text;

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
                file.AddKeyword("CCD-TEMP", temperature, "Actual Sensor Temperature");

                file.AddKeyword("NAXIS", 2, "XISF File Manager");
                file.Binning = Int32.Parse(ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text);
                string secondsTextUI = ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Text;

                string secondsText;
                if (globalSeconds)
                {
                    secondsText = file.ExposureSeconds.FormatExposureTime();
                    if (secondsText == string.Empty)
                    {
                        secondsText = globalSecondsText;
                    }
                }
                else
                {
                    if (secondsTextUI == string.Empty)
                    {
                        globalSecondsText = file.ExposureSeconds.FormatExposureTime();
                        if (globalSecondsText.Contains("Global_"))
                        {
                            globalSeconds = true;
                            globalSecondsText = globalSecondsText.Replace("Global_", "");
                        }

                        secondsText = globalSecondsText;
                    }
                    else
                    {
                        secondsText = file.ExposureSeconds.FormatExposureTime();
                        if (secondsText == string.Empty)
                        {
                            secondsText = secondsTextUI;
                        }
                    }
                }

                double seconds;
                status = double.TryParse(secondsText, out seconds);
                file.AddKeyword("EXPTIME", seconds, "Exposure Time in Seconds");




                int gainValue;
                int gainValueUI;
                int offsetValue;
                int offsetValueUI;
                if (CheckBox_KeywordUpdateTab_Camera_Z533.Checked)
                {
                    file.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    file.AddKeyword("NAXIS1", 3008, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 3008, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 3.76, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 3.76, "Vertical Pixel Size in Microns");
                    file.AddKeyword("BAYERPAT", "RGGB");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = file.Gain;
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = file.Gain;
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = file.Gain;
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    file.Gain = gainValue;


                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = file.Offset;
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = file.Offset;
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = file.Offset;
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    file.Offset = offsetValue;
                }

                if (CheckBox_KeywordUpdateTab_Camera_Z183.Checked)
                {
                    file.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    file.AddKeyword("NAXIS1", 5496, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 3672, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = file.Gain;
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = file.Gain;
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = file.Gain;
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    file.Gain = gainValue;


                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = file.Offset;
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = file.Offset;
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = file.Offset;
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    file.AddKeyword("OFFSET", offsetValue, "Camera Offset");
                }




                if (CheckBox_KeywordUpdateTab_Camera_Q178.Checked)
                {
                    file.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    file.AddKeyword("NAXIS1", 3072, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 2048, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = file.Gain;
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = file.Gain;
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = file.Gain;
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    file.Gain = gainValue;


                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = file.Offset;
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = file.Offset;
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = file.Offset;
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    file.Offset = offsetValue;
                }

                if (CheckBox_KeywordUpdateTab_Camera_A144.Checked)
                {
                    file.AddKeyword("INSTRUME", "A144", "Atik Infinity Camera (2018)");
                    file.AddKeyword("NAXIS1", 1392, "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", 1040, "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", 6.45, "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", 6.45, "Vertical Pixel Size in Microns");
                    file.AddKeyword("BAYERPAT", "RGGB");
                    file.RemoveKeyword("GAIN");
                    file.RemoveKeyword("OFFSET");
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

                    frames = file.TotalFrames;
                    if (frames < 0)
                    {
                        frames = globalFrames;
                    }

                }
                else
                {
                    frames = file.TotalFrames;
                    if (frames < 0)
                    {
                        globalTotalFrames = true;
                        globalFrames = -frames;
                        frames = globalFrames;
                    }
                }

                file.AddKeyword("NUM-FRMS", frames, "Number of Integrated SubFrames");
            }
        }

        private void CheckBox_Master_CheckedChanged(object sender, EventArgs e)
        {
            bool foundNumberOfImages = false;
            bool foundRejection = false;

            int numberOfImages = 0;
            string rejection = string.Empty;
            string comment = string.Empty;

            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.Enabled = CheckBox_FileSelection_DirectorySelection_Master.Checked;
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Enabled = CheckBox_FileSelection_DirectorySelection_Master.Checked;

            if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
            {
                foreach (XisfFile file in mFileList)
                {
                    foreach (Keyword node in file.mKeywordList.mKeywordList)
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

                            if (node.Comment.ToLower().Contains("winsor"))
                            {
                                rejection = "WSC";
                                comment = "PixInsight Winsorized Sigma Clipping";
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
                        }
                    }

                    if (foundNumberOfImages)
                        file.AddKeyword("NUM-FRMS", numberOfImages, "Number of Integrated SubFrames");

                    if (foundRejection)
                        file.AddKeyword("RJCT-ALG", rejection, comment);
                }
            }
        }

        private void Button_KeywordSubFrameWeight_Remove_Click(object sender, EventArgs e)
        {
            List<string> WeightKeywords = new List<string>();

            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();

            // Repopulate the list of any present weight keywords (not values). Find unique Keyords, sort and populate Weight combobox
            foreach (XisfFile xFile in mFileList)
            {
                WeightKeywords.Add(xFile.WeightKeyword.ToString());
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
                        file.RemoveKeyword(item);
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
                    file.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Text);
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
                string targetCalibrationDirectory = mCalibration.SetTargetCalibrationFileDirectories(mFileList[0].FilePath);

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


            string directoryName = Path.GetDirectoryName(mFileList[0].FilePath);
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
                file.RemoveKeyword("CDARK");
                file.CDARK = string.Empty;

                file.RemoveKeyword("CFLAT");
                file.CFLAT = string.Empty;

                file.RemoveKeyword("CBIAS");
                file.CBIAS = string.Empty;

                file.RemoveKeyword("CPANEL");
                file.CPANEL = string.Empty;

                file.RemoveKeyword("CLIGHT");
                file.CLIGHT = string.Empty;
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
            if ((string.IsNullOrEmpty(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text) || ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text == "Keyword"))
                return;

            List<Keyword> keywordList = new List<Keyword>();

            foreach (XisfFile file in mFileList)
            {
                foreach (var keyword in file.mKeywordList.mKeywordList)
                {
                    keywordList.Add(keyword);
                }
            }

            // Uniquify the keywordList based on Keyword.Name and Keyword.Value while keeping associated Comment and FilePath
            keywordList = keywordList
                .GroupBy(k => new { k.Name, k.Value })
                .Select(g => g.First())
                .OrderBy(k => k.Name)
                .ToList();

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text = "";

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text = "";



            foreach (var value in keywordList)
            {
                if (ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text == value.Name)
                {
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Add(value.Value);
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text = value.Value.ToString();
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text = value.Comment;
                }
            }
        }

        private void ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue_SelectedValueChanged(object sender, EventArgs e)
        {
            List<Keyword> keywordList = new List<Keyword>();

            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Checked)
                    file.mKeywordList.AddKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);

                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Checked)
                {
                    file.mKeywordList.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text);
                    file.mKeywordList.AddKeywordKeepDuplicates(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);
                }
            }
        }

        private void Button_KeywordUpdateTab_SubFrameKeywords_Delete_Click(object sender, EventArgs e)
        {
            string name = ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text;
            string value = ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text;

            foreach (XisfFile xFile in mFileList)
            {
                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Checked)
                    xFile.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text);

                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Checked)
                    xFile.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text);
            }

            RefreshComboBoxes();
        }

        private void Button_KeywordUpdateTab_SubFrameKeywords_AddReplace_Click(object sender, EventArgs e)
        {
            foreach (XisfFile xFile in mFileList)
            {
                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Checked)
                    xFile.mKeywordList.AddKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);

                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Checked)
                {
                    xFile.mKeywordList.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text);
                    xFile.mKeywordList.AddKeywordKeepDuplicates(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);
                }
            }

            RefreshComboBoxes();

            /*
            foreach (XisfFile file in mFileList)
            {
                bool bFound = false;

                string foundItem = file.mKeywordList.mKeywordList.OfType<string>().FirstOrDefault(item => item == ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text);


                foreach (var keyword in file.mKeywordList.mKeywordList.ToList())
                {
                    if (keyword.Name.Equals(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text))
                    {
                        keyword.Value = (object)ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text;
                        file.AddKeyword(keyword.Name, keyword.Value, keyword.Comment);
                        bFound = true;
                    }
                }

                if (!bFound)
                    file.AddKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text);
            }

            RefreshComboBoxes();
            */
        }

        private void RefreshComboBoxes()
        {
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text = "Name";

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text = "Value";

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text = "Comment";


            List<string> keywordNamelist = new List<string>();

            foreach (XisfFile xFile in mFileList)
            {
                foreach (var keywordName in xFile.mKeywordList.mKeywordList)
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
        }
    }
}
