using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XisfFileManager.Files;
using System.Drawing;
using XisfFileManager.Calculations;
using System.Reflection;
using XisfFileManager.Enums;
using TreeView = System.Windows.Forms.TreeView;
using XisfFileManager.DirectoryOperations;

namespace XisfFileManager
{
    public delegate void DataReceivedEvent(CalibrationTabPageValues data);

    // ##########################################################################################################################
    // ##########################################################################################################################
    public partial class MainForm : Form
    {
        private List<XisfFile> mFileList;
        private XisfFile mFile;
        private Calibration mCalibration;
        private readonly ImageCalculations ImageParameterLists;
        private readonly XisfFileReader mFileReader;
        private readonly XisfFileRename mRenameFile;
        private string mFolderBrowseState;
        private XisfFileManager.TargetScheduler.SqlLiteManager mSchedulerDB;
        private bool mBCancel;
        private XisfFileUpdate mXisfFileUpdate;
        private eKeywordUpdateMode mKeywordUpdateProtection;
        private DirectoryProperties mDirectoryProperties;


        public MainForm()
        {
            InitializeComponent();
            CalibrationTabPageEvent.CalibrationTabPage_InvokeEvent += EventHandler_UpdateCalibrationPageForm;
            TreeView_SchedulerTab_ProfileTree.NodeMouseClick += TreeView_SchedulerTab_ProfileTree_NodeMouseClick;
            TreeView_SchedulerTab_ProjectTree.NodeMouseClick += TreeView_SchedulerTab_ProjectTree_NodeMouseClick;
            TreeView_SchedulerTab_TargetTree.NodeMouseClick += TreeView_SchedulerTab_TargetTree_NodeMouseClick_NodeMouseClick;
            TreeView_SchedulerTab_PlansTree.NodeMouseClick += TreeView_SchedulerTab_PlanTree_NodeMouseClick_NodeMouseClick;
            mDirectoryProperties = new DirectoryProperties();
            mCalibration = new Calibration();
            mFileReader = new XisfFileReader();
            mSchedulerDB = new XisfFileManager.TargetScheduler.SqlLiteManager();
            mXisfFileUpdate = new XisfFileUpdate();
            mKeywordUpdateProtection = eKeywordUpdateMode.UPDATE_NEW;
            Label_FileSelection_Statistics_Task.Text = "";
            mFileList = new List<XisfFile>();
            mRenameFile = new XisfFileRename
            {
                RenameOrder = eOrder.INDEX
            };

            // This set of a much smaller number of numeric lists contains per image data used for Focuser Temperature compensation coefficient calculation and SSWEIGHTs
            ImageParameterLists = new ImageCalculations();

            Label_FileSelection_Statistics_Task.Text = "No Images Selected";
            Label_FileSelection_Statistics_TempratureCompensation.Text = "Temperature Coefficient: Not Computed";

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = $"XISF File Manager - " + System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString("yyyy.MM.dd - h:mm tt");


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
                case eMessageMode.CLEAR:
                    TextBox_CalibrationTab_Messgaes.Clear();
                    break;

                case eMessageMode.APPEND:
                    TextBox_CalibrationTab_Messgaes.AppendText(data.MatchCalibrationMessage);
                    break;

                case eMessageMode.NEW:
                    TextBox_CalibrationTab_Messgaes.Clear();
                    TextBox_CalibrationTab_Messgaes.AppendText(data.MatchCalibrationMessage);
                    break;

                default:
                    break;

            }
            data.MessageMode = eMessageMode.KEEP;

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

            mFolderBrowseState = Properties.Settings.Default.Persist_FolderBrowseState;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Checked = Properties.Settings.Default.Persist_UpdateTargetNameState;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Checked = Properties.Settings.Default.Persist_UpdatePanelNameState;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            Properties.Settings.Default.Persist_FolderBrowseState = mFolderBrowseState;
            Properties.Settings.Default.Persist_UpdateTargetNameState = CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Checked;
            Properties.Settings.Default.Persist_UpdatePanelNameState = CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Checked;

            Properties.Settings.Default.Save();
        }

