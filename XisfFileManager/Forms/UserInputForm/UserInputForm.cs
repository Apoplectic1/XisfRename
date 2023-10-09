using System;
using System.Windows.Forms;

namespace XisfFileManager
{
    public partial class UserInputForm : Form
    {
        public Forms.UserInputForm.UserInputFormData mData;
        public bool GlobalCheckBox { get; set; }
        public string TextBox { get { return mData.TextBox; } set { mData.TextBox = value; } }

        public UserInputForm()
        {
            InitializeComponent();

            mData = new Forms.UserInputForm.UserInputFormData();

            Button_OK.DialogResult = DialogResult.OK;
            Button_Cancel.DialogResult = DialogResult.Cancel;
            AcceptButton = Button_OK;
            CancelButton = Button_Cancel;
 
            Label_EntryText.Text = "Uninitialized";
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            GlobalCheckBox = CheckBox_Global.Checked;
            TextBox = this.TextBox_Text.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
