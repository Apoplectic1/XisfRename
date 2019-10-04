namespace XisfRename
{
    partial class Main
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
            this.Label_Task = new System.Windows.Forms.Label();
            this.GroupBox_Numbering.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button_Browse
            // 
            this.Button_Browse.Location = new System.Drawing.Point(24, 12);
            this.Button_Browse.Name = "Button_Browse";
            this.Button_Browse.Size = new System.Drawing.Size(75, 23);
            this.Button_Browse.TabIndex = 0;
            this.Button_Browse.Text = "Browse";
            this.Button_Browse.UseVisualStyleBackColor = true;
            this.Button_Browse.Click += new System.EventHandler(this.Button_Browse_Click);
            // 
            // ProgressBar_OverAll
            // 
            this.ProgressBar_OverAll.Location = new System.Drawing.Point(18, 74);
            this.ProgressBar_OverAll.Name = "ProgressBar_OverAll";
            this.ProgressBar_OverAll.Size = new System.Drawing.Size(449, 11);
            this.ProgressBar_OverAll.TabIndex = 1;
            // 
            // CheckBox_Recurse
            // 
            this.CheckBox_Recurse.AutoSize = true;
            this.CheckBox_Recurse.Location = new System.Drawing.Point(106, 15);
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
            this.GroupBox_Numbering.Location = new System.Drawing.Point(130, 107);
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
            this.Lable_IndexOrder.Location = new System.Drawing.Point(75, 48);
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
            this.RadioButton_SSWEIGHT.Location = new System.Drawing.Point(7, 48);
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
            this.RadioButton_Chronological.Location = new System.Drawing.Point(7, 24);
            this.RadioButton_Chronological.Name = "RadioButton_Chronological";
            this.RadioButton_Chronological.Size = new System.Drawing.Size(89, 17);
            this.RadioButton_Chronological.TabIndex = 0;
            this.RadioButton_Chronological.Text = "Chronological";
            this.RadioButton_Chronological.UseVisualStyleBackColor = true;
            this.RadioButton_Chronological.CheckedChanged += new System.EventHandler(this.RadioButton_Chronological_CheckedChanged);
            // 
            // Button_Rename
            // 
            this.Button_Rename.Location = new System.Drawing.Point(32, 146);
            this.Button_Rename.Name = "Button_Rename";
            this.Button_Rename.Size = new System.Drawing.Size(75, 23);
            this.Button_Rename.TabIndex = 4;
            this.Button_Rename.Text = "Rename";
            this.Button_Rename.UseVisualStyleBackColor = true;
            this.Button_Rename.Click += new System.EventHandler(this.Button_Rename_Click);
            // 
            // Label_Task
            // 
            this.Label_Task.AutoSize = true;
            this.Label_Task.Location = new System.Drawing.Point(18, 55);
            this.Label_Task.Name = "Label_Task";
            this.Label_Task.Size = new System.Drawing.Size(81, 13);
            this.Label_Task.TabIndex = 5;
            this.Label_Task.Text = "Browse for Files";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 225);
            this.Controls.Add(this.Label_Task);
            this.Controls.Add(this.Button_Rename);
            this.Controls.Add(this.GroupBox_Numbering);
            this.Controls.Add(this.CheckBox_Recurse);
            this.Controls.Add(this.ProgressBar_OverAll);
            this.Controls.Add(this.Button_Browse);
            this.Name = "Form1";
            this.Text = "Astro Image Renamer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SavePersistedStates);
            this.GroupBox_Numbering.ResumeLayout(false);
            this.GroupBox_Numbering.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label Label_Task;
        private System.Windows.Forms.CheckBox CheckBox_KeepIndex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RadioButton_IndexFirst;
        private System.Windows.Forms.RadioButton RadioButton_WeightFirst;
        private System.Windows.Forms.Label Lable_IndexOrder;
    }
}

