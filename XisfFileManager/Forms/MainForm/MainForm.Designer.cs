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
            this.Button_Browse = new System.Windows.Forms.Button();
            this.ProgressBar_OverAll = new System.Windows.Forms.ProgressBar();
            this.CheckBox_Recurse = new System.Windows.Forms.CheckBox();
            this.GroupBox_RenameOrder = new System.Windows.Forms.GroupBox();
            this.RadioButton_RenameOrder_Weight = new System.Windows.Forms.RadioButton();
            this.RadioButton_RenameOrder_Index = new System.Windows.Forms.RadioButton();
            this.RadioButton_RenameOrder_IndexWeight = new System.Windows.Forms.RadioButton();
            this.RadioButton_RenameOrder_WeightIndex = new System.Windows.Forms.RadioButton();
            this.Button_Rename = new System.Windows.Forms.Button();
            this.GroupBox_XisfFileUpdate = new System.Windows.Forms.GroupBox();
            this.Label_UpdateFileName = new System.Windows.Forms.Label();
            this.GroupBox_Filter = new System.Windows.Forms.GroupBox();
            this.CheckBox_Filter_SetFilter = new System.Windows.Forms.CheckBox();
            this.RadioButton_Filter_S2 = new System.Windows.Forms.RadioButton();
            this.RadioButton_Filter_Blue = new System.Windows.Forms.RadioButton();
            this.RadioButton_Filter_O3 = new System.Windows.Forms.RadioButton();
            this.RadioButton_Filter_Green = new System.Windows.Forms.RadioButton();
            this.RadioButton_Filter_Ha = new System.Windows.Forms.RadioButton();
            this.RadioButton_Filter_Red = new System.Windows.Forms.RadioButton();
            this.RadioButton_Filter_Luma = new System.Windows.Forms.RadioButton();
            this.GroupBox_SubFrameKeywords = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.TextBox_SetKeyword = new System.Windows.Forms.TextBox();
            this.ComboBox_SetKeyword = new System.Windows.Forms.ComboBox();
            this.CheckBox_SubFrameKeywords_UpdateTargetName = new System.Windows.Forms.CheckBox();
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations = new System.Windows.Forms.RadioButton();
            this.RadioButton_SubFrameKeywords_Alphabetize = new System.Windows.Forms.RadioButton();
            this.Button_UpdateXisfFiles = new System.Windows.Forms.Button();
            this.ComboBox_TargetName = new System.Windows.Forms.ComboBox();
            this.Label_TagetName = new System.Windows.Forms.Label();
            this.ProgressBar_XisfFile = new System.Windows.Forms.ProgressBar();
            this.GroupBox_DirectorySelection = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupBox_WeightsAndStatistics = new System.Windows.Forms.GroupBox();
            this.GroupBox_WeightCalculations = new System.Windows.Forms.GroupBox();
            this.GroupBox_StarResidual = new System.Windows.Forms.GroupBox();
            this.Label_StarResidualMinValue = new System.Windows.Forms.Label();
            this.Label_StarResidualMaxValue = new System.Windows.Forms.Label();
            this.Label_StarResidualMedianValue = new System.Windows.Forms.Label();
            this.Label_StarResidualMeanValue = new System.Windows.Forms.Label();
            this.Label_StarResidualSigmaValue = new System.Windows.Forms.Label();
            this.Label_StarResidualMin = new System.Windows.Forms.Label();
            this.Label_StarResidualMax = new System.Windows.Forms.Label();
            this.Label_StarResidualMedian = new System.Windows.Forms.Label();
            this.Label_StarResidualMean = new System.Windows.Forms.Label();
            this.Label_StarResidualSigma = new System.Windows.Forms.Label();
            this.Label_StarResidualRangeLow = new System.Windows.Forms.Label();
            this.Label_StarResidualRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_StarResidualRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_StarResidualRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_FwhmWeight = new System.Windows.Forms.GroupBox();
            this.Label_FwhmMinValue = new System.Windows.Forms.Label();
            this.Label_FwhmMaxValue = new System.Windows.Forms.Label();
            this.Label_FwhmMedianValue = new System.Windows.Forms.Label();
            this.Label_FwhmMeanValue = new System.Windows.Forms.Label();
            this.Label_FwhmSigmaValue = new System.Windows.Forms.Label();
            this.Label_FwhmMin = new System.Windows.Forms.Label();
            this.Label_FwhmMax = new System.Windows.Forms.Label();
            this.Label_FwhmMedian = new System.Windows.Forms.Label();
            this.Label_FwhmRangeLow = new System.Windows.Forms.Label();
            this.Label_FwhmMean = new System.Windows.Forms.Label();
            this.Label_FwhmRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_FwhmRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_FwhmRangeLow = new System.Windows.Forms.TextBox();
            this.Label_FwhmSigma = new System.Windows.Forms.Label();
            this.GroupBox_StarsWeight = new System.Windows.Forms.GroupBox();
            this.Label_StarsMinValue = new System.Windows.Forms.Label();
            this.Label_StarsMaxValue = new System.Windows.Forms.Label();
            this.Label_StarsMedianValue = new System.Windows.Forms.Label();
            this.Label_StarsMeanValue = new System.Windows.Forms.Label();
            this.Label_StarsSigmaValue = new System.Windows.Forms.Label();
            this.Label_StarsMin = new System.Windows.Forms.Label();
            this.Label_StarsMax = new System.Windows.Forms.Label();
            this.Label_StarsMedian = new System.Windows.Forms.Label();
            this.Label_StarsMean = new System.Windows.Forms.Label();
            this.Label_StarsSigma = new System.Windows.Forms.Label();
            this.Label_StarRangeLow = new System.Windows.Forms.Label();
            this.Label_StarRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_StarRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_StarRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_EccentricityWeight = new System.Windows.Forms.GroupBox();
            this.Label_EccentricityMinValue = new System.Windows.Forms.Label();
            this.Label_EccentricityMaxValue = new System.Windows.Forms.Label();
            this.Label_EccentricityMedianValue = new System.Windows.Forms.Label();
            this.Label_EccentricityMeanValue = new System.Windows.Forms.Label();
            this.Label_EccentricitySigmaValue = new System.Windows.Forms.Label();
            this.Label_EccentricityMin = new System.Windows.Forms.Label();
            this.Label_EccentricityMax = new System.Windows.Forms.Label();
            this.Label_EccentricityMedian = new System.Windows.Forms.Label();
            this.Label_EccentricityMean = new System.Windows.Forms.Label();
            this.Label_EccentricitySigma = new System.Windows.Forms.Label();
            this.Label_EccentricityRangeLow = new System.Windows.Forms.Label();
            this.Label_EccentricityRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_EccentricityRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_EccentricityRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_AirMassWeight = new System.Windows.Forms.GroupBox();
            this.Label_AirMassMinValue = new System.Windows.Forms.Label();
            this.Label_AirMassMaxValue = new System.Windows.Forms.Label();
            this.Label_AirMassMedianValue = new System.Windows.Forms.Label();
            this.Label_AirMassMeanValue = new System.Windows.Forms.Label();
            this.Label_AirMassSigmaValue = new System.Windows.Forms.Label();
            this.TextBox_AirMassRangeLow = new System.Windows.Forms.TextBox();
            this.TextBox_AirMassRangeHigh = new System.Windows.Forms.TextBox();
            this.Label_AirMassMin = new System.Windows.Forms.Label();
            this.Label_AirMassMax = new System.Windows.Forms.Label();
            this.Label_AirMassMedian = new System.Windows.Forms.Label();
            this.Label_AirMassMean = new System.Windows.Forms.Label();
            this.Label_AirMassSigma = new System.Windows.Forms.Label();
            this.Label_FwhmMeanDeviationRangeLow = new System.Windows.Forms.Label();
            this.Label_FwhmMeanDeviationRangeHigh = new System.Windows.Forms.Label();
            this.GroupBox_NoiseWeight = new System.Windows.Forms.GroupBox();
            this.Label_NoiseMinValue = new System.Windows.Forms.Label();
            this.Label_NoiseMaxValue = new System.Windows.Forms.Label();
            this.Label_NoiseMedianValue = new System.Windows.Forms.Label();
            this.Label_NoiseMeanValue = new System.Windows.Forms.Label();
            this.Label_NoiseSigmaValue = new System.Windows.Forms.Label();
            this.Label_NoiseMin = new System.Windows.Forms.Label();
            this.Label_NoiseMax = new System.Windows.Forms.Label();
            this.Label_NoiseMedian = new System.Windows.Forms.Label();
            this.Label_NoiseMean = new System.Windows.Forms.Label();
            this.Label_NoiseSigma = new System.Windows.Forms.Label();
            this.Label_NoiseRangeLow = new System.Windows.Forms.Label();
            this.Label_NoiseRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_NoiseRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_NoiseRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_MedianWeight = new System.Windows.Forms.GroupBox();
            this.Label_MedianMinValue = new System.Windows.Forms.Label();
            this.Label_MedianMaxValue = new System.Windows.Forms.Label();
            this.Label_MedianMedianValue = new System.Windows.Forms.Label();
            this.Label_MedianMeanValue = new System.Windows.Forms.Label();
            this.Label_MedianSigmaValue = new System.Windows.Forms.Label();
            this.Label_MedianMin = new System.Windows.Forms.Label();
            this.Label_MedianMax = new System.Windows.Forms.Label();
            this.Label_MedianMedian = new System.Windows.Forms.Label();
            this.Label_MedianMean = new System.Windows.Forms.Label();
            this.Label_MedianSigma = new System.Windows.Forms.Label();
            this.Label_MedianRangeLow = new System.Windows.Forms.Label();
            this.Label_MedianRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_MedianRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_MedianRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_SnrWeight = new System.Windows.Forms.GroupBox();
            this.Label_SnrMinValue = new System.Windows.Forms.Label();
            this.Label_SnrMaxValue = new System.Windows.Forms.Label();
            this.Label_SnrMedianValue = new System.Windows.Forms.Label();
            this.Label_SnrMeanValue = new System.Windows.Forms.Label();
            this.Label_SnrSigmaValue = new System.Windows.Forms.Label();
            this.Label_SnrMin = new System.Windows.Forms.Label();
            this.Label_SnrMax = new System.Windows.Forms.Label();
            this.Label_SnrMedian = new System.Windows.Forms.Label();
            this.Label_SnrMean = new System.Windows.Forms.Label();
            this.Label_SnrSigma = new System.Windows.Forms.Label();
            this.Label_SnrRangeLow = new System.Windows.Forms.Label();
            this.Label_SnrRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_SnrRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_SnrRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_InitialRejectionCriteria = new System.Windows.Forms.GroupBox();
            this.NumericUpDown_Rejection_Snr = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.NumericUpDown_Rejection_StarResidual = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.NumericUpDown_Rejection_Stars = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDown_Rejection_AirMass = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.NumericUpDown_Rejection_Noise = new System.Windows.Forms.NumericUpDown();
            this.Label_Rejection_Noise = new System.Windows.Forms.Label();
            this.Button_Rejection_RejectionSet = new System.Windows.Forms.Button();
            this.NumericUpDown_Rejection_Median = new System.Windows.Forms.NumericUpDown();
            this.Label_Rejection_Median = new System.Windows.Forms.Label();
            this.NumericUpDown_Rejection_Eccentricity = new System.Windows.Forms.NumericUpDown();
            this.NumericUpDown_Rejection_FWHM = new System.Windows.Forms.NumericUpDown();
            this.TextBox_Rejection_Total = new System.Windows.Forms.TextBox();
            this.Label_Rejection_Total = new System.Windows.Forms.Label();
            this.Label_Rejection_Eccentricity = new System.Windows.Forms.Label();
            this.Label_Rejection_FWHM = new System.Windows.Forms.Label();
            this.GroupBox_UpdateStatistics = new System.Windows.Forms.GroupBox();
            this.RadioButton_SetImageStatistics_CalculateWeights = new System.Windows.Forms.RadioButton();
            this.RadioButton_SetImageStatistics_RescaleWeights = new System.Windows.Forms.RadioButton();
            this.RadioButton_SetImageStatistics_KeepWeights = new System.Windows.Forms.RadioButton();
            this.Button_ReadSubFrameSelectorCsvFile = new System.Windows.Forms.Button();
            this.Label_UpdateStatistics = new System.Windows.Forms.Label();
            this.Label_UpdateStatisticsRangeHigh = new System.Windows.Forms.Label();
            this.TextBox_UpdateStatisticsRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_UpdateStatisticsRangeLow = new System.Windows.Forms.TextBox();
            this.Label_UpdateStatisticsRangeLow = new System.Windows.Forms.Label();
            this.Label_Task = new System.Windows.Forms.Label();
            this.Label_TempratureCompensation = new System.Windows.Forms.Label();
            this.Label_File_Selection_SubFrameOverhead = new System.Windows.Forms.Label();
            this.GroupBox_FileSlection = new System.Windows.Forms.GroupBox();
            this.Label_BrowseFileName = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.RadioButton_Filter_Shutter = new System.Windows.Forms.RadioButton();
            this.GroupBox_RenameOrder.SuspendLayout();
            this.GroupBox_XisfFileUpdate.SuspendLayout();
            this.GroupBox_Filter.SuspendLayout();
            this.GroupBox_SubFrameKeywords.SuspendLayout();
            this.GroupBox_DirectorySelection.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.GroupBox_WeightsAndStatistics.SuspendLayout();
            this.GroupBox_WeightCalculations.SuspendLayout();
            this.GroupBox_StarResidual.SuspendLayout();
            this.GroupBox_FwhmWeight.SuspendLayout();
            this.GroupBox_StarsWeight.SuspendLayout();
            this.GroupBox_EccentricityWeight.SuspendLayout();
            this.GroupBox_AirMassWeight.SuspendLayout();
            this.GroupBox_NoiseWeight.SuspendLayout();
            this.GroupBox_MedianWeight.SuspendLayout();
            this.GroupBox_SnrWeight.SuspendLayout();
            this.GroupBox_InitialRejectionCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Snr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_StarResidual)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Stars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_AirMass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Noise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Median)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Eccentricity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_FWHM)).BeginInit();
            this.GroupBox_UpdateStatistics.SuspendLayout();
            this.GroupBox_FileSlection.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Browse
            // 
            this.Button_Browse.Location = new System.Drawing.Point(12, 46);
            this.Button_Browse.Name = "Button_Browse";
            this.Button_Browse.Size = new System.Drawing.Size(75, 23);
            this.Button_Browse.TabIndex = 0;
            this.Button_Browse.Text = "Browse";
            this.Button_Browse.UseVisualStyleBackColor = true;
            this.Button_Browse.Click += new System.EventHandler(this.Button_Browse_Click);
            // 
            // ProgressBar_OverAll
            // 
            this.ProgressBar_OverAll.Location = new System.Drawing.Point(28, 166);
            this.ProgressBar_OverAll.Name = "ProgressBar_OverAll";
            this.ProgressBar_OverAll.Size = new System.Drawing.Size(889, 11);
            this.ProgressBar_OverAll.Step = 1;
            this.ProgressBar_OverAll.TabIndex = 1;
            // 
            // CheckBox_Recurse
            // 
            this.CheckBox_Recurse.AutoSize = true;
            this.CheckBox_Recurse.Location = new System.Drawing.Point(100, 49);
            this.CheckBox_Recurse.Name = "CheckBox_Recurse";
            this.CheckBox_Recurse.Size = new System.Drawing.Size(119, 17);
            this.CheckBox_Recurse.TabIndex = 2;
            this.CheckBox_Recurse.Text = "Recurse Directories";
            this.CheckBox_Recurse.UseVisualStyleBackColor = true;
            // 
            // GroupBox_RenameOrder
            // 
            this.GroupBox_RenameOrder.Controls.Add(this.RadioButton_RenameOrder_Weight);
            this.GroupBox_RenameOrder.Controls.Add(this.RadioButton_RenameOrder_Index);
            this.GroupBox_RenameOrder.Controls.Add(this.RadioButton_RenameOrder_IndexWeight);
            this.GroupBox_RenameOrder.Controls.Add(this.RadioButton_RenameOrder_WeightIndex);
            this.GroupBox_RenameOrder.Controls.Add(this.Button_Rename);
            this.GroupBox_RenameOrder.Location = new System.Drawing.Point(696, 19);
            this.GroupBox_RenameOrder.Name = "GroupBox_RenameOrder";
            this.GroupBox_RenameOrder.Size = new System.Drawing.Size(231, 105);
            this.GroupBox_RenameOrder.TabIndex = 3;
            this.GroupBox_RenameOrder.TabStop = false;
            this.GroupBox_RenameOrder.Text = "Rename Order";
            // 
            // RadioButton_RenameOrder_Weight
            // 
            this.RadioButton_RenameOrder_Weight.AutoSize = true;
            this.RadioButton_RenameOrder_Weight.Location = new System.Drawing.Point(28, 48);
            this.RadioButton_RenameOrder_Weight.Name = "RadioButton_RenameOrder_Weight";
            this.RadioButton_RenameOrder_Weight.Size = new System.Drawing.Size(83, 17);
            this.RadioButton_RenameOrder_Weight.TabIndex = 3;
            this.RadioButton_RenameOrder_Weight.Text = "Weight Only";
            this.RadioButton_RenameOrder_Weight.UseVisualStyleBackColor = true;
            this.RadioButton_RenameOrder_Weight.CheckedChanged += new System.EventHandler(this.RadioButton_Weight_CheckedChanged);
            // 
            // RadioButton_RenameOrder_Index
            // 
            this.RadioButton_RenameOrder_Index.AutoSize = true;
            this.RadioButton_RenameOrder_Index.Location = new System.Drawing.Point(28, 22);
            this.RadioButton_RenameOrder_Index.Name = "RadioButton_RenameOrder_Index";
            this.RadioButton_RenameOrder_Index.Size = new System.Drawing.Size(75, 17);
            this.RadioButton_RenameOrder_Index.TabIndex = 2;
            this.RadioButton_RenameOrder_Index.Text = "Index Only";
            this.RadioButton_RenameOrder_Index.UseVisualStyleBackColor = true;
            this.RadioButton_RenameOrder_Index.CheckedChanged += new System.EventHandler(this.RadioButton_Index_CheckedChanged);
            // 
            // RadioButton_RenameOrder_IndexWeight
            // 
            this.RadioButton_RenameOrder_IndexWeight.AutoSize = true;
            this.RadioButton_RenameOrder_IndexWeight.Checked = true;
            this.RadioButton_RenameOrder_IndexWeight.Location = new System.Drawing.Point(117, 22);
            this.RadioButton_RenameOrder_IndexWeight.Name = "RadioButton_RenameOrder_IndexWeight";
            this.RadioButton_RenameOrder_IndexWeight.Size = new System.Drawing.Size(88, 17);
            this.RadioButton_RenameOrder_IndexWeight.TabIndex = 1;
            this.RadioButton_RenameOrder_IndexWeight.TabStop = true;
            this.RadioButton_RenameOrder_IndexWeight.Text = "Index Weight";
            this.RadioButton_RenameOrder_IndexWeight.UseVisualStyleBackColor = true;
            this.RadioButton_RenameOrder_IndexWeight.CheckedChanged += new System.EventHandler(this.RadioButton_IndexWeight_CheckedChanged);
            // 
            // RadioButton_RenameOrder_WeightIndex
            // 
            this.RadioButton_RenameOrder_WeightIndex.AutoSize = true;
            this.RadioButton_RenameOrder_WeightIndex.Location = new System.Drawing.Point(117, 48);
            this.RadioButton_RenameOrder_WeightIndex.Name = "RadioButton_RenameOrder_WeightIndex";
            this.RadioButton_RenameOrder_WeightIndex.Size = new System.Drawing.Size(88, 17);
            this.RadioButton_RenameOrder_WeightIndex.TabIndex = 0;
            this.RadioButton_RenameOrder_WeightIndex.Text = "Weight Index";
            this.RadioButton_RenameOrder_WeightIndex.UseVisualStyleBackColor = true;
            this.RadioButton_RenameOrder_WeightIndex.CheckedChanged += new System.EventHandler(this.RadioButton_WeightIndex_CheckedChanged);
            // 
            // Button_Rename
            // 
            this.Button_Rename.Location = new System.Drawing.Point(47, 72);
            this.Button_Rename.Name = "Button_Rename";
            this.Button_Rename.Size = new System.Drawing.Size(124, 23);
            this.Button_Rename.TabIndex = 4;
            this.Button_Rename.Text = "Rename XISF Files";
            this.Button_Rename.UseVisualStyleBackColor = true;
            this.Button_Rename.Click += new System.EventHandler(this.Button_Rename_Click);
            // 
            // GroupBox_XisfFileUpdate
            // 
            this.GroupBox_XisfFileUpdate.Controls.Add(this.Label_UpdateFileName);
            this.GroupBox_XisfFileUpdate.Controls.Add(this.GroupBox_Filter);
            this.GroupBox_XisfFileUpdate.Controls.Add(this.GroupBox_SubFrameKeywords);
            this.GroupBox_XisfFileUpdate.Controls.Add(this.ProgressBar_XisfFile);
            this.GroupBox_XisfFileUpdate.Enabled = false;
            this.GroupBox_XisfFileUpdate.Location = new System.Drawing.Point(12, 228);
            this.GroupBox_XisfFileUpdate.Name = "GroupBox_XisfFileUpdate";
            this.GroupBox_XisfFileUpdate.Size = new System.Drawing.Size(950, 193);
            this.GroupBox_XisfFileUpdate.TabIndex = 6;
            this.GroupBox_XisfFileUpdate.TabStop = false;
            this.GroupBox_XisfFileUpdate.Text = "File Update";
            // 
            // Label_UpdateFileName
            // 
            this.Label_UpdateFileName.AutoSize = true;
            this.Label_UpdateFileName.Location = new System.Drawing.Point(17, 133);
            this.Label_UpdateFileName.Name = "Label_UpdateFileName";
            this.Label_UpdateFileName.Size = new System.Drawing.Size(92, 13);
            this.Label_UpdateFileName.TabIndex = 19;
            this.Label_UpdateFileName.Text = "Update File Name";
            // 
            // GroupBox_Filter
            // 
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_Shutter);
            this.GroupBox_Filter.Controls.Add(this.CheckBox_Filter_SetFilter);
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_S2);
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_Blue);
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_O3);
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_Green);
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_Ha);
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_Red);
            this.GroupBox_Filter.Controls.Add(this.RadioButton_Filter_Luma);
            this.GroupBox_Filter.Location = new System.Drawing.Point(675, 19);
            this.GroupBox_Filter.Name = "GroupBox_Filter";
            this.GroupBox_Filter.Size = new System.Drawing.Size(263, 105);
            this.GroupBox_Filter.TabIndex = 18;
            this.GroupBox_Filter.TabStop = false;
            this.GroupBox_Filter.Text = "Filters";
            // 
            // CheckBox_Filter_SetFilter
            // 
            this.CheckBox_Filter_SetFilter.AutoSize = true;
            this.CheckBox_Filter_SetFilter.Location = new System.Drawing.Point(103, 75);
            this.CheckBox_Filter_SetFilter.Name = "CheckBox_Filter_SetFilter";
            this.CheckBox_Filter_SetFilter.Size = new System.Drawing.Size(67, 17);
            this.CheckBox_Filter_SetFilter.TabIndex = 7;
            this.CheckBox_Filter_SetFilter.Text = "Set Filter";
            this.CheckBox_Filter_SetFilter.UseVisualStyleBackColor = true;
            this.CheckBox_Filter_SetFilter.CheckedChanged += new System.EventHandler(this.CheckBox_Filter_SetFilter_CheckedChanged);
            // 
            // RadioButton_Filter_S2
            // 
            this.RadioButton_Filter_S2.AutoSize = true;
            this.RadioButton_Filter_S2.Location = new System.Drawing.Point(193, 22);
            this.RadioButton_Filter_S2.Name = "RadioButton_Filter_S2";
            this.RadioButton_Filter_S2.Size = new System.Drawing.Size(41, 17);
            this.RadioButton_Filter_S2.TabIndex = 6;
            this.RadioButton_Filter_S2.TabStop = true;
            this.RadioButton_Filter_S2.Text = "S II";
            this.RadioButton_Filter_S2.UseVisualStyleBackColor = true;
            // 
            // RadioButton_Filter_Blue
            // 
            this.RadioButton_Filter_Blue.AutoSize = true;
            this.RadioButton_Filter_Blue.Location = new System.Drawing.Point(139, 48);
            this.RadioButton_Filter_Blue.Name = "RadioButton_Filter_Blue";
            this.RadioButton_Filter_Blue.Size = new System.Drawing.Size(46, 17);
            this.RadioButton_Filter_Blue.TabIndex = 5;
            this.RadioButton_Filter_Blue.TabStop = true;
            this.RadioButton_Filter_Blue.Text = "Blue";
            this.RadioButton_Filter_Blue.UseVisualStyleBackColor = true;
            // 
            // RadioButton_Filter_O3
            // 
            this.RadioButton_Filter_O3.AutoSize = true;
            this.RadioButton_Filter_O3.Location = new System.Drawing.Point(136, 22);
            this.RadioButton_Filter_O3.Name = "RadioButton_Filter_O3";
            this.RadioButton_Filter_O3.Size = new System.Drawing.Size(45, 17);
            this.RadioButton_Filter_O3.TabIndex = 4;
            this.RadioButton_Filter_O3.TabStop = true;
            this.RadioButton_Filter_O3.Text = "O III";
            this.RadioButton_Filter_O3.UseVisualStyleBackColor = true;
            // 
            // RadioButton_Filter_Green
            // 
            this.RadioButton_Filter_Green.AutoSize = true;
            this.RadioButton_Filter_Green.Location = new System.Drawing.Point(76, 48);
            this.RadioButton_Filter_Green.Name = "RadioButton_Filter_Green";
            this.RadioButton_Filter_Green.Size = new System.Drawing.Size(54, 17);
            this.RadioButton_Filter_Green.TabIndex = 3;
            this.RadioButton_Filter_Green.TabStop = true;
            this.RadioButton_Filter_Green.Text = "Green";
            this.RadioButton_Filter_Green.UseVisualStyleBackColor = true;
            // 
            // RadioButton_Filter_Ha
            // 
            this.RadioButton_Filter_Ha.AutoSize = true;
            this.RadioButton_Filter_Ha.Location = new System.Drawing.Point(85, 22);
            this.RadioButton_Filter_Ha.Name = "RadioButton_Filter_Ha";
            this.RadioButton_Filter_Ha.Size = new System.Drawing.Size(39, 17);
            this.RadioButton_Filter_Ha.TabIndex = 2;
            this.RadioButton_Filter_Ha.TabStop = true;
            this.RadioButton_Filter_Ha.Text = "Ha";
            this.RadioButton_Filter_Ha.UseVisualStyleBackColor = true;
            // 
            // RadioButton_Filter_Red
            // 
            this.RadioButton_Filter_Red.AutoSize = true;
            this.RadioButton_Filter_Red.Location = new System.Drawing.Point(22, 48);
            this.RadioButton_Filter_Red.Name = "RadioButton_Filter_Red";
            this.RadioButton_Filter_Red.Size = new System.Drawing.Size(45, 17);
            this.RadioButton_Filter_Red.TabIndex = 1;
            this.RadioButton_Filter_Red.TabStop = true;
            this.RadioButton_Filter_Red.Text = "Red";
            this.RadioButton_Filter_Red.UseVisualStyleBackColor = true;
            // 
            // RadioButton_Filter_Luma
            // 
            this.RadioButton_Filter_Luma.AutoSize = true;
            this.RadioButton_Filter_Luma.Location = new System.Drawing.Point(22, 22);
            this.RadioButton_Filter_Luma.Name = "RadioButton_Filter_Luma";
            this.RadioButton_Filter_Luma.Size = new System.Drawing.Size(51, 17);
            this.RadioButton_Filter_Luma.TabIndex = 0;
            this.RadioButton_Filter_Luma.TabStop = true;
            this.RadioButton_Filter_Luma.Text = "Luma";
            this.RadioButton_Filter_Luma.UseVisualStyleBackColor = true;
            // 
            // GroupBox_SubFrameKeywords
            // 
            this.GroupBox_SubFrameKeywords.Controls.Add(this.button2);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.button1);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.TextBox_SetKeyword);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.ComboBox_SetKeyword);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.CheckBox_SubFrameKeywords_UpdateTargetName);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.RadioButton_SubFrameKeywords_Alphabetize);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.Button_UpdateXisfFiles);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.ComboBox_TargetName);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.Label_TagetName);
            this.GroupBox_SubFrameKeywords.Location = new System.Drawing.Point(11, 19);
            this.GroupBox_SubFrameKeywords.Name = "GroupBox_SubFrameKeywords";
            this.GroupBox_SubFrameKeywords.Size = new System.Drawing.Size(658, 105);
            this.GroupBox_SubFrameKeywords.TabIndex = 14;
            this.GroupBox_SubFrameKeywords.TabStop = false;
            this.GroupBox_SubFrameKeywords.Text = "SubFrame Keywords";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(542, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(93, 23);
            this.button2.TabIndex = 21;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(418, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Add/Replace";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // TextBox_SetKeyword
            // 
            this.TextBox_SetKeyword.Location = new System.Drawing.Point(418, 46);
            this.TextBox_SetKeyword.Name = "TextBox_SetKeyword";
            this.TextBox_SetKeyword.Size = new System.Drawing.Size(217, 20);
            this.TextBox_SetKeyword.TabIndex = 19;
            // 
            // ComboBox_SetKeyword
            // 
            this.ComboBox_SetKeyword.AllowDrop = true;
            this.ComboBox_SetKeyword.FormattingEnabled = true;
            this.ComboBox_SetKeyword.Location = new System.Drawing.Point(418, 20);
            this.ComboBox_SetKeyword.Name = "ComboBox_SetKeyword";
            this.ComboBox_SetKeyword.Size = new System.Drawing.Size(217, 21);
            this.ComboBox_SetKeyword.Sorted = true;
            this.ComboBox_SetKeyword.TabIndex = 18;
            // 
            // CheckBox_SubFrameKeywords_UpdateTargetName
            // 
            this.CheckBox_SubFrameKeywords_UpdateTargetName.AutoSize = true;
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Location = new System.Drawing.Point(211, 74);
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Name = "CheckBox_SubFrameKeywords_UpdateTargetName";
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Size = new System.Drawing.Size(126, 17);
            this.CheckBox_SubFrameKeywords_UpdateTargetName.TabIndex = 17;
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Text = "Update Target Name";
            this.CheckBox_SubFrameKeywords_UpdateTargetName.UseVisualStyleBackColor = true;
            // 
            // RadioButton_SubFrameKeyWords_SubFrameWeightCalculations
            // 
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.AutoSize = true;
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.Location = new System.Drawing.Point(211, 48);
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.Name = "RadioButton_SubFrameKeyWords_SubFrameWeightCalculations";
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.Size = new System.Drawing.Size(170, 17);
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.TabIndex = 15;
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.Text = "SubFrame Weight Calculations";
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.UseVisualStyleBackColor = true;
            this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations.CheckedChanged += new System.EventHandler(this.RadioButton_SubFrameKeyWords_SubFrameWeightCalculations_CheckedChanged);
            // 
            // RadioButton_SubFrameKeywords_Alphabetize
            // 
            this.RadioButton_SubFrameKeywords_Alphabetize.AutoSize = true;
            this.RadioButton_SubFrameKeywords_Alphabetize.Checked = true;
            this.RadioButton_SubFrameKeywords_Alphabetize.Location = new System.Drawing.Point(211, 22);
            this.RadioButton_SubFrameKeywords_Alphabetize.Name = "RadioButton_SubFrameKeywords_Alphabetize";
            this.RadioButton_SubFrameKeywords_Alphabetize.Size = new System.Drawing.Size(168, 17);
            this.RadioButton_SubFrameKeywords_Alphabetize.TabIndex = 14;
            this.RadioButton_SubFrameKeywords_Alphabetize.TabStop = true;
            this.RadioButton_SubFrameKeywords_Alphabetize.Text = "Alphabetize Existing Keywords";
            this.RadioButton_SubFrameKeywords_Alphabetize.UseVisualStyleBackColor = true;
            this.RadioButton_SubFrameKeywords_Alphabetize.CheckedChanged += new System.EventHandler(this.RadioButton_SubFrameKeywords_Alphabetize_CheckedChanged);
            // 
            // Button_UpdateXisfFiles
            // 
            this.Button_UpdateXisfFiles.Location = new System.Drawing.Point(21, 72);
            this.Button_UpdateXisfFiles.Name = "Button_UpdateXisfFiles";
            this.Button_UpdateXisfFiles.Size = new System.Drawing.Size(167, 23);
            this.Button_UpdateXisfFiles.TabIndex = 4;
            this.Button_UpdateXisfFiles.Text = "Update XISF File Keywords";
            this.Button_UpdateXisfFiles.UseVisualStyleBackColor = true;
            this.Button_UpdateXisfFiles.Click += new System.EventHandler(this.Button_Update_Click);
            // 
            // ComboBox_TargetName
            // 
            this.ComboBox_TargetName.AllowDrop = true;
            this.ComboBox_TargetName.FormattingEnabled = true;
            this.ComboBox_TargetName.Location = new System.Drawing.Point(21, 46);
            this.ComboBox_TargetName.Name = "ComboBox_TargetName";
            this.ComboBox_TargetName.Size = new System.Drawing.Size(167, 21);
            this.ComboBox_TargetName.Sorted = true;
            this.ComboBox_TargetName.TabIndex = 5;
            // 
            // Label_TagetName
            // 
            this.Label_TagetName.AllowDrop = true;
            this.Label_TagetName.AutoSize = true;
            this.Label_TagetName.Location = new System.Drawing.Point(70, 24);
            this.Label_TagetName.Name = "Label_TagetName";
            this.Label_TagetName.Size = new System.Drawing.Size(69, 13);
            this.Label_TagetName.TabIndex = 0;
            this.Label_TagetName.Text = "Target Name";
            this.Label_TagetName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgressBar_XisfFile
            // 
            this.ProgressBar_XisfFile.Location = new System.Drawing.Point(28, 165);
            this.ProgressBar_XisfFile.Name = "ProgressBar_XisfFile";
            this.ProgressBar_XisfFile.Size = new System.Drawing.Size(889, 11);
            this.ProgressBar_XisfFile.Step = 1;
            this.ProgressBar_XisfFile.TabIndex = 13;
            // 
            // GroupBox_DirectorySelection
            // 
            this.GroupBox_DirectorySelection.Controls.Add(this.Button_Browse);
            this.GroupBox_DirectorySelection.Controls.Add(this.CheckBox_Recurse);
            this.GroupBox_DirectorySelection.Location = new System.Drawing.Point(17, 20);
            this.GroupBox_DirectorySelection.Name = "GroupBox_DirectorySelection";
            this.GroupBox_DirectorySelection.Size = new System.Drawing.Size(231, 105);
            this.GroupBox_DirectorySelection.TabIndex = 7;
            this.GroupBox_DirectorySelection.TabStop = false;
            this.GroupBox_DirectorySelection.Text = "Directory Selection";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(973, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectTemplateToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // selectTemplateToolStripMenuItem
            // 
            this.selectTemplateToolStripMenuItem.Name = "selectTemplateToolStripMenuItem";
            this.selectTemplateToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.selectTemplateToolStripMenuItem.Text = "Select Template";
            this.selectTemplateToolStripMenuItem.Click += new System.EventHandler(this.SelectTemplateToolStripMenuItem_Click);
            // 
            // GroupBox_WeightsAndStatistics
            // 
            this.GroupBox_WeightsAndStatistics.Controls.Add(this.GroupBox_WeightCalculations);
            this.GroupBox_WeightsAndStatistics.Controls.Add(this.GroupBox_InitialRejectionCriteria);
            this.GroupBox_WeightsAndStatistics.Controls.Add(this.GroupBox_UpdateStatistics);
            this.GroupBox_WeightsAndStatistics.Enabled = false;
            this.GroupBox_WeightsAndStatistics.Location = new System.Drawing.Point(12, 427);
            this.GroupBox_WeightsAndStatistics.Name = "GroupBox_WeightsAndStatistics";
            this.GroupBox_WeightsAndStatistics.Size = new System.Drawing.Size(950, 477);
            this.GroupBox_WeightsAndStatistics.TabIndex = 11;
            this.GroupBox_WeightsAndStatistics.TabStop = false;
            this.GroupBox_WeightsAndStatistics.Text = "SubFrame Weights and Statistics";
            // 
            // GroupBox_WeightCalculations
            // 
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_StarResidual);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_FwhmWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_StarsWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_EccentricityWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_AirMassWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_NoiseWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_MedianWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_SnrWeight);
            this.GroupBox_WeightCalculations.Location = new System.Drawing.Point(17, 213);
            this.GroupBox_WeightCalculations.Name = "GroupBox_WeightCalculations";
            this.GroupBox_WeightCalculations.Size = new System.Drawing.Size(911, 251);
            this.GroupBox_WeightCalculations.TabIndex = 27;
            this.GroupBox_WeightCalculations.TabStop = false;
            this.GroupBox_WeightCalculations.Text = "Weight Caluculations";
            // 
            // GroupBox_StarResidual
            // 
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMinValue);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMaxValue);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMedianValue);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMeanValue);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualSigmaValue);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMin);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMax);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMedian);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMean);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualSigma);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualRangeLow);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualRangeHigh);
            this.GroupBox_StarResidual.Controls.Add(this.TextBox_StarResidualRangeHigh);
            this.GroupBox_StarResidual.Controls.Add(this.TextBox_StarResidualRangeLow);
            this.GroupBox_StarResidual.Location = new System.Drawing.Point(465, 137);
            this.GroupBox_StarResidual.Name = "GroupBox_StarResidual";
            this.GroupBox_StarResidual.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_StarResidual.TabIndex = 24;
            this.GroupBox_StarResidual.TabStop = false;
            this.GroupBox_StarResidual.Text = "Star Residual Weight";
            // 
            // Label_StarResidualMinValue
            // 
            this.Label_StarResidualMinValue.AutoSize = true;
            this.Label_StarResidualMinValue.Location = new System.Drawing.Point(55, 50);
            this.Label_StarResidualMinValue.Name = "Label_StarResidualMinValue";
            this.Label_StarResidualMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarResidualMinValue.TabIndex = 38;
            this.Label_StarResidualMinValue.Text = "0";
            // 
            // Label_StarResidualMaxValue
            // 
            this.Label_StarResidualMaxValue.AutoSize = true;
            this.Label_StarResidualMaxValue.Location = new System.Drawing.Point(55, 67);
            this.Label_StarResidualMaxValue.Name = "Label_StarResidualMaxValue";
            this.Label_StarResidualMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarResidualMaxValue.TabIndex = 37;
            this.Label_StarResidualMaxValue.Text = "0";
            // 
            // Label_StarResidualMedianValue
            // 
            this.Label_StarResidualMedianValue.AutoSize = true;
            this.Label_StarResidualMedianValue.Location = new System.Drawing.Point(55, 33);
            this.Label_StarResidualMedianValue.Name = "Label_StarResidualMedianValue";
            this.Label_StarResidualMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarResidualMedianValue.TabIndex = 36;
            this.Label_StarResidualMedianValue.Text = "0";
            // 
            // Label_StarResidualMeanValue
            // 
            this.Label_StarResidualMeanValue.AutoSize = true;
            this.Label_StarResidualMeanValue.Location = new System.Drawing.Point(55, 16);
            this.Label_StarResidualMeanValue.Name = "Label_StarResidualMeanValue";
            this.Label_StarResidualMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarResidualMeanValue.TabIndex = 34;
            this.Label_StarResidualMeanValue.Text = "0";
            // 
            // Label_StarResidualSigmaValue
            // 
            this.Label_StarResidualSigmaValue.AutoSize = true;
            this.Label_StarResidualSigmaValue.Location = new System.Drawing.Point(55, 84);
            this.Label_StarResidualSigmaValue.Name = "Label_StarResidualSigmaValue";
            this.Label_StarResidualSigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarResidualSigmaValue.TabIndex = 35;
            this.Label_StarResidualSigmaValue.Text = "0";
            // 
            // Label_StarResidualMin
            // 
            this.Label_StarResidualMin.AutoSize = true;
            this.Label_StarResidualMin.Location = new System.Drawing.Point(12, 49);
            this.Label_StarResidualMin.Name = "Label_StarResidualMin";
            this.Label_StarResidualMin.Size = new System.Drawing.Size(30, 13);
            this.Label_StarResidualMin.TabIndex = 23;
            this.Label_StarResidualMin.Text = "Min: ";
            // 
            // Label_StarResidualMax
            // 
            this.Label_StarResidualMax.AutoSize = true;
            this.Label_StarResidualMax.Location = new System.Drawing.Point(12, 66);
            this.Label_StarResidualMax.Name = "Label_StarResidualMax";
            this.Label_StarResidualMax.Size = new System.Drawing.Size(33, 13);
            this.Label_StarResidualMax.TabIndex = 22;
            this.Label_StarResidualMax.Text = "Max: ";
            // 
            // Label_StarResidualMedian
            // 
            this.Label_StarResidualMedian.AutoSize = true;
            this.Label_StarResidualMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_StarResidualMedian.Name = "Label_StarResidualMedian";
            this.Label_StarResidualMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_StarResidualMedian.TabIndex = 21;
            this.Label_StarResidualMedian.Text = "Median:";
            // 
            // Label_StarResidualMean
            // 
            this.Label_StarResidualMean.AutoSize = true;
            this.Label_StarResidualMean.Location = new System.Drawing.Point(12, 15);
            this.Label_StarResidualMean.Name = "Label_StarResidualMean";
            this.Label_StarResidualMean.Size = new System.Drawing.Size(40, 13);
            this.Label_StarResidualMean.TabIndex = 19;
            this.Label_StarResidualMean.Text = "Mean: ";
            // 
            // Label_StarResidualSigma
            // 
            this.Label_StarResidualSigma.AutoSize = true;
            this.Label_StarResidualSigma.Location = new System.Drawing.Point(12, 83);
            this.Label_StarResidualSigma.Name = "Label_StarResidualSigma";
            this.Label_StarResidualSigma.Size = new System.Drawing.Size(42, 13);
            this.Label_StarResidualSigma.TabIndex = 20;
            this.Label_StarResidualSigma.Text = "Sigma: ";
            // 
            // Label_StarResidualRangeLow
            // 
            this.Label_StarResidualRangeLow.AutoSize = true;
            this.Label_StarResidualRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_StarResidualRangeLow.Name = "Label_StarResidualRangeLow";
            this.Label_StarResidualRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_StarResidualRangeLow.TabIndex = 13;
            this.Label_StarResidualRangeLow.Text = "Low";
            this.Label_StarResidualRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarResidualRangeHigh
            // 
            this.Label_StarResidualRangeHigh.AutoSize = true;
            this.Label_StarResidualRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_StarResidualRangeHigh.Name = "Label_StarResidualRangeHigh";
            this.Label_StarResidualRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_StarResidualRangeHigh.TabIndex = 12;
            this.Label_StarResidualRangeHigh.Text = "High";
            this.Label_StarResidualRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_StarResidualRangeHigh
            // 
            this.TextBox_StarResidualRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_StarResidualRangeHigh.Name = "TextBox_StarResidualRangeHigh";
            this.TextBox_StarResidualRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarResidualRangeHigh.TabIndex = 8;
            this.TextBox_StarResidualRangeHigh.Text = "1.0";
            this.TextBox_StarResidualRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualRangeHigh.TextChanged += new System.EventHandler(this.TextBox_StarResidualRangeHigh_TextChanged);
            // 
            // TextBox_StarResidualRangeLow
            // 
            this.TextBox_StarResidualRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_StarResidualRangeLow.Name = "TextBox_StarResidualRangeLow";
            this.TextBox_StarResidualRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarResidualRangeLow.TabIndex = 9;
            this.TextBox_StarResidualRangeLow.Text = "0.0";
            this.TextBox_StarResidualRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualRangeLow.TextChanged += new System.EventHandler(this.TextBox_StarResidualRangeLow_TextChanged);
            // 
            // GroupBox_FwhmWeight
            // 
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMinValue);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMaxValue);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMedianValue);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMeanValue);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmSigmaValue);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMin);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMax);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMedian);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmRangeLow);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMean);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmRangeHigh);
            this.GroupBox_FwhmWeight.Controls.Add(this.TextBox_FwhmRangeHigh);
            this.GroupBox_FwhmWeight.Controls.Add(this.TextBox_FwhmRangeLow);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmSigma);
            this.GroupBox_FwhmWeight.Location = new System.Drawing.Point(17, 31);
            this.GroupBox_FwhmWeight.Name = "GroupBox_FwhmWeight";
            this.GroupBox_FwhmWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_FwhmWeight.TabIndex = 12;
            this.GroupBox_FwhmWeight.TabStop = false;
            this.GroupBox_FwhmWeight.Text = "FWHM Weight";
            // 
            // Label_FwhmMinValue
            // 
            this.Label_FwhmMinValue.AutoSize = true;
            this.Label_FwhmMinValue.Location = new System.Drawing.Point(55, 49);
            this.Label_FwhmMinValue.Name = "Label_FwhmMinValue";
            this.Label_FwhmMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_FwhmMinValue.TabIndex = 23;
            this.Label_FwhmMinValue.Text = "0";
            // 
            // Label_FwhmMaxValue
            // 
            this.Label_FwhmMaxValue.AutoSize = true;
            this.Label_FwhmMaxValue.Location = new System.Drawing.Point(55, 66);
            this.Label_FwhmMaxValue.Name = "Label_FwhmMaxValue";
            this.Label_FwhmMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_FwhmMaxValue.TabIndex = 22;
            this.Label_FwhmMaxValue.Text = "0";
            // 
            // Label_FwhmMedianValue
            // 
            this.Label_FwhmMedianValue.AutoSize = true;
            this.Label_FwhmMedianValue.Location = new System.Drawing.Point(55, 32);
            this.Label_FwhmMedianValue.Name = "Label_FwhmMedianValue";
            this.Label_FwhmMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_FwhmMedianValue.TabIndex = 21;
            this.Label_FwhmMedianValue.Text = "0";
            // 
            // Label_FwhmMeanValue
            // 
            this.Label_FwhmMeanValue.AutoSize = true;
            this.Label_FwhmMeanValue.Location = new System.Drawing.Point(55, 15);
            this.Label_FwhmMeanValue.Name = "Label_FwhmMeanValue";
            this.Label_FwhmMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_FwhmMeanValue.TabIndex = 19;
            this.Label_FwhmMeanValue.Text = "0";
            // 
            // Label_FwhmSigmaValue
            // 
            this.Label_FwhmSigmaValue.AutoSize = true;
            this.Label_FwhmSigmaValue.Location = new System.Drawing.Point(55, 83);
            this.Label_FwhmSigmaValue.Name = "Label_FwhmSigmaValue";
            this.Label_FwhmSigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_FwhmSigmaValue.TabIndex = 20;
            this.Label_FwhmSigmaValue.Text = "0";
            // 
            // Label_FwhmMin
            // 
            this.Label_FwhmMin.AutoSize = true;
            this.Label_FwhmMin.Location = new System.Drawing.Point(12, 49);
            this.Label_FwhmMin.Name = "Label_FwhmMin";
            this.Label_FwhmMin.Size = new System.Drawing.Size(30, 13);
            this.Label_FwhmMin.TabIndex = 18;
            this.Label_FwhmMin.Text = "Min: ";
            // 
            // Label_FwhmMax
            // 
            this.Label_FwhmMax.AutoSize = true;
            this.Label_FwhmMax.Location = new System.Drawing.Point(12, 66);
            this.Label_FwhmMax.Name = "Label_FwhmMax";
            this.Label_FwhmMax.Size = new System.Drawing.Size(33, 13);
            this.Label_FwhmMax.TabIndex = 17;
            this.Label_FwhmMax.Text = "Max: ";
            // 
            // Label_FwhmMedian
            // 
            this.Label_FwhmMedian.AutoSize = true;
            this.Label_FwhmMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_FwhmMedian.Name = "Label_FwhmMedian";
            this.Label_FwhmMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_FwhmMedian.TabIndex = 16;
            this.Label_FwhmMedian.Text = "Median:";
            // 
            // Label_FwhmRangeLow
            // 
            this.Label_FwhmRangeLow.AutoSize = true;
            this.Label_FwhmRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_FwhmRangeLow.Name = "Label_FwhmRangeLow";
            this.Label_FwhmRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_FwhmRangeLow.TabIndex = 15;
            this.Label_FwhmRangeLow.Text = "Low";
            this.Label_FwhmRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmMean
            // 
            this.Label_FwhmMean.AutoSize = true;
            this.Label_FwhmMean.Location = new System.Drawing.Point(12, 15);
            this.Label_FwhmMean.Name = "Label_FwhmMean";
            this.Label_FwhmMean.Size = new System.Drawing.Size(40, 13);
            this.Label_FwhmMean.TabIndex = 4;
            this.Label_FwhmMean.Text = "Mean: ";
            // 
            // Label_FwhmRangeHigh
            // 
            this.Label_FwhmRangeHigh.AutoSize = true;
            this.Label_FwhmRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_FwhmRangeHigh.Name = "Label_FwhmRangeHigh";
            this.Label_FwhmRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_FwhmRangeHigh.TabIndex = 14;
            this.Label_FwhmRangeHigh.Text = "High";
            this.Label_FwhmRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_FwhmRangeHigh
            // 
            this.TextBox_FwhmRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_FwhmRangeHigh.Name = "TextBox_FwhmRangeHigh";
            this.TextBox_FwhmRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_FwhmRangeHigh.TabIndex = 2;
            this.TextBox_FwhmRangeHigh.Text = "1.0";
            this.TextBox_FwhmRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmRangeHigh.TextChanged += new System.EventHandler(this.TextBox_FwhmRangeHigh_TextChanged);
            // 
            // TextBox_FwhmRangeLow
            // 
            this.TextBox_FwhmRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_FwhmRangeLow.Name = "TextBox_FwhmRangeLow";
            this.TextBox_FwhmRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_FwhmRangeLow.TabIndex = 3;
            this.TextBox_FwhmRangeLow.Text = "0.0";
            this.TextBox_FwhmRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmRangeLow.TextChanged += new System.EventHandler(this.TextBox_FwhmRangeLow_TextChanged);
            // 
            // Label_FwhmSigma
            // 
            this.Label_FwhmSigma.AutoSize = true;
            this.Label_FwhmSigma.Location = new System.Drawing.Point(12, 83);
            this.Label_FwhmSigma.Name = "Label_FwhmSigma";
            this.Label_FwhmSigma.Size = new System.Drawing.Size(42, 13);
            this.Label_FwhmSigma.TabIndex = 5;
            this.Label_FwhmSigma.Text = "Sigma: ";
            // 
            // GroupBox_StarsWeight
            // 
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMinValue);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMaxValue);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMedianValue);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMeanValue);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsSigmaValue);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMin);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMax);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMedian);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMean);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsSigma);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarRangeLow);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarRangeHigh);
            this.GroupBox_StarsWeight.Controls.Add(this.TextBox_StarRangeHigh);
            this.GroupBox_StarsWeight.Controls.Add(this.TextBox_StarRangeLow);
            this.GroupBox_StarsWeight.Location = new System.Drawing.Point(241, 137);
            this.GroupBox_StarsWeight.Name = "GroupBox_StarsWeight";
            this.GroupBox_StarsWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_StarsWeight.TabIndex = 23;
            this.GroupBox_StarsWeight.TabStop = false;
            this.GroupBox_StarsWeight.Text = "Star Weight";
            // 
            // Label_StarsMinValue
            // 
            this.Label_StarsMinValue.AutoSize = true;
            this.Label_StarsMinValue.Location = new System.Drawing.Point(55, 50);
            this.Label_StarsMinValue.Name = "Label_StarsMinValue";
            this.Label_StarsMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarsMinValue.TabIndex = 33;
            this.Label_StarsMinValue.Text = "0";
            // 
            // Label_StarsMaxValue
            // 
            this.Label_StarsMaxValue.AutoSize = true;
            this.Label_StarsMaxValue.Location = new System.Drawing.Point(55, 67);
            this.Label_StarsMaxValue.Name = "Label_StarsMaxValue";
            this.Label_StarsMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarsMaxValue.TabIndex = 32;
            this.Label_StarsMaxValue.Text = "0";
            // 
            // Label_StarsMedianValue
            // 
            this.Label_StarsMedianValue.AutoSize = true;
            this.Label_StarsMedianValue.Location = new System.Drawing.Point(55, 33);
            this.Label_StarsMedianValue.Name = "Label_StarsMedianValue";
            this.Label_StarsMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarsMedianValue.TabIndex = 31;
            this.Label_StarsMedianValue.Text = "0";
            // 
            // Label_StarsMeanValue
            // 
            this.Label_StarsMeanValue.AutoSize = true;
            this.Label_StarsMeanValue.Location = new System.Drawing.Point(55, 16);
            this.Label_StarsMeanValue.Name = "Label_StarsMeanValue";
            this.Label_StarsMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarsMeanValue.TabIndex = 29;
            this.Label_StarsMeanValue.Text = "0";
            // 
            // Label_StarsSigmaValue
            // 
            this.Label_StarsSigmaValue.AutoSize = true;
            this.Label_StarsSigmaValue.Location = new System.Drawing.Point(55, 84);
            this.Label_StarsSigmaValue.Name = "Label_StarsSigmaValue";
            this.Label_StarsSigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_StarsSigmaValue.TabIndex = 30;
            this.Label_StarsSigmaValue.Text = "0";
            // 
            // Label_StarsMin
            // 
            this.Label_StarsMin.AutoSize = true;
            this.Label_StarsMin.Location = new System.Drawing.Point(12, 49);
            this.Label_StarsMin.Name = "Label_StarsMin";
            this.Label_StarsMin.Size = new System.Drawing.Size(30, 13);
            this.Label_StarsMin.TabIndex = 23;
            this.Label_StarsMin.Text = "Min: ";
            // 
            // Label_StarsMax
            // 
            this.Label_StarsMax.AutoSize = true;
            this.Label_StarsMax.Location = new System.Drawing.Point(12, 66);
            this.Label_StarsMax.Name = "Label_StarsMax";
            this.Label_StarsMax.Size = new System.Drawing.Size(33, 13);
            this.Label_StarsMax.TabIndex = 22;
            this.Label_StarsMax.Text = "Max: ";
            // 
            // Label_StarsMedian
            // 
            this.Label_StarsMedian.AutoSize = true;
            this.Label_StarsMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_StarsMedian.Name = "Label_StarsMedian";
            this.Label_StarsMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_StarsMedian.TabIndex = 21;
            this.Label_StarsMedian.Text = "Median:";
            // 
            // Label_StarsMean
            // 
            this.Label_StarsMean.AutoSize = true;
            this.Label_StarsMean.Location = new System.Drawing.Point(12, 15);
            this.Label_StarsMean.Name = "Label_StarsMean";
            this.Label_StarsMean.Size = new System.Drawing.Size(40, 13);
            this.Label_StarsMean.TabIndex = 19;
            this.Label_StarsMean.Text = "Mean: ";
            // 
            // Label_StarsSigma
            // 
            this.Label_StarsSigma.AutoSize = true;
            this.Label_StarsSigma.Location = new System.Drawing.Point(12, 83);
            this.Label_StarsSigma.Name = "Label_StarsSigma";
            this.Label_StarsSigma.Size = new System.Drawing.Size(42, 13);
            this.Label_StarsSigma.TabIndex = 20;
            this.Label_StarsSigma.Text = "Sigma: ";
            // 
            // Label_StarRangeLow
            // 
            this.Label_StarRangeLow.AutoSize = true;
            this.Label_StarRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_StarRangeLow.Name = "Label_StarRangeLow";
            this.Label_StarRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_StarRangeLow.TabIndex = 15;
            this.Label_StarRangeLow.Text = "Low";
            this.Label_StarRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarRangeHigh
            // 
            this.Label_StarRangeHigh.AutoSize = true;
            this.Label_StarRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_StarRangeHigh.Name = "Label_StarRangeHigh";
            this.Label_StarRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_StarRangeHigh.TabIndex = 14;
            this.Label_StarRangeHigh.Text = "High";
            this.Label_StarRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_StarRangeHigh
            // 
            this.TextBox_StarRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_StarRangeHigh.Name = "TextBox_StarRangeHigh";
            this.TextBox_StarRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarRangeHigh.TabIndex = 2;
            this.TextBox_StarRangeHigh.Text = "1.0";
            this.TextBox_StarRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarRangeHigh.TextChanged += new System.EventHandler(this.TextBox_StarsRangeHigh_TextChanged);
            // 
            // TextBox_StarRangeLow
            // 
            this.TextBox_StarRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_StarRangeLow.Name = "TextBox_StarRangeLow";
            this.TextBox_StarRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarRangeLow.TabIndex = 3;
            this.TextBox_StarRangeLow.Text = "0.0";
            this.TextBox_StarRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarRangeLow.TextChanged += new System.EventHandler(this.TextBox_StarsRangeLow_TextChanged);
            // 
            // GroupBox_EccentricityWeight
            // 
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMinValue);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMaxValue);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMedianValue);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMeanValue);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricitySigmaValue);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMin);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMax);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMedian);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityMean);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricitySigma);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityRangeLow);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityRangeHigh);
            this.GroupBox_EccentricityWeight.Controls.Add(this.TextBox_EccentricityRangeHigh);
            this.GroupBox_EccentricityWeight.Controls.Add(this.TextBox_EccentricityRangeLow);
            this.GroupBox_EccentricityWeight.Location = new System.Drawing.Point(241, 31);
            this.GroupBox_EccentricityWeight.Name = "GroupBox_EccentricityWeight";
            this.GroupBox_EccentricityWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_EccentricityWeight.TabIndex = 13;
            this.GroupBox_EccentricityWeight.TabStop = false;
            this.GroupBox_EccentricityWeight.Text = "Eccentricity Weight";
            // 
            // Label_EccentricityMinValue
            // 
            this.Label_EccentricityMinValue.AutoSize = true;
            this.Label_EccentricityMinValue.Location = new System.Drawing.Point(55, 49);
            this.Label_EccentricityMinValue.Name = "Label_EccentricityMinValue";
            this.Label_EccentricityMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_EccentricityMinValue.TabIndex = 28;
            this.Label_EccentricityMinValue.Text = "0";
            // 
            // Label_EccentricityMaxValue
            // 
            this.Label_EccentricityMaxValue.AutoSize = true;
            this.Label_EccentricityMaxValue.Location = new System.Drawing.Point(55, 66);
            this.Label_EccentricityMaxValue.Name = "Label_EccentricityMaxValue";
            this.Label_EccentricityMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_EccentricityMaxValue.TabIndex = 27;
            this.Label_EccentricityMaxValue.Text = "0";
            // 
            // Label_EccentricityMedianValue
            // 
            this.Label_EccentricityMedianValue.AutoSize = true;
            this.Label_EccentricityMedianValue.Location = new System.Drawing.Point(55, 32);
            this.Label_EccentricityMedianValue.Name = "Label_EccentricityMedianValue";
            this.Label_EccentricityMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_EccentricityMedianValue.TabIndex = 26;
            this.Label_EccentricityMedianValue.Text = "0";
            // 
            // Label_EccentricityMeanValue
            // 
            this.Label_EccentricityMeanValue.AutoSize = true;
            this.Label_EccentricityMeanValue.Location = new System.Drawing.Point(55, 15);
            this.Label_EccentricityMeanValue.Name = "Label_EccentricityMeanValue";
            this.Label_EccentricityMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_EccentricityMeanValue.TabIndex = 24;
            this.Label_EccentricityMeanValue.Text = "0";
            // 
            // Label_EccentricitySigmaValue
            // 
            this.Label_EccentricitySigmaValue.AutoSize = true;
            this.Label_EccentricitySigmaValue.Location = new System.Drawing.Point(55, 83);
            this.Label_EccentricitySigmaValue.Name = "Label_EccentricitySigmaValue";
            this.Label_EccentricitySigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_EccentricitySigmaValue.TabIndex = 25;
            this.Label_EccentricitySigmaValue.Text = "0";
            // 
            // Label_EccentricityMin
            // 
            this.Label_EccentricityMin.AutoSize = true;
            this.Label_EccentricityMin.Location = new System.Drawing.Point(12, 49);
            this.Label_EccentricityMin.Name = "Label_EccentricityMin";
            this.Label_EccentricityMin.Size = new System.Drawing.Size(30, 13);
            this.Label_EccentricityMin.TabIndex = 23;
            this.Label_EccentricityMin.Text = "Min: ";
            // 
            // Label_EccentricityMax
            // 
            this.Label_EccentricityMax.AutoSize = true;
            this.Label_EccentricityMax.Location = new System.Drawing.Point(12, 66);
            this.Label_EccentricityMax.Name = "Label_EccentricityMax";
            this.Label_EccentricityMax.Size = new System.Drawing.Size(33, 13);
            this.Label_EccentricityMax.TabIndex = 22;
            this.Label_EccentricityMax.Text = "Max: ";
            // 
            // Label_EccentricityMedian
            // 
            this.Label_EccentricityMedian.AutoSize = true;
            this.Label_EccentricityMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_EccentricityMedian.Name = "Label_EccentricityMedian";
            this.Label_EccentricityMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_EccentricityMedian.TabIndex = 21;
            this.Label_EccentricityMedian.Text = "Median:";
            // 
            // Label_EccentricityMean
            // 
            this.Label_EccentricityMean.AutoSize = true;
            this.Label_EccentricityMean.Location = new System.Drawing.Point(12, 15);
            this.Label_EccentricityMean.Name = "Label_EccentricityMean";
            this.Label_EccentricityMean.Size = new System.Drawing.Size(40, 13);
            this.Label_EccentricityMean.TabIndex = 19;
            this.Label_EccentricityMean.Text = "Mean: ";
            // 
            // Label_EccentricitySigma
            // 
            this.Label_EccentricitySigma.AutoSize = true;
            this.Label_EccentricitySigma.Location = new System.Drawing.Point(12, 83);
            this.Label_EccentricitySigma.Name = "Label_EccentricitySigma";
            this.Label_EccentricitySigma.Size = new System.Drawing.Size(42, 13);
            this.Label_EccentricitySigma.TabIndex = 20;
            this.Label_EccentricitySigma.Text = "Sigma: ";
            // 
            // Label_EccentricityRangeLow
            // 
            this.Label_EccentricityRangeLow.AutoSize = true;
            this.Label_EccentricityRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_EccentricityRangeLow.Name = "Label_EccentricityRangeLow";
            this.Label_EccentricityRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_EccentricityRangeLow.TabIndex = 13;
            this.Label_EccentricityRangeLow.Text = "Low";
            this.Label_EccentricityRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EccentricityRangeHigh
            // 
            this.Label_EccentricityRangeHigh.AutoSize = true;
            this.Label_EccentricityRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_EccentricityRangeHigh.Name = "Label_EccentricityRangeHigh";
            this.Label_EccentricityRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_EccentricityRangeHigh.TabIndex = 12;
            this.Label_EccentricityRangeHigh.Text = "High";
            this.Label_EccentricityRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_EccentricityRangeHigh
            // 
            this.TextBox_EccentricityRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_EccentricityRangeHigh.Name = "TextBox_EccentricityRangeHigh";
            this.TextBox_EccentricityRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_EccentricityRangeHigh.TabIndex = 8;
            this.TextBox_EccentricityRangeHigh.Text = "1.0";
            this.TextBox_EccentricityRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityRangeHigh.TextChanged += new System.EventHandler(this.TextBox_EccentricityRangeHigh_TextChanged);
            // 
            // TextBox_EccentricityRangeLow
            // 
            this.TextBox_EccentricityRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_EccentricityRangeLow.Name = "TextBox_EccentricityRangeLow";
            this.TextBox_EccentricityRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_EccentricityRangeLow.TabIndex = 9;
            this.TextBox_EccentricityRangeLow.Text = "0.0";
            this.TextBox_EccentricityRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityRangeLow.TextChanged += new System.EventHandler(this.TextBox_EccentricityRangeLow_TextChanged);
            // 
            // GroupBox_AirMassWeight
            // 
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMinValue);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMaxValue);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMedianValue);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMeanValue);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassSigmaValue);
            this.GroupBox_AirMassWeight.Controls.Add(this.TextBox_AirMassRangeLow);
            this.GroupBox_AirMassWeight.Controls.Add(this.TextBox_AirMassRangeHigh);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMin);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMax);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMedian);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassMean);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_AirMassSigma);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_FwhmMeanDeviationRangeLow);
            this.GroupBox_AirMassWeight.Controls.Add(this.Label_FwhmMeanDeviationRangeHigh);
            this.GroupBox_AirMassWeight.Location = new System.Drawing.Point(17, 137);
            this.GroupBox_AirMassWeight.Name = "GroupBox_AirMassWeight";
            this.GroupBox_AirMassWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_AirMassWeight.TabIndex = 22;
            this.GroupBox_AirMassWeight.TabStop = false;
            this.GroupBox_AirMassWeight.Text = "Air Mass Weight";
            // 
            // Label_AirMassMinValue
            // 
            this.Label_AirMassMinValue.AutoSize = true;
            this.Label_AirMassMinValue.Location = new System.Drawing.Point(55, 49);
            this.Label_AirMassMinValue.Name = "Label_AirMassMinValue";
            this.Label_AirMassMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_AirMassMinValue.TabIndex = 30;
            this.Label_AirMassMinValue.Text = "0";
            // 
            // Label_AirMassMaxValue
            // 
            this.Label_AirMassMaxValue.AutoSize = true;
            this.Label_AirMassMaxValue.Location = new System.Drawing.Point(55, 66);
            this.Label_AirMassMaxValue.Name = "Label_AirMassMaxValue";
            this.Label_AirMassMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_AirMassMaxValue.TabIndex = 29;
            this.Label_AirMassMaxValue.Text = "0";
            // 
            // Label_AirMassMedianValue
            // 
            this.Label_AirMassMedianValue.AutoSize = true;
            this.Label_AirMassMedianValue.Location = new System.Drawing.Point(55, 32);
            this.Label_AirMassMedianValue.Name = "Label_AirMassMedianValue";
            this.Label_AirMassMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_AirMassMedianValue.TabIndex = 28;
            this.Label_AirMassMedianValue.Text = "0";
            // 
            // Label_AirMassMeanValue
            // 
            this.Label_AirMassMeanValue.AutoSize = true;
            this.Label_AirMassMeanValue.Location = new System.Drawing.Point(55, 15);
            this.Label_AirMassMeanValue.Name = "Label_AirMassMeanValue";
            this.Label_AirMassMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_AirMassMeanValue.TabIndex = 26;
            this.Label_AirMassMeanValue.Text = "0";
            // 
            // Label_AirMassSigmaValue
            // 
            this.Label_AirMassSigmaValue.AutoSize = true;
            this.Label_AirMassSigmaValue.Location = new System.Drawing.Point(55, 83);
            this.Label_AirMassSigmaValue.Name = "Label_AirMassSigmaValue";
            this.Label_AirMassSigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_AirMassSigmaValue.TabIndex = 27;
            this.Label_AirMassSigmaValue.Text = "0";
            // 
            // TextBox_AirMassRangeLow
            // 
            this.TextBox_AirMassRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_AirMassRangeLow.Name = "TextBox_AirMassRangeLow";
            this.TextBox_AirMassRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_AirMassRangeLow.TabIndex = 25;
            this.TextBox_AirMassRangeLow.Text = "0.0";
            this.TextBox_AirMassRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBox_AirMassRangeHigh
            // 
            this.TextBox_AirMassRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_AirMassRangeHigh.Name = "TextBox_AirMassRangeHigh";
            this.TextBox_AirMassRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_AirMassRangeHigh.TabIndex = 24;
            this.TextBox_AirMassRangeHigh.Text = "1.0";
            this.TextBox_AirMassRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label_AirMassMin
            // 
            this.Label_AirMassMin.AutoSize = true;
            this.Label_AirMassMin.Location = new System.Drawing.Point(12, 49);
            this.Label_AirMassMin.Name = "Label_AirMassMin";
            this.Label_AirMassMin.Size = new System.Drawing.Size(30, 13);
            this.Label_AirMassMin.TabIndex = 23;
            this.Label_AirMassMin.Text = "Min: ";
            // 
            // Label_AirMassMax
            // 
            this.Label_AirMassMax.AutoSize = true;
            this.Label_AirMassMax.Location = new System.Drawing.Point(12, 66);
            this.Label_AirMassMax.Name = "Label_AirMassMax";
            this.Label_AirMassMax.Size = new System.Drawing.Size(33, 13);
            this.Label_AirMassMax.TabIndex = 22;
            this.Label_AirMassMax.Text = "Max: ";
            // 
            // Label_AirMassMedian
            // 
            this.Label_AirMassMedian.AutoSize = true;
            this.Label_AirMassMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_AirMassMedian.Name = "Label_AirMassMedian";
            this.Label_AirMassMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_AirMassMedian.TabIndex = 21;
            this.Label_AirMassMedian.Text = "Median:";
            // 
            // Label_AirMassMean
            // 
            this.Label_AirMassMean.AutoSize = true;
            this.Label_AirMassMean.Location = new System.Drawing.Point(12, 15);
            this.Label_AirMassMean.Name = "Label_AirMassMean";
            this.Label_AirMassMean.Size = new System.Drawing.Size(40, 13);
            this.Label_AirMassMean.TabIndex = 19;
            this.Label_AirMassMean.Text = "Mean: ";
            // 
            // Label_AirMassSigma
            // 
            this.Label_AirMassSigma.AutoSize = true;
            this.Label_AirMassSigma.Location = new System.Drawing.Point(12, 83);
            this.Label_AirMassSigma.Name = "Label_AirMassSigma";
            this.Label_AirMassSigma.Size = new System.Drawing.Size(42, 13);
            this.Label_AirMassSigma.TabIndex = 20;
            this.Label_AirMassSigma.Text = "Sigma: ";
            // 
            // Label_FwhmMeanDeviationRangeLow
            // 
            this.Label_FwhmMeanDeviationRangeLow.AutoSize = true;
            this.Label_FwhmMeanDeviationRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_FwhmMeanDeviationRangeLow.Name = "Label_FwhmMeanDeviationRangeLow";
            this.Label_FwhmMeanDeviationRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_FwhmMeanDeviationRangeLow.TabIndex = 15;
            this.Label_FwhmMeanDeviationRangeLow.Text = "Low";
            this.Label_FwhmMeanDeviationRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmMeanDeviationRangeHigh
            // 
            this.Label_FwhmMeanDeviationRangeHigh.AutoSize = true;
            this.Label_FwhmMeanDeviationRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_FwhmMeanDeviationRangeHigh.Name = "Label_FwhmMeanDeviationRangeHigh";
            this.Label_FwhmMeanDeviationRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_FwhmMeanDeviationRangeHigh.TabIndex = 14;
            this.Label_FwhmMeanDeviationRangeHigh.Text = "High";
            this.Label_FwhmMeanDeviationRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox_NoiseWeight
            // 
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMinValue);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMaxValue);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMedianValue);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMeanValue);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseSigmaValue);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMin);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMax);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMedian);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMean);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseSigma);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseRangeLow);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseRangeHigh);
            this.GroupBox_NoiseWeight.Controls.Add(this.TextBox_NoiseRangeHigh);
            this.GroupBox_NoiseWeight.Controls.Add(this.TextBox_NoiseRangeLow);
            this.GroupBox_NoiseWeight.Location = new System.Drawing.Point(689, 31);
            this.GroupBox_NoiseWeight.Name = "GroupBox_NoiseWeight";
            this.GroupBox_NoiseWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_NoiseWeight.TabIndex = 19;
            this.GroupBox_NoiseWeight.TabStop = false;
            this.GroupBox_NoiseWeight.Text = "Noise Weight";
            // 
            // Label_NoiseMinValue
            // 
            this.Label_NoiseMinValue.AutoSize = true;
            this.Label_NoiseMinValue.Location = new System.Drawing.Point(56, 50);
            this.Label_NoiseMinValue.Name = "Label_NoiseMinValue";
            this.Label_NoiseMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_NoiseMinValue.TabIndex = 38;
            this.Label_NoiseMinValue.Text = "0";
            // 
            // Label_NoiseMaxValue
            // 
            this.Label_NoiseMaxValue.AutoSize = true;
            this.Label_NoiseMaxValue.Location = new System.Drawing.Point(56, 67);
            this.Label_NoiseMaxValue.Name = "Label_NoiseMaxValue";
            this.Label_NoiseMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_NoiseMaxValue.TabIndex = 37;
            this.Label_NoiseMaxValue.Text = "0";
            // 
            // Label_NoiseMedianValue
            // 
            this.Label_NoiseMedianValue.AutoSize = true;
            this.Label_NoiseMedianValue.Location = new System.Drawing.Point(56, 33);
            this.Label_NoiseMedianValue.Name = "Label_NoiseMedianValue";
            this.Label_NoiseMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_NoiseMedianValue.TabIndex = 36;
            this.Label_NoiseMedianValue.Text = "0";
            // 
            // Label_NoiseMeanValue
            // 
            this.Label_NoiseMeanValue.AutoSize = true;
            this.Label_NoiseMeanValue.Location = new System.Drawing.Point(56, 16);
            this.Label_NoiseMeanValue.Name = "Label_NoiseMeanValue";
            this.Label_NoiseMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_NoiseMeanValue.TabIndex = 34;
            this.Label_NoiseMeanValue.Text = "0";
            // 
            // Label_NoiseSigmaValue
            // 
            this.Label_NoiseSigmaValue.AutoSize = true;
            this.Label_NoiseSigmaValue.Location = new System.Drawing.Point(56, 84);
            this.Label_NoiseSigmaValue.Name = "Label_NoiseSigmaValue";
            this.Label_NoiseSigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_NoiseSigmaValue.TabIndex = 35;
            this.Label_NoiseSigmaValue.Text = "0";
            // 
            // Label_NoiseMin
            // 
            this.Label_NoiseMin.AutoSize = true;
            this.Label_NoiseMin.Location = new System.Drawing.Point(12, 49);
            this.Label_NoiseMin.Name = "Label_NoiseMin";
            this.Label_NoiseMin.Size = new System.Drawing.Size(30, 13);
            this.Label_NoiseMin.TabIndex = 23;
            this.Label_NoiseMin.Text = "Min: ";
            // 
            // Label_NoiseMax
            // 
            this.Label_NoiseMax.AutoSize = true;
            this.Label_NoiseMax.Location = new System.Drawing.Point(12, 66);
            this.Label_NoiseMax.Name = "Label_NoiseMax";
            this.Label_NoiseMax.Size = new System.Drawing.Size(33, 13);
            this.Label_NoiseMax.TabIndex = 22;
            this.Label_NoiseMax.Text = "Max: ";
            // 
            // Label_NoiseMedian
            // 
            this.Label_NoiseMedian.AutoSize = true;
            this.Label_NoiseMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_NoiseMedian.Name = "Label_NoiseMedian";
            this.Label_NoiseMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_NoiseMedian.TabIndex = 21;
            this.Label_NoiseMedian.Text = "Median:";
            // 
            // Label_NoiseMean
            // 
            this.Label_NoiseMean.AutoSize = true;
            this.Label_NoiseMean.Location = new System.Drawing.Point(12, 15);
            this.Label_NoiseMean.Name = "Label_NoiseMean";
            this.Label_NoiseMean.Size = new System.Drawing.Size(40, 13);
            this.Label_NoiseMean.TabIndex = 19;
            this.Label_NoiseMean.Text = "Mean: ";
            // 
            // Label_NoiseSigma
            // 
            this.Label_NoiseSigma.AutoSize = true;
            this.Label_NoiseSigma.Location = new System.Drawing.Point(12, 83);
            this.Label_NoiseSigma.Name = "Label_NoiseSigma";
            this.Label_NoiseSigma.Size = new System.Drawing.Size(42, 13);
            this.Label_NoiseSigma.TabIndex = 20;
            this.Label_NoiseSigma.Text = "Sigma: ";
            // 
            // Label_NoiseRangeLow
            // 
            this.Label_NoiseRangeLow.AutoSize = true;
            this.Label_NoiseRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_NoiseRangeLow.Name = "Label_NoiseRangeLow";
            this.Label_NoiseRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_NoiseRangeLow.TabIndex = 15;
            this.Label_NoiseRangeLow.Text = "Low";
            this.Label_NoiseRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_NoiseRangeHigh
            // 
            this.Label_NoiseRangeHigh.AutoSize = true;
            this.Label_NoiseRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_NoiseRangeHigh.Name = "Label_NoiseRangeHigh";
            this.Label_NoiseRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_NoiseRangeHigh.TabIndex = 14;
            this.Label_NoiseRangeHigh.Text = "High";
            this.Label_NoiseRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_NoiseRangeHigh
            // 
            this.TextBox_NoiseRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_NoiseRangeHigh.Name = "TextBox_NoiseRangeHigh";
            this.TextBox_NoiseRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_NoiseRangeHigh.TabIndex = 2;
            this.TextBox_NoiseRangeHigh.Text = "1.0";
            this.TextBox_NoiseRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoiseRangeHigh.TextChanged += new System.EventHandler(this.TextBox_NoiseRangeHigh_TextChanged);
            // 
            // TextBox_NoiseRangeLow
            // 
            this.TextBox_NoiseRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_NoiseRangeLow.Name = "TextBox_NoiseRangeLow";
            this.TextBox_NoiseRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_NoiseRangeLow.TabIndex = 3;
            this.TextBox_NoiseRangeLow.Text = "0.0";
            this.TextBox_NoiseRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoiseRangeLow.TextChanged += new System.EventHandler(this.TextBox_NoiseRangeLow_TextChanged);
            // 
            // GroupBox_MedianWeight
            // 
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMinValue);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMaxValue);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMedianValue);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMeanValue);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianSigmaValue);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMin);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMax);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMedian);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMean);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianSigma);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianRangeLow);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianRangeHigh);
            this.GroupBox_MedianWeight.Controls.Add(this.TextBox_MedianRangeHigh);
            this.GroupBox_MedianWeight.Controls.Add(this.TextBox_MedianRangeLow);
            this.GroupBox_MedianWeight.Location = new System.Drawing.Point(465, 31);
            this.GroupBox_MedianWeight.Name = "GroupBox_MedianWeight";
            this.GroupBox_MedianWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_MedianWeight.TabIndex = 17;
            this.GroupBox_MedianWeight.TabStop = false;
            this.GroupBox_MedianWeight.Text = "Median Weight";
            // 
            // Label_MedianMinValue
            // 
            this.Label_MedianMinValue.AutoSize = true;
            this.Label_MedianMinValue.Location = new System.Drawing.Point(55, 50);
            this.Label_MedianMinValue.Name = "Label_MedianMinValue";
            this.Label_MedianMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_MedianMinValue.TabIndex = 33;
            this.Label_MedianMinValue.Text = "0";
            // 
            // Label_MedianMaxValue
            // 
            this.Label_MedianMaxValue.AutoSize = true;
            this.Label_MedianMaxValue.Location = new System.Drawing.Point(55, 67);
            this.Label_MedianMaxValue.Name = "Label_MedianMaxValue";
            this.Label_MedianMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_MedianMaxValue.TabIndex = 32;
            this.Label_MedianMaxValue.Text = "0";
            // 
            // Label_MedianMedianValue
            // 
            this.Label_MedianMedianValue.AutoSize = true;
            this.Label_MedianMedianValue.Location = new System.Drawing.Point(55, 33);
            this.Label_MedianMedianValue.Name = "Label_MedianMedianValue";
            this.Label_MedianMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_MedianMedianValue.TabIndex = 31;
            this.Label_MedianMedianValue.Text = "0";
            // 
            // Label_MedianMeanValue
            // 
            this.Label_MedianMeanValue.AutoSize = true;
            this.Label_MedianMeanValue.Location = new System.Drawing.Point(55, 16);
            this.Label_MedianMeanValue.Name = "Label_MedianMeanValue";
            this.Label_MedianMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_MedianMeanValue.TabIndex = 29;
            this.Label_MedianMeanValue.Text = "0";
            // 
            // Label_MedianSigmaValue
            // 
            this.Label_MedianSigmaValue.AutoSize = true;
            this.Label_MedianSigmaValue.Location = new System.Drawing.Point(55, 84);
            this.Label_MedianSigmaValue.Name = "Label_MedianSigmaValue";
            this.Label_MedianSigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_MedianSigmaValue.TabIndex = 30;
            this.Label_MedianSigmaValue.Text = "0";
            // 
            // Label_MedianMin
            // 
            this.Label_MedianMin.AutoSize = true;
            this.Label_MedianMin.Location = new System.Drawing.Point(12, 49);
            this.Label_MedianMin.Name = "Label_MedianMin";
            this.Label_MedianMin.Size = new System.Drawing.Size(30, 13);
            this.Label_MedianMin.TabIndex = 23;
            this.Label_MedianMin.Text = "Min: ";
            // 
            // Label_MedianMax
            // 
            this.Label_MedianMax.AutoSize = true;
            this.Label_MedianMax.Location = new System.Drawing.Point(12, 66);
            this.Label_MedianMax.Name = "Label_MedianMax";
            this.Label_MedianMax.Size = new System.Drawing.Size(33, 13);
            this.Label_MedianMax.TabIndex = 22;
            this.Label_MedianMax.Text = "Max: ";
            // 
            // Label_MedianMedian
            // 
            this.Label_MedianMedian.AutoSize = true;
            this.Label_MedianMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_MedianMedian.Name = "Label_MedianMedian";
            this.Label_MedianMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_MedianMedian.TabIndex = 21;
            this.Label_MedianMedian.Text = "Median:";
            // 
            // Label_MedianMean
            // 
            this.Label_MedianMean.AutoSize = true;
            this.Label_MedianMean.Location = new System.Drawing.Point(12, 15);
            this.Label_MedianMean.Name = "Label_MedianMean";
            this.Label_MedianMean.Size = new System.Drawing.Size(40, 13);
            this.Label_MedianMean.TabIndex = 19;
            this.Label_MedianMean.Text = "Mean: ";
            // 
            // Label_MedianSigma
            // 
            this.Label_MedianSigma.AutoSize = true;
            this.Label_MedianSigma.Location = new System.Drawing.Point(12, 83);
            this.Label_MedianSigma.Name = "Label_MedianSigma";
            this.Label_MedianSigma.Size = new System.Drawing.Size(42, 13);
            this.Label_MedianSigma.TabIndex = 20;
            this.Label_MedianSigma.Text = "Sigma: ";
            // 
            // Label_MedianRangeLow
            // 
            this.Label_MedianRangeLow.AutoSize = true;
            this.Label_MedianRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_MedianRangeLow.Name = "Label_MedianRangeLow";
            this.Label_MedianRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_MedianRangeLow.TabIndex = 15;
            this.Label_MedianRangeLow.Text = "Low";
            this.Label_MedianRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MedianRangeHigh
            // 
            this.Label_MedianRangeHigh.AutoSize = true;
            this.Label_MedianRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_MedianRangeHigh.Name = "Label_MedianRangeHigh";
            this.Label_MedianRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_MedianRangeHigh.TabIndex = 14;
            this.Label_MedianRangeHigh.Text = "High";
            this.Label_MedianRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_MedianRangeHigh
            // 
            this.TextBox_MedianRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_MedianRangeHigh.Name = "TextBox_MedianRangeHigh";
            this.TextBox_MedianRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_MedianRangeHigh.TabIndex = 2;
            this.TextBox_MedianRangeHigh.Text = "1.0";
            this.TextBox_MedianRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianRangeHigh.TextChanged += new System.EventHandler(this.TextBox_MedianRangeHigh_TextChanged);
            // 
            // TextBox_MedianRangeLow
            // 
            this.TextBox_MedianRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_MedianRangeLow.Name = "TextBox_MedianRangeLow";
            this.TextBox_MedianRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_MedianRangeLow.TabIndex = 3;
            this.TextBox_MedianRangeLow.Text = "0.0";
            this.TextBox_MedianRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianRangeLow.TextChanged += new System.EventHandler(this.TextBox_MedianRangeLow_TextChanged);
            // 
            // GroupBox_SnrWeight
            // 
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMinValue);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMaxValue);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMedianValue);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMeanValue);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrSigmaValue);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMin);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMax);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMedian);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMean);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrSigma);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrRangeLow);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrRangeHigh);
            this.GroupBox_SnrWeight.Controls.Add(this.TextBox_SnrRangeHigh);
            this.GroupBox_SnrWeight.Controls.Add(this.TextBox_SnrRangeLow);
            this.GroupBox_SnrWeight.Location = new System.Drawing.Point(689, 137);
            this.GroupBox_SnrWeight.Name = "GroupBox_SnrWeight";
            this.GroupBox_SnrWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_SnrWeight.TabIndex = 16;
            this.GroupBox_SnrWeight.TabStop = false;
            this.GroupBox_SnrWeight.Text = "SNR Weight";
            // 
            // Label_SnrMinValue
            // 
            this.Label_SnrMinValue.AutoSize = true;
            this.Label_SnrMinValue.Location = new System.Drawing.Point(56, 49);
            this.Label_SnrMinValue.Name = "Label_SnrMinValue";
            this.Label_SnrMinValue.Size = new System.Drawing.Size(13, 13);
            this.Label_SnrMinValue.TabIndex = 38;
            this.Label_SnrMinValue.Text = "0";
            // 
            // Label_SnrMaxValue
            // 
            this.Label_SnrMaxValue.AutoSize = true;
            this.Label_SnrMaxValue.Location = new System.Drawing.Point(56, 66);
            this.Label_SnrMaxValue.Name = "Label_SnrMaxValue";
            this.Label_SnrMaxValue.Size = new System.Drawing.Size(13, 13);
            this.Label_SnrMaxValue.TabIndex = 37;
            this.Label_SnrMaxValue.Text = "0";
            // 
            // Label_SnrMedianValue
            // 
            this.Label_SnrMedianValue.AutoSize = true;
            this.Label_SnrMedianValue.Location = new System.Drawing.Point(56, 32);
            this.Label_SnrMedianValue.Name = "Label_SnrMedianValue";
            this.Label_SnrMedianValue.Size = new System.Drawing.Size(13, 13);
            this.Label_SnrMedianValue.TabIndex = 36;
            this.Label_SnrMedianValue.Text = "0";
            // 
            // Label_SnrMeanValue
            // 
            this.Label_SnrMeanValue.AutoSize = true;
            this.Label_SnrMeanValue.Location = new System.Drawing.Point(56, 15);
            this.Label_SnrMeanValue.Name = "Label_SnrMeanValue";
            this.Label_SnrMeanValue.Size = new System.Drawing.Size(13, 13);
            this.Label_SnrMeanValue.TabIndex = 34;
            this.Label_SnrMeanValue.Text = "0";
            // 
            // Label_SnrSigmaValue
            // 
            this.Label_SnrSigmaValue.AutoSize = true;
            this.Label_SnrSigmaValue.Location = new System.Drawing.Point(56, 83);
            this.Label_SnrSigmaValue.Name = "Label_SnrSigmaValue";
            this.Label_SnrSigmaValue.Size = new System.Drawing.Size(13, 13);
            this.Label_SnrSigmaValue.TabIndex = 35;
            this.Label_SnrSigmaValue.Text = "0";
            // 
            // Label_SnrMin
            // 
            this.Label_SnrMin.AutoSize = true;
            this.Label_SnrMin.Location = new System.Drawing.Point(12, 49);
            this.Label_SnrMin.Name = "Label_SnrMin";
            this.Label_SnrMin.Size = new System.Drawing.Size(30, 13);
            this.Label_SnrMin.TabIndex = 23;
            this.Label_SnrMin.Text = "Min: ";
            // 
            // Label_SnrMax
            // 
            this.Label_SnrMax.AutoSize = true;
            this.Label_SnrMax.Location = new System.Drawing.Point(12, 66);
            this.Label_SnrMax.Name = "Label_SnrMax";
            this.Label_SnrMax.Size = new System.Drawing.Size(33, 13);
            this.Label_SnrMax.TabIndex = 22;
            this.Label_SnrMax.Text = "Max: ";
            // 
            // Label_SnrMedian
            // 
            this.Label_SnrMedian.AutoSize = true;
            this.Label_SnrMedian.Location = new System.Drawing.Point(12, 32);
            this.Label_SnrMedian.Name = "Label_SnrMedian";
            this.Label_SnrMedian.Size = new System.Drawing.Size(45, 13);
            this.Label_SnrMedian.TabIndex = 21;
            this.Label_SnrMedian.Text = "Median:";
            // 
            // Label_SnrMean
            // 
            this.Label_SnrMean.AutoSize = true;
            this.Label_SnrMean.Location = new System.Drawing.Point(12, 15);
            this.Label_SnrMean.Name = "Label_SnrMean";
            this.Label_SnrMean.Size = new System.Drawing.Size(40, 13);
            this.Label_SnrMean.TabIndex = 19;
            this.Label_SnrMean.Text = "Mean: ";
            // 
            // Label_SnrSigma
            // 
            this.Label_SnrSigma.AutoSize = true;
            this.Label_SnrSigma.Location = new System.Drawing.Point(12, 83);
            this.Label_SnrSigma.Name = "Label_SnrSigma";
            this.Label_SnrSigma.Size = new System.Drawing.Size(42, 13);
            this.Label_SnrSigma.TabIndex = 20;
            this.Label_SnrSigma.Text = "Sigma: ";
            // 
            // Label_SnrRangeLow
            // 
            this.Label_SnrRangeLow.AutoSize = true;
            this.Label_SnrRangeLow.Location = new System.Drawing.Point(112, 66);
            this.Label_SnrRangeLow.Name = "Label_SnrRangeLow";
            this.Label_SnrRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_SnrRangeLow.TabIndex = 15;
            this.Label_SnrRangeLow.Text = "Low";
            this.Label_SnrRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_SnrRangeHigh
            // 
            this.Label_SnrRangeHigh.AutoSize = true;
            this.Label_SnrRangeHigh.Location = new System.Drawing.Point(110, 32);
            this.Label_SnrRangeHigh.Name = "Label_SnrRangeHigh";
            this.Label_SnrRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_SnrRangeHigh.TabIndex = 14;
            this.Label_SnrRangeHigh.Text = "High";
            this.Label_SnrRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_SnrRangeHigh
            // 
            this.TextBox_SnrRangeHigh.Location = new System.Drawing.Point(141, 28);
            this.TextBox_SnrRangeHigh.Name = "TextBox_SnrRangeHigh";
            this.TextBox_SnrRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_SnrRangeHigh.TabIndex = 2;
            this.TextBox_SnrRangeHigh.Text = "1.0";
            this.TextBox_SnrRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_SnrRangeHigh.TextChanged += new System.EventHandler(this.TextBox_SnrRangeHigh_TextChanged);
            // 
            // TextBox_SnrRangeLow
            // 
            this.TextBox_SnrRangeLow.Location = new System.Drawing.Point(141, 62);
            this.TextBox_SnrRangeLow.Name = "TextBox_SnrRangeLow";
            this.TextBox_SnrRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_SnrRangeLow.TabIndex = 3;
            this.TextBox_SnrRangeLow.Text = "0.0";
            this.TextBox_SnrRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_SnrRangeLow.TextChanged += new System.EventHandler(this.TextBox_SnrRangeLow_TextChanged);
            // 
            // GroupBox_InitialRejectionCriteria
            // 
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_Snr);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.label10);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_StarResidual);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.label15);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_Stars);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_AirMass);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.label20);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.label25);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_Noise);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_Noise);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Button_Rejection_RejectionSet);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_Median);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_Median);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_Eccentricity);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_FWHM);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.TextBox_Rejection_Total);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_Total);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_Eccentricity);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_FWHM);
            this.GroupBox_InitialRejectionCriteria.Location = new System.Drawing.Point(20, 109);
            this.GroupBox_InitialRejectionCriteria.Name = "GroupBox_InitialRejectionCriteria";
            this.GroupBox_InitialRejectionCriteria.Size = new System.Drawing.Size(908, 98);
            this.GroupBox_InitialRejectionCriteria.TabIndex = 26;
            this.GroupBox_InitialRejectionCriteria.TabStop = false;
            this.GroupBox_InitialRejectionCriteria.Text = "Initial Rejection Criteria";
            // 
            // NumericUpDown_Rejection_Snr
            // 
            this.NumericUpDown_Rejection_Snr.DecimalPlaces = 2;
            this.NumericUpDown_Rejection_Snr.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumericUpDown_Rejection_Snr.Location = new System.Drawing.Point(486, 61);
            this.NumericUpDown_Rejection_Snr.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Snr.Name = "NumericUpDown_Rejection_Snr";
            this.NumericUpDown_Rejection_Snr.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_Snr.TabIndex = 20;
            this.NumericUpDown_Rejection_Snr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_Snr.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Snr.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_Snr_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(451, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(33, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "SNR:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_StarResidual
            // 
            this.NumericUpDown_Rejection_StarResidual.DecimalPlaces = 3;
            this.NumericUpDown_Rejection_StarResidual.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.NumericUpDown_Rejection_StarResidual.Location = new System.Drawing.Point(373, 61);
            this.NumericUpDown_Rejection_StarResidual.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_StarResidual.Name = "NumericUpDown_Rejection_StarResidual";
            this.NumericUpDown_Rejection_StarResidual.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_StarResidual.TabIndex = 18;
            this.NumericUpDown_Rejection_StarResidual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_StarResidual.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_StarResidual.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_StarResidual_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(298, 65);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(73, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "Star Residual:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_Stars
            // 
            this.NumericUpDown_Rejection_Stars.Location = new System.Drawing.Point(223, 61);
            this.NumericUpDown_Rejection_Stars.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Stars.Name = "NumericUpDown_Rejection_Stars";
            this.NumericUpDown_Rejection_Stars.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_Stars.TabIndex = 16;
            this.NumericUpDown_Rejection_Stars.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_Stars.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_Stars_ValueChanged);
            // 
            // NumericUpDown_Rejection_AirMass
            // 
            this.NumericUpDown_Rejection_AirMass.DecimalPlaces = 2;
            this.NumericUpDown_Rejection_AirMass.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumericUpDown_Rejection_AirMass.Location = new System.Drawing.Point(85, 61);
            this.NumericUpDown_Rejection_AirMass.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_AirMass.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_AirMass.Name = "NumericUpDown_Rejection_AirMass";
            this.NumericUpDown_Rejection_AirMass.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_AirMass.TabIndex = 15;
            this.NumericUpDown_Rejection_AirMass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_AirMass.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_AirMass.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_AirMass_ValueChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(187, 65);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(34, 13);
            this.label20.TabIndex = 14;
            this.label20.Text = "Stars:";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(33, 65);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(50, 13);
            this.label25.TabIndex = 13;
            this.label25.Text = "Air Mass:";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_Noise
            // 
            this.NumericUpDown_Rejection_Noise.DecimalPlaces = 2;
            this.NumericUpDown_Rejection_Noise.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumericUpDown_Rejection_Noise.Location = new System.Drawing.Point(485, 28);
            this.NumericUpDown_Rejection_Noise.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Noise.Name = "NumericUpDown_Rejection_Noise";
            this.NumericUpDown_Rejection_Noise.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_Noise.TabIndex = 12;
            this.NumericUpDown_Rejection_Noise.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_Noise.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Noise.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_Noise_ValueChanged);
            // 
            // Label_Rejection_Noise
            // 
            this.Label_Rejection_Noise.AutoSize = true;
            this.Label_Rejection_Noise.Location = new System.Drawing.Point(446, 32);
            this.Label_Rejection_Noise.Name = "Label_Rejection_Noise";
            this.Label_Rejection_Noise.Size = new System.Drawing.Size(37, 13);
            this.Label_Rejection_Noise.TabIndex = 11;
            this.Label_Rejection_Noise.Text = "Noise:";
            this.Label_Rejection_Noise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Button_Rejection_RejectionSet
            // 
            this.Button_Rejection_RejectionSet.Location = new System.Drawing.Point(751, 43);
            this.Button_Rejection_RejectionSet.Name = "Button_Rejection_RejectionSet";
            this.Button_Rejection_RejectionSet.Size = new System.Drawing.Size(83, 23);
            this.Button_Rejection_RejectionSet.TabIndex = 10;
            this.Button_Rejection_RejectionSet.Text = "Set Rejected";
            this.Button_Rejection_RejectionSet.UseVisualStyleBackColor = true;
            this.Button_Rejection_RejectionSet.Click += new System.EventHandler(this.Button_Rejection_RejectionSet_Click);
            // 
            // NumericUpDown_Rejection_Median
            // 
            this.NumericUpDown_Rejection_Median.Location = new System.Drawing.Point(373, 28);
            this.NumericUpDown_Rejection_Median.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Median.Name = "NumericUpDown_Rejection_Median";
            this.NumericUpDown_Rejection_Median.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_Median.TabIndex = 9;
            this.NumericUpDown_Rejection_Median.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_Median.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Median.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_Median_ValueChanged);
            // 
            // Label_Rejection_Median
            // 
            this.Label_Rejection_Median.AutoSize = true;
            this.Label_Rejection_Median.Location = new System.Drawing.Point(326, 32);
            this.Label_Rejection_Median.Name = "Label_Rejection_Median";
            this.Label_Rejection_Median.Size = new System.Drawing.Size(45, 13);
            this.Label_Rejection_Median.TabIndex = 8;
            this.Label_Rejection_Median.Text = "Median:";
            this.Label_Rejection_Median.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumericUpDown_Rejection_Eccentricity
            // 
            this.NumericUpDown_Rejection_Eccentricity.DecimalPlaces = 2;
            this.NumericUpDown_Rejection_Eccentricity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumericUpDown_Rejection_Eccentricity.Location = new System.Drawing.Point(223, 28);
            this.NumericUpDown_Rejection_Eccentricity.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Eccentricity.Name = "NumericUpDown_Rejection_Eccentricity";
            this.NumericUpDown_Rejection_Eccentricity.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_Eccentricity.TabIndex = 7;
            this.NumericUpDown_Rejection_Eccentricity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_Eccentricity.Value = new decimal(new int[] {
            65,
            0,
            0,
            131072});
            this.NumericUpDown_Rejection_Eccentricity.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_Eccentricity_ValueChanged);
            // 
            // NumericUpDown_Rejection_FWHM
            // 
            this.NumericUpDown_Rejection_FWHM.DecimalPlaces = 2;
            this.NumericUpDown_Rejection_FWHM.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumericUpDown_Rejection_FWHM.Location = new System.Drawing.Point(85, 28);
            this.NumericUpDown_Rejection_FWHM.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_FWHM.Name = "NumericUpDown_Rejection_FWHM";
            this.NumericUpDown_Rejection_FWHM.Size = new System.Drawing.Size(59, 20);
            this.NumericUpDown_Rejection_FWHM.TabIndex = 6;
            this.NumericUpDown_Rejection_FWHM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_FWHM.Value = new decimal(new int[] {
            45,
            0,
            0,
            65536});
            this.NumericUpDown_Rejection_FWHM.ValueChanged += new System.EventHandler(this.NumericUpDown_Rejection_FWHM_ValueChanged);
            // 
            // TextBox_Rejection_Total
            // 
            this.TextBox_Rejection_Total.Location = new System.Drawing.Point(700, 44);
            this.TextBox_Rejection_Total.Name = "TextBox_Rejection_Total";
            this.TextBox_Rejection_Total.Size = new System.Drawing.Size(44, 20);
            this.TextBox_Rejection_Total.TabIndex = 5;
            this.TextBox_Rejection_Total.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label_Rejection_Total
            // 
            this.Label_Rejection_Total.AutoSize = true;
            this.Label_Rejection_Total.Location = new System.Drawing.Point(590, 48);
            this.Label_Rejection_Total.Name = "Label_Rejection_Total";
            this.Label_Rejection_Total.Size = new System.Drawing.Size(109, 13);
            this.Label_Rejection_Total.TabIndex = 4;
            this.Label_Rejection_Total.Text = "Rejected SubFrames:";
            this.Label_Rejection_Total.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Rejection_Eccentricity
            // 
            this.Label_Rejection_Eccentricity.AutoSize = true;
            this.Label_Rejection_Eccentricity.Location = new System.Drawing.Point(156, 32);
            this.Label_Rejection_Eccentricity.Name = "Label_Rejection_Eccentricity";
            this.Label_Rejection_Eccentricity.Size = new System.Drawing.Size(65, 13);
            this.Label_Rejection_Eccentricity.TabIndex = 3;
            this.Label_Rejection_Eccentricity.Text = "Eccentricity:";
            this.Label_Rejection_Eccentricity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Rejection_FWHM
            // 
            this.Label_Rejection_FWHM.AutoSize = true;
            this.Label_Rejection_FWHM.Location = new System.Drawing.Point(39, 32);
            this.Label_Rejection_FWHM.Name = "Label_Rejection_FWHM";
            this.Label_Rejection_FWHM.Size = new System.Drawing.Size(44, 13);
            this.Label_Rejection_FWHM.TabIndex = 2;
            this.Label_Rejection_FWHM.Text = "FWHM:";
            this.Label_Rejection_FWHM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox_UpdateStatistics
            // 
            this.GroupBox_UpdateStatistics.Controls.Add(this.RadioButton_SetImageStatistics_CalculateWeights);
            this.GroupBox_UpdateStatistics.Controls.Add(this.RadioButton_SetImageStatistics_RescaleWeights);
            this.GroupBox_UpdateStatistics.Controls.Add(this.RadioButton_SetImageStatistics_KeepWeights);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Button_ReadSubFrameSelectorCsvFile);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Label_UpdateStatistics);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Label_UpdateStatisticsRangeHigh);
            this.GroupBox_UpdateStatistics.Controls.Add(this.TextBox_UpdateStatisticsRangeHigh);
            this.GroupBox_UpdateStatistics.Controls.Add(this.TextBox_UpdateStatisticsRangeLow);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Label_UpdateStatisticsRangeLow);
            this.GroupBox_UpdateStatistics.Location = new System.Drawing.Point(17, 20);
            this.GroupBox_UpdateStatistics.Name = "GroupBox_UpdateStatistics";
            this.GroupBox_UpdateStatistics.Size = new System.Drawing.Size(911, 73);
            this.GroupBox_UpdateStatistics.TabIndex = 20;
            this.GroupBox_UpdateStatistics.TabStop = false;
            this.GroupBox_UpdateStatistics.Text = "Set Image Statistics";
            // 
            // RadioButton_SetImageStatistics_CalculateWeights
            // 
            this.RadioButton_SetImageStatistics_CalculateWeights.AutoSize = true;
            this.RadioButton_SetImageStatistics_CalculateWeights.Location = new System.Drawing.Point(173, 47);
            this.RadioButton_SetImageStatistics_CalculateWeights.Name = "RadioButton_SetImageStatistics_CalculateWeights";
            this.RadioButton_SetImageStatistics_CalculateWeights.Size = new System.Drawing.Size(117, 17);
            this.RadioButton_SetImageStatistics_CalculateWeights.TabIndex = 24;
            this.RadioButton_SetImageStatistics_CalculateWeights.Text = "Calculated Weights";
            this.RadioButton_SetImageStatistics_CalculateWeights.UseVisualStyleBackColor = true;
            this.RadioButton_SetImageStatistics_CalculateWeights.CheckedChanged += new System.EventHandler(this.RadioButton_SetImageStatistics_CalculateWeights_CheckedChanged);
            // 
            // RadioButton_SetImageStatistics_RescaleWeights
            // 
            this.RadioButton_SetImageStatistics_RescaleWeights.AutoSize = true;
            this.RadioButton_SetImageStatistics_RescaleWeights.Location = new System.Drawing.Point(173, 30);
            this.RadioButton_SetImageStatistics_RescaleWeights.Name = "RadioButton_SetImageStatistics_RescaleWeights";
            this.RadioButton_SetImageStatistics_RescaleWeights.Size = new System.Drawing.Size(106, 17);
            this.RadioButton_SetImageStatistics_RescaleWeights.TabIndex = 22;
            this.RadioButton_SetImageStatistics_RescaleWeights.Text = "Rescale Weights";
            this.RadioButton_SetImageStatistics_RescaleWeights.UseVisualStyleBackColor = true;
            this.RadioButton_SetImageStatistics_RescaleWeights.CheckedChanged += new System.EventHandler(this.RadioButton_SetImageStatistics_RescaleWeights_CheckedChanged);
            // 
            // RadioButton_SetImageStatistics_KeepWeights
            // 
            this.RadioButton_SetImageStatistics_KeepWeights.AutoSize = true;
            this.RadioButton_SetImageStatistics_KeepWeights.Checked = true;
            this.RadioButton_SetImageStatistics_KeepWeights.Location = new System.Drawing.Point(173, 13);
            this.RadioButton_SetImageStatistics_KeepWeights.Name = "RadioButton_SetImageStatistics_KeepWeights";
            this.RadioButton_SetImageStatistics_KeepWeights.Size = new System.Drawing.Size(131, 17);
            this.RadioButton_SetImageStatistics_KeepWeights.TabIndex = 21;
            this.RadioButton_SetImageStatistics_KeepWeights.TabStop = true;
            this.RadioButton_SetImageStatistics_KeepWeights.Text = "Keep Existing Weights";
            this.RadioButton_SetImageStatistics_KeepWeights.UseVisualStyleBackColor = true;
            this.RadioButton_SetImageStatistics_KeepWeights.CheckedChanged += new System.EventHandler(this.RadioButton_SetImageStatistics_KeepWeights_CheckedChanged);
            // 
            // Button_ReadSubFrameSelectorCsvFile
            // 
            this.Button_ReadSubFrameSelectorCsvFile.Location = new System.Drawing.Point(19, 21);
            this.Button_ReadSubFrameSelectorCsvFile.Name = "Button_ReadSubFrameSelectorCsvFile";
            this.Button_ReadSubFrameSelectorCsvFile.Size = new System.Drawing.Size(132, 34);
            this.Button_ReadSubFrameSelectorCsvFile.TabIndex = 0;
            this.Button_ReadSubFrameSelectorCsvFile.Text = "Read SubFrame Selector CSV File";
            this.Button_ReadSubFrameSelectorCsvFile.UseVisualStyleBackColor = true;
            this.Button_ReadSubFrameSelectorCsvFile.Click += new System.EventHandler(this.Button_ReadCSV_Click);
            // 
            // Label_UpdateStatistics
            // 
            this.Label_UpdateStatistics.AutoSize = true;
            this.Label_UpdateStatistics.Location = new System.Drawing.Point(463, 32);
            this.Label_UpdateStatistics.Name = "Label_UpdateStatistics";
            this.Label_UpdateStatistics.Size = new System.Drawing.Size(151, 13);
            this.Label_UpdateStatistics.TabIndex = 20;
            this.Label_UpdateStatistics.Text = "Rescale SSWEIGHT - Range:";
            // 
            // Label_UpdateStatisticsRangeHigh
            // 
            this.Label_UpdateStatisticsRangeHigh.AutoSize = true;
            this.Label_UpdateStatisticsRangeHigh.Location = new System.Drawing.Point(615, 32);
            this.Label_UpdateStatisticsRangeHigh.Name = "Label_UpdateStatisticsRangeHigh";
            this.Label_UpdateStatisticsRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_UpdateStatisticsRangeHigh.TabIndex = 18;
            this.Label_UpdateStatisticsRangeHigh.Text = "High";
            this.Label_UpdateStatisticsRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_UpdateStatisticsRangeHigh
            // 
            this.TextBox_UpdateStatisticsRangeHigh.Location = new System.Drawing.Point(646, 28);
            this.TextBox_UpdateStatisticsRangeHigh.Name = "TextBox_UpdateStatisticsRangeHigh";
            this.TextBox_UpdateStatisticsRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_UpdateStatisticsRangeHigh.TabIndex = 16;
            this.TextBox_UpdateStatisticsRangeHigh.Text = "100";
            this.TextBox_UpdateStatisticsRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_UpdateStatisticsRangeHigh.TextChanged += new System.EventHandler(this.TextBox_UpdateStatisticsRangeHigh_TextChanged);
            // 
            // TextBox_UpdateStatisticsRangeLow
            // 
            this.TextBox_UpdateStatisticsRangeLow.Location = new System.Drawing.Point(731, 28);
            this.TextBox_UpdateStatisticsRangeLow.Name = "TextBox_UpdateStatisticsRangeLow";
            this.TextBox_UpdateStatisticsRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_UpdateStatisticsRangeLow.TabIndex = 17;
            this.TextBox_UpdateStatisticsRangeLow.Text = "50";
            this.TextBox_UpdateStatisticsRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_UpdateStatisticsRangeLow.TextChanged += new System.EventHandler(this.TextBox_UpdateStatisticsRangeLow_TextChanged);
            // 
            // Label_UpdateStatisticsRangeLow
            // 
            this.Label_UpdateStatisticsRangeLow.AutoSize = true;
            this.Label_UpdateStatisticsRangeLow.Location = new System.Drawing.Point(702, 32);
            this.Label_UpdateStatisticsRangeLow.Name = "Label_UpdateStatisticsRangeLow";
            this.Label_UpdateStatisticsRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_UpdateStatisticsRangeLow.TabIndex = 19;
            this.Label_UpdateStatisticsRangeLow.Text = "Low";
            this.Label_UpdateStatisticsRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Task
            // 
            this.Label_Task.AutoSize = true;
            this.Label_Task.Location = new System.Drawing.Point(10, 28);
            this.Label_Task.Name = "Label_Task";
            this.Label_Task.Size = new System.Drawing.Size(86, 13);
            this.Label_Task.TabIndex = 12;
            this.Label_Task.Text = "Operation Status";
            // 
            // Label_TempratureCompensation
            // 
            this.Label_TempratureCompensation.AutoSize = true;
            this.Label_TempratureCompensation.Location = new System.Drawing.Point(10, 46);
            this.Label_TempratureCompensation.Name = "Label_TempratureCompensation";
            this.Label_TempratureCompensation.Size = new System.Drawing.Size(161, 13);
            this.Label_TempratureCompensation.TabIndex = 13;
            this.Label_TempratureCompensation.Text = "Temp Coefficient: Not Computed";
            // 
            // Label_File_Selection_SubFrameOverhead
            // 
            this.Label_File_Selection_SubFrameOverhead.AutoSize = true;
            this.Label_File_Selection_SubFrameOverhead.Location = new System.Drawing.Point(10, 64);
            this.Label_File_Selection_SubFrameOverhead.Name = "Label_File_Selection_SubFrameOverhead";
            this.Label_File_Selection_SubFrameOverhead.Size = new System.Drawing.Size(179, 13);
            this.Label_File_Selection_SubFrameOverhead.TabIndex = 14;
            this.Label_File_Selection_SubFrameOverhead.Text = "SubFrame Overhead: Not Computed";
            // 
            // GroupBox_FileSlection
            // 
            this.GroupBox_FileSlection.Controls.Add(this.Label_BrowseFileName);
            this.GroupBox_FileSlection.Controls.Add(this.groupBox2);
            this.GroupBox_FileSlection.Controls.Add(this.GroupBox_DirectorySelection);
            this.GroupBox_FileSlection.Controls.Add(this.GroupBox_RenameOrder);
            this.GroupBox_FileSlection.Controls.Add(this.ProgressBar_OverAll);
            this.GroupBox_FileSlection.Location = new System.Drawing.Point(12, 24);
            this.GroupBox_FileSlection.Name = "GroupBox_FileSlection";
            this.GroupBox_FileSlection.Size = new System.Drawing.Size(950, 187);
            this.GroupBox_FileSlection.TabIndex = 19;
            this.GroupBox_FileSlection.TabStop = false;
            this.GroupBox_FileSlection.Text = "File Selection";
            // 
            // Label_BrowseFileName
            // 
            this.Label_BrowseFileName.AutoSize = true;
            this.Label_BrowseFileName.Location = new System.Drawing.Point(17, 134);
            this.Label_BrowseFileName.Name = "Label_BrowseFileName";
            this.Label_BrowseFileName.Size = new System.Drawing.Size(92, 13);
            this.Label_BrowseFileName.TabIndex = 21;
            this.Label_BrowseFileName.Text = "Browse File Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Label_Task);
            this.groupBox2.Controls.Add(this.Label_File_Selection_SubFrameOverhead);
            this.groupBox2.Controls.Add(this.Label_TempratureCompensation);
            this.groupBox2.Location = new System.Drawing.Point(273, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(398, 105);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistics";
            // 
            // RadioButton_Filter_Shutter
            // 
            this.RadioButton_Filter_Shutter.AutoSize = true;
            this.RadioButton_Filter_Shutter.Location = new System.Drawing.Point(193, 48);
            this.RadioButton_Filter_Shutter.Name = "RadioButton_Filter_Shutter";
            this.RadioButton_Filter_Shutter.Size = new System.Drawing.Size(59, 17);
            this.RadioButton_Filter_Shutter.TabIndex = 8;
            this.RadioButton_Filter_Shutter.TabStop = true;
            this.RadioButton_Filter_Shutter.Text = "Shutter";
            this.RadioButton_Filter_Shutter.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(973, 919);
            this.Controls.Add(this.GroupBox_XisfFileUpdate);
            this.Controls.Add(this.GroupBox_FileSlection);
            this.Controls.Add(this.GroupBox_WeightsAndStatistics);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XSIF File Manager";
            this.GroupBox_RenameOrder.ResumeLayout(false);
            this.GroupBox_RenameOrder.PerformLayout();
            this.GroupBox_XisfFileUpdate.ResumeLayout(false);
            this.GroupBox_XisfFileUpdate.PerformLayout();
            this.GroupBox_Filter.ResumeLayout(false);
            this.GroupBox_Filter.PerformLayout();
            this.GroupBox_SubFrameKeywords.ResumeLayout(false);
            this.GroupBox_SubFrameKeywords.PerformLayout();
            this.GroupBox_DirectorySelection.ResumeLayout(false);
            this.GroupBox_DirectorySelection.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.GroupBox_WeightsAndStatistics.ResumeLayout(false);
            this.GroupBox_WeightCalculations.ResumeLayout(false);
            this.GroupBox_StarResidual.ResumeLayout(false);
            this.GroupBox_StarResidual.PerformLayout();
            this.GroupBox_FwhmWeight.ResumeLayout(false);
            this.GroupBox_FwhmWeight.PerformLayout();
            this.GroupBox_StarsWeight.ResumeLayout(false);
            this.GroupBox_StarsWeight.PerformLayout();
            this.GroupBox_EccentricityWeight.ResumeLayout(false);
            this.GroupBox_EccentricityWeight.PerformLayout();
            this.GroupBox_AirMassWeight.ResumeLayout(false);
            this.GroupBox_AirMassWeight.PerformLayout();
            this.GroupBox_NoiseWeight.ResumeLayout(false);
            this.GroupBox_NoiseWeight.PerformLayout();
            this.GroupBox_MedianWeight.ResumeLayout(false);
            this.GroupBox_MedianWeight.PerformLayout();
            this.GroupBox_SnrWeight.ResumeLayout(false);
            this.GroupBox_SnrWeight.PerformLayout();
            this.GroupBox_InitialRejectionCriteria.ResumeLayout(false);
            this.GroupBox_InitialRejectionCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Snr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_StarResidual)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Stars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_AirMass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Noise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Median)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Eccentricity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_FWHM)).EndInit();
            this.GroupBox_UpdateStatistics.ResumeLayout(false);
            this.GroupBox_UpdateStatistics.PerformLayout();
            this.GroupBox_FileSlection.ResumeLayout(false);
            this.GroupBox_FileSlection.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Browse;
        private System.Windows.Forms.ProgressBar ProgressBar_OverAll;
        private System.Windows.Forms.CheckBox CheckBox_Recurse;
        private System.Windows.Forms.GroupBox GroupBox_RenameOrder;
        private System.Windows.Forms.Button Button_Rename;
        private System.Windows.Forms.GroupBox GroupBox_XisfFileUpdate;
        private System.Windows.Forms.GroupBox GroupBox_DirectorySelection;
        private System.Windows.Forms.Label Label_TagetName;
        private System.Windows.Forms.Button Button_UpdateXisfFiles;
        private System.Windows.Forms.ComboBox ComboBox_TargetName;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTemplateToolStripMenuItem;
        private System.Windows.Forms.GroupBox GroupBox_WeightsAndStatistics;
        private System.Windows.Forms.Button Button_ReadSubFrameSelectorCsvFile;
        private System.Windows.Forms.Label Label_FwhmSigma;
        private System.Windows.Forms.Label Label_FwhmMean;
        private System.Windows.Forms.TextBox TextBox_FwhmRangeLow;
        private System.Windows.Forms.TextBox TextBox_FwhmRangeHigh;
        private System.Windows.Forms.GroupBox GroupBox_UpdateStatistics;
        private System.Windows.Forms.Label Label_UpdateStatistics;
        private System.Windows.Forms.TextBox TextBox_UpdateStatisticsRangeHigh;
        private System.Windows.Forms.Label Label_UpdateStatisticsRangeLow;
        private System.Windows.Forms.TextBox TextBox_UpdateStatisticsRangeLow;
        private System.Windows.Forms.Label Label_UpdateStatisticsRangeHigh;
        private System.Windows.Forms.GroupBox GroupBox_NoiseWeight;
        private System.Windows.Forms.Label Label_NoiseRangeLow;
        private System.Windows.Forms.Label Label_NoiseRangeHigh;
        private System.Windows.Forms.TextBox TextBox_NoiseRangeHigh;
        private System.Windows.Forms.TextBox TextBox_NoiseRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_EccentricityWeight;
        private System.Windows.Forms.Label Label_EccentricityRangeLow;
        private System.Windows.Forms.Label Label_EccentricityRangeHigh;
        private System.Windows.Forms.TextBox TextBox_EccentricityRangeHigh;
        private System.Windows.Forms.TextBox TextBox_EccentricityRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_MedianWeight;
        private System.Windows.Forms.Label Label_MedianRangeLow;
        private System.Windows.Forms.Label Label_MedianRangeHigh;
        private System.Windows.Forms.TextBox TextBox_MedianRangeHigh;
        private System.Windows.Forms.TextBox TextBox_MedianRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_SnrWeight;
        private System.Windows.Forms.Label Label_SnrRangeLow;
        private System.Windows.Forms.Label Label_SnrRangeHigh;
        private System.Windows.Forms.TextBox TextBox_SnrRangeHigh;
        private System.Windows.Forms.TextBox TextBox_SnrRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_FwhmWeight;
        private System.Windows.Forms.Label Label_FwhmRangeLow;
        private System.Windows.Forms.Label Label_FwhmRangeHigh;
        private System.Windows.Forms.Label Label_Task;
        private System.Windows.Forms.GroupBox GroupBox_StarsWeight;
        private System.Windows.Forms.Label Label_StarRangeLow;
        private System.Windows.Forms.Label Label_StarRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_AirMassWeight;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationRangeLow;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationRangeHigh;
        private System.Windows.Forms.ProgressBar ProgressBar_XisfFile;
        private System.Windows.Forms.GroupBox GroupBox_InitialRejectionCriteria;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_Weight;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_Index;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_IndexWeight;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_WeightIndex;
        private System.Windows.Forms.Label Label_TempratureCompensation;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Eccentricity;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_FWHM;
        private System.Windows.Forms.TextBox TextBox_Rejection_Total;
        private System.Windows.Forms.Label Label_Rejection_Total;
        private System.Windows.Forms.Label Label_Rejection_Eccentricity;
        private System.Windows.Forms.Label Label_Rejection_FWHM;
        private System.Windows.Forms.GroupBox GroupBox_SubFrameKeywords;
        private System.Windows.Forms.RadioButton RadioButton_SubFrameKeyWords_SubFrameWeightCalculations;
        private System.Windows.Forms.RadioButton RadioButton_SubFrameKeywords_Alphabetize;
        private System.Windows.Forms.Label Label_File_Selection_SubFrameOverhead;
        private System.Windows.Forms.CheckBox CheckBox_SubFrameKeywords_UpdateTargetName;
        private System.Windows.Forms.Button Button_Rejection_RejectionSet;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Median;
        private System.Windows.Forms.Label Label_Rejection_Median;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_CalculateWeights;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_RescaleWeights;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_KeepWeights;
        private System.Windows.Forms.GroupBox GroupBox_WeightCalculations;
        private System.Windows.Forms.GroupBox GroupBox_FileSlection;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox GroupBox_Filter;
        private System.Windows.Forms.CheckBox CheckBox_Filter_SetFilter;
        private System.Windows.Forms.RadioButton RadioButton_Filter_S2;
        private System.Windows.Forms.RadioButton RadioButton_Filter_Blue;
        private System.Windows.Forms.RadioButton RadioButton_Filter_O3;
        private System.Windows.Forms.RadioButton RadioButton_Filter_Green;
        private System.Windows.Forms.RadioButton RadioButton_Filter_Ha;
        private System.Windows.Forms.RadioButton RadioButton_Filter_Red;
        private System.Windows.Forms.RadioButton RadioButton_Filter_Luma;
        private System.Windows.Forms.TextBox TextBox_SetKeyword;
        private System.Windows.Forms.ComboBox ComboBox_SetKeyword;
        private System.Windows.Forms.Label Label_UpdateFileName;
        private System.Windows.Forms.Label Label_BrowseFileName;
        private System.Windows.Forms.Label Label_FwhmMin;
        private System.Windows.Forms.Label Label_FwhmMax;
        private System.Windows.Forms.Label Label_FwhmMedian;
        private System.Windows.Forms.Label Label_StarsMin;
        private System.Windows.Forms.Label Label_StarsMax;
        private System.Windows.Forms.Label Label_StarsMedian;
        private System.Windows.Forms.Label Label_StarsMean;
        private System.Windows.Forms.Label Label_StarsSigma;
        private System.Windows.Forms.Label Label_EccentricityMin;
        private System.Windows.Forms.Label Label_EccentricityMax;
        private System.Windows.Forms.Label Label_EccentricityMedian;
        private System.Windows.Forms.Label Label_EccentricityMean;
        private System.Windows.Forms.Label Label_EccentricitySigma;
        private System.Windows.Forms.Label Label_AirMassMin;
        private System.Windows.Forms.Label Label_AirMassMax;
        private System.Windows.Forms.Label Label_AirMassMedian;
        private System.Windows.Forms.Label Label_AirMassMean;
        private System.Windows.Forms.Label Label_AirMassSigma;
        private System.Windows.Forms.Label Label_NoiseMin;
        private System.Windows.Forms.Label Label_NoiseMax;
        private System.Windows.Forms.Label Label_NoiseMedian;
        private System.Windows.Forms.Label Label_NoiseMean;
        private System.Windows.Forms.Label Label_NoiseSigma;
        private System.Windows.Forms.Label Label_MedianMin;
        private System.Windows.Forms.Label Label_MedianMax;
        private System.Windows.Forms.Label Label_MedianMedian;
        private System.Windows.Forms.Label Label_MedianMean;
        private System.Windows.Forms.Label Label_MedianSigma;
        private System.Windows.Forms.Label Label_SnrMin;
        private System.Windows.Forms.Label Label_SnrMax;
        private System.Windows.Forms.Label Label_SnrMedian;
        private System.Windows.Forms.Label Label_SnrMean;
        private System.Windows.Forms.Label Label_SnrSigma;
        private System.Windows.Forms.GroupBox GroupBox_StarResidual;
        private System.Windows.Forms.Label Label_StarResidualMin;
        private System.Windows.Forms.Label Label_StarResidualMax;
        private System.Windows.Forms.Label Label_StarResidualMedian;
        private System.Windows.Forms.Label Label_StarResidualMean;
        private System.Windows.Forms.Label Label_StarResidualSigma;
        private System.Windows.Forms.Label Label_StarResidualRangeLow;
        private System.Windows.Forms.Label Label_StarResidualRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarResidualRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarResidualRangeLow;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Snr;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_StarResidual;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Stars;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_AirMass;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Noise;
        private System.Windows.Forms.Label Label_Rejection_Noise;
        private System.Windows.Forms.TextBox TextBox_AirMassRangeLow;
        private System.Windows.Forms.TextBox TextBox_AirMassRangeHigh;
        private System.Windows.Forms.Label Label_FwhmMinValue;
        private System.Windows.Forms.Label Label_FwhmMaxValue;
        private System.Windows.Forms.Label Label_FwhmMedianValue;
        private System.Windows.Forms.Label Label_FwhmMeanValue;
        private System.Windows.Forms.Label Label_FwhmSigmaValue;
        private System.Windows.Forms.Label Label_StarsMinValue;
        private System.Windows.Forms.Label Label_StarsMaxValue;
        private System.Windows.Forms.Label Label_StarsMedianValue;
        private System.Windows.Forms.Label Label_StarsMeanValue;
        private System.Windows.Forms.Label Label_StarsSigmaValue;
        private System.Windows.Forms.Label Label_EccentricityMinValue;
        private System.Windows.Forms.Label Label_EccentricityMaxValue;
        private System.Windows.Forms.Label Label_EccentricityMedianValue;
        private System.Windows.Forms.Label Label_EccentricityMeanValue;
        private System.Windows.Forms.Label Label_EccentricitySigmaValue;
        private System.Windows.Forms.Label Label_AirMassMinValue;
        private System.Windows.Forms.Label Label_AirMassMaxValue;
        private System.Windows.Forms.Label Label_AirMassMedianValue;
        private System.Windows.Forms.Label Label_AirMassMeanValue;
        private System.Windows.Forms.Label Label_AirMassSigmaValue;
        private System.Windows.Forms.Label Label_StarResidualMinValue;
        private System.Windows.Forms.Label Label_StarResidualMaxValue;
        private System.Windows.Forms.Label Label_StarResidualMedianValue;
        private System.Windows.Forms.Label Label_StarResidualMeanValue;
        private System.Windows.Forms.Label Label_StarResidualSigmaValue;
        private System.Windows.Forms.Label Label_NoiseMinValue;
        private System.Windows.Forms.Label Label_NoiseMaxValue;
        private System.Windows.Forms.Label Label_NoiseMedianValue;
        private System.Windows.Forms.Label Label_NoiseMeanValue;
        private System.Windows.Forms.Label Label_NoiseSigmaValue;
        private System.Windows.Forms.Label Label_MedianMinValue;
        private System.Windows.Forms.Label Label_MedianMaxValue;
        private System.Windows.Forms.Label Label_MedianMedianValue;
        private System.Windows.Forms.Label Label_MedianMeanValue;
        private System.Windows.Forms.Label Label_MedianSigmaValue;
        private System.Windows.Forms.Label Label_SnrMinValue;
        private System.Windows.Forms.Label Label_SnrMaxValue;
        private System.Windows.Forms.Label Label_SnrMedianValue;
        private System.Windows.Forms.Label Label_SnrMeanValue;
        private System.Windows.Forms.Label Label_SnrSigmaValue;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton RadioButton_Filter_Shutter;
    }
}

