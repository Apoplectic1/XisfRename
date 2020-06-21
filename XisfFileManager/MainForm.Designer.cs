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
            this.GroupBox_Order = new System.Windows.Forms.GroupBox();
            this.RadioButton_RenameOrder_Weight = new System.Windows.Forms.RadioButton();
            this.RadioButton_RenameOrder_Index = new System.Windows.Forms.RadioButton();
            this.RadioButton_RenameOrder_IndexWeight = new System.Windows.Forms.RadioButton();
            this.RadioButton_RenameOrder_WeightIndex = new System.Windows.Forms.RadioButton();
            this.Button_Rename = new System.Windows.Forms.Button();
            this.GroupBox_XisfFileUpdate = new System.Windows.Forms.GroupBox();
            this.GroupBox_SubFrameKeywords = new System.Windows.Forms.GroupBox();
            this.CheckBox_SubFrameKeywords_UpdateTargetName = new System.Windows.Forms.CheckBox();
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData = new System.Windows.Forms.RadioButton();
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
            this.GroupBox_InitialRejectionCriteria = new System.Windows.Forms.GroupBox();
            this.Button_Regection_MoveRejected = new System.Windows.Forms.Button();
            this.NumericUpDown_Rejection_Median = new System.Windows.Forms.NumericUpDown();
            this.Label_Rejection_Median = new System.Windows.Forms.Label();
            this.NumericUpDown_Rejection_Eccentricity = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Rejection_FWHM = new System.Windows.Forms.NumericUpDown();
            this.TextBox_Rejection_Total = new System.Windows.Forms.TextBox();
            this.Label_Rejection_Total = new System.Windows.Forms.Label();
            this.Label_Rejection_Eccentricity = new System.Windows.Forms.Label();
            this.Label_Rejection_FWHM = new System.Windows.Forms.Label();
            this.GroupBox_StarResidual = new System.Windows.Forms.GroupBox();
            this.Label_StarResidualRangeLow = new System.Windows.Forms.Label();
            this.Label_StarResidualRangeHigh = new System.Windows.Forms.Label();
            this.Label_StarResidualMean = new System.Windows.Forms.Label();
            this.Label_StarResidualPercent = new System.Windows.Forms.Label();
            this.Label_StarResidualStdDev = new System.Windows.Forms.Label();
            this.TextBox_StarResidualPercent = new System.Windows.Forms.TextBox();
            this.TextBox_StarResidualRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_StarResidualRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_UpdateStatistics = new System.Windows.Forms.GroupBox();
            this.RadioButton_SetImageStatistics_CalculateWeights = new System.Windows.Forms.RadioButton();
            this.RadioButton_SetImageStatistics_RescaleWeights = new System.Windows.Forms.RadioButton();
            this.RadioButton_SetImageStatistics_KeepWeights = new System.Windows.Forms.RadioButton();
            this.Button_ReadSubFrameSelectorCsvFile = new System.Windows.Forms.Button();
            this.Label_UpdateStatistics = new System.Windows.Forms.Label();
            this.TextBox_UpdateStatisticsRangeHigh = new System.Windows.Forms.TextBox();
            this.Label_UpdateStatisticsRangeLow = new System.Windows.Forms.Label();
            this.TextBox_UpdateStatisticsRangeLow = new System.Windows.Forms.TextBox();
            this.Label_UpdateStatisticsRangeHigh = new System.Windows.Forms.Label();
            this.GroupBox_StarResidualMeanDeviation = new System.Windows.Forms.GroupBox();
            this.Label_StarResidualMeanDevationRangeLow = new System.Windows.Forms.Label();
            this.Label_StarResidualMeanDevationMean = new System.Windows.Forms.Label();
            this.Label_StarResidualMeanDevationRangeHigh = new System.Windows.Forms.Label();
            this.Label_StarResidualMeanDevationPercent = new System.Windows.Forms.Label();
            this.TextBox_StarResidualMeanDeviationPercent = new System.Windows.Forms.TextBox();
            this.TextBox_StarResidualMeanDevationRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_StarResidualMeanDevationRangeLow = new System.Windows.Forms.TextBox();
            this.Label_StarResidualMeanDevationStdDev = new System.Windows.Forms.Label();
            this.GroupBox_MedianMeanDeviationWeight = new System.Windows.Forms.GroupBox();
            this.Label_MedianMeanDeviationRangeLow = new System.Windows.Forms.Label();
            this.Label_MedianMeanDeviationRangeHigh = new System.Windows.Forms.Label();
            this.Label_MedianMeanDeviationMean = new System.Windows.Forms.Label();
            this.Label_MedianMeanDeviationPercent = new System.Windows.Forms.Label();
            this.Label_MedianMeanDeviationStdDev = new System.Windows.Forms.Label();
            this.TextBox_MedianMeanDeviationPercent = new System.Windows.Forms.TextBox();
            this.TextBox_MedianMeanDeviationRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_MedianMeanDeviationRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_EccentricityMeanDeviationWeight = new System.Windows.Forms.GroupBox();
            this.Label_EccentricityMeanDeviationRangeLow = new System.Windows.Forms.Label();
            this.Label_EccentricityMeanDeviationRangeHigh = new System.Windows.Forms.Label();
            this.Label_EccentricityMeanDeviationMean = new System.Windows.Forms.Label();
            this.Label_EccentricityMeanDeviationPercent = new System.Windows.Forms.Label();
            this.Label_EccentricityMeanDeviationStdDev = new System.Windows.Forms.Label();
            this.TextBox_EccentricityMeanDeviationPercent = new System.Windows.Forms.TextBox();
            this.TextBox_EccentricityMeanDeviationRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_EccentricityMeanDeviationRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_StarsWeight = new System.Windows.Forms.GroupBox();
            this.Label_StarRangeLow = new System.Windows.Forms.Label();
            this.Label_StarsMean = new System.Windows.Forms.Label();
            this.Label_StarRangeHigh = new System.Windows.Forms.Label();
            this.Label_StarPercent = new System.Windows.Forms.Label();
            this.TextBox_StarsPercent = new System.Windows.Forms.TextBox();
            this.TextBox_StarRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_StarRangeLow = new System.Windows.Forms.TextBox();
            this.Label_StarStdDev = new System.Windows.Forms.Label();
            this.GroupBox_SnrWeight = new System.Windows.Forms.GroupBox();
            this.Label_SnrRangeLow = new System.Windows.Forms.Label();
            this.Label_SnrMean = new System.Windows.Forms.Label();
            this.Label_SnrRangeHigh = new System.Windows.Forms.Label();
            this.Label_SnrPersent = new System.Windows.Forms.Label();
            this.TextBox_SnrPercent = new System.Windows.Forms.TextBox();
            this.TextBox_SnrRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_SnrRangeLow = new System.Windows.Forms.TextBox();
            this.Label_SnrStdDev = new System.Windows.Forms.Label();
            this.GroupBox_MedianWeight = new System.Windows.Forms.GroupBox();
            this.Label_MedianRangeLow = new System.Windows.Forms.Label();
            this.Label_MedianMean = new System.Windows.Forms.Label();
            this.Label_MedianRangeHigh = new System.Windows.Forms.Label();
            this.Label_MedianPercent = new System.Windows.Forms.Label();
            this.TextBox_MedianPercent = new System.Windows.Forms.TextBox();
            this.TextBox_MedianRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_MedianRangeLow = new System.Windows.Forms.TextBox();
            this.Label_MedianStdDev = new System.Windows.Forms.Label();
            this.GroupBox_NoiseWeight = new System.Windows.Forms.GroupBox();
            this.Label_NoiseRangeLow = new System.Windows.Forms.Label();
            this.Label_NoiseMean = new System.Windows.Forms.Label();
            this.Label_NoiseRangeHigh = new System.Windows.Forms.Label();
            this.Label_NoisePercent = new System.Windows.Forms.Label();
            this.TextBox_NoisePercent = new System.Windows.Forms.TextBox();
            this.TextBox_NoiseRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_NoiseRangeLow = new System.Windows.Forms.TextBox();
            this.Label_NoiseStdDev = new System.Windows.Forms.Label();
            this.GroupBox_FwhmMeanDeviationWeight = new System.Windows.Forms.GroupBox();
            this.Label_FwhmMeanDeviationRangeLow = new System.Windows.Forms.Label();
            this.Label_FwhmMeanDeviationMean = new System.Windows.Forms.Label();
            this.Label_FwhmMeanDeviationRangeHigh = new System.Windows.Forms.Label();
            this.Label_FwhmMeanDeviationPercent = new System.Windows.Forms.Label();
            this.TextBox_FwhmMeanDeviationPercent = new System.Windows.Forms.TextBox();
            this.TextBox_FwhmMeanDeviationRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_FwhmMeanDeviationRangeLow = new System.Windows.Forms.TextBox();
            this.Label_FwhmMeanDeviationStdDev = new System.Windows.Forms.Label();
            this.GroupBox_EccentricityWeight = new System.Windows.Forms.GroupBox();
            this.Label_EccentricityRangeLow = new System.Windows.Forms.Label();
            this.Label_EccentricityRangeHigh = new System.Windows.Forms.Label();
            this.Label_EccentricyMean = new System.Windows.Forms.Label();
            this.Label_EccentricityPercent = new System.Windows.Forms.Label();
            this.Label_EccentricityStdDev = new System.Windows.Forms.Label();
            this.TextBox_EccentricityPercent = new System.Windows.Forms.TextBox();
            this.TextBox_EccentricityRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_EccentricityRangeLow = new System.Windows.Forms.TextBox();
            this.GroupBox_NoiseRatioWeight = new System.Windows.Forms.GroupBox();
            this.Label_NoiseRatioRangeLow = new System.Windows.Forms.Label();
            this.Label_NoiseRatioMean = new System.Windows.Forms.Label();
            this.Label_NoiseRatioRangeHigh = new System.Windows.Forms.Label();
            this.Label_NoiseRatioPercent = new System.Windows.Forms.Label();
            this.TextBox_NoiseRatioPercent = new System.Windows.Forms.TextBox();
            this.TextBox_NoiseRatioRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_NoiseRatioRangeLow = new System.Windows.Forms.TextBox();
            this.label_NoiseRatioStdDev = new System.Windows.Forms.Label();
            this.GroupBox_FwhmWeight = new System.Windows.Forms.GroupBox();
            this.Label_FwhmRangeLow = new System.Windows.Forms.Label();
            this.Label_FwhmMean = new System.Windows.Forms.Label();
            this.Label_FwhmRangeHigh = new System.Windows.Forms.Label();
            this.Label_FwhmPercent = new System.Windows.Forms.Label();
            this.TextBox_FwhmPercent = new System.Windows.Forms.TextBox();
            this.TextBox_FwhmRangeHigh = new System.Windows.Forms.TextBox();
            this.TextBox_FwhmRangeLow = new System.Windows.Forms.TextBox();
            this.Label_FwhmStdDev = new System.Windows.Forms.Label();
            this.Label_Task = new System.Windows.Forms.Label();
            this.Label_TempratureCompensation = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Label_OverheadPercent = new System.Windows.Forms.Label();
            this.Label_OPercent = new System.Windows.Forms.Label();
            this.Label_OSeconds = new System.Windows.Forms.Label();
            this.Label_OverheadSeconds = new System.Windows.Forms.Label();
            this.GroupBox_WeightCalculations = new System.Windows.Forms.GroupBox();
            this.GroupBox_Order.SuspendLayout();
            this.GroupBox_XisfFileUpdate.SuspendLayout();
            this.GroupBox_SubFrameKeywords.SuspendLayout();
            this.GroupBox_DirectorySelection.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.GroupBox_WeightsAndStatistics.SuspendLayout();
            this.GroupBox_InitialRejectionCriteria.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Median)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Eccentricity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Rejection_FWHM)).BeginInit();
            this.GroupBox_StarResidual.SuspendLayout();
            this.GroupBox_UpdateStatistics.SuspendLayout();
            this.GroupBox_StarResidualMeanDeviation.SuspendLayout();
            this.GroupBox_MedianMeanDeviationWeight.SuspendLayout();
            this.GroupBox_EccentricityMeanDeviationWeight.SuspendLayout();
            this.GroupBox_StarsWeight.SuspendLayout();
            this.GroupBox_SnrWeight.SuspendLayout();
            this.GroupBox_MedianWeight.SuspendLayout();
            this.GroupBox_NoiseWeight.SuspendLayout();
            this.GroupBox_FwhmMeanDeviationWeight.SuspendLayout();
            this.GroupBox_EccentricityWeight.SuspendLayout();
            this.GroupBox_NoiseRatioWeight.SuspendLayout();
            this.GroupBox_FwhmWeight.SuspendLayout();
            this.GroupBox_WeightCalculations.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Browse
            // 
            this.Button_Browse.Location = new System.Drawing.Point(26, 23);
            this.Button_Browse.Name = "Button_Browse";
            this.Button_Browse.Size = new System.Drawing.Size(75, 23);
            this.Button_Browse.TabIndex = 0;
            this.Button_Browse.Text = "Browse";
            this.Button_Browse.UseVisualStyleBackColor = true;
            this.Button_Browse.Click += new System.EventHandler(this.Button_Browse_Click);
            // 
            // ProgressBar_OverAll
            // 
            this.ProgressBar_OverAll.Location = new System.Drawing.Point(269, 91);
            this.ProgressBar_OverAll.Name = "ProgressBar_OverAll";
            this.ProgressBar_OverAll.Size = new System.Drawing.Size(420, 11);
            this.ProgressBar_OverAll.Step = 1;
            this.ProgressBar_OverAll.TabIndex = 1;
            // 
            // CheckBox_Recurse
            // 
            this.CheckBox_Recurse.AutoSize = true;
            this.CheckBox_Recurse.Location = new System.Drawing.Point(114, 27);
            this.CheckBox_Recurse.Name = "CheckBox_Recurse";
            this.CheckBox_Recurse.Size = new System.Drawing.Size(119, 17);
            this.CheckBox_Recurse.TabIndex = 2;
            this.CheckBox_Recurse.Text = "Recurse Directories";
            this.CheckBox_Recurse.UseVisualStyleBackColor = true;
            // 
            // GroupBox_Order
            // 
            this.GroupBox_Order.Controls.Add(this.RadioButton_RenameOrder_Weight);
            this.GroupBox_Order.Controls.Add(this.RadioButton_RenameOrder_Index);
            this.GroupBox_Order.Controls.Add(this.RadioButton_RenameOrder_IndexWeight);
            this.GroupBox_Order.Controls.Add(this.RadioButton_RenameOrder_WeightIndex);
            this.GroupBox_Order.Controls.Add(this.Button_Rename);
            this.GroupBox_Order.Location = new System.Drawing.Point(18, 19);
            this.GroupBox_Order.Name = "GroupBox_Order";
            this.GroupBox_Order.Size = new System.Drawing.Size(231, 105);
            this.GroupBox_Order.TabIndex = 3;
            this.GroupBox_Order.TabStop = false;
            this.GroupBox_Order.Text = "Rename Order";
            // 
            // RadioButton_RenameOrder_Weight
            // 
            this.RadioButton_RenameOrder_Weight.AutoSize = true;
            this.RadioButton_RenameOrder_Weight.Location = new System.Drawing.Point(28, 45);
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
            this.RadioButton_RenameOrder_WeightIndex.Location = new System.Drawing.Point(117, 45);
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
            this.GroupBox_XisfFileUpdate.Controls.Add(this.GroupBox_SubFrameKeywords);
            this.GroupBox_XisfFileUpdate.Controls.Add(this.GroupBox_Order);
            this.GroupBox_XisfFileUpdate.Controls.Add(this.ProgressBar_XisfFile);
            this.GroupBox_XisfFileUpdate.Location = new System.Drawing.Point(12, 111);
            this.GroupBox_XisfFileUpdate.Name = "GroupBox_XisfFileUpdate";
            this.GroupBox_XisfFileUpdate.Size = new System.Drawing.Size(685, 156);
            this.GroupBox_XisfFileUpdate.TabIndex = 6;
            this.GroupBox_XisfFileUpdate.TabStop = false;
            this.GroupBox_XisfFileUpdate.Text = "XISF File Update";
            // 
            // GroupBox_SubFrameKeywords
            // 
            this.GroupBox_SubFrameKeywords.Controls.Add(this.CheckBox_SubFrameKeywords_UpdateTargetName);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.RadioButton_SubFrameKeyWords_EnableSubFrameData);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.RadioButton_SubFrameKeywords_Alphabetize);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.Button_UpdateXisfFiles);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.ComboBox_TargetName);
            this.GroupBox_SubFrameKeywords.Controls.Add(this.Label_TagetName);
            this.GroupBox_SubFrameKeywords.Location = new System.Drawing.Point(268, 19);
            this.GroupBox_SubFrameKeywords.Name = "GroupBox_SubFrameKeywords";
            this.GroupBox_SubFrameKeywords.Size = new System.Drawing.Size(398, 105);
            this.GroupBox_SubFrameKeywords.TabIndex = 14;
            this.GroupBox_SubFrameKeywords.TabStop = false;
            this.GroupBox_SubFrameKeywords.Text = "SubFrame Keywords";
            // 
            // CheckBox_SubFrameKeywords_UpdateTargetName
            // 
            this.CheckBox_SubFrameKeywords_UpdateTargetName.AutoSize = true;
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Location = new System.Drawing.Point(211, 72);
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Name = "CheckBox_SubFrameKeywords_UpdateTargetName";
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Size = new System.Drawing.Size(126, 17);
            this.CheckBox_SubFrameKeywords_UpdateTargetName.TabIndex = 17;
            this.CheckBox_SubFrameKeywords_UpdateTargetName.Text = "Update Target Name";
            this.CheckBox_SubFrameKeywords_UpdateTargetName.UseVisualStyleBackColor = true;
            // 
            // RadioButton_SubFrameKeyWords_EnableSubFrameData
            // 
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.AutoSize = true;
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.Location = new System.Drawing.Point(211, 48);
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.Name = "RadioButton_SubFrameKeyWords_EnableSubFrameData";
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.Size = new System.Drawing.Size(135, 17);
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.TabIndex = 15;
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.Text = "Enable SubFrame Data";
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.UseVisualStyleBackColor = true;
            this.RadioButton_SubFrameKeyWords_EnableSubFrameData.CheckedChanged += new System.EventHandler(this.RadioButton_SubFrameKeyWords_Update_CheckedChanged);
            // 
            // RadioButton_SubFrameKeywords_Alphabetize
            // 
            this.RadioButton_SubFrameKeywords_Alphabetize.AutoSize = true;
            this.RadioButton_SubFrameKeywords_Alphabetize.Checked = true;
            this.RadioButton_SubFrameKeywords_Alphabetize.Location = new System.Drawing.Point(211, 24);
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
            this.ComboBox_TargetName.Location = new System.Drawing.Point(21, 43);
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
            this.ProgressBar_XisfFile.Location = new System.Drawing.Point(26, 134);
            this.ProgressBar_XisfFile.Name = "ProgressBar_XisfFile";
            this.ProgressBar_XisfFile.Size = new System.Drawing.Size(632, 11);
            this.ProgressBar_XisfFile.Step = 1;
            this.ProgressBar_XisfFile.TabIndex = 13;
            // 
            // GroupBox_DirectorySelection
            // 
            this.GroupBox_DirectorySelection.Controls.Add(this.Button_Browse);
            this.GroupBox_DirectorySelection.Controls.Add(this.CheckBox_Recurse);
            this.GroupBox_DirectorySelection.Location = new System.Drawing.Point(20, 36);
            this.GroupBox_DirectorySelection.Name = "GroupBox_DirectorySelection";
            this.GroupBox_DirectorySelection.Size = new System.Drawing.Size(234, 66);
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
            this.menuStrip1.Size = new System.Drawing.Size(709, 24);
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
            this.GroupBox_WeightsAndStatistics.Location = new System.Drawing.Point(12, 273);
            this.GroupBox_WeightsAndStatistics.Name = "GroupBox_WeightsAndStatistics";
            this.GroupBox_WeightsAndStatistics.Size = new System.Drawing.Size(685, 619);
            this.GroupBox_WeightsAndStatistics.TabIndex = 11;
            this.GroupBox_WeightsAndStatistics.TabStop = false;
            this.GroupBox_WeightsAndStatistics.Text = "SubFrame Weights and Statistics";
            // 
            // GroupBox_InitialRejectionCriteria
            // 
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Button_Regection_MoveRejected);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_Median);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_Median);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.NumericUpDown_Rejection_Eccentricity);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.numericUpDown_Rejection_FWHM);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.TextBox_Rejection_Total);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_Total);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_Eccentricity);
            this.GroupBox_InitialRejectionCriteria.Controls.Add(this.Label_Rejection_FWHM);
            this.GroupBox_InitialRejectionCriteria.Location = new System.Drawing.Point(18, 99);
            this.GroupBox_InitialRejectionCriteria.Name = "GroupBox_InitialRejectionCriteria";
            this.GroupBox_InitialRejectionCriteria.Size = new System.Drawing.Size(648, 56);
            this.GroupBox_InitialRejectionCriteria.TabIndex = 26;
            this.GroupBox_InitialRejectionCriteria.TabStop = false;
            this.GroupBox_InitialRejectionCriteria.Text = "Initial Rejection Criteria";
            // 
            // Button_Regection_MoveRejected
            // 
            this.Button_Regection_MoveRejected.Location = new System.Drawing.Point(526, 22);
            this.Button_Regection_MoveRejected.Name = "Button_Regection_MoveRejected";
            this.Button_Regection_MoveRejected.Size = new System.Drawing.Size(75, 23);
            this.Button_Regection_MoveRejected.TabIndex = 10;
            this.Button_Regection_MoveRejected.Text = "Move";
            this.Button_Regection_MoveRejected.UseVisualStyleBackColor = true;
            // 
            // NumericUpDown_Rejection_Median
            // 
            this.NumericUpDown_Rejection_Median.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Median.Location = new System.Drawing.Point(285, 23);
            this.NumericUpDown_Rejection_Median.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Median.Name = "NumericUpDown_Rejection_Median";
            this.NumericUpDown_Rejection_Median.Size = new System.Drawing.Size(47, 20);
            this.NumericUpDown_Rejection_Median.TabIndex = 9;
            this.NumericUpDown_Rejection_Median.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_Median.Value = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            // 
            // Label_Rejection_Median
            // 
            this.Label_Rejection_Median.AutoSize = true;
            this.Label_Rejection_Median.Location = new System.Drawing.Point(237, 27);
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
            this.NumericUpDown_Rejection_Eccentricity.Location = new System.Drawing.Point(190, 23);
            this.NumericUpDown_Rejection_Eccentricity.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumericUpDown_Rejection_Eccentricity.Name = "NumericUpDown_Rejection_Eccentricity";
            this.NumericUpDown_Rejection_Eccentricity.Size = new System.Drawing.Size(44, 20);
            this.NumericUpDown_Rejection_Eccentricity.TabIndex = 7;
            this.NumericUpDown_Rejection_Eccentricity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumericUpDown_Rejection_Eccentricity.Value = new decimal(new int[] {
            65,
            0,
            0,
            131072});
            // 
            // numericUpDown_Rejection_FWHM
            // 
            this.numericUpDown_Rejection_FWHM.DecimalPlaces = 1;
            this.numericUpDown_Rejection_FWHM.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_Rejection_FWHM.Location = new System.Drawing.Point(70, 23);
            this.numericUpDown_Rejection_FWHM.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDown_Rejection_FWHM.Name = "numericUpDown_Rejection_FWHM";
            this.numericUpDown_Rejection_FWHM.Size = new System.Drawing.Size(44, 20);
            this.numericUpDown_Rejection_FWHM.TabIndex = 6;
            this.numericUpDown_Rejection_FWHM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_Rejection_FWHM.Value = new decimal(new int[] {
            45,
            0,
            0,
            65536});
            // 
            // TextBox_Rejection_Total
            // 
            this.TextBox_Rejection_Total.Location = new System.Drawing.Point(452, 23);
            this.TextBox_Rejection_Total.Name = "TextBox_Rejection_Total";
            this.TextBox_Rejection_Total.Size = new System.Drawing.Size(44, 20);
            this.TextBox_Rejection_Total.TabIndex = 5;
            this.TextBox_Rejection_Total.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label_Rejection_Total
            // 
            this.Label_Rejection_Total.AutoSize = true;
            this.Label_Rejection_Total.Location = new System.Drawing.Point(342, 27);
            this.Label_Rejection_Total.Name = "Label_Rejection_Total";
            this.Label_Rejection_Total.Size = new System.Drawing.Size(109, 13);
            this.Label_Rejection_Total.TabIndex = 4;
            this.Label_Rejection_Total.Text = "Rejected SubFrames:";
            this.Label_Rejection_Total.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Rejection_Eccentricity
            // 
            this.Label_Rejection_Eccentricity.AutoSize = true;
            this.Label_Rejection_Eccentricity.Location = new System.Drawing.Point(122, 27);
            this.Label_Rejection_Eccentricity.Name = "Label_Rejection_Eccentricity";
            this.Label_Rejection_Eccentricity.Size = new System.Drawing.Size(65, 13);
            this.Label_Rejection_Eccentricity.TabIndex = 3;
            this.Label_Rejection_Eccentricity.Text = "Eccentricity:";
            this.Label_Rejection_Eccentricity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_Rejection_FWHM
            // 
            this.Label_Rejection_FWHM.AutoSize = true;
            this.Label_Rejection_FWHM.Location = new System.Drawing.Point(25, 27);
            this.Label_Rejection_FWHM.Name = "Label_Rejection_FWHM";
            this.Label_Rejection_FWHM.Size = new System.Drawing.Size(44, 13);
            this.Label_Rejection_FWHM.TabIndex = 2;
            this.Label_Rejection_FWHM.Text = "FWHM:";
            this.Label_Rejection_FWHM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox_StarResidual
            // 
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualRangeLow);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualRangeHigh);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualMean);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualPercent);
            this.GroupBox_StarResidual.Controls.Add(this.Label_StarResidualStdDev);
            this.GroupBox_StarResidual.Controls.Add(this.TextBox_StarResidualPercent);
            this.GroupBox_StarResidual.Controls.Add(this.TextBox_StarResidualRangeHigh);
            this.GroupBox_StarResidual.Controls.Add(this.TextBox_StarResidualRangeLow);
            this.GroupBox_StarResidual.Location = new System.Drawing.Point(224, 339);
            this.GroupBox_StarResidual.Name = "GroupBox_StarResidual";
            this.GroupBox_StarResidual.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_StarResidual.TabIndex = 24;
            this.GroupBox_StarResidual.TabStop = false;
            this.GroupBox_StarResidual.Text = "Star Residual Weight";
            // 
            // Label_StarResidualRangeLow
            // 
            this.Label_StarResidualRangeLow.AutoSize = true;
            this.Label_StarResidualRangeLow.Location = new System.Drawing.Point(112, 73);
            this.Label_StarResidualRangeLow.Name = "Label_StarResidualRangeLow";
            this.Label_StarResidualRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_StarResidualRangeLow.TabIndex = 13;
            this.Label_StarResidualRangeLow.Text = "Low";
            this.Label_StarResidualRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarResidualRangeHigh
            // 
            this.Label_StarResidualRangeHigh.AutoSize = true;
            this.Label_StarResidualRangeHigh.Location = new System.Drawing.Point(110, 47);
            this.Label_StarResidualRangeHigh.Name = "Label_StarResidualRangeHigh";
            this.Label_StarResidualRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_StarResidualRangeHigh.TabIndex = 12;
            this.Label_StarResidualRangeHigh.Text = "High";
            this.Label_StarResidualRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarResidualMean
            // 
            this.Label_StarResidualMean.AutoSize = true;
            this.Label_StarResidualMean.Location = new System.Drawing.Point(33, 25);
            this.Label_StarResidualMean.Name = "Label_StarResidualMean";
            this.Label_StarResidualMean.Size = new System.Drawing.Size(40, 13);
            this.Label_StarResidualMean.TabIndex = 10;
            this.Label_StarResidualMean.Text = "Mean: ";
            // 
            // Label_StarResidualPercent
            // 
            this.Label_StarResidualPercent.AutoSize = true;
            this.Label_StarResidualPercent.Location = new System.Drawing.Point(53, 59);
            this.Label_StarResidualPercent.Name = "Label_StarResidualPercent";
            this.Label_StarResidualPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_StarResidualPercent.TabIndex = 6;
            this.Label_StarResidualPercent.Text = "Percent";
            // 
            // Label_StarResidualStdDev
            // 
            this.Label_StarResidualStdDev.AutoSize = true;
            this.Label_StarResidualStdDev.Location = new System.Drawing.Point(106, 25);
            this.Label_StarResidualStdDev.Name = "Label_StarResidualStdDev";
            this.Label_StarResidualStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_StarResidualStdDev.TabIndex = 11;
            this.Label_StarResidualStdDev.Text = "StdDev: ";
            // 
            // TextBox_StarResidualPercent
            // 
            this.TextBox_StarResidualPercent.Location = new System.Drawing.Point(18, 55);
            this.TextBox_StarResidualPercent.Name = "TextBox_StarResidualPercent";
            this.TextBox_StarResidualPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_StarResidualPercent.TabIndex = 7;
            this.TextBox_StarResidualPercent.Text = "0";
            this.TextBox_StarResidualPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualPercent.TextChanged += new System.EventHandler(this.TextBox_StarResidualPercent_TextChanged);
            // 
            // TextBox_StarResidualRangeHigh
            // 
            this.TextBox_StarResidualRangeHigh.Location = new System.Drawing.Point(141, 43);
            this.TextBox_StarResidualRangeHigh.Name = "TextBox_StarResidualRangeHigh";
            this.TextBox_StarResidualRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarResidualRangeHigh.TabIndex = 8;
            this.TextBox_StarResidualRangeHigh.Text = "1.0";
            this.TextBox_StarResidualRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualRangeHigh.TextChanged += new System.EventHandler(this.TextBox_StarResidualRangeHigh_TextChanged);
            // 
            // TextBox_StarResidualRangeLow
            // 
            this.TextBox_StarResidualRangeLow.Location = new System.Drawing.Point(141, 69);
            this.TextBox_StarResidualRangeLow.Name = "TextBox_StarResidualRangeLow";
            this.TextBox_StarResidualRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarResidualRangeLow.TabIndex = 9;
            this.TextBox_StarResidualRangeLow.Text = "0.0";
            this.TextBox_StarResidualRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualRangeLow.TextChanged += new System.EventHandler(this.TextBox_StarResidualRangeLow_TextChanged);
            // 
            // GroupBox_UpdateStatistics
            // 
            this.GroupBox_UpdateStatistics.Controls.Add(this.RadioButton_SetImageStatistics_CalculateWeights);
            this.GroupBox_UpdateStatistics.Controls.Add(this.RadioButton_SetImageStatistics_RescaleWeights);
            this.GroupBox_UpdateStatistics.Controls.Add(this.RadioButton_SetImageStatistics_KeepWeights);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Button_ReadSubFrameSelectorCsvFile);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Label_UpdateStatistics);
            this.GroupBox_UpdateStatistics.Controls.Add(this.TextBox_UpdateStatisticsRangeHigh);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Label_UpdateStatisticsRangeLow);
            this.GroupBox_UpdateStatistics.Controls.Add(this.TextBox_UpdateStatisticsRangeLow);
            this.GroupBox_UpdateStatistics.Controls.Add(this.Label_UpdateStatisticsRangeHigh);
            this.GroupBox_UpdateStatistics.Location = new System.Drawing.Point(18, 20);
            this.GroupBox_UpdateStatistics.Name = "GroupBox_UpdateStatistics";
            this.GroupBox_UpdateStatistics.Size = new System.Drawing.Size(648, 73);
            this.GroupBox_UpdateStatistics.TabIndex = 20;
            this.GroupBox_UpdateStatistics.TabStop = false;
            this.GroupBox_UpdateStatistics.Text = "Set Image Statistics";
            // 
            // RadioButton_SetImageStatistics_CalculateWeights
            // 
            this.RadioButton_SetImageStatistics_CalculateWeights.AutoSize = true;
            this.RadioButton_SetImageStatistics_CalculateWeights.Location = new System.Drawing.Point(173, 47);
            this.RadioButton_SetImageStatistics_CalculateWeights.Name = "RadioButton_SetImageStatistics_CalculateWeights";
            this.RadioButton_SetImageStatistics_CalculateWeights.Size = new System.Drawing.Size(111, 17);
            this.RadioButton_SetImageStatistics_CalculateWeights.TabIndex = 24;
            this.RadioButton_SetImageStatistics_CalculateWeights.Text = "Calculate Weights";
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
            this.RadioButton_SetImageStatistics_RescaleWeights.CheckedChanged += new System.EventHandler(this.RadioButton_SetImageStatistics_RescaleCSVWeights_CheckedChanged);
            // 
            // RadioButton_SetImageStatistics_KeepWeights
            // 
            this.RadioButton_SetImageStatistics_KeepWeights.AutoSize = true;
            this.RadioButton_SetImageStatistics_KeepWeights.Checked = true;
            this.RadioButton_SetImageStatistics_KeepWeights.Location = new System.Drawing.Point(173, 13);
            this.RadioButton_SetImageStatistics_KeepWeights.Name = "RadioButton_SetImageStatistics_KeepWeights";
            this.RadioButton_SetImageStatistics_KeepWeights.Size = new System.Drawing.Size(92, 17);
            this.RadioButton_SetImageStatistics_KeepWeights.TabIndex = 21;
            this.RadioButton_SetImageStatistics_KeepWeights.TabStop = true;
            this.RadioButton_SetImageStatistics_KeepWeights.Text = "Keep Weights";
            this.RadioButton_SetImageStatistics_KeepWeights.UseVisualStyleBackColor = true;
            this.RadioButton_SetImageStatistics_KeepWeights.CheckedChanged += new System.EventHandler(this.RadioButton_SetImageStatistics_KeepCSVWeights_CheckedChanged);
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
            this.Label_UpdateStatistics.Location = new System.Drawing.Point(317, 32);
            this.Label_UpdateStatistics.Name = "Label_UpdateStatistics";
            this.Label_UpdateStatistics.Size = new System.Drawing.Size(151, 13);
            this.Label_UpdateStatistics.TabIndex = 20;
            this.Label_UpdateStatistics.Text = "Rescale SSWEIGHT - Range:";
            // 
            // TextBox_UpdateStatisticsRangeHigh
            // 
            this.TextBox_UpdateStatisticsRangeHigh.Location = new System.Drawing.Point(500, 28);
            this.TextBox_UpdateStatisticsRangeHigh.Name = "TextBox_UpdateStatisticsRangeHigh";
            this.TextBox_UpdateStatisticsRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_UpdateStatisticsRangeHigh.TabIndex = 16;
            this.TextBox_UpdateStatisticsRangeHigh.Text = "100";
            this.TextBox_UpdateStatisticsRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_UpdateStatisticsRangeHigh.TextChanged += new System.EventHandler(this.TextBox_UpdateStatisticsRangeHigh_TextChanged);
            // 
            // Label_UpdateStatisticsRangeLow
            // 
            this.Label_UpdateStatisticsRangeLow.AutoSize = true;
            this.Label_UpdateStatisticsRangeLow.Location = new System.Drawing.Point(556, 32);
            this.Label_UpdateStatisticsRangeLow.Name = "Label_UpdateStatisticsRangeLow";
            this.Label_UpdateStatisticsRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_UpdateStatisticsRangeLow.TabIndex = 19;
            this.Label_UpdateStatisticsRangeLow.Text = "Low";
            this.Label_UpdateStatisticsRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextBox_UpdateStatisticsRangeLow
            // 
            this.TextBox_UpdateStatisticsRangeLow.Location = new System.Drawing.Point(585, 28);
            this.TextBox_UpdateStatisticsRangeLow.Name = "TextBox_UpdateStatisticsRangeLow";
            this.TextBox_UpdateStatisticsRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_UpdateStatisticsRangeLow.TabIndex = 17;
            this.TextBox_UpdateStatisticsRangeLow.Text = "50";
            this.TextBox_UpdateStatisticsRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_UpdateStatisticsRangeLow.TextChanged += new System.EventHandler(this.TextBox_UpdateStatisticsRangeLow_TextChanged);
            // 
            // Label_UpdateStatisticsRangeHigh
            // 
            this.Label_UpdateStatisticsRangeHigh.AutoSize = true;
            this.Label_UpdateStatisticsRangeHigh.Location = new System.Drawing.Point(469, 32);
            this.Label_UpdateStatisticsRangeHigh.Name = "Label_UpdateStatisticsRangeHigh";
            this.Label_UpdateStatisticsRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_UpdateStatisticsRangeHigh.TabIndex = 18;
            this.Label_UpdateStatisticsRangeHigh.Text = "High";
            this.Label_UpdateStatisticsRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // GroupBox_StarResidualMeanDeviation
            // 
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.Label_StarResidualMeanDevationRangeLow);
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.Label_StarResidualMeanDevationMean);
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.Label_StarResidualMeanDevationRangeHigh);
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.Label_StarResidualMeanDevationPercent);
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.TextBox_StarResidualMeanDeviationPercent);
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.TextBox_StarResidualMeanDevationRangeHigh);
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.TextBox_StarResidualMeanDevationRangeLow);
            this.GroupBox_StarResidualMeanDeviation.Controls.Add(this.Label_StarResidualMeanDevationStdDev);
            this.GroupBox_StarResidualMeanDeviation.Location = new System.Drawing.Point(440, 339);
            this.GroupBox_StarResidualMeanDeviation.Name = "GroupBox_StarResidualMeanDeviation";
            this.GroupBox_StarResidualMeanDeviation.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_StarResidualMeanDeviation.TabIndex = 25;
            this.GroupBox_StarResidualMeanDeviation.TabStop = false;
            this.GroupBox_StarResidualMeanDeviation.Text = "Star Residual Mean Deviation Weight";
            // 
            // Label_StarResidualMeanDevationRangeLow
            // 
            this.Label_StarResidualMeanDevationRangeLow.AutoSize = true;
            this.Label_StarResidualMeanDevationRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_StarResidualMeanDevationRangeLow.Name = "Label_StarResidualMeanDevationRangeLow";
            this.Label_StarResidualMeanDevationRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_StarResidualMeanDevationRangeLow.TabIndex = 15;
            this.Label_StarResidualMeanDevationRangeLow.Text = "Low";
            this.Label_StarResidualMeanDevationRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarResidualMeanDevationMean
            // 
            this.Label_StarResidualMeanDevationMean.AutoSize = true;
            this.Label_StarResidualMeanDevationMean.Location = new System.Drawing.Point(29, 26);
            this.Label_StarResidualMeanDevationMean.Name = "Label_StarResidualMeanDevationMean";
            this.Label_StarResidualMeanDevationMean.Size = new System.Drawing.Size(40, 13);
            this.Label_StarResidualMeanDevationMean.TabIndex = 4;
            this.Label_StarResidualMeanDevationMean.Text = "Mean: ";
            // 
            // Label_StarResidualMeanDevationRangeHigh
            // 
            this.Label_StarResidualMeanDevationRangeHigh.AutoSize = true;
            this.Label_StarResidualMeanDevationRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_StarResidualMeanDevationRangeHigh.Name = "Label_StarResidualMeanDevationRangeHigh";
            this.Label_StarResidualMeanDevationRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_StarResidualMeanDevationRangeHigh.TabIndex = 14;
            this.Label_StarResidualMeanDevationRangeHigh.Text = "High";
            this.Label_StarResidualMeanDevationRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarResidualMeanDevationPercent
            // 
            this.Label_StarResidualMeanDevationPercent.AutoSize = true;
            this.Label_StarResidualMeanDevationPercent.Location = new System.Drawing.Point(53, 60);
            this.Label_StarResidualMeanDevationPercent.Name = "Label_StarResidualMeanDevationPercent";
            this.Label_StarResidualMeanDevationPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_StarResidualMeanDevationPercent.TabIndex = 0;
            this.Label_StarResidualMeanDevationPercent.Text = "Percent";
            // 
            // TextBox_StarResidualMeanDeviationPercent
            // 
            this.TextBox_StarResidualMeanDeviationPercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_StarResidualMeanDeviationPercent.Name = "TextBox_StarResidualMeanDeviationPercent";
            this.TextBox_StarResidualMeanDeviationPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_StarResidualMeanDeviationPercent.TabIndex = 1;
            this.TextBox_StarResidualMeanDeviationPercent.Text = "0";
            this.TextBox_StarResidualMeanDeviationPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualMeanDeviationPercent.TextChanged += new System.EventHandler(this.TextBox_StarResidualMeanDevationPercent_TextChanged);
            // 
            // TextBox_StarResidualMeanDevationRangeHigh
            // 
            this.TextBox_StarResidualMeanDevationRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_StarResidualMeanDevationRangeHigh.Name = "TextBox_StarResidualMeanDevationRangeHigh";
            this.TextBox_StarResidualMeanDevationRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarResidualMeanDevationRangeHigh.TabIndex = 2;
            this.TextBox_StarResidualMeanDevationRangeHigh.Text = "1.0";
            this.TextBox_StarResidualMeanDevationRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualMeanDevationRangeHigh.TextChanged += new System.EventHandler(this.TextBox_StarResidualMeanDevationRangeHigh_TextChanged);
            // 
            // TextBox_StarResidualMeanDevationRangeLow
            // 
            this.TextBox_StarResidualMeanDevationRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_StarResidualMeanDevationRangeLow.Name = "TextBox_StarResidualMeanDevationRangeLow";
            this.TextBox_StarResidualMeanDevationRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarResidualMeanDevationRangeLow.TabIndex = 3;
            this.TextBox_StarResidualMeanDevationRangeLow.Text = "0.0";
            this.TextBox_StarResidualMeanDevationRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarResidualMeanDevationRangeLow.TextChanged += new System.EventHandler(this.TextBox_StarResidualMeanDevationRangeLow_TextChanged);
            // 
            // Label_StarResidualMeanDevationStdDev
            // 
            this.Label_StarResidualMeanDevationStdDev.AutoSize = true;
            this.Label_StarResidualMeanDevationStdDev.Location = new System.Drawing.Point(102, 26);
            this.Label_StarResidualMeanDevationStdDev.Name = "Label_StarResidualMeanDevationStdDev";
            this.Label_StarResidualMeanDevationStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_StarResidualMeanDevationStdDev.TabIndex = 5;
            this.Label_StarResidualMeanDevationStdDev.Text = "StdDev: ";
            // 
            // GroupBox_MedianMeanDeviationWeight
            // 
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.Label_MedianMeanDeviationRangeLow);
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.Label_MedianMeanDeviationRangeHigh);
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.Label_MedianMeanDeviationMean);
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.Label_MedianMeanDeviationPercent);
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.Label_MedianMeanDeviationStdDev);
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.TextBox_MedianMeanDeviationPercent);
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.TextBox_MedianMeanDeviationRangeHigh);
            this.GroupBox_MedianMeanDeviationWeight.Controls.Add(this.TextBox_MedianMeanDeviationRangeLow);
            this.GroupBox_MedianMeanDeviationWeight.Location = new System.Drawing.Point(440, 127);
            this.GroupBox_MedianMeanDeviationWeight.Name = "GroupBox_MedianMeanDeviationWeight";
            this.GroupBox_MedianMeanDeviationWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_MedianMeanDeviationWeight.TabIndex = 18;
            this.GroupBox_MedianMeanDeviationWeight.TabStop = false;
            this.GroupBox_MedianMeanDeviationWeight.Text = "Median Mean Deviation Weight";
            // 
            // Label_MedianMeanDeviationRangeLow
            // 
            this.Label_MedianMeanDeviationRangeLow.AutoSize = true;
            this.Label_MedianMeanDeviationRangeLow.Location = new System.Drawing.Point(112, 73);
            this.Label_MedianMeanDeviationRangeLow.Name = "Label_MedianMeanDeviationRangeLow";
            this.Label_MedianMeanDeviationRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_MedianMeanDeviationRangeLow.TabIndex = 13;
            this.Label_MedianMeanDeviationRangeLow.Text = "Low";
            this.Label_MedianMeanDeviationRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MedianMeanDeviationRangeHigh
            // 
            this.Label_MedianMeanDeviationRangeHigh.AutoSize = true;
            this.Label_MedianMeanDeviationRangeHigh.Location = new System.Drawing.Point(110, 47);
            this.Label_MedianMeanDeviationRangeHigh.Name = "Label_MedianMeanDeviationRangeHigh";
            this.Label_MedianMeanDeviationRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_MedianMeanDeviationRangeHigh.TabIndex = 12;
            this.Label_MedianMeanDeviationRangeHigh.Text = "High";
            this.Label_MedianMeanDeviationRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MedianMeanDeviationMean
            // 
            this.Label_MedianMeanDeviationMean.AutoSize = true;
            this.Label_MedianMeanDeviationMean.Location = new System.Drawing.Point(33, 25);
            this.Label_MedianMeanDeviationMean.Name = "Label_MedianMeanDeviationMean";
            this.Label_MedianMeanDeviationMean.Size = new System.Drawing.Size(40, 13);
            this.Label_MedianMeanDeviationMean.TabIndex = 10;
            this.Label_MedianMeanDeviationMean.Text = "Mean: ";
            // 
            // Label_MedianMeanDeviationPercent
            // 
            this.Label_MedianMeanDeviationPercent.AutoSize = true;
            this.Label_MedianMeanDeviationPercent.Location = new System.Drawing.Point(53, 59);
            this.Label_MedianMeanDeviationPercent.Name = "Label_MedianMeanDeviationPercent";
            this.Label_MedianMeanDeviationPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_MedianMeanDeviationPercent.TabIndex = 6;
            this.Label_MedianMeanDeviationPercent.Text = "Percent";
            // 
            // Label_MedianMeanDeviationStdDev
            // 
            this.Label_MedianMeanDeviationStdDev.AutoSize = true;
            this.Label_MedianMeanDeviationStdDev.Location = new System.Drawing.Point(106, 25);
            this.Label_MedianMeanDeviationStdDev.Name = "Label_MedianMeanDeviationStdDev";
            this.Label_MedianMeanDeviationStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_MedianMeanDeviationStdDev.TabIndex = 11;
            this.Label_MedianMeanDeviationStdDev.Text = "StdDev: ";
            // 
            // TextBox_MedianMeanDeviationPercent
            // 
            this.TextBox_MedianMeanDeviationPercent.Location = new System.Drawing.Point(18, 55);
            this.TextBox_MedianMeanDeviationPercent.Name = "TextBox_MedianMeanDeviationPercent";
            this.TextBox_MedianMeanDeviationPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_MedianMeanDeviationPercent.TabIndex = 7;
            this.TextBox_MedianMeanDeviationPercent.Text = "0";
            this.TextBox_MedianMeanDeviationPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianMeanDeviationPercent.TextChanged += new System.EventHandler(this.TextBox_MeanMedianDeviationPercent_TextChanged);
            // 
            // TextBox_MedianMeanDeviationRangeHigh
            // 
            this.TextBox_MedianMeanDeviationRangeHigh.Location = new System.Drawing.Point(141, 43);
            this.TextBox_MedianMeanDeviationRangeHigh.Name = "TextBox_MedianMeanDeviationRangeHigh";
            this.TextBox_MedianMeanDeviationRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_MedianMeanDeviationRangeHigh.TabIndex = 8;
            this.TextBox_MedianMeanDeviationRangeHigh.Text = "1.0";
            this.TextBox_MedianMeanDeviationRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianMeanDeviationRangeHigh.TextChanged += new System.EventHandler(this.TextBox_MeanMedianDeviationRangeHigh_TextChanged);
            // 
            // TextBox_MedianMeanDeviationRangeLow
            // 
            this.TextBox_MedianMeanDeviationRangeLow.Location = new System.Drawing.Point(141, 69);
            this.TextBox_MedianMeanDeviationRangeLow.Name = "TextBox_MedianMeanDeviationRangeLow";
            this.TextBox_MedianMeanDeviationRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_MedianMeanDeviationRangeLow.TabIndex = 9;
            this.TextBox_MedianMeanDeviationRangeLow.Text = "0.0";
            this.TextBox_MedianMeanDeviationRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianMeanDeviationRangeLow.TextChanged += new System.EventHandler(this.TextBox_MeanMedianDeviationRangeLow_TextChanged);
            // 
            // GroupBox_EccentricityMeanDeviationWeight
            // 
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.Label_EccentricityMeanDeviationRangeLow);
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.Label_EccentricityMeanDeviationRangeHigh);
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.Label_EccentricityMeanDeviationMean);
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.Label_EccentricityMeanDeviationPercent);
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.Label_EccentricityMeanDeviationStdDev);
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.TextBox_EccentricityMeanDeviationPercent);
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.TextBox_EccentricityMeanDeviationRangeHigh);
            this.GroupBox_EccentricityMeanDeviationWeight.Controls.Add(this.TextBox_EccentricityMeanDeviationRangeLow);
            this.GroupBox_EccentricityMeanDeviationWeight.Location = new System.Drawing.Point(224, 127);
            this.GroupBox_EccentricityMeanDeviationWeight.Name = "GroupBox_EccentricityMeanDeviationWeight";
            this.GroupBox_EccentricityMeanDeviationWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_EccentricityMeanDeviationWeight.TabIndex = 21;
            this.GroupBox_EccentricityMeanDeviationWeight.TabStop = false;
            this.GroupBox_EccentricityMeanDeviationWeight.Text = "Eccentricity Mean Deviation Weight";
            // 
            // Label_EccentricityMeanDeviationRangeLow
            // 
            this.Label_EccentricityMeanDeviationRangeLow.AutoSize = true;
            this.Label_EccentricityMeanDeviationRangeLow.Location = new System.Drawing.Point(112, 73);
            this.Label_EccentricityMeanDeviationRangeLow.Name = "Label_EccentricityMeanDeviationRangeLow";
            this.Label_EccentricityMeanDeviationRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_EccentricityMeanDeviationRangeLow.TabIndex = 13;
            this.Label_EccentricityMeanDeviationRangeLow.Text = "Low";
            this.Label_EccentricityMeanDeviationRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EccentricityMeanDeviationRangeHigh
            // 
            this.Label_EccentricityMeanDeviationRangeHigh.AutoSize = true;
            this.Label_EccentricityMeanDeviationRangeHigh.Location = new System.Drawing.Point(110, 47);
            this.Label_EccentricityMeanDeviationRangeHigh.Name = "Label_EccentricityMeanDeviationRangeHigh";
            this.Label_EccentricityMeanDeviationRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_EccentricityMeanDeviationRangeHigh.TabIndex = 12;
            this.Label_EccentricityMeanDeviationRangeHigh.Text = "High";
            this.Label_EccentricityMeanDeviationRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EccentricityMeanDeviationMean
            // 
            this.Label_EccentricityMeanDeviationMean.AutoSize = true;
            this.Label_EccentricityMeanDeviationMean.Location = new System.Drawing.Point(33, 25);
            this.Label_EccentricityMeanDeviationMean.Name = "Label_EccentricityMeanDeviationMean";
            this.Label_EccentricityMeanDeviationMean.Size = new System.Drawing.Size(40, 13);
            this.Label_EccentricityMeanDeviationMean.TabIndex = 10;
            this.Label_EccentricityMeanDeviationMean.Text = "Mean: ";
            // 
            // Label_EccentricityMeanDeviationPercent
            // 
            this.Label_EccentricityMeanDeviationPercent.AutoSize = true;
            this.Label_EccentricityMeanDeviationPercent.Location = new System.Drawing.Point(53, 59);
            this.Label_EccentricityMeanDeviationPercent.Name = "Label_EccentricityMeanDeviationPercent";
            this.Label_EccentricityMeanDeviationPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_EccentricityMeanDeviationPercent.TabIndex = 6;
            this.Label_EccentricityMeanDeviationPercent.Text = "Percent";
            // 
            // Label_EccentricityMeanDeviationStdDev
            // 
            this.Label_EccentricityMeanDeviationStdDev.AutoSize = true;
            this.Label_EccentricityMeanDeviationStdDev.Location = new System.Drawing.Point(106, 25);
            this.Label_EccentricityMeanDeviationStdDev.Name = "Label_EccentricityMeanDeviationStdDev";
            this.Label_EccentricityMeanDeviationStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_EccentricityMeanDeviationStdDev.TabIndex = 11;
            this.Label_EccentricityMeanDeviationStdDev.Text = "StdDev: ";
            // 
            // TextBox_EccentricityMeanDeviationPercent
            // 
            this.TextBox_EccentricityMeanDeviationPercent.Location = new System.Drawing.Point(18, 55);
            this.TextBox_EccentricityMeanDeviationPercent.Name = "TextBox_EccentricityMeanDeviationPercent";
            this.TextBox_EccentricityMeanDeviationPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_EccentricityMeanDeviationPercent.TabIndex = 7;
            this.TextBox_EccentricityMeanDeviationPercent.Text = "15";
            this.TextBox_EccentricityMeanDeviationPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityMeanDeviationPercent.TextChanged += new System.EventHandler(this.TextBox_EccentricityMeanDeviationPercent_TextChanged);
            // 
            // TextBox_EccentricityMeanDeviationRangeHigh
            // 
            this.TextBox_EccentricityMeanDeviationRangeHigh.Location = new System.Drawing.Point(141, 43);
            this.TextBox_EccentricityMeanDeviationRangeHigh.Name = "TextBox_EccentricityMeanDeviationRangeHigh";
            this.TextBox_EccentricityMeanDeviationRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_EccentricityMeanDeviationRangeHigh.TabIndex = 8;
            this.TextBox_EccentricityMeanDeviationRangeHigh.Text = "1.0";
            this.TextBox_EccentricityMeanDeviationRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityMeanDeviationRangeHigh.TextChanged += new System.EventHandler(this.TextBox_EccentricityMeanDeviationRangeHigh_TextChanged);
            // 
            // TextBox_EccentricityMeanDeviationRangeLow
            // 
            this.TextBox_EccentricityMeanDeviationRangeLow.Location = new System.Drawing.Point(141, 69);
            this.TextBox_EccentricityMeanDeviationRangeLow.Name = "TextBox_EccentricityMeanDeviationRangeLow";
            this.TextBox_EccentricityMeanDeviationRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_EccentricityMeanDeviationRangeLow.TabIndex = 9;
            this.TextBox_EccentricityMeanDeviationRangeLow.Text = "0.0";
            this.TextBox_EccentricityMeanDeviationRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityMeanDeviationRangeLow.TextChanged += new System.EventHandler(this.TextBox_EccentricityMeanDeviationRangeLow_TextChanged);
            // 
            // GroupBox_StarsWeight
            // 
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarRangeLow);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarsMean);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarRangeHigh);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarPercent);
            this.GroupBox_StarsWeight.Controls.Add(this.TextBox_StarsPercent);
            this.GroupBox_StarsWeight.Controls.Add(this.TextBox_StarRangeHigh);
            this.GroupBox_StarsWeight.Controls.Add(this.TextBox_StarRangeLow);
            this.GroupBox_StarsWeight.Controls.Add(this.Label_StarStdDev);
            this.GroupBox_StarsWeight.Location = new System.Drawing.Point(8, 339);
            this.GroupBox_StarsWeight.Name = "GroupBox_StarsWeight";
            this.GroupBox_StarsWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_StarsWeight.TabIndex = 23;
            this.GroupBox_StarsWeight.TabStop = false;
            this.GroupBox_StarsWeight.Text = "Star Weight";
            // 
            // Label_StarRangeLow
            // 
            this.Label_StarRangeLow.AutoSize = true;
            this.Label_StarRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_StarRangeLow.Name = "Label_StarRangeLow";
            this.Label_StarRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_StarRangeLow.TabIndex = 15;
            this.Label_StarRangeLow.Text = "Low";
            this.Label_StarRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarMean
            // 
            this.Label_StarsMean.AutoSize = true;
            this.Label_StarsMean.Location = new System.Drawing.Point(29, 26);
            this.Label_StarsMean.Name = "Label_StarMean";
            this.Label_StarsMean.Size = new System.Drawing.Size(40, 13);
            this.Label_StarsMean.TabIndex = 4;
            this.Label_StarsMean.Text = "Mean: ";
            // 
            // Label_StarRangeHigh
            // 
            this.Label_StarRangeHigh.AutoSize = true;
            this.Label_StarRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_StarRangeHigh.Name = "Label_StarRangeHigh";
            this.Label_StarRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_StarRangeHigh.TabIndex = 14;
            this.Label_StarRangeHigh.Text = "High";
            this.Label_StarRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_StarPercent
            // 
            this.Label_StarPercent.AutoSize = true;
            this.Label_StarPercent.Location = new System.Drawing.Point(53, 60);
            this.Label_StarPercent.Name = "Label_StarPercent";
            this.Label_StarPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_StarPercent.TabIndex = 0;
            this.Label_StarPercent.Text = "Percent";
            // 
            // TextBox_StarsPercent
            // 
            this.TextBox_StarsPercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_StarsPercent.Name = "TextBox_StarsPercent";
            this.TextBox_StarsPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_StarsPercent.TabIndex = 1;
            this.TextBox_StarsPercent.Text = "0";
            this.TextBox_StarsPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarsPercent.TextChanged += new System.EventHandler(this.TextBox_StarsPercent_TextChanged);
            // 
            // TextBox_StarRangeHigh
            // 
            this.TextBox_StarRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_StarRangeHigh.Name = "TextBox_StarRangeHigh";
            this.TextBox_StarRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarRangeHigh.TabIndex = 2;
            this.TextBox_StarRangeHigh.Text = "1.0";
            this.TextBox_StarRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarRangeHigh.TextChanged += new System.EventHandler(this.TextBox_StarsRangeHigh_TextChanged);
            // 
            // TextBox_StarRangeLow
            // 
            this.TextBox_StarRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_StarRangeLow.Name = "TextBox_StarRangeLow";
            this.TextBox_StarRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_StarRangeLow.TabIndex = 3;
            this.TextBox_StarRangeLow.Text = "0.0";
            this.TextBox_StarRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_StarRangeLow.TextChanged += new System.EventHandler(this.TextBox_StarsRangeLow_TextChanged);
            // 
            // Label_StarStdDev
            // 
            this.Label_StarStdDev.AutoSize = true;
            this.Label_StarStdDev.Location = new System.Drawing.Point(102, 26);
            this.Label_StarStdDev.Name = "Label_StarStdDev";
            this.Label_StarStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_StarStdDev.TabIndex = 5;
            this.Label_StarStdDev.Text = "StdDev: ";
            // 
            // GroupBox_SnrWeight
            // 
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrRangeLow);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrMean);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrRangeHigh);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrPersent);
            this.GroupBox_SnrWeight.Controls.Add(this.TextBox_SnrPercent);
            this.GroupBox_SnrWeight.Controls.Add(this.TextBox_SnrRangeHigh);
            this.GroupBox_SnrWeight.Controls.Add(this.TextBox_SnrRangeLow);
            this.GroupBox_SnrWeight.Controls.Add(this.Label_SnrStdDev);
            this.GroupBox_SnrWeight.Location = new System.Drawing.Point(440, 233);
            this.GroupBox_SnrWeight.Name = "GroupBox_SnrWeight";
            this.GroupBox_SnrWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_SnrWeight.TabIndex = 16;
            this.GroupBox_SnrWeight.TabStop = false;
            this.GroupBox_SnrWeight.Text = "SNR Weight";
            // 
            // Label_SnrRangeLow
            // 
            this.Label_SnrRangeLow.AutoSize = true;
            this.Label_SnrRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_SnrRangeLow.Name = "Label_SnrRangeLow";
            this.Label_SnrRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_SnrRangeLow.TabIndex = 15;
            this.Label_SnrRangeLow.Text = "Low";
            this.Label_SnrRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_SnrMean
            // 
            this.Label_SnrMean.AutoSize = true;
            this.Label_SnrMean.Location = new System.Drawing.Point(29, 26);
            this.Label_SnrMean.Name = "Label_SnrMean";
            this.Label_SnrMean.Size = new System.Drawing.Size(40, 13);
            this.Label_SnrMean.TabIndex = 4;
            this.Label_SnrMean.Text = "Mean: ";
            // 
            // Label_SnrRangeHigh
            // 
            this.Label_SnrRangeHigh.AutoSize = true;
            this.Label_SnrRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_SnrRangeHigh.Name = "Label_SnrRangeHigh";
            this.Label_SnrRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_SnrRangeHigh.TabIndex = 14;
            this.Label_SnrRangeHigh.Text = "High";
            this.Label_SnrRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_SnrPersent
            // 
            this.Label_SnrPersent.AutoSize = true;
            this.Label_SnrPersent.Location = new System.Drawing.Point(53, 60);
            this.Label_SnrPersent.Name = "Label_SnrPersent";
            this.Label_SnrPersent.Size = new System.Drawing.Size(44, 13);
            this.Label_SnrPersent.TabIndex = 0;
            this.Label_SnrPersent.Text = "Percent";
            // 
            // TextBox_SnrPercent
            // 
            this.TextBox_SnrPercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_SnrPercent.Name = "TextBox_SnrPercent";
            this.TextBox_SnrPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_SnrPercent.TabIndex = 1;
            this.TextBox_SnrPercent.Text = "10";
            this.TextBox_SnrPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_SnrPercent.TextChanged += new System.EventHandler(this.TextBox_SnrPercent_TextChanged);
            // 
            // TextBox_SnrRangeHigh
            // 
            this.TextBox_SnrRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_SnrRangeHigh.Name = "TextBox_SnrRangeHigh";
            this.TextBox_SnrRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_SnrRangeHigh.TabIndex = 2;
            this.TextBox_SnrRangeHigh.Text = "1.0";
            this.TextBox_SnrRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_SnrRangeHigh.TextChanged += new System.EventHandler(this.TextBox_SnrRangeHigh_TextChanged);
            // 
            // TextBox_SnrRangeLow
            // 
            this.TextBox_SnrRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_SnrRangeLow.Name = "TextBox_SnrRangeLow";
            this.TextBox_SnrRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_SnrRangeLow.TabIndex = 3;
            this.TextBox_SnrRangeLow.Text = "0.0";
            this.TextBox_SnrRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_SnrRangeLow.TextChanged += new System.EventHandler(this.TextBox_SnrRangeLow_TextChanged);
            // 
            // Label_SnrStdDev
            // 
            this.Label_SnrStdDev.AutoSize = true;
            this.Label_SnrStdDev.Location = new System.Drawing.Point(102, 26);
            this.Label_SnrStdDev.Name = "Label_SnrStdDev";
            this.Label_SnrStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_SnrStdDev.TabIndex = 5;
            this.Label_SnrStdDev.Text = "StdDev: ";
            // 
            // GroupBox_MedianWeight
            // 
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianRangeLow);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianMean);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianRangeHigh);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianPercent);
            this.GroupBox_MedianWeight.Controls.Add(this.TextBox_MedianPercent);
            this.GroupBox_MedianWeight.Controls.Add(this.TextBox_MedianRangeHigh);
            this.GroupBox_MedianWeight.Controls.Add(this.TextBox_MedianRangeLow);
            this.GroupBox_MedianWeight.Controls.Add(this.Label_MedianStdDev);
            this.GroupBox_MedianWeight.Location = new System.Drawing.Point(440, 21);
            this.GroupBox_MedianWeight.Name = "GroupBox_MedianWeight";
            this.GroupBox_MedianWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_MedianWeight.TabIndex = 17;
            this.GroupBox_MedianWeight.TabStop = false;
            this.GroupBox_MedianWeight.Text = "Median Weight";
            // 
            // Label_MedianRangeLow
            // 
            this.Label_MedianRangeLow.AutoSize = true;
            this.Label_MedianRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_MedianRangeLow.Name = "Label_MedianRangeLow";
            this.Label_MedianRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_MedianRangeLow.TabIndex = 15;
            this.Label_MedianRangeLow.Text = "Low";
            this.Label_MedianRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MedianMean
            // 
            this.Label_MedianMean.AutoSize = true;
            this.Label_MedianMean.Location = new System.Drawing.Point(29, 26);
            this.Label_MedianMean.Name = "Label_MedianMean";
            this.Label_MedianMean.Size = new System.Drawing.Size(40, 13);
            this.Label_MedianMean.TabIndex = 4;
            this.Label_MedianMean.Text = "Mean: ";
            // 
            // Label_MedianRangeHigh
            // 
            this.Label_MedianRangeHigh.AutoSize = true;
            this.Label_MedianRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_MedianRangeHigh.Name = "Label_MedianRangeHigh";
            this.Label_MedianRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_MedianRangeHigh.TabIndex = 14;
            this.Label_MedianRangeHigh.Text = "High";
            this.Label_MedianRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_MedianPercent
            // 
            this.Label_MedianPercent.AutoSize = true;
            this.Label_MedianPercent.Location = new System.Drawing.Point(53, 60);
            this.Label_MedianPercent.Name = "Label_MedianPercent";
            this.Label_MedianPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_MedianPercent.TabIndex = 0;
            this.Label_MedianPercent.Text = "Percent";
            // 
            // TextBox_MedianPercent
            // 
            this.TextBox_MedianPercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_MedianPercent.Name = "TextBox_MedianPercent";
            this.TextBox_MedianPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_MedianPercent.TabIndex = 1;
            this.TextBox_MedianPercent.Text = "0";
            this.TextBox_MedianPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianPercent.TextChanged += new System.EventHandler(this.TextBox_MedianPercent_TextChanged);
            // 
            // TextBox_MedianRangeHigh
            // 
            this.TextBox_MedianRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_MedianRangeHigh.Name = "TextBox_MedianRangeHigh";
            this.TextBox_MedianRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_MedianRangeHigh.TabIndex = 2;
            this.TextBox_MedianRangeHigh.Text = "1.0";
            this.TextBox_MedianRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianRangeHigh.TextChanged += new System.EventHandler(this.TextBox_MedianRangeHigh_TextChanged);
            // 
            // TextBox_MedianRangeLow
            // 
            this.TextBox_MedianRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_MedianRangeLow.Name = "TextBox_MedianRangeLow";
            this.TextBox_MedianRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_MedianRangeLow.TabIndex = 3;
            this.TextBox_MedianRangeLow.Text = "0.0";
            this.TextBox_MedianRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_MedianRangeLow.TextChanged += new System.EventHandler(this.TextBox_MedianRangeLow_TextChanged);
            // 
            // Label_MedianStdDev
            // 
            this.Label_MedianStdDev.AutoSize = true;
            this.Label_MedianStdDev.Location = new System.Drawing.Point(102, 26);
            this.Label_MedianStdDev.Name = "Label_MedianStdDev";
            this.Label_MedianStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_MedianStdDev.TabIndex = 5;
            this.Label_MedianStdDev.Text = "StdDev: ";
            // 
            // GroupBox_NoiseWeight
            // 
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseRangeLow);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseMean);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseRangeHigh);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoisePercent);
            this.GroupBox_NoiseWeight.Controls.Add(this.TextBox_NoisePercent);
            this.GroupBox_NoiseWeight.Controls.Add(this.TextBox_NoiseRangeHigh);
            this.GroupBox_NoiseWeight.Controls.Add(this.TextBox_NoiseRangeLow);
            this.GroupBox_NoiseWeight.Controls.Add(this.Label_NoiseStdDev);
            this.GroupBox_NoiseWeight.Location = new System.Drawing.Point(8, 233);
            this.GroupBox_NoiseWeight.Name = "GroupBox_NoiseWeight";
            this.GroupBox_NoiseWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_NoiseWeight.TabIndex = 19;
            this.GroupBox_NoiseWeight.TabStop = false;
            this.GroupBox_NoiseWeight.Text = "Noise Weight";
            // 
            // Label_NoiseRangeLow
            // 
            this.Label_NoiseRangeLow.AutoSize = true;
            this.Label_NoiseRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_NoiseRangeLow.Name = "Label_NoiseRangeLow";
            this.Label_NoiseRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_NoiseRangeLow.TabIndex = 15;
            this.Label_NoiseRangeLow.Text = "Low";
            this.Label_NoiseRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_NoiseMean
            // 
            this.Label_NoiseMean.AutoSize = true;
            this.Label_NoiseMean.Location = new System.Drawing.Point(29, 26);
            this.Label_NoiseMean.Name = "Label_NoiseMean";
            this.Label_NoiseMean.Size = new System.Drawing.Size(40, 13);
            this.Label_NoiseMean.TabIndex = 4;
            this.Label_NoiseMean.Text = "Mean: ";
            // 
            // Label_NoiseRangeHigh
            // 
            this.Label_NoiseRangeHigh.AutoSize = true;
            this.Label_NoiseRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_NoiseRangeHigh.Name = "Label_NoiseRangeHigh";
            this.Label_NoiseRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_NoiseRangeHigh.TabIndex = 14;
            this.Label_NoiseRangeHigh.Text = "High";
            this.Label_NoiseRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_NoisePercent
            // 
            this.Label_NoisePercent.AutoSize = true;
            this.Label_NoisePercent.Location = new System.Drawing.Point(53, 60);
            this.Label_NoisePercent.Name = "Label_NoisePercent";
            this.Label_NoisePercent.Size = new System.Drawing.Size(44, 13);
            this.Label_NoisePercent.TabIndex = 0;
            this.Label_NoisePercent.Text = "Percent";
            // 
            // TextBox_NoisePercent
            // 
            this.TextBox_NoisePercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_NoisePercent.Name = "TextBox_NoisePercent";
            this.TextBox_NoisePercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_NoisePercent.TabIndex = 1;
            this.TextBox_NoisePercent.Text = "0";
            this.TextBox_NoisePercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoisePercent.TextChanged += new System.EventHandler(this.TextBox_NoisePercent_TextChanged);
            // 
            // TextBox_NoiseRangeHigh
            // 
            this.TextBox_NoiseRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_NoiseRangeHigh.Name = "TextBox_NoiseRangeHigh";
            this.TextBox_NoiseRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_NoiseRangeHigh.TabIndex = 2;
            this.TextBox_NoiseRangeHigh.Text = "1.0";
            this.TextBox_NoiseRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoiseRangeHigh.TextChanged += new System.EventHandler(this.TextBox_NoiseRangeHigh_TextChanged);
            // 
            // TextBox_NoiseRangeLow
            // 
            this.TextBox_NoiseRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_NoiseRangeLow.Name = "TextBox_NoiseRangeLow";
            this.TextBox_NoiseRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_NoiseRangeLow.TabIndex = 3;
            this.TextBox_NoiseRangeLow.Text = "0.0";
            this.TextBox_NoiseRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoiseRangeLow.TextChanged += new System.EventHandler(this.TextBox_NoiseRangeLow_TextChanged);
            // 
            // Label_NoiseStdDev
            // 
            this.Label_NoiseStdDev.AutoSize = true;
            this.Label_NoiseStdDev.Location = new System.Drawing.Point(102, 26);
            this.Label_NoiseStdDev.Name = "Label_NoiseStdDev";
            this.Label_NoiseStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_NoiseStdDev.TabIndex = 5;
            this.Label_NoiseStdDev.Text = "StdDev: ";
            // 
            // GroupBox_FwhmMeanDeviationWeight
            // 
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.Label_FwhmMeanDeviationRangeLow);
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.Label_FwhmMeanDeviationMean);
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.Label_FwhmMeanDeviationRangeHigh);
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.Label_FwhmMeanDeviationPercent);
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.TextBox_FwhmMeanDeviationPercent);
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.TextBox_FwhmMeanDeviationRangeHigh);
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.TextBox_FwhmMeanDeviationRangeLow);
            this.GroupBox_FwhmMeanDeviationWeight.Controls.Add(this.Label_FwhmMeanDeviationStdDev);
            this.GroupBox_FwhmMeanDeviationWeight.Location = new System.Drawing.Point(8, 127);
            this.GroupBox_FwhmMeanDeviationWeight.Name = "GroupBox_FwhmMeanDeviationWeight";
            this.GroupBox_FwhmMeanDeviationWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_FwhmMeanDeviationWeight.TabIndex = 22;
            this.GroupBox_FwhmMeanDeviationWeight.TabStop = false;
            this.GroupBox_FwhmMeanDeviationWeight.Text = "FWHM Mean Deviation Weight";
            // 
            // Label_FwhmMeanDeviationRangeLow
            // 
            this.Label_FwhmMeanDeviationRangeLow.AutoSize = true;
            this.Label_FwhmMeanDeviationRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_FwhmMeanDeviationRangeLow.Name = "Label_FwhmMeanDeviationRangeLow";
            this.Label_FwhmMeanDeviationRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_FwhmMeanDeviationRangeLow.TabIndex = 15;
            this.Label_FwhmMeanDeviationRangeLow.Text = "Low";
            this.Label_FwhmMeanDeviationRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmMeanDeviationMean
            // 
            this.Label_FwhmMeanDeviationMean.AutoSize = true;
            this.Label_FwhmMeanDeviationMean.Location = new System.Drawing.Point(29, 26);
            this.Label_FwhmMeanDeviationMean.Name = "Label_FwhmMeanDeviationMean";
            this.Label_FwhmMeanDeviationMean.Size = new System.Drawing.Size(40, 13);
            this.Label_FwhmMeanDeviationMean.TabIndex = 4;
            this.Label_FwhmMeanDeviationMean.Text = "Mean: ";
            // 
            // Label_FwhmMeanDeviationRangeHigh
            // 
            this.Label_FwhmMeanDeviationRangeHigh.AutoSize = true;
            this.Label_FwhmMeanDeviationRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_FwhmMeanDeviationRangeHigh.Name = "Label_FwhmMeanDeviationRangeHigh";
            this.Label_FwhmMeanDeviationRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_FwhmMeanDeviationRangeHigh.TabIndex = 14;
            this.Label_FwhmMeanDeviationRangeHigh.Text = "High";
            this.Label_FwhmMeanDeviationRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmMeanDeviationPercent
            // 
            this.Label_FwhmMeanDeviationPercent.AutoSize = true;
            this.Label_FwhmMeanDeviationPercent.Location = new System.Drawing.Point(53, 60);
            this.Label_FwhmMeanDeviationPercent.Name = "Label_FwhmMeanDeviationPercent";
            this.Label_FwhmMeanDeviationPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_FwhmMeanDeviationPercent.TabIndex = 0;
            this.Label_FwhmMeanDeviationPercent.Text = "Percent";
            // 
            // TextBox_FwhmMeanDeviationPercent
            // 
            this.TextBox_FwhmMeanDeviationPercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_FwhmMeanDeviationPercent.Name = "TextBox_FwhmMeanDeviationPercent";
            this.TextBox_FwhmMeanDeviationPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_FwhmMeanDeviationPercent.TabIndex = 1;
            this.TextBox_FwhmMeanDeviationPercent.Text = "10";
            this.TextBox_FwhmMeanDeviationPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmMeanDeviationPercent.TextChanged += new System.EventHandler(this.TextBox_FwhmMeanDeviationPercent_TextChanged);
            // 
            // TextBox_FwhmMeanDeviationRangeHigh
            // 
            this.TextBox_FwhmMeanDeviationRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_FwhmMeanDeviationRangeHigh.Name = "TextBox_FwhmMeanDeviationRangeHigh";
            this.TextBox_FwhmMeanDeviationRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_FwhmMeanDeviationRangeHigh.TabIndex = 2;
            this.TextBox_FwhmMeanDeviationRangeHigh.Text = "1.0";
            this.TextBox_FwhmMeanDeviationRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmMeanDeviationRangeHigh.TextChanged += new System.EventHandler(this.TextBox_FwhmMeanDeviationRangeHigh_TextChanged);
            // 
            // TextBox_FwhmMeanDeviationRangeLow
            // 
            this.TextBox_FwhmMeanDeviationRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_FwhmMeanDeviationRangeLow.Name = "TextBox_FwhmMeanDeviationRangeLow";
            this.TextBox_FwhmMeanDeviationRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_FwhmMeanDeviationRangeLow.TabIndex = 3;
            this.TextBox_FwhmMeanDeviationRangeLow.Text = "0.0";
            this.TextBox_FwhmMeanDeviationRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmMeanDeviationRangeLow.TextChanged += new System.EventHandler(this.TextBox_FwhmMeanDeviationRangeLow_TextChanged);
            // 
            // Label_FwhmMeanDeviationStdDev
            // 
            this.Label_FwhmMeanDeviationStdDev.AutoSize = true;
            this.Label_FwhmMeanDeviationStdDev.Location = new System.Drawing.Point(102, 26);
            this.Label_FwhmMeanDeviationStdDev.Name = "Label_FwhmMeanDeviationStdDev";
            this.Label_FwhmMeanDeviationStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_FwhmMeanDeviationStdDev.TabIndex = 5;
            this.Label_FwhmMeanDeviationStdDev.Text = "StdDev: ";
            // 
            // GroupBox_EccentricityWeight
            // 
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityRangeLow);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityRangeHigh);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricyMean);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityPercent);
            this.GroupBox_EccentricityWeight.Controls.Add(this.Label_EccentricityStdDev);
            this.GroupBox_EccentricityWeight.Controls.Add(this.TextBox_EccentricityPercent);
            this.GroupBox_EccentricityWeight.Controls.Add(this.TextBox_EccentricityRangeHigh);
            this.GroupBox_EccentricityWeight.Controls.Add(this.TextBox_EccentricityRangeLow);
            this.GroupBox_EccentricityWeight.Location = new System.Drawing.Point(224, 21);
            this.GroupBox_EccentricityWeight.Name = "GroupBox_EccentricityWeight";
            this.GroupBox_EccentricityWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_EccentricityWeight.TabIndex = 13;
            this.GroupBox_EccentricityWeight.TabStop = false;
            this.GroupBox_EccentricityWeight.Text = "Eccentricity Weight";
            // 
            // Label_EccentricityRangeLow
            // 
            this.Label_EccentricityRangeLow.AutoSize = true;
            this.Label_EccentricityRangeLow.Location = new System.Drawing.Point(112, 73);
            this.Label_EccentricityRangeLow.Name = "Label_EccentricityRangeLow";
            this.Label_EccentricityRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_EccentricityRangeLow.TabIndex = 13;
            this.Label_EccentricityRangeLow.Text = "Low";
            this.Label_EccentricityRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EccentricityRangeHigh
            // 
            this.Label_EccentricityRangeHigh.AutoSize = true;
            this.Label_EccentricityRangeHigh.Location = new System.Drawing.Point(110, 47);
            this.Label_EccentricityRangeHigh.Name = "Label_EccentricityRangeHigh";
            this.Label_EccentricityRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_EccentricityRangeHigh.TabIndex = 12;
            this.Label_EccentricityRangeHigh.Text = "High";
            this.Label_EccentricityRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_EccentricyMean
            // 
            this.Label_EccentricyMean.AutoSize = true;
            this.Label_EccentricyMean.Location = new System.Drawing.Point(33, 25);
            this.Label_EccentricyMean.Name = "Label_EccentricyMean";
            this.Label_EccentricyMean.Size = new System.Drawing.Size(40, 13);
            this.Label_EccentricyMean.TabIndex = 10;
            this.Label_EccentricyMean.Text = "Mean: ";
            // 
            // Label_EccentricityPercent
            // 
            this.Label_EccentricityPercent.AutoSize = true;
            this.Label_EccentricityPercent.Location = new System.Drawing.Point(53, 59);
            this.Label_EccentricityPercent.Name = "Label_EccentricityPercent";
            this.Label_EccentricityPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_EccentricityPercent.TabIndex = 6;
            this.Label_EccentricityPercent.Text = "Percent";
            // 
            // Label_EccentricityStdDev
            // 
            this.Label_EccentricityStdDev.AutoSize = true;
            this.Label_EccentricityStdDev.Location = new System.Drawing.Point(106, 25);
            this.Label_EccentricityStdDev.Name = "Label_EccentricityStdDev";
            this.Label_EccentricityStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_EccentricityStdDev.TabIndex = 11;
            this.Label_EccentricityStdDev.Text = "StdDev: ";
            // 
            // TextBox_EccentricityPercent
            // 
            this.TextBox_EccentricityPercent.Location = new System.Drawing.Point(18, 55);
            this.TextBox_EccentricityPercent.Name = "TextBox_EccentricityPercent";
            this.TextBox_EccentricityPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_EccentricityPercent.TabIndex = 7;
            this.TextBox_EccentricityPercent.Text = "15";
            this.TextBox_EccentricityPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityPercent.TextChanged += new System.EventHandler(this.TextBox_EccentricityPercent_TextChanged);
            // 
            // TextBox_EccentricityRangeHigh
            // 
            this.TextBox_EccentricityRangeHigh.Location = new System.Drawing.Point(141, 43);
            this.TextBox_EccentricityRangeHigh.Name = "TextBox_EccentricityRangeHigh";
            this.TextBox_EccentricityRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_EccentricityRangeHigh.TabIndex = 8;
            this.TextBox_EccentricityRangeHigh.Text = "1.0";
            this.TextBox_EccentricityRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityRangeHigh.TextChanged += new System.EventHandler(this.TextBox_EccentricityRangeHigh_TextChanged);
            // 
            // TextBox_EccentricityRangeLow
            // 
            this.TextBox_EccentricityRangeLow.Location = new System.Drawing.Point(141, 69);
            this.TextBox_EccentricityRangeLow.Name = "TextBox_EccentricityRangeLow";
            this.TextBox_EccentricityRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_EccentricityRangeLow.TabIndex = 9;
            this.TextBox_EccentricityRangeLow.Text = "0.0";
            this.TextBox_EccentricityRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_EccentricityRangeLow.TextChanged += new System.EventHandler(this.TextBox_EccentricityRangeLow_TextChanged);
            // 
            // GroupBox_NoiseRatioWeight
            // 
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.Label_NoiseRatioRangeLow);
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.Label_NoiseRatioMean);
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.Label_NoiseRatioRangeHigh);
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.Label_NoiseRatioPercent);
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.TextBox_NoiseRatioPercent);
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.TextBox_NoiseRatioRangeHigh);
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.TextBox_NoiseRatioRangeLow);
            this.GroupBox_NoiseRatioWeight.Controls.Add(this.label_NoiseRatioStdDev);
            this.GroupBox_NoiseRatioWeight.Location = new System.Drawing.Point(224, 233);
            this.GroupBox_NoiseRatioWeight.Name = "GroupBox_NoiseRatioWeight";
            this.GroupBox_NoiseRatioWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_NoiseRatioWeight.TabIndex = 20;
            this.GroupBox_NoiseRatioWeight.TabStop = false;
            this.GroupBox_NoiseRatioWeight.Text = "Noise Ratio Weight";
            // 
            // Label_NoiseRatioRangeLow
            // 
            this.Label_NoiseRatioRangeLow.AutoSize = true;
            this.Label_NoiseRatioRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_NoiseRatioRangeLow.Name = "Label_NoiseRatioRangeLow";
            this.Label_NoiseRatioRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_NoiseRatioRangeLow.TabIndex = 15;
            this.Label_NoiseRatioRangeLow.Text = "Low";
            this.Label_NoiseRatioRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_NoiseRatioMean
            // 
            this.Label_NoiseRatioMean.AutoSize = true;
            this.Label_NoiseRatioMean.Location = new System.Drawing.Point(29, 26);
            this.Label_NoiseRatioMean.Name = "Label_NoiseRatioMean";
            this.Label_NoiseRatioMean.Size = new System.Drawing.Size(40, 13);
            this.Label_NoiseRatioMean.TabIndex = 4;
            this.Label_NoiseRatioMean.Text = "Mean: ";
            // 
            // Label_NoiseRatioRangeHigh
            // 
            this.Label_NoiseRatioRangeHigh.AutoSize = true;
            this.Label_NoiseRatioRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_NoiseRatioRangeHigh.Name = "Label_NoiseRatioRangeHigh";
            this.Label_NoiseRatioRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_NoiseRatioRangeHigh.TabIndex = 14;
            this.Label_NoiseRatioRangeHigh.Text = "High";
            this.Label_NoiseRatioRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_NoiseRatioPercent
            // 
            this.Label_NoiseRatioPercent.AutoSize = true;
            this.Label_NoiseRatioPercent.Location = new System.Drawing.Point(53, 60);
            this.Label_NoiseRatioPercent.Name = "Label_NoiseRatioPercent";
            this.Label_NoiseRatioPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_NoiseRatioPercent.TabIndex = 0;
            this.Label_NoiseRatioPercent.Text = "Percent";
            // 
            // TextBox_NoiseRatioPercent
            // 
            this.TextBox_NoiseRatioPercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_NoiseRatioPercent.Name = "TextBox_NoiseRatioPercent";
            this.TextBox_NoiseRatioPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_NoiseRatioPercent.TabIndex = 1;
            this.TextBox_NoiseRatioPercent.Text = "75";
            this.TextBox_NoiseRatioPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoiseRatioPercent.TextChanged += new System.EventHandler(this.TextBox_NoiseRatioPercent_TextChanged);
            // 
            // TextBox_NoiseRatioRangeHigh
            // 
            this.TextBox_NoiseRatioRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_NoiseRatioRangeHigh.Name = "TextBox_NoiseRatioRangeHigh";
            this.TextBox_NoiseRatioRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_NoiseRatioRangeHigh.TabIndex = 2;
            this.TextBox_NoiseRatioRangeHigh.Text = "1.0";
            this.TextBox_NoiseRatioRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoiseRatioRangeHigh.TextChanged += new System.EventHandler(this.TextBox_NoiseRatioRangeHigh_TextChanged);
            // 
            // TextBox_NoiseRatioRangeLow
            // 
            this.TextBox_NoiseRatioRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_NoiseRatioRangeLow.Name = "TextBox_NoiseRatioRangeLow";
            this.TextBox_NoiseRatioRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_NoiseRatioRangeLow.TabIndex = 3;
            this.TextBox_NoiseRatioRangeLow.Text = "0.0";
            this.TextBox_NoiseRatioRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_NoiseRatioRangeLow.TextChanged += new System.EventHandler(this.TextBox_NoiseRationRangeLow_TextChanged);
            // 
            // label_NoiseRatioStdDev
            // 
            this.label_NoiseRatioStdDev.AutoSize = true;
            this.label_NoiseRatioStdDev.Location = new System.Drawing.Point(102, 26);
            this.label_NoiseRatioStdDev.Name = "label_NoiseRatioStdDev";
            this.label_NoiseRatioStdDev.Size = new System.Drawing.Size(49, 13);
            this.label_NoiseRatioStdDev.TabIndex = 5;
            this.label_NoiseRatioStdDev.Text = "StdDev: ";
            // 
            // GroupBox_FwhmWeight
            // 
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmRangeLow);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmMean);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmRangeHigh);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmPercent);
            this.GroupBox_FwhmWeight.Controls.Add(this.TextBox_FwhmPercent);
            this.GroupBox_FwhmWeight.Controls.Add(this.TextBox_FwhmRangeHigh);
            this.GroupBox_FwhmWeight.Controls.Add(this.TextBox_FwhmRangeLow);
            this.GroupBox_FwhmWeight.Controls.Add(this.Label_FwhmStdDev);
            this.GroupBox_FwhmWeight.Location = new System.Drawing.Point(8, 21);
            this.GroupBox_FwhmWeight.Name = "GroupBox_FwhmWeight";
            this.GroupBox_FwhmWeight.Size = new System.Drawing.Size(200, 100);
            this.GroupBox_FwhmWeight.TabIndex = 12;
            this.GroupBox_FwhmWeight.TabStop = false;
            this.GroupBox_FwhmWeight.Text = "FWHM Weight";
            // 
            // Label_FwhmRangeLow
            // 
            this.Label_FwhmRangeLow.AutoSize = true;
            this.Label_FwhmRangeLow.Location = new System.Drawing.Point(112, 74);
            this.Label_FwhmRangeLow.Name = "Label_FwhmRangeLow";
            this.Label_FwhmRangeLow.Size = new System.Drawing.Size(27, 13);
            this.Label_FwhmRangeLow.TabIndex = 15;
            this.Label_FwhmRangeLow.Text = "Low";
            this.Label_FwhmRangeLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmMean
            // 
            this.Label_FwhmMean.AutoSize = true;
            this.Label_FwhmMean.Location = new System.Drawing.Point(29, 26);
            this.Label_FwhmMean.Name = "Label_FwhmMean";
            this.Label_FwhmMean.Size = new System.Drawing.Size(40, 13);
            this.Label_FwhmMean.TabIndex = 4;
            this.Label_FwhmMean.Text = "Mean: ";
            // 
            // Label_FwhmRangeHigh
            // 
            this.Label_FwhmRangeHigh.AutoSize = true;
            this.Label_FwhmRangeHigh.Location = new System.Drawing.Point(110, 48);
            this.Label_FwhmRangeHigh.Name = "Label_FwhmRangeHigh";
            this.Label_FwhmRangeHigh.Size = new System.Drawing.Size(29, 13);
            this.Label_FwhmRangeHigh.TabIndex = 14;
            this.Label_FwhmRangeHigh.Text = "High";
            this.Label_FwhmRangeHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Label_FwhmPercent
            // 
            this.Label_FwhmPercent.AutoSize = true;
            this.Label_FwhmPercent.Location = new System.Drawing.Point(53, 60);
            this.Label_FwhmPercent.Name = "Label_FwhmPercent";
            this.Label_FwhmPercent.Size = new System.Drawing.Size(44, 13);
            this.Label_FwhmPercent.TabIndex = 0;
            this.Label_FwhmPercent.Text = "Percent";
            // 
            // TextBox_FwhmPercent
            // 
            this.TextBox_FwhmPercent.Location = new System.Drawing.Point(18, 56);
            this.TextBox_FwhmPercent.Name = "TextBox_FwhmPercent";
            this.TextBox_FwhmPercent.Size = new System.Drawing.Size(33, 20);
            this.TextBox_FwhmPercent.TabIndex = 1;
            this.TextBox_FwhmPercent.Text = "75";
            this.TextBox_FwhmPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmPercent.TextChanged += new System.EventHandler(this.TextBox_FwhmPercent_TextChanged);
            // 
            // TextBox_FwhmRangeHigh
            // 
            this.TextBox_FwhmRangeHigh.Location = new System.Drawing.Point(141, 44);
            this.TextBox_FwhmRangeHigh.Name = "TextBox_FwhmRangeHigh";
            this.TextBox_FwhmRangeHigh.Size = new System.Drawing.Size(44, 20);
            this.TextBox_FwhmRangeHigh.TabIndex = 2;
            this.TextBox_FwhmRangeHigh.Text = "1.0";
            this.TextBox_FwhmRangeHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmRangeHigh.TextChanged += new System.EventHandler(this.TextBox_FwhmRangeHigh_TextChanged);
            // 
            // TextBox_FwhmRangeLow
            // 
            this.TextBox_FwhmRangeLow.Location = new System.Drawing.Point(141, 70);
            this.TextBox_FwhmRangeLow.Name = "TextBox_FwhmRangeLow";
            this.TextBox_FwhmRangeLow.Size = new System.Drawing.Size(44, 20);
            this.TextBox_FwhmRangeLow.TabIndex = 3;
            this.TextBox_FwhmRangeLow.Text = "0.0";
            this.TextBox_FwhmRangeLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TextBox_FwhmRangeLow.TextChanged += new System.EventHandler(this.TextBox_FwhmRangeLow_TextChanged);
            // 
            // Label_FwhmStdDev
            // 
            this.Label_FwhmStdDev.AutoSize = true;
            this.Label_FwhmStdDev.Location = new System.Drawing.Point(102, 26);
            this.Label_FwhmStdDev.Name = "Label_FwhmStdDev";
            this.Label_FwhmStdDev.Size = new System.Drawing.Size(49, 13);
            this.Label_FwhmStdDev.TabIndex = 5;
            this.Label_FwhmStdDev.Text = "StdDev: ";
            // 
            // Label_Task
            // 
            this.Label_Task.AutoSize = true;
            this.Label_Task.Location = new System.Drawing.Point(274, 45);
            this.Label_Task.Name = "Label_Task";
            this.Label_Task.Size = new System.Drawing.Size(35, 13);
            this.Label_Task.TabIndex = 12;
            this.Label_Task.Text = "label1";
            // 
            // Label_TempratureCompensation
            // 
            this.Label_TempratureCompensation.AutoSize = true;
            this.Label_TempratureCompensation.Location = new System.Drawing.Point(274, 68);
            this.Label_TempratureCompensation.Name = "Label_TempratureCompensation";
            this.Label_TempratureCompensation.Size = new System.Drawing.Size(13, 13);
            this.Label_TempratureCompensation.TabIndex = 13;
            this.Label_TempratureCompensation.Text = "L";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(422, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "SubFrame Overhead:";
            // 
            // Label_OverheadPercent
            // 
            this.Label_OverheadPercent.AutoSize = true;
            this.Label_OverheadPercent.Location = new System.Drawing.Point(529, 45);
            this.Label_OverheadPercent.Name = "Label_OverheadPercent";
            this.Label_OverheadPercent.Size = new System.Drawing.Size(27, 13);
            this.Label_OverheadPercent.TabIndex = 15;
            this.Label_OverheadPercent.Text = "N/A";
            // 
            // Label_OPercent
            // 
            this.Label_OPercent.AutoSize = true;
            this.Label_OPercent.Location = new System.Drawing.Point(553, 45);
            this.Label_OPercent.Name = "Label_OPercent";
            this.Label_OPercent.Size = new System.Drawing.Size(15, 13);
            this.Label_OPercent.TabIndex = 16;
            this.Label_OPercent.Text = "%";
            // 
            // Label_OSeconds
            // 
            this.Label_OSeconds.AutoSize = true;
            this.Label_OSeconds.Location = new System.Drawing.Point(575, 45);
            this.Label_OSeconds.Name = "Label_OSeconds";
            this.Label_OSeconds.Size = new System.Drawing.Size(86, 13);
            this.Label_OSeconds.TabIndex = 17;
            this.Label_OSeconds.Text = "Seconds/Frame:";
            // 
            // Label_OverheadSeconds
            // 
            this.Label_OverheadSeconds.AutoSize = true;
            this.Label_OverheadSeconds.Location = new System.Drawing.Point(659, 45);
            this.Label_OverheadSeconds.Name = "Label_OverheadSeconds";
            this.Label_OverheadSeconds.Size = new System.Drawing.Size(27, 13);
            this.Label_OverheadSeconds.TabIndex = 18;
            this.Label_OverheadSeconds.Text = "N/A";
            // 
            // GroupBox_WeightCalculations
            // 
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_StarResidual);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_FwhmWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_NoiseRatioWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_StarResidualMeanDeviation);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_EccentricityWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_FwhmMeanDeviationWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_MedianMeanDeviationWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_NoiseWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_MedianWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_EccentricityMeanDeviationWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_SnrWeight);
            this.GroupBox_WeightCalculations.Controls.Add(this.GroupBox_StarsWeight);
            this.GroupBox_WeightCalculations.Location = new System.Drawing.Point(18, 161);
            this.GroupBox_WeightCalculations.Name = "GroupBox_WeightCalculations";
            this.GroupBox_WeightCalculations.Size = new System.Drawing.Size(648, 450);
            this.GroupBox_WeightCalculations.TabIndex = 27;
            this.GroupBox_WeightCalculations.TabStop = false;
            this.GroupBox_WeightCalculations.Text = "Weight Caluculations";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 901);
            this.Controls.Add(this.Label_OverheadSeconds);
            this.Controls.Add(this.Label_OSeconds);
            this.Controls.Add(this.Label_OPercent);
            this.Controls.Add(this.Label_OverheadPercent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Label_TempratureCompensation);
            this.Controls.Add(this.Label_Task);
            this.Controls.Add(this.GroupBox_WeightsAndStatistics);
            this.Controls.Add(this.ProgressBar_OverAll);
            this.Controls.Add(this.GroupBox_XisfFileUpdate);
            this.Controls.Add(this.GroupBox_DirectorySelection);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "XSIF File Manager";
            this.GroupBox_Order.ResumeLayout(false);
            this.GroupBox_Order.PerformLayout();
            this.GroupBox_XisfFileUpdate.ResumeLayout(false);
            this.GroupBox_SubFrameKeywords.ResumeLayout(false);
            this.GroupBox_SubFrameKeywords.PerformLayout();
            this.GroupBox_DirectorySelection.ResumeLayout(false);
            this.GroupBox_DirectorySelection.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.GroupBox_WeightsAndStatistics.ResumeLayout(false);
            this.GroupBox_InitialRejectionCriteria.ResumeLayout(false);
            this.GroupBox_InitialRejectionCriteria.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Median)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDown_Rejection_Eccentricity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Rejection_FWHM)).EndInit();
            this.GroupBox_StarResidual.ResumeLayout(false);
            this.GroupBox_StarResidual.PerformLayout();
            this.GroupBox_UpdateStatistics.ResumeLayout(false);
            this.GroupBox_UpdateStatistics.PerformLayout();
            this.GroupBox_StarResidualMeanDeviation.ResumeLayout(false);
            this.GroupBox_StarResidualMeanDeviation.PerformLayout();
            this.GroupBox_MedianMeanDeviationWeight.ResumeLayout(false);
            this.GroupBox_MedianMeanDeviationWeight.PerformLayout();
            this.GroupBox_EccentricityMeanDeviationWeight.ResumeLayout(false);
            this.GroupBox_EccentricityMeanDeviationWeight.PerformLayout();
            this.GroupBox_StarsWeight.ResumeLayout(false);
            this.GroupBox_StarsWeight.PerformLayout();
            this.GroupBox_SnrWeight.ResumeLayout(false);
            this.GroupBox_SnrWeight.PerformLayout();
            this.GroupBox_MedianWeight.ResumeLayout(false);
            this.GroupBox_MedianWeight.PerformLayout();
            this.GroupBox_NoiseWeight.ResumeLayout(false);
            this.GroupBox_NoiseWeight.PerformLayout();
            this.GroupBox_FwhmMeanDeviationWeight.ResumeLayout(false);
            this.GroupBox_FwhmMeanDeviationWeight.PerformLayout();
            this.GroupBox_EccentricityWeight.ResumeLayout(false);
            this.GroupBox_EccentricityWeight.PerformLayout();
            this.GroupBox_NoiseRatioWeight.ResumeLayout(false);
            this.GroupBox_NoiseRatioWeight.PerformLayout();
            this.GroupBox_FwhmWeight.ResumeLayout(false);
            this.GroupBox_FwhmWeight.PerformLayout();
            this.GroupBox_WeightCalculations.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Browse;
        private System.Windows.Forms.ProgressBar ProgressBar_OverAll;
        private System.Windows.Forms.CheckBox CheckBox_Recurse;
        private System.Windows.Forms.GroupBox GroupBox_Order;
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
        private System.Windows.Forms.Label Label_FwhmStdDev;
        private System.Windows.Forms.Label Label_FwhmMean;
        private System.Windows.Forms.TextBox TextBox_FwhmRangeLow;
        private System.Windows.Forms.TextBox TextBox_FwhmRangeHigh;
        private System.Windows.Forms.TextBox TextBox_FwhmPercent;
        private System.Windows.Forms.Label Label_FwhmPercent;
        private System.Windows.Forms.GroupBox GroupBox_UpdateStatistics;
        private System.Windows.Forms.Label Label_UpdateStatistics;
        private System.Windows.Forms.TextBox TextBox_UpdateStatisticsRangeHigh;
        private System.Windows.Forms.Label Label_UpdateStatisticsRangeLow;
        private System.Windows.Forms.TextBox TextBox_UpdateStatisticsRangeLow;
        private System.Windows.Forms.Label Label_UpdateStatisticsRangeHigh;
        private System.Windows.Forms.GroupBox GroupBox_MedianMeanDeviationWeight;
        private System.Windows.Forms.Label Label_MedianMeanDeviationRangeLow;
        private System.Windows.Forms.Label Label_MedianMeanDeviationRangeHigh;
        private System.Windows.Forms.Label Label_MedianMeanDeviationMean;
        private System.Windows.Forms.Label Label_MedianMeanDeviationPercent;
        private System.Windows.Forms.Label Label_MedianMeanDeviationStdDev;
        private System.Windows.Forms.TextBox TextBox_MedianMeanDeviationPercent;
        private System.Windows.Forms.TextBox TextBox_MedianMeanDeviationRangeHigh;
        private System.Windows.Forms.TextBox TextBox_MedianMeanDeviationRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_NoiseWeight;
        private System.Windows.Forms.Label Label_NoiseRangeLow;
        private System.Windows.Forms.Label Label_NoiseMean;
        private System.Windows.Forms.Label Label_NoiseRangeHigh;
        private System.Windows.Forms.Label Label_NoisePercent;
        private System.Windows.Forms.TextBox TextBox_NoisePercent;
        private System.Windows.Forms.TextBox TextBox_NoiseRangeHigh;
        private System.Windows.Forms.TextBox TextBox_NoiseRangeLow;
        private System.Windows.Forms.Label Label_NoiseStdDev;
        private System.Windows.Forms.GroupBox GroupBox_EccentricityWeight;
        private System.Windows.Forms.Label Label_EccentricityRangeLow;
        private System.Windows.Forms.Label Label_EccentricityRangeHigh;
        private System.Windows.Forms.Label Label_EccentricyMean;
        private System.Windows.Forms.Label Label_EccentricityPercent;
        private System.Windows.Forms.Label Label_EccentricityStdDev;
        private System.Windows.Forms.TextBox TextBox_EccentricityPercent;
        private System.Windows.Forms.TextBox TextBox_EccentricityRangeHigh;
        private System.Windows.Forms.TextBox TextBox_EccentricityRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_MedianWeight;
        private System.Windows.Forms.Label Label_MedianRangeLow;
        private System.Windows.Forms.Label Label_MedianMean;
        private System.Windows.Forms.Label Label_MedianRangeHigh;
        private System.Windows.Forms.Label Label_MedianPercent;
        private System.Windows.Forms.TextBox TextBox_MedianPercent;
        private System.Windows.Forms.TextBox TextBox_MedianRangeHigh;
        private System.Windows.Forms.TextBox TextBox_MedianRangeLow;
        private System.Windows.Forms.Label Label_MedianStdDev;
        private System.Windows.Forms.GroupBox GroupBox_SnrWeight;
        private System.Windows.Forms.Label Label_SnrRangeLow;
        private System.Windows.Forms.Label Label_SnrMean;
        private System.Windows.Forms.Label Label_SnrRangeHigh;
        private System.Windows.Forms.Label Label_SnrPersent;
        private System.Windows.Forms.TextBox TextBox_SnrPercent;
        private System.Windows.Forms.TextBox TextBox_SnrRangeHigh;
        private System.Windows.Forms.TextBox TextBox_SnrRangeLow;
        private System.Windows.Forms.Label Label_SnrStdDev;
        private System.Windows.Forms.GroupBox GroupBox_FwhmWeight;
        private System.Windows.Forms.Label Label_FwhmRangeLow;
        private System.Windows.Forms.Label Label_FwhmRangeHigh;
        private System.Windows.Forms.Label Label_Task;
        private System.Windows.Forms.GroupBox GroupBox_StarResidual;
        private System.Windows.Forms.Label Label_StarResidualRangeLow;
        private System.Windows.Forms.Label Label_StarResidualRangeHigh;
        private System.Windows.Forms.Label Label_StarResidualMean;
        private System.Windows.Forms.Label Label_StarResidualPercent;
        private System.Windows.Forms.Label Label_StarResidualStdDev;
        private System.Windows.Forms.TextBox TextBox_StarResidualPercent;
        private System.Windows.Forms.TextBox TextBox_StarResidualRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarResidualRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_StarResidualMeanDeviation;
        private System.Windows.Forms.Label Label_StarResidualMeanDevationRangeLow;
        private System.Windows.Forms.Label Label_StarResidualMeanDevationMean;
        private System.Windows.Forms.Label Label_StarResidualMeanDevationRangeHigh;
        private System.Windows.Forms.Label Label_StarResidualMeanDevationPercent;
        private System.Windows.Forms.TextBox TextBox_StarResidualMeanDeviationPercent;
        private System.Windows.Forms.TextBox TextBox_StarResidualMeanDevationRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarResidualMeanDevationRangeLow;
        private System.Windows.Forms.Label Label_StarResidualMeanDevationStdDev;
        private System.Windows.Forms.GroupBox GroupBox_EccentricityMeanDeviationWeight;
        private System.Windows.Forms.Label Label_EccentricityMeanDeviationRangeLow;
        private System.Windows.Forms.Label Label_EccentricityMeanDeviationRangeHigh;
        private System.Windows.Forms.Label Label_EccentricityMeanDeviationMean;
        private System.Windows.Forms.Label Label_EccentricityMeanDeviationPercent;
        private System.Windows.Forms.Label Label_EccentricityMeanDeviationStdDev;
        private System.Windows.Forms.TextBox TextBox_EccentricityMeanDeviationPercent;
        private System.Windows.Forms.TextBox TextBox_EccentricityMeanDeviationRangeHigh;
        private System.Windows.Forms.TextBox TextBox_EccentricityMeanDeviationRangeLow;
        private System.Windows.Forms.GroupBox GroupBox_StarsWeight;
        private System.Windows.Forms.Label Label_StarRangeLow;
        private System.Windows.Forms.Label Label_StarsMean;
        private System.Windows.Forms.Label Label_StarRangeHigh;
        private System.Windows.Forms.Label Label_StarPercent;
        private System.Windows.Forms.TextBox TextBox_StarsPercent;
        private System.Windows.Forms.TextBox TextBox_StarRangeHigh;
        private System.Windows.Forms.TextBox TextBox_StarRangeLow;
        private System.Windows.Forms.Label Label_StarStdDev;
        private System.Windows.Forms.GroupBox GroupBox_FwhmMeanDeviationWeight;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationRangeLow;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationMean;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationRangeHigh;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationPercent;
        private System.Windows.Forms.TextBox TextBox_FwhmMeanDeviationPercent;
        private System.Windows.Forms.TextBox TextBox_FwhmMeanDeviationRangeHigh;
        private System.Windows.Forms.TextBox TextBox_FwhmMeanDeviationRangeLow;
        private System.Windows.Forms.Label Label_FwhmMeanDeviationStdDev;
        private System.Windows.Forms.GroupBox GroupBox_NoiseRatioWeight;
        private System.Windows.Forms.Label Label_NoiseRatioRangeLow;
        private System.Windows.Forms.Label Label_NoiseRatioMean;
        private System.Windows.Forms.Label Label_NoiseRatioRangeHigh;
        private System.Windows.Forms.Label Label_NoiseRatioPercent;
        private System.Windows.Forms.TextBox TextBox_NoiseRatioPercent;
        private System.Windows.Forms.TextBox TextBox_NoiseRatioRangeHigh;
        private System.Windows.Forms.TextBox TextBox_NoiseRatioRangeLow;
        private System.Windows.Forms.Label label_NoiseRatioStdDev;
        private System.Windows.Forms.ProgressBar ProgressBar_XisfFile;
        private System.Windows.Forms.GroupBox GroupBox_InitialRejectionCriteria;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_Weight;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_Index;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_IndexWeight;
        private System.Windows.Forms.RadioButton RadioButton_RenameOrder_WeightIndex;
        private System.Windows.Forms.Label Label_TempratureCompensation;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Eccentricity;
        private System.Windows.Forms.NumericUpDown numericUpDown_Rejection_FWHM;
        private System.Windows.Forms.TextBox TextBox_Rejection_Total;
        private System.Windows.Forms.Label Label_Rejection_Total;
        private System.Windows.Forms.Label Label_Rejection_Eccentricity;
        private System.Windows.Forms.Label Label_Rejection_FWHM;
        private System.Windows.Forms.GroupBox GroupBox_SubFrameKeywords;
        private System.Windows.Forms.RadioButton RadioButton_SubFrameKeyWords_EnableSubFrameData;
        private System.Windows.Forms.RadioButton RadioButton_SubFrameKeywords_Alphabetize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Label_OverheadPercent;
        private System.Windows.Forms.Label Label_OPercent;
        private System.Windows.Forms.Label Label_OSeconds;
        private System.Windows.Forms.Label Label_OverheadSeconds;
        private System.Windows.Forms.CheckBox CheckBox_SubFrameKeywords_UpdateTargetName;
        private System.Windows.Forms.Button Button_Regection_MoveRejected;
        private System.Windows.Forms.NumericUpDown NumericUpDown_Rejection_Median;
        private System.Windows.Forms.Label Label_Rejection_Median;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_CalculateWeights;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_RescaleWeights;
        private System.Windows.Forms.RadioButton RadioButton_SetImageStatistics_KeepWeights;
        private System.Windows.Forms.GroupBox GroupBox_WeightCalculations;
    }
}

