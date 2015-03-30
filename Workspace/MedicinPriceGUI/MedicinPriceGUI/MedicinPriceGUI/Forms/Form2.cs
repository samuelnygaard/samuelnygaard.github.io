using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MedicinPriceGUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            passwordTextBox.PasswordChar = '\u25CF';
            this.CancelButton = cancelButton;
            this.AcceptButton = okButtom;
            okButtom.Enabled = false;
        }

        private void okButtom_Click(object sender, EventArgs e)
        {
            if (hostTextBox.Text != "" && portTextBox.Text != "")
            {
                Form1.host = hostTextBox.Text;
                Form1.port = portTextBox.Text;
                Form1.host = usernameTextBox.Text;
                Form1.host = passwordTextBox.Text;
                Form1.host = databaseTextBox.Text;
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hostTextBox_TextChanged(object sender, EventArgs e)
        {
            if (hostTextBox.Text != "" && portTextBox.Text != "")
                okButtom.Enabled = true;
        }

        private void portTextBox_TextChanged(object sender, EventArgs e)
        {
            if (hostTextBox.Text != "" && portTextBox.Text != "")
                okButtom.Enabled = true;
        }
    }
}
