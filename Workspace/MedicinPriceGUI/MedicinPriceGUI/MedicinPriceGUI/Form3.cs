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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            this.AcceptButton = loginButton;
            this.CancelButton = cancelButton;
            this.loginButton.Enabled = false;
            this.textBox2.PasswordChar = '\u25CF';
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                loginButton.Enabled = true;
            else
                loginButton.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                loginButton.Enabled = true;
            else
                loginButton.Enabled = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                loginButton.Enabled = true;
            else
                loginButton.Enabled = false;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            {
                Form1.ftpUsername = textBox1.Text;
                Form1.ftpPassword = textBox2.Text;
                if (!textBox3.Text.EndsWith("/"))
                    Form1.ftpServer = textBox3.Text + "/";
                else
                    Form1.ftpServer = textBox3.Text;
                Form1.goON = true;
                this.Close();
            }
        }
    }
}
