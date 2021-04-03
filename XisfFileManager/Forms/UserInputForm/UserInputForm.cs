using System;
using System.Windows.Forms;

namespace XisfFileManager.Forms
{
    public partial class UserInputForm : Form
    {
        public UserInputForm()
        {
            InitializeComponent();

            Button_OK.DialogResult = DialogResult.OK;
            Button_Cancel.DialogResult = DialogResult.Cancel;
            AcceptButton = Button_OK;
            CancelButton = Button_Cancel;

            Label_Text.Text = "Uninitialized";
        }

        public event EventHandler DataAvailable;

        protected virtual void OnDataAvailable(EventArgs e)
        {
            EventHandler eh = DataAvailable;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            OnDataAvailable(null);
        }
    }
}
