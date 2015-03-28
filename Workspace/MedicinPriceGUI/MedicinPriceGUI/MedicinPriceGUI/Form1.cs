using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MedicinPriceGUI
{
    public partial class Form1 : Form
    {
        static string zipFilePath = "C:\\";

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = zipFilePath;
            ReadDataButton.Enabled = false;
            updateFiles.Enabled = false;
            //toCSV.Enabled = false;
            statusbarLabel.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog newZipFileDialog = new OpenFileDialog();
            var d = newZipFileDialog;
            d.InitialDirectory = "C:\\";
            d.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
            d.FilterIndex = 1;
            d.RestoreDirectory = true;

            if (d.ShowDialog() == DialogResult.OK)
            {
                zipFilePath = d.FileName.ToString();
                textBox1.Text = zipFilePath;
                if (zipFilePath.Contains("NYESTE\\lms.zip"))
                    enableButtons();
                else disableButtons();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            zipFilePath = textBox1.Text;
            if (zipFilePath.Contains("NYESTE\\lms.zip"))
                enableButtons();
            else disableButtons();
        }

        private void ReadDataButton_Click(object sender, EventArgs e)
        {
            statusbarLabel.Text = "Reading data ...";
            Util.readData(zipFilePath);
            statusbarLabel.Text = "Finish reading data.";
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

        public static void updateProgressBar(int i)
        {
            progressBar1.Value = i;
        }

        private void toCSV_Click(object sender, EventArgs e)
        {
            string path = "C:\\";

            SaveFileDialog CSVpathDialog = new SaveFileDialog();
            var d = CSVpathDialog;
            d.InitialDirectory = "C:\\";
            d.FileName = "data.csv";
            d.Filter = "CSV files (*.csv)|*.csv";
            d.FilterIndex = 1;
            d.RestoreDirectory = true;

            if (d.ShowDialog() == DialogResult.OK)
            {
                path = d.FileName;
                statusbarLabel.Text = "Saving " + path + " ...";
                Thread.Sleep(1000);
                Util.printToFile(path);
                statusbarLabel.Text = "CSV file saved.";
            }
        }
    }
}
