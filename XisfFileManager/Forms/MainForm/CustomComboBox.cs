using System.Drawing;
using System.Windows.Forms;

namespace XisfFileManager.Forms.MainForm
{
    public class CustomComboBox : ComboBox
    {
        private Color outlineColor = Color.Red; // Default outline color

        public CustomComboBox()
        {
            // Set some custom properties if needed
            this.DrawMode = DrawMode.OwnerDrawFixed; // Use custom drawing
            this.DropDownStyle = ComboBoxStyle.DropDownList; // Disable editing of text
            // Set the text color for the displayed text
            this.ForeColor = Color.Red; // Change this to the desired color
        }

        public Color OutlineColor
        {
            get { return outlineColor; }
            set
            {
                outlineColor = value;
                // Force the control to repaint with the new outline color
                this.Invalidate();
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            // Check if it's a valid item index and the combo box is not in design mode
            if (e.Index >= 0 && !DesignMode)
            {
                // Set the text color for list items
                e.DrawBackground();
                string itemText = this.Items[e.Index].ToString();
                Brush textColor = (e.State & DrawItemState.Selected) == DrawItemState.Selected ? SystemBrushes.HighlightText : SystemBrushes.ControlText;
                e.Graphics.DrawString(itemText, this.Font, textColor, e.Bounds);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Change the outline color here
            using (Pen outlinePen = new Pen(outlineColor, 2)) // Use the outline color property
            {
                e.Graphics.DrawRectangle(outlinePen, 0, 0, Width - 1, Height - 1);
            }
        }
    }
}
