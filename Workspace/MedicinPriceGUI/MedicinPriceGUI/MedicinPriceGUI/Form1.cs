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
    public partial class Form1 : Form
    {
        static string newZipFilePath = "C:\\";

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = newZipFilePath;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog newZipFileDialog = new OpenFileDialog();
            var d = newZipFileDialog;
            d.InitialDirectory = "C:\\";
            d.Filter = "ZIP files (*.zip)|*.zip|All files (*.*)|*.*";
            d.FilterIndex = 1;
            d.RestoreDirectory = true;

            if (d.ShowDialog() == DialogResult.OK)
            {
                newZipFilePath = d.FileName;
                textBox1.Text = newZipFilePath;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            newZipFilePath = Text;
        }
    }
}
