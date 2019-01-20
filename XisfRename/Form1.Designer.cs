namespace XisfRename
{
    partial class Form1
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
            this.RadioButton_SFSWEIGHT = new System.Windows.Forms.RadioButton();
            this.RadioButton_Chronological = new System.Windows.Forms.RadioButton();
            this.Button_Rename = new System.Windows.Forms.Button();
            this.GroupBox_Numbering.SuspendLayout();
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
            this.ProgressBar_OverAll.Location = new System.Drawing.Point(24, 123);
            this.ProgressBar_OverAll.Name = "ProgressBar_OverAll";
            this.ProgressBar_OverAll.Size = new System.Drawing.Size(355, 11);
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
            this.GroupBox_Numbering.Controls.Add(this.RadioButton_SFSWEIGHT);
            this.GroupBox_Numbering.Controls.Add(this.RadioButton_Chronological);
            this.GroupBox_Numbering.Location = new System.Drawing.Point(231, 15);
            this.GroupBox_Numbering.Name = "GroupBox_Numbering";
            this.GroupBox_Numbering.Size = new System.Drawing.Size(106, 79);
            this.GroupBox_Numbering.TabIndex = 3;
            this.GroupBox_Numbering.TabStop = false;
            this.GroupBox_Numbering.Text = "Numbering";
            // 
            // RadioButton_SFSWEIGHT
            // 
            this.RadioButton_SFSWEIGHT.AutoSize = true;
            this.RadioButton_SFSWEIGHT.Checked = true;
            this.RadioButton_SFSWEIGHT.Location = new System.Drawing.Point(7, 48);
            this.RadioButton_SFSWEIGHT.Name = "RadioButton_SFSWEIGHT";
            this.RadioButton_SFSWEIGHT.Size = new System.Drawing.Size(89, 17);
            this.RadioButton_SFSWEIGHT.TabIndex = 1;
            this.RadioButton_SFSWEIGHT.TabStop = true;
            this.RadioButton_SFSWEIGHT.Text = "SFSWEIGHT";
            this.RadioButton_SFSWEIGHT.UseVisualStyleBackColor = true;
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
            // 
            // Button_Rename
            // 
            this.Button_Rename.Location = new System.Drawing.Point(24, 162);
            this.Button_Rename.Name = "Button_Rename";
            this.Button_Rename.Size = new System.Drawing.Size(75, 23);
            this.Button_Rename.TabIndex = 4;
            this.Button_Rename.Text = "Rename";
            this.Button_Rename.UseVisualStyleBackColor = true;
            this.Button_Rename.Click += new System.EventHandler(this.Button_Rename_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 267);
            this.Controls.Add(this.Button_Rename);
            this.Controls.Add(this.GroupBox_Numbering);
            this.Controls.Add(this.CheckBox_Recurse);
            this.Controls.Add(this.ProgressBar_OverAll);
            this.Controls.Add(this.Button_Browse);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SavePersistedStates);
            this.GroupBox_Numbering.ResumeLayout(false);
            this.GroupBox_Numbering.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_Browse;
        private System.Windows.Forms.ProgressBar ProgressBar_OverAll;
        private System.Windows.Forms.CheckBox CheckBox_Recurse;
        private System.Windows.Forms.GroupBox GroupBox_Numbering;
        private System.Windows.Forms.RadioButton RadioButton_SFSWEIGHT;
        private System.Windows.Forms.RadioButton RadioButton_Chronological;
        private System.Windows.Forms.Button Button_Rename;
    }
}

