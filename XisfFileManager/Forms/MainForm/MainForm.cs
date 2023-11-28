using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using XisfFileManager.Files;
using XisfFileManager.Calculations;
using System.Reflection;
using XisfFileManager.Enums;
using XisfFileManager.DirectoryOperations;

namespace XisfFileManager
{
    // Event used to update the Calibration Tab Page Text Boxes and Progress Bar
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

        private void Button_KeywordUpdateTab_Cancel_Click(object sender, EventArgs e)
        {
            mBCancel = true;
        }

    }
}
