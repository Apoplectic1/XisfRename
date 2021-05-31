using System;
using System.Windows.Forms;

namespace XisfFileManager.Forms
{
    public partial class UserInputForm : Form
    {
        public UserInputFormData mData = new UserInputFormData();

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
            this.mData.mGlobalCheckBox = CheckBox_Global.Checked;
            this.mData.mTextBox = this.TextBox_Text.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }

    public class UserInputFormData
    {
        public string mTextBox;
        public bool mGlobalCheckBox;
        public string mFileName;
    }
}
