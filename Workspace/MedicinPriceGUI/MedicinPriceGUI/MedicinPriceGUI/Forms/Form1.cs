using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace MedicinPriceGUI
{
    public partial class Form1 : Form
    {
        static string path = "C:\\";
        public static string host, port, username, password, database;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = path;
            ReadDataButton.Enabled = false;
            updateFiles.Enabled = false;
            toCSV.Enabled = false;
            statusbarLabel.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void errorHandler(Exception e)
        {
            MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openZipFileDialog = new OpenFileDialog();
            var d = openZipFileDialog;
            d.InitialDirectory = path;
            d.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
            d.FilterIndex = 1;
            d.RestoreDirectory = true;

            if (d.ShowDialog() == DialogResult.OK)
            {
                path = d.FileName.ToString();
                textBox1.Text = path;
                if (path.ToLower().EndsWith("nyeste\\lms.zip"))
                    enableButtons();
                else disableButtons();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            path = textBox1.Text;
            if (path.ToLower().EndsWith("nyeste\\lms.zip"))
            {
                enableButtons();
                statusbarLabel.Text = "";
            }
            else
            {
                disableButtons();
                statusbarLabel.Text = "Path dosent match.";
            }
        }

        private void ReadDataButton_Click(object sender, EventArgs e)
        {
            Util.readData(this, path);                
            toCSV.Enabled = true;
        }

        private void enableButtons()
        {
            ReadDataButton.Enabled = true;
            updateFiles.Enabled = true;
        }

        private void disableButtons()
        {
            ReadDataButton.Enabled = false;
            updateFiles.Enabled = false;
        }

        public void updateProgressBar(int i)
        {
            progressBar1.Value = i;
        }

        public void updateStatusBar(string s)
        {
            statusbarLabel.Text = s;
        }

        private void toCSV_Click(object sender, EventArgs e)
        {
            string initialPath = "C:\\";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            var d = saveFileDialog;
            d.InitialDirectory = "C:\\";
            d.FileName = "data.csv";
            d.Filter = "CSV files (*.csv)|*.csv";
            d.FilterIndex = 1;
            d.RestoreDirectory = true;

            if (d.ShowDialog() == DialogResult.OK)
            {
                initialPath = d.FileName;
                statusbarLabel.Text = "Saving " + initialPath + " ...";
                Util.printToFile(this, initialPath);
            }
        }

        private void dbSettingsButtom_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void updateFiles_Click(object sender, EventArgs e)
        {
            statusbarLabel.Text = "Testing internet connection ...";
            Util.updateFiles(this, path, "mpe00599", "Mayday100");
        }
    }
}
