using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace TXD.CF
{
    public partial class FormCaution : Form
    {
        public FormCaution(String msg)
        {
            InitializeComponent();
            this.txtMessage.Text = msg;
        }

        private void enter_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void enter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void cancel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }

    static public partial class HelpTXD
    {
        static public bool isDeleteConfirmed()
        {
            FormCaution cc = new FormCaution("ȷ��Ҫɾ��?");
            return (cc.ShowDialog() == DialogResult.OK);
        }

        static public bool isDeleteConfirmed(string msg)
        {
            FormCaution cc = new FormCaution(msg);
            cc.cancel.Focus();
            return (cc.ShowDialog() == DialogResult.OK);
        }

        static public bool isContinue(string msg)
        {
            FormCaution cc = new FormCaution(msg);
            cc.cancel.Focus();
            return (cc.ShowDialog() == DialogResult.OK);
        }
    }
}