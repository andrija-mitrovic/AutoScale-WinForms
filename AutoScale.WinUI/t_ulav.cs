using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Web;
using AutoScale.Library;

namespace AutoScale.WinUI
{
    public partial class t_ulav: Form
    {
        private AutoSize _autoResize;

        public t_ulav()
        {
            InitializeComponent();

            _autoResize = new AutoSize(this);
            this.Resize += T_ulav_Resize;
        }

        private void T_ulav_Resize(object sender, EventArgs e)
        {
            Form form = (Form)sender;
            _autoResize.Resize(form);
        }
    }
}
