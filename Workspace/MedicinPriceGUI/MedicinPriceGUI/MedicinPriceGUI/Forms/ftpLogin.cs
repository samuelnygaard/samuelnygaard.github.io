using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicinPriceGUI.Forms
{
    public partial class ftpLogin : Form
    {
        public ftpLogin()
        {
            InitializeComponent();
            this.AcceptButton = loginButton;
            this.CancelButton = cancelButton;
        }
    }
}
