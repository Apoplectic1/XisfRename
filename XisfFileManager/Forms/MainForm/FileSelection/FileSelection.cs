using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XisfFileManager.Enums;
using XisfFileManager.Files;

namespace XisfFileManager
{
    public partial class MainForm
    {
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

            ClearCaptureSoftwareGroup();
            ClearTelescopeGroup();
            ClearCameraGroup();
            ClearFilterFrameTypeGroup();

            ProgressBar_FileSelection_ReadProgress.Value = 0;
            ProgressBar_KeywordUpdateTab_WriteProgress.Value = 0;

            // Exclude List
            // This list can contain any number of strings that will be used to exclude any full path (including a specified file name)
            // that contains the string BELOW the selected folder.

            List<string> mExcludeList = new List<string>()
                {
                    "Calibration",
                    "PreProcessing",
                    "Duplicates",
                    "Registered",
                    "Calibrated",
                    "Project"
                };

            DialogResult result = Files.DirectoryOperations.FindTargetfFiles(mFolderBrowseState, mExcludeList);

            if ((result != DialogResult.OK) || (Files.DirectoryOperations.FileInfoList.Count == 0))
            {
                MessageBox.Show("No Xisf Files Found", "Select a different folder");
                return;
            }

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

                    if (matchCounts.TryGetValue(baseItem, out int value))
                        matchCounts[baseItem] = ++value;
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
            TabControl_Updater.Enabled = true;
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
    }
}
