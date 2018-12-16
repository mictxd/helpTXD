using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

///��Ϣ��ʾ����
///author:carlos
///version 1.0 20071228
///ʹ�÷�ʽ: SMKFramework.Controls.CautionControl cl = new SMKFramework.Controls.CautionControl("����");
///           System.Windows.Forms.DialogResult rs = cl.ShowDialog();
///          if (rs == System.Windows.Forms.DialogResult.OK)
///               Console.WriteLine("����[��");
///          else
///             Console.WriteLine("cancel");
namespace TXD.CF
{
    public partial class FormCautionOne : Form
    {
        private int pv_������������ = 0;
        private bool pv_isCanClose = true;
        public bool pv_isShowed = false;
        public FormCautionOne(String msg)
        {
            InitializeComponent();
            this.txtMessage.Text = msg;
            picInformation.Visible = true;
        }

        public FormCautionOne(String msg, CautionOneResult cautionResult)
        {
            InitializeComponent();
            this.txtMessage.Text = msg;
            switch (cautionResult)
            {
                case CautionOneResult.Error:
                    picError.Visible = true;
                    this.Text = "Error";
                    break;
                case CautionOneResult.Information:
                    picInformation.Visible = true;
                    this.Text = "Infomation";
                    break;
                case CautionOneResult.Warning:
                    picWarning.Visible = true;
                    this.Text = "Warning";
                    break;
            }
        }

        public FormCautionOne( Form hostform)
        {
            InitializeComponent();
            pv_isCanClose = false;
            this.Text = "Infomation";
            this.FormClosing += new  FormClosingEventHandler(FormCautionOne_FormClosing);
        }

        public void pt_showMessage(string msg)
        {
            this.txtMessage.Text = msg;
            pv_������������ = msg.Length % 20;
            if (pv_������������ < 3)
            {
                pv_������������ = 3;
            }
            pv_����ȷ����ť();
            if (!this.Visible)
            {
                this.Visible=true;
            }
            //this.timer1.Interval = 5000;
            this.timer1.Start();
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

        private void FormCautionOne_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!pv_isCanClose)
            {
                this.timer1.Stop();
                e.Cancel = true;
                this.Visible = false;
               
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pv_������������--;
            if(pv_������������<0)
            { this.Close();}
            pv_����ȷ����ť();
        }

        private void pv_����ȷ����ť()
        {
            if(pv_������������>=0)
            { enter.Text = "ȷ����" + pv_������������ + "��";}
            else
            {
                enter.Text = "ȷ��";
            }
        }
    }

    public enum CautionOneResult
    {
        Error,
        Information,
        Question,
        Warning
    }

    static public partial class HelpTXD
    {
        static public void ShowSaveOK()
        {
            FormCautionOne cc1 = new FormCautionOne("����ɹ�.");
            cc1.ShowDialog();
        }

        static public void ShowWarning(string theWarning)
        {
            FormCautionOne cc1 = new FormCautionOne(theWarning, CautionOneResult.Warning);
            cc1.ShowDialog();
        }

        static public void ShowError(string theError)
        {
            FormCautionOne cc1 = new FormCautionOne(theError, CautionOneResult.Error);
            cc1.ShowDialog();
        }

        static public void ShowInfo(string theError)
        {
            FormCautionOne cc1 = new FormCautionOne(theError, CautionOneResult.Information);
            cc1.ShowDialog();
        }
    }
}