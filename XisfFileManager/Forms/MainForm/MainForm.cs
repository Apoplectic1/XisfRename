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

    public delegate void DataReceivedEventHandler(CalibrationTabPageValues data);


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
        private readonly XisfFileRead mFileReader = new XisfFileRead();
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
        private Calibration mCalibration;
        private DirectoryOps mDirectoryOps;
        private DirectoryOps.FileType mFileType = DirectoryOps.FileType.NO_MASTERS;
        

        public MainForm()
        {
            InitializeComponent();
            CalibrationTabPageEvent.CalibrationTabPage_InvokeEvent += EventHandler_UpdateCalibrationPageForm;

            //mDelegateValues = new CalibrationPageValues();
            mDirectoryOps = new DirectoryOps();
            mCalibration = new Calibration();
            TabControl_Update.Selected += new TabControlEventHandler(TabControl_Update_Selected);

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
            Label_FileSelection_TempratureCompensation.Text = "Temperature Coefficient: Not Computed";


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

            Utility.ToolTips.AddToolTip(RadioButton_FileSelection_Index_ByFilter, "Orders Files by Capture Time per Filter", "\"By Target\" orders each filter's files consecutively.\r\n\"By Night\" orders each filter's files consecutively by night.");
            Utility.ToolTips.AddToolTip(RadioButton_FileSelection_Index_ByTime, "Orders Files by Capture Time", "\"By Target\" orders all files consecutively.\r\n\"By Night\" orders all files consecutively by night.");
        }

        // ****************************************************************************************************************
        // ************************ Event Handlers ************************************************************************
        // ****************************************************************************************************************

        // Executes when TabControl_Updated is selected (changed)
        private void TabControl_Update_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == TabPage_Calibration.Name)
            {
                if (mFile != null)
                {
                    if (mFile.Camera.Contains("Z533")) mCalibration.Camera = DirectoryOps.CameraType.Z533;
                    if (mFile.Camera.Contains("Z183")) mCalibration.Camera = DirectoryOps.CameraType.Z183;
                    if (mFile.Camera.Contains("A144")) mCalibration.Camera = DirectoryOps.CameraType.A144;
                    if (mFile.Camera.Contains("Q178")) mCalibration.Camera = DirectoryOps.CameraType.Q178;
                }
            }
        }
        
        private void EventHandler_UpdateCalibrationPageForm(CalibrationTabPageValues data)
        {
            ProgressBar_Calibration.Value = data.Progress;
            Label_Calibration_ReadFileName.Text = data.FileName;
            Label_Calibration_TotalFiles.Text = "Found " + data.TotalFiles.ToString() + " Files";
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

            //this.Size = new Size(1019, 750);
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
            // Clear all lists - we are reading or re-reading what will become a new xisf file data set that will invalidate any existing data.         
            mFileList.Clear();
            SubFrameLists.Clear();
            SubFrameNumericLists.Clear();
            ImageParameterLists.Clear();
            ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.Text = "";
            ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.Items.Clear();

            ProgressBar_FileSelection_OverAll.Value = 0;
            ProgressBar_Keyword_XisfFile.Value = 0;
            TabControl_Update.Enabled = false;

            mFolder = new OpenFolderDialog()
            {
                Title = "Select .xisf Folder",
                //AutoUpgradeEnabled = true,
                CheckPathExists = false,
                InitialDirectory = mFolderBrowseState, // @"E:\Photography\Astro Photography\Processing",
                Multiselect = false,
                RestoreDirectory = true
            };

            if (mFolder.ShowDialog(IntPtr.Zero).Equals(DialogResult.OK) == false)
            {
                return;
            }

            mFolderBrowseState = mFolder.SelectedPaths[0];

            try
            {
                DirectoryInfo diDirectoryTree = new DirectoryInfo(mFolder.SelectedPaths[0]);

                mDirectoryOps.ClearFileList();
                mDirectoryOps.Filter = DirectoryOps.FilterType.ALL;
                mDirectoryOps.File = mFileType;
                mDirectoryOps.Camera = DirectoryOps.CameraType.ALL;
                mDirectoryOps.Frame = DirectoryOps.FrameType.ALL;
                mDirectoryOps.Recurse = CheckBox_FileSelection_DirectorySelection_Recurse.Checked;

                mDirectoryOps.RecuseDirectories(diDirectoryTree);

                Label_FileSelection_Statistics_Task.Text = "Reading " + mDirectoryOps.Files.Count.ToString() + " Image Files";

                ProgressBar_FileSelection_OverAll.Value = 0;

                if (mDirectoryOps.Files.Count == 0)
                {
                    MessageBox.Show("No .xisf Files Found", "Select .xisf Folder");
                    return;
                }

                ProgressBar_FileSelection_OverAll.Maximum = mDirectoryOps.Files.Count;

                foreach (FileInfo file in mDirectoryOps.Files)
                {
                    bool bStatus = false;
                    ProgressBar_FileSelection_OverAll.Value += 1;
                    Application.DoEvents();

                    // Create a new xisf file instance
                    mFile = new XisfFile
                    {
                        SourceFileName = file.FullName
                    };

                    Label_FileSelection_BrowseFileName.Text = Path.GetDirectoryName(file.FullName) + "\n" + Path.GetFileName(file.FullName);

                    // Get the keyword data contained within the current file (mFile)
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


                    bStatus = mFileReader.ReadXisfFile(mFile);

                    // If data was able to be properly read from our current .xisf file, add the current mFile instance to our master list mFileList.
                    if (bStatus)
                    {
                        mFileList.Add(mFile);
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

                Label_FileSelection_Statistics_Task.Text = "Browse Aborted";
                return;
            }

            // Sort Image File Lists by Capture Time
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

                Label_FileSelection_Statistics_Task.Text = "Browse Aborted";
                return;
            }

            Label_FileSelection_Statistics_Task.Text = "Found " + mFileList.Count().ToString() + " Images";

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
            // Get TargetName and and Weights to populate ComboBoxes

            // First get a list of all the target names found in the source files, then find unique names ans sort.
            // Place culled list in the target name combobox
            List<string> TargetNames = new List<string>();
            List<string> WeightKeywords = new List<string>();

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
                    Label_KeywordUpdate_SubFrameKeywords_TagetName.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdate_SubFrameKeywords_TagetName.ForeColor = Color.Black;
                }

                foreach (string item in TargetNames)
                {
                    ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.Items.Add(item);
                }

                ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.SelectedIndex = 0;
            }
            else
            {
                ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.Items.Clear();
                Label_KeywordUpdate_SubFrameKeywords_TagetName.ForeColor = Color.DarkViolet;
            }


            // Now find a list of any present weight keywords (not values). Find unique Keyords, sort and populate Weight combobox
            foreach (XisfFile file in mFileList)
            {
                WeightKeywords.Add(file.KeywordData.WeightKeyword());
            }

            if (WeightKeywords.Count > 0)
            {
                WeightKeywords = WeightKeywords.Distinct().ToList();
                WeightKeywords = WeightKeywords.OrderBy(q => q).ToList();

                foreach (string item in WeightKeywords)
                {
                    ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items.Add(item);
                }

                if (WeightKeywords.Count > 1)
                {
                    Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Black;
                }

                ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.SelectedIndex = 0;
            }
            else
            {
                ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();
                Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Black;
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

            Label_FileSelection_Statistics_SubFrameOverhead.Text = ImageParameterLists.CalculateOverhead(mFileList);
            string stepsPerDegree = ImageParameterLists.CalculateFocuserTemperatureCompensationCoefficient();
            Label_FileSelection_TempratureCompensation.Text = "Temperature Coefficient: " + stepsPerDegree;
            // **********************************************************************


            // Need to add calculations for average capture duration/overhead
            // **********************************************************************

            SetUISubFrameGroupBoxState();

            // **********************************************************************
            FindCaptureSoftware();
            FindFrameType();
            FindTelescope();
            FindCamera();
            FindFrameType();
            // **********************************************************************
            TabControl_Update.Enabled = true;
        }

        public void SetFileIndex(bool bTarget, bool bNight, bool bFilter, bool bTime, List<XisfFile> fileList)
        {
            int index = 0;
            int lumaIndex = 0;
            int redIndex = 0;
            int greenIndex = 0;
            int blueIndex = 0;
            int haIndex = 0;
            int o3Index = 0;
            int s2Index = 0;

            foreach (XisfFile file in fileList)
            {
                if (bTarget)
                {
                    if (bFilter)
                    {
                        if (file.KeywordData.FilterName().Equals("Luma"))
                            file.Index = (file.Unique) ? ++lumaIndex : lumaIndex + 1;

                        if (file.KeywordData.FilterName().Equals("Red"))
                            file.Index = (file.Unique) ? ++redIndex : redIndex + 1;

                        if (file.KeywordData.FilterName().Equals("Green"))
                            file.Index = (file.Unique) ? ++greenIndex : greenIndex + 1;

                        if (file.KeywordData.FilterName().Equals("Blue"))
                            file.Index = (file.Unique) ? ++blueIndex : blueIndex + 1;

                        if (file.KeywordData.FilterName().Equals("Ha"))
                            file.Index = (file.Unique) ? ++haIndex : haIndex + 1;

                        if (file.KeywordData.FilterName().Equals("O3"))
                            file.Index = (file.Unique) ? ++o3Index : o3Index + 1;

                        if (file.KeywordData.FilterName().Equals("S2"))
                            file.Index = (file.Unique) ? ++s2Index : s2Index + 1;
                    }

                    if (bTime)
                    {
                        file.Index = (file.Unique) ? ++index : index + 1;
                    }
                }

                if (bNight)
                {
                    if (bFilter)
                    {

                    }

                    if (bTime)
                    {

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

            ProgressBar_Keyword_XisfFile.Maximum = mFileList.Count();
            ProgressBar_Keyword_XisfFile.Value = 0;

            mRenameFile.MarkDuplicates(mFileList);

            SetFileIndex(byTarget, byNight, byFilter, byTime, mFileList);

            foreach (XisfFile file in mFileList)
            {
                ProgressBar_Keyword_XisfFile.Value += 1;
                Label_FileSelection_BrowseFileName.Text = Path.GetDirectoryName(file.SourceFileName) + "\n" + Path.GetFileName(file.SourceFileName);

                file.Master = CheckBox_FileSelection_DirectorySelection_Master.Checked;

                Tuple<int, string> renameTuple = mRenameFile.RenameFile(file.Index, file);

                duplicates += (renameTuple.Item1 == 0) ? 1 : 0;

                Label_Keyword_UpdateFileName.Text = Path.GetDirectoryName(renameTuple.Item2) + "\n" + Path.GetFileName(renameTuple.Item2);

                Application.DoEvents();
            }

            ProgressBar_Keyword_XisfFile.Value = ProgressBar_Keyword_XisfFile.Maximum;

            if (duplicates == 1)
                Label_FileSelection_Statistics_Task.Text = (mFileList.Count() - duplicates).ToString() + " Images Renamed\n" + duplicates.ToString() + " Duplicate";
            else
                Label_FileSelection_Statistics_Task.Text = (mFileList.Count() - duplicates).ToString() + " Images Renamed\n" + duplicates.ToString() + " Duplicates";

            mFileList.Clear();

            ProgressBar_FileSelection_OverAll.Value = 0;
        }

        private void Button_KeywordSubFrame_UpdateXisfFiles_Click(object sender, EventArgs e)
        {
            bool bStatus;
            GroupBox_FileSelection.Enabled = false;
            TabControl_Update.Enabled = false;

            Label_FileSelection_Statistics_Task.Text = "Updating " + mFileList.Count().ToString() + " File Keywords";
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

                    Label_FileSelection_Statistics_Task.Text = "Update Aborted";
                    GroupBox_FileSelection.Enabled = true;
                    TabControl_Update.Enabled = true;
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

            XisfFileUpdate.TargetName = ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.Text.Replace("'", "").Replace("\"", "");

            foreach (XisfFile file in mFileList)
            {
                file.KeywordData.SetObservationSite();

                if (CheckBox_KeywordUpdate_SubFrameKeywords_UpdateTargetName.Checked)
                    file.KeywordData.AddKeyword("OBJECT", ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.Text.Replace("'", "").Replace("\"", ""), "Imaging Target");

                file.Master = CheckBox_FileSelection_DirectorySelection_Master.Checked;

                if (file.Master)
                    file.KeywordData.AddKeyword("OBJECT", "Master", "Master Integration Frame");

                ProgressBar_Keyword_XisfFile.Value += 1;
                bStatus = XisfFileUpdate.UpdateFile(file, SubFrameLists);
                Label_Keyword_UpdateFileName.Text = Label_Keyword_UpdateFileName.Text = Path.GetDirectoryName(file.SourceFileName) + "\n" + Path.GetFileName(file.SourceFileName);
                Application.DoEvents();

                if (bStatus == false)
                {
                    Label_FileSelection_Statistics_Task.Text = "File Write Error";

                    var result = MessageBox.Show(
                        "File Update Failed.\n\n" + Label_Keyword_UpdateFileName.Text,
                        "\nMainForm.cs Button_Update_Click()",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    GroupBox_FileSelection.Enabled = true;
                    TabControl_Update.Enabled = true;
                    return;
                }
            }

            Label_FileSelection_Statistics_Task.Text = mFileList.Count().ToString() + " Images Updated";
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
            if (RadioButton_SubFrameKeywords_AlphabetizeKeywords.Checked)
            {
                SetUISubFrameGroupBoxState();
            }
        }

        private void RadioButton_SubFrameKeyWords_SubFrameWeightCalculations_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_KeywordUpdate_SubFrameKeywords_SubFrameWeightCalculations.Checked)
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


        private void FindCaptureSoftware()
        {
            RadioButton_KeywordSoftware_TSX.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_NINA.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_SGP.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_VOY.ForeColor = Color.Black;
            RadioButton_KeywordSoftware_SCP.ForeColor = Color.Black;

            RadioButton_KeywordSoftware_TSX.Checked = false;
            RadioButton_KeywordSoftware_NINA.Checked = false;
            RadioButton_KeywordSoftware_SGP.Checked = false;
            RadioButton_KeywordSoftware_VOY.Checked = false;
            RadioButton_KeywordSoftware_SCP.Checked = false;

            Button_KeywordSoftware_SetAll.ForeColor = Color.Black;
            Button_KeywordSoftware_SetByFile.ForeColor = Color.Black;

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
                    RadioButton_KeywordSoftware_TSX.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_TSX.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_TSX.Checked = true;
                }
            }

            if (foundNINA)
            {
                if (foundTSX | foundSGP | foundVOY | foundSCP)
                {
                    RadioButton_KeywordSoftware_NINA.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_NINA.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_NINA.Checked = true;
                }
            }

            if (foundSGP)
            {
                if (foundTSX | foundNINA | foundVOY | foundSCP)
                {
                    RadioButton_KeywordSoftware_SGP.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_SGP.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_SGP.Checked = true;
                }
            }

            if (foundVOY)
            {
                if (foundTSX | foundNINA | foundSGP | foundSCP)
                {
                    RadioButton_KeywordSoftware_VOY.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_VOY.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_VOY.Checked = true;
                }
            }

            if (foundSCP)
            {
                if (foundTSX | foundNINA | foundSGP | foundVOY)
                {
                    RadioButton_KeywordSoftware_SCP.ForeColor = Color.Red;
                    RadioButton_KeywordSoftware_SCP.Checked = false;
                }
                else
                {
                    RadioButton_KeywordSoftware_SCP.Checked = true;
                }
            }

            if (!foundTSX && !foundNINA && !foundSGP && !foundVOY && !foundSCP)
            {
                RadioButton_KeywordSoftware_TSX.ForeColor = Color.DarkViolet;
                RadioButton_KeywordSoftware_NINA.ForeColor = Color.DarkViolet;
                RadioButton_KeywordSoftware_SGP.ForeColor = Color.DarkViolet;
                RadioButton_KeywordSoftware_VOY.ForeColor = Color.DarkViolet;
                RadioButton_KeywordSoftware_SCP.ForeColor = Color.DarkViolet;
            }

            if (foundTSX ^ foundNINA ^ foundSGP ^ foundVOY ^ foundSCP)
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

                if (RadioButton_KeywordSoftware_NINA.Checked)
                {
                    count++;
                    file.KeywordData.AddKeyword("CREATOR", "NINA");
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

                file.ParseRequiredKeywords();
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
            if (CheckBox_KeywordTelescope_Riccardi.Checked)
            {
                TextBox_KeywordTelescope_FocalLength.Text = "525";
            }
            else
            {
                TextBox_KeywordTelescope_FocalLength.Text = "700";
            }
        }

        private void RadioButton_KeywordTelescope_EVO150_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_KeywordTelescope_Riccardi.Checked)
            {
                TextBox_KeywordTelescope_FocalLength.Text = "750";
            }
            else
            {
                TextBox_KeywordTelescope_FocalLength.Text = "1000";
            }
        }

        private void RadioButton_KeywordTelescope_NWT254_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_KeywordTelescope_Riccardi.Checked)
            {
                TextBox_KeywordTelescope_FocalLength.Text = "825";
            }
            else
            {
                TextBox_KeywordTelescope_FocalLength.Text = "1100";
            }
        }

        private void CheckBox_KeywordTelescope_Riccardi_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_KeywordTelescope_APM107.Checked)
            {
                if (CheckBox_KeywordTelescope_Riccardi.Checked)
                    TextBox_KeywordTelescope_FocalLength.Text = "525";
                else
                    TextBox_KeywordTelescope_FocalLength.Text = "700";
            }

            if (RadioButton_KeywordTelescope_EVO150.Checked)
            {
                if (CheckBox_KeywordTelescope_Riccardi.Checked)
                    TextBox_KeywordTelescope_FocalLength.Text = "750";
                else
                    TextBox_KeywordTelescope_FocalLength.Text = "1000";
            }

            if (RadioButton_KeywordTelescope_NWT254.Checked)
            {
                if (CheckBox_KeywordTelescope_Riccardi.Checked)
                    TextBox_KeywordTelescope_FocalLength.Text = "825";
                else
                    TextBox_KeywordTelescope_FocalLength.Text = "1100";
            }
        }

        private void FindTelescope()
        {
            string telescope;
            int focalLength;
            int telescopeCount = 0;
            int riccardiCount = 0;
            int focalCount = 0;
            bool foundAPM = false;
            bool foundEVO = false;
            bool foundNWT = false;
            bool foundRiccardi = false;
            bool multipleFocalLengths = false;
            bool foundFocalLength = false;

            RadioButton_KeywordTelescope_APM107.Checked = false;
            RadioButton_KeywordTelescope_APM107.ForeColor = Color.Black;

            RadioButton_KeywordTelescope_EVO150.Checked = false;
            RadioButton_KeywordTelescope_EVO150.ForeColor = Color.Black;

            RadioButton_KeywordTelescope_NWT254.Checked = false;
            RadioButton_KeywordTelescope_NWT254.ForeColor = Color.Black;

            CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Black;
            CheckBox_KeywordTelescope_Riccardi.Checked = false;

            TextBox_KeywordTelescope_FocalLength.Text = "";
            Label_KeywordTelescope_FocalLength.ForeColor = Color.Black;

            Button_KeywordTelescope_SetAll.ForeColor = Color.Black;
            Button_KeywordTelescope_SetByFile.ForeColor = Color.Black;

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
                CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.Red;
            }
            else
            {
                CheckBox_KeywordTelescope_Riccardi.Checked = true;
            }

            if ((focalCount != mFileList.Count) || !foundFocalLength || multipleFocalLengths)
            {
                Label_KeywordTelescope_FocalLength.ForeColor = Color.Red;
            }


            if (foundAPM)
            {
                if (foundEVO || foundNWT)
                {
                    RadioButton_KeywordTelescope_APM107.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordTelescope_APM107.Checked = true;

                    if (foundRiccardi)
                        TextBox_KeywordTelescope_FocalLength.Text = "525";
                    else
                        TextBox_KeywordTelescope_FocalLength.Text = "700";
                }
            }

            if (foundEVO)
            {
                if (foundAPM || foundNWT)
                {
                    RadioButton_KeywordTelescope_EVO150.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordTelescope_EVO150.Checked = true;

                    if (foundRiccardi)
                        TextBox_KeywordTelescope_FocalLength.Text = "750";
                    else
                        TextBox_KeywordTelescope_FocalLength.Text = "1000";
                }
            }

            if (foundNWT)
            {
                if (foundAPM || foundEVO)
                {
                    RadioButton_KeywordTelescope_NWT254.ForeColor = Color.Red;
                }
                else
                {
                    RadioButton_KeywordTelescope_NWT254.Checked = true;

                    if (foundRiccardi)
                        TextBox_KeywordTelescope_FocalLength.Text = "825";
                    else
                        TextBox_KeywordTelescope_FocalLength.Text = "1100";
                }
            }

            if (!foundAPM && !foundEVO & !foundNWT)
            {
                RadioButton_KeywordTelescope_APM107.ForeColor = Color.DarkViolet;
                RadioButton_KeywordTelescope_EVO150.ForeColor = Color.DarkViolet;
                RadioButton_KeywordTelescope_NWT254.ForeColor = Color.DarkViolet;
                Label_KeywordTelescope_FocalLength.ForeColor = Color.DarkViolet;
                CheckBox_KeywordTelescope_Riccardi.Checked = false;
                CheckBox_KeywordTelescope_Riccardi.ForeColor = Color.DarkViolet;
                Button_KeywordTelescope_SetAll.ForeColor = Color.Red;
                Button_KeywordTelescope_SetByFile.ForeColor = Color.Red;
                return;
            }

            // Set SetAll button to black if only a single telescope has been found or a signle focal lenght has been found
            if ((foundAPM ^ foundEVO ^ foundNWT) && (focalCount == mFileList.Count))
            {
                // Set "SetAll" to black if only a single filter and a single frame type was found
                Button_KeywordTelescope_SetAll.ForeColor = Color.Black;
            }
            else
            {
                // More that one software program - set "SetByFile" to red
                Button_KeywordTelescope_SetAll.ForeColor = Color.Red;
            }

            if ((telescopeCount < mFileList.Count) || (riccardiCount < mFileList.Count) || (focalCount < mFileList.Count))
            {
                Button_KeywordTelescope_SetByFile.ForeColor = Color.Red;
            }
        }

        private void SetTelescopeUI(XisfFile file)
        {
            if (RadioButton_KeywordTelescope_APM107.Checked)
            {
                file.KeywordData.AddKeyword("APTDIA", 107.0, "Aperture Diameter in mm");
                file.KeywordData.AddKeyword("APTAREA", 8992.02, "Aperture area in square mm minus obstructions");

                if (CheckBox_KeywordTelescope_Riccardi.Checked)
                {
                    file.KeywordData.AddKeyword("TELESCOP", "APM107R", "APM107 Super ED with Riccardi 0.75 Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 525, "APM107 Super ED with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.KeywordData.AddKeyword("TELESCOP", "APM107", "APM107 Super ED without Reducer");
                    file.KeywordData.AddKeyword("FOCALLEN", 700, "APM107 Super ED without Reducer");
                }
            }

            if (RadioButton_KeywordTelescope_EVO150.Checked)
            {
                if (CheckBox_KeywordTelescope_Riccardi.Checked)
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

            if (RadioButton_KeywordTelescope_NWT254.Checked)
            {
                if (CheckBox_KeywordTelescope_Riccardi.Checked)
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

                file.ParseRequiredKeywords();
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
                            CheckBox_KeywordTelescope_Riccardi.Checked = true;
                        else
                            CheckBox_KeywordTelescope_Riccardi.Checked = false;

                        // Checking the radio button for the found telescope with also set focal length and Riccardi checkbox
                        RadioButton_KeywordTelescope_APM107.Checked = telescope.Contains("APM") ? true : false;
                        RadioButton_KeywordTelescope_EVO150.Checked = telescope.Contains("EVO") ? true : false;
                        RadioButton_KeywordTelescope_NWT254.Checked = telescope.Contains("NWT") ? true : false;

                        SetTelescopeUI(file);
                    }
                }

                file.ParseRequiredKeywords();
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
                        file.KeywordData.AddKeyword("FRAMETYP", frameTypeText, "XISF File Manager");
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

                file.KeywordData.AddKeyword("FRAMETYP", frameTypeText, "XISF File Manager");
                if (frameTypeText.Equals("Dark"))
                {
                    file.KeywordData.AddKeyword("FILTER", "Shutter", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");
                }


                file.ParseRequiredKeywords();
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

                file.ParseRequiredKeywords();
            }



            FindFrameType();
        }

        private void Button_KeywordImageTypeFrame_SetAll_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                if (RadioButton_KeywordImageTypeFrame_Light.Checked)
                    file.KeywordData.AddKeyword("IMAGETYP", "Light", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFrame_Dark.Checked)
                    file.KeywordData.AddKeyword("IMAGETYP", "Dark", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFrame_Flat.Checked)
                    file.KeywordData.AddKeyword("IMAGETYP", "Flat", "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFrame_Bias.Checked)
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

                if (RadioButton_KeywordImageTypeFilter_Luma.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Luma", "Astrodon Luma 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFilter_Red.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Red", "Astrodon Red 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFilter_Green.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Green", "Astrodon Green 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFilter_Blue.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Blue", "Astrodon Blue 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFilter_Ha.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Ha", "Astrodon Ha E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFilter_O3.Checked)
                    file.KeywordData.AddKeyword("FILTER", "O3", "Astrodon O3 E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFilter_S2.Checked)
                    file.KeywordData.AddKeyword("FILTER", "S2", "Astrodon S2 E-Series 1.25 via Starlight Xpress USB 7 Position Wheel");

                if (RadioButton_KeywordImageTypeFilter_Shutter.Checked)
                    file.KeywordData.AddKeyword("FILTER", "Shutter", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");

                file.ParseRequiredKeywords();
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

            RadioButton_KeywordImageTypeFilter_Luma.ForeColor = Color.Black;
            RadioButton_KeywordImageTypeFilter_Red.ForeColor = Color.Black;
            RadioButton_KeywordImageTypeFilter_Green.ForeColor = Color.Black;
            RadioButton_KeywordImageTypeFilter_Blue.ForeColor = Color.Black;
            RadioButton_KeywordImageTypeFilter_Ha.ForeColor = Color.Black;
            RadioButton_KeywordImageTypeFilter_O3.ForeColor = Color.Black;
            RadioButton_KeywordImageTypeFilter_S2.ForeColor = Color.Black;
            RadioButton_KeywordImageTypeFilter_Shutter.ForeColor = Color.Black;

            RadioButton_KeywordImageTypeFilter_Luma.Checked = false;
            RadioButton_KeywordImageTypeFilter_Red.Checked = false;
            RadioButton_KeywordImageTypeFilter_Green.Checked = false;
            RadioButton_KeywordImageTypeFilter_Blue.Checked = false;
            RadioButton_KeywordImageTypeFilter_Ha.Checked = false;
            RadioButton_KeywordImageTypeFilter_O3.Checked = false;
            RadioButton_KeywordImageTypeFilter_S2.Checked = false;
            RadioButton_KeywordImageTypeFilter_Shutter.Checked = false;

            CheckBox_FileSelection_DirectorySelection_Master.ForeColor = Color.Black;
            CheckBox_FileSelection_DirectorySelection_Master.Checked = false;

            Button_KeywordImageTypeFrame_SetMaster.ForeColor = Color.Black;
            Button_KeywordImageType_SetAll.ForeColor = Color.Black;
            Button_KeywordImageType_SetByFile.ForeColor = Color.Black;


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
                RadioButton_KeywordImageTypeFilter_Luma.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFilter_Red.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFilter_Green.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFilter_Blue.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFilter_Ha.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFilter_O3.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFilter_S2.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFilter_Shutter.ForeColor = Color.DarkViolet;
            }

            if (foundLuma)
            {
                if (foundRed || foundGreen || foundBlue || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordImageTypeFilter_Luma.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_Luma.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_Luma.Checked = true;
                }
            }

            if (foundRed)
            {
                if (foundLuma || foundGreen || foundBlue || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordImageTypeFilter_Red.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_Red.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_Red.Checked = true;
                }
            }

            if (foundGreen)
            {
                if (foundLuma || foundRed || foundBlue || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordImageTypeFilter_Green.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_Green.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_Green.Checked = true;
                }
            }

            if (foundBlue)
            {
                if (foundLuma || foundRed || foundGreen || foundHa || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordImageTypeFilter_Blue.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_Blue.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_Blue.Checked = true;
                }
            }

            if (foundHa)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundO3 || foundS2 || foundShutter)
                {
                    RadioButton_KeywordImageTypeFilter_Ha.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_Ha.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_Ha.Checked = true;
                }
            }

            if (foundO3)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundHa || foundS2 || foundShutter)
                {
                    RadioButton_KeywordImageTypeFilter_O3.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_O3.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_O3.Checked = true;
                }
            }

            if (foundS2)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundHa || foundO3 || foundShutter)
                {
                    RadioButton_KeywordImageTypeFilter_S2.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_S2.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_S2.Checked = true;
                }
            }

            if (foundShutter)
            {
                if (foundLuma || foundRed || foundGreen || foundBlue || foundHa || foundO3 || foundS2)
                {
                    RadioButton_KeywordImageTypeFilter_Shutter.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFilter_Shutter.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFilter_Shutter.Checked = true;
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
                string frameType;

                frameType = file.FrameType;

                if (frameType.Contains("Light"))
                {
                    foundLight = true;
                    frameTypeCount++;
                }

                if (frameType.Contains("Dark"))
                {
                    foundDark = true;
                    frameTypeCount++;
                }

                if (frameType.Contains("Flat"))
                {
                    foundFlat = true;
                    frameTypeCount++;
                }

                if (frameType.Contains("Bias"))
                {
                    foundBias = true;
                    frameTypeCount++;
                }

                if (file.KeywordData.TargetName().Contains("Master"))
                {
                    masterCount++;
                    foundMaster = true;
                }
            }

            if (foundLight)
            {
                if (foundDark || foundFlat || foundBias)
                {
                    RadioButton_KeywordImageTypeFrame_Light.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFrame_Light.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFrame_Light.Checked = true;
                }
            }

            if (foundDark)
            {
                if (foundLight || foundFlat || foundBias)
                {
                    RadioButton_KeywordImageTypeFrame_Dark.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFrame_Dark.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFrame_Dark.Checked = true;
                }
            }

            if (foundFlat)
            {
                if (foundLight || foundDark || foundBias)
                {
                    RadioButton_KeywordImageTypeFrame_Flat.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFrame_Flat.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFrame_Flat.Checked = true;
                }
            }

            if (foundBias)
            {
                if (foundLight || foundDark || foundFlat)
                {
                    RadioButton_KeywordImageTypeFrame_Bias.ForeColor = Color.Red;
                    RadioButton_KeywordImageTypeFrame_Bias.Checked = false;
                }
                else
                {
                    RadioButton_KeywordImageTypeFrame_Bias.Checked = true;
                }
            }

            if (!foundLight && !foundDark && !foundFlat && !foundBias)
            {
                RadioButton_KeywordImageTypeFrame_Light.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFrame_Dark.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFrame_Flat.ForeColor = Color.DarkViolet;
                RadioButton_KeywordImageTypeFrame_Bias.ForeColor = Color.DarkViolet;

                return;
            }

            if (foundMaster)
            {
                if ((masterCount != mFileList.Count) && (masterCount > 0))
                {
                    CheckBox_FileSelection_DirectorySelection_Master.ForeColor = Color.Red;
                    Button_KeywordImageTypeFrame_SetMaster.ForeColor = Color.Red;
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
                Button_KeywordImageType_SetAll.ForeColor = Color.Black;
            }
            else
            {
                // More that one software program - set "SetByFile" to red
                Button_KeywordImageType_SetAll.ForeColor = Color.Red;
            }

            if ((masterCount != mFileList.Count) && (masterCount != 0))
            {
                CheckBox_FileSelection_DirectorySelection_Master.ForeColor = Color.Red;
                Button_KeywordImageType_SetByFile.ForeColor = Color.Red;
            }

            if ((filterCount != mFileList.Count) || (frameTypeCount != mFileList.Count))
            {
                // The number of source files didn't equal the number of files with a known filter
                // Set "SetByFile" to red
                Button_KeywordImageType_SetByFile.ForeColor = Color.Red;
            }
        }

        public void FindCamera()
        {
            Label_KeywordCamera_Camera.ForeColor = Color.Black;

            RadioButton_KeywordCamera_Z533.Checked = false;
            RadioButton_KeywordCamera_Z533.ForeColor = Color.Black;

            RadioButton_KeywordCamera_Z183.Checked = false;
            RadioButton_KeywordCamera_Z183.ForeColor = Color.Black;

            RadioButton_KeywordCamera_Q178.Checked = false;
            RadioButton_KeywordCamera_Q178.ForeColor = Color.Black;

            RadioButton_KeywordCamera_A144.Checked = false;
            RadioButton_KeywordCamera_A144.ForeColor = Color.Black;

            Label_KeywordCamera_SensorTemperature.ForeColor = Color.Black;
            Label_KeywordCamera_Gain.ForeColor = Color.Black;
            Label_KeywordCamera_Offset.ForeColor = Color.Black;
            Label_KeywordCamera_Binning.ForeColor = Color.Black;
            Label_KeywordCamera_Seconds.ForeColor = Color.Black;

            Button_KeywordCamera_SetAll.ForeColor = Color.Black;
            Button_KeywordCamera_SetByFile.ForeColor = Color.Black;


            if (mFileList.Count == 0) return;

            bool foundZ533 = mFileList.Where(i => i.Camera.Equals("Z533")).Count() > 0;
            bool foundZ183 = mFileList.Where(i => i.Camera.Equals("Z183")).Count() > 0;
            bool foundQ178 = mFileList.Where(i => i.Camera.Equals("Q178")).Count() > 0;
            bool foundA144 = mFileList.Where(i => i.Camera.Equals("A144")).Count() > 0;

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
                }
            }

            if (!foundA144 && !foundQ178 && !foundZ183 && !foundZ533)
            {
                RadioButton_KeywordCamera_A144.ForeColor = Color.DarkViolet;
                RadioButton_KeywordCamera_Q178.ForeColor = Color.DarkViolet;
                RadioButton_KeywordCamera_Z183.ForeColor = Color.DarkViolet;
                RadioButton_KeywordCamera_Z533.ForeColor = Color.DarkViolet;
                Label_KeywordCamera_Gain.ForeColor = Color.DarkViolet;
                Label_KeywordCamera_Offset.ForeColor = Color.DarkViolet;
                Label_KeywordCamera_SensorTemperature.ForeColor = Color.DarkViolet;
                Label_KeywordCamera_Binning.ForeColor = Color.DarkViolet;
                Label_KeywordCamera_Seconds.ForeColor = Color.DarkViolet;

                Button_KeywordCamera_SetAll.ForeColor = Color.Red;
                Button_KeywordCamera_SetByFile.ForeColor = Color.Red;

                //return;
            }

            // ****************************************************************

            bool missingGain = mFileList.Exists(i => i.Gain == -1);
            bool uniqueGain = mFileList.Select(i => i.Gain).Distinct().Count() == 1;

            if (missingGain)
            {
                Label_KeywordCamera_Gain.ForeColor = Color.Red;
            }
            else
            {
                if (!uniqueGain)
                {
                    Label_KeywordCamera_Gain.ForeColor = Color.Red;
                }
            }

            if (missingGain && !uniqueGain)
            {
                Button_KeywordCamera_SetAll.ForeColor = Color.Red;
                Button_KeywordCamera_SetByFile.ForeColor = Color.Red;
            }

            foreach (XisfFile file in mFileList)
            {
                if (file.Camera.Equals("Z533"))
                    TextBox_KeywordCamera_Z533Gain.Text = file.Gain.ToString();
                if (file.Camera.Equals("Z183"))
                    TextBox_KeywordCamera_Z183Gain.Text = file.Gain.ToString();
                if (file.Camera.Equals("Z533"))
                    TextBox_KeywordCamera_Q178Gain.Text = file.Gain.ToString();
            }

            // ****************************************************************

            bool hasOffset = mFileList.Exists(i => i.Offset != -1);
            bool missingOffset = mFileList.Exists(i => i.Offset == -1);
            bool uniqueOffset = mFileList.Select(i => i.Offset).Distinct().Count() == 1;

            if (missingOffset)
            {
                Label_KeywordCamera_Offset.ForeColor = Color.Red;
            }
            else
            {
                if (!uniqueOffset)
                {
                    Label_KeywordCamera_Offset.ForeColor = Color.Red;
                }
            }

            if ((missingOffset && !uniqueOffset) || !hasOffset)
            {
                Button_KeywordCamera_SetAll.ForeColor = Color.Red;
                Button_KeywordCamera_SetByFile.ForeColor = Color.Red;
            }

            foreach (XisfFile file in mFileList)
            {
                if (file.Camera.Equals("Z533"))
                    TextBox_KeywordCamera_Z533Offset.Text = file.Offset.ToString();
                if (file.Camera.Equals("Z183"))
                    TextBox_KeywordCamera_Z183Offset.Text = file.Offset.ToString();
                if (file.Camera.Equals("Z533"))
                    TextBox_KeywordCamera_Q178Offset.Text = file.Offset.ToString();
            }

            // ****************************************************************

            bool hasTemperature = mFileList.Exists(i => i.Temperature != string.Empty);
            bool missingTemperature = mFileList.Exists(i => i.Temperature == string.Empty);
            bool uniqueTemperature = mFileList.Select(i => i.Temperature).Distinct().Count() == 1;

            if (!hasTemperature)
            {
                // No temperatures for any file
                Label_KeywordCamera_SensorTemperature.ForeColor = Color.Red;
                TextBox_KeywordCamera_SensorTemperature.Text = string.Empty;
            }
            else
            {
                if (missingTemperature)
                {
                    // At least one, but not all, files have a missing temperature
                    Label_KeywordCamera_SensorTemperature.ForeColor = Color.DarkViolet;

                    if (uniqueTemperature)
                    {
                        // Of the files that do have a temperature, this is the unique value
                        TextBox_KeywordCamera_SensorTemperature.Text = Convert.ToDouble(mFileList[0].Temperature).ToString("F1");
                    }
                    else
                    {
                        // At least two files have different temperatures
                        TextBox_KeywordCamera_SensorTemperature.Text = string.Empty;
                    }
                }
                else
                {
                    if (uniqueTemperature)
                    {
                        // All files contain the same temerature
                        TextBox_KeywordCamera_SensorTemperature.Text = Convert.ToDouble(mFileList[0].Temperature).ToString("F1");
                    }
                    else
                    {
                        // All files contain temperatures but at least two files contain different temperatures
                        TextBox_KeywordCamera_SensorTemperature.Text = string.Empty;
                    }
                }
            }

            // Now set button colors
            if ((missingTemperature && !uniqueTemperature) || !hasTemperature)
            {
                Button_KeywordCamera_SetAll.ForeColor = Color.Red;
                Button_KeywordCamera_SetByFile.ForeColor = Color.Red;
            }

            // ***************************************************************

            bool missingBinning = mFileList.Exists(i => i.Binning == -1);
            bool uniqueBinning = mFileList.Select(i => i.Binning).Distinct().Count() == 1;

            if (missingBinning)
            {
                Label_KeywordCamera_Binning.ForeColor = Color.Red;
            }
            else
            {
                if (!uniqueBinning)
                {
                    Label_KeywordCamera_Binning.ForeColor = Color.Red;
                }
                else
                {
                    NumericUpDown_KeywordCamera_Binning.Value = mFileList[0].Binning;
                }
            }

            if (missingBinning && !uniqueBinning)
            {
                Button_KeywordCamera_SetAll.ForeColor = Color.Red;
                Button_KeywordCamera_SetByFile.ForeColor = Color.Red;
            }

            // ****************************************************************

            bool hasExposure = mFileList.Exists(i => i.Exposure != string.Empty);
            bool missingExposure = mFileList.Exists(i => i.Exposure == string.Empty);
            bool uniqueExposure = mFileList.Select(i => i.Exposure).Distinct().Count() == 1;

            if (!hasExposure)
            {
                // No exposure for any file
                Label_KeywordCamera_Seconds.ForeColor = Color.Red;
                TextBox_KeywordCamera_Seconds.Text = string.Empty;
            }
            else
            {
                bool status;
                double value;
                status = Double.TryParse(mFileList[0].Exposure, out value);

                if (missingExposure)
                {
                    // At least one, but not all, files have a missing exposure
                    Label_KeywordCamera_Seconds.ForeColor = Color.DarkViolet;

                    if (uniqueExposure)
                    {
                        // Of the files that do have an exposure, this is the unique value
                        if (status)
                        {
                            if (value < 10)
                                TextBox_KeywordCamera_Seconds.Text = value.ToString("F3");
                            else
                                TextBox_KeywordCamera_Seconds.Text = value.ToString("F1");
                        }
                        else
                        {
                            // Not a number
                            TextBox_KeywordCamera_Seconds.Text = string.Empty;
                        }
                    }
                    else
                    {
                        // At least two files have different exposures
                        TextBox_KeywordCamera_Seconds.Text = string.Empty;
                    }
                }
                else
                {
                    if (uniqueExposure)
                    {
                        // All files contain the same exposure
                        if (value < 10)
                            TextBox_KeywordCamera_Seconds.Text = value.ToString("F3");
                        else
                            TextBox_KeywordCamera_Seconds.Text = value.ToString("F1");
                    }
                    else
                    {
                        // All files contain exposures but at least two files contain different exposures
                        TextBox_KeywordCamera_Seconds.Text = string.Empty;
                    }
                }
            }


            if ((missingExposure && !uniqueExposure) || !hasExposure)
            {
                Button_KeywordCamera_SetAll.ForeColor = Color.Red;
                Button_KeywordCamera_SetByFile.ForeColor = Color.Red;
            }

            // ****************************************************************
        }

        private void CheckBox_CameraNarrowBand_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_KeywordCamera_NarrowBand.Checked)
            {
                TextBox_KeywordCamera_Z533Gain.Text = "100";
                TextBox_KeywordCamera_Z533Offset.Text = "50";

                TextBox_KeywordCamera_Z183Gain.Text = "111";
                TextBox_KeywordCamera_Z183Offset.Text = "10";

                TextBox_KeywordCamera_Q178Gain.Text = "40";
                TextBox_KeywordCamera_Q178Offset.Text = "15";
            }
            else
            {
                TextBox_KeywordCamera_Z533Gain.Text = "100";
                TextBox_KeywordCamera_Z533Offset.Text = "50";

                TextBox_KeywordCamera_Z183Gain.Text = "53";
                TextBox_KeywordCamera_Z183Offset.Text = "10";

                TextBox_KeywordCamera_Q178Gain.Text = "40";
                TextBox_KeywordCamera_Q178Offset.Text = "15";
            }
        }

        private void Button_KeywordCamera_SetAll_Click(object sender, EventArgs e)
        {
            double value;

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

                bool status = double.TryParse(TextBox_KeywordCamera_SensorTemperature.Text, out value);
                if (status)
                {
                    file.KeywordData.AddKeyword("CCD-TEMP", value, "Actual Sensor Temperature");
                }

                file.KeywordData.AddKeyword("NAXIS", 2, "XISF File Manager");
                file.KeywordData.AddKeyword("XBINNING", NumericUpDown_KeywordCamera_Binning.Value.ToString(), "Horizontal Binning");
                file.KeywordData.AddKeyword("YBINNING", NumericUpDown_KeywordCamera_Binning.Value.ToString(), "Vertical Bining");

                status = double.TryParse(TextBox_KeywordCamera_Seconds.Text, out value);
                if (status)
                {
                    file.KeywordData.AddKeyword("EXPTIME", value, "Exposure Time in Seconds");
                }

                if (RadioButton_KeywordCamera_Z533.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    file.KeywordData.AddKeyword("NAXIS1", 3008, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3008, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 3.76, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 3.76, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");
                    file.KeywordData.AddKeyword("GAIN", Int32.Parse(TextBox_KeywordCamera_Z533Gain.Text), "Camera Gain");
                    file.KeywordData.AddKeyword("OFFSET", Int32.Parse(TextBox_KeywordCamera_Z533Offset.Text), "Camera Offset");
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordCamera_Z183.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    file.KeywordData.AddKeyword("NAXIS1", 5496, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3672, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");
                    file.KeywordData.AddKeyword("GAIN", Int32.Parse(TextBox_KeywordCamera_Z183Gain.Text), "Camera Gain");
                    file.KeywordData.AddKeyword("OFFSET", Int32.Parse(TextBox_KeywordCamera_Z183Offset.Text), "Camera Offset");
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordCamera_Q178.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    file.KeywordData.AddKeyword("NAXIS1", 3072, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 2048, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");
                    file.KeywordData.AddKeyword("GAIN", Int32.Parse(TextBox_KeywordCamera_Q178Gain.Text), "Camera Gain");
                    file.KeywordData.AddKeyword("OFFSET", Int32.Parse(TextBox_KeywordCamera_Q178Offset.Text), "Camera Offset");
                    file.KeywordData.SetEGain();
                }

                if (RadioButton_KeywordCamera_A144.Checked)
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

                file.ParseRequiredKeywords();
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
                string temperatureTextUI = TextBox_KeywordCamera_SensorTemperature.Text;

                string temperatureText;
                if (globalTemperature)
                {
                    temperatureText = file.KeywordData.SensorTemperature();
                    if (temperatureText == string.Empty)
                    {
                        temperatureText = globalTemperatureText;
                    }
                }
                else
                {
                    if (temperatureTextUI == string.Empty)
                    {
                        globalTemperatureText = file.KeywordData.SensorTemperature(true);
                        if (globalTemperatureText.Contains("Global_"))
                        {
                            globalTemperature = true;
                            globalTemperatureText = globalTemperatureText.Replace("Global_", "");
                        }

                        temperatureText = globalTemperatureText;
                    }
                    else
                    {
                        temperatureText = file.KeywordData.SensorTemperature();
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
                file.KeywordData.AddKeyword("XBINNING", NumericUpDown_KeywordCamera_Binning.Value.ToString(), "Horizontal Binning");
                file.KeywordData.AddKeyword("YBINNING", NumericUpDown_KeywordCamera_Binning.Value.ToString(), "Vertical Bining");
                string secondsTextUI = TextBox_KeywordCamera_Seconds.Text;

                string secondsText;
                if (globalSeconds)
                {
                    secondsText = file.KeywordData.ExposureSeconds();
                    if (secondsText == string.Empty)
                    {
                        secondsText = globalSecondsText;
                    }
                }
                else
                {
                    if (secondsTextUI == string.Empty)
                    {
                        globalSecondsText = file.KeywordData.ExposureSeconds(true);
                        if (globalSecondsText.Contains("Global_"))
                        {
                            globalSeconds = true;
                            globalSecondsText = globalSecondsText.Replace("Global_", "");
                        }

                        secondsText = globalSecondsText;
                    }
                    else
                    {
                        secondsText = file.KeywordData.ExposureSeconds();
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
                if (RadioButton_KeywordCamera_Z533.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    file.KeywordData.AddKeyword("NAXIS1", 3008, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3008, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 3.76, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 3.76, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("BAYERPAT", "RGGB");

                    status = int.TryParse(TextBox_KeywordCamera_Z533Gain.Text, out gainValueUI);
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


                    status = int.TryParse(TextBox_KeywordCamera_Z533Offset.Text, out offsetValueUI);
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

                if (RadioButton_KeywordCamera_Z183.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    file.KeywordData.AddKeyword("NAXIS1", 5496, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 3672, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(TextBox_KeywordCamera_Z183Gain.Text, out gainValueUI);
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


                    status = int.TryParse(TextBox_KeywordCamera_Z183Offset.Text, out offsetValueUI);
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




                if (RadioButton_KeywordCamera_Q178.Checked)
                {
                    file.KeywordData.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    file.KeywordData.AddKeyword("NAXIS1", 3072, "Horizontal Pixel Width");
                    file.KeywordData.AddKeyword("NAXIS2", 2048, "Vertical Pixel Height");
                    file.KeywordData.AddKeyword("XPIXSZ", 2.4, "Horizonal Pixel Size in Microns");
                    file.KeywordData.AddKeyword("YPIXSZ", 2.4, "Vertical Pixel Size in Microns");
                    file.KeywordData.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(TextBox_KeywordCamera_Q178Gain.Text, out gainValueUI);
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


                    status = int.TryParse(TextBox_KeywordCamera_Q178Offset.Text, out offsetValueUI);
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

                if (RadioButton_KeywordCamera_A144.Checked)
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

                file.ParseRequiredKeywords();
            }

            FindCamera();
        }

        private void RadioButton_KeywordCamera_Z533_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_KeywordCamera_SensorTemperature.Text = "-10";

            if (CheckBox_KeywordCamera_NarrowBand.Checked)
            {
                TextBox_KeywordCamera_Z533Gain.Text = "100";
                TextBox_KeywordCamera_Z533Offset.Text = "50";
            }
            else
            {
                TextBox_KeywordCamera_Z533Gain.Text = "100";
                TextBox_KeywordCamera_Z533Offset.Text = "50";
            }
        }

        private void RadioButton_KeywordCamera_Z183_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_KeywordCamera_SensorTemperature.Text = "-20";

            if (CheckBox_KeywordCamera_NarrowBand.Checked)
            {
                TextBox_KeywordCamera_Z183Gain.Text = "111";
                TextBox_KeywordCamera_Z183Offset.Text = "10";
            }
            else
            {
                TextBox_KeywordCamera_Z183Gain.Text = "53";
                TextBox_KeywordCamera_Z183Offset.Text = "10";
            }
        }

        private void RadioButton_KeywordCamera_Q178_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_KeywordCamera_SensorTemperature.Text = "";

            if (CheckBox_KeywordCamera_NarrowBand.Checked)
            {
                TextBox_KeywordCamera_Q178Gain.Text = "40";
                TextBox_KeywordCamera_Q178Offset.Text = "15";
            }
            else
            {
                TextBox_KeywordCamera_Q178Gain.Text = "40";
                TextBox_KeywordCamera_Q178Offset.Text = "15";
            }
        }

        private void RadioButton_KeywordCamera_A144_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_KeywordCamera_SensorTemperature.Text = "";
        }


        private void Button_KeywordImageTypeFrame_SetMaster_Click(object sender, EventArgs e)
        {
            ComboBox_KeywordUpdate_SubFrameKeywords_TargetNames.Text = "Master";
            CheckBox_KeywordUpdate_SubFrameKeywords_UpdateTargetName.Checked = true;
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

            ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();

            // Repopulate the list of any present weight keywords (not values). Find unique Keyords, sort and populate Weight combobox
            foreach (XisfFile file in mFileList)
            {
                WeightKeywords.Add(file.KeywordData.WeightKeyword());
            }

            if (WeightKeywords.Count > 0)
            {
                WeightKeywords = WeightKeywords.Distinct().ToList();
                WeightKeywords = WeightKeywords.OrderBy(q => q).ToList();

                foreach (string item in WeightKeywords)
                {
                    ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items.Add(item);
                }

                if (WeightKeywords.Count > 1)
                {
                    Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Black;
                }
            }
            else
            {
                ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();
                Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Black;
                return;
            }

            // Remove ALL WEIGHT items
            if (RadioButton_KeywordUpdate_SubFrameKeywords_Weights_All.Checked)
            {
                foreach (string item in ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items)
                {
                    foreach (XisfFile file in mFileList)
                    {
                        file.KeywordData.RemoveKeyword(item);
                    }
                }

                Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Black;
                ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items.Clear();
                ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Text = "";
                return;
            }

            // Only Remove selected item
            if (RadioButton_KeywordUpdate_SubFrameKeywords_Weights_Selected.Checked)
            {
                foreach (XisfFile file in mFileList)
                {
                    file.KeywordData.RemoveKeyword(ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Text);
                }

                WeightKeywords.Remove(ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Text);
                ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Items.Remove(ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Text);
                ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.Text = "";

                if (WeightKeywords.Count > 1)
                {
                    Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Red;
                }
                else
                {
                    Label_KeywordUpdate_SubFrameKeyword_Weights_WeightKeyword.ForeColor = Color.Black;
                }

                if (WeightKeywords.Count > 0)
                {
                    ComboBox_KeywordUpdate_SubFrameKeywords_Weights_WeightKeywords.SelectedIndex = 0;
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

        private void Calibration_FindCalibrationFrames_Click(object sender, EventArgs e)
        {
            mCalibration.Frame = DirectoryOps.FrameType.ALL;
            mCalibration.FindCalibrationFrames(mFileList);

            int count = mCalibration.CalibrationFiles.Count;
        }

        private void Calibration_MatchCalibrationFrames_Click(object sender, EventArgs e)
        {
            mCalibration.MatchCalibrationFrames(mFileList);
        }

        private void Calibration_CreateCalibrationDirectory_Click(object sender, EventArgs e)
        {
            mCalibration.CreateTargetCalibrationDirectory(mFileList);
        }
    }
}
