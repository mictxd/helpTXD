using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace TXD.CF
{
    public partial class FormBase : Form
    {
        public int pv_Behavoir = 0;
        public FormMain PV_MsgForm = null;
        public LoginUserWcf pv_loginUser = null;

        public string Pv_func_name = "";

        public FormBase()
        {
            InitializeComponent();
            this.ImeMode = ImeMode.Alpha;
        }

        [Browsable(true), Category("Key"), Description("Return Key Down as Tab Key")]
        public void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void BaseForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (PV_MsgForm != null)
            //{
            //    Message lvmsg = Message.Create(PV_MsgForm.Handle, HelpTXD.WM_USER_FLOWCHART, (IntPtr)0, (IntPtr)0);
            //    PostMessage(lvmsg.HWnd,lvmsg.Msg,lvmsg.WParam,lvmsg.LParam);
        }

        /// <summary>
        /// 使用功能画面，展开参数所限制的内容,新画面将嵌在主画面里
        /// </summary>
        /// <param name="formType">form的类</param>
        /// <param name="dataParams">相关内容的参数</param>
        public void PT_JumpFormInPages(Type formType, Object dataParams, int behavior = 0)
        {
            FormMain lvFormMain = PV_MsgForm as FormMain;

            FormBase lll = lvFormMain.PT_LoadForm(formType, dataParams, behavior);
        }

        /// <summary>
        /// 当前画面 ，接收参数处理过程
        /// </summary>
        /// <param name="theParams">要展现内容的参数</param>
        public virtual void PT_mountParams(Object theParams)
        {
        }


        protected void FormBase_ImeModeChanged(object sender, EventArgs e)
        {
            if ((this.ImeMode != ImeMode.Close) || (this.ImeMode != ImeMode.Off))
            {
                this.ImeMode = ImeMode.OnHalf;
            }
        }

        protected void PT_BroadcastText(string SingleMsg)
        {
            if (PV_MsgForm != null)
            {
                PV_MsgForm.PT_BroadcastText(SingleMsg);
            }
        }

        public void pt_设置最小滑动宽度(Control aMax)
        {
            //Point p = aMax.PointToScreen(new Point(0, 0));
            //p.X += aMax.Width;
            //Point p1 = this.FindForm().PointToScreen(new Point(0, 0));
            //p.X -= p1.X;
            //p.Y -= p1.Y;
            //System.Drawing.Size lvsize = new Size(p.X, 0);
            //this.AutoScrollMinSize = lvsize;
        }
    }
}