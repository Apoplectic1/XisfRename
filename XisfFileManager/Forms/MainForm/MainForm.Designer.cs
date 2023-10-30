namespace XisfFileManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ProgressBar_FileSelection_ReadProgress = new System.Windows.Forms.ProgressBar();
            GroupBox_FileSelection_SequenceOrder = new System.Windows.Forms.GroupBox();
            RadioButton_FileSelection_SequenceNumbering_WeightOnly = new System.Windows.Forms.RadioButton();
            GroupBox_FileSelection_Count = new System.Windows.Forms.GroupBox();
            RadioButton_FileSelection_Index_ByFilter = new System.Windows.Forms.RadioButton();
            RadioButton_FileSelection_Index_ByTime = new System.Windows.Forms.RadioButton();
            RadioButton_FileSelection_SequenceNumbering_IndexOnly = new System.Windows.Forms.RadioButton();
            RadioButton_FileSelection_SequenceNumbering_IndexWeight = new System.Windows.Forms.RadioButton();
            RadioButton_FileSelection_SequenceNumbering_WeightIndex = new System.Windows.Forms.RadioButton();
            GroupBox_FileSelection_DirectorySelection = new System.Windows.Forms.GroupBox();
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm = new System.Windows.Forms.ComboBox();
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames = new System.Windows.Forms.NumericUpDown();
            RadioButton_DirectorySelection_MastersOnly = new System.Windows.Forms.RadioButton();
            RadioButton_DirectorySelection_ExcludeMasters = new System.Windows.Forms.RadioButton();
            RadioButton_DirectorySelection_AlFiles = new System.Windows.Forms.RadioButton();
            CheckBox_FileSelection_DirectorySelection_Master = new System.Windows.Forms.CheckBox();
            Button_FileSelection_DirectorySelection_Browse = new System.Windows.Forms.Button();
            CheckBox_FileSelection_DirectorySelection_Recurse = new System.Windows.Forms.CheckBox();
            Button_FileSlection_Rename = new System.Windows.Forms.Button();
            GroupBox_FileSelection_Statistics = new System.Windows.Forms.GroupBox();
            Label_FileSelection_Statistics_Task = new System.Windows.Forms.Label();
            Label_FileSelection_Statistics_SubFrameOverhead = new System.Windows.Forms.Label();
            Label_FileSelection_Statistics_TempratureCompensation = new System.Windows.Forms.Label();
            Label_FileSelection_BrowseFileName = new System.Windows.Forms.Label();
            GroupBox_FileSelection = new System.Windows.Forms.GroupBox();
            CheckBox_FileSlection_NoTotals = new System.Windows.Forms.CheckBox();
            TabPage_SubFrameWeights = new System.Windows.Forms.TabPage();
            GroupBox_WeightCalculations = new System.Windows.Forms.GroupBox();
            GroupBox_StarResidual = new System.Windows.Forms.GroupBox();
            Label_StarResidualMinValue = new System.Windows.Forms.Label();
            Label_StarResidualMaxValue = new System.Windows.Forms.Label();
            Label_StarResidualMedianValue = new System.Windows.Forms.Label();
            Label_StarResidualMeanValue = new System.Windows.Forms.Label();
            Label_StarResidualSigmaValue = new System.Windows.Forms.Label();
            Label_StarResidualMin = new System.Windows.Forms.Label();
            Label_StarResidualMax = new System.Windows.Forms.Label();
            Label_StarResidualMedian = new System.Windows.Forms.Label();
            Label_StarResidualMean = new System.Windows.Forms.Label();
            Label_StarResidualSigma = new System.Windows.Forms.Label();
            Label_StarResidualRangeLow = new System.Windows.Forms.Label();
            Label_StarResidualRangeHigh = new System.Windows.Forms.Label();
            TextBox_StarResidualRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_StarResidualRangeLow = new System.Windows.Forms.TextBox();
            GroupBox_FwhmWeight = new System.Windows.Forms.GroupBox();
            Label_FwhmMinValue = new System.Windows.Forms.Label();
            Label_FwhmMaxValue = new System.Windows.Forms.Label();
            Label_FwhmMedianValue = new System.Windows.Forms.Label();
            Label_FwhmMeanValue = new System.Windows.Forms.Label();
            Label_FwhmSigmaValue = new System.Windows.Forms.Label();
            Label_FwhmMin = new System.Windows.Forms.Label();
            Label_FwhmMax = new System.Windows.Forms.Label();
            Label_FwhmMedian = new System.Windows.Forms.Label();
            Label_FwhmRangeLow = new System.Windows.Forms.Label();
            Label_FwhmMean = new System.Windows.Forms.Label();
            Label_FwhmRangeHigh = new System.Windows.Forms.Label();
            TextBox_FwhmRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_FwhmRangeLow = new System.Windows.Forms.TextBox();
            Label_FwhmSigma = new System.Windows.Forms.Label();
            GroupBox_StarsWeight = new System.Windows.Forms.GroupBox();
            Label_StarsMinValue = new System.Windows.Forms.Label();
            Label_StarsMaxValue = new System.Windows.Forms.Label();
            Label_StarsMedianValue = new System.Windows.Forms.Label();
            Label_StarsMeanValue = new System.Windows.Forms.Label();
            Label_StarsSigmaValue = new System.Windows.Forms.Label();
            Label_StarsMin = new System.Windows.Forms.Label();
            Label_StarsMax = new System.Windows.Forms.Label();
            Label_StarsMedian = new System.Windows.Forms.Label();
            Label_StarsMean = new System.Windows.Forms.Label();
            Label_StarsSigma = new System.Windows.Forms.Label();
            Label_StarRangeLow = new System.Windows.Forms.Label();
            Label_StarRangeHigh = new System.Windows.Forms.Label();
            TextBox_StarRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_StarRangeLow = new System.Windows.Forms.TextBox();
            GroupBox_EccentricityWeight = new System.Windows.Forms.GroupBox();
            Label_EccentricityMinValue = new System.Windows.Forms.Label();
            Label_EccentricityMaxValue = new System.Windows.Forms.Label();
            Label_EccentricityMedianValue = new System.Windows.Forms.Label();
            Label_EccentricityMeanValue = new System.Windows.Forms.Label();
            Label_EccentricitySigmaValue = new System.Windows.Forms.Label();
            Label_EccentricityMin = new System.Windows.Forms.Label();
            Label_EccentricityMax = new System.Windows.Forms.Label();
            Label_EccentricityMedian = new System.Windows.Forms.Label();
            Label_EccentricityMean = new System.Windows.Forms.Label();
            Label_EccentricitySigma = new System.Windows.Forms.Label();
            Label_EccentricityRangeLow = new System.Windows.Forms.Label();
            Label_EccentricityRangeHigh = new System.Windows.Forms.Label();
            TextBox_EccentricityRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_EccentricityRangeLow = new System.Windows.Forms.TextBox();
            GroupBox_AirMassWeight = new System.Windows.Forms.GroupBox();
            Label_AirMassMinValue = new System.Windows.Forms.Label();
            Label_AirMassMaxValue = new System.Windows.Forms.Label();
            Label_AirMassMedianValue = new System.Windows.Forms.Label();
            Label_AirMassMeanValue = new System.Windows.Forms.Label();
            Label_AirMassSigmaValue = new System.Windows.Forms.Label();
            TextBox_AirMassRangeLow = new System.Windows.Forms.TextBox();
            TextBox_AirMassRangeHigh = new System.Windows.Forms.TextBox();
            Label_AirMassMin = new System.Windows.Forms.Label();
            Label_AirMassMax = new System.Windows.Forms.Label();
            Label_AirMassMedian = new System.Windows.Forms.Label();
            Label_AirMassMean = new System.Windows.Forms.Label();
            Label_AirMassSigma = new System.Windows.Forms.Label();
            Label_FwhmMeanDeviationRangeLow = new System.Windows.Forms.Label();
            Label_FwhmMeanDeviationRangeHigh = new System.Windows.Forms.Label();
            GroupBox_NoiseWeight = new System.Windows.Forms.GroupBox();
            Label_NoiseMinValue = new System.Windows.Forms.Label();
            Label_NoiseMaxValue = new System.Windows.Forms.Label();
            Label_NoiseMedianValue = new System.Windows.Forms.Label();
            Label_NoiseMeanValue = new System.Windows.Forms.Label();
            Label_NoiseSigmaValue = new System.Windows.Forms.Label();
            Label_NoiseMin = new System.Windows.Forms.Label();
            Label_NoiseMax = new System.Windows.Forms.Label();
            Label_NoiseMedian = new System.Windows.Forms.Label();
            Label_NoiseMean = new System.Windows.Forms.Label();
            Label_NoiseSigma = new System.Windows.Forms.Label();
            Label_NoiseRangeLow = new System.Windows.Forms.Label();
            Label_NoiseRangeHigh = new System.Windows.Forms.Label();
            TextBox_NoiseRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_NoiseRangeLow = new System.Windows.Forms.TextBox();
            GroupBox_MedianWeight = new System.Windows.Forms.GroupBox();
            Label_MedianMinValue = new System.Windows.Forms.Label();
            Label_MedianMaxValue = new System.Windows.Forms.Label();
            Label_MedianMedianValue = new System.Windows.Forms.Label();
            Label_MedianMeanValue = new System.Windows.Forms.Label();
            Label_MedianSigmaValue = new System.Windows.Forms.Label();
            Label_MedianMin = new System.Windows.Forms.Label();
            Label_MedianMax = new System.Windows.Forms.Label();
            Label_MedianMedian = new System.Windows.Forms.Label();
            Label_MedianMean = new System.Windows.Forms.Label();
            Label_MedianSigma = new System.Windows.Forms.Label();
            Label_MedianRangeLow = new System.Windows.Forms.Label();
            Label_MedianRangeHigh = new System.Windows.Forms.Label();
            TextBox_MedianRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_MedianRangeLow = new System.Windows.Forms.TextBox();
            GroupBox_SnrWeight = new System.Windows.Forms.GroupBox();
            Label_SnrMinValue = new System.Windows.Forms.Label();
            Label_SnrMaxValue = new System.Windows.Forms.Label();
            Label_SnrMedianValue = new System.Windows.Forms.Label();
            Label_SnrMeanValue = new System.Windows.Forms.Label();
            Label_SnrSigmaValue = new System.Windows.Forms.Label();
            Label_SnrMin = new System.Windows.Forms.Label();
            Label_SnrMax = new System.Windows.Forms.Label();
            Label_SnrMedian = new System.Windows.Forms.Label();
            Label_SnrMean = new System.Windows.Forms.Label();
            Label_SnrSigma = new System.Windows.Forms.Label();
            Label_SnrRangeLow = new System.Windows.Forms.Label();
            Label_SnrRangeHigh = new System.Windows.Forms.Label();
            TextBox_SnrRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_SnrRangeLow = new System.Windows.Forms.TextBox();
            GroupBox_UpdateStatistics = new System.Windows.Forms.GroupBox();
            RadioButton_SetImageStatistics_CalculateWeights = new System.Windows.Forms.RadioButton();
            RadioButton_SetImageStatistics_RescaleWeights = new System.Windows.Forms.RadioButton();
            RadioButton_SetImageStatistics_KeepWeights = new System.Windows.Forms.RadioButton();
            Button_ReadSubFrameSelectorCsvFile = new System.Windows.Forms.Button();
            Label_UpdateStatistics = new System.Windows.Forms.Label();
            Label_UpdateStatisticsRangeHigh = new System.Windows.Forms.Label();
            TextBox_UpdateStatisticsRangeHigh = new System.Windows.Forms.TextBox();
            TextBox_UpdateStatisticsRangeLow = new System.Windows.Forms.TextBox();
            Label_UpdateStatisticsRangeLow = new System.Windows.Forms.Label();
            GroupBox_InitialRejectionCriteria = new System.Windows.Forms.GroupBox();
            NumericUpDown_Rejection_Snr = new System.Windows.Forms.NumericUpDown();
            Label_Rejection_SNR = new System.Windows.Forms.Label();
            NumericUpDown_Rejection_StarResidual = new System.Windows.Forms.NumericUpDown();
            Label_Rejection_StarResidual = new System.Windows.Forms.Label();
            NumericUpDown_Rejection_Stars = new System.Windows.Forms.NumericUpDown();
            NumericUpDown_Rejection_AirMass = new System.Windows.Forms.NumericUpDown();
            Label_Rejection_Stars = new System.Windows.Forms.Label();
            Label_Rejection_AirMass = new System.Windows.Forms.Label();
            NumericUpDown_Rejection_Noise = new System.Windows.Forms.NumericUpDown();
            Label_Rejection_Noise = new System.Windows.Forms.Label();
            Button_Rejection_RejectionSet = new System.Windows.Forms.Button();
            NumericUpDown_Rejection_Median = new System.Windows.Forms.NumericUpDown();
            Label_Rejection_Median = new System.Windows.Forms.Label();
            NumericUpDown_Rejection_Eccentricity = new System.Windows.Forms.NumericUpDown();
            NumericUpDown_Rejection_FWHM = new System.Windows.Forms.NumericUpDown();
            TextBox_Rejection_Total = new System.Windows.Forms.TextBox();
            Label_Rejection_Total = new System.Windows.Forms.Label();
            Label_Rejection_Eccentricity = new System.Windows.Forms.Label();
            Label_Rejection_FWHM = new System.Windows.Forms.Label();
            TabPage_Calibration = new System.Windows.Forms.TabPage();
            CheckBox_CalibrationTab_CreateNew = new System.Windows.Forms.CheckBox();
            TreeView_CalibrationTab_TargetFileTree = new System.Windows.Forms.TreeView();
            TextBox_CalibrationTab_Messgaes = new System.Windows.Forms.TextBox();
            GroupBox_CalibrationTab_MatchingTolerance = new System.Windows.Forms.GroupBox();
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance = new System.Windows.Forms.Label();
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance = new System.Windows.Forms.Label();
            Label_CalibrationTab_MatchingTolerance_GainTolerance = new System.Windows.Forms.Label();
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance = new System.Windows.Forms.Label();
            Label_CalibrationTab_MatchingTolerance_Percentage = new System.Windows.Forms.Label();
            TextBox_CalibrationTab_MatchingTolerance_Temperature = new System.Windows.Forms.TextBox();
            TextBox_CalibrationTab_MatchingTolerance_Offset = new System.Windows.Forms.TextBox();
            TextBox_CalibrationTab_MatchingTolerance_Gain = new System.Windows.Forms.TextBox();
            Label_CalibrationTab_MatchingTolerance_Temperature = new System.Windows.Forms.Label();
            Label_CalibrationTab_MatchingTolerance_Offset = new System.Windows.Forms.Label();
            Label_CalibrationTab_MatchingTolerance_Gain = new System.Windows.Forms.Label();
            Label_CalibrationTab_MatchingTolerance_Exposure = new System.Windows.Forms.Label();
            TextBox_CalibrationTab_MatchingTolerance_Exposure = new System.Windows.Forms.TextBox();
            Label_CalibrationTab_TotalFiles = new System.Windows.Forms.Label();
            ProgressBar_CalibrationTab = new System.Windows.Forms.ProgressBar();
            Label_CalibrationTab_ReadFileName = new System.Windows.Forms.Label();
            Button_CalibrationTab_CreateCalibrationDirectory = new System.Windows.Forms.Button();
            Button_CalibrationTab_MatchCalibrationFrames = new System.Windows.Forms.Button();
            Button_CalibrationTab_FindCalibrationFrames = new System.Windows.Forms.Button();
            TabPage_KeywordUpdate = new System.Windows.Forms.TabPage();
            Label_KeywordUpdateTab_FileName = new System.Windows.Forms.Label();
            ProgressBar_KeywordUpdateTab_WriteProgress = new System.Windows.Forms.ProgressBar();
            GroupBox_KeywordUpdateTab_CaptureSoftware = new System.Windows.Forms.GroupBox();
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA = new System.Windows.Forms.RadioButton();
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile = new System.Windows.Forms.Button();
            Button_KeywordUpdateTab_CaptureSoftware_SetAll = new System.Windows.Forms.Button();
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX = new System.Windows.Forms.RadioButton();
            GroupBox_KeywordUpdateTab_Telescope = new System.Windows.Forms.GroupBox();
            TextBox_KeywordUpdateTab_Telescope_FocalLength = new System.Windows.Forms.ComboBox();
            Label_KeywordUpdateTab_Telescope_FocalLength = new System.Windows.Forms.Label();
            Button_KeywordUpdateTab_Telescope_SetByFile = new System.Windows.Forms.Button();
            Button_KeywordUpdateTab_Telescope_SetAll = new System.Windows.Forms.Button();
            CheckBox_KeywordUpdateTab_Telescope_Riccardi = new System.Windows.Forms.CheckBox();
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254 = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150 = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_Telescope_APM107 = new System.Windows.Forms.RadioButton();
            GroupBox_KeywordUpdateTab_SubFrameKeywords = new System.Windows.Forms.GroupBox();
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName = new System.Windows.Forms.CheckBox();
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues = new System.Windows.Forms.RadioButton();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue = new System.Windows.Forms.ComboBox();
            GroupBox_SubFrameKeywords_CalibrationFiles = new System.Windows.Forms.GroupBox();
            Button_SubFrameKeywords_CalibrationFiles_ClearAll = new System.Windows.Forms.Button();
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection = new System.Windows.Forms.GroupBox();
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All = new System.Windows.Forms.RadioButton();
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect = new System.Windows.Forms.CheckBox();
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords = new System.Windows.Forms.CheckBox();
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights = new System.Windows.Forms.GroupBox();
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration = new System.Windows.Forms.RadioButton();
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove = new System.Windows.Forms.Button();
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All = new System.Windows.Forms.RadioButton();
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords = new System.Windows.Forms.Label();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords = new System.Windows.Forms.ComboBox();
            Button_KeywordUpdateTab_SubFrameKeywords_Delete = new System.Windows.Forms.Button();
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace = new System.Windows.Forms.Button();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName = new System.Windows.Forms.ComboBox();
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName = new System.Windows.Forms.CheckBox();
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords = new System.Windows.Forms.Button();
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames = new System.Windows.Forms.ComboBox();
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName = new System.Windows.Forms.Label();
            GroupBox_KeywordUpdateTab_Camera = new System.Windows.Forms.GroupBox();
            ComboBox_KeywordUpdateTab_Camera_A144Binning = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Q178Binning = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z183Binning = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z533Binning = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Q178Offset = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z183Offset = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z533Offset = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Q178Gain = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z183Gain = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z533Gain = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_A144Seconds = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds = new System.Windows.Forms.ComboBox();
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds = new System.Windows.Forms.ComboBox();
            Label_KeywordUpdateTab_Camera_ToggleNBPreset = new System.Windows.Forms.Label();
            Label_KeywordUpdateTab_Camera_Seconds = new System.Windows.Forms.Label();
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB = new System.Windows.Forms.Button();
            CheckBox_KeywordUpdateTab_Camera_A144 = new System.Windows.Forms.CheckBox();
            CheckBox_KeywordUpdateTab_Camera_Q178 = new System.Windows.Forms.CheckBox();
            CheckBox_KeywordUpdateTab_Camera_Z183 = new System.Windows.Forms.CheckBox();
            CheckBox_KeywordUpdateTab_Camera_Z533 = new System.Windows.Forms.CheckBox();
            Label_KeywordUpdateTab_Camera_Binning = new System.Windows.Forms.Label();
            Label_KeywordUpdateTab_Camera_SensorTemp = new System.Windows.Forms.Label();
            Label_KeywordUpdateTab_Camera_Camera = new System.Windows.Forms.Label();
            Button_KeywordUpdateTab_Camera_SetByFile = new System.Windows.Forms.Button();
            Button_KeywordUpdateTab_Camera_SetAll = new System.Windows.Forms.Button();
            Label_KeywordUpdateTab_Camera_A144Gain = new System.Windows.Forms.Label();
            Label_KeywordUpdateTab_Camera_Offset = new System.Windows.Forms.Label();
            Label_KeywordUpdateTab_Camera_Gain = new System.Windows.Forms.Label();
            GroupBox_KeywordUpdateTab_ImageType = new System.Windows.Forms.GroupBox();
            Button_KeywordUpdateTab_ImageType_SetByFile = new System.Windows.Forms.Button();
            Button_KeywordUpdateTab_ImageType_SetAll = new System.Windows.Forms.Button();
            GroupBox_KeywordUpdateTab_ImageType_Frame = new System.Windows.Forms.GroupBox();
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster = new System.Windows.Forms.Button();
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light = new System.Windows.Forms.RadioButton();
            GroupBox_KeywordUpdateTab_ImageType_Filter = new System.Windows.Forms.GroupBox();
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2 = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green = new System.Windows.Forms.RadioButton();
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3 = new System.Windows.Forms.RadioButton();
            TabControl_Update_TargetScheduler = new System.Windows.Forms.TabControl();
            TabPage_TargetScheduler = new System.Windows.Forms.TabPage();
            GroupBox_SchedulerTab_Project = new System.Windows.Forms.GroupBox();
            CheckBox_Project_Active = new System.Windows.Forms.CheckBox();
            GroupBox_Project_Priority = new System.Windows.Forms.GroupBox();
            RadioButton_ProjectPriority_High = new System.Windows.Forms.RadioButton();
            RadioButton_ProjectPriority_Normal = new System.Windows.Forms.RadioButton();
            RadioButton_ProjectPriority_Low = new System.Windows.Forms.RadioButton();
            Label_SchedulerTab_PlansText = new System.Windows.Forms.Label();
            TreeView_SchedulerTab_PlansTree = new System.Windows.Forms.TreeView();
            Label_SchedulerTab_TargetsText = new System.Windows.Forms.Label();
            TreeView_SchedulerTab_TargetTree = new System.Windows.Forms.TreeView();
            Label_SchedulerTab_ProjectsText = new System.Windows.Forms.Label();
            Label_SchedulerTab_ProfilesText = new System.Windows.Forms.Label();
            TreeView_SchedulerTab_ProjectTree = new System.Windows.Forms.TreeView();
            TreeView_SchedulerTab_ProfileTree = new System.Windows.Forms.TreeView();
            Button_SchedulerTab_OpenDatabase = new System.Windows.Forms.Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            GroupBox_FileSelection_SequenceOrder.SuspendLayout();
            GroupBox_FileSelection_Count.SuspendLayout();
            GroupBox_FileSelection_DirectorySelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_FileSelection_DirectorySelection_TotalFrames).BeginInit();
            GroupBox_FileSelection_Statistics.SuspendLayout();
            GroupBox_FileSelection.SuspendLayout();
            TabPage_SubFrameWeights.SuspendLayout();
            GroupBox_WeightCalculations.SuspendLayout();
            GroupBox_StarResidual.SuspendLayout();
            GroupBox_FwhmWeight.SuspendLayout();
            GroupBox_StarsWeight.SuspendLayout();
            GroupBox_EccentricityWeight.SuspendLayout();
            GroupBox_AirMassWeight.SuspendLayout();
            GroupBox_NoiseWeight.SuspendLayout();
            GroupBox_MedianWeight.SuspendLayout();
            GroupBox_SnrWeight.SuspendLayout();
            GroupBox_UpdateStatistics.SuspendLayout();
            GroupBox_InitialRejectionCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Snr).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_StarResidual).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Stars).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_AirMass).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Noise).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Median).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Eccentricity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_FWHM).BeginInit();
            TabPage_Calibration.SuspendLayout();
            GroupBox_CalibrationTab_MatchingTolerance.SuspendLayout();
            TabPage_KeywordUpdate.SuspendLayout();
            GroupBox_KeywordUpdateTab_CaptureSoftware.SuspendLayout();
            GroupBox_KeywordUpdateTab_Telescope.SuspendLayout();
            GroupBox_KeywordUpdateTab_SubFrameKeywords.SuspendLayout();
            GroupBox_SubFrameKeywords_CalibrationFiles.SuspendLayout();
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.SuspendLayout();
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.SuspendLayout();
            GroupBox_KeywordUpdateTab_Camera.SuspendLayout();
            GroupBox_KeywordUpdateTab_ImageType.SuspendLayout();
            GroupBox_KeywordUpdateTab_ImageType_Frame.SuspendLayout();
            GroupBox_KeywordUpdateTab_ImageType_Filter.SuspendLayout();
            TabControl_Update_TargetScheduler.SuspendLayout();
            TabPage_TargetScheduler.SuspendLayout();
            GroupBox_SchedulerTab_Project.SuspendLayout();
            GroupBox_Project_Priority.SuspendLayout();
            SuspendLayout();
            // 
            // ProgressBar_FileSelection_ReadProgress
            // 
            ProgressBar_FileSelection_ReadProgress.Location = new System.Drawing.Point(20, 197);
            ProgressBar_FileSelection_ReadProgress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ProgressBar_FileSelection_ReadProgress.Name = "ProgressBar_FileSelection_ReadProgress";
            ProgressBar_FileSelection_ReadProgress.Size = new System.Drawing.Size(1099, 13);
            ProgressBar_FileSelection_ReadProgress.Step = 1;
            ProgressBar_FileSelection_ReadProgress.TabIndex = 1;
            // 
            // GroupBox_FileSelection_SequenceOrder
            // 
            GroupBox_FileSelection_SequenceOrder.Controls.Add(RadioButton_FileSelection_SequenceNumbering_WeightOnly);
            GroupBox_FileSelection_SequenceOrder.Controls.Add(GroupBox_FileSelection_Count);
            GroupBox_FileSelection_SequenceOrder.Controls.Add(RadioButton_FileSelection_SequenceNumbering_IndexOnly);
            GroupBox_FileSelection_SequenceOrder.Controls.Add(RadioButton_FileSelection_SequenceNumbering_IndexWeight);
            GroupBox_FileSelection_SequenceOrder.Controls.Add(RadioButton_FileSelection_SequenceNumbering_WeightIndex);
            GroupBox_FileSelection_SequenceOrder.Location = new System.Drawing.Point(887, 12);
            GroupBox_FileSelection_SequenceOrder.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_SequenceOrder.Name = "GroupBox_FileSelection_SequenceOrder";
            GroupBox_FileSelection_SequenceOrder.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_SequenceOrder.Size = new System.Drawing.Size(232, 130);
            GroupBox_FileSelection_SequenceOrder.TabIndex = 3;
            GroupBox_FileSelection_SequenceOrder.TabStop = false;
            GroupBox_FileSelection_SequenceOrder.Text = "Sequence Numbering";
            // 
            // RadioButton_FileSelection_SequenceNumbering_WeightOnly
            // 
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.AutoSize = true;
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.Location = new System.Drawing.Point(27, 38);
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.Name = "RadioButton_FileSelection_SequenceNumbering_WeightOnly";
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.Size = new System.Drawing.Size(91, 19);
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.TabIndex = 3;
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.Text = "Weight Only";
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.UseVisualStyleBackColor = true;
            RadioButton_FileSelection_SequenceNumbering_WeightOnly.CheckedChanged += RadioButton_Weight_CheckedChanged;
            // 
            // GroupBox_FileSelection_Count
            // 
            GroupBox_FileSelection_Count.Controls.Add(RadioButton_FileSelection_Index_ByFilter);
            GroupBox_FileSelection_Count.Controls.Add(RadioButton_FileSelection_Index_ByTime);
            GroupBox_FileSelection_Count.Location = new System.Drawing.Point(19, 65);
            GroupBox_FileSelection_Count.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_Count.Name = "GroupBox_FileSelection_Count";
            GroupBox_FileSelection_Count.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_Count.Size = new System.Drawing.Size(205, 59);
            GroupBox_FileSelection_Count.TabIndex = 16;
            GroupBox_FileSelection_Count.TabStop = false;
            GroupBox_FileSelection_Count.Text = "Index";
            // 
            // RadioButton_FileSelection_Index_ByFilter
            // 
            RadioButton_FileSelection_Index_ByFilter.AutoSize = true;
            RadioButton_FileSelection_Index_ByFilter.Checked = true;
            RadioButton_FileSelection_Index_ByFilter.Location = new System.Drawing.Point(33, 24);
            RadioButton_FileSelection_Index_ByFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_FileSelection_Index_ByFilter.Name = "RadioButton_FileSelection_Index_ByFilter";
            RadioButton_FileSelection_Index_ByFilter.Size = new System.Drawing.Size(67, 19);
            RadioButton_FileSelection_Index_ByFilter.TabIndex = 6;
            RadioButton_FileSelection_Index_ByFilter.TabStop = true;
            RadioButton_FileSelection_Index_ByFilter.Text = "By Filter";
            RadioButton_FileSelection_Index_ByFilter.UseVisualStyleBackColor = true;
            // 
            // RadioButton_FileSelection_Index_ByTime
            // 
            RadioButton_FileSelection_Index_ByTime.AutoSize = true;
            RadioButton_FileSelection_Index_ByTime.Location = new System.Drawing.Point(110, 24);
            RadioButton_FileSelection_Index_ByTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_FileSelection_Index_ByTime.Name = "RadioButton_FileSelection_Index_ByTime";
            RadioButton_FileSelection_Index_ByTime.Size = new System.Drawing.Size(67, 19);
            RadioButton_FileSelection_Index_ByTime.TabIndex = 5;
            RadioButton_FileSelection_Index_ByTime.Text = "By Time";
            RadioButton_FileSelection_Index_ByTime.UseVisualStyleBackColor = true;
            // 
            // RadioButton_FileSelection_SequenceNumbering_IndexOnly
            // 
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.AutoSize = true;
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.Checked = true;
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.Location = new System.Drawing.Point(27, 16);
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.Name = "RadioButton_FileSelection_SequenceNumbering_IndexOnly";
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.Size = new System.Drawing.Size(82, 19);
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.TabIndex = 2;
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.TabStop = true;
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.Text = "Index Only";
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.UseVisualStyleBackColor = true;
            RadioButton_FileSelection_SequenceNumbering_IndexOnly.CheckedChanged += RadioButton_Index_CheckedChanged;
            // 
            // RadioButton_FileSelection_SequenceNumbering_IndexWeight
            // 
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.AutoSize = true;
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.Location = new System.Drawing.Point(122, 16);
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.Name = "RadioButton_FileSelection_SequenceNumbering_IndexWeight";
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.Size = new System.Drawing.Size(95, 19);
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.TabIndex = 1;
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.Text = "Index Weight";
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.UseVisualStyleBackColor = true;
            RadioButton_FileSelection_SequenceNumbering_IndexWeight.CheckedChanged += RadioButton_IndexWeight_CheckedChanged;
            // 
            // RadioButton_FileSelection_SequenceNumbering_WeightIndex
            // 
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.AutoSize = true;
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.Location = new System.Drawing.Point(122, 38);
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.Name = "RadioButton_FileSelection_SequenceNumbering_WeightIndex";
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.Size = new System.Drawing.Size(95, 19);
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.TabIndex = 0;
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.Text = "Weight Index";
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.UseVisualStyleBackColor = true;
            RadioButton_FileSelection_SequenceNumbering_WeightIndex.CheckedChanged += RadioButton_WeightIndex_CheckedChanged;
            // 
            // GroupBox_FileSelection_DirectorySelection
            // 
            GroupBox_FileSelection_DirectorySelection.Controls.Add(ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm);
            GroupBox_FileSelection_DirectorySelection.Controls.Add(NumericUpDown_FileSelection_DirectorySelection_TotalFrames);
            GroupBox_FileSelection_DirectorySelection.Controls.Add(RadioButton_DirectorySelection_MastersOnly);
            GroupBox_FileSelection_DirectorySelection.Controls.Add(RadioButton_DirectorySelection_ExcludeMasters);
            GroupBox_FileSelection_DirectorySelection.Controls.Add(RadioButton_DirectorySelection_AlFiles);
            GroupBox_FileSelection_DirectorySelection.Controls.Add(CheckBox_FileSelection_DirectorySelection_Master);
            GroupBox_FileSelection_DirectorySelection.Controls.Add(Button_FileSelection_DirectorySelection_Browse);
            GroupBox_FileSelection_DirectorySelection.Controls.Add(CheckBox_FileSelection_DirectorySelection_Recurse);
            GroupBox_FileSelection_DirectorySelection.Location = new System.Drawing.Point(20, 23);
            GroupBox_FileSelection_DirectorySelection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_DirectorySelection.Name = "GroupBox_FileSelection_DirectorySelection";
            GroupBox_FileSelection_DirectorySelection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_DirectorySelection.Size = new System.Drawing.Size(278, 132);
            GroupBox_FileSelection_DirectorySelection.TabIndex = 7;
            GroupBox_FileSelection_DirectorySelection.TabStop = false;
            GroupBox_FileSelection_DirectorySelection.Text = "Directory Selection";
            // 
            // ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm
            // 
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Enabled = false;
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.FormattingEnabled = true;
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Items.AddRange(new object[] { "MM", "PC", "SC", "WSC", "ASC", "LFC", "ESD", "RCR" });
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Location = new System.Drawing.Point(194, 102);
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Name = "ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm";
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.Size = new System.Drawing.Size(76, 23);
            ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm.TabIndex = 8;
            // 
            // NumericUpDown_FileSelection_DirectorySelection_TotalFrames
            // 
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.Enabled = false;
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.Location = new System.Drawing.Point(125, 102);
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.Name = "NumericUpDown_FileSelection_DirectorySelection_TotalFrames";
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.Size = new System.Drawing.Size(62, 23);
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.TabIndex = 7;
            NumericUpDown_FileSelection_DirectorySelection_TotalFrames.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RadioButton_DirectorySelection_MastersOnly
            // 
            RadioButton_DirectorySelection_MastersOnly.AutoSize = true;
            RadioButton_DirectorySelection_MastersOnly.Location = new System.Drawing.Point(156, 80);
            RadioButton_DirectorySelection_MastersOnly.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_DirectorySelection_MastersOnly.Name = "RadioButton_DirectorySelection_MastersOnly";
            RadioButton_DirectorySelection_MastersOnly.Size = new System.Drawing.Size(94, 19);
            RadioButton_DirectorySelection_MastersOnly.TabIndex = 6;
            RadioButton_DirectorySelection_MastersOnly.Text = "Masters Only";
            RadioButton_DirectorySelection_MastersOnly.UseVisualStyleBackColor = true;
            RadioButton_DirectorySelection_MastersOnly.CheckedChanged += RadioButton_DirectorySelection_MastersOnly_CheckedChanged;
            // 
            // RadioButton_DirectorySelection_ExcludeMasters
            // 
            RadioButton_DirectorySelection_ExcludeMasters.AutoSize = true;
            RadioButton_DirectorySelection_ExcludeMasters.Checked = true;
            RadioButton_DirectorySelection_ExcludeMasters.Location = new System.Drawing.Point(156, 54);
            RadioButton_DirectorySelection_ExcludeMasters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_DirectorySelection_ExcludeMasters.Name = "RadioButton_DirectorySelection_ExcludeMasters";
            RadioButton_DirectorySelection_ExcludeMasters.Size = new System.Drawing.Size(110, 19);
            RadioButton_DirectorySelection_ExcludeMasters.TabIndex = 5;
            RadioButton_DirectorySelection_ExcludeMasters.TabStop = true;
            RadioButton_DirectorySelection_ExcludeMasters.Text = "Exclude Masters";
            RadioButton_DirectorySelection_ExcludeMasters.UseVisualStyleBackColor = true;
            RadioButton_DirectorySelection_ExcludeMasters.CheckedChanged += RadioButton_DirectorySelection_ExcludeMasters_CheckedChanged;
            // 
            // RadioButton_DirectorySelection_AlFiles
            // 
            RadioButton_DirectorySelection_AlFiles.AutoSize = true;
            RadioButton_DirectorySelection_AlFiles.Location = new System.Drawing.Point(156, 29);
            RadioButton_DirectorySelection_AlFiles.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_DirectorySelection_AlFiles.Name = "RadioButton_DirectorySelection_AlFiles";
            RadioButton_DirectorySelection_AlFiles.Size = new System.Drawing.Size(65, 19);
            RadioButton_DirectorySelection_AlFiles.TabIndex = 4;
            RadioButton_DirectorySelection_AlFiles.Text = "All Files";
            RadioButton_DirectorySelection_AlFiles.UseVisualStyleBackColor = true;
            RadioButton_DirectorySelection_AlFiles.CheckedChanged += RadioButton_DirectorySelection_AllFiles_CheckedChanged;
            // 
            // CheckBox_FileSelection_DirectorySelection_Master
            // 
            CheckBox_FileSelection_DirectorySelection_Master.AutoSize = true;
            CheckBox_FileSelection_DirectorySelection_Master.Location = new System.Drawing.Point(14, 104);
            CheckBox_FileSelection_DirectorySelection_Master.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_FileSelection_DirectorySelection_Master.Name = "CheckBox_FileSelection_DirectorySelection_Master";
            CheckBox_FileSelection_DirectorySelection_Master.Size = new System.Drawing.Size(95, 19);
            CheckBox_FileSelection_DirectorySelection_Master.TabIndex = 3;
            CheckBox_FileSelection_DirectorySelection_Master.Text = "Set as Master";
            CheckBox_FileSelection_DirectorySelection_Master.UseVisualStyleBackColor = true;
            CheckBox_FileSelection_DirectorySelection_Master.CheckedChanged += CheckBox_Master_CheckedChanged;
            // 
            // Button_FileSelection_DirectorySelection_Browse
            // 
            Button_FileSelection_DirectorySelection_Browse.Location = new System.Drawing.Point(14, 24);
            Button_FileSelection_DirectorySelection_Browse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_FileSelection_DirectorySelection_Browse.Name = "Button_FileSelection_DirectorySelection_Browse";
            Button_FileSelection_DirectorySelection_Browse.Size = new System.Drawing.Size(88, 27);
            Button_FileSelection_DirectorySelection_Browse.TabIndex = 0;
            Button_FileSelection_DirectorySelection_Browse.Text = "Browse";
            Button_FileSelection_DirectorySelection_Browse.UseVisualStyleBackColor = true;
            Button_FileSelection_DirectorySelection_Browse.Click += Button_Browse_Click;
            // 
            // CheckBox_FileSelection_DirectorySelection_Recurse
            // 
            CheckBox_FileSelection_DirectorySelection_Recurse.AutoSize = true;
            CheckBox_FileSelection_DirectorySelection_Recurse.Checked = true;
            CheckBox_FileSelection_DirectorySelection_Recurse.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox_FileSelection_DirectorySelection_Recurse.Location = new System.Drawing.Point(14, 60);
            CheckBox_FileSelection_DirectorySelection_Recurse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_FileSelection_DirectorySelection_Recurse.Name = "CheckBox_FileSelection_DirectorySelection_Recurse";
            CheckBox_FileSelection_DirectorySelection_Recurse.Size = new System.Drawing.Size(126, 19);
            CheckBox_FileSelection_DirectorySelection_Recurse.TabIndex = 2;
            CheckBox_FileSelection_DirectorySelection_Recurse.Text = "Recurse Directories";
            CheckBox_FileSelection_DirectorySelection_Recurse.UseVisualStyleBackColor = true;
            // 
            // Button_FileSlection_Rename
            // 
            Button_FileSlection_Rename.Location = new System.Drawing.Point(892, 155);
            Button_FileSlection_Rename.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_FileSlection_Rename.Name = "Button_FileSlection_Rename";
            Button_FileSlection_Rename.Size = new System.Drawing.Size(145, 27);
            Button_FileSlection_Rename.TabIndex = 4;
            Button_FileSlection_Rename.Text = "Rename XISF Files";
            Button_FileSlection_Rename.UseVisualStyleBackColor = true;
            Button_FileSlection_Rename.Click += Button_Rename_Click;
            // 
            // GroupBox_FileSelection_Statistics
            // 
            GroupBox_FileSelection_Statistics.Controls.Add(Label_FileSelection_Statistics_Task);
            GroupBox_FileSelection_Statistics.Controls.Add(Label_FileSelection_Statistics_SubFrameOverhead);
            GroupBox_FileSelection_Statistics.Controls.Add(Label_FileSelection_Statistics_TempratureCompensation);
            GroupBox_FileSelection_Statistics.Location = new System.Drawing.Point(308, 23);
            GroupBox_FileSelection_Statistics.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_Statistics.Name = "GroupBox_FileSelection_Statistics";
            GroupBox_FileSelection_Statistics.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection_Statistics.Size = new System.Drawing.Size(568, 128);
            GroupBox_FileSelection_Statistics.TabIndex = 20;
            GroupBox_FileSelection_Statistics.TabStop = false;
            GroupBox_FileSelection_Statistics.Text = "Statistics";
            // 
            // Label_FileSelection_Statistics_Task
            // 
            Label_FileSelection_Statistics_Task.AutoSize = true;
            Label_FileSelection_Statistics_Task.Location = new System.Drawing.Point(16, 23);
            Label_FileSelection_Statistics_Task.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FileSelection_Statistics_Task.Name = "Label_FileSelection_Statistics_Task";
            Label_FileSelection_Statistics_Task.Size = new System.Drawing.Size(95, 15);
            Label_FileSelection_Statistics_Task.TabIndex = 12;
            Label_FileSelection_Statistics_Task.Text = "Operation Status";
            // 
            // Label_FileSelection_Statistics_SubFrameOverhead
            // 
            Label_FileSelection_Statistics_SubFrameOverhead.AutoSize = true;
            Label_FileSelection_Statistics_SubFrameOverhead.Location = new System.Drawing.Point(16, 86);
            Label_FileSelection_Statistics_SubFrameOverhead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FileSelection_Statistics_SubFrameOverhead.Name = "Label_FileSelection_Statistics_SubFrameOverhead";
            Label_FileSelection_Statistics_SubFrameOverhead.Size = new System.Drawing.Size(200, 15);
            Label_FileSelection_Statistics_SubFrameOverhead.TabIndex = 14;
            Label_FileSelection_Statistics_SubFrameOverhead.Text = "SubFrame Overhead: Not Computed";
            // 
            // Label_FileSelection_Statistics_TempratureCompensation
            // 
            Label_FileSelection_Statistics_TempratureCompensation.AutoSize = true;
            Label_FileSelection_Statistics_TempratureCompensation.Location = new System.Drawing.Point(16, 51);
            Label_FileSelection_Statistics_TempratureCompensation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FileSelection_Statistics_TempratureCompensation.Name = "Label_FileSelection_Statistics_TempratureCompensation";
            Label_FileSelection_Statistics_TempratureCompensation.Size = new System.Drawing.Size(220, 15);
            Label_FileSelection_Statistics_TempratureCompensation.TabIndex = 13;
            Label_FileSelection_Statistics_TempratureCompensation.Text = "Temperature Coefficient: Not Computed";
            // 
            // Label_FileSelection_BrowseFileName
            // 
            Label_FileSelection_BrowseFileName.AutoSize = true;
            Label_FileSelection_BrowseFileName.Location = new System.Drawing.Point(20, 160);
            Label_FileSelection_BrowseFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FileSelection_BrowseFileName.Name = "Label_FileSelection_BrowseFileName";
            Label_FileSelection_BrowseFileName.Size = new System.Drawing.Size(101, 15);
            Label_FileSelection_BrowseFileName.TabIndex = 21;
            Label_FileSelection_BrowseFileName.Text = "Browse File Name";
            // 
            // GroupBox_FileSelection
            // 
            GroupBox_FileSelection.Controls.Add(CheckBox_FileSlection_NoTotals);
            GroupBox_FileSelection.Controls.Add(Label_FileSelection_BrowseFileName);
            GroupBox_FileSelection.Controls.Add(GroupBox_FileSelection_Statistics);
            GroupBox_FileSelection.Controls.Add(Button_FileSlection_Rename);
            GroupBox_FileSelection.Controls.Add(GroupBox_FileSelection_DirectorySelection);
            GroupBox_FileSelection.Controls.Add(GroupBox_FileSelection_SequenceOrder);
            GroupBox_FileSelection.Controls.Add(ProgressBar_FileSelection_ReadProgress);
            GroupBox_FileSelection.Location = new System.Drawing.Point(14, 6);
            GroupBox_FileSelection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection.Name = "GroupBox_FileSelection";
            GroupBox_FileSelection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FileSelection.Size = new System.Drawing.Size(1142, 226);
            GroupBox_FileSelection.TabIndex = 19;
            GroupBox_FileSelection.TabStop = false;
            GroupBox_FileSelection.Text = "File Selection";
            // 
            // CheckBox_FileSlection_NoTotals
            // 
            CheckBox_FileSlection_NoTotals.AutoSize = true;
            CheckBox_FileSlection_NoTotals.Location = new System.Drawing.Point(1048, 160);
            CheckBox_FileSlection_NoTotals.Name = "CheckBox_FileSlection_NoTotals";
            CheckBox_FileSlection_NoTotals.Size = new System.Drawing.Size(75, 19);
            CheckBox_FileSlection_NoTotals.TabIndex = 22;
            CheckBox_FileSlection_NoTotals.Text = "No Totals";
            CheckBox_FileSlection_NoTotals.UseVisualStyleBackColor = true;
            // 
            // TabPage_SubFrameWeights
            // 
            TabPage_SubFrameWeights.BackColor = System.Drawing.SystemColors.Control;
            TabPage_SubFrameWeights.Controls.Add(GroupBox_WeightCalculations);
            TabPage_SubFrameWeights.Controls.Add(GroupBox_UpdateStatistics);
            TabPage_SubFrameWeights.Controls.Add(GroupBox_InitialRejectionCriteria);
            TabPage_SubFrameWeights.Location = new System.Drawing.Point(4, 24);
            TabPage_SubFrameWeights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_SubFrameWeights.Name = "TabPage_SubFrameWeights";
            TabPage_SubFrameWeights.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_SubFrameWeights.Size = new System.Drawing.Size(1139, 498);
            TabPage_SubFrameWeights.TabIndex = 2;
            TabPage_SubFrameWeights.Text = "SubFrame Weights";
            // 
            // GroupBox_WeightCalculations
            // 
            GroupBox_WeightCalculations.Controls.Add(GroupBox_StarResidual);
            GroupBox_WeightCalculations.Controls.Add(GroupBox_FwhmWeight);
            GroupBox_WeightCalculations.Controls.Add(GroupBox_StarsWeight);
            GroupBox_WeightCalculations.Controls.Add(GroupBox_EccentricityWeight);
            GroupBox_WeightCalculations.Controls.Add(GroupBox_AirMassWeight);
            GroupBox_WeightCalculations.Controls.Add(GroupBox_NoiseWeight);
            GroupBox_WeightCalculations.Controls.Add(GroupBox_MedianWeight);
            GroupBox_WeightCalculations.Controls.Add(GroupBox_SnrWeight);
            GroupBox_WeightCalculations.Location = new System.Drawing.Point(36, 242);
            GroupBox_WeightCalculations.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_WeightCalculations.Name = "GroupBox_WeightCalculations";
            GroupBox_WeightCalculations.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_WeightCalculations.Size = new System.Drawing.Size(1063, 290);
            GroupBox_WeightCalculations.TabIndex = 27;
            GroupBox_WeightCalculations.TabStop = false;
            GroupBox_WeightCalculations.Text = "Weight Caluculations";
            // 
            // GroupBox_StarResidual
            // 
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMinValue);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMaxValue);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMedianValue);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMeanValue);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualSigmaValue);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMin);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMax);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMedian);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualMean);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualSigma);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualRangeLow);
            GroupBox_StarResidual.Controls.Add(Label_StarResidualRangeHigh);
            GroupBox_StarResidual.Controls.Add(TextBox_StarResidualRangeHigh);
            GroupBox_StarResidual.Controls.Add(TextBox_StarResidualRangeLow);
            GroupBox_StarResidual.Location = new System.Drawing.Point(542, 158);
            GroupBox_StarResidual.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_StarResidual.Name = "GroupBox_StarResidual";
            GroupBox_StarResidual.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_StarResidual.Size = new System.Drawing.Size(233, 115);
            GroupBox_StarResidual.TabIndex = 24;
            GroupBox_StarResidual.TabStop = false;
            GroupBox_StarResidual.Text = "Star Residual Weight";
            // 
            // Label_StarResidualMinValue
            // 
            Label_StarResidualMinValue.AutoSize = true;
            Label_StarResidualMinValue.Location = new System.Drawing.Point(64, 58);
            Label_StarResidualMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMinValue.Name = "Label_StarResidualMinValue";
            Label_StarResidualMinValue.Size = new System.Drawing.Size(13, 15);
            Label_StarResidualMinValue.TabIndex = 38;
            Label_StarResidualMinValue.Text = "0";
            // 
            // Label_StarResidualMaxValue
            // 
            Label_StarResidualMaxValue.AutoSize = true;
            Label_StarResidualMaxValue.Location = new System.Drawing.Point(64, 77);
            Label_StarResidualMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMaxValue.Name = "Label_StarResidualMaxValue";
            Label_StarResidualMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_StarResidualMaxValue.TabIndex = 37;
            Label_StarResidualMaxValue.Text = "0";
            // 
            // Label_StarResidualMedianValue
            // 
            Label_StarResidualMedianValue.AutoSize = true;
            Label_StarResidualMedianValue.Location = new System.Drawing.Point(64, 38);
            Label_StarResidualMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMedianValue.Name = "Label_StarResidualMedianValue";
            Label_StarResidualMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_StarResidualMedianValue.TabIndex = 36;
            Label_StarResidualMedianValue.Text = "0";
            // 
            // Label_StarResidualMeanValue
            // 
            Label_StarResidualMeanValue.AutoSize = true;
            Label_StarResidualMeanValue.Location = new System.Drawing.Point(64, 18);
            Label_StarResidualMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMeanValue.Name = "Label_StarResidualMeanValue";
            Label_StarResidualMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_StarResidualMeanValue.TabIndex = 34;
            Label_StarResidualMeanValue.Text = "0";
            // 
            // Label_StarResidualSigmaValue
            // 
            Label_StarResidualSigmaValue.AutoSize = true;
            Label_StarResidualSigmaValue.Location = new System.Drawing.Point(64, 97);
            Label_StarResidualSigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualSigmaValue.Name = "Label_StarResidualSigmaValue";
            Label_StarResidualSigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_StarResidualSigmaValue.TabIndex = 35;
            Label_StarResidualSigmaValue.Text = "0";
            // 
            // Label_StarResidualMin
            // 
            Label_StarResidualMin.AutoSize = true;
            Label_StarResidualMin.Location = new System.Drawing.Point(14, 57);
            Label_StarResidualMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMin.Name = "Label_StarResidualMin";
            Label_StarResidualMin.Size = new System.Drawing.Size(34, 15);
            Label_StarResidualMin.TabIndex = 23;
            Label_StarResidualMin.Text = "Min: ";
            // 
            // Label_StarResidualMax
            // 
            Label_StarResidualMax.AutoSize = true;
            Label_StarResidualMax.Location = new System.Drawing.Point(14, 76);
            Label_StarResidualMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMax.Name = "Label_StarResidualMax";
            Label_StarResidualMax.Size = new System.Drawing.Size(36, 15);
            Label_StarResidualMax.TabIndex = 22;
            Label_StarResidualMax.Text = "Max: ";
            // 
            // Label_StarResidualMedian
            // 
            Label_StarResidualMedian.AutoSize = true;
            Label_StarResidualMedian.Location = new System.Drawing.Point(14, 37);
            Label_StarResidualMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMedian.Name = "Label_StarResidualMedian";
            Label_StarResidualMedian.Size = new System.Drawing.Size(50, 15);
            Label_StarResidualMedian.TabIndex = 21;
            Label_StarResidualMedian.Text = "Median:";
            // 
            // Label_StarResidualMean
            // 
            Label_StarResidualMean.AutoSize = true;
            Label_StarResidualMean.Location = new System.Drawing.Point(14, 17);
            Label_StarResidualMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualMean.Name = "Label_StarResidualMean";
            Label_StarResidualMean.Size = new System.Drawing.Size(43, 15);
            Label_StarResidualMean.TabIndex = 19;
            Label_StarResidualMean.Text = "Mean: ";
            // 
            // Label_StarResidualSigma
            // 
            Label_StarResidualSigma.AutoSize = true;
            Label_StarResidualSigma.Location = new System.Drawing.Point(14, 96);
            Label_StarResidualSigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualSigma.Name = "Label_StarResidualSigma";
            Label_StarResidualSigma.Size = new System.Drawing.Size(46, 15);
            Label_StarResidualSigma.TabIndex = 20;
            Label_StarResidualSigma.Text = "Sigma: ";
            // 
            // Label_StarResidualRangeLow
            // 
            Label_StarResidualRangeLow.AutoSize = true;
            Label_StarResidualRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_StarResidualRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualRangeLow.Name = "Label_StarResidualRangeLow";
            Label_StarResidualRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_StarResidualRangeLow.TabIndex = 13;
            Label_StarResidualRangeLow.Text = "Low";
            Label_StarResidualRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarResidualRangeHigh
            // 
            Label_StarResidualRangeHigh.AutoSize = true;
            Label_StarResidualRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_StarResidualRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarResidualRangeHigh.Name = "Label_StarResidualRangeHigh";
            Label_StarResidualRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_StarResidualRangeHigh.TabIndex = 12;
            Label_StarResidualRangeHigh.Text = "High";
            Label_StarResidualRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_StarResidualRangeHigh
            // 
            TextBox_StarResidualRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_StarResidualRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_StarResidualRangeHigh.Name = "TextBox_StarResidualRangeHigh";
            TextBox_StarResidualRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_StarResidualRangeHigh.TabIndex = 8;
            TextBox_StarResidualRangeHigh.Text = "1.0";
            TextBox_StarResidualRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_StarResidualRangeHigh.TextChanged += TextBox_StarResidualRangeHigh_TextChanged;
            // 
            // TextBox_StarResidualRangeLow
            // 
            TextBox_StarResidualRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_StarResidualRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_StarResidualRangeLow.Name = "TextBox_StarResidualRangeLow";
            TextBox_StarResidualRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_StarResidualRangeLow.TabIndex = 9;
            TextBox_StarResidualRangeLow.Text = "0.0";
            TextBox_StarResidualRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_StarResidualRangeLow.TextChanged += TextBox_StarResidualRangeLow_TextChanged;
            // 
            // GroupBox_FwhmWeight
            // 
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMinValue);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMaxValue);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMedianValue);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMeanValue);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmSigmaValue);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMin);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMax);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMedian);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmRangeLow);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmMean);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmRangeHigh);
            GroupBox_FwhmWeight.Controls.Add(TextBox_FwhmRangeHigh);
            GroupBox_FwhmWeight.Controls.Add(TextBox_FwhmRangeLow);
            GroupBox_FwhmWeight.Controls.Add(Label_FwhmSigma);
            GroupBox_FwhmWeight.Location = new System.Drawing.Point(20, 36);
            GroupBox_FwhmWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FwhmWeight.Name = "GroupBox_FwhmWeight";
            GroupBox_FwhmWeight.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_FwhmWeight.Size = new System.Drawing.Size(233, 115);
            GroupBox_FwhmWeight.TabIndex = 12;
            GroupBox_FwhmWeight.TabStop = false;
            GroupBox_FwhmWeight.Text = "FWHM Weight";
            // 
            // Label_FwhmMinValue
            // 
            Label_FwhmMinValue.AutoSize = true;
            Label_FwhmMinValue.Location = new System.Drawing.Point(64, 57);
            Label_FwhmMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMinValue.Name = "Label_FwhmMinValue";
            Label_FwhmMinValue.Size = new System.Drawing.Size(13, 15);
            Label_FwhmMinValue.TabIndex = 23;
            Label_FwhmMinValue.Text = "0";
            // 
            // Label_FwhmMaxValue
            // 
            Label_FwhmMaxValue.AutoSize = true;
            Label_FwhmMaxValue.Location = new System.Drawing.Point(64, 76);
            Label_FwhmMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMaxValue.Name = "Label_FwhmMaxValue";
            Label_FwhmMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_FwhmMaxValue.TabIndex = 22;
            Label_FwhmMaxValue.Text = "0";
            // 
            // Label_FwhmMedianValue
            // 
            Label_FwhmMedianValue.AutoSize = true;
            Label_FwhmMedianValue.Location = new System.Drawing.Point(64, 37);
            Label_FwhmMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMedianValue.Name = "Label_FwhmMedianValue";
            Label_FwhmMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_FwhmMedianValue.TabIndex = 21;
            Label_FwhmMedianValue.Text = "0";
            // 
            // Label_FwhmMeanValue
            // 
            Label_FwhmMeanValue.AutoSize = true;
            Label_FwhmMeanValue.Location = new System.Drawing.Point(64, 17);
            Label_FwhmMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMeanValue.Name = "Label_FwhmMeanValue";
            Label_FwhmMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_FwhmMeanValue.TabIndex = 19;
            Label_FwhmMeanValue.Text = "0";
            // 
            // Label_FwhmSigmaValue
            // 
            Label_FwhmSigmaValue.AutoSize = true;
            Label_FwhmSigmaValue.Location = new System.Drawing.Point(64, 96);
            Label_FwhmSigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmSigmaValue.Name = "Label_FwhmSigmaValue";
            Label_FwhmSigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_FwhmSigmaValue.TabIndex = 20;
            Label_FwhmSigmaValue.Text = "0";
            // 
            // Label_FwhmMin
            // 
            Label_FwhmMin.AutoSize = true;
            Label_FwhmMin.Location = new System.Drawing.Point(14, 57);
            Label_FwhmMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMin.Name = "Label_FwhmMin";
            Label_FwhmMin.Size = new System.Drawing.Size(34, 15);
            Label_FwhmMin.TabIndex = 18;
            Label_FwhmMin.Text = "Min: ";
            // 
            // Label_FwhmMax
            // 
            Label_FwhmMax.AutoSize = true;
            Label_FwhmMax.Location = new System.Drawing.Point(14, 76);
            Label_FwhmMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMax.Name = "Label_FwhmMax";
            Label_FwhmMax.Size = new System.Drawing.Size(36, 15);
            Label_FwhmMax.TabIndex = 17;
            Label_FwhmMax.Text = "Max: ";
            // 
            // Label_FwhmMedian
            // 
            Label_FwhmMedian.AutoSize = true;
            Label_FwhmMedian.Location = new System.Drawing.Point(14, 37);
            Label_FwhmMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMedian.Name = "Label_FwhmMedian";
            Label_FwhmMedian.Size = new System.Drawing.Size(50, 15);
            Label_FwhmMedian.TabIndex = 16;
            Label_FwhmMedian.Text = "Median:";
            // 
            // Label_FwhmRangeLow
            // 
            Label_FwhmRangeLow.AutoSize = true;
            Label_FwhmRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_FwhmRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmRangeLow.Name = "Label_FwhmRangeLow";
            Label_FwhmRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_FwhmRangeLow.TabIndex = 15;
            Label_FwhmRangeLow.Text = "Low";
            Label_FwhmRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmMean
            // 
            Label_FwhmMean.AutoSize = true;
            Label_FwhmMean.Location = new System.Drawing.Point(14, 17);
            Label_FwhmMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMean.Name = "Label_FwhmMean";
            Label_FwhmMean.Size = new System.Drawing.Size(43, 15);
            Label_FwhmMean.TabIndex = 4;
            Label_FwhmMean.Text = "Mean: ";
            // 
            // Label_FwhmRangeHigh
            // 
            Label_FwhmRangeHigh.AutoSize = true;
            Label_FwhmRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_FwhmRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmRangeHigh.Name = "Label_FwhmRangeHigh";
            Label_FwhmRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_FwhmRangeHigh.TabIndex = 14;
            Label_FwhmRangeHigh.Text = "High";
            Label_FwhmRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_FwhmRangeHigh
            // 
            TextBox_FwhmRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_FwhmRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_FwhmRangeHigh.Name = "TextBox_FwhmRangeHigh";
            TextBox_FwhmRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_FwhmRangeHigh.TabIndex = 2;
            TextBox_FwhmRangeHigh.Text = "1.0";
            TextBox_FwhmRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_FwhmRangeHigh.TextChanged += TextBox_FwhmRangeHigh_TextChanged;
            // 
            // TextBox_FwhmRangeLow
            // 
            TextBox_FwhmRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_FwhmRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_FwhmRangeLow.Name = "TextBox_FwhmRangeLow";
            TextBox_FwhmRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_FwhmRangeLow.TabIndex = 3;
            TextBox_FwhmRangeLow.Text = "0.0";
            TextBox_FwhmRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_FwhmRangeLow.TextChanged += TextBox_FwhmRangeLow_TextChanged;
            // 
            // Label_FwhmSigma
            // 
            Label_FwhmSigma.AutoSize = true;
            Label_FwhmSigma.Location = new System.Drawing.Point(14, 96);
            Label_FwhmSigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmSigma.Name = "Label_FwhmSigma";
            Label_FwhmSigma.Size = new System.Drawing.Size(46, 15);
            Label_FwhmSigma.TabIndex = 5;
            Label_FwhmSigma.Text = "Sigma: ";
            // 
            // GroupBox_StarsWeight
            // 
            GroupBox_StarsWeight.Controls.Add(Label_StarsMinValue);
            GroupBox_StarsWeight.Controls.Add(Label_StarsMaxValue);
            GroupBox_StarsWeight.Controls.Add(Label_StarsMedianValue);
            GroupBox_StarsWeight.Controls.Add(Label_StarsMeanValue);
            GroupBox_StarsWeight.Controls.Add(Label_StarsSigmaValue);
            GroupBox_StarsWeight.Controls.Add(Label_StarsMin);
            GroupBox_StarsWeight.Controls.Add(Label_StarsMax);
            GroupBox_StarsWeight.Controls.Add(Label_StarsMedian);
            GroupBox_StarsWeight.Controls.Add(Label_StarsMean);
            GroupBox_StarsWeight.Controls.Add(Label_StarsSigma);
            GroupBox_StarsWeight.Controls.Add(Label_StarRangeLow);
            GroupBox_StarsWeight.Controls.Add(Label_StarRangeHigh);
            GroupBox_StarsWeight.Controls.Add(TextBox_StarRangeHigh);
            GroupBox_StarsWeight.Controls.Add(TextBox_StarRangeLow);
            GroupBox_StarsWeight.Location = new System.Drawing.Point(281, 158);
            GroupBox_StarsWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_StarsWeight.Name = "GroupBox_StarsWeight";
            GroupBox_StarsWeight.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_StarsWeight.Size = new System.Drawing.Size(233, 115);
            GroupBox_StarsWeight.TabIndex = 23;
            GroupBox_StarsWeight.TabStop = false;
            GroupBox_StarsWeight.Text = "Star Weight";
            // 
            // Label_StarsMinValue
            // 
            Label_StarsMinValue.AutoSize = true;
            Label_StarsMinValue.Location = new System.Drawing.Point(64, 58);
            Label_StarsMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMinValue.Name = "Label_StarsMinValue";
            Label_StarsMinValue.Size = new System.Drawing.Size(13, 15);
            Label_StarsMinValue.TabIndex = 33;
            Label_StarsMinValue.Text = "0";
            // 
            // Label_StarsMaxValue
            // 
            Label_StarsMaxValue.AutoSize = true;
            Label_StarsMaxValue.Location = new System.Drawing.Point(64, 77);
            Label_StarsMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMaxValue.Name = "Label_StarsMaxValue";
            Label_StarsMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_StarsMaxValue.TabIndex = 32;
            Label_StarsMaxValue.Text = "0";
            // 
            // Label_StarsMedianValue
            // 
            Label_StarsMedianValue.AutoSize = true;
            Label_StarsMedianValue.Location = new System.Drawing.Point(64, 38);
            Label_StarsMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMedianValue.Name = "Label_StarsMedianValue";
            Label_StarsMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_StarsMedianValue.TabIndex = 31;
            Label_StarsMedianValue.Text = "0";
            // 
            // Label_StarsMeanValue
            // 
            Label_StarsMeanValue.AutoSize = true;
            Label_StarsMeanValue.Location = new System.Drawing.Point(64, 18);
            Label_StarsMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMeanValue.Name = "Label_StarsMeanValue";
            Label_StarsMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_StarsMeanValue.TabIndex = 29;
            Label_StarsMeanValue.Text = "0";
            // 
            // Label_StarsSigmaValue
            // 
            Label_StarsSigmaValue.AutoSize = true;
            Label_StarsSigmaValue.Location = new System.Drawing.Point(64, 97);
            Label_StarsSigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsSigmaValue.Name = "Label_StarsSigmaValue";
            Label_StarsSigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_StarsSigmaValue.TabIndex = 30;
            Label_StarsSigmaValue.Text = "0";
            // 
            // Label_StarsMin
            // 
            Label_StarsMin.AutoSize = true;
            Label_StarsMin.Location = new System.Drawing.Point(14, 57);
            Label_StarsMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMin.Name = "Label_StarsMin";
            Label_StarsMin.Size = new System.Drawing.Size(34, 15);
            Label_StarsMin.TabIndex = 23;
            Label_StarsMin.Text = "Min: ";
            // 
            // Label_StarsMax
            // 
            Label_StarsMax.AutoSize = true;
            Label_StarsMax.Location = new System.Drawing.Point(14, 76);
            Label_StarsMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMax.Name = "Label_StarsMax";
            Label_StarsMax.Size = new System.Drawing.Size(36, 15);
            Label_StarsMax.TabIndex = 22;
            Label_StarsMax.Text = "Max: ";
            // 
            // Label_StarsMedian
            // 
            Label_StarsMedian.AutoSize = true;
            Label_StarsMedian.Location = new System.Drawing.Point(14, 37);
            Label_StarsMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMedian.Name = "Label_StarsMedian";
            Label_StarsMedian.Size = new System.Drawing.Size(50, 15);
            Label_StarsMedian.TabIndex = 21;
            Label_StarsMedian.Text = "Median:";
            // 
            // Label_StarsMean
            // 
            Label_StarsMean.AutoSize = true;
            Label_StarsMean.Location = new System.Drawing.Point(14, 17);
            Label_StarsMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsMean.Name = "Label_StarsMean";
            Label_StarsMean.Size = new System.Drawing.Size(43, 15);
            Label_StarsMean.TabIndex = 19;
            Label_StarsMean.Text = "Mean: ";
            // 
            // Label_StarsSigma
            // 
            Label_StarsSigma.AutoSize = true;
            Label_StarsSigma.Location = new System.Drawing.Point(14, 96);
            Label_StarsSigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarsSigma.Name = "Label_StarsSigma";
            Label_StarsSigma.Size = new System.Drawing.Size(46, 15);
            Label_StarsSigma.TabIndex = 20;
            Label_StarsSigma.Text = "Sigma: ";
            // 
            // Label_StarRangeLow
            // 
            Label_StarRangeLow.AutoSize = true;
            Label_StarRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_StarRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarRangeLow.Name = "Label_StarRangeLow";
            Label_StarRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_StarRangeLow.TabIndex = 15;
            Label_StarRangeLow.Text = "Low";
            Label_StarRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarRangeHigh
            // 
            Label_StarRangeHigh.AutoSize = true;
            Label_StarRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_StarRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_StarRangeHigh.Name = "Label_StarRangeHigh";
            Label_StarRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_StarRangeHigh.TabIndex = 14;
            Label_StarRangeHigh.Text = "High";
            Label_StarRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_StarRangeHigh
            // 
            TextBox_StarRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_StarRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_StarRangeHigh.Name = "TextBox_StarRangeHigh";
            TextBox_StarRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_StarRangeHigh.TabIndex = 2;
            TextBox_StarRangeHigh.Text = "1.0";
            TextBox_StarRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_StarRangeHigh.TextChanged += TextBox_StarsRangeHigh_TextChanged;
            // 
            // TextBox_StarRangeLow
            // 
            TextBox_StarRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_StarRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_StarRangeLow.Name = "TextBox_StarRangeLow";
            TextBox_StarRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_StarRangeLow.TabIndex = 3;
            TextBox_StarRangeLow.Text = "0.0";
            TextBox_StarRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_StarRangeLow.TextChanged += TextBox_StarsRangeLow_TextChanged;
            // 
            // GroupBox_EccentricityWeight
            // 
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMinValue);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMaxValue);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMedianValue);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMeanValue);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricitySigmaValue);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMin);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMax);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMedian);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityMean);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricitySigma);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityRangeLow);
            GroupBox_EccentricityWeight.Controls.Add(Label_EccentricityRangeHigh);
            GroupBox_EccentricityWeight.Controls.Add(TextBox_EccentricityRangeHigh);
            GroupBox_EccentricityWeight.Controls.Add(TextBox_EccentricityRangeLow);
            GroupBox_EccentricityWeight.Location = new System.Drawing.Point(281, 36);
            GroupBox_EccentricityWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_EccentricityWeight.Name = "GroupBox_EccentricityWeight";
            GroupBox_EccentricityWeight.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_EccentricityWeight.Size = new System.Drawing.Size(233, 115);
            GroupBox_EccentricityWeight.TabIndex = 13;
            GroupBox_EccentricityWeight.TabStop = false;
            GroupBox_EccentricityWeight.Text = "Eccentricity Weight";
            // 
            // Label_EccentricityMinValue
            // 
            Label_EccentricityMinValue.AutoSize = true;
            Label_EccentricityMinValue.Location = new System.Drawing.Point(64, 57);
            Label_EccentricityMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMinValue.Name = "Label_EccentricityMinValue";
            Label_EccentricityMinValue.Size = new System.Drawing.Size(13, 15);
            Label_EccentricityMinValue.TabIndex = 28;
            Label_EccentricityMinValue.Text = "0";
            // 
            // Label_EccentricityMaxValue
            // 
            Label_EccentricityMaxValue.AutoSize = true;
            Label_EccentricityMaxValue.Location = new System.Drawing.Point(64, 76);
            Label_EccentricityMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMaxValue.Name = "Label_EccentricityMaxValue";
            Label_EccentricityMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_EccentricityMaxValue.TabIndex = 27;
            Label_EccentricityMaxValue.Text = "0";
            // 
            // Label_EccentricityMedianValue
            // 
            Label_EccentricityMedianValue.AutoSize = true;
            Label_EccentricityMedianValue.Location = new System.Drawing.Point(64, 37);
            Label_EccentricityMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMedianValue.Name = "Label_EccentricityMedianValue";
            Label_EccentricityMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_EccentricityMedianValue.TabIndex = 26;
            Label_EccentricityMedianValue.Text = "0";
            // 
            // Label_EccentricityMeanValue
            // 
            Label_EccentricityMeanValue.AutoSize = true;
            Label_EccentricityMeanValue.Location = new System.Drawing.Point(64, 17);
            Label_EccentricityMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMeanValue.Name = "Label_EccentricityMeanValue";
            Label_EccentricityMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_EccentricityMeanValue.TabIndex = 24;
            Label_EccentricityMeanValue.Text = "0";
            // 
            // Label_EccentricitySigmaValue
            // 
            Label_EccentricitySigmaValue.AutoSize = true;
            Label_EccentricitySigmaValue.Location = new System.Drawing.Point(64, 96);
            Label_EccentricitySigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricitySigmaValue.Name = "Label_EccentricitySigmaValue";
            Label_EccentricitySigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_EccentricitySigmaValue.TabIndex = 25;
            Label_EccentricitySigmaValue.Text = "0";
            // 
            // Label_EccentricityMin
            // 
            Label_EccentricityMin.AutoSize = true;
            Label_EccentricityMin.Location = new System.Drawing.Point(14, 57);
            Label_EccentricityMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMin.Name = "Label_EccentricityMin";
            Label_EccentricityMin.Size = new System.Drawing.Size(34, 15);
            Label_EccentricityMin.TabIndex = 23;
            Label_EccentricityMin.Text = "Min: ";
            // 
            // Label_EccentricityMax
            // 
            Label_EccentricityMax.AutoSize = true;
            Label_EccentricityMax.Location = new System.Drawing.Point(14, 76);
            Label_EccentricityMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMax.Name = "Label_EccentricityMax";
            Label_EccentricityMax.Size = new System.Drawing.Size(36, 15);
            Label_EccentricityMax.TabIndex = 22;
            Label_EccentricityMax.Text = "Max: ";
            // 
            // Label_EccentricityMedian
            // 
            Label_EccentricityMedian.AutoSize = true;
            Label_EccentricityMedian.Location = new System.Drawing.Point(14, 37);
            Label_EccentricityMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMedian.Name = "Label_EccentricityMedian";
            Label_EccentricityMedian.Size = new System.Drawing.Size(50, 15);
            Label_EccentricityMedian.TabIndex = 21;
            Label_EccentricityMedian.Text = "Median:";
            // 
            // Label_EccentricityMean
            // 
            Label_EccentricityMean.AutoSize = true;
            Label_EccentricityMean.Location = new System.Drawing.Point(14, 17);
            Label_EccentricityMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityMean.Name = "Label_EccentricityMean";
            Label_EccentricityMean.Size = new System.Drawing.Size(43, 15);
            Label_EccentricityMean.TabIndex = 19;
            Label_EccentricityMean.Text = "Mean: ";
            // 
            // Label_EccentricitySigma
            // 
            Label_EccentricitySigma.AutoSize = true;
            Label_EccentricitySigma.Location = new System.Drawing.Point(14, 96);
            Label_EccentricitySigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricitySigma.Name = "Label_EccentricitySigma";
            Label_EccentricitySigma.Size = new System.Drawing.Size(46, 15);
            Label_EccentricitySigma.TabIndex = 20;
            Label_EccentricitySigma.Text = "Sigma: ";
            // 
            // Label_EccentricityRangeLow
            // 
            Label_EccentricityRangeLow.AutoSize = true;
            Label_EccentricityRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_EccentricityRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityRangeLow.Name = "Label_EccentricityRangeLow";
            Label_EccentricityRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_EccentricityRangeLow.TabIndex = 13;
            Label_EccentricityRangeLow.Text = "Low";
            Label_EccentricityRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EccentricityRangeHigh
            // 
            Label_EccentricityRangeHigh.AutoSize = true;
            Label_EccentricityRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_EccentricityRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_EccentricityRangeHigh.Name = "Label_EccentricityRangeHigh";
            Label_EccentricityRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_EccentricityRangeHigh.TabIndex = 12;
            Label_EccentricityRangeHigh.Text = "High";
            Label_EccentricityRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_EccentricityRangeHigh
            // 
            TextBox_EccentricityRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_EccentricityRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_EccentricityRangeHigh.Name = "TextBox_EccentricityRangeHigh";
            TextBox_EccentricityRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_EccentricityRangeHigh.TabIndex = 8;
            TextBox_EccentricityRangeHigh.Text = "1.0";
            TextBox_EccentricityRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_EccentricityRangeHigh.TextChanged += TextBox_EccentricityRangeHigh_TextChanged;
            // 
            // TextBox_EccentricityRangeLow
            // 
            TextBox_EccentricityRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_EccentricityRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_EccentricityRangeLow.Name = "TextBox_EccentricityRangeLow";
            TextBox_EccentricityRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_EccentricityRangeLow.TabIndex = 9;
            TextBox_EccentricityRangeLow.Text = "0.0";
            TextBox_EccentricityRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_EccentricityRangeLow.TextChanged += TextBox_EccentricityRangeLow_TextChanged;
            // 
            // GroupBox_AirMassWeight
            // 
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMinValue);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMaxValue);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMedianValue);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMeanValue);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassSigmaValue);
            GroupBox_AirMassWeight.Controls.Add(TextBox_AirMassRangeLow);
            GroupBox_AirMassWeight.Controls.Add(TextBox_AirMassRangeHigh);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMin);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMax);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMedian);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassMean);
            GroupBox_AirMassWeight.Controls.Add(Label_AirMassSigma);
            GroupBox_AirMassWeight.Controls.Add(Label_FwhmMeanDeviationRangeLow);
            GroupBox_AirMassWeight.Controls.Add(Label_FwhmMeanDeviationRangeHigh);
            GroupBox_AirMassWeight.Location = new System.Drawing.Point(20, 158);
            GroupBox_AirMassWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_AirMassWeight.Name = "GroupBox_AirMassWeight";
            GroupBox_AirMassWeight.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_AirMassWeight.Size = new System.Drawing.Size(233, 115);
            GroupBox_AirMassWeight.TabIndex = 22;
            GroupBox_AirMassWeight.TabStop = false;
            GroupBox_AirMassWeight.Text = "Air Mass Weight";
            // 
            // Label_AirMassMinValue
            // 
            Label_AirMassMinValue.AutoSize = true;
            Label_AirMassMinValue.Location = new System.Drawing.Point(64, 57);
            Label_AirMassMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMinValue.Name = "Label_AirMassMinValue";
            Label_AirMassMinValue.Size = new System.Drawing.Size(13, 15);
            Label_AirMassMinValue.TabIndex = 30;
            Label_AirMassMinValue.Text = "0";
            // 
            // Label_AirMassMaxValue
            // 
            Label_AirMassMaxValue.AutoSize = true;
            Label_AirMassMaxValue.Location = new System.Drawing.Point(64, 76);
            Label_AirMassMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMaxValue.Name = "Label_AirMassMaxValue";
            Label_AirMassMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_AirMassMaxValue.TabIndex = 29;
            Label_AirMassMaxValue.Text = "0";
            // 
            // Label_AirMassMedianValue
            // 
            Label_AirMassMedianValue.AutoSize = true;
            Label_AirMassMedianValue.Location = new System.Drawing.Point(64, 37);
            Label_AirMassMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMedianValue.Name = "Label_AirMassMedianValue";
            Label_AirMassMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_AirMassMedianValue.TabIndex = 28;
            Label_AirMassMedianValue.Text = "0";
            // 
            // Label_AirMassMeanValue
            // 
            Label_AirMassMeanValue.AutoSize = true;
            Label_AirMassMeanValue.Location = new System.Drawing.Point(64, 17);
            Label_AirMassMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMeanValue.Name = "Label_AirMassMeanValue";
            Label_AirMassMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_AirMassMeanValue.TabIndex = 26;
            Label_AirMassMeanValue.Text = "0";
            // 
            // Label_AirMassSigmaValue
            // 
            Label_AirMassSigmaValue.AutoSize = true;
            Label_AirMassSigmaValue.Location = new System.Drawing.Point(64, 96);
            Label_AirMassSigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassSigmaValue.Name = "Label_AirMassSigmaValue";
            Label_AirMassSigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_AirMassSigmaValue.TabIndex = 27;
            Label_AirMassSigmaValue.Text = "0";
            // 
            // TextBox_AirMassRangeLow
            // 
            TextBox_AirMassRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_AirMassRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_AirMassRangeLow.Name = "TextBox_AirMassRangeLow";
            TextBox_AirMassRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_AirMassRangeLow.TabIndex = 25;
            TextBox_AirMassRangeLow.Text = "0.0";
            TextBox_AirMassRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBox_AirMassRangeHigh
            // 
            TextBox_AirMassRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_AirMassRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_AirMassRangeHigh.Name = "TextBox_AirMassRangeHigh";
            TextBox_AirMassRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_AirMassRangeHigh.TabIndex = 24;
            TextBox_AirMassRangeHigh.Text = "1.0";
            TextBox_AirMassRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label_AirMassMin
            // 
            Label_AirMassMin.AutoSize = true;
            Label_AirMassMin.Location = new System.Drawing.Point(14, 57);
            Label_AirMassMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMin.Name = "Label_AirMassMin";
            Label_AirMassMin.Size = new System.Drawing.Size(34, 15);
            Label_AirMassMin.TabIndex = 23;
            Label_AirMassMin.Text = "Min: ";
            // 
            // Label_AirMassMax
            // 
            Label_AirMassMax.AutoSize = true;
            Label_AirMassMax.Location = new System.Drawing.Point(14, 76);
            Label_AirMassMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMax.Name = "Label_AirMassMax";
            Label_AirMassMax.Size = new System.Drawing.Size(36, 15);
            Label_AirMassMax.TabIndex = 22;
            Label_AirMassMax.Text = "Max: ";
            // 
            // Label_AirMassMedian
            // 
            Label_AirMassMedian.AutoSize = true;
            Label_AirMassMedian.Location = new System.Drawing.Point(14, 37);
            Label_AirMassMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMedian.Name = "Label_AirMassMedian";
            Label_AirMassMedian.Size = new System.Drawing.Size(50, 15);
            Label_AirMassMedian.TabIndex = 21;
            Label_AirMassMedian.Text = "Median:";
            // 
            // Label_AirMassMean
            // 
            Label_AirMassMean.AutoSize = true;
            Label_AirMassMean.Location = new System.Drawing.Point(14, 17);
            Label_AirMassMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassMean.Name = "Label_AirMassMean";
            Label_AirMassMean.Size = new System.Drawing.Size(43, 15);
            Label_AirMassMean.TabIndex = 19;
            Label_AirMassMean.Text = "Mean: ";
            // 
            // Label_AirMassSigma
            // 
            Label_AirMassSigma.AutoSize = true;
            Label_AirMassSigma.Location = new System.Drawing.Point(14, 96);
            Label_AirMassSigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_AirMassSigma.Name = "Label_AirMassSigma";
            Label_AirMassSigma.Size = new System.Drawing.Size(46, 15);
            Label_AirMassSigma.TabIndex = 20;
            Label_AirMassSigma.Text = "Sigma: ";
            // 
            // Label_FwhmMeanDeviationRangeLow
            // 
            Label_FwhmMeanDeviationRangeLow.AutoSize = true;
            Label_FwhmMeanDeviationRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_FwhmMeanDeviationRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMeanDeviationRangeLow.Name = "Label_FwhmMeanDeviationRangeLow";
            Label_FwhmMeanDeviationRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_FwhmMeanDeviationRangeLow.TabIndex = 15;
            Label_FwhmMeanDeviationRangeLow.Text = "Low";
            Label_FwhmMeanDeviationRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmMeanDeviationRangeHigh
            // 
            Label_FwhmMeanDeviationRangeHigh.AutoSize = true;
            Label_FwhmMeanDeviationRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_FwhmMeanDeviationRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_FwhmMeanDeviationRangeHigh.Name = "Label_FwhmMeanDeviationRangeHigh";
            Label_FwhmMeanDeviationRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_FwhmMeanDeviationRangeHigh.TabIndex = 14;
            Label_FwhmMeanDeviationRangeHigh.Text = "High";
            Label_FwhmMeanDeviationRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox_NoiseWeight
            // 
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMinValue);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMaxValue);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMedianValue);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMeanValue);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseSigmaValue);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMin);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMax);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMedian);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseMean);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseSigma);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseRangeLow);
            GroupBox_NoiseWeight.Controls.Add(Label_NoiseRangeHigh);
            GroupBox_NoiseWeight.Controls.Add(TextBox_NoiseRangeHigh);
            GroupBox_NoiseWeight.Controls.Add(TextBox_NoiseRangeLow);
            GroupBox_NoiseWeight.Location = new System.Drawing.Point(804, 36);
            GroupBox_NoiseWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_NoiseWeight.Name = "GroupBox_NoiseWeight";
            GroupBox_NoiseWeight.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_NoiseWeight.Size = new System.Drawing.Size(233, 115);
            GroupBox_NoiseWeight.TabIndex = 19;
            GroupBox_NoiseWeight.TabStop = false;
            GroupBox_NoiseWeight.Text = "Noise Weight";
            // 
            // Label_NoiseMinValue
            // 
            Label_NoiseMinValue.AutoSize = true;
            Label_NoiseMinValue.Location = new System.Drawing.Point(65, 58);
            Label_NoiseMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMinValue.Name = "Label_NoiseMinValue";
            Label_NoiseMinValue.Size = new System.Drawing.Size(13, 15);
            Label_NoiseMinValue.TabIndex = 38;
            Label_NoiseMinValue.Text = "0";
            // 
            // Label_NoiseMaxValue
            // 
            Label_NoiseMaxValue.AutoSize = true;
            Label_NoiseMaxValue.Location = new System.Drawing.Point(65, 77);
            Label_NoiseMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMaxValue.Name = "Label_NoiseMaxValue";
            Label_NoiseMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_NoiseMaxValue.TabIndex = 37;
            Label_NoiseMaxValue.Text = "0";
            // 
            // Label_NoiseMedianValue
            // 
            Label_NoiseMedianValue.AutoSize = true;
            Label_NoiseMedianValue.Location = new System.Drawing.Point(65, 38);
            Label_NoiseMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMedianValue.Name = "Label_NoiseMedianValue";
            Label_NoiseMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_NoiseMedianValue.TabIndex = 36;
            Label_NoiseMedianValue.Text = "0";
            // 
            // Label_NoiseMeanValue
            // 
            Label_NoiseMeanValue.AutoSize = true;
            Label_NoiseMeanValue.Location = new System.Drawing.Point(65, 18);
            Label_NoiseMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMeanValue.Name = "Label_NoiseMeanValue";
            Label_NoiseMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_NoiseMeanValue.TabIndex = 34;
            Label_NoiseMeanValue.Text = "0";
            // 
            // Label_NoiseSigmaValue
            // 
            Label_NoiseSigmaValue.AutoSize = true;
            Label_NoiseSigmaValue.Location = new System.Drawing.Point(65, 97);
            Label_NoiseSigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseSigmaValue.Name = "Label_NoiseSigmaValue";
            Label_NoiseSigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_NoiseSigmaValue.TabIndex = 35;
            Label_NoiseSigmaValue.Text = "0";
            // 
            // Label_NoiseMin
            // 
            Label_NoiseMin.AutoSize = true;
            Label_NoiseMin.Location = new System.Drawing.Point(14, 57);
            Label_NoiseMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMin.Name = "Label_NoiseMin";
            Label_NoiseMin.Size = new System.Drawing.Size(34, 15);
            Label_NoiseMin.TabIndex = 23;
            Label_NoiseMin.Text = "Min: ";
            // 
            // Label_NoiseMax
            // 
            Label_NoiseMax.AutoSize = true;
            Label_NoiseMax.Location = new System.Drawing.Point(14, 76);
            Label_NoiseMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMax.Name = "Label_NoiseMax";
            Label_NoiseMax.Size = new System.Drawing.Size(36, 15);
            Label_NoiseMax.TabIndex = 22;
            Label_NoiseMax.Text = "Max: ";
            // 
            // Label_NoiseMedian
            // 
            Label_NoiseMedian.AutoSize = true;
            Label_NoiseMedian.Location = new System.Drawing.Point(14, 37);
            Label_NoiseMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMedian.Name = "Label_NoiseMedian";
            Label_NoiseMedian.Size = new System.Drawing.Size(50, 15);
            Label_NoiseMedian.TabIndex = 21;
            Label_NoiseMedian.Text = "Median:";
            // 
            // Label_NoiseMean
            // 
            Label_NoiseMean.AutoSize = true;
            Label_NoiseMean.Location = new System.Drawing.Point(14, 17);
            Label_NoiseMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseMean.Name = "Label_NoiseMean";
            Label_NoiseMean.Size = new System.Drawing.Size(43, 15);
            Label_NoiseMean.TabIndex = 19;
            Label_NoiseMean.Text = "Mean: ";
            // 
            // Label_NoiseSigma
            // 
            Label_NoiseSigma.AutoSize = true;
            Label_NoiseSigma.Location = new System.Drawing.Point(14, 96);
            Label_NoiseSigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseSigma.Name = "Label_NoiseSigma";
            Label_NoiseSigma.Size = new System.Drawing.Size(46, 15);
            Label_NoiseSigma.TabIndex = 20;
            Label_NoiseSigma.Text = "Sigma: ";
            // 
            // Label_NoiseRangeLow
            // 
            Label_NoiseRangeLow.AutoSize = true;
            Label_NoiseRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_NoiseRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseRangeLow.Name = "Label_NoiseRangeLow";
            Label_NoiseRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_NoiseRangeLow.TabIndex = 15;
            Label_NoiseRangeLow.Text = "Low";
            Label_NoiseRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_NoiseRangeHigh
            // 
            Label_NoiseRangeHigh.AutoSize = true;
            Label_NoiseRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_NoiseRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_NoiseRangeHigh.Name = "Label_NoiseRangeHigh";
            Label_NoiseRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_NoiseRangeHigh.TabIndex = 14;
            Label_NoiseRangeHigh.Text = "High";
            Label_NoiseRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_NoiseRangeHigh
            // 
            TextBox_NoiseRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_NoiseRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_NoiseRangeHigh.Name = "TextBox_NoiseRangeHigh";
            TextBox_NoiseRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_NoiseRangeHigh.TabIndex = 2;
            TextBox_NoiseRangeHigh.Text = "1.0";
            TextBox_NoiseRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_NoiseRangeHigh.TextChanged += TextBox_NoiseRangeHigh_TextChanged;
            // 
            // TextBox_NoiseRangeLow
            // 
            TextBox_NoiseRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_NoiseRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_NoiseRangeLow.Name = "TextBox_NoiseRangeLow";
            TextBox_NoiseRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_NoiseRangeLow.TabIndex = 3;
            TextBox_NoiseRangeLow.Text = "0.0";
            TextBox_NoiseRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_NoiseRangeLow.TextChanged += TextBox_NoiseRangeLow_TextChanged;
            // 
            // GroupBox_MedianWeight
            // 
            GroupBox_MedianWeight.Controls.Add(Label_MedianMinValue);
            GroupBox_MedianWeight.Controls.Add(Label_MedianMaxValue);
            GroupBox_MedianWeight.Controls.Add(Label_MedianMedianValue);
            GroupBox_MedianWeight.Controls.Add(Label_MedianMeanValue);
            GroupBox_MedianWeight.Controls.Add(Label_MedianSigmaValue);
            GroupBox_MedianWeight.Controls.Add(Label_MedianMin);
            GroupBox_MedianWeight.Controls.Add(Label_MedianMax);
            GroupBox_MedianWeight.Controls.Add(Label_MedianMedian);
            GroupBox_MedianWeight.Controls.Add(Label_MedianMean);
            GroupBox_MedianWeight.Controls.Add(Label_MedianSigma);
            GroupBox_MedianWeight.Controls.Add(Label_MedianRangeLow);
            GroupBox_MedianWeight.Controls.Add(Label_MedianRangeHigh);
            GroupBox_MedianWeight.Controls.Add(TextBox_MedianRangeHigh);
            GroupBox_MedianWeight.Controls.Add(TextBox_MedianRangeLow);
            GroupBox_MedianWeight.Location = new System.Drawing.Point(542, 36);
            GroupBox_MedianWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_MedianWeight.Name = "GroupBox_MedianWeight";
            GroupBox_MedianWeight.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_MedianWeight.Size = new System.Drawing.Size(233, 115);
            GroupBox_MedianWeight.TabIndex = 17;
            GroupBox_MedianWeight.TabStop = false;
            GroupBox_MedianWeight.Text = "Median Weight";
            // 
            // Label_MedianMinValue
            // 
            Label_MedianMinValue.AutoSize = true;
            Label_MedianMinValue.Location = new System.Drawing.Point(64, 58);
            Label_MedianMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMinValue.Name = "Label_MedianMinValue";
            Label_MedianMinValue.Size = new System.Drawing.Size(13, 15);
            Label_MedianMinValue.TabIndex = 33;
            Label_MedianMinValue.Text = "0";
            // 
            // Label_MedianMaxValue
            // 
            Label_MedianMaxValue.AutoSize = true;
            Label_MedianMaxValue.Location = new System.Drawing.Point(64, 77);
            Label_MedianMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMaxValue.Name = "Label_MedianMaxValue";
            Label_MedianMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_MedianMaxValue.TabIndex = 32;
            Label_MedianMaxValue.Text = "0";
            // 
            // Label_MedianMedianValue
            // 
            Label_MedianMedianValue.AutoSize = true;
            Label_MedianMedianValue.Location = new System.Drawing.Point(64, 38);
            Label_MedianMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMedianValue.Name = "Label_MedianMedianValue";
            Label_MedianMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_MedianMedianValue.TabIndex = 31;
            Label_MedianMedianValue.Text = "0";
            // 
            // Label_MedianMeanValue
            // 
            Label_MedianMeanValue.AutoSize = true;
            Label_MedianMeanValue.Location = new System.Drawing.Point(64, 18);
            Label_MedianMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMeanValue.Name = "Label_MedianMeanValue";
            Label_MedianMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_MedianMeanValue.TabIndex = 29;
            Label_MedianMeanValue.Text = "0";
            // 
            // Label_MedianSigmaValue
            // 
            Label_MedianSigmaValue.AutoSize = true;
            Label_MedianSigmaValue.Location = new System.Drawing.Point(64, 97);
            Label_MedianSigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianSigmaValue.Name = "Label_MedianSigmaValue";
            Label_MedianSigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_MedianSigmaValue.TabIndex = 30;
            Label_MedianSigmaValue.Text = "0";
            // 
            // Label_MedianMin
            // 
            Label_MedianMin.AutoSize = true;
            Label_MedianMin.Location = new System.Drawing.Point(14, 57);
            Label_MedianMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMin.Name = "Label_MedianMin";
            Label_MedianMin.Size = new System.Drawing.Size(34, 15);
            Label_MedianMin.TabIndex = 23;
            Label_MedianMin.Text = "Min: ";
            // 
            // Label_MedianMax
            // 
            Label_MedianMax.AutoSize = true;
            Label_MedianMax.Location = new System.Drawing.Point(14, 76);
            Label_MedianMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMax.Name = "Label_MedianMax";
            Label_MedianMax.Size = new System.Drawing.Size(36, 15);
            Label_MedianMax.TabIndex = 22;
            Label_MedianMax.Text = "Max: ";
            // 
            // Label_MedianMedian
            // 
            Label_MedianMedian.AutoSize = true;
            Label_MedianMedian.Location = new System.Drawing.Point(14, 37);
            Label_MedianMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMedian.Name = "Label_MedianMedian";
            Label_MedianMedian.Size = new System.Drawing.Size(50, 15);
            Label_MedianMedian.TabIndex = 21;
            Label_MedianMedian.Text = "Median:";
            // 
            // Label_MedianMean
            // 
            Label_MedianMean.AutoSize = true;
            Label_MedianMean.Location = new System.Drawing.Point(14, 17);
            Label_MedianMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianMean.Name = "Label_MedianMean";
            Label_MedianMean.Size = new System.Drawing.Size(43, 15);
            Label_MedianMean.TabIndex = 19;
            Label_MedianMean.Text = "Mean: ";
            // 
            // Label_MedianSigma
            // 
            Label_MedianSigma.AutoSize = true;
            Label_MedianSigma.Location = new System.Drawing.Point(14, 96);
            Label_MedianSigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianSigma.Name = "Label_MedianSigma";
            Label_MedianSigma.Size = new System.Drawing.Size(46, 15);
            Label_MedianSigma.TabIndex = 20;
            Label_MedianSigma.Text = "Sigma: ";
            // 
            // Label_MedianRangeLow
            // 
            Label_MedianRangeLow.AutoSize = true;
            Label_MedianRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_MedianRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianRangeLow.Name = "Label_MedianRangeLow";
            Label_MedianRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_MedianRangeLow.TabIndex = 15;
            Label_MedianRangeLow.Text = "Low";
            Label_MedianRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MedianRangeHigh
            // 
            Label_MedianRangeHigh.AutoSize = true;
            Label_MedianRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_MedianRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_MedianRangeHigh.Name = "Label_MedianRangeHigh";
            Label_MedianRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_MedianRangeHigh.TabIndex = 14;
            Label_MedianRangeHigh.Text = "High";
            Label_MedianRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_MedianRangeHigh
            // 
            TextBox_MedianRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_MedianRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_MedianRangeHigh.Name = "TextBox_MedianRangeHigh";
            TextBox_MedianRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_MedianRangeHigh.TabIndex = 2;
            TextBox_MedianRangeHigh.Text = "1.0";
            TextBox_MedianRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_MedianRangeHigh.TextChanged += TextBox_MedianRangeHigh_TextChanged;
            // 
            // TextBox_MedianRangeLow
            // 
            TextBox_MedianRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_MedianRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_MedianRangeLow.Name = "TextBox_MedianRangeLow";
            TextBox_MedianRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_MedianRangeLow.TabIndex = 3;
            TextBox_MedianRangeLow.Text = "0.0";
            TextBox_MedianRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_MedianRangeLow.TextChanged += TextBox_MedianRangeLow_TextChanged;
            // 
            // GroupBox_SnrWeight
            // 
            GroupBox_SnrWeight.Controls.Add(Label_SnrMinValue);
            GroupBox_SnrWeight.Controls.Add(Label_SnrMaxValue);
            GroupBox_SnrWeight.Controls.Add(Label_SnrMedianValue);
            GroupBox_SnrWeight.Controls.Add(Label_SnrMeanValue);
            GroupBox_SnrWeight.Controls.Add(Label_SnrSigmaValue);
            GroupBox_SnrWeight.Controls.Add(Label_SnrMin);
            GroupBox_SnrWeight.Controls.Add(Label_SnrMax);
            GroupBox_SnrWeight.Controls.Add(Label_SnrMedian);
            GroupBox_SnrWeight.Controls.Add(Label_SnrMean);
            GroupBox_SnrWeight.Controls.Add(Label_SnrSigma);
            GroupBox_SnrWeight.Controls.Add(Label_SnrRangeLow);
            GroupBox_SnrWeight.Controls.Add(Label_SnrRangeHigh);
            GroupBox_SnrWeight.Controls.Add(TextBox_SnrRangeHigh);
            GroupBox_SnrWeight.Controls.Add(TextBox_SnrRangeLow);
            GroupBox_SnrWeight.Location = new System.Drawing.Point(804, 158);
            GroupBox_SnrWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_SnrWeight.Name = "GroupBox_SnrWeight";
            GroupBox_SnrWeight.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_SnrWeight.Size = new System.Drawing.Size(233, 115);
            GroupBox_SnrWeight.TabIndex = 16;
            GroupBox_SnrWeight.TabStop = false;
            GroupBox_SnrWeight.Text = "SNR Weight";
            // 
            // Label_SnrMinValue
            // 
            Label_SnrMinValue.AutoSize = true;
            Label_SnrMinValue.Location = new System.Drawing.Point(65, 57);
            Label_SnrMinValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMinValue.Name = "Label_SnrMinValue";
            Label_SnrMinValue.Size = new System.Drawing.Size(13, 15);
            Label_SnrMinValue.TabIndex = 38;
            Label_SnrMinValue.Text = "0";
            // 
            // Label_SnrMaxValue
            // 
            Label_SnrMaxValue.AutoSize = true;
            Label_SnrMaxValue.Location = new System.Drawing.Point(65, 76);
            Label_SnrMaxValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMaxValue.Name = "Label_SnrMaxValue";
            Label_SnrMaxValue.Size = new System.Drawing.Size(13, 15);
            Label_SnrMaxValue.TabIndex = 37;
            Label_SnrMaxValue.Text = "0";
            // 
            // Label_SnrMedianValue
            // 
            Label_SnrMedianValue.AutoSize = true;
            Label_SnrMedianValue.Location = new System.Drawing.Point(65, 37);
            Label_SnrMedianValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMedianValue.Name = "Label_SnrMedianValue";
            Label_SnrMedianValue.Size = new System.Drawing.Size(13, 15);
            Label_SnrMedianValue.TabIndex = 36;
            Label_SnrMedianValue.Text = "0";
            // 
            // Label_SnrMeanValue
            // 
            Label_SnrMeanValue.AutoSize = true;
            Label_SnrMeanValue.Location = new System.Drawing.Point(65, 17);
            Label_SnrMeanValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMeanValue.Name = "Label_SnrMeanValue";
            Label_SnrMeanValue.Size = new System.Drawing.Size(13, 15);
            Label_SnrMeanValue.TabIndex = 34;
            Label_SnrMeanValue.Text = "0";
            // 
            // Label_SnrSigmaValue
            // 
            Label_SnrSigmaValue.AutoSize = true;
            Label_SnrSigmaValue.Location = new System.Drawing.Point(65, 96);
            Label_SnrSigmaValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrSigmaValue.Name = "Label_SnrSigmaValue";
            Label_SnrSigmaValue.Size = new System.Drawing.Size(13, 15);
            Label_SnrSigmaValue.TabIndex = 35;
            Label_SnrSigmaValue.Text = "0";
            // 
            // Label_SnrMin
            // 
            Label_SnrMin.AutoSize = true;
            Label_SnrMin.Location = new System.Drawing.Point(14, 57);
            Label_SnrMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMin.Name = "Label_SnrMin";
            Label_SnrMin.Size = new System.Drawing.Size(34, 15);
            Label_SnrMin.TabIndex = 23;
            Label_SnrMin.Text = "Min: ";
            // 
            // Label_SnrMax
            // 
            Label_SnrMax.AutoSize = true;
            Label_SnrMax.Location = new System.Drawing.Point(14, 76);
            Label_SnrMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMax.Name = "Label_SnrMax";
            Label_SnrMax.Size = new System.Drawing.Size(36, 15);
            Label_SnrMax.TabIndex = 22;
            Label_SnrMax.Text = "Max: ";
            // 
            // Label_SnrMedian
            // 
            Label_SnrMedian.AutoSize = true;
            Label_SnrMedian.Location = new System.Drawing.Point(14, 37);
            Label_SnrMedian.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMedian.Name = "Label_SnrMedian";
            Label_SnrMedian.Size = new System.Drawing.Size(50, 15);
            Label_SnrMedian.TabIndex = 21;
            Label_SnrMedian.Text = "Median:";
            // 
            // Label_SnrMean
            // 
            Label_SnrMean.AutoSize = true;
            Label_SnrMean.Location = new System.Drawing.Point(14, 17);
            Label_SnrMean.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrMean.Name = "Label_SnrMean";
            Label_SnrMean.Size = new System.Drawing.Size(43, 15);
            Label_SnrMean.TabIndex = 19;
            Label_SnrMean.Text = "Mean: ";
            // 
            // Label_SnrSigma
            // 
            Label_SnrSigma.AutoSize = true;
            Label_SnrSigma.Location = new System.Drawing.Point(14, 96);
            Label_SnrSigma.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrSigma.Name = "Label_SnrSigma";
            Label_SnrSigma.Size = new System.Drawing.Size(46, 15);
            Label_SnrSigma.TabIndex = 20;
            Label_SnrSigma.Text = "Sigma: ";
            // 
            // Label_SnrRangeLow
            // 
            Label_SnrRangeLow.AutoSize = true;
            Label_SnrRangeLow.Location = new System.Drawing.Point(131, 76);
            Label_SnrRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrRangeLow.Name = "Label_SnrRangeLow";
            Label_SnrRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_SnrRangeLow.TabIndex = 15;
            Label_SnrRangeLow.Text = "Low";
            Label_SnrRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_SnrRangeHigh
            // 
            Label_SnrRangeHigh.AutoSize = true;
            Label_SnrRangeHigh.Location = new System.Drawing.Point(128, 37);
            Label_SnrRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_SnrRangeHigh.Name = "Label_SnrRangeHigh";
            Label_SnrRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_SnrRangeHigh.TabIndex = 14;
            Label_SnrRangeHigh.Text = "High";
            Label_SnrRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_SnrRangeHigh
            // 
            TextBox_SnrRangeHigh.Location = new System.Drawing.Point(164, 32);
            TextBox_SnrRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_SnrRangeHigh.Name = "TextBox_SnrRangeHigh";
            TextBox_SnrRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_SnrRangeHigh.TabIndex = 2;
            TextBox_SnrRangeHigh.Text = "1.0";
            TextBox_SnrRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_SnrRangeHigh.TextChanged += TextBox_SnrRangeHigh_TextChanged;
            // 
            // TextBox_SnrRangeLow
            // 
            TextBox_SnrRangeLow.Location = new System.Drawing.Point(164, 72);
            TextBox_SnrRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_SnrRangeLow.Name = "TextBox_SnrRangeLow";
            TextBox_SnrRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_SnrRangeLow.TabIndex = 3;
            TextBox_SnrRangeLow.Text = "0.0";
            TextBox_SnrRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_SnrRangeLow.TextChanged += TextBox_SnrRangeLow_TextChanged;
            // 
            // GroupBox_UpdateStatistics
            // 
            GroupBox_UpdateStatistics.Controls.Add(RadioButton_SetImageStatistics_CalculateWeights);
            GroupBox_UpdateStatistics.Controls.Add(RadioButton_SetImageStatistics_RescaleWeights);
            GroupBox_UpdateStatistics.Controls.Add(RadioButton_SetImageStatistics_KeepWeights);
            GroupBox_UpdateStatistics.Controls.Add(Button_ReadSubFrameSelectorCsvFile);
            GroupBox_UpdateStatistics.Controls.Add(Label_UpdateStatistics);
            GroupBox_UpdateStatistics.Controls.Add(Label_UpdateStatisticsRangeHigh);
            GroupBox_UpdateStatistics.Controls.Add(TextBox_UpdateStatisticsRangeHigh);
            GroupBox_UpdateStatistics.Controls.Add(TextBox_UpdateStatisticsRangeLow);
            GroupBox_UpdateStatistics.Controls.Add(Label_UpdateStatisticsRangeLow);
            GroupBox_UpdateStatistics.Location = new System.Drawing.Point(142, 17);
            GroupBox_UpdateStatistics.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_UpdateStatistics.Name = "GroupBox_UpdateStatistics";
            GroupBox_UpdateStatistics.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_UpdateStatistics.Size = new System.Drawing.Size(817, 84);
            GroupBox_UpdateStatistics.TabIndex = 20;
            GroupBox_UpdateStatistics.TabStop = false;
            GroupBox_UpdateStatistics.Text = "Set Image Statistics";
            // 
            // RadioButton_SetImageStatistics_CalculateWeights
            // 
            RadioButton_SetImageStatistics_CalculateWeights.AutoSize = true;
            RadioButton_SetImageStatistics_CalculateWeights.Location = new System.Drawing.Point(224, 54);
            RadioButton_SetImageStatistics_CalculateWeights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_SetImageStatistics_CalculateWeights.Name = "RadioButton_SetImageStatistics_CalculateWeights";
            RadioButton_SetImageStatistics_CalculateWeights.Size = new System.Drawing.Size(127, 19);
            RadioButton_SetImageStatistics_CalculateWeights.TabIndex = 24;
            RadioButton_SetImageStatistics_CalculateWeights.Text = "Calculated Weights";
            RadioButton_SetImageStatistics_CalculateWeights.UseVisualStyleBackColor = true;
            RadioButton_SetImageStatistics_CalculateWeights.CheckedChanged += RadioButton_SetImageStatistics_CalculateWeights_CheckedChanged;
            // 
            // RadioButton_SetImageStatistics_RescaleWeights
            // 
            RadioButton_SetImageStatistics_RescaleWeights.AutoSize = true;
            RadioButton_SetImageStatistics_RescaleWeights.Location = new System.Drawing.Point(224, 35);
            RadioButton_SetImageStatistics_RescaleWeights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_SetImageStatistics_RescaleWeights.Name = "RadioButton_SetImageStatistics_RescaleWeights";
            RadioButton_SetImageStatistics_RescaleWeights.Size = new System.Drawing.Size(110, 19);
            RadioButton_SetImageStatistics_RescaleWeights.TabIndex = 22;
            RadioButton_SetImageStatistics_RescaleWeights.Text = "Rescale Weights";
            RadioButton_SetImageStatistics_RescaleWeights.UseVisualStyleBackColor = true;
            RadioButton_SetImageStatistics_RescaleWeights.CheckedChanged += RadioButton_SetImageStatistics_RescaleWeights_CheckedChanged;
            // 
            // RadioButton_SetImageStatistics_KeepWeights
            // 
            RadioButton_SetImageStatistics_KeepWeights.AutoSize = true;
            RadioButton_SetImageStatistics_KeepWeights.Checked = true;
            RadioButton_SetImageStatistics_KeepWeights.Location = new System.Drawing.Point(224, 15);
            RadioButton_SetImageStatistics_KeepWeights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_SetImageStatistics_KeepWeights.Name = "RadioButton_SetImageStatistics_KeepWeights";
            RadioButton_SetImageStatistics_KeepWeights.Size = new System.Drawing.Size(141, 19);
            RadioButton_SetImageStatistics_KeepWeights.TabIndex = 21;
            RadioButton_SetImageStatistics_KeepWeights.TabStop = true;
            RadioButton_SetImageStatistics_KeepWeights.Text = "Keep Existing Weights";
            RadioButton_SetImageStatistics_KeepWeights.UseVisualStyleBackColor = true;
            RadioButton_SetImageStatistics_KeepWeights.CheckedChanged += RadioButton_SetImageStatistics_KeepWeights_CheckedChanged;
            // 
            // Button_ReadSubFrameSelectorCsvFile
            // 
            Button_ReadSubFrameSelectorCsvFile.Location = new System.Drawing.Point(44, 24);
            Button_ReadSubFrameSelectorCsvFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_ReadSubFrameSelectorCsvFile.Name = "Button_ReadSubFrameSelectorCsvFile";
            Button_ReadSubFrameSelectorCsvFile.Size = new System.Drawing.Size(154, 39);
            Button_ReadSubFrameSelectorCsvFile.TabIndex = 0;
            Button_ReadSubFrameSelectorCsvFile.Text = "Read SubFrame Selector CSV File";
            Button_ReadSubFrameSelectorCsvFile.UseVisualStyleBackColor = true;
            Button_ReadSubFrameSelectorCsvFile.Click += Button_ReadCSV_Click;
            // 
            // Label_UpdateStatistics
            // 
            Label_UpdateStatistics.AutoSize = true;
            Label_UpdateStatistics.Location = new System.Drawing.Point(435, 37);
            Label_UpdateStatistics.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_UpdateStatistics.Name = "Label_UpdateStatistics";
            Label_UpdateStatistics.Size = new System.Drawing.Size(151, 15);
            Label_UpdateStatistics.TabIndex = 20;
            Label_UpdateStatistics.Text = "Rescale SSWEIGHT - Range:";
            // 
            // Label_UpdateStatisticsRangeHigh
            // 
            Label_UpdateStatisticsRangeHigh.AutoSize = true;
            Label_UpdateStatisticsRangeHigh.Location = new System.Drawing.Point(612, 37);
            Label_UpdateStatisticsRangeHigh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_UpdateStatisticsRangeHigh.Name = "Label_UpdateStatisticsRangeHigh";
            Label_UpdateStatisticsRangeHigh.Size = new System.Drawing.Size(33, 15);
            Label_UpdateStatisticsRangeHigh.TabIndex = 18;
            Label_UpdateStatisticsRangeHigh.Text = "High";
            Label_UpdateStatisticsRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_UpdateStatisticsRangeHigh
            // 
            TextBox_UpdateStatisticsRangeHigh.Location = new System.Drawing.Point(649, 32);
            TextBox_UpdateStatisticsRangeHigh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_UpdateStatisticsRangeHigh.Name = "TextBox_UpdateStatisticsRangeHigh";
            TextBox_UpdateStatisticsRangeHigh.Size = new System.Drawing.Size(51, 23);
            TextBox_UpdateStatisticsRangeHigh.TabIndex = 16;
            TextBox_UpdateStatisticsRangeHigh.Text = "100";
            TextBox_UpdateStatisticsRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_UpdateStatisticsRangeHigh.TextChanged += TextBox_UpdateStatisticsRangeHigh_TextChanged;
            // 
            // TextBox_UpdateStatisticsRangeLow
            // 
            TextBox_UpdateStatisticsRangeLow.Location = new System.Drawing.Point(748, 32);
            TextBox_UpdateStatisticsRangeLow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_UpdateStatisticsRangeLow.Name = "TextBox_UpdateStatisticsRangeLow";
            TextBox_UpdateStatisticsRangeLow.Size = new System.Drawing.Size(51, 23);
            TextBox_UpdateStatisticsRangeLow.TabIndex = 17;
            TextBox_UpdateStatisticsRangeLow.Text = "50";
            TextBox_UpdateStatisticsRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_UpdateStatisticsRangeLow.TextChanged += TextBox_UpdateStatisticsRangeLow_TextChanged;
            // 
            // Label_UpdateStatisticsRangeLow
            // 
            Label_UpdateStatisticsRangeLow.AutoSize = true;
            Label_UpdateStatisticsRangeLow.Location = new System.Drawing.Point(714, 37);
            Label_UpdateStatisticsRangeLow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_UpdateStatisticsRangeLow.Name = "Label_UpdateStatisticsRangeLow";
            Label_UpdateStatisticsRangeLow.Size = new System.Drawing.Size(29, 15);
            Label_UpdateStatisticsRangeLow.TabIndex = 19;
            Label_UpdateStatisticsRangeLow.Text = "Low";
            Label_UpdateStatisticsRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox_InitialRejectionCriteria
            // 
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_Snr);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_SNR);
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_StarResidual);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_StarResidual);
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_Stars);
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_AirMass);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_Stars);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_AirMass);
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_Noise);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_Noise);
            GroupBox_InitialRejectionCriteria.Controls.Add(Button_Rejection_RejectionSet);
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_Median);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_Median);
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_Eccentricity);
            GroupBox_InitialRejectionCriteria.Controls.Add(NumericUpDown_Rejection_FWHM);
            GroupBox_InitialRejectionCriteria.Controls.Add(TextBox_Rejection_Total);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_Total);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_Eccentricity);
            GroupBox_InitialRejectionCriteria.Controls.Add(Label_Rejection_FWHM);
            GroupBox_InitialRejectionCriteria.Location = new System.Drawing.Point(36, 115);
            GroupBox_InitialRejectionCriteria.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_InitialRejectionCriteria.Name = "GroupBox_InitialRejectionCriteria";
            GroupBox_InitialRejectionCriteria.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_InitialRejectionCriteria.Size = new System.Drawing.Size(1063, 113);
            GroupBox_InitialRejectionCriteria.TabIndex = 26;
            GroupBox_InitialRejectionCriteria.TabStop = false;
            GroupBox_InitialRejectionCriteria.Text = "Initial Rejection Criteria";
            // 
            // NumericUpDown_Rejection_Snr
            // 
            NumericUpDown_Rejection_Snr.DecimalPlaces = 2;
            NumericUpDown_Rejection_Snr.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NumericUpDown_Rejection_Snr.Location = new System.Drawing.Point(567, 70);
            NumericUpDown_Rejection_Snr.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_Snr.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            NumericUpDown_Rejection_Snr.Name = "NumericUpDown_Rejection_Snr";
            NumericUpDown_Rejection_Snr.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_Snr.TabIndex = 20;
            NumericUpDown_Rejection_Snr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_Snr.Value = new decimal(new int[] { 4000, 0, 0, 0 });
            NumericUpDown_Rejection_Snr.ValueChanged += NumericUpDown_Rejection_Snr_ValueChanged;
            // 
            // Label_Rejection_SNR
            // 
            Label_Rejection_SNR.AutoSize = true;
            Label_Rejection_SNR.Location = new System.Drawing.Point(526, 75);
            Label_Rejection_SNR.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_SNR.Name = "Label_Rejection_SNR";
            Label_Rejection_SNR.Size = new System.Drawing.Size(32, 15);
            Label_Rejection_SNR.TabIndex = 19;
            Label_Rejection_SNR.Text = "SNR:";
            Label_Rejection_SNR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_StarResidual
            // 
            NumericUpDown_Rejection_StarResidual.DecimalPlaces = 3;
            NumericUpDown_Rejection_StarResidual.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            NumericUpDown_Rejection_StarResidual.Location = new System.Drawing.Point(435, 70);
            NumericUpDown_Rejection_StarResidual.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_StarResidual.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NumericUpDown_Rejection_StarResidual.Name = "NumericUpDown_Rejection_StarResidual";
            NumericUpDown_Rejection_StarResidual.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_StarResidual.TabIndex = 18;
            NumericUpDown_Rejection_StarResidual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_StarResidual.Value = new decimal(new int[] { 1, 0, 0, 0 });
            NumericUpDown_Rejection_StarResidual.ValueChanged += NumericUpDown_Rejection_StarResidual_ValueChanged;
            // 
            // Label_Rejection_StarResidual
            // 
            Label_Rejection_StarResidual.AutoSize = true;
            Label_Rejection_StarResidual.Location = new System.Drawing.Point(348, 75);
            Label_Rejection_StarResidual.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_StarResidual.Name = "Label_Rejection_StarResidual";
            Label_Rejection_StarResidual.Size = new System.Drawing.Size(77, 15);
            Label_Rejection_StarResidual.TabIndex = 17;
            Label_Rejection_StarResidual.Text = "Star Residual:";
            Label_Rejection_StarResidual.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_Stars
            // 
            NumericUpDown_Rejection_Stars.Location = new System.Drawing.Point(260, 70);
            NumericUpDown_Rejection_Stars.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_Stars.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            NumericUpDown_Rejection_Stars.Name = "NumericUpDown_Rejection_Stars";
            NumericUpDown_Rejection_Stars.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_Stars.TabIndex = 16;
            NumericUpDown_Rejection_Stars.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_Stars.ValueChanged += NumericUpDown_Rejection_Stars_ValueChanged;
            // 
            // NumericUpDown_Rejection_AirMass
            // 
            NumericUpDown_Rejection_AirMass.DecimalPlaces = 2;
            NumericUpDown_Rejection_AirMass.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NumericUpDown_Rejection_AirMass.Location = new System.Drawing.Point(99, 70);
            NumericUpDown_Rejection_AirMass.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_AirMass.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            NumericUpDown_Rejection_AirMass.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            NumericUpDown_Rejection_AirMass.Name = "NumericUpDown_Rejection_AirMass";
            NumericUpDown_Rejection_AirMass.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_AirMass.TabIndex = 15;
            NumericUpDown_Rejection_AirMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_AirMass.Value = new decimal(new int[] { 2, 0, 0, 0 });
            NumericUpDown_Rejection_AirMass.ValueChanged += NumericUpDown_Rejection_AirMass_ValueChanged;
            // 
            // Label_Rejection_Stars
            // 
            Label_Rejection_Stars.AutoSize = true;
            Label_Rejection_Stars.Location = new System.Drawing.Point(218, 75);
            Label_Rejection_Stars.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_Stars.Name = "Label_Rejection_Stars";
            Label_Rejection_Stars.Size = new System.Drawing.Size(35, 15);
            Label_Rejection_Stars.TabIndex = 14;
            Label_Rejection_Stars.Text = "Stars:";
            Label_Rejection_Stars.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Rejection_AirMass
            // 
            Label_Rejection_AirMass.AutoSize = true;
            Label_Rejection_AirMass.Location = new System.Drawing.Point(38, 75);
            Label_Rejection_AirMass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_AirMass.Name = "Label_Rejection_AirMass";
            Label_Rejection_AirMass.Size = new System.Drawing.Size(55, 15);
            Label_Rejection_AirMass.TabIndex = 13;
            Label_Rejection_AirMass.Text = "Air Mass:";
            Label_Rejection_AirMass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_Noise
            // 
            NumericUpDown_Rejection_Noise.DecimalPlaces = 2;
            NumericUpDown_Rejection_Noise.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NumericUpDown_Rejection_Noise.Location = new System.Drawing.Point(566, 32);
            NumericUpDown_Rejection_Noise.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_Noise.Maximum = new decimal(new int[] { 300, 0, 0, 0 });
            NumericUpDown_Rejection_Noise.Name = "NumericUpDown_Rejection_Noise";
            NumericUpDown_Rejection_Noise.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_Noise.TabIndex = 12;
            NumericUpDown_Rejection_Noise.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_Noise.Value = new decimal(new int[] { 200, 0, 0, 0 });
            NumericUpDown_Rejection_Noise.ValueChanged += NumericUpDown_Rejection_Noise_ValueChanged;
            // 
            // Label_Rejection_Noise
            // 
            Label_Rejection_Noise.AutoSize = true;
            Label_Rejection_Noise.Location = new System.Drawing.Point(520, 37);
            Label_Rejection_Noise.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_Noise.Name = "Label_Rejection_Noise";
            Label_Rejection_Noise.Size = new System.Drawing.Size(40, 15);
            Label_Rejection_Noise.TabIndex = 11;
            Label_Rejection_Noise.Text = "Noise:";
            Label_Rejection_Noise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Button_Rejection_RejectionSet
            // 
            Button_Rejection_RejectionSet.Location = new System.Drawing.Point(906, 50);
            Button_Rejection_RejectionSet.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_Rejection_RejectionSet.Name = "Button_Rejection_RejectionSet";
            Button_Rejection_RejectionSet.Size = new System.Drawing.Size(97, 27);
            Button_Rejection_RejectionSet.TabIndex = 10;
            Button_Rejection_RejectionSet.Text = "Set Rejected";
            Button_Rejection_RejectionSet.UseVisualStyleBackColor = true;
            Button_Rejection_RejectionSet.Click += Button_Rejection_RejectionSet_Click;
            // 
            // NumericUpDown_Rejection_Median
            // 
            NumericUpDown_Rejection_Median.Location = new System.Drawing.Point(435, 32);
            NumericUpDown_Rejection_Median.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_Median.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            NumericUpDown_Rejection_Median.Name = "NumericUpDown_Rejection_Median";
            NumericUpDown_Rejection_Median.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_Median.TabIndex = 9;
            NumericUpDown_Rejection_Median.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_Median.Value = new decimal(new int[] { 4000, 0, 0, 0 });
            NumericUpDown_Rejection_Median.ValueChanged += NumericUpDown_Rejection_Median_ValueChanged;
            // 
            // Label_Rejection_Median
            // 
            Label_Rejection_Median.AutoSize = true;
            Label_Rejection_Median.Location = new System.Drawing.Point(380, 37);
            Label_Rejection_Median.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_Median.Name = "Label_Rejection_Median";
            Label_Rejection_Median.Size = new System.Drawing.Size(50, 15);
            Label_Rejection_Median.TabIndex = 8;
            Label_Rejection_Median.Text = "Median:";
            Label_Rejection_Median.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_Eccentricity
            // 
            NumericUpDown_Rejection_Eccentricity.DecimalPlaces = 2;
            NumericUpDown_Rejection_Eccentricity.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NumericUpDown_Rejection_Eccentricity.Location = new System.Drawing.Point(260, 32);
            NumericUpDown_Rejection_Eccentricity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_Eccentricity.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NumericUpDown_Rejection_Eccentricity.Name = "NumericUpDown_Rejection_Eccentricity";
            NumericUpDown_Rejection_Eccentricity.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_Eccentricity.TabIndex = 7;
            NumericUpDown_Rejection_Eccentricity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_Eccentricity.Value = new decimal(new int[] { 65, 0, 0, 131072 });
            NumericUpDown_Rejection_Eccentricity.ValueChanged += NumericUpDown_Rejection_Eccentricity_ValueChanged;
            // 
            // NumericUpDown_Rejection_FWHM
            // 
            NumericUpDown_Rejection_FWHM.DecimalPlaces = 2;
            NumericUpDown_Rejection_FWHM.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NumericUpDown_Rejection_FWHM.Location = new System.Drawing.Point(99, 32);
            NumericUpDown_Rejection_FWHM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NumericUpDown_Rejection_FWHM.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            NumericUpDown_Rejection_FWHM.Name = "NumericUpDown_Rejection_FWHM";
            NumericUpDown_Rejection_FWHM.Size = new System.Drawing.Size(69, 23);
            NumericUpDown_Rejection_FWHM.TabIndex = 6;
            NumericUpDown_Rejection_FWHM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            NumericUpDown_Rejection_FWHM.Value = new decimal(new int[] { 45, 0, 0, 65536 });
            NumericUpDown_Rejection_FWHM.ValueChanged += NumericUpDown_Rejection_FWHM_ValueChanged;
            // 
            // TextBox_Rejection_Total
            // 
            TextBox_Rejection_Total.Location = new System.Drawing.Point(847, 51);
            TextBox_Rejection_Total.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_Rejection_Total.Name = "TextBox_Rejection_Total";
            TextBox_Rejection_Total.Size = new System.Drawing.Size(51, 23);
            TextBox_Rejection_Total.TabIndex = 5;
            TextBox_Rejection_Total.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label_Rejection_Total
            // 
            Label_Rejection_Total.AutoSize = true;
            Label_Rejection_Total.Location = new System.Drawing.Point(719, 55);
            Label_Rejection_Total.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_Total.Name = "Label_Rejection_Total";
            Label_Rejection_Total.Size = new System.Drawing.Size(116, 15);
            Label_Rejection_Total.TabIndex = 4;
            Label_Rejection_Total.Text = "Rejected SubFrames:";
            Label_Rejection_Total.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Rejection_Eccentricity
            // 
            Label_Rejection_Eccentricity.AutoSize = true;
            Label_Rejection_Eccentricity.Location = new System.Drawing.Point(182, 37);
            Label_Rejection_Eccentricity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_Eccentricity.Name = "Label_Rejection_Eccentricity";
            Label_Rejection_Eccentricity.Size = new System.Drawing.Size(71, 15);
            Label_Rejection_Eccentricity.TabIndex = 3;
            Label_Rejection_Eccentricity.Text = "Eccentricity:";
            Label_Rejection_Eccentricity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Rejection_FWHM
            // 
            Label_Rejection_FWHM.AutoSize = true;
            Label_Rejection_FWHM.Location = new System.Drawing.Point(46, 37);
            Label_Rejection_FWHM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_Rejection_FWHM.Name = "Label_Rejection_FWHM";
            Label_Rejection_FWHM.Size = new System.Drawing.Size(47, 15);
            Label_Rejection_FWHM.TabIndex = 2;
            Label_Rejection_FWHM.Text = "FWHM:";
            Label_Rejection_FWHM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TabPage_Calibration
            // 
            TabPage_Calibration.BackColor = System.Drawing.SystemColors.Control;
            TabPage_Calibration.Controls.Add(CheckBox_CalibrationTab_CreateNew);
            TabPage_Calibration.Controls.Add(TreeView_CalibrationTab_TargetFileTree);
            TabPage_Calibration.Controls.Add(TextBox_CalibrationTab_Messgaes);
            TabPage_Calibration.Controls.Add(GroupBox_CalibrationTab_MatchingTolerance);
            TabPage_Calibration.Controls.Add(Label_CalibrationTab_TotalFiles);
            TabPage_Calibration.Controls.Add(ProgressBar_CalibrationTab);
            TabPage_Calibration.Controls.Add(Label_CalibrationTab_ReadFileName);
            TabPage_Calibration.Controls.Add(Button_CalibrationTab_CreateCalibrationDirectory);
            TabPage_Calibration.Controls.Add(Button_CalibrationTab_MatchCalibrationFrames);
            TabPage_Calibration.Controls.Add(Button_CalibrationTab_FindCalibrationFrames);
            TabPage_Calibration.Location = new System.Drawing.Point(4, 24);
            TabPage_Calibration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_Calibration.Name = "TabPage_Calibration";
            TabPage_Calibration.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_Calibration.Size = new System.Drawing.Size(1139, 498);
            TabPage_Calibration.TabIndex = 1;
            TabPage_Calibration.Text = "Calibration";
            // 
            // CheckBox_CalibrationTab_CreateNew
            // 
            CheckBox_CalibrationTab_CreateNew.AutoSize = true;
            CheckBox_CalibrationTab_CreateNew.Checked = true;
            CheckBox_CalibrationTab_CreateNew.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox_CalibrationTab_CreateNew.Location = new System.Drawing.Point(147, 211);
            CheckBox_CalibrationTab_CreateNew.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_CalibrationTab_CreateNew.Name = "CheckBox_CalibrationTab_CreateNew";
            CheckBox_CalibrationTab_CreateNew.Size = new System.Drawing.Size(87, 19);
            CheckBox_CalibrationTab_CreateNew.TabIndex = 11;
            CheckBox_CalibrationTab_CreateNew.Text = "Create New";
            CheckBox_CalibrationTab_CreateNew.UseVisualStyleBackColor = true;
            // 
            // TreeView_CalibrationTab_TargetFileTree
            // 
            TreeView_CalibrationTab_TargetFileTree.Location = new System.Drawing.Point(575, 6);
            TreeView_CalibrationTab_TargetFileTree.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TreeView_CalibrationTab_TargetFileTree.Name = "TreeView_CalibrationTab_TargetFileTree";
            TreeView_CalibrationTab_TargetFileTree.Size = new System.Drawing.Size(535, 257);
            TreeView_CalibrationTab_TargetFileTree.TabIndex = 10;
            // 
            // TextBox_CalibrationTab_Messgaes
            // 
            TextBox_CalibrationTab_Messgaes.Location = new System.Drawing.Point(25, 274);
            TextBox_CalibrationTab_Messgaes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_CalibrationTab_Messgaes.Multiline = true;
            TextBox_CalibrationTab_Messgaes.Name = "TextBox_CalibrationTab_Messgaes";
            TextBox_CalibrationTab_Messgaes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            TextBox_CalibrationTab_Messgaes.Size = new System.Drawing.Size(1084, 161);
            TextBox_CalibrationTab_Messgaes.TabIndex = 8;
            // 
            // GroupBox_CalibrationTab_MatchingTolerance
            // 
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_TemperatureTolerance);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_OffsetTolerance);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_GainTolerance);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_ExposureTolerance);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_Percentage);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(TextBox_CalibrationTab_MatchingTolerance_Temperature);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(TextBox_CalibrationTab_MatchingTolerance_Offset);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(TextBox_CalibrationTab_MatchingTolerance_Gain);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_Temperature);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_Offset);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_Gain);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(Label_CalibrationTab_MatchingTolerance_Exposure);
            GroupBox_CalibrationTab_MatchingTolerance.Controls.Add(TextBox_CalibrationTab_MatchingTolerance_Exposure);
            GroupBox_CalibrationTab_MatchingTolerance.Location = new System.Drawing.Point(253, 23);
            GroupBox_CalibrationTab_MatchingTolerance.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_CalibrationTab_MatchingTolerance.Name = "GroupBox_CalibrationTab_MatchingTolerance";
            GroupBox_CalibrationTab_MatchingTolerance.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_CalibrationTab_MatchingTolerance.Size = new System.Drawing.Size(303, 216);
            GroupBox_CalibrationTab_MatchingTolerance.TabIndex = 7;
            GroupBox_CalibrationTab_MatchingTolerance.TabStop = false;
            GroupBox_CalibrationTab_MatchingTolerance.Text = "Matching Tolerance";
            // 
            // Label_CalibrationTab_MatchingTolerance_TemperatureTolerance
            // 
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance.Location = new System.Drawing.Point(194, 152);
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance.Name = "Label_CalibrationTab_MatchingTolerance_TemperatureTolerance";
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance.Size = new System.Drawing.Size(49, 15);
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance.TabIndex = 12;
            Label_CalibrationTab_MatchingTolerance_TemperatureTolerance.Text = "Degrees";
            // 
            // Label_CalibrationTab_MatchingTolerance_OffsetTolerance
            // 
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance.Location = new System.Drawing.Point(194, 123);
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance.Name = "Label_CalibrationTab_MatchingTolerance_OffsetTolerance";
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance.Size = new System.Drawing.Size(31, 15);
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance.TabIndex = 11;
            Label_CalibrationTab_MatchingTolerance_OffsetTolerance.Text = "ADU";
            // 
            // Label_CalibrationTab_MatchingTolerance_GainTolerance
            // 
            Label_CalibrationTab_MatchingTolerance_GainTolerance.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_GainTolerance.Location = new System.Drawing.Point(194, 95);
            Label_CalibrationTab_MatchingTolerance_GainTolerance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_GainTolerance.Name = "Label_CalibrationTab_MatchingTolerance_GainTolerance";
            Label_CalibrationTab_MatchingTolerance_GainTolerance.Size = new System.Drawing.Size(34, 15);
            Label_CalibrationTab_MatchingTolerance_GainTolerance.TabIndex = 10;
            Label_CalibrationTab_MatchingTolerance_GainTolerance.Text = "Units";
            // 
            // Label_CalibrationTab_MatchingTolerance_ExposureTolerance
            // 
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance.Location = new System.Drawing.Point(194, 66);
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance.Name = "Label_CalibrationTab_MatchingTolerance_ExposureTolerance";
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance.Size = new System.Drawing.Size(51, 15);
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance.TabIndex = 9;
            Label_CalibrationTab_MatchingTolerance_ExposureTolerance.Text = "Seconds";
            // 
            // Label_CalibrationTab_MatchingTolerance_Percentage
            // 
            Label_CalibrationTab_MatchingTolerance_Percentage.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_Percentage.Location = new System.Drawing.Point(100, 38);
            Label_CalibrationTab_MatchingTolerance_Percentage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_Percentage.Name = "Label_CalibrationTab_MatchingTolerance_Percentage";
            Label_CalibrationTab_MatchingTolerance_Percentage.Size = new System.Drawing.Size(79, 15);
            Label_CalibrationTab_MatchingTolerance_Percentage.TabIndex = 8;
            Label_CalibrationTab_MatchingTolerance_Percentage.Text = "Match Within";
            // 
            // TextBox_CalibrationTab_MatchingTolerance_Temperature
            // 
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Location = new System.Drawing.Point(117, 148);
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Name = "TextBox_CalibrationTab_MatchingTolerance_Temperature";
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Size = new System.Drawing.Size(48, 23);
            TextBox_CalibrationTab_MatchingTolerance_Temperature.TabIndex = 7;
            TextBox_CalibrationTab_MatchingTolerance_Temperature.Text = "5";
            TextBox_CalibrationTab_MatchingTolerance_Temperature.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_CalibrationTab_MatchingTolerance_Temperature.TextChanged += TextBox_CalibrationTab_TemperatureTolerance_TextChanged;
            // 
            // TextBox_CalibrationTab_MatchingTolerance_Offset
            // 
            TextBox_CalibrationTab_MatchingTolerance_Offset.Location = new System.Drawing.Point(117, 119);
            TextBox_CalibrationTab_MatchingTolerance_Offset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_CalibrationTab_MatchingTolerance_Offset.Name = "TextBox_CalibrationTab_MatchingTolerance_Offset";
            TextBox_CalibrationTab_MatchingTolerance_Offset.Size = new System.Drawing.Size(48, 23);
            TextBox_CalibrationTab_MatchingTolerance_Offset.TabIndex = 6;
            TextBox_CalibrationTab_MatchingTolerance_Offset.Text = "5";
            TextBox_CalibrationTab_MatchingTolerance_Offset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_CalibrationTab_MatchingTolerance_Offset.TextChanged += TextBox_CalibrationTab_OffsetTolerance_TextChanged;
            // 
            // TextBox_CalibrationTab_MatchingTolerance_Gain
            // 
            TextBox_CalibrationTab_MatchingTolerance_Gain.Location = new System.Drawing.Point(117, 90);
            TextBox_CalibrationTab_MatchingTolerance_Gain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_CalibrationTab_MatchingTolerance_Gain.Name = "TextBox_CalibrationTab_MatchingTolerance_Gain";
            TextBox_CalibrationTab_MatchingTolerance_Gain.Size = new System.Drawing.Size(48, 23);
            TextBox_CalibrationTab_MatchingTolerance_Gain.TabIndex = 5;
            TextBox_CalibrationTab_MatchingTolerance_Gain.Text = "10";
            TextBox_CalibrationTab_MatchingTolerance_Gain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_CalibrationTab_MatchingTolerance_Gain.TextChanged += TextBox_CalibrationTab_GainTolerance_TextChanged;
            // 
            // Label_CalibrationTab_MatchingTolerance_Temperature
            // 
            Label_CalibrationTab_MatchingTolerance_Temperature.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_Temperature.Location = new System.Drawing.Point(36, 152);
            Label_CalibrationTab_MatchingTolerance_Temperature.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_Temperature.Name = "Label_CalibrationTab_MatchingTolerance_Temperature";
            Label_CalibrationTab_MatchingTolerance_Temperature.Size = new System.Drawing.Size(73, 15);
            Label_CalibrationTab_MatchingTolerance_Temperature.TabIndex = 4;
            Label_CalibrationTab_MatchingTolerance_Temperature.Text = "Temperature";
            // 
            // Label_CalibrationTab_MatchingTolerance_Offset
            // 
            Label_CalibrationTab_MatchingTolerance_Offset.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_Offset.Location = new System.Drawing.Point(36, 123);
            Label_CalibrationTab_MatchingTolerance_Offset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_Offset.Name = "Label_CalibrationTab_MatchingTolerance_Offset";
            Label_CalibrationTab_MatchingTolerance_Offset.Size = new System.Drawing.Size(39, 15);
            Label_CalibrationTab_MatchingTolerance_Offset.TabIndex = 3;
            Label_CalibrationTab_MatchingTolerance_Offset.Text = "Offset";
            // 
            // Label_CalibrationTab_MatchingTolerance_Gain
            // 
            Label_CalibrationTab_MatchingTolerance_Gain.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_Gain.Location = new System.Drawing.Point(36, 95);
            Label_CalibrationTab_MatchingTolerance_Gain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_Gain.Name = "Label_CalibrationTab_MatchingTolerance_Gain";
            Label_CalibrationTab_MatchingTolerance_Gain.Size = new System.Drawing.Size(31, 15);
            Label_CalibrationTab_MatchingTolerance_Gain.TabIndex = 2;
            Label_CalibrationTab_MatchingTolerance_Gain.Text = "Gain";
            // 
            // Label_CalibrationTab_MatchingTolerance_Exposure
            // 
            Label_CalibrationTab_MatchingTolerance_Exposure.AutoSize = true;
            Label_CalibrationTab_MatchingTolerance_Exposure.Location = new System.Drawing.Point(36, 66);
            Label_CalibrationTab_MatchingTolerance_Exposure.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_MatchingTolerance_Exposure.Name = "Label_CalibrationTab_MatchingTolerance_Exposure";
            Label_CalibrationTab_MatchingTolerance_Exposure.Size = new System.Drawing.Size(55, 15);
            Label_CalibrationTab_MatchingTolerance_Exposure.TabIndex = 1;
            Label_CalibrationTab_MatchingTolerance_Exposure.Text = "Exposure";
            // 
            // TextBox_CalibrationTab_MatchingTolerance_Exposure
            // 
            TextBox_CalibrationTab_MatchingTolerance_Exposure.Location = new System.Drawing.Point(117, 61);
            TextBox_CalibrationTab_MatchingTolerance_Exposure.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TextBox_CalibrationTab_MatchingTolerance_Exposure.Name = "TextBox_CalibrationTab_MatchingTolerance_Exposure";
            TextBox_CalibrationTab_MatchingTolerance_Exposure.Size = new System.Drawing.Size(48, 23);
            TextBox_CalibrationTab_MatchingTolerance_Exposure.TabIndex = 0;
            TextBox_CalibrationTab_MatchingTolerance_Exposure.Text = "10";
            TextBox_CalibrationTab_MatchingTolerance_Exposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            TextBox_CalibrationTab_MatchingTolerance_Exposure.TextChanged += TextBox_CalibrationTab_ExposureTolerance_TextChanged;
            // 
            // Label_CalibrationTab_TotalFiles
            // 
            Label_CalibrationTab_TotalFiles.AutoSize = true;
            Label_CalibrationTab_TotalFiles.Location = new System.Drawing.Point(31, 23);
            Label_CalibrationTab_TotalFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_TotalFiles.Name = "Label_CalibrationTab_TotalFiles";
            Label_CalibrationTab_TotalFiles.Size = new System.Drawing.Size(125, 15);
            Label_CalibrationTab_TotalFiles.TabIndex = 6;
            Label_CalibrationTab_TotalFiles.Text = "No Calibration Frames";
            Label_CalibrationTab_TotalFiles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgressBar_CalibrationTab
            // 
            ProgressBar_CalibrationTab.Location = new System.Drawing.Point(20, 476);
            ProgressBar_CalibrationTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ProgressBar_CalibrationTab.Name = "ProgressBar_CalibrationTab";
            ProgressBar_CalibrationTab.Size = new System.Drawing.Size(1099, 13);
            ProgressBar_CalibrationTab.TabIndex = 5;
            // 
            // Label_CalibrationTab_ReadFileName
            // 
            Label_CalibrationTab_ReadFileName.AutoSize = true;
            Label_CalibrationTab_ReadFileName.Location = new System.Drawing.Point(16, 440);
            Label_CalibrationTab_ReadFileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_CalibrationTab_ReadFileName.Name = "Label_CalibrationTab_ReadFileName";
            Label_CalibrationTab_ReadFileName.Size = new System.Drawing.Size(121, 15);
            Label_CalibrationTab_ReadFileName.TabIndex = 4;
            Label_CalibrationTab_ReadFileName.Text = "Calibration File Name";
            // 
            // Button_CalibrationTab_CreateCalibrationDirectory
            // 
            Button_CalibrationTab_CreateCalibrationDirectory.Location = new System.Drawing.Point(48, 177);
            Button_CalibrationTab_CreateCalibrationDirectory.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_CalibrationTab_CreateCalibrationDirectory.Name = "Button_CalibrationTab_CreateCalibrationDirectory";
            Button_CalibrationTab_CreateCalibrationDirectory.Size = new System.Drawing.Size(88, 87);
            Button_CalibrationTab_CreateCalibrationDirectory.TabIndex = 2;
            Button_CalibrationTab_CreateCalibrationDirectory.Text = "Create Target Calibration Directory";
            Button_CalibrationTab_CreateCalibrationDirectory.UseVisualStyleBackColor = true;
            Button_CalibrationTab_CreateCalibrationDirectory.Click += CalibrationTab_CreateCalibrationDirectory_Click;
            // 
            // Button_CalibrationTab_MatchCalibrationFrames
            // 
            Button_CalibrationTab_MatchCalibrationFrames.Location = new System.Drawing.Point(52, 113);
            Button_CalibrationTab_MatchCalibrationFrames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_CalibrationTab_MatchCalibrationFrames.Name = "Button_CalibrationTab_MatchCalibrationFrames";
            Button_CalibrationTab_MatchCalibrationFrames.Size = new System.Drawing.Size(88, 58);
            Button_CalibrationTab_MatchCalibrationFrames.TabIndex = 1;
            Button_CalibrationTab_MatchCalibrationFrames.Text = "ReMatch Calibration Frames";
            Button_CalibrationTab_MatchCalibrationFrames.UseVisualStyleBackColor = true;
            Button_CalibrationTab_MatchCalibrationFrames.Click += CalibrationTab_ReMatchCalibrationFrames_Click;
            // 
            // Button_CalibrationTab_FindCalibrationFrames
            // 
            Button_CalibrationTab_FindCalibrationFrames.Location = new System.Drawing.Point(52, 48);
            Button_CalibrationTab_FindCalibrationFrames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_CalibrationTab_FindCalibrationFrames.Name = "Button_CalibrationTab_FindCalibrationFrames";
            Button_CalibrationTab_FindCalibrationFrames.Size = new System.Drawing.Size(88, 58);
            Button_CalibrationTab_FindCalibrationFrames.TabIndex = 0;
            Button_CalibrationTab_FindCalibrationFrames.Text = "Find Calibration Frames";
            Button_CalibrationTab_FindCalibrationFrames.UseVisualStyleBackColor = true;
            Button_CalibrationTab_FindCalibrationFrames.Click += CalibrationTab_FindCalibrationFrames_Click;
            // 
            // TabPage_KeywordUpdate
            // 
            TabPage_KeywordUpdate.BackColor = System.Drawing.SystemColors.Control;
            TabPage_KeywordUpdate.Controls.Add(Label_KeywordUpdateTab_FileName);
            TabPage_KeywordUpdate.Controls.Add(ProgressBar_KeywordUpdateTab_WriteProgress);
            TabPage_KeywordUpdate.Controls.Add(GroupBox_KeywordUpdateTab_CaptureSoftware);
            TabPage_KeywordUpdate.Controls.Add(GroupBox_KeywordUpdateTab_Telescope);
            TabPage_KeywordUpdate.Controls.Add(GroupBox_KeywordUpdateTab_SubFrameKeywords);
            TabPage_KeywordUpdate.Controls.Add(GroupBox_KeywordUpdateTab_Camera);
            TabPage_KeywordUpdate.Controls.Add(GroupBox_KeywordUpdateTab_ImageType);
            TabPage_KeywordUpdate.Location = new System.Drawing.Point(4, 24);
            TabPage_KeywordUpdate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_KeywordUpdate.Name = "TabPage_KeywordUpdate";
            TabPage_KeywordUpdate.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabPage_KeywordUpdate.Size = new System.Drawing.Size(1139, 498);
            TabPage_KeywordUpdate.TabIndex = 0;
            TabPage_KeywordUpdate.Text = "Keyword Update";
            // 
            // Label_KeywordUpdateTab_FileName
            // 
            Label_KeywordUpdateTab_FileName.AutoSize = true;
            Label_KeywordUpdateTab_FileName.Location = new System.Drawing.Point(20, 416);
            Label_KeywordUpdateTab_FileName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_FileName.Name = "Label_KeywordUpdateTab_FileName";
            Label_KeywordUpdateTab_FileName.Size = new System.Drawing.Size(77, 15);
            Label_KeywordUpdateTab_FileName.TabIndex = 19;
            Label_KeywordUpdateTab_FileName.Text = "Updating File";
            // 
            // ProgressBar_KeywordUpdateTab_WriteProgress
            // 
            ProgressBar_KeywordUpdateTab_WriteProgress.Location = new System.Drawing.Point(20, 454);
            ProgressBar_KeywordUpdateTab_WriteProgress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ProgressBar_KeywordUpdateTab_WriteProgress.Name = "ProgressBar_KeywordUpdateTab_WriteProgress";
            ProgressBar_KeywordUpdateTab_WriteProgress.Size = new System.Drawing.Size(1094, 13);
            ProgressBar_KeywordUpdateTab_WriteProgress.Step = 1;
            ProgressBar_KeywordUpdateTab_WriteProgress.TabIndex = 13;
            // 
            // GroupBox_KeywordUpdateTab_CaptureSoftware
            // 
            GroupBox_KeywordUpdateTab_CaptureSoftware.Controls.Add(RadioButton_KeywordUpdateTab_CaptureSoftware_NINA);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Controls.Add(Button_KeywordUpdateTab_CaptureSoftware_SetByFile);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Controls.Add(Button_KeywordUpdateTab_CaptureSoftware_SetAll);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Controls.Add(RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Controls.Add(RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Controls.Add(RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Controls.Add(RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Location = new System.Drawing.Point(20, 187);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Name = "GroupBox_KeywordUpdateTab_CaptureSoftware";
            GroupBox_KeywordUpdateTab_CaptureSoftware.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_CaptureSoftware.Size = new System.Drawing.Size(148, 216);
            GroupBox_KeywordUpdateTab_CaptureSoftware.TabIndex = 22;
            GroupBox_KeywordUpdateTab_CaptureSoftware.TabStop = false;
            GroupBox_KeywordUpdateTab_CaptureSoftware.Text = "Capture Software";
            // 
            // RadioButton_KeywordUpdateTab_CaptureSoftware_NINA
            // 
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.AutoSize = true;
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Location = new System.Drawing.Point(23, 47);
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Name = "RadioButton_KeywordUpdateTab_CaptureSoftware_NINA";
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Size = new System.Drawing.Size(54, 19);
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.TabIndex = 6;
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.Text = "NINA";
            RadioButton_KeywordUpdateTab_CaptureSoftware_NINA.UseVisualStyleBackColor = true;
            // 
            // Button_KeywordUpdateTab_CaptureSoftware_SetByFile
            // 
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.Location = new System.Drawing.Point(34, 181);
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.Name = "Button_KeywordUpdateTab_CaptureSoftware_SetByFile";
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.TabIndex = 5;
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.Text = "Set By File";
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_CaptureSoftware_SetByFile.Click += Button_CaptureSoftware_SetByFile_Click;
            // 
            // Button_KeywordUpdateTab_CaptureSoftware_SetAll
            // 
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.Location = new System.Drawing.Point(34, 150);
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.Name = "Button_KeywordUpdateTab_CaptureSoftware_SetAll";
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.TabIndex = 4;
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.Text = "Set All";
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_CaptureSoftware_SetAll.Click += Button_CaptureSoftware_SetAll_Click;
            // 
            // RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager
            // 
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.AutoSize = true;
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Location = new System.Drawing.Point(23, 93);
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Name = "RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager";
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Size = new System.Drawing.Size(67, 19);
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.TabIndex = 3;
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.Text = "Voyager";
            RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap
            // 
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.AutoSize = true;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Location = new System.Drawing.Point(23, 117);
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Name = "RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap";
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Size = new System.Drawing.Size(76, 19);
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.TabIndex = 2;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.Text = "SharpCap";
            RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro
            // 
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.AutoSize = true;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Location = new System.Drawing.Point(23, 70);
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Name = "RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro";
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Size = new System.Drawing.Size(57, 19);
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.TabIndex = 1;
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.Text = "SGPro";
            RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX
            // 
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.AutoSize = true;
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Location = new System.Drawing.Point(23, 24);
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Name = "RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX";
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Size = new System.Drawing.Size(72, 19);
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.TabIndex = 0;
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.Text = "The SkyX";
            RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX.UseVisualStyleBackColor = true;
            // 
            // GroupBox_KeywordUpdateTab_Telescope
            // 
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(TextBox_KeywordUpdateTab_Telescope_FocalLength);
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(Label_KeywordUpdateTab_Telescope_FocalLength);
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(Button_KeywordUpdateTab_Telescope_SetByFile);
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(Button_KeywordUpdateTab_Telescope_SetAll);
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(CheckBox_KeywordUpdateTab_Telescope_Riccardi);
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(RadioButton_KeywordUpdateTab_Telescope_Newtonian254);
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(RadioButton_KeywordUpdateTab_Telescope_EvoStar150);
            GroupBox_KeywordUpdateTab_Telescope.Controls.Add(RadioButton_KeywordUpdateTab_Telescope_APM107);
            GroupBox_KeywordUpdateTab_Telescope.Location = new System.Drawing.Point(176, 187);
            GroupBox_KeywordUpdateTab_Telescope.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_Telescope.Name = "GroupBox_KeywordUpdateTab_Telescope";
            GroupBox_KeywordUpdateTab_Telescope.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_Telescope.Size = new System.Drawing.Size(211, 216);
            GroupBox_KeywordUpdateTab_Telescope.TabIndex = 21;
            GroupBox_KeywordUpdateTab_Telescope.TabStop = false;
            GroupBox_KeywordUpdateTab_Telescope.Text = "Telescope";
            // 
            // TextBox_KeywordUpdateTab_Telescope_FocalLength
            // 
            TextBox_KeywordUpdateTab_Telescope_FocalLength.FormattingEnabled = true;
            TextBox_KeywordUpdateTab_Telescope_FocalLength.Location = new System.Drawing.Point(39, 139);
            TextBox_KeywordUpdateTab_Telescope_FocalLength.Name = "TextBox_KeywordUpdateTab_Telescope_FocalLength";
            TextBox_KeywordUpdateTab_Telescope_FocalLength.Size = new System.Drawing.Size(67, 23);
            TextBox_KeywordUpdateTab_Telescope_FocalLength.TabIndex = 19;
            // 
            // Label_KeywordUpdateTab_Telescope_FocalLength
            // 
            Label_KeywordUpdateTab_Telescope_FocalLength.AutoSize = true;
            Label_KeywordUpdateTab_Telescope_FocalLength.Location = new System.Drawing.Point(110, 143);
            Label_KeywordUpdateTab_Telescope_FocalLength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Telescope_FocalLength.Name = "Label_KeywordUpdateTab_Telescope_FocalLength";
            Label_KeywordUpdateTab_Telescope_FocalLength.Size = new System.Drawing.Size(75, 15);
            Label_KeywordUpdateTab_Telescope_FocalLength.TabIndex = 18;
            Label_KeywordUpdateTab_Telescope_FocalLength.Text = "Focal Length";
            // 
            // Button_KeywordUpdateTab_Telescope_SetByFile
            // 
            Button_KeywordUpdateTab_Telescope_SetByFile.Location = new System.Drawing.Point(108, 181);
            Button_KeywordUpdateTab_Telescope_SetByFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_Telescope_SetByFile.Name = "Button_KeywordUpdateTab_Telescope_SetByFile";
            Button_KeywordUpdateTab_Telescope_SetByFile.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_Telescope_SetByFile.TabIndex = 17;
            Button_KeywordUpdateTab_Telescope_SetByFile.Text = "Set By File";
            Button_KeywordUpdateTab_Telescope_SetByFile.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_Telescope_SetByFile.Click += Button_Telescope_SetByFile_Click;
            // 
            // Button_KeywordUpdateTab_Telescope_SetAll
            // 
            Button_KeywordUpdateTab_Telescope_SetAll.Location = new System.Drawing.Point(13, 181);
            Button_KeywordUpdateTab_Telescope_SetAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_Telescope_SetAll.Name = "Button_KeywordUpdateTab_Telescope_SetAll";
            Button_KeywordUpdateTab_Telescope_SetAll.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_Telescope_SetAll.TabIndex = 16;
            Button_KeywordUpdateTab_Telescope_SetAll.Text = "Set All";
            Button_KeywordUpdateTab_Telescope_SetAll.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_Telescope_SetAll.Click += Button_Telescope_SetAll_Click;
            // 
            // CheckBox_KeywordUpdateTab_Telescope_Riccardi
            // 
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.AutoSize = true;
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Location = new System.Drawing.Point(20, 114);
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Name = "CheckBox_KeywordUpdateTab_Telescope_Riccardi";
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Size = new System.Drawing.Size(138, 19);
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.TabIndex = 3;
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.Text = "Riccardi 0.75 Reducer";
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.UseVisualStyleBackColor = true;
            CheckBox_KeywordUpdateTab_Telescope_Riccardi.CheckedChanged += CheckBox_KeywordTelescope_Riccardi_CheckedChanged;
            // 
            // RadioButton_KeywordUpdateTab_Telescope_Newtonian254
            // 
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.AutoSize = true;
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Location = new System.Drawing.Point(20, 84);
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Name = "RadioButton_KeywordUpdateTab_Telescope_Newtonian254";
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Size = new System.Drawing.Size(104, 19);
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.TabIndex = 2;
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.Text = "Newtonian 254";
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.UseVisualStyleBackColor = true;
            RadioButton_KeywordUpdateTab_Telescope_Newtonian254.CheckedChanged += RadioButton_KeywordTelescope_NWT254_CheckedChanged;
            // 
            // RadioButton_KeywordUpdateTab_Telescope_EvoStar150
            // 
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.AutoSize = true;
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Location = new System.Drawing.Point(20, 54);
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Name = "RadioButton_KeywordUpdateTab_Telescope_EvoStar150";
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Size = new System.Drawing.Size(85, 19);
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.TabIndex = 1;
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.Text = "EvoStar 150";
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.UseVisualStyleBackColor = true;
            RadioButton_KeywordUpdateTab_Telescope_EvoStar150.CheckedChanged += RadioButton_KeywordTelescope_EVO150_CheckedChanged;
            // 
            // RadioButton_KeywordUpdateTab_Telescope_APM107
            // 
            RadioButton_KeywordUpdateTab_Telescope_APM107.AutoSize = true;
            RadioButton_KeywordUpdateTab_Telescope_APM107.Location = new System.Drawing.Point(20, 24);
            RadioButton_KeywordUpdateTab_Telescope_APM107.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_Telescope_APM107.Name = "RadioButton_KeywordUpdateTab_Telescope_APM107";
            RadioButton_KeywordUpdateTab_Telescope_APM107.Size = new System.Drawing.Size(72, 19);
            RadioButton_KeywordUpdateTab_Telescope_APM107.TabIndex = 0;
            RadioButton_KeywordUpdateTab_Telescope_APM107.Text = "APM 107";
            RadioButton_KeywordUpdateTab_Telescope_APM107.UseVisualStyleBackColor = true;
            RadioButton_KeywordUpdateTab_Telescope_APM107.CheckedChanged += RadioButton_KeywordTelescope_APM107_CheckedChanged;
            // 
            // GroupBox_KeywordUpdateTab_SubFrameKeywords
            // 
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(GroupBox_SubFrameKeywords_CalibrationFiles);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(Button_KeywordUpdateTab_SubFrameKeywords_Delete);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(Button_KeywordUpdateTab_SubFrameKeywords_AddReplace);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Controls.Add(Label_KeywordUpdateTab_SubFrameKeywords_TagetName);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Location = new System.Drawing.Point(20, 15);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Name = "GroupBox_KeywordUpdateTab_SubFrameKeywords";
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Size = new System.Drawing.Size(1092, 165);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.TabIndex = 14;
            GroupBox_KeywordUpdateTab_SubFrameKeywords.TabStop = false;
            GroupBox_KeywordUpdateTab_SubFrameKeywords.Text = "SubFrame Keywords";
            // 
            // CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName
            // 
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.AutoSize = true;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Location = new System.Drawing.Point(39, 88);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Name = "CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Size = new System.Drawing.Size(151, 19);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.TabIndex = 31;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.Text = "Do Not Update CPANEL";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue
            // 
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.AutoSize = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Location = new System.Drawing.Point(631, 111);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Name = "RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Size = new System.Drawing.Size(97, 19);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.TabIndex = 30;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.Text = "Specific Value";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues
            // 
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.AutoSize = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Checked = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Location = new System.Drawing.Point(547, 111);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Name = "RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Size = new System.Drawing.Size(75, 19);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.TabIndex = 29;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.TabStop = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.Text = "All Values";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues.UseVisualStyleBackColor = true;
            // 
            // ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment
            // 
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Location = new System.Drawing.Point(517, 85);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Name = "ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Size = new System.Drawing.Size(252, 23);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Sorted = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.TabIndex = 28;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment.Text = "Comment";
            // 
            // ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue
            // 
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Location = new System.Drawing.Point(517, 53);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Name = "ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Size = new System.Drawing.Size(252, 23);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Sorted = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.TabIndex = 27;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.Text = "Value";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue.SelectedValueChanged += ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue_SelectedValueChanged;
            // 
            // GroupBox_SubFrameKeywords_CalibrationFiles
            // 
            GroupBox_SubFrameKeywords_CalibrationFiles.Controls.Add(Button_SubFrameKeywords_CalibrationFiles_ClearAll);
            GroupBox_SubFrameKeywords_CalibrationFiles.Location = new System.Drawing.Point(237, 105);
            GroupBox_SubFrameKeywords_CalibrationFiles.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_SubFrameKeywords_CalibrationFiles.Name = "GroupBox_SubFrameKeywords_CalibrationFiles";
            GroupBox_SubFrameKeywords_CalibrationFiles.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_SubFrameKeywords_CalibrationFiles.Size = new System.Drawing.Size(252, 53);
            GroupBox_SubFrameKeywords_CalibrationFiles.TabIndex = 25;
            GroupBox_SubFrameKeywords_CalibrationFiles.TabStop = false;
            GroupBox_SubFrameKeywords_CalibrationFiles.Text = "Calibration Files and Keywords";
            // 
            // Button_SubFrameKeywords_CalibrationFiles_ClearAll
            // 
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.Location = new System.Drawing.Point(44, 20);
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.Name = "Button_SubFrameKeywords_CalibrationFiles_ClearAll";
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.Size = new System.Drawing.Size(168, 27);
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.TabIndex = 0;
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.Text = "Delete All Calibration Data";
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.UseVisualStyleBackColor = true;
            Button_SubFrameKeywords_CalibrationFiles_ClearAll.Click += Button_SubFrameKeywords_CalibrationFiles_ClearAll_Click;
            // 
            // GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection
            // 
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Controls.Add(RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Controls.Add(RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Controls.Add(CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Location = new System.Drawing.Point(237, 48);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Name = "GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection";
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Size = new System.Drawing.Size(252, 48);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.TabIndex = 24;
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.TabStop = false;
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.Text = "Keyword Protection";
            // 
            // RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew
            // 
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.AutoSize = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Checked = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Location = new System.Drawing.Point(136, 20);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Name = "RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Size = new System.Drawing.Size(90, 19);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.TabIndex = 24;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.TabStop = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.Text = "Update New";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.UseVisualStyleBackColor = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew.CheckedChanged += RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew_CheckedChanged;
            // 
            // RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All
            // 
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.AutoSize = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.Location = new System.Drawing.Point(91, 20);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.Name = "RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.Size = new System.Drawing.Size(39, 19);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.TabIndex = 23;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.Text = "All";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All.UseVisualStyleBackColor = true;
            // 
            // CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect
            // 
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.AutoSize = true;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Checked = true;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Location = new System.Drawing.Point(12, 21);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Name = "CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Size = new System.Drawing.Size(64, 19);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.TabIndex = 22;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.Text = "Protect";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.UseVisualStyleBackColor = true;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect.CheckedChanged += CheckBox_KeywordUpdate_SubFrameKeywords_Protect_CheckedChanged;
            // 
            // CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords
            // 
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.AutoSize = true;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.Checked = true;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.CheckState = System.Windows.Forms.CheckState.Checked;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.Location = new System.Drawing.Point(248, 21);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.Name = "CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.Size = new System.Drawing.Size(142, 19);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.TabIndex = 23;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.Text = "Alphabetize Keywords";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords.UseVisualStyleBackColor = true;
            // 
            // GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights
            // 
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Controls.Add(RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Controls.Add(Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Controls.Add(RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Controls.Add(RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Controls.Add(Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Controls.Add(ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Location = new System.Drawing.Point(798, 14);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Name = "GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights";
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Size = new System.Drawing.Size(279, 134);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.TabIndex = 7;
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.TabStop = false;
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.Text = "Weights";
            // 
            // RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration
            // 
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.AutoSize = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.Location = new System.Drawing.Point(184, 60);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.Name = "RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.Size = new System.Drawing.Size(83, 19);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.TabIndex = 21;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.TabStop = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.Text = "Calibration";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration.UseVisualStyleBackColor = true;
            // 
            // Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove
            // 
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.Location = new System.Drawing.Point(41, 86);
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.Name = "Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove";
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.TabIndex = 20;
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.Text = "Remove";
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove.Click += Button_KeywordSubFrameWeight_Remove_Click;
            // 
            // RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected
            // 
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.AutoSize = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.Location = new System.Drawing.Point(184, 85);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.Name = "RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.Size = new System.Drawing.Size(69, 19);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.TabIndex = 9;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.TabStop = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.Text = "Selected";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All
            // 
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.AutoSize = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.Location = new System.Drawing.Point(184, 34);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.Name = "RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.Size = new System.Drawing.Size(39, 19);
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.TabIndex = 8;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.TabStop = true;
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.Text = "All";
            RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All.UseVisualStyleBackColor = true;
            // 
            // Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords
            // 
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.AutoSize = true;
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Location = new System.Drawing.Point(36, 36);
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Name = "Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords";
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Size = new System.Drawing.Size(99, 15);
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.TabIndex = 6;
            Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Text = "Weight Keywords";
            // 
            // ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords
            // 
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Location = new System.Drawing.Point(15, 58);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Name = "ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Size = new System.Drawing.Size(140, 23);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.Sorted = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords.TabIndex = 5;
            // 
            // Button_KeywordUpdateTab_SubFrameKeywords_Delete
            // 
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.Location = new System.Drawing.Point(662, 132);
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.Name = "Button_KeywordUpdateTab_SubFrameKeywords_Delete";
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.Size = new System.Drawing.Size(108, 27);
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.TabIndex = 21;
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.Text = "Delete";
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_SubFrameKeywords_Delete.Click += Button_KeywordUpdateTab_SubFrameKeywords_Delete_Click;
            // 
            // Button_KeywordUpdateTab_SubFrameKeywords_AddReplace
            // 
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.Location = new System.Drawing.Point(517, 132);
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.Name = "Button_KeywordUpdateTab_SubFrameKeywords_AddReplace";
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.Size = new System.Drawing.Size(126, 27);
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.TabIndex = 20;
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.Text = "Add/Replace";
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_SubFrameKeywords_AddReplace.Click += Button_KeywordUpdateTab_SubFrameKeywords_AddReplace_Click;
            // 
            // ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName
            // 
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Location = new System.Drawing.Point(517, 21);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Name = "ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Size = new System.Drawing.Size(252, 23);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Sorted = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.TabIndex = 18;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.Text = "Keyword";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName.SelectedIndexChanged += ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName_SelectedIndexChanged;
            // 
            // CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName
            // 
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.AutoSize = true;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Location = new System.Drawing.Point(47, 66);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Name = "CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Size = new System.Drawing.Size(134, 19);
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.TabIndex = 17;
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.Text = "Update Target Name";
            CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName.UseVisualStyleBackColor = true;
            // 
            // Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords
            // 
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.Location = new System.Drawing.Point(17, 124);
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.Name = "Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords";
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.Size = new System.Drawing.Size(195, 27);
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.TabIndex = 4;
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.Text = "Update XISF File Keywords";
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords.Click += Button_KeywordSubFrame_UpdateXisfFiles_Click;
            // 
            // ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames
            // 
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.AllowDrop = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Location = new System.Drawing.Point(17, 40);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Name = "ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames";
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Size = new System.Drawing.Size(194, 23);
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.Sorted = true;
            ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames.TabIndex = 5;
            // 
            // Label_KeywordUpdateTab_SubFrameKeywords_TagetName
            // 
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.AllowDrop = true;
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.AutoSize = true;
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.Location = new System.Drawing.Point(77, 22);
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.Name = "Label_KeywordUpdateTab_SubFrameKeywords_TagetName";
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.Size = new System.Drawing.Size(74, 15);
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.TabIndex = 0;
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.Text = "Target Name";
            Label_KeywordUpdateTab_SubFrameKeywords_TagetName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GroupBox_KeywordUpdateTab_Camera
            // 
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_A144Binning);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Q178Binning);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z183Binning);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z533Binning);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_A144SensorTemp);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Q178Offset);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z183Offset);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z533Offset);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Q178Gain);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z183Gain);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z533Gain);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_A144Seconds);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Q178Seconds);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z533Seconds);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(ComboBox_KeywordUpdateTab_Camera_Z183Seconds);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_ToggleNBPreset);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_Seconds);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(CheckBox_KeywordUpdateTab_Camera_A144);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(CheckBox_KeywordUpdateTab_Camera_Q178);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(CheckBox_KeywordUpdateTab_Camera_Z183);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(CheckBox_KeywordUpdateTab_Camera_Z533);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_Binning);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_SensorTemp);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_Camera);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Button_KeywordUpdateTab_Camera_SetByFile);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Button_KeywordUpdateTab_Camera_SetAll);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_A144Gain);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_Offset);
            GroupBox_KeywordUpdateTab_Camera.Controls.Add(Label_KeywordUpdateTab_Camera_Gain);
            GroupBox_KeywordUpdateTab_Camera.Location = new System.Drawing.Point(395, 187);
            GroupBox_KeywordUpdateTab_Camera.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_Camera.Name = "GroupBox_KeywordUpdateTab_Camera";
            GroupBox_KeywordUpdateTab_Camera.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_Camera.Size = new System.Drawing.Size(385, 216);
            GroupBox_KeywordUpdateTab_Camera.TabIndex = 20;
            GroupBox_KeywordUpdateTab_Camera.TabStop = false;
            GroupBox_KeywordUpdateTab_Camera.Text = "Camera";
            // 
            // ComboBox_KeywordUpdateTab_Camera_A144Binning
            // 
            ComboBox_KeywordUpdateTab_Camera_A144Binning.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_A144Binning.Location = new System.Drawing.Point(330, 133);
            ComboBox_KeywordUpdateTab_Camera_A144Binning.Name = "ComboBox_KeywordUpdateTab_Camera_A144Binning";
            ComboBox_KeywordUpdateTab_Camera_A144Binning.Size = new System.Drawing.Size(44, 23);
            ComboBox_KeywordUpdateTab_Camera_A144Binning.TabIndex = 58;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Q178Binning
            // 
            ComboBox_KeywordUpdateTab_Camera_Q178Binning.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Q178Binning.Location = new System.Drawing.Point(330, 103);
            ComboBox_KeywordUpdateTab_Camera_Q178Binning.Name = "ComboBox_KeywordUpdateTab_Camera_Q178Binning";
            ComboBox_KeywordUpdateTab_Camera_Q178Binning.Size = new System.Drawing.Size(44, 23);
            ComboBox_KeywordUpdateTab_Camera_Q178Binning.TabIndex = 57;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z183Binning
            // 
            ComboBox_KeywordUpdateTab_Camera_Z183Binning.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z183Binning.Location = new System.Drawing.Point(330, 73);
            ComboBox_KeywordUpdateTab_Camera_Z183Binning.Name = "ComboBox_KeywordUpdateTab_Camera_Z183Binning";
            ComboBox_KeywordUpdateTab_Camera_Z183Binning.Size = new System.Drawing.Size(44, 23);
            ComboBox_KeywordUpdateTab_Camera_Z183Binning.TabIndex = 56;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z533Binning
            // 
            ComboBox_KeywordUpdateTab_Camera_Z533Binning.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z533Binning.Location = new System.Drawing.Point(330, 43);
            ComboBox_KeywordUpdateTab_Camera_Z533Binning.Name = "ComboBox_KeywordUpdateTab_Camera_Z533Binning";
            ComboBox_KeywordUpdateTab_Camera_Z533Binning.Size = new System.Drawing.Size(44, 23);
            ComboBox_KeywordUpdateTab_Camera_Z533Binning.TabIndex = 55;
            // 
            // ComboBox_KeywordUpdateTab_Camera_A144SensorTemp
            // 
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.Location = new System.Drawing.Point(265, 133);
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.Name = "ComboBox_KeywordUpdateTab_Camera_A144SensorTemp";
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_A144SensorTemp.TabIndex = 54;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp
            // 
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.Location = new System.Drawing.Point(265, 103);
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.Name = "ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp";
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp.TabIndex = 53;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp
            // 
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.Location = new System.Drawing.Point(265, 73);
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.Name = "ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp";
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp.TabIndex = 52;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp
            // 
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Location = new System.Drawing.Point(265, 43);
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Name = "ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp";
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp.TabIndex = 51;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Q178Offset
            // 
            ComboBox_KeywordUpdateTab_Camera_Q178Offset.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Q178Offset.Location = new System.Drawing.Point(204, 103);
            ComboBox_KeywordUpdateTab_Camera_Q178Offset.Name = "ComboBox_KeywordUpdateTab_Camera_Q178Offset";
            ComboBox_KeywordUpdateTab_Camera_Q178Offset.Size = new System.Drawing.Size(51, 23);
            ComboBox_KeywordUpdateTab_Camera_Q178Offset.TabIndex = 50;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z183Offset
            // 
            ComboBox_KeywordUpdateTab_Camera_Z183Offset.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z183Offset.Location = new System.Drawing.Point(204, 73);
            ComboBox_KeywordUpdateTab_Camera_Z183Offset.Name = "ComboBox_KeywordUpdateTab_Camera_Z183Offset";
            ComboBox_KeywordUpdateTab_Camera_Z183Offset.Size = new System.Drawing.Size(51, 23);
            ComboBox_KeywordUpdateTab_Camera_Z183Offset.TabIndex = 49;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z533Offset
            // 
            ComboBox_KeywordUpdateTab_Camera_Z533Offset.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z533Offset.Location = new System.Drawing.Point(204, 43);
            ComboBox_KeywordUpdateTab_Camera_Z533Offset.Name = "ComboBox_KeywordUpdateTab_Camera_Z533Offset";
            ComboBox_KeywordUpdateTab_Camera_Z533Offset.Size = new System.Drawing.Size(51, 23);
            ComboBox_KeywordUpdateTab_Camera_Z533Offset.TabIndex = 48;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Q178Gain
            // 
            ComboBox_KeywordUpdateTab_Camera_Q178Gain.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Q178Gain.Location = new System.Drawing.Point(144, 103);
            ComboBox_KeywordUpdateTab_Camera_Q178Gain.Name = "ComboBox_KeywordUpdateTab_Camera_Q178Gain";
            ComboBox_KeywordUpdateTab_Camera_Q178Gain.Size = new System.Drawing.Size(51, 23);
            ComboBox_KeywordUpdateTab_Camera_Q178Gain.TabIndex = 47;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z183Gain
            // 
            ComboBox_KeywordUpdateTab_Camera_Z183Gain.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z183Gain.Location = new System.Drawing.Point(144, 73);
            ComboBox_KeywordUpdateTab_Camera_Z183Gain.Name = "ComboBox_KeywordUpdateTab_Camera_Z183Gain";
            ComboBox_KeywordUpdateTab_Camera_Z183Gain.Size = new System.Drawing.Size(51, 23);
            ComboBox_KeywordUpdateTab_Camera_Z183Gain.TabIndex = 46;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z533Gain
            // 
            ComboBox_KeywordUpdateTab_Camera_Z533Gain.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z533Gain.Location = new System.Drawing.Point(144, 43);
            ComboBox_KeywordUpdateTab_Camera_Z533Gain.Name = "ComboBox_KeywordUpdateTab_Camera_Z533Gain";
            ComboBox_KeywordUpdateTab_Camera_Z533Gain.Size = new System.Drawing.Size(51, 23);
            ComboBox_KeywordUpdateTab_Camera_Z533Gain.TabIndex = 45;
            // 
            // ComboBox_KeywordUpdateTab_Camera_A144Seconds
            // 
            ComboBox_KeywordUpdateTab_Camera_A144Seconds.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_A144Seconds.Location = new System.Drawing.Point(78, 133);
            ComboBox_KeywordUpdateTab_Camera_A144Seconds.Name = "ComboBox_KeywordUpdateTab_Camera_A144Seconds";
            ComboBox_KeywordUpdateTab_Camera_A144Seconds.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_A144Seconds.TabIndex = 44;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Q178Seconds
            // 
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds.Location = new System.Drawing.Point(78, 103);
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds.Name = "ComboBox_KeywordUpdateTab_Camera_Q178Seconds";
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_Q178Seconds.TabIndex = 43;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z533Seconds
            // 
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Location = new System.Drawing.Point(78, 43);
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Name = "ComboBox_KeywordUpdateTab_Camera_Z533Seconds";
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_Z533Seconds.TabIndex = 42;
            // 
            // ComboBox_KeywordUpdateTab_Camera_Z183Seconds
            // 
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds.FormattingEnabled = true;
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds.Location = new System.Drawing.Point(78, 73);
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds.Name = "ComboBox_KeywordUpdateTab_Camera_Z183Seconds";
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds.Size = new System.Drawing.Size(55, 23);
            ComboBox_KeywordUpdateTab_Camera_Z183Seconds.TabIndex = 41;
            // 
            // Label_KeywordUpdateTab_Camera_ToggleNBPreset
            // 
            Label_KeywordUpdateTab_Camera_ToggleNBPreset.AutoSize = true;
            Label_KeywordUpdateTab_Camera_ToggleNBPreset.Location = new System.Drawing.Point(323, 185);
            Label_KeywordUpdateTab_Camera_ToggleNBPreset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_ToggleNBPreset.Name = "Label_KeywordUpdateTab_Camera_ToggleNBPreset";
            Label_KeywordUpdateTab_Camera_ToggleNBPreset.Size = new System.Drawing.Size(39, 15);
            Label_KeywordUpdateTab_Camera_ToggleNBPreset.TabIndex = 25;
            Label_KeywordUpdateTab_Camera_ToggleNBPreset.Text = "Preset";
            // 
            // Label_KeywordUpdateTab_Camera_Seconds
            // 
            Label_KeywordUpdateTab_Camera_Seconds.AutoSize = true;
            Label_KeywordUpdateTab_Camera_Seconds.Location = new System.Drawing.Point(80, 21);
            Label_KeywordUpdateTab_Camera_Seconds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_Seconds.Name = "Label_KeywordUpdateTab_Camera_Seconds";
            Label_KeywordUpdateTab_Camera_Seconds.Size = new System.Drawing.Size(51, 15);
            Label_KeywordUpdateTab_Camera_Seconds.TabIndex = 21;
            Label_KeywordUpdateTab_Camera_Seconds.Text = "Seconds";
            // 
            // Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB
            // 
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.Location = new System.Drawing.Point(265, 181);
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.Name = "Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB";
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.Size = new System.Drawing.Size(56, 23);
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.TabIndex = 24;
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.Text = "Set";
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.UseVisualStyleBackColor = true;
            Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB.Click += Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB_Click;
            // 
            // CheckBox_KeywordUpdateTab_Camera_A144
            // 
            CheckBox_KeywordUpdateTab_Camera_A144.AutoSize = true;
            CheckBox_KeywordUpdateTab_Camera_A144.Location = new System.Drawing.Point(16, 135);
            CheckBox_KeywordUpdateTab_Camera_A144.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_Camera_A144.Name = "CheckBox_KeywordUpdateTab_Camera_A144";
            CheckBox_KeywordUpdateTab_Camera_A144.Size = new System.Drawing.Size(52, 19);
            CheckBox_KeywordUpdateTab_Camera_A144.TabIndex = 31;
            CheckBox_KeywordUpdateTab_Camera_A144.Text = "A144";
            CheckBox_KeywordUpdateTab_Camera_A144.UseVisualStyleBackColor = true;
            // 
            // CheckBox_KeywordUpdateTab_Camera_Q178
            // 
            CheckBox_KeywordUpdateTab_Camera_Q178.AutoSize = true;
            CheckBox_KeywordUpdateTab_Camera_Q178.Location = new System.Drawing.Point(16, 105);
            CheckBox_KeywordUpdateTab_Camera_Q178.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_Camera_Q178.Name = "CheckBox_KeywordUpdateTab_Camera_Q178";
            CheckBox_KeywordUpdateTab_Camera_Q178.Size = new System.Drawing.Size(53, 19);
            CheckBox_KeywordUpdateTab_Camera_Q178.TabIndex = 30;
            CheckBox_KeywordUpdateTab_Camera_Q178.Text = "Q178";
            CheckBox_KeywordUpdateTab_Camera_Q178.UseVisualStyleBackColor = true;
            // 
            // CheckBox_KeywordUpdateTab_Camera_Z183
            // 
            CheckBox_KeywordUpdateTab_Camera_Z183.AutoSize = true;
            CheckBox_KeywordUpdateTab_Camera_Z183.Location = new System.Drawing.Point(16, 75);
            CheckBox_KeywordUpdateTab_Camera_Z183.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_Camera_Z183.Name = "CheckBox_KeywordUpdateTab_Camera_Z183";
            CheckBox_KeywordUpdateTab_Camera_Z183.Size = new System.Drawing.Size(51, 19);
            CheckBox_KeywordUpdateTab_Camera_Z183.TabIndex = 29;
            CheckBox_KeywordUpdateTab_Camera_Z183.Text = "Z183";
            CheckBox_KeywordUpdateTab_Camera_Z183.UseVisualStyleBackColor = true;
            // 
            // CheckBox_KeywordUpdateTab_Camera_Z533
            // 
            CheckBox_KeywordUpdateTab_Camera_Z533.AutoSize = true;
            CheckBox_KeywordUpdateTab_Camera_Z533.Location = new System.Drawing.Point(16, 45);
            CheckBox_KeywordUpdateTab_Camera_Z533.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CheckBox_KeywordUpdateTab_Camera_Z533.Name = "CheckBox_KeywordUpdateTab_Camera_Z533";
            CheckBox_KeywordUpdateTab_Camera_Z533.Size = new System.Drawing.Size(51, 19);
            CheckBox_KeywordUpdateTab_Camera_Z533.TabIndex = 28;
            CheckBox_KeywordUpdateTab_Camera_Z533.Text = "Z533";
            CheckBox_KeywordUpdateTab_Camera_Z533.UseVisualStyleBackColor = true;
            // 
            // Label_KeywordUpdateTab_Camera_Binning
            // 
            Label_KeywordUpdateTab_Camera_Binning.AutoSize = true;
            Label_KeywordUpdateTab_Camera_Binning.Location = new System.Drawing.Point(328, 21);
            Label_KeywordUpdateTab_Camera_Binning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_Binning.Name = "Label_KeywordUpdateTab_Camera_Binning";
            Label_KeywordUpdateTab_Camera_Binning.Size = new System.Drawing.Size(48, 15);
            Label_KeywordUpdateTab_Camera_Binning.TabIndex = 18;
            Label_KeywordUpdateTab_Camera_Binning.Text = "Binning";
            // 
            // Label_KeywordUpdateTab_Camera_SensorTemp
            // 
            Label_KeywordUpdateTab_Camera_SensorTemp.AutoSize = true;
            Label_KeywordUpdateTab_Camera_SensorTemp.Location = new System.Drawing.Point(257, 21);
            Label_KeywordUpdateTab_Camera_SensorTemp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_SensorTemp.Name = "Label_KeywordUpdateTab_Camera_SensorTemp";
            Label_KeywordUpdateTab_Camera_SensorTemp.Size = new System.Drawing.Size(71, 15);
            Label_KeywordUpdateTab_Camera_SensorTemp.TabIndex = 16;
            Label_KeywordUpdateTab_Camera_SensorTemp.Text = "SensorTemp";
            // 
            // Label_KeywordUpdateTab_Camera_Camera
            // 
            Label_KeywordUpdateTab_Camera_Camera.AutoSize = true;
            Label_KeywordUpdateTab_Camera_Camera.Location = new System.Drawing.Point(17, 21);
            Label_KeywordUpdateTab_Camera_Camera.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_Camera.Name = "Label_KeywordUpdateTab_Camera_Camera";
            Label_KeywordUpdateTab_Camera_Camera.Size = new System.Drawing.Size(48, 15);
            Label_KeywordUpdateTab_Camera_Camera.TabIndex = 23;
            Label_KeywordUpdateTab_Camera_Camera.Text = "Camera";
            // 
            // Button_KeywordUpdateTab_Camera_SetByFile
            // 
            Button_KeywordUpdateTab_Camera_SetByFile.Location = new System.Drawing.Point(126, 181);
            Button_KeywordUpdateTab_Camera_SetByFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_Camera_SetByFile.Name = "Button_KeywordUpdateTab_Camera_SetByFile";
            Button_KeywordUpdateTab_Camera_SetByFile.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_Camera_SetByFile.TabIndex = 19;
            Button_KeywordUpdateTab_Camera_SetByFile.Text = "Set By File";
            Button_KeywordUpdateTab_Camera_SetByFile.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_Camera_SetByFile.Click += Button_KeywordCamera_SetByFile_Click;
            // 
            // Button_KeywordUpdateTab_Camera_SetAll
            // 
            Button_KeywordUpdateTab_Camera_SetAll.Location = new System.Drawing.Point(30, 181);
            Button_KeywordUpdateTab_Camera_SetAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_Camera_SetAll.Name = "Button_KeywordUpdateTab_Camera_SetAll";
            Button_KeywordUpdateTab_Camera_SetAll.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_Camera_SetAll.TabIndex = 19;
            Button_KeywordUpdateTab_Camera_SetAll.Text = "Set All";
            Button_KeywordUpdateTab_Camera_SetAll.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_Camera_SetAll.Click += Button_KeywordCamera_SetAll_Click;
            // 
            // Label_KeywordUpdateTab_Camera_A144Gain
            // 
            Label_KeywordUpdateTab_Camera_A144Gain.AutoSize = true;
            Label_KeywordUpdateTab_Camera_A144Gain.Location = new System.Drawing.Point(155, 137);
            Label_KeywordUpdateTab_Camera_A144Gain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_A144Gain.Name = "Label_KeywordUpdateTab_Camera_A144Gain";
            Label_KeywordUpdateTab_Camera_A144Gain.Size = new System.Drawing.Size(28, 15);
            Label_KeywordUpdateTab_Camera_A144Gain.TabIndex = 12;
            Label_KeywordUpdateTab_Camera_A144Gain.Text = "0.37";
            // 
            // Label_KeywordUpdateTab_Camera_Offset
            // 
            Label_KeywordUpdateTab_Camera_Offset.AutoSize = true;
            Label_KeywordUpdateTab_Camera_Offset.Location = new System.Drawing.Point(210, 21);
            Label_KeywordUpdateTab_Camera_Offset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_Offset.Name = "Label_KeywordUpdateTab_Camera_Offset";
            Label_KeywordUpdateTab_Camera_Offset.Size = new System.Drawing.Size(39, 15);
            Label_KeywordUpdateTab_Camera_Offset.TabIndex = 11;
            Label_KeywordUpdateTab_Camera_Offset.Text = "Offset";
            // 
            // Label_KeywordUpdateTab_Camera_Gain
            // 
            Label_KeywordUpdateTab_Camera_Gain.AutoSize = true;
            Label_KeywordUpdateTab_Camera_Gain.Location = new System.Drawing.Point(154, 21);
            Label_KeywordUpdateTab_Camera_Gain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            Label_KeywordUpdateTab_Camera_Gain.Name = "Label_KeywordUpdateTab_Camera_Gain";
            Label_KeywordUpdateTab_Camera_Gain.Size = new System.Drawing.Size(31, 15);
            Label_KeywordUpdateTab_Camera_Gain.TabIndex = 10;
            Label_KeywordUpdateTab_Camera_Gain.Text = "Gain";
            // 
            // GroupBox_KeywordUpdateTab_ImageType
            // 
            GroupBox_KeywordUpdateTab_ImageType.Controls.Add(Button_KeywordUpdateTab_ImageType_SetByFile);
            GroupBox_KeywordUpdateTab_ImageType.Controls.Add(Button_KeywordUpdateTab_ImageType_SetAll);
            GroupBox_KeywordUpdateTab_ImageType.Controls.Add(GroupBox_KeywordUpdateTab_ImageType_Frame);
            GroupBox_KeywordUpdateTab_ImageType.Controls.Add(GroupBox_KeywordUpdateTab_ImageType_Filter);
            GroupBox_KeywordUpdateTab_ImageType.Location = new System.Drawing.Point(788, 187);
            GroupBox_KeywordUpdateTab_ImageType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_ImageType.Name = "GroupBox_KeywordUpdateTab_ImageType";
            GroupBox_KeywordUpdateTab_ImageType.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_ImageType.Size = new System.Drawing.Size(323, 216);
            GroupBox_KeywordUpdateTab_ImageType.TabIndex = 18;
            GroupBox_KeywordUpdateTab_ImageType.TabStop = false;
            GroupBox_KeywordUpdateTab_ImageType.Text = "Image Type";
            // 
            // Button_KeywordUpdateTab_ImageType_SetByFile
            // 
            Button_KeywordUpdateTab_ImageType_SetByFile.Location = new System.Drawing.Point(170, 181);
            Button_KeywordUpdateTab_ImageType_SetByFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_ImageType_SetByFile.Name = "Button_KeywordUpdateTab_ImageType_SetByFile";
            Button_KeywordUpdateTab_ImageType_SetByFile.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_ImageType_SetByFile.TabIndex = 18;
            Button_KeywordUpdateTab_ImageType_SetByFile.Text = "Set By File";
            Button_KeywordUpdateTab_ImageType_SetByFile.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_ImageType_SetByFile.Click += Button_KeywordImageTypeFrame_SetByFile_Click;
            // 
            // Button_KeywordUpdateTab_ImageType_SetAll
            // 
            Button_KeywordUpdateTab_ImageType_SetAll.Location = new System.Drawing.Point(68, 181);
            Button_KeywordUpdateTab_ImageType_SetAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_ImageType_SetAll.Name = "Button_KeywordUpdateTab_ImageType_SetAll";
            Button_KeywordUpdateTab_ImageType_SetAll.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_ImageType_SetAll.TabIndex = 18;
            Button_KeywordUpdateTab_ImageType_SetAll.Text = "Set All";
            Button_KeywordUpdateTab_ImageType_SetAll.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_ImageType_SetAll.Click += Button_KeywordImageTypeFrame_SetAll_Click;
            // 
            // GroupBox_KeywordUpdateTab_ImageType_Frame
            // 
            GroupBox_KeywordUpdateTab_ImageType_Frame.Controls.Add(Button_KeywordUpdateTab_ImageType_Frame_SetMaster);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Frame_Bias);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Frame_Flat);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Frame_Dark);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Frame_Light);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Location = new System.Drawing.Point(10, 100);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Name = "GroupBox_KeywordUpdateTab_ImageType_Frame";
            GroupBox_KeywordUpdateTab_ImageType_Frame.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_ImageType_Frame.Size = new System.Drawing.Size(299, 75);
            GroupBox_KeywordUpdateTab_ImageType_Frame.TabIndex = 11;
            GroupBox_KeywordUpdateTab_ImageType_Frame.TabStop = false;
            GroupBox_KeywordUpdateTab_ImageType_Frame.Text = "Frame";
            // 
            // Button_KeywordUpdateTab_ImageType_Frame_SetMaster
            // 
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.Location = new System.Drawing.Point(112, 43);
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.Name = "Button_KeywordUpdateTab_ImageType_Frame_SetMaster";
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.Size = new System.Drawing.Size(88, 27);
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.TabIndex = 4;
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.Text = "Set Master";
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.UseVisualStyleBackColor = true;
            Button_KeywordUpdateTab_ImageType_Frame_SetMaster.Click += Button_KeywordImageTypeFrame_SetMaster_Click;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Frame_Bias
            // 
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Location = new System.Drawing.Point(231, 20);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Name = "RadioButton_KeywordUpdateTab_ImageType_Frame_Bias";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Size = new System.Drawing.Size(46, 19);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.TabIndex = 3;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.Text = "Bias";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Bias.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Frame_Flat
            // 
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Location = new System.Drawing.Point(170, 20);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Name = "RadioButton_KeywordUpdateTab_ImageType_Frame_Flat";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Size = new System.Drawing.Size(44, 19);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.TabIndex = 2;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.Text = "Flat";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Flat.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Frame_Dark
            // 
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Location = new System.Drawing.Point(104, 20);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Name = "RadioButton_KeywordUpdateTab_ImageType_Frame_Dark";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Size = new System.Drawing.Size(49, 19);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.TabIndex = 1;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.Text = "Dark";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Dark.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Frame_Light
            // 
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Location = new System.Drawing.Point(35, 20);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Name = "RadioButton_KeywordUpdateTab_ImageType_Frame_Light";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Size = new System.Drawing.Size(52, 19);
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.TabIndex = 0;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.Text = "Light";
            RadioButton_KeywordUpdateTab_ImageType_Frame_Light.UseVisualStyleBackColor = true;
            // 
            // GroupBox_KeywordUpdateTab_ImageType_Filter
            // 
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_Luma);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_Red);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_S2);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_Ha);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_Blue);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_Green);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Controls.Add(RadioButton_KeywordUpdateTab_ImageType_Filter_O3);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Location = new System.Drawing.Point(10, 20);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Name = "GroupBox_KeywordUpdateTab_ImageType_Filter";
            GroupBox_KeywordUpdateTab_ImageType_Filter.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GroupBox_KeywordUpdateTab_ImageType_Filter.Size = new System.Drawing.Size(299, 81);
            GroupBox_KeywordUpdateTab_ImageType_Filter.TabIndex = 10;
            GroupBox_KeywordUpdateTab_ImageType_Filter.TabStop = false;
            GroupBox_KeywordUpdateTab_ImageType_Filter.Text = "Filter";
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_Luma
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Location = new System.Drawing.Point(34, 21);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_Luma";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Size = new System.Drawing.Size(55, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.TabIndex = 0;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.Text = "Luma";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Luma.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Location = new System.Drawing.Point(231, 51);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Size = new System.Drawing.Size(63, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.TabIndex = 8;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.Text = "Shutter";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_Red
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Location = new System.Drawing.Point(34, 51);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_Red";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Size = new System.Drawing.Size(45, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.TabIndex = 1;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.Text = "Red";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Red.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_S2
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Location = new System.Drawing.Point(231, 21);
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_S2";
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Size = new System.Drawing.Size(40, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.TabIndex = 6;
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.Text = "S II";
            RadioButton_KeywordUpdateTab_ImageType_Filter_S2.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_Ha
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Location = new System.Drawing.Point(108, 21);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_Ha";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Size = new System.Drawing.Size(40, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.TabIndex = 2;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.Text = "Ha";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Ha.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_Blue
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Location = new System.Drawing.Point(167, 51);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_Blue";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Size = new System.Drawing.Size(48, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.TabIndex = 5;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.Text = "Blue";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Blue.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_Green
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Location = new System.Drawing.Point(95, 51);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_Green";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Size = new System.Drawing.Size(56, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.TabIndex = 3;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.Text = "Green";
            RadioButton_KeywordUpdateTab_ImageType_Filter_Green.UseVisualStyleBackColor = true;
            // 
            // RadioButton_KeywordUpdateTab_ImageType_Filter_O3
            // 
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.AutoSize = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Location = new System.Drawing.Point(167, 21);
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Name = "RadioButton_KeywordUpdateTab_ImageType_Filter_O3";
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Size = new System.Drawing.Size(46, 19);
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.TabIndex = 4;
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.TabStop = true;
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.Text = "O III";
            RadioButton_KeywordUpdateTab_ImageType_Filter_O3.UseVisualStyleBackColor = true;
            // 
            // TabControl_Update_TargetScheduler
            // 
            TabControl_Update_TargetScheduler.Controls.Add(TabPage_KeywordUpdate);
            TabControl_Update_TargetScheduler.Controls.Add(TabPage_Calibration);
            TabControl_Update_TargetScheduler.Controls.Add(TabPage_TargetScheduler);
            TabControl_Update_TargetScheduler.Controls.Add(TabPage_SubFrameWeights);
            TabControl_Update_TargetScheduler.Location = new System.Drawing.Point(14, 241);
            TabControl_Update_TargetScheduler.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TabControl_Update_TargetScheduler.Name = "TabControl_Update_TargetScheduler";
            TabControl_Update_TargetScheduler.SelectedIndex = 0;
            TabControl_Update_TargetScheduler.Size = new System.Drawing.Size(1147, 526);
            TabControl_Update_TargetScheduler.TabIndex = 23;
            // 
            // TabPage_TargetScheduler
            // 
            TabPage_TargetScheduler.BackColor = System.Drawing.SystemColors.Control;
            TabPage_TargetScheduler.Controls.Add(GroupBox_SchedulerTab_Project);
            TabPage_TargetScheduler.Controls.Add(Label_SchedulerTab_PlansText);
            TabPage_TargetScheduler.Controls.Add(TreeView_SchedulerTab_PlansTree);
            TabPage_TargetScheduler.Controls.Add(Label_SchedulerTab_TargetsText);
            TabPage_TargetScheduler.Controls.Add(TreeView_SchedulerTab_TargetTree);
            TabPage_TargetScheduler.Controls.Add(Label_SchedulerTab_ProjectsText);
            TabPage_TargetScheduler.Controls.Add(Label_SchedulerTab_ProfilesText);
            TabPage_TargetScheduler.Controls.Add(TreeView_SchedulerTab_ProjectTree);
            TabPage_TargetScheduler.Controls.Add(TreeView_SchedulerTab_ProfileTree);
            TabPage_TargetScheduler.Controls.Add(Button_SchedulerTab_OpenDatabase);
            TabPage_TargetScheduler.Location = new System.Drawing.Point(4, 24);
            TabPage_TargetScheduler.Name = "TabPage_TargetScheduler";
            TabPage_TargetScheduler.Padding = new System.Windows.Forms.Padding(3);
            TabPage_TargetScheduler.Size = new System.Drawing.Size(1139, 498);
            TabPage_TargetScheduler.TabIndex = 3;
            TabPage_TargetScheduler.Text = "Target Scheduler";
            // 
            // GroupBox_SchedulerTab_Project
            // 
            GroupBox_SchedulerTab_Project.Controls.Add(CheckBox_Project_Active);
            GroupBox_SchedulerTab_Project.Controls.Add(GroupBox_Project_Priority);
            GroupBox_SchedulerTab_Project.Location = new System.Drawing.Point(759, 38);
            GroupBox_SchedulerTab_Project.Name = "GroupBox_SchedulerTab_Project";
            GroupBox_SchedulerTab_Project.Size = new System.Drawing.Size(360, 121);
            GroupBox_SchedulerTab_Project.TabIndex = 9;
            GroupBox_SchedulerTab_Project.TabStop = false;
            GroupBox_SchedulerTab_Project.Text = "Project";
            // 
            // CheckBox_Project_Active
            // 
            CheckBox_Project_Active.AutoSize = true;
            CheckBox_Project_Active.Location = new System.Drawing.Point(12, 22);
            CheckBox_Project_Active.Name = "CheckBox_Project_Active";
            CheckBox_Project_Active.Size = new System.Drawing.Size(59, 19);
            CheckBox_Project_Active.TabIndex = 1;
            CheckBox_Project_Active.Text = "Active";
            CheckBox_Project_Active.UseVisualStyleBackColor = true;
            // 
            // GroupBox_Project_Priority
            // 
            GroupBox_Project_Priority.Controls.Add(RadioButton_ProjectPriority_High);
            GroupBox_Project_Priority.Controls.Add(RadioButton_ProjectPriority_Normal);
            GroupBox_Project_Priority.Controls.Add(RadioButton_ProjectPriority_Low);
            GroupBox_Project_Priority.Location = new System.Drawing.Point(6, 44);
            GroupBox_Project_Priority.Name = "GroupBox_Project_Priority";
            GroupBox_Project_Priority.Size = new System.Drawing.Size(197, 45);
            GroupBox_Project_Priority.TabIndex = 0;
            GroupBox_Project_Priority.TabStop = false;
            GroupBox_Project_Priority.Text = "Priority";
            // 
            // RadioButton_ProjectPriority_High
            // 
            RadioButton_ProjectPriority_High.AutoSize = true;
            RadioButton_ProjectPriority_High.Location = new System.Drawing.Point(139, 18);
            RadioButton_ProjectPriority_High.Name = "RadioButton_ProjectPriority_High";
            RadioButton_ProjectPriority_High.Size = new System.Drawing.Size(51, 19);
            RadioButton_ProjectPriority_High.TabIndex = 2;
            RadioButton_ProjectPriority_High.TabStop = true;
            RadioButton_ProjectPriority_High.Text = "High";
            RadioButton_ProjectPriority_High.UseVisualStyleBackColor = true;
            // 
            // RadioButton_ProjectPriority_Normal
            // 
            RadioButton_ProjectPriority_Normal.AutoSize = true;
            RadioButton_ProjectPriority_Normal.Location = new System.Drawing.Point(67, 18);
            RadioButton_ProjectPriority_Normal.Name = "RadioButton_ProjectPriority_Normal";
            RadioButton_ProjectPriority_Normal.Size = new System.Drawing.Size(65, 19);
            RadioButton_ProjectPriority_Normal.TabIndex = 1;
            RadioButton_ProjectPriority_Normal.TabStop = true;
            RadioButton_ProjectPriority_Normal.Text = "Normal";
            RadioButton_ProjectPriority_Normal.UseVisualStyleBackColor = true;
            // 
            // RadioButton_ProjectPriority_Low
            // 
            RadioButton_ProjectPriority_Low.AutoSize = true;
            RadioButton_ProjectPriority_Low.Location = new System.Drawing.Point(13, 18);
            RadioButton_ProjectPriority_Low.Name = "RadioButton_ProjectPriority_Low";
            RadioButton_ProjectPriority_Low.Size = new System.Drawing.Size(47, 19);
            RadioButton_ProjectPriority_Low.TabIndex = 0;
            RadioButton_ProjectPriority_Low.TabStop = true;
            RadioButton_ProjectPriority_Low.Text = "Low";
            RadioButton_ProjectPriority_Low.UseVisualStyleBackColor = true;
            // 
            // Label_SchedulerTab_PlansText
            // 
            Label_SchedulerTab_PlansText.AutoSize = true;
            Label_SchedulerTab_PlansText.Location = new System.Drawing.Point(590, 21);
            Label_SchedulerTab_PlansText.Name = "Label_SchedulerTab_PlansText";
            Label_SchedulerTab_PlansText.Size = new System.Drawing.Size(86, 15);
            Label_SchedulerTab_PlansText.TabIndex = 8;
            Label_SchedulerTab_PlansText.Text = "Exposure Plans";
            // 
            // TreeView_SchedulerTab_PlansTree
            // 
            TreeView_SchedulerTab_PlansTree.Location = new System.Drawing.Point(529, 38);
            TreeView_SchedulerTab_PlansTree.Name = "TreeView_SchedulerTab_PlansTree";
            TreeView_SchedulerTab_PlansTree.Size = new System.Drawing.Size(208, 444);
            TreeView_SchedulerTab_PlansTree.TabIndex = 7;
            // 
            // Label_SchedulerTab_TargetsText
            // 
            Label_SchedulerTab_TargetsText.AutoSize = true;
            Label_SchedulerTab_TargetsText.Location = new System.Drawing.Point(403, 21);
            Label_SchedulerTab_TargetsText.Name = "Label_SchedulerTab_TargetsText";
            Label_SchedulerTab_TargetsText.Size = new System.Drawing.Size(44, 15);
            Label_SchedulerTab_TargetsText.TabIndex = 6;
            Label_SchedulerTab_TargetsText.Text = "Targets";
            // 
            // TreeView_SchedulerTab_TargetTree
            // 
            TreeView_SchedulerTab_TargetTree.Location = new System.Drawing.Point(332, 38);
            TreeView_SchedulerTab_TargetTree.Name = "TreeView_SchedulerTab_TargetTree";
            TreeView_SchedulerTab_TargetTree.Size = new System.Drawing.Size(187, 444);
            TreeView_SchedulerTab_TargetTree.TabIndex = 5;
            // 
            // Label_SchedulerTab_ProjectsText
            // 
            Label_SchedulerTab_ProjectsText.AutoSize = true;
            Label_SchedulerTab_ProjectsText.Location = new System.Drawing.Point(204, 21);
            Label_SchedulerTab_ProjectsText.Name = "Label_SchedulerTab_ProjectsText";
            Label_SchedulerTab_ProjectsText.Size = new System.Drawing.Size(49, 15);
            Label_SchedulerTab_ProjectsText.TabIndex = 4;
            Label_SchedulerTab_ProjectsText.Text = "Projects";
            // 
            // Label_SchedulerTab_ProfilesText
            // 
            Label_SchedulerTab_ProfilesText.AutoSize = true;
            Label_SchedulerTab_ProfilesText.Location = new System.Drawing.Point(44, 72);
            Label_SchedulerTab_ProfilesText.Name = "Label_SchedulerTab_ProfilesText";
            Label_SchedulerTab_ProfilesText.Size = new System.Drawing.Size(46, 15);
            Label_SchedulerTab_ProfilesText.TabIndex = 3;
            Label_SchedulerTab_ProfilesText.Text = "Profiles";
            // 
            // TreeView_SchedulerTab_ProjectTree
            // 
            TreeView_SchedulerTab_ProjectTree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            TreeView_SchedulerTab_ProjectTree.Location = new System.Drawing.Point(135, 38);
            TreeView_SchedulerTab_ProjectTree.Name = "TreeView_SchedulerTab_ProjectTree";
            TreeView_SchedulerTab_ProjectTree.Size = new System.Drawing.Size(187, 444);
            TreeView_SchedulerTab_ProjectTree.TabIndex = 2;
            TreeView_SchedulerTab_ProjectTree.DrawNode += TreeView_SchedulerTab_ProjectTree_DrawNode;
            // 
            // TreeView_SchedulerTab_ProfileTree
            // 
            TreeView_SchedulerTab_ProfileTree.Location = new System.Drawing.Point(10, 89);
            TreeView_SchedulerTab_ProfileTree.Name = "TreeView_SchedulerTab_ProfileTree";
            TreeView_SchedulerTab_ProfileTree.Size = new System.Drawing.Size(115, 141);
            TreeView_SchedulerTab_ProfileTree.TabIndex = 1;
            // 
            // Button_SchedulerTab_OpenDatabase
            // 
            Button_SchedulerTab_OpenDatabase.Location = new System.Drawing.Point(10, 15);
            Button_SchedulerTab_OpenDatabase.Name = "Button_SchedulerTab_OpenDatabase";
            Button_SchedulerTab_OpenDatabase.Size = new System.Drawing.Size(115, 40);
            Button_SchedulerTab_OpenDatabase.TabIndex = 0;
            Button_SchedulerTab_OpenDatabase.Text = "Open Scheduler Database";
            Button_SchedulerTab_OpenDatabase.UseVisualStyleBackColor = true;
            Button_SchedulerTab_OpenDatabase.Click += Button_SchedulerTab_OpenDatabase_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1170, 775);
            Controls.Add(TabControl_Update_TargetScheduler);
            Controls.Add(GroupBox_FileSelection);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "XISF File Manager";
            GroupBox_FileSelection_SequenceOrder.ResumeLayout(false);
            GroupBox_FileSelection_SequenceOrder.PerformLayout();
            GroupBox_FileSelection_Count.ResumeLayout(false);
            GroupBox_FileSelection_Count.PerformLayout();
            GroupBox_FileSelection_DirectorySelection.ResumeLayout(false);
            GroupBox_FileSelection_DirectorySelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_FileSelection_DirectorySelection_TotalFrames).EndInit();
            GroupBox_FileSelection_Statistics.ResumeLayout(false);
            GroupBox_FileSelection_Statistics.PerformLayout();
            GroupBox_FileSelection.ResumeLayout(false);
            GroupBox_FileSelection.PerformLayout();
            TabPage_SubFrameWeights.ResumeLayout(false);
            GroupBox_WeightCalculations.ResumeLayout(false);
            GroupBox_StarResidual.ResumeLayout(false);
            GroupBox_StarResidual.PerformLayout();
            GroupBox_FwhmWeight.ResumeLayout(false);
            GroupBox_FwhmWeight.PerformLayout();
            GroupBox_StarsWeight.ResumeLayout(false);
            GroupBox_StarsWeight.PerformLayout();
            GroupBox_EccentricityWeight.ResumeLayout(false);
            GroupBox_EccentricityWeight.PerformLayout();
            GroupBox_AirMassWeight.ResumeLayout(false);
            GroupBox_AirMassWeight.PerformLayout();
            GroupBox_NoiseWeight.ResumeLayout(false);
            GroupBox_NoiseWeight.PerformLayout();
            GroupBox_MedianWeight.ResumeLayout(false);
            GroupBox_MedianWeight.PerformLayout();
            GroupBox_SnrWeight.ResumeLayout(false);
            GroupBox_SnrWeight.PerformLayout();
            GroupBox_UpdateStatistics.ResumeLayout(false);
            GroupBox_UpdateStatistics.PerformLayout();
            GroupBox_InitialRejectionCriteria.ResumeLayout(false);
            GroupBox_InitialRejectionCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Snr).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_StarResidual).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Stars).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_AirMass).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Noise).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Median).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_Eccentricity).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumericUpDown_Rejection_FWHM).EndInit();
            TabPage_Calibration.ResumeLayout(false);
            TabPage_Calibration.PerformLayout();
            GroupBox_CalibrationTab_MatchingTolerance.ResumeLayout(false);
            GroupBox_CalibrationTab_MatchingTolerance.PerformLayout();
            TabPage_KeywordUpdate.ResumeLayout(false);
            TabPage_KeywordUpdate.PerformLayout();
            GroupBox_KeywordUpdateTab_CaptureSoftware.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_CaptureSoftware.PerformLayout();
            GroupBox_KeywordUpdateTab_Telescope.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_Telescope.PerformLayout();
            GroupBox_KeywordUpdateTab_SubFrameKeywords.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_SubFrameKeywords.PerformLayout();
            GroupBox_SubFrameKeywords_CalibrationFiles.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection.PerformLayout();
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights.PerformLayout();
            GroupBox_KeywordUpdateTab_Camera.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_Camera.PerformLayout();
            GroupBox_KeywordUpdateTab_ImageType.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_ImageType_Frame.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_ImageType_Frame.PerformLayout();
            GroupBox_KeywordUpdateTab_ImageType_Filter.ResumeLayout(false);
            GroupBox_KeywordUpdateTab_ImageType_Filter.PerformLayout();
            TabControl_Update_TargetScheduler.ResumeLayout(false);
            TabPage_TargetScheduler.ResumeLayout(false);
            TabPage_TargetScheduler.PerformLayout();
            GroupBox_SchedulerTab_Project.ResumeLayout(false);
            GroupBox_SchedulerTab_Project.PerformLayout();
            GroupBox_Project_Priority.ResumeLayout(false);
            GroupBox_Project_Priority.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ProgressBar ProgressBar_FileSelection_ReadProgress;
        private System.Windows.Forms.GroupBox GroupBox_FileSelection_SequenceOrder;
        private System.Windows.Forms.RadioButton RadioButton_FileSelection_SequenceNumbering_WeightOnly;
        private System.Windows.Forms.RadioButton RadioButton_FileSelection_SequenceNumbering_IndexOnly;
        private System.Windows.Forms.RadioButton RadioButton_FileSelection_SequenceNumbering_IndexWeight;
        private System.Windows.Forms.RadioButton RadioButton_FileSelection_SequenceNumbering_WeightIndex;
        private System.Windows.Forms.GroupBox GroupBox_FileSelection_DirectorySelection;
        private System.Windows.Forms.ComboBox ComboBox_FileSelection_DirectorySelection_RejectionAlgorithm;
        private System.Windows.Forms.NumericUpDown NumericUpDown_FileSelection_DirectorySelection_TotalFrames;
        private System.Windows.Forms.RadioButton RadioButton_DirectorySelection_MastersOnly;
        private System.Windows.Forms.RadioButton RadioButton_DirectorySelection_ExcludeMasters;
        private System.Windows.Forms.RadioButton RadioButton_DirectorySelection_AlFiles;
        private System.Windows.Forms.CheckBox CheckBox_FileSelection_DirectorySelection_Master;
        private System.Windows.Forms.Button Button_FileSelection_DirectorySelection_Browse;
        private System.Windows.Forms.CheckBox CheckBox_FileSelection_DirectorySelection_Recurse;
        private System.Windows.Forms.Button Button_FileSlection_Rename;
        private System.Windows.Forms.GroupBox GroupBox_FileSelection_Statistics;
        private System.Windows.Forms.Label Label_FileSelection_Statistics_Task;
        private System.Windows.Forms.Label Label_FileSelection_Statistics_SubFrameOverhead;
        private System.Windows.Forms.Label Label_FileSelection_Statistics_TempratureCompensation;
        private System.Windows.Forms.Label Label_FileSelection_BrowseFileName;
        private System.Windows.Forms.GroupBox GroupBox_FileSelection_Count;
        private System.Windows.Forms.RadioButton RadioButton_FileSelection_Index_ByFilter;
        private System.Windows.Forms.RadioButton RadioButton_FileSelection_Index_ByTime;
        private System.Windows.Forms.GroupBox GroupBox_FileSelection;
        private System.Windows.Forms.CheckBox CheckBox_FileSlection_NoTotals;
        private System.Windows.Forms.TabPage TabPage_SubFrameWeights;
        private System.Windows.Forms.GroupBox GroupBox_WeightCalculations;
        private System.Windows.Forms.GroupBox GroupBox_StarResidual;
        private System.Windows.Forms.Label Label_StarResidualMinValue;
        private System.Windows.Forms.Label Label_StarResidualMaxValue;
        private System.Windows.Forms.Label Label_StarResidualMedianValue;
        private System.Windows.Forms.Label Label_StarResidualMeanValue;
        private System.Windows.Forms.Label Label_StarResidualSigmaValue;
        private System.Windows.Forms.Label Label_StarResidualMin;
        private System.Windows.Forms.Label Label_StarResidualMax;
        private System.Windows.Forms.Label Label_StarResidualMedian;
        private System.Windows.Forms.Label Label_StarResidualMean;
        private System.Windows.Forms.Label Label_StarResidualSigma;
        private System.Windows.Forms.Label Label_StarResidualRangeLow;
        private System.Windows.Forms.Label Label_StarResidualRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarResidualRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarResidualRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_FwhmWeight;
        private System.Windows.Forms.Label Label_FwhmMinValue;
        private System.Windows.Forms.Label Label_FwhmMaxValue;
        private System.Windows.Forms.Label Label_FwhmMedianValue;
        private System.Windows.Forms.Label Label_FwhmMeanValue;
        private System.Windows.Forms.Label Label_FwhmSigmaValue;
        private System.Windows.Forms.Label Label_FwhmMin;
        private System.Windows.Forms.Label Label_FwhmMax;
        private System.Windows.Forms.Label Label_FwhmMedian;
        private System.Windows.Forms.Label Label_FwhmRangeLow;
        private System.Windows.Forms.Label Label_FwhmMean;
        private System.Windows.Forms.Label Label_FwhmRangeHigh;
        private System.Windows.Forms.TextBox TextBox_FwhmRangeHigh;
        private System.Windows.Forms.TextBox TextBox_FwhmRangeLow;
        private System.Windows.Forms.Label Label_FwhmSigma;
        private System.Windows.Forms.GroupBox GroupBox_StarsWeight;
        private System.Windows.Forms.Label Label_StarsMinValue;
        private System.Windows.Forms.Label Label_StarsMaxValue;
        private System.Windows.Forms.Label Label_StarsMedianValue;
        private System.Windows.Forms.Label Label_StarsMeanValue;
        private System.Windows.Forms.Label Label_StarsSigmaValue;
        private System.Windows.Forms.Label Label_StarsMin;
        private System.Windows.Forms.Label Label_StarsMax;
        private System.Windows.Forms.Label Label_StarsMedian;
        private System.Windows.Forms.Label Label_StarsMean;
        private System.Windows.Forms.Label Label_StarsSigma;
        private System.Windows.Forms.Label Label_StarRangeLow;
        private System.Windows.Forms.Label Label_StarRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_EccentricityWeight;
        private System.Windows.Forms.Label Label_EccentricityMinValue;
        private System.Windows.Forms.Label Label_EccentricityMaxValue;
        private System.Windows.Forms.Label Label_EccentricityMedianValue;
        private System.Windows.Forms.Label Label_EccentricityMeanValue;
        private System.Windows.Forms.Label Label_EccentricitySigmaValue;
        private System.Windows.Forms.Label Label_EccentricityMin;
        private System.Windows.Forms.Label Label_EccentricityMax;
        private System.Windows.Forms.Label Label_EccentricityMedian;
        private System.Windows.Forms.Label Label_EccentricityMean;
        private System.Windows.Forms.Label Label_EccentricitySigma;
        private System.Windows.Forms.Label Label_EccentricityRangeLow;
        private System.Windows.Forms.Label Label_EccentricityRangeHigh;
        private System.Windows.Forms.TextBox TextBox_EccentricityRangeHigh;
        private System.Windows.Forms.TextBox TextBox_EccentricityRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_AirMassWeight;
        private System.Windows.Forms.Label Label_AirMassMinValue;
        private System.Windows.Forms.Label Label_AirMassMaxValue;
        private System.Windows.Forms.Label Label_AirMassMedianValue;
        private System.Windows.Forms.Label Label_AirMassMeanValue;
        private System.Windows.Forms.Label Label_AirMassSigmaValue;
        private System.Windows.Forms.TextBox TextBox_AirMassRangeLow;
        private System.Windows.Forms.TextBox TextBox_AirMassRangeHigh;
        private System.Windows.Forms.Label Label_AirMassMin;
        private System.Windows.Forms.Label Label_AirMassMax;
        private System.Windows.Forms.Label Label_AirMassMedian;
        private System.Windows.Forms.Label Label_AirMassMean;
        private System.Windows.Forms.Label Label_AirMassSigma;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationRangeLow;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationRangeHigh;
        private System.Windows.Forms.GroupBox GroupBox_NoiseWeight;
        private System.Windows.Forms.Label Label_NoiseMinValue;
        private System.Windows.Forms.Label Label_NoiseMaxValue;
        private System.Windows.Forms.Label Label_NoiseMedianValue;
        private System.Windows.Forms.Label Label_NoiseMeanValue;
        private System.Windows.Forms.Label Label_NoiseSigmaValue;
        private System.Windows.Forms.Label Label_NoiseMin;
        private System.Windows.Forms.Label Label_NoiseMax;
        private System.Windows.Forms.Label Label_NoiseMedian;
        private System.Windows.Forms.Label Label_NoiseMean;
        private System.Windows.Forms.Label Label_NoiseSigma;
        private System.Windows.Forms.Label Label_NoiseRangeLow;
        private System.Windows.Forms.Label Label_NoiseRangeHigh;
        private System.Windows.Forms.TextBox TextBox_NoiseRangeHigh;
        private System.Windows.Forms.TextBox TextBox_NoiseRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_MedianWeight;
        private System.Windows.Forms.Label Label_MedianMinValue;
        private System.Windows.Forms.Label Label_MedianMaxValue;
        private System.Windows.Forms.Label Label_MedianMedianValue;
        private System.Windows.Forms.Label Label_MedianMeanValue;
        private System.Windows.Forms.Label Label_MedianSigmaValue;
        private System.Windows.Forms.Label Label_MedianMin;
        private System.Windows.Forms.Label Label_MedianMax;
        private System.Windows.Forms.Label Label_MedianMedian;
        private System.Windows.Forms.Label Label_MedianMean;
        private System.Windows.Forms.Label Label_MedianSigma;
        private System.Windows.Forms.Label Label_MedianRangeLow;
        private System.Windows.Forms.Label Label_MedianRangeHigh;
        private System.Windows.Forms.TextBox TextBox_MedianRangeHigh;
        private System.Windows.Forms.TextBox TextBox_MedianRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_SnrWeight;
        private System.Windows.Forms.Label Label_SnrMinValue;
        private System.Windows.Forms.Label Label_SnrMaxValue;
        private System.Windows.Forms.Label Label_SnrMedianValue;
        private System.Windows.Forms.Label Label_SnrMeanValue;
        private System.Windows.Forms.Label Label_SnrSigmaValue;
        private System.Windows.Forms.Label Label_SnrMin;
        private System.Windows.Forms.Label Label_SnrMax;
        private System.Windows.Forms.Label Label_SnrMedian;
        private System.Windows.Forms.Label Label_SnrMean;
        private System.Windows.Forms.Label Label_SnrSigma;
        private System.Windows.Forms.Label Label_SnrRangeLow;
        private System.Windows.Forms.Label Label_SnrRangeHigh;
        private System.Windows.Forms.TextBox TextBox_SnrRangeHigh;
        private System.Windows.Forms.TextBox TextBox_SnrRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_UpdateStatistics;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_CalculateWeights;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_RescaleWeights;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_KeepWeights;
        private System.Windows.Forms.Button Button_ReadSubFrameSelectorCsvFile;
        private System.Windows.Forms.Label Label_UpdateStatistics;
        private System.Windows.Forms.Label Label_UpdateStatisticsRangeHigh;
        private System.Windows.Forms.TextBox TextBox_UpdateStatisticsRangeHigh;
        private System.Windows.Forms.TextBox TextBox_UpdateStatisticsRangeLow;
        private System.Windows.Forms.Label Label_UpdateStatisticsRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_InitialRejectionCriteria;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Snr;
        private System.Windows.Forms.Label Label_Rejection_SNR;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_StarResidual;
        private System.Windows.Forms.Label Label_Rejection_StarResidual;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Stars;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_AirMass;
        private System.Windows.Forms.Label Label_Rejection_Stars;
        private System.Windows.Forms.Label Label_Rejection_AirMass;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Noise;
        private System.Windows.Forms.Label Label_Rejection_Noise;
        private System.Windows.Forms.Button Button_Rejection_RejectionSet;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Median;
        private System.Windows.Forms.Label Label_Rejection_Median;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Eccentricity;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_FWHM;
        private System.Windows.Forms.TextBox TextBox_Rejection_Total;
        private System.Windows.Forms.Label Label_Rejection_Total;
        private System.Windows.Forms.Label Label_Rejection_Eccentricity;
        private System.Windows.Forms.Label Label_Rejection_FWHM;
        private System.Windows.Forms.TabPage TabPage_Calibration;
        private System.Windows.Forms.CheckBox CheckBox_CalibrationTab_CreateNew;
        private System.Windows.Forms.TreeView TreeView_CalibrationTab_TargetFileTree;
        private System.Windows.Forms.TextBox TextBox_CalibrationTab_Messgaes;
        private System.Windows.Forms.GroupBox GroupBox_CalibrationTab_MatchingTolerance;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_TemperatureTolerance;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_OffsetTolerance;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_GainTolerance;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_ExposureTolerance;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_Percentage;
        private System.Windows.Forms.TextBox TextBox_CalibrationTab_MatchingTolerance_Temperature;
        private System.Windows.Forms.TextBox TextBox_CalibrationTab_MatchingTolerance_Offset;
        private System.Windows.Forms.TextBox TextBox_CalibrationTab_MatchingTolerance_Gain;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_Temperature;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_Offset;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_Gain;
        private System.Windows.Forms.Label Label_CalibrationTab_MatchingTolerance_Exposure;
        private System.Windows.Forms.TextBox TextBox_CalibrationTab_MatchingTolerance_Exposure;
        private System.Windows.Forms.Label Label_CalibrationTab_TotalFiles;
        private System.Windows.Forms.ProgressBar ProgressBar_CalibrationTab;
        private System.Windows.Forms.Label Label_CalibrationTab_ReadFileName;
        private System.Windows.Forms.Button Button_CalibrationTab_CreateCalibrationDirectory;
        private System.Windows.Forms.Button Button_CalibrationTab_MatchCalibrationFrames;
        private System.Windows.Forms.Button Button_CalibrationTab_FindCalibrationFrames;
        private System.Windows.Forms.TabPage TabPage_KeywordUpdate;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_FileName;
        private System.Windows.Forms.ProgressBar ProgressBar_KeywordUpdateTab_WriteProgress;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_CaptureSoftware;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_CaptureSoftware_NINA;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_CaptureSoftware_SetByFile;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_CaptureSoftware_SetAll;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_CaptureSoftware_Voyager;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_CaptureSoftware_SharpCap;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_CaptureSoftware_SGPro;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_CaptureSoftware_TheSkyX;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_Telescope;
        private System.Windows.Forms.ComboBox TextBox_KeywordUpdateTab_Telescope_FocalLength;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Telescope_FocalLength;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_Telescope_SetByFile;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_Telescope_SetAll;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_Telescope_Riccardi;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_Telescope_Newtonian254;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_Telescope_EvoStar150;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_Telescope_APM107;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_SubFrameKeywords;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_SubFrameKeywords_SpecificValue;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_SubFrameKeywords_AllValues;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordComment;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordValue;
        private System.Windows.Forms.GroupBox GroupBox_SubFrameKeywords_CalibrationFiles;
        private System.Windows.Forms.Button Button_SubFrameKeywords_CalibrationFiles_ClearAll;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_UpdateNew;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_All;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_SubFrameKeywords_KeywordProtection_Protect;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_SubFrameKeywords_AlphabetizeKeywords;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_SubFrameKeywords_Weights;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Calibration;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_SubFrameKeywords_Weights_Remove;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_Selected;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_SubFrameKeywords_Weights_All;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_SubFrameKeywords_Weights_WeightKeywords;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_SubFrameKeywords_Delete;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_SubFrameKeywords_AddReplace;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_SubFrameKeywords_KeywordName;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdateTargetName;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_SubFrameKeywords_UpdateXisfFileKeywords;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_SubFrameKeywords_TargetNames;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_SubFrameKeywords_TagetName;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_Camera;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_A144Binning;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Q178Binning;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z183Binning;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z533Binning;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_A144SensorTemp;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Q178SensorTemp;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z183SensorTemp;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z533SensorTemp;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Q178Offset;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z183Offset;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z533Offset;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Q178Gain;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z183Gain;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z533Gain;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_A144Seconds;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Q178Seconds;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z533Seconds;
        private System.Windows.Forms.ComboBox ComboBox_KeywordUpdateTab_Camera_Z183Seconds;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_ToggleNBPreset;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_Seconds;
        private System.Windows.Forms.Button Button_KeywordUpdateSubFrameKeywordsCamera_ToggleNB;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_Camera_A144;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_Camera_Q178;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_Camera_Z183;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_Camera_Z533;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_Binning;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_SensorTemp;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_Camera;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_Camera_SetByFile;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_Camera_SetAll;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_A144Gain;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_Offset;
        private System.Windows.Forms.Label Label_KeywordUpdateTab_Camera_Gain;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_ImageType;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_ImageType_SetByFile;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_ImageType_SetAll;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_ImageType_Frame;
        private System.Windows.Forms.Button Button_KeywordUpdateTab_ImageType_Frame_SetMaster;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Frame_Bias;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Frame_Flat;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Frame_Dark;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Frame_Light;
        private System.Windows.Forms.GroupBox GroupBox_KeywordUpdateTab_ImageType_Filter;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_Luma;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_Shutter;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_Red;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_S2;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_Ha;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_Blue;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_Green;
        private System.Windows.Forms.RadioButton RadioButton_KeywordUpdateTab_ImageType_Filter_O3;
        private System.Windows.Forms.TabControl TabControl_Update_TargetScheduler;
        private System.Windows.Forms.CheckBox CheckBox_KeywordUpdateTab_SubFrameKeywords_UpdatePanelName;
        private System.Windows.Forms.TabPage TabPage_TargetScheduler;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button Button_SchedulerTab_OpenDatabase;
        private System.Windows.Forms.Label Label_SchedulerTab_ProjectsText;
        private System.Windows.Forms.Label Label_SchedulerTab_ProfilesText;
        private System.Windows.Forms.TreeView TreeView_SchedulerTab_ProjectTree;
        private System.Windows.Forms.TreeView TreeView_SchedulerTab_ProfileTree;
        private System.Windows.Forms.Label Label_SchedulerTab_TargetsText;
        private System.Windows.Forms.TreeView TreeView_SchedulerTab_TargetTree;
        private System.Windows.Forms.Label Label_SchedulerTab_PlansText;
        private System.Windows.Forms.TreeView TreeView_SchedulerTab_PlansTree;
        private System.Windows.Forms.GroupBox GroupBox_SchedulerTab_Project;
        private System.Windows.Forms.GroupBox GroupBox_Project_Priority;
        private System.Windows.Forms.CheckBox CheckBox_Project_Active;
        private System.Windows.Forms.RadioButton RadioButton_ProjectPriority_High;
        private System.Windows.Forms.RadioButton RadioButton_ProjectPriority_Normal;
        private System.Windows.Forms.RadioButton RadioButton_ProjectPriority_Low;
    }
}