
using System.Drawing;
using System.Windows.Forms;

namespace XisfFileManager.Forms
{
    partial class UserInputForm
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
            this.Button_OK = new System.Windows.Forms.Button();
            this.Button_Cancel = new System.Windows.Forms.Button();
            this.Label_Text = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBox_Text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Button_OK
            // 
            this.Button_OK.Location = new System.Drawing.Point(138, 134);
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(75, 23);
            this.Button_OK.TabIndex = 0;
            this.Button_OK.Text = "OK";
            this.Button_OK.UseVisualStyleBackColor = true;
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Location = new System.Drawing.Point(245, 134);
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(75, 23);
            this.Button_Cancel.TabIndex = 1;
            this.Button_Cancel.Text = "Cancel";
            this.Button_Cancel.UseVisualStyleBackColor = true;
            // 
            // Label_Text
            // 
            this.Label_Text.Location = new System.Drawing.Point(0, 0);
            this.Label_Text.Name = "Label_Text";
            this.Label_Text.Size = new System.Drawing.Size(459, 92);
            this.Label_Text.TabIndex = 2;
            this.Label_Text.Text = "Enter Missing Data";
            this.Label_Text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label2.Location = new System.Drawing.Point(0, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(459, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Values entered will be used for all subsequent image files";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // TextBox_Text
            // 
            this.TextBox_Text.Location = new System.Drawing.Point(138, 67);
            this.TextBox_Text.Name = "TextBox_Text";
            this.TextBox_Text.Size = new System.Drawing.Size(182, 20);
            this.TextBox_Text.TabIndex = 4;
            this.TextBox_Text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // UserInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 197);
            this.Controls.Add(this.TextBox_Text);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Label_Text);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.Button_OK);
            this.Name = "UserInputForm";
            this.Text = "UserInputForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Button_OK;
        private System.Windows.Forms.Button Button_Cancel;
        public System.Windows.Forms.Label Label_Text;
        private System.Windows.Forms.Label label2;
        public TextBox TextBox_Text;
    }
}