        // ##########################################################################################################################
        // ##########################################################################################################################
        private async void Button_Browse_Click(object sender, EventArgs e)
        {
            // Clear all lists - we are reading or re-reading what will become a new xisf file data set that will invalidate any existing data.         
            mBCancel = false;
            mFileList.Clear();
            ImageParameterLists.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text = "Keyword";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text = "Value";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text = "";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Items.Clear();
            TextBox_CalibrationTab_Messgaes.Clear();
            TreeView_CalibrationTab_TargetFileTree.Nodes.Clear();

            mCalibration.ResetAll();

            ClearCameraForm();

            ProgressBar_FileSelection_ReadProgress.Value = 0;
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;
            TabControl_Update_TargetScheduler.Enabled = false;

            List<string> mXisfExclude = new List<string>()
                {
                    "Calibration",
                    "PreProcessing",
                    "Duplicates",
                    "Registered",
                    "Calibrated",
                    "Project"
                };

            DialogResult result = Files.DirectoryOperations.FindTargetfFiles(mFolderBrowseState, mXisfExclude);

            if ((result != DialogResult.OK) || (Files.DirectoryOperations.FileInfoList.Count == 0))
            {
                MessageBox.Show("No Xisf Files Found", "Select a different .xisf Folder");
                return;
            }

            DirectoryInfo diDirectoryTree = new DirectoryInfo(Files.DirectoryOperations.SelectedFolder);

            Label_FileSelection_Statistics_Task.Text = "Reading " + Files.DirectoryOperations.FileInfoList.Count.ToString() + " Image Files";
            Label_FileSelection_Statistics_TempratureCompensation.Text = "Temperature Coefficient: Not Computed";
            Label_FileSelection_Statistics_SubFrameOverhead.Text = "SubFrame Overhead: Not Computed";

            ProgressBar_FileSelection_ReadProgress.Value = 0;
            ProgressBar_FileSelection_ReadProgress.Maximum = Files.DirectoryOperations.FileInfoList.Count;


            // Upate the UI with data from the .xisf recursive directory search
            ProgressBar_FileSelection_ReadProgress.Maximum = Files.DirectoryOperations.FileInfoList.Count;
            System.Windows.Forms.Application.DoEvents();

            foreach (var xFile in Files.DirectoryOperations.FileInfoList)
            {
                Label_FileSelection_BrowseFileName.Text = xFile.DirectoryName + "\n" + xFile.Name;
                ProgressBar_FileSelection_ReadProgress.Value += 1;

                // Create a new xisf file instance
                mFile = new XisfFile
                {
                    FilePath = xFile.FullName
                };

                await mFileReader.ReadXisfFileHeaderKeywords(mFile);

                mFileList.Add(mFile);
            }

            mFileList.Sort((a, b) => a.CaptureTime.CompareTo(b.CaptureTime)); // oldest is first

            // **********************************************************************
            // Get TargetName and and Weights to populate ComboBoxes

            // First get a list of all the target names found in the source files, then find unique names and sort.
            // Place culled list in the target name combobox
            List<string> targetNameList = new();
            List<string> weightKeywordList = new();

            foreach (XisfFile file in mFileList)
            {
                targetNameList.Add(file.TargetName);
            }

            targetNameList = targetNameList.Distinct().ToList();
            targetNameList = targetNameList.OrderBy(q => q).ToList();

            // Add the target names to the combobox
            foreach (string item in targetNameList)
            {
                ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Items.Add(item);
            }

            // Select the first item in the combobox
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.SelectedIndex = 0;


            if (targetNameList.Count <= 1)
            {
                // Single name or blank
                Label_KeywordUpdateTab_SubFrameKeywords_TagetName.ForeColor = Color.Black;
            }
            else
            {
                // If target names are not unique, check for pairs
                var matchCounts = new Dictionary<string, int>();
                foreach (var item in targetNameList)
                {
                    var baseItem = item.EndsWith(" stars") ? item.Substring(0, item.Length - 6) : item;

                    if (matchCounts.ContainsKey(baseItem))
                        matchCounts[baseItem]++;
                    else
                        matchCounts[baseItem] = 1;
                }

                bool bGreen = true;

                foreach (var count in matchCounts.Values)
                {
                    if (count == 1)
                    {
                        // Rule for items without a pair
                        Label_KeywordUpdateTab_SubFrameKeywords_TagetName.ForeColor = Color.DarkViolet;
                        bGreen = false;
                        break;
                    }
                    else if (count != 2)
                    {
                        // Rule for items that do not form exact pairs
                        Label_KeywordUpdateTab_SubFrameKeywords_TagetName.ForeColor = Color.Red;
                        bGreen = false;
                        break;
                    }
                }

                if (bGreen)
                    Label_KeywordUpdateTab_SubFrameKeywords_TagetName.ForeColor = Color.Green;
            }


            // Now find a list of any present weight keywords (not values). Find unique Keyords, sort and populate Weight combobox
            foreach (XisfFile file in mFileList)
            {
                weightKeywordList = file.WeightKeyword;
            }

            if (weightKeywordList.Count > 0)
            {
                weightKeywordList = weightKeywordList.Distinct().ToList();
                weightKeywordList = weightKeywordList.OrderBy(q => q).ToList();

                foreach (var item in weightKeywordList)
                {
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Items.Add(item);
                }

                if (weightKeywordList.Count > 1)
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
            List<string> keywordNamelist = new();

            foreach (XisfFile xFile in mFileList)
            {
                ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordFile.Items.Add(Path.GetFileName(xFile.FilePath));

                foreach (var keywordName in xFile.KeywordList.mKeywordList)
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
            foreach (XisfFile xFile in mFileList)
            {
                if (xFile.FilePath == string.Empty)
                    xFile.AddKeyword("FILENAME", "Original Name", Path.GetFileName(xFile.FilePath));

                ImageParameterLists.BuildImageParameterValueLists(xFile);
            }

            if (Files.DirectoryOperations.FileInfoList.Count == mFileList.Count)
                Label_FileSelection_Statistics_Task.Text = "Read all " + mFileList.Count.ToString() + " Image Files";
            else
                Label_FileSelection_Statistics_Task.Text = "Read " + mFileList.Count.ToString() + " out of " + Files.DirectoryOperations.FileInfoList.Count + " Image Files";

            Label_FileSelection_Statistics_SubFrameOverhead.Text = ImageParameterLists.CalculateOverhead(mFileList);
            string stepsPerDegree = ImageParameterLists.CalculateFocuserTemperatureCompensationCoefficient(mFileList);
            Label_FileSelection_Statistics_TempratureCompensation.Text = "Temperature Coefficient: " + stepsPerDegree;

            // **********************************************************************

            FindCaptureSoftware();
            FindFilterFrameType();
            FindTelescope();
            FindCamera();
            FindMasterStatistics();

            // **********************************************************************

            // TreeView_CalibrationTab_Dates

            // Create the TreeView

            TreeView_CalibrationTab_TargetFileTree.Nodes.Clear();

            var groupedByTargetName = mFileList.GroupBy(item => item.TargetName).OrderBy(group => group.Key);

            // Create the hierarchical TreeView
            foreach (var targetGroup in groupedByTargetName)
            {
                TreeNode targetNode = new TreeNode(targetGroup.Key);
                TreeView_CalibrationTab_TargetFileTree.Nodes.Add(targetNode);

                // Group the items by Camera
                var groupedByCamera = targetGroup.GroupBy(item => item.Camera).OrderBy(group => group.Key);

                foreach (var cameraGroup in groupedByCamera)
                {
                    TreeNode cameraNode = new TreeNode(cameraGroup.Key);
                    targetNode.Nodes.Add(cameraNode);

                    // Group the item by ExposureSeconds
                    var groupedByExposureSeconds = cameraGroup.GroupBy(item => item.ExposureSeconds).OrderByDescending(group => group.Key);

                    foreach (var exposureGroup in groupedByExposureSeconds)
                    {
                        TreeNode exposureNode = new TreeNode(exposureGroup.Key.ToString());
                        cameraNode.Nodes.Add(exposureNode);

                        // Group the items by Filter
                        var groupedByFilter = exposureGroup.GroupBy(item => item.FilterName).OrderBy(group => group.Key);

                        foreach (var filterGroup in groupedByFilter)
                        {
                            TreeNode filterNode = new TreeNode($"{filterGroup.Key} - {filterGroup.Count()} files");
                            exposureNode.Nodes.Add(filterNode);
                        }
                    }
                }
            }

            ExpandAllNodes(TreeView_CalibrationTab_TargetFileTree.Nodes);
            TabControl_Update_TargetScheduler.Enabled = true;
        }

        public void SetDirectoryStatistics()
        {
            // Potentally rename containing Directory
            mDirectoryProperties.SetDirectoryStatistics(mFileList, CheckBox_FileSlection_NoTotals.Checked);

            // Iterate through each Target, Camera and associated Panel Directory
            foreach (var group in mDirectoryProperties.DirectoryStatistics)
            {
                string currentDirectory = group.Key;
                string newDirectory = group.Value;

                if (currentDirectory.Equals(newDirectory))
                    continue;

                if (!Directory.Exists(newDirectory))
                    Directory.Move(currentDirectory, newDirectory);
            }
        }

        public void SetFileIndex(bool bFilter, bool bTime)
        {
            if (bTime)
            {
                int index = 0;

                foreach (XisfFile xFile in mFileList)
                {
                    xFile.FileNameNumberIndex = ++index;
                }
                return;
            }

            foreach (var group in mDirectoryProperties.DirectoryStatistics)
            {
                int lumaIndex = 0;
                int redIndex = 0;
                int greenIndex = 0;
                int blueIndex = 0;
                int haIndex = 0;
                int o3Index = 0;
                int s2Index = 0;
                int shutterIndex = 0;

                // The newDirectory may or may not be the same as the original directory.
                // The group.Key will index into the DirectoryStatistics Dictionary to get the directory we are working in.
                // This allows us to rename the directory BEFORE renaming the files in the renamed directory without changing mFileList and
                // it's an attempt to avoid ndows file rename/busy problems doing this the other way around.

                string originalDirectory = group.Key;
                string newDirectory = mDirectoryProperties.DirectoryStatistics[group.Key];



                foreach (XisfFile xFile in mFileList)
                {
                    // Assuptions
                    // For a given Target, we are assuming that there are one set of captures (Filters) under a particular Camera.
                    // or a Panel in a Mosaic (the Mosaic Panel is effectively treaded as unique target.
                    // 
                    // We are also assuming that mFileList is in time sequential order independent of Filter (this is done before SetFileIndex() is called).

                    // This code will sequentially number Filter image files in each group

                    if (xFile.FilePath.Contains(originalDirectory))
                    {
                        if (xFile.FilterName.Equals("Luma"))
                            xFile.FileNameNumberIndex = ++lumaIndex;

                        if (xFile.FilterName.Equals("Red"))
                            xFile.FileNameNumberIndex = ++redIndex;

                        if (xFile.FilterName.Equals("Green"))
                            xFile.FileNameNumberIndex = ++greenIndex;

                        if (xFile.FilterName.Equals("Blue"))
                            xFile.FileNameNumberIndex = ++blueIndex;

                        if (xFile.FilterName.Equals("Ha"))
                            xFile.FileNameNumberIndex = ++haIndex;

                        if (xFile.FilterName.Equals("O3"))
                            xFile.FileNameNumberIndex = ++o3Index;

                        if (xFile.FilterName.Equals("S2"))
                            xFile.FileNameNumberIndex = ++s2Index;

                        if (xFile.FilterName.Equals("Shutter"))
                            xFile.FileNameNumberIndex = ++shutterIndex;
                    }
                }
            }
        }

        private void Button_Rename_Click(object sender, EventArgs e)
        {
            bool bFilter = RadioButton_FileSelection_Index_ByFilter.Checked;
            bool bTime = RadioButton_FileSelection_Index_ByTime.Checked;

            Label_FileSelection_Statistics_Task.Text = "Renaming " + mFileList.Count.ToString() + " Images";

            ProgressBar_KeywordUpdateTab_WriteProgress.Maximum = mFileList.Count;
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;

            int duplicates = mRenameFile.MoveDuplicates(mFileList);

            // SetFileIndex will preset the index for each file in mFileList based on the bools for Target, Night (by existing subdirectory (typically yyyy-mm-dd)), Filter and Time (Date and Time)
            // Filters with different exposure times are not considered to be unique meaning a 600 second Blue filter uses the same index list as 60 second Blue filter
            // An exception to this is if the containing directory includes the word "Stars". Files in "Stars" directories have unique Filter indexes that are independent of exposure time. 
            // Any found Duplicates are handled inside the RenameFile method

            SetDirectoryStatistics();

            SetFileIndex(bFilter, bTime);

            foreach (XisfFile xFile in mFileList)
            {
                if (mBCancel) { mBCancel = false; return; }

                ProgressBar_KeywordUpdateTab_WriteProgress.Value += 1;

                string key = Path.GetDirectoryName(xFile.FilePath);
                xFile.FilePath = mDirectoryProperties.DirectoryStatistics[key] + "\\" + Path.GetFileName(xFile.FilePath);

                Label_FileSelection_BrowseFileName.Text = Path.GetDirectoryName(xFile.FilePath) + "\n" + Path.GetFileName(xFile.FilePath);

                Tuple<int, string> renameTuple = mRenameFile.RenameFile(xFile);

                Label_KeywordUpdateTab_FileName.Text = Path.GetDirectoryName(renameTuple.Item2) + "\n" + Path.GetFileName(renameTuple.Item2);

                System.Windows.Forms.Application.DoEvents(); // Update UI
            }

            ProgressBar_KeywordUpdateTab_WriteProgress.Value = ProgressBar_KeywordUpdateTab_WriteProgress.Maximum;

            if (duplicates == 1)
                Label_FileSelection_Statistics_Task.Text = (mFileList.Count).ToString() + " Images Renamed\n" + duplicates.ToString() + " Duplicate";
            else
                Label_FileSelection_Statistics_Task.Text = (mFileList.Count).ToString() + " Images Renamed\n" + duplicates.ToString() + " Duplicates";

            mDirectoryProperties.DirectoryStatistics.Clear();
            mFileList.Clear();

            ProgressBar_FileSelection_ReadProgress.Value = 0;
        }

        private void Button_KeywordSubFrame_UpdateXisfFiles_Click(object sender, EventArgs e)
        {
            if (RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked)
                return;

            bool bStatus;
            GroupBox_FileSelection.Enabled = false;
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Enabled = false;
            GroupBox_KeywordUpdateTab_CaptureSoftware.Enabled = false;
            GroupBox_KeywordUpdateTab_Telescope.Enabled = false;
            GroupBox_KeywordUpdateTab_Camera.Enabled = false;
            GroupBox_KeywordUpdateTab_ImageType.Enabled = false;
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;
            ProgressBar_KeywordUpdateTab_WriteProgress.Maximum = mFileList.Count;

            // If multiple Targets or if a Target has multiple Panels do not update with the ComboBox Text
            List<string> targetNames = new List<string>();
            targetNames.Clear();
            foreach (string target in ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Items)
            {
                // Remove " Stars" from targetName so there is a single target name for the next foreach below (" Stars" will be added there)
                string targetName = target.Replace(" Stars", "");
                targetNames.Add(targetName.Trim());
            }
            targetNames = targetNames.Distinct().ToList();


            int count = 0;
            foreach (XisfFile xFile in mFileList)
            {
                xFile.KeywordUpdateMode = mKeywordUpdateProtection;
                if (xFile.KeywordUpdateMode == eKeywordUpdateMode.PROTECT)
                    return;

                if (mBCancel) { mBCancel = false; return; }

                xFile.SetObservationSite();
                xFile.KeepPanel = CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Checked;

                // Update with ComboBox Text if checked
                if (CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Checked)
                    // Rename everything to the ComboBox Text value
                    xFile.TargetName = ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text;

                ProgressBar_KeywordUpdateTab_WriteProgress.Value += 1;
                bStatus = mXisfFileUpdate.UpdateFile(xFile, xFile.FilePath);
                Label_KeywordUpdateTab_FileName.Text = Label_KeywordUpdateTab_FileName.Text = Path.GetDirectoryName(xFile.FilePath) + "\n" + Path.GetFileName(xFile.FilePath);
                System.Windows.Forms.Application.DoEvents();

                if (bStatus == false)
                {
                    Label_FileSelection_Statistics_Task.Text = "File Write Error";

                    var result = MessageBox.Show(
                        "File Update Failed - Protected or I/O Error.\n\n" + Label_KeywordUpdateTab_FileName.Text,
                        "\nMainForm.cs Button_Update_Click()",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    GroupBox_FileSelection.Enabled = true;
                    GroupBox_KeywordUpdateTab_SubFrameKeywords.Enabled = true;
                    GroupBox_KeywordUpdateTab_CaptureSoftware.Enabled = true;
                    GroupBox_KeywordUpdateTab_Telescope.Enabled = true;
                    GroupBox_KeywordUpdateTab_Camera.Enabled = true;
                    GroupBox_KeywordUpdateTab_ImageType.Enabled = true;
                    return;
                }

                count++;
            }

            Label_FileSelection_Statistics_Task.Text = count.ToString() + " Images Updated";
            GroupBox_FileSelection.Enabled = true;
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Enabled = true;
            GroupBox_KeywordUpdateTab_CaptureSoftware.Enabled = true;
            GroupBox_KeywordUpdateTab_Telescope.Enabled = true;
            GroupBox_KeywordUpdateTab_Camera.Enabled = true;
            GroupBox_KeywordUpdateTab_ImageType.Enabled = true;


            FindFilterFrameType(); // Update UI - NOT SURE WHY I NEED THIS HERE
        }

        private void RadioButton_WeightIndex_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_FileSelection_SequenceNumbering_WeightIndex.Checked)
            {
                mRenameFile.RenameOrder = eOrder.WEIGHTINDEX;
            }
        }

        private void RadioButton_Index_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_FileSelection_SequenceNumbering_IndexOnly.Checked)
            {
                mRenameFile.RenameOrder = eOrder.INDEX;
            }
        }

        private void RadioButton_Weight_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_FileSelection_SequenceNumbering_WeightOnly.Checked)
            {
                mRenameFile.RenameOrder = eOrder.WEIGHT;
            }
        }

        private void RadioButton_IndexWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton_FileSelection_SequenceNumbering_IndexWeight.Checked)
            {
                mRenameFile.RenameOrder = eOrder.INDEXWEIGHT;
            }
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
                string softwareCreator = file.CaptureSoftware; // from SWCREATE

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
                    if (!file.CaptureSoftware.Equals("NINA"))
                        file.AddKeyword("SWCREATE", "NINA", "[name] Equipment Control and Automation Application");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Checked)
                    if (!file.CaptureSoftware.Equals("TSX"))
                        file.AddKeyword("SWCREATE", "TSX", "[name] Equipment Control and Automation Application");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Checked)
                    if (!file.CaptureSoftware.Equals("SGP"))
                        file.AddKeyword("SWCREATE", "SGP", "[name] Equipment Control and Automation Application");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Checked)
                    if (!file.CaptureSoftware.Equals("VOY"))
                        file.AddKeyword("SWCREATE", "VOY", "[name] Equipment Control and Automation Application");

                if (RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Checked)
                    if (!file.CaptureSoftware.Equals("SCP"))
                        file.AddKeyword("SWCREATE", "SCP", "[name] Equipment Control and Automation Application");
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
                        file.AddKeyword("SWCREATE", captureSoftware.ToString(), "XISF File Manager");
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

                if (telescope.EndsWith('R'))
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
                file.AddKeyword("APTDIA", "107.0", "Aperture Diameter in mm");
                file.AddKeyword("APTAREA", "8992.02", "Aperture area in square mm minus obstructions");

                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.AddKeyword("TELESCOP", "APM107R", "APM107 Super ED with Riccardi 0.75 Reducer");
                    file.AddKeyword("FOCALLEN", "531", "APM107 Super ED with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.AddKeyword("TELESCOP", "APM107", "APM107 Super ED without Reducer");
                    file.AddKeyword("FOCALLEN", "700", "APM107 Super ED without Reducer");
                }
            }

            if (RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.AddKeyword("TELESCOP", "EVO150R", "EvoStar 150 with Riccardi 0.75 Reducer");
                    file.AddKeyword("FOCALLEN", "750", "EvoStar 150 with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.AddKeyword("TELESCOP", "EVO150", "EvoStar 150 without Reducer");
                    file.AddKeyword("FOCALLEN", "1000", "EvoStar 150 without Reducer");
                }
            }

            if (RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Checked)
            {
                if (CheckBox_KeywordUpdateTab_Telescope_Riccardi.Checked)
                {
                    file.AddKeyword("TELESCOP", "NWT254R", "10 Inch Newtownian with Riccardi 0.75 Reducer");
                    file.AddKeyword("FOCALLEN", "825", "10 inch Newtonian with Riccardi 0.75 Reducer");
                }
                else
                {
                    file.AddKeyword("TELESCOP", "NWT254", "10 Inch Newtonian without Reducer");
                    file.AddKeyword("FOCALLEN", "1100", "10 Inch Newtonian without Reducer");
                }
            }
        }

        private void Button_Telescope_SetAll_Click(object sender, EventArgs e)
        {
            foreach (XisfFile file in mFileList)
            {
                SetTelescopeUI(file);
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

                        if (telescope.EndsWith('R'))
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
                    if (file.FrameType == eFrame.EMPTY)
                        file.AddKeyword("IMAGETYP", frameTypeText.ToString(), "XISF File Manager");
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

                file.AddKeyword("IMAGETYP", frameTypeText.ToString(), "XISF File Manager");
                if (frameTypeText.Equals("Dark") || frameTypeText.Equals("Bias"))
                {
                    file.AddKeyword("FILTER", "Shutter", "Opaque 1.25 via Starlight Xpress USB 7 Position Wheel");
                }
            }



            foreach (XisfFile file in mFileList)
            {
                if (globalFilter)
                {
                    if (file.FilterName == string.Empty)
                        file.AddKeyword("FILTER", globalFilterText.ToString(), "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");
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

                file.AddKeyword("FILTER", globalFilterText.ToString(), "Astrodon 1.25 via Starlight Xpress USB 7 Position Wheel");
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
                filter = file.FilterName.Trim();

                file.FilterName = filter;

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
                if (file.FrameType == eFrame.LIGHT)
                {
                    foundLight = true;
                    lightCount++;
                    frameTypeCount++;
                }

                if (file.FrameType == eFrame.DARK)
                {
                    foundDark = true;
                    darkCount++;
                    frameTypeCount++;
                }

                if (file.FrameType == eFrame.FLAT)
                {
                    foundFlat = true;
                    flatCount++;
                    frameTypeCount++;
                }

                if (file.FrameType == eFrame.BIAS)
                {
                    foundBias = true;
                    biasCount++;
                    frameTypeCount++;
                }

                if (file.TargetName.Equals("Master"))
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
                    CheckBox_FileSlection_NoTotals.Checked = true;
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

            // cameraList should contain an entry for each file
            List<string> CameraList = mFileList.Select(c => c.Camera).ToList();
            bool bFoundZ183 = CameraList.Any(c => c.Contains("183"));
            bool bFoundZ533 = CameraList.Any(c => c.Contains("533"));
            bool bFoundQ178 = CameraList.Any(c => c.Contains("178"));
            bool bFoundA144 = CameraList.Any(c => c.Contains("144"));

            bool bNoCameras = CameraList.Count == 0;
            bool bMissingCameras = (CameraList.Count != mFileList.Count) && !bNoCameras;
            bool bDifferentCameras = ((bFoundZ183 ? 1 : 0) + (bFoundZ533 ? 1 : 0) + (bFoundQ178 ? 1 : 0) + (bFoundA144 ? 1 : 0) >= 2) && !bMissingCameras;
            bool bUniqueCamera = !bMissingCameras && !bDifferentCameras && !bNoCameras;


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
                List<double> SecondsListZ533 = mFileList.Where(i => i.ExposureSeconds >= 0 && i.Camera.Contains("533")).Select(i => i.ExposureSeconds).ToList();
                bNoSecondsZ533 = SecondsListZ533.Count == 0;
                bMissingSecondsZ533 = SecondsListZ533.Count != CameraList.Count(c => c.Contains("533")) && !bNoSecondsZ533;
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
                List<double> SecondsListZ183 = mFileList.Where(i => i.ExposureSeconds >= 0 && i.Camera.Contains("183")).Select(i => i.ExposureSeconds).ToList();
                bNoSecondsZ183 = SecondsListZ183.Count == 0;
                bMissingSecondsZ183 = SecondsListZ183.Count != CameraList.Count(c => c.Contains("183")) && !bNoSecondsZ183;
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
                List<double> SecondsListQ178 = mFileList.Where(i => i.ExposureSeconds >= 0 && i.Camera.Contains("178")).Select(i => i.ExposureSeconds).ToList();
                bNoSecondsQ178 = SecondsListQ178.Count == 0;
                bMissingSecondsQ178 = SecondsListQ178.Count != CameraList.Count(c => c.Contains("178")) && !bNoSecondsQ178;
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
                List<double> SecondsListA144 = mFileList.Where(i => i.ExposureSeconds >= 0 && i.Camera.Contains("A44")).Select(i => i.ExposureSeconds).ToList();
                bNoSecondsA144 = SecondsListA144.Count == 0;
                bMissingSecondsA144 = SecondsListA144.Count != CameraList.Count(c => c.Contains("144")) && !bNoSecondsA144;
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
                List<int> GainListZ533 = mFileList.Where(i => i.Gain >= 0 && i.Camera.Contains("533")).Select(i => i.Gain).ToList();
                bNoGainsZ533 = GainListZ533.Count == 0;
                bMissingGainsZ533 = GainListZ533.Count != CameraList.Count(c => c.Contains("533")) && !bNoGainsZ533;
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
                List<int> GainListZ183 = mFileList.Where(i => i.Gain >= 0 && i.Camera.Contains("183")).Select(i => i.Gain).ToList();
                bNoGainsZ183 = GainListZ183.Count == 0;
                bMissingGainsZ183 = GainListZ183.Count != CameraList.Count(c => c.Contains("183")) && !bNoGainsZ183;
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
                List<int> GainListQ178 = mFileList.Where(i => i.Gain >= 0 && i.Camera.Contains("178")).Select(i => i.Gain).ToList();
                bNoGainsQ178 = GainListQ178.Count == 0;
                bMissingGainsQ178 = GainListQ178.Count != CameraList.Count(c => c.Contains("178")) && !bNoGainsQ178;
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
                List<int> OffsetListZ533 = mFileList.Where(i => i.Offset >= 0 && i.Camera.Contains("533")).Select(i => i.Offset).ToList();
                bNoOffsetsZ533 = OffsetListZ533.Count == 0;
                bMissingOffsetsZ533 = OffsetListZ533.Count != CameraList.Count(c => c.Contains("533")) && !bNoOffsetsZ533;
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
                List<int> OffsetListZ183 = mFileList.Where(i => i.Offset >= 0 && i.Camera.Contains("183")).Select(i => i.Offset).ToList();
                bNoOffsetsZ183 = OffsetListZ183.Count == 0;
                bMissingOffsetsZ183 = OffsetListZ183.Count != CameraList.Count(c => c.Contains("183")) && !bNoOffsetsZ183;
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
                List<int> OffsetListQ178 = mFileList.Where(i => i.Offset >= 0 && i.Camera.Contains("178")).Select(i => i.Offset).ToList();
                bNoOffsetsQ178 = OffsetListQ178.Count == 0;
                bMissingOffsetsQ178 = OffsetListQ178.Count != CameraList.Count(c => c.Contains("178")) && !bNoOffsetsQ178;
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
                List<double> SensorTempListZ533 = mFileList.Where(i => i.SensorTemperature != -273 && i.Camera.Contains("533")).Select(i => i.SensorTemperature).ToList();
                bNoSensorTempsZ533 = SensorTempListZ533.Count == 0;
                bMissingSensorTempsZ533 = SensorTempListZ533.Count != CameraList.Count(c => c.Contains("533")) && !bNoSensorTempsZ533;
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
                List<double> SensorTempListZ183 = mFileList.Where(i => i.SensorTemperature != -273 && i.Camera.Contains("183")).Select(i => i.SensorTemperature).ToList();
                bNoSensorTempsZ183 = SensorTempListZ183.Count == 0;
                bMissingSensorTempsZ183 = SensorTempListZ183.Count != CameraList.Count(c => c.Contains("183")) && !bNoSensorTempsZ183;
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
                List<double> SensorTempListQ178 = mFileList.Where(i => i.FocuserTemperature != -273 && i.Camera.Contains("178")).Select(i => i.FocuserTemperature).ToList();
                bNoSensorTempsQ178 = SensorTempListQ178.Count == 0;
                bMissingSensorTempsQ178 = SensorTempListQ178.Count != CameraList.Count(c => c.Contains("178")) && !bNoSensorTempsQ178;
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
                List<double> SensorTempListA144 = mFileList.Where(i => i.FocuserTemperature != -273 && i.Camera.Contains("144")).Select(i => i.FocuserTemperature).ToList();
                bNoSensorTempsA144 = SensorTempListA144.Count == 0;
                bMissingSensorTempsA144 = SensorTempListA144.Count != CameraList.Count(c => c.Contains("144")) && !bNoSensorTempsA144;
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
                List<int> BinningListZ533 = mFileList.Where(i => i.Binning > 0 && i.Camera.Contains("533")).Select(i => i.Binning).ToList();
                bNoBinningsZ533 = BinningListZ533.Count == 0;
                bMissingBinningsZ533 = BinningListZ533.Count != CameraList.Count(c => c.Contains("533")) && !bNoBinningsZ533;
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
                List<int> BinningListZ183 = mFileList.Where(i => i.Binning > 0 && i.Camera.Contains("183")).Select(i => i.Binning).ToList();
                bNoBinningsZ183 = BinningListZ183.Count == 0;
                bMissingBinningsZ183 = BinningListZ183.Count != CameraList.Count(c => c.Contains("183")) && !bNoBinningsZ183;
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
                List<int> BinningListQ178 = mFileList.Where(i => i.Binning > 0 && i.Camera.Contains("178")).Select(i => i.Binning).ToList();
                bNoBinningsQ178 = BinningListQ178.Count == 0;
                bMissingBinningsQ178 = BinningListQ178.Count != CameraList.Count(c => c.Contains("178")) && !bNoBinningsQ178;
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
                List<int> BinningsListA144 = mFileList.Where(i => i.Binning > 0 && i.Camera.Contains("144")).Select(i => i.Binning).ToList();
                bNoBinningsA144 = BinningsListA144.Count == 0;
                bMissingBinningsA144 = BinningsListA144.Count != CameraList.Count(c => c.Contains("144")) && !bNoBinningsA144;
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
        private static void FindMasterStatistics()
        {
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
            bool bStatus;

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

                file.AddKeyword("BITPIX", "16", "Bits Per Pixel");
                file.AddKeyword("BSCALE", "1", "Multiply Raw Values by BSCALE");
                file.AddKeyword("BZERO", "32768", "Add value to scale to 65536 (16 bit) values");
                file.AddKeyword("NAXIS", "2", "XISF File Manager");


                if (CheckBox_KeywordUpdateTab_Camera_Z533.Checked)
                {
                    file.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    file.AddKeyword("NAXIS1", "3008", "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", "3008", "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", "3.76", "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", "3.76", "Vertical Pixel Size in Microns");
                    file.AddKeyword("BAYERPAT", "RGGB");
                    file.AddKeyword("COLORSPC", "Color", "Color Image");

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Text, out value);
                    if (bStatus)
                        file.ExposureSeconds = value;
                    else
                        file.ExposureSeconds = file.ExposureSeconds;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Gain.Text, out parseInt);
                    if (bStatus)
                        file.Gain = parseInt;
                    else
                        file.Gain = file.Gain;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Offset.Text, out parseInt);
                    if (bStatus)
                        file.Offset = parseInt;
                    else
                        file.Offset = file.Offset;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text, out parseInt);
                    if (bStatus)
                        file.Binning = parseInt;
                    else
                        file.Binning = file.Binning;

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Text, out value);
                    if (bStatus)
                        file.SensorTemperature = value;
                    else
                        file.SensorTemperature = file.SensorTemperature;
                }

                if (CheckBox_KeywordUpdateTab_Camera_Z183.Checked)
                {
                    file.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    file.AddKeyword("NAXIS1", "5496", "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", "3672", "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", "2.4", "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", "2.4", "Vertical Pixel Size in Microns");
                    file.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Seconds.Text, out value);
                    if (bStatus)
                        file.ExposureSeconds = value;
                    else
                        file.ExposureSeconds = file.ExposureSeconds;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Gain.Text, out parseInt);
                    if (bStatus)
                        file.Gain = parseInt;
                    else
                        file.Gain = file.Gain;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Offset.Text, out parseInt);
                    if (bStatus)
                        file.Offset = parseInt;
                    else
                        file.Offset = file.Offset;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Binning.Text, out parseInt);
                    if (bStatus)
                        file.Binning = parseInt;
                    else
                        file.Binning = file.Binning;

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.Text, out value);
                    if (bStatus)
                        file.SensorTemperature = value;
                    else
                        file.SensorTemperature = file.SensorTemperature;
                }

                if (CheckBox_KeywordUpdateTab_Camera_Q178.Checked)
                {
                    file.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    file.AddKeyword("NAXIS1", "3072", "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", "2048", "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", "2.4", "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", "2.4", "Vertical Pixel Size in Microns");
                    file.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Seconds.Text, out value);
                    if (bStatus)
                        file.ExposureSeconds = value;
                    else
                        file.ExposureSeconds = file.ExposureSeconds;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Gain.Text, out parseInt);
                    if (bStatus)
                        file.Gain = parseInt;
                    else
                        file.Gain = file.Gain;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Offset.Text, out parseInt);
                    if (bStatus)
                        file.Offset = parseInt;
                    else
                        file.Offset = file.Offset;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Binning.Text, out parseInt);
                    if (bStatus)
                        file.Binning = parseInt;
                    else
                        file.Binning = file.Binning;

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.Text, out value);
                    if (bStatus)
                    {
                        file.FocuserTemperature = value;
                        file.SensorTemperature = value;
                    }
                    else
                    {
                        file.FocuserTemperature = file.FocuserTemperature;
                        file.SensorTemperature = file.SensorTemperature;
                    }
                }

                if (CheckBox_KeywordUpdateTab_Camera_A144.Checked)
                {
                    file.AddKeyword("INSTRUME", "A144", "Atik Infinity Camera (2018)");
                    file.AddKeyword("NAXIS1", "1392", "Horizontal Pixel Width");
                    file.AddKeyword("NAXIS2", "1040", "Vertical Pixel Height");
                    file.AddKeyword("XPIXSZ", "6.45", "Horizonal Pixel Size in Microns");
                    file.AddKeyword("YPIXSZ", "6.45", "Vertical Pixel Size in Microns");
                    file.AddKeyword("BAYERPAT", "RGGB");
                    file.AddKeyword("COLORSPC", "Color", "Color Image");
                    file.AddKeyword("GAIN", "0.37", "Fixed");
                    file.RemoveKeyword("OFFSET");

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_A144Seconds.Text, out value);
                    if (bStatus)
                        file.ExposureSeconds = value;
                    else
                        file.ExposureSeconds = file.ExposureSeconds;

                    bStatus = int.TryParse(ComboBox_KeywordUpdateTab_Camera_A144Binning.Text, out parseInt);
                    if (bStatus)
                        file.Binning = parseInt;
                    else
                        file.Binning = file.Binning;

                    bStatus = double.TryParse(ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.Text, out value);
                    if (bStatus)
                        file.SensorTemperature = value;
                    else
                        file.SensorTemperature = file.SensorTemperature;
                }
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

            foreach (XisfFile xFile in mFileList)
            {
                xFile.RemoveKeyword("NAXIS3");
                xFile.RemoveKeyword("EXPOSURE");

                xFile.AddKeyword("BITPIX", "16", "Bits Per Pixel");
                xFile.AddKeyword("BSCALE", "1", "Multiply Raw Values by BSCALE");
                xFile.AddKeyword("BZERO", "32768", "Add value to scale to 65536 (16 bit) values");
                string temperatureTextUI = ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Text;

                string temperatureText;
                if (globalTemperature)
                {
                    temperatureText = xFile.SensorTemperature.ToString();
                    if (temperatureText == string.Empty)
                    {
                        temperatureText = globalTemperatureText;
                    }
                }
                else
                {
                    if (temperatureTextUI == string.Empty)
                    {
                        globalTemperatureText = xFile.SensorTemperature.ToString();
                        if (globalTemperatureText.Contains("Global_"))
                        {
                            globalTemperature = true;
                            globalTemperatureText = globalTemperatureText.Replace("Global_", "");
                        }

                        temperatureText = globalTemperatureText;
                    }
                    else
                    {
                        temperatureText = xFile.SensorTemperature.ToString();
                        if (temperatureText == string.Empty)
                        {
                            temperatureText = temperatureTextUI;
                        }
                    }
                }

                double temperature;
                status = double.TryParse(temperatureText, out temperature);
                xFile.AddKeyword("CCD-TEMP", temperature.ToString(), "Actual Sensor Temperature");

                xFile.AddKeyword("NAXIS", "2", "XISF File Manager");
                xFile.Binning = Int32.Parse(ComboBox_KeywordUpdateTab_Camera_Z533Binning.Text);
                string secondsTextUI = ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Text;

                string secondsText;
                if (globalSeconds)
                {
                    secondsText = xFile.ExposureSeconds.FormatExposureTime();
                    if (secondsText == string.Empty)
                    {
                        secondsText = globalSecondsText;
                    }
                }
                else
                {
                    if (secondsTextUI == string.Empty)
                    {
                        globalSecondsText = xFile.ExposureSeconds.FormatExposureTime();
                        if (globalSecondsText.Contains("Global_"))
                        {
                            globalSeconds = true;
                            globalSecondsText = globalSecondsText.Replace("Global_", "");
                        }

                        secondsText = globalSecondsText;
                    }
                    else
                    {
                        secondsText = xFile.ExposureSeconds.FormatExposureTime();
                        if (secondsText == string.Empty)
                        {
                            secondsText = secondsTextUI;
                        }
                    }
                }

                double seconds;
                status = double.TryParse(secondsText, out seconds);
                xFile.AddKeyword("EXPTIME", seconds.ToString(), "Exposure Time in Seconds");




                int gainValue;
                int gainValueUI;
                int offsetValue;
                int offsetValueUI;
                if (CheckBox_KeywordUpdateTab_Camera_Z533.Checked)
                {
                    xFile.AddKeyword("INSTRUME", "Z533", "ZWO ASI533MC Pro Camera (2021)");
                    xFile.AddKeyword("NAXIS1", "3008", "Horizontal Pixel Width");
                    xFile.AddKeyword("NAXIS2", "3008", "Vertical Pixel Height");
                    xFile.AddKeyword("XPIXSZ", "3.76", "Horizonal Pixel Size in Microns");
                    xFile.AddKeyword("YPIXSZ", "3.76", "Vertical Pixel Size in Microns");
                    xFile.AddKeyword("BAYERPAT", "RGGB");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = xFile.Gain;
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = xFile.Gain;
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = xFile.Gain;
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    xFile.Gain = gainValue;


                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z533Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = xFile.Offset;
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = xFile.Offset;
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = xFile.Offset;
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    xFile.Offset = offsetValue;
                }

                if (CheckBox_KeywordUpdateTab_Camera_Z183.Checked)
                {
                    xFile.AddKeyword("INSTRUME", "Z183", "ZWO ASI183MM Pro Camera (2019)");
                    xFile.AddKeyword("NAXIS1", "5496", "Horizontal Pixel Width");
                    xFile.AddKeyword("NAXIS2", "3672", "Vertical Pixel Height");
                    xFile.AddKeyword("XPIXSZ", "2.4", "Horizonal Pixel Size in Microns");
                    xFile.AddKeyword("YPIXSZ", "2.4", "Vertical Pixel Size in Microns");
                    xFile.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = xFile.Gain;
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = xFile.Gain;
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = xFile.Gain;
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    xFile.Gain = gainValue;


                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Z183Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = xFile.Offset;
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = xFile.Offset;
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = xFile.Offset;
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    xFile.AddKeyword("OFFSET", offsetValue.ToString(), "Camera Offset");
                }




                if (CheckBox_KeywordUpdateTab_Camera_Q178.Checked)
                {
                    xFile.AddKeyword("INSTRUME", "Q178", "QHYCCD QHY5III178M Camera (2018)");
                    xFile.AddKeyword("NAXIS1", "3072", "Horizontal Pixel Width");
                    xFile.AddKeyword("NAXIS2", "2048", "Vertical Pixel Height");
                    xFile.AddKeyword("XPIXSZ", "2.4", "Horizonal Pixel Size in Microns");
                    xFile.AddKeyword("YPIXSZ", "2.4", "Vertical Pixel Size in Microns");
                    xFile.AddKeyword("COLORSPC", "Grayscale", "Monochrome Image");

                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Gain.Text, out gainValueUI);
                    gainValueUI = status ? gainValueUI : -1;

                    if (globalGain)
                    {
                        gainValue = xFile.Gain;
                        if (gainValue < 0)
                        {
                            gainValue = globalGainValue;
                        }
                    }
                    else
                    {
                        if (gainValueUI < 0)
                        {
                            globalGainValue = xFile.Gain;
                            if (globalGainValue < 0)
                            {
                                globalGain = true;
                                globalGainValue = -globalGainValue;
                            }

                            gainValue = globalGainValue;
                        }
                        else
                        {
                            gainValue = xFile.Gain;
                            if (gainValue < 0)
                            {
                                gainValue = gainValueUI;
                            }
                        }
                    }

                    xFile.Gain = gainValue;


                    status = int.TryParse(ComboBox_KeywordUpdateTab_Camera_Q178Offset.Text, out offsetValueUI);
                    offsetValueUI = status ? offsetValueUI : -1;

                    if (globalOffset)
                    {
                        offsetValue = xFile.Offset;
                        if (offsetValue < 0)
                        {
                            offsetValue = globalOffsetValue;
                        }
                    }
                    else
                    {
                        if (offsetValueUI < 0)
                        {
                            globalOffsetValue = xFile.Offset;
                            if (globalOffsetValue < 0)
                            {
                                globalOffset = true;
                                globalOffsetValue = -globalOffsetValue;
                            }

                            offsetValue = globalOffsetValue;
                        }
                        else
                        {
                            offsetValue = xFile.Offset;
                            if (offsetValue < 0)
                            {
                                offsetValue = offsetValueUI;
                            }
                        }
                    }

                    xFile.Offset = offsetValue;
                }

                if (CheckBox_KeywordUpdateTab_Camera_A144.Checked)
                {
                    xFile.AddKeyword("INSTRUME", "A144", "Atik Infinity Camera (2018)");
                    xFile.AddKeyword("NAXIS1", "1392", "Horizontal Pixel Width");
                    xFile.AddKeyword("NAXIS2", "1040", "Vertical Pixel Height");
                    xFile.AddKeyword("XPIXSZ", "6.45", "Horizonal Pixel Size in Microns");
                    xFile.AddKeyword("YPIXSZ", "6.45", "Vertical Pixel Size in Microns");
                    xFile.AddKeyword("BAYERPAT", "RGGB");
                    xFile.RemoveKeyword("GAIN");
                    xFile.RemoveKeyword("OFFSET");
                }
            }

            FindCamera();
        }

        private void Button_KeywordImageTypeFrame_SetMaster_Click(object sender, EventArgs e)
        {
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Text = "Master";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Checked = true;
            CheckBox_FileSelection_DirectorySelection_Master.Checked = true;
            CheckBox_FileSlection_NoTotals.Checked = true;
        }

        private void CheckBox_Master_CheckedChanged(object sender, EventArgs e)
        {
            string rejection = string.Empty;
            string comment = string.Empty;

            Files.DirectoryOperations.Recurse = CheckBox_FileSelection_DirectorySelection_Recurse.Checked;

            TextBox_FileSelection_DirectorySelection_TotalFrames.Enabled = CheckBox_FileSelection_DirectorySelection_Master.Checked;
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Enabled = CheckBox_FileSelection_DirectorySelection_Master.Checked;

            if (CheckBox_FileSelection_DirectorySelection_Master.Checked)
            {
                foreach (XisfFile file in mFileList)
                {
                    file.KeywordList.SetMasterFrameKeywords();
                }
            }
        }

        private void Button_KeywordSubFrameWeight_Remove_Click(object sender, EventArgs e)
        {
            List<string> WeightKeywords = new List<string>();

            bool bStatus;
            string frames = TextBox_FileSelection_DirectorySelection_TotalFrames.Text;
            string algo = ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Text;

            int mTotalFrames = 0;
            bStatus = int.TryParse(frames, out mTotalFrames);



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
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Red;
                else
                    Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.ForeColor = Color.Black;

                if (WeightKeywords.Count > 0)
                    ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.SelectedIndex = 0;
            }
        }

        private async void CalibrationTab_FindCalibrationFrames_Click(object sender, EventArgs e)
        {
            bool bMatchedAllFiles = false;
            string calibrationFileMasterLibraryLocation;

            TextBox_CalibrationTab_Messgaes.Clear();
            mCalibration.Frame = eFrame.ALL;

            calibrationFileMasterLibraryLocation = @"E:\Photography\Astro Photography\Calibration";

            if (!bMatchedAllFiles)
                await mCalibration.ReadCalibrationFramesAsync(calibrationFileMasterLibraryLocation);

            mCalibration.MatchTargetsWithCalibrationLibraryFrames(mFileList);
        }

        private void CalibrationTab_ReMatchCalibrationFrames_Click(object sender, EventArgs e)
        {
            TextBox_CalibrationTab_Messgaes.Clear();

            mCalibration.MatchTargetsWithCalibrationLibraryFrames(mFileList);
        }

        private void CalibrationTab_CreateCalibrationDirectory_Click(object sender, EventArgs e)
        {
            if (CheckBox_CalibrationTab_CreateNew.Checked == true)
            {
                string targetCalibrationDirectory = Calibration.SetTargetCalibrationFileDirectories(mFileList[0].FilePath);

                if (Directory.Exists(targetCalibrationDirectory))
                    Directory.Delete(targetCalibrationDirectory, true);

                Directory.CreateDirectory(targetCalibrationDirectory);
            }

            mCalibration.CreateTargetCalibrationDirectory(mFileList);
        }

        private void TextBox_CalibrationTab_ExposureTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Exposure.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Exposure.Text = "0";
                return;
            }

            mCalibration.ExposureTolerance = value;
        }

        private void TextBox_CalibrationTab_GainTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Gain.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Gain.Text = "0";
                return;
            }

            mCalibration.GainTolerance = value;
        }

        private void TextBox_CalibrationTab_OffsetTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Offset.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Offset.Text = "0";
                return;
            }

            mCalibration.OffsetTolerance = value;

        }

        private void TextBox_CalibrationTab_TemperatureTolerance_TextChanged(object sender, EventArgs e)
        {
            double value;

            if (double.TryParse(TextBox_CalibrationTab_MatchingTolerance_Temperature.Text, out value) == false)
            {
                TextBox_CalibrationTab_MatchingTolerance_Temperature.Text = "5";
                return;
            }

            mCalibration.TemperatureTolerance = value;
        }

        private void Button_SubFrameKeywords_CalibrationFiles_ClearAll_Click(object sender, EventArgs e)
        {
            foreach (var file in mFileList)
            {
                file.CDARK = string.Empty;
                file.CFLAT = string.Empty;
                file.CBIAS = string.Empty;
                file.CPANEL = string.Empty;
                file.CSTARS = string.Empty;
                file.RemoveKeyword("CLIGHT");
            }
        }

        private void ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text) || ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text == "Keyword"))
                return;

            List<Keyword> keywordList = new List<Keyword>();

            foreach (XisfFile file in mFileList)
            {
                foreach (var keyword in file.KeywordList.mKeywordList)
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


            //List<XisfFile> filesWithKeyword = mFileList
            //            .Where(file => file.KeywordList.Any(keyword => keyword.Value == targetKeywordValue))
            //            .ToList();


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
                    file.KeywordList.AddKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);

                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Checked)
                {
                    file.KeywordList.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text);
                    file.KeywordList.AddKeywordKeepDuplicates(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);
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
                    xFile.KeywordList.AddKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);

                if (RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Checked)
                {
                    xFile.KeywordList.RemoveKeyword(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text);
                    xFile.KeywordList.AddKeywordKeepDuplicates(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text, ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text);
                }
            }

            RefreshComboBoxes();
        }

        private void RefreshComboBoxes()
        {
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordFile.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordFile.Text = "File";

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text = "Name";

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text = "Value";

            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Items.Clear();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text = "Comment";


            List<string> keywordNamelist = new List<string>();

            foreach (XisfFile xFile in mFileList)
            {
                foreach (var keywordName in xFile.KeywordList.mKeywordList)
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

        // **********************************************************************************************************************************
        // **********************************************************************************************************************************
        // Target Scheduler Methods
        // **********************************************************************************************************************************
        // **********************************************************************************************************************************

        private void Button_SchedulerTab_OpenDatabase_Click(object sender, EventArgs e)
        {
            mSchedulerDB.mSqlReader.ReadDataBaseFile(@"\\BIRDWATCHER\SchedulerPlugin\schedulerdbTest.sqlite");

            TreeView_SchedulerTab_ProfileTree.Nodes.Clear();
            TreeView_SchedulerTab_ProjectTree.Nodes.Clear();

            foreach (var profilePreference in mSchedulerDB.mProfilePreferenceList)
            {
                TreeNode TreeView_SchedulerTab_ProfileTree_RootNode = new TreeNode(profilePreference.profileId.Substring(profilePreference.profileId.LastIndexOf('-') + 5));
                TreeView_SchedulerTab_ProfileTree.Nodes.Add(TreeView_SchedulerTab_ProfileTree_RootNode);

                TreeNode TreeView_SchedulerTab_ProjectTree_RootNode = new TreeNode(profilePreference.profileId.Substring(profilePreference.profileId.LastIndexOf('-') + 5));
                TreeView_SchedulerTab_ProjectTree.Nodes.Add(TreeView_SchedulerTab_ProjectTree_RootNode);

                foreach (var project in mSchedulerDB.mProjectList)
                {
                    if (project.profileId == profilePreference.profileId)
                    {
                        TreeNode projectNode = new TreeNode(project.name);
                        TreeView_SchedulerTab_ProjectTree_RootNode.Nodes.Add(projectNode);
                    }
                }
            }

            TreeView_SchedulerTab_TargetTree.Nodes.Clear();

            foreach (var target in mSchedulerDB.mTargetList)
            {
                TreeNode TreeView_SchedulerTab_TargetTree_RootNode = new TreeNode(target.name);
                TreeView_SchedulerTab_TargetTree.Nodes.Add(TreeView_SchedulerTab_TargetTree_RootNode);
            }

            RefineExposurePlans();

            TreeView_SchedulerTab_ProfileTree.ExpandAll();
            TreeView_SchedulerTab_ProjectTree.ExpandAll();


            // Do Things
            mSchedulerDB.UpdateTargetImageCounts(mFileList);
        }

        private void TreeView_SchedulerTab_ProfileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode clickedNode = e.Node;

            string clickedItem = clickedNode.Text;
            MessageBox.Show($"You clicked on: {clickedItem}");
        }

        private void TreeView_SchedulerTab_ProjectTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode clickedNode = e.Node;

            if (clickedNode.Parent == null)
                // We clicked on a Profile
                RefineSelectedProjectTreeView(clickedNode);
            else
            {
                // We clicked on a Project
                SetProjectActiveCheckBox(clickedNode);
                SetProjectPriorityRadioButtons(clickedNode);

                RefineSelectedTargetTreeView(clickedNode);
            }

            RefineExposurePlans();
        }
        private string ProfilePreference(TreeNode projectNode) { return mSchedulerDB.mProfilePreferenceList.Find(profile => profile.profileId.Contains(projectNode.Parent.Text)).profileId; }
        private bool ProjectState(TreeNode projectNode, string sProfilePreference) { return mSchedulerDB.mProjectList.Any(project => project.name == projectNode.Text && project.profileId == sProfilePreference); }
        private int ProjectPriority(TreeNode projectNode, string sProfilePreference) { return mSchedulerDB.mProjectList.Find(project => (project.name == projectNode.Text) && (project.profileId == sProfilePreference)).priority; }

        private void SetProjectActiveCheckBox(TreeNode projectNode)
        {
            string profilePreference = ProfilePreference(projectNode);
            CheckBox_Project_Active.Checked = ProjectState(projectNode, profilePreference);
        }

        private void SetProjectPriorityRadioButtons(TreeNode projectNode)
        {
            string profilePreference = ProfilePreference(projectNode);
            int priority = ProjectPriority(projectNode, profilePreference);
            switch (priority)
            {
                case (int)eProjectPriority.LOW:
                    RadioButton_ProjectPriority_Low.Checked = true;
                    break;
                case (int)eProjectPriority.NORMAL:
                    RadioButton_ProjectPriority_Normal.Checked = true;
                    break;
                case (int)eProjectPriority.HIGH:
                    RadioButton_ProjectPriority_High.Checked = true;
                    break;
            }


            RefineSelectedProjectTreeView(projectNode);
        }

        private static void ExpandAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Expand();
                ExpandAllNodes(node.Nodes);
            }
        }

        private void TreeView_SchedulerTab_ProjectTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                e.DrawDefault = true;
                return;
            }
            string profilePreference = ProfilePreference(e.Node);

            Font nodeFont;
            bool bActive = ProjectState(e.Node, profilePreference);
            if (bActive)
                nodeFont = ((TreeView)sender).Font;
            else
                nodeFont = new Font(((TreeView)sender).Font, FontStyle.Strikeout);


            int priority = ProjectPriority(e.Node, profilePreference);
            switch (priority)
            {
                case (int)eProjectPriority.LOW:
                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.SandyBrown, e.Bounds);
                    break;
                case (int)eProjectPriority.NORMAL:
                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.Black, e.Bounds);
                    break;
                case (int)eProjectPriority.HIGH:
                    e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.DarkMagenta, e.Bounds);
                    break;
            }
        }

        private void TreeView_SchedulerTab_TargetTree_NodeMouseClick_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode clickedNode = e.Node;

            TreeNode targetNodeParent = clickedNode.Parent;

            RefineExposurePlans();
        }

        private void TreeView_SchedulerTab_PlanTree_NodeMouseClick_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // Get the clicked TreeNode
            TreeNode clickedNode = e.Node;

            if (clickedNode == null)
                return;

            string clickedItem = clickedNode.Text;
            MessageBox.Show($"You clicked on: {clickedItem}");
        }

        private void RefineSelectedProjectTreeView(TreeNode clickedNode)
        {
            TreeView_SchedulerTab_ProjectTree.Nodes.Clear();
            TreeView_SchedulerTab_TargetTree.Nodes.Clear();

            string profilePreference = ProfilePreference(clickedNode);

            TreeNode TreeView_SchedulerTab_ProjectTree_RootNode = new TreeNode(profilePreference.Substring(profilePreference.LastIndexOf('-') + 5));
            TreeView_SchedulerTab_ProjectTree.Nodes.Add(TreeView_SchedulerTab_ProjectTree_RootNode);

            foreach (var project in mSchedulerDB.mProjectList)
            {
                if (project.profileId == profilePreference)
                {
                    TreeNode projectNode = new TreeNode(project.name);
                    TreeView_SchedulerTab_ProjectTree_RootNode.Nodes.Add(projectNode);
                }
            }

            TreeView_SchedulerTab_ProjectTree.ExpandAll();
        }

        private void RefineSelectedTargetTreeView(TreeNode clickedNode)
        {
            TreeNode profileNode = clickedNode.Parent;

            TreeView_SchedulerTab_TargetTree.Nodes.Clear();

            string projectProfileId = mSchedulerDB.mProjectList.Find(project => project.profileId.Contains(profileNode.Text)).profileId;

            int projectId = mSchedulerDB.mProjectList.Find(project => project.name == clickedNode.Text && project.profileId == projectProfileId).Id;

            string profileId = mSchedulerDB.mProjectList.Find(project => project.profileId == projectProfileId).profileId;

            foreach (var profilePreference in mSchedulerDB.mProfilePreferenceList)
            {
                foreach (var target in mSchedulerDB.mTargetList)
                {
                    if ((projectId == target.projectid) && (projectProfileId == profilePreference.profileId))
                    {
                        TreeNode TreeView_SchedulerTab_TargetTree_RootNode = new TreeNode(target.name);
                        TreeView_SchedulerTab_TargetTree.Nodes.Add(TreeView_SchedulerTab_TargetTree_RootNode);
                    }
                }
            }
        }

        private void RefineExposurePlans()
        {
            TreeView_SchedulerTab_PlansTree.Nodes.Clear();

            foreach (TreeNode targetNode in TreeView_SchedulerTab_TargetTree.Nodes)
            {
                int targetId = mSchedulerDB.mTargetList.Find(target => target.name.Equals(targetNode.Text)).Id;
                string targetName = targetNode.Text;

                List<int> exposureTemplateIdList = mSchedulerDB.mExposurePlanList
                                .Where(plan => plan.targetid == targetId)
                                .Select(plan => plan.exposureTemplateId)
                                .ToList();

                foreach (var plan in exposureTemplateIdList)
                {
                    string exposurePlanName = targetName + " " + mSchedulerDB.mExposureTemplateList.Find(template => template.Id == plan).filtername;
                    TreeNode TreeView_SchedulerTab_PlansTree_RootNode = new TreeNode(exposurePlanName);
                    TreeView_SchedulerTab_PlansTree.Nodes.Add(TreeView_SchedulerTab_PlansTree_RootNode);
                }
            }
        }

        private void Button_KeywordUpdateTab_Cancel_Click(object sender, EventArgs e)
        {
            mBCancel = true;
        }

        private void CheckBox_CalibrationTab_MatchingTolerance_ExposureNearest_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_CalibrationTab_MatchingTolerance_Exposure.Enabled = !CheckBox_CalibrationTab_MatchingTolerance_ExposureNearest.Checked;
        }

        private void CheckBox_CalibrationTab_MatchingTolerance_GainNearest_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_CalibrationTab_MatchingTolerance_Gain.Enabled = !CheckBox_CalibrationTab_MatchingTolerance_GainNearest.Checked;
        }

        private void CheckBox_CalibrationTab_MatchingTolerance_OffsetNearest_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_CalibrationTab_MatchingTolerance_Offset.Enabled = !CheckBox_CalibrationTab_MatchingTolerance_OffsetNearest.Checked;
        }

        private void CheckBox_CalibrationTab_MatchingTolerance_TemperatureNearest_CheckedChanged(object sender, EventArgs e)
        {
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Enabled = !CheckBox_CalibrationTab_MatchingTolerance_TemperatureNearest.Checked;
        }

        private void RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect_CheckedChanged(object sender, EventArgs e)
        {
            mKeywordUpdateProtection = (RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked) ? eKeywordUpdateMode.PROTECT : mKeywordUpdateProtection;
        }
        private void RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew_CheckedChanged(object sender, EventArgs e)
        {
            mKeywordUpdateProtection = (RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Checked) ? eKeywordUpdateMode.UPDATE_NEW : mKeywordUpdateProtection;
        }
        private void RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Force_CheckedChanged(object sender, EventArgs e)
        {
            mKeywordUpdateProtection = (RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Force.Checked) ? eKeywordUpdateMode.FORCE : mKeywordUpdateProtection;
        }
    }
}
