namespace XisfRename
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
            this.Button_Browse = new System.Windows.Forms.Button();
            this.ProgressBar_OverAll = new System.Windows.Forms.ProgressBar();
            this.CheckBox_Recurse = new System.Windows.Forms.CheckBox();
            this.GroupBox_Numbering = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Lable_IndexOrder = new System.Windows.Forms.Label();
            this.RadioButton_IndexFirst = new System.Windows.Forms.RadioButton();
            this.CheckBox_KeepIndex = new System.Windows.Forms.CheckBox();
            this.RadioButton_WeightFirst = new System.Windows.Forms.RadioButton();
            this.RadioButton_SSWEIGHT = new System.Windows.Forms.RadioButton();
            this.RadioButton_Chronological = new System.Windows.Forms.RadioButton();
            this.Button_Rename = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.GroupBox_DirectorySelection = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ComboBox_TargetName = new System.Windows.Forms.ComboBox();
            this.Button_UpdateXisfFiles = new System.Windows.Forms.Button();
            this.Label_TagetName = new System.Windows.Forms.Label();
            this.Label_Task = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTemplateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupBox_SubFrameCSV = new System.Windows.Forms.GroupBox();
            this.Button_ReadCSV = new System.Windows.Forms.Button();
            this.GroupBox_Numbering.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.GroupBox_DirectorySelection.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.GroupBox_SubFrameCSV.SuspendLayout();
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
            this.ProgressBar_OverAll.Location = new System.Drawing.Point(24, 119);
            this.ProgressBar_OverAll.Name = "ProgressBar_OverAll";
            this.ProgressBar_OverAll.Size = new System.Drawing.Size(463, 11);
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
            // GroupBox_Numbering
            // 
            this.GroupBox_Numbering.Controls.Add(this.groupBox1);
            this.GroupBox_Numbering.Controls.Add(this.RadioButton_SSWEIGHT);
            this.GroupBox_Numbering.Controls.Add(this.RadioButton_Chronological);
            this.GroupBox_Numbering.Location = new System.Drawing.Point(118, 19);
            this.GroupBox_Numbering.Name = "GroupBox_Numbering";
            this.GroupBox_Numbering.Size = new System.Drawing.Size(325, 101);
            this.GroupBox_Numbering.TabIndex = 3;
            this.GroupBox_Numbering.TabStop = false;
            this.GroupBox_Numbering.Text = "Numbering";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Lable_IndexOrder);
            this.groupBox1.Controls.Add(this.RadioButton_IndexFirst);
            this.groupBox1.Controls.Add(this.CheckBox_KeepIndex);
            this.groupBox1.Controls.Add(this.RadioButton_WeightFirst);
            this.groupBox1.Location = new System.Drawing.Point(121, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 77);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Index Order";
            // 
            // Lable_IndexOrder
            // 
            this.Lable_IndexOrder.AutoSize = true;
            this.Lable_IndexOrder.Location = new System.Drawing.Point(73, 48);
            this.Lable_IndexOrder.Name = "Lable_IndexOrder";
            this.Lable_IndexOrder.Size = new System.Drawing.Size(28, 13);
            this.Lable_IndexOrder.TabIndex = 5;
            this.Lable_IndexOrder.Text = "then";
            // 
            // RadioButton_IndexFirst
            // 
            this.RadioButton_IndexFirst.AutoSize = true;
            this.RadioButton_IndexFirst.Location = new System.Drawing.Point(104, 46);
            this.RadioButton_IndexFirst.Name = "RadioButton_IndexFirst";
            this.RadioButton_IndexFirst.Size = new System.Drawing.Size(51, 17);
            this.RadioButton_IndexFirst.TabIndex = 4;
            this.RadioButton_IndexFirst.Text = "Index";
            this.RadioButton_IndexFirst.UseVisualStyleBackColor = true;
            this.RadioButton_IndexFirst.CheckedChanged += new System.EventHandler(this.RadioButton_IndexFirst_CheckedChanged);
            // 
            // CheckBox_KeepIndex
            // 
            this.CheckBox_KeepIndex.AutoSize = true;
            this.CheckBox_KeepIndex.Checked = true;
            this.CheckBox_KeepIndex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBox_KeepIndex.Location = new System.Drawing.Point(15, 23);
            this.CheckBox_KeepIndex.Name = "CheckBox_KeepIndex";
            this.CheckBox_KeepIndex.Size = new System.Drawing.Size(80, 17);
            this.CheckBox_KeepIndex.TabIndex = 2;
            this.CheckBox_KeepIndex.Text = "Keep Index";
            this.CheckBox_KeepIndex.UseVisualStyleBackColor = true;
            this.CheckBox_KeepIndex.CheckedChanged += new System.EventHandler(this.CheckBox_KeepIndex_CheckedChanged);
            // 
            // RadioButton_WeightFirst
            // 
            this.RadioButton_WeightFirst.AutoSize = true;
            this.RadioButton_WeightFirst.Checked = true;
            this.RadioButton_WeightFirst.Location = new System.Drawing.Point(15, 46);
            this.RadioButton_WeightFirst.Name = "RadioButton_WeightFirst";
            this.RadioButton_WeightFirst.Size = new System.Drawing.Size(59, 17);
            this.RadioButton_WeightFirst.TabIndex = 3;
            this.RadioButton_WeightFirst.TabStop = true;
            this.RadioButton_WeightFirst.Text = "Weight";
            this.RadioButton_WeightFirst.UseVisualStyleBackColor = true;
            this.RadioButton_WeightFirst.CheckedChanged += new System.EventHandler(this.RadioButton_WeightFirst_CheckedChanged);
            // 
            // RadioButton_SSWEIGHT
            // 
            this.RadioButton_SSWEIGHT.AutoSize = true;
            this.RadioButton_SSWEIGHT.Checked = true;
            this.RadioButton_SSWEIGHT.Location = new System.Drawing.Point(18, 62);
            this.RadioButton_SSWEIGHT.Name = "RadioButton_SSWEIGHT";
            this.RadioButton_SSWEIGHT.Size = new System.Drawing.Size(83, 17);
            this.RadioButton_SSWEIGHT.TabIndex = 1;
            this.RadioButton_SSWEIGHT.TabStop = true;
            this.RadioButton_SSWEIGHT.Text = "SSWEIGHT";
            this.RadioButton_SSWEIGHT.UseVisualStyleBackColor = true;
            this.RadioButton_SSWEIGHT.CheckedChanged += new System.EventHandler(this.RadioButton_SSWEIGHT_CheckedChanged);
            // 
            // RadioButton_Chronological
            // 
            this.RadioButton_Chronological.AutoSize = true;
            this.RadioButton_Chronological.Location = new System.Drawing.Point(18, 37);
            this.RadioButton_Chronological.Name = "RadioButton_Chronological";
            this.RadioButton_Chronological.Size = new System.Drawing.Size(89, 17);
            this.RadioButton_Chronological.TabIndex = 0;
            this.RadioButton_Chronological.Text = "Chronological";
            this.RadioButton_Chronological.UseVisualStyleBackColor = true;
            this.RadioButton_Chronological.CheckedChanged += new System.EventHandler(this.RadioButton_Chronological_CheckedChanged);
            // 
            // Button_Rename
            // 
            this.Button_Rename.Location = new System.Drawing.Point(23, 52);
            this.Button_Rename.Name = "Button_Rename";
            this.Button_Rename.Size = new System.Drawing.Size(75, 23);
            this.Button_Rename.TabIndex = 4;
            this.Button_Rename.Text = "Rename";
            this.Button_Rename.UseVisualStyleBackColor = true;
            this.Button_Rename.Click += new System.EventHandler(this.Button_Rename_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.GroupBox_Numbering);
            this.groupBox2.Controls.Add(this.Button_Rename);
            this.groupBox2.Location = new System.Drawing.Point(24, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(463, 130);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File Rename";
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
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ComboBox_TargetName);
            this.groupBox4.Controls.Add(this.Button_UpdateXisfFiles);
            this.groupBox4.Controls.Add(this.Label_TagetName);
            this.groupBox4.Location = new System.Drawing.Point(24, 313);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(463, 133);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Update XSIF Fits Keywords";
            // 
            // ComboBox_TargetName
            // 
            this.ComboBox_TargetName.AllowDrop = true;
            this.ComboBox_TargetName.FormattingEnabled = true;
            this.ComboBox_TargetName.Location = new System.Drawing.Point(118, 30);
            this.ComboBox_TargetName.Name = "ComboBox_TargetName";
            this.ComboBox_TargetName.Size = new System.Drawing.Size(256, 21);
            this.ComboBox_TargetName.Sorted = true;
            this.ComboBox_TargetName.TabIndex = 5;
            // 
            // Button_UpdateXisfFiles
            // 
            this.Button_UpdateXisfFiles.Location = new System.Drawing.Point(171, 69);
            this.Button_UpdateXisfFiles.Name = "Button_UpdateXisfFiles";
            this.Button_UpdateXisfFiles.Size = new System.Drawing.Size(124, 23);
            this.Button_UpdateXisfFiles.TabIndex = 4;
            this.Button_UpdateXisfFiles.Text = "Update XISF Files";
            this.Button_UpdateXisfFiles.UseVisualStyleBackColor = true;
            this.Button_UpdateXisfFiles.Click += new System.EventHandler(this.Button_Update_Click);
            // 
            // Label_TagetName
            // 
            this.Label_TagetName.AutoSize = true;
            this.Label_TagetName.Location = new System.Drawing.Point(40, 34);
            this.Label_TagetName.Name = "Label_TagetName";
            this.Label_TagetName.Size = new System.Drawing.Size(72, 13);
            this.Label_TagetName.TabIndex = 0;
            this.Label_TagetName.Text = "Target Name:";
            // 
            // Label_Task
            // 
            this.Label_Task.AutoSize = true;
            this.Label_Task.Location = new System.Drawing.Point(126, 146);
            this.Label_Task.Name = "Label_Task";
            this.Label_Task.Size = new System.Drawing.Size(63, 13);
            this.Label_Task.TabIndex = 9;
            this.Label_Task.Text = "Label_Task";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(516, 24);
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
            // GroupBox_SubFrameCSV
            // 
            this.GroupBox_SubFrameCSV.Controls.Add(this.Button_ReadCSV);
            this.GroupBox_SubFrameCSV.Location = new System.Drawing.Point(263, 36);
            this.GroupBox_SubFrameCSV.Name = "GroupBox_SubFrameCSV";
            this.GroupBox_SubFrameCSV.Size = new System.Drawing.Size(234, 66);
            this.GroupBox_SubFrameCSV.TabIndex = 11;
            this.GroupBox_SubFrameCSV.TabStop = false;
            this.GroupBox_SubFrameCSV.Text = "SubFrame CSV";
            // 
            // Button_ReadCSV
            // 
            this.Button_ReadCSV.Location = new System.Drawing.Point(84, 23);
            this.Button_ReadCSV.Name = "Button_ReadCSV";
            this.Button_ReadCSV.Size = new System.Drawing.Size(75, 23);
            this.Button_ReadCSV.TabIndex = 0;
            this.Button_ReadCSV.Text = "Read CSV";
            this.Button_ReadCSV.UseVisualStyleBackColor = true;
            this.Button_ReadCSV.Click += new System.EventHandler(this.Button_ReadCSV_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 458);
            this.Controls.Add(this.GroupBox_SubFrameCSV);
            this.Controls.Add(this.Label_Task);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.ProgressBar_OverAll);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.GroupBox_DirectorySelection);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "XSIF File Rename";
            this.GroupBox_Numbering.ResumeLayout(false);
            this.GroupBox_Numbering.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.GroupBox_DirectorySelection.ResumeLayout(false);
            this.GroupBox_DirectorySelection.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.GroupBox_SubFrameCSV.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Browse;
        private System.Windows.Forms.ProgressBar ProgressBar_OverAll;
        private System.Windows.Forms.CheckBox CheckBox_Recurse;
        private System.Windows.Forms.GroupBox GroupBox_Numbering;
        private System.Windows.Forms.RadioButton RadioButton_SSWEIGHT;
        private System.Windows.Forms.RadioButton RadioButton_Chronological;
        private System.Windows.Forms.Button Button_Rename;
        private System.Windows.Forms.CheckBox CheckBox_KeepIndex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RadioButton_IndexFirst;
        private System.Windows.Forms.RadioButton RadioButton_WeightFirst;
        private System.Windows.Forms.Label Lable_IndexOrder;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox GroupBox_DirectorySelection;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label Label_Task;
        private System.Windows.Forms.Label Label_TagetName;
        private System.Windows.Forms.Button Button_UpdateXisfFiles;
        private System.Windows.Forms.ComboBox ComboBox_TargetName;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTemplateToolStripMenuItem;
        private System.Windows.Forms.GroupBox GroupBox_SubFrameCSV;
        private System.Windows.Forms.Button Button_ReadCSV;
    }
}

