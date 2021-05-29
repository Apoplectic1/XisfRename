using System;
using System.Windows.Forms;

namespace XisfFileManager.Forms
{
    public partial class UserInputForm : Form
    {
        public string FormReturnValue { get; set; }

        public UserInputForm()
        {
            InitializeComponent();

            Button_OK.DialogResult = DialogResult.OK;
            Button_Cancel.DialogResult = DialogResult.Cancel;
            AcceptButton = Button_OK;
            CancelButton = Button_Cancel;
 
            Label_Text.Text = "Uninitialized";
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            this.FormReturnValue = this.TextBox_Text.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
