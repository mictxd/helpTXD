using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TXD.CF
{
    public partial class DatetimePickerUTXD : UserControl
    {
        public DatetimePickerUTXD()
        {
            InitializeComponent();
        }

        private void DatetimePickerUTXD_Load(object sender, EventArgs e)
        {
            this.Height = this.dateTimePicker1.Height;
            this.Width = this.checkBox1.Width + this.dateTimePicker1.Width;

        }

        private void DatetimePickerUTXD_Resize(object sender, EventArgs e)
        {

            this.dateTimePicker1.Width = this.Width - this.checkBox1.Width;
        }

        public DateTime Value
        {
            get { return this.dateTimePicker1.Value; }
            set
            { try { this.dateTimePicker1.Value = value; } catch { } }
        }
        public bool Checked
        {
            get { return this.checkBox1.Checked; }
            set
            { this.checkBox1.Checked = value;
            this.dateTimePicker1.Enabled = value;
            }
        }
    }
}
