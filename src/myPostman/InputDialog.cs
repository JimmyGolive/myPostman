using System;
using System.Drawing;
using System.Windows.Forms;

namespace myPostman
{
    /// <summary>
    /// A simple input dialog to replace Microsoft.VisualBasic.Interaction.InputBox
    /// </summary>
    public class InputDialog : Form
    {
        private Label lblPrompt;
        private TextBox txtInput;
        private Button btnOK;
        private Button btnCancel;

        public string InputValue
        {
            get { return txtInput.Text; }
        }

        public InputDialog(string prompt, string title, string defaultValue)
        {
            InitializeComponents(prompt, title, defaultValue);
        }

        private void InitializeComponents(string prompt, string title, string defaultValue)
        {
            this.Text = title;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(350, 120);

            lblPrompt = new Label();
            lblPrompt.Text = prompt;
            lblPrompt.Location = new Point(12, 15);
            lblPrompt.Size = new Size(326, 20);
            lblPrompt.AutoSize = false;

            txtInput = new TextBox();
            txtInput.Text = defaultValue;
            txtInput.Location = new Point(12, 40);
            txtInput.Size = new Size(326, 20);

            btnOK = new Button();
            btnOK.Text = "確定 / OK";
            btnOK.Location = new Point(182, 75);
            btnOK.Size = new Size(75, 28);
            btnOK.DialogResult = DialogResult.OK;
            this.AcceptButton = btnOK;

            btnCancel = new Button();
            btnCancel.Text = "取消 / Cancel";
            btnCancel.Location = new Point(263, 75);
            btnCancel.Size = new Size(75, 28);
            btnCancel.DialogResult = DialogResult.Cancel;
            this.CancelButton = btnCancel;

            this.Controls.Add(lblPrompt);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
        }

        /// <summary>
        /// Shows an input dialog and returns the input value
        /// </summary>
        /// <param name="prompt">The prompt message</param>
        /// <param name="title">The dialog title</param>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The input value, or empty string if canceled</returns>
        public static string Show(string prompt, string title, string defaultValue)
        {
            using (InputDialog dialog = new InputDialog(prompt, title, defaultValue))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.InputValue;
                }
                return "";
            }
        }
    }
}
