using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TXD.CF
{
    public partial class UC_FORMS : UserControl
    {
        public FormMain PV_MsgForm = null;
        public List<FormBase> PV_Forms = new List<FormBase>();

        private List<FormBase> pv_nextShow = new List<FormBase>(); //最后一个在最上面显示。

        public UC_FORMS()
        {
            InitializeComponent();
        }

        private void UC_FORMS_Resize(object sender, EventArgs e)
        {
            this.panel_Forms.Top = this.tableLayoutPanel1.Height;
            this.panel_Forms.Width = this.Width;
            this.panel_Forms.Left = 0;
            this.panel_Forms.Height = this.Height - this.panel_Forms.Top;
        }

        private void UC_FORMS_Load(object sender, EventArgs e)
        {
            PV_Forms = new List<FormBase>();
            UC_FORMS_Resize(null, null);
        }

        public void CloseAll()
        {
            flowLayoutPanel_FormIndex.Controls.Clear();
            panel_Forms.Controls.Clear();
            foreach (FormBase lvf in PV_Forms)
            {
                lvf.Close();
            }
            PV_Forms.Clear();
        }

        private bool CheckFormIsExist(Type t, int behavoir, out int index)
        {
            int lvIndex = -1;
            bool lvFound = false;
            while (true)
            {
                lvIndex++;
                if (lvIndex >= PV_Forms.Count)
                {
                    break;
                }
                FormBase lvForm = PV_Forms[lvIndex];
                if (lvForm.GetType() != t)
                {
                    continue;
                }
                if (lvForm.pv_Behavoir != behavoir)
                {
                    continue;
                }
                lvFound = true;
                break;
            }
            if (!lvFound)
            {
                lvIndex = -1;
            }
            index = lvIndex;
            return (lvIndex >= 0);
        }

        public void pt_CloseForm(FormBase aCloseOne)
        {
            int lvIdx = 0;
            while (lvIdx < PV_Forms.Count)
            {
                if (Object.ReferenceEquals(aCloseOne, PV_Forms[lvIdx]))
                {
                    int lvSDX = 0;
                    while (lvSDX < pv_nextShow.Count)
                    {
                        if (Object.ReferenceEquals(aCloseOne, pv_nextShow[lvSDX]))
                        {
                            pv_nextShow.RemoveAt(lvSDX);
                            break;
                        }
                        lvSDX++;
                    }
                    lvSDX = 0;
                    while (lvSDX < flowLayoutPanel_FormIndex.Controls.Count)
                    {
                        if (Object.ReferenceEquals(aCloseOne, (flowLayoutPanel_FormIndex.Controls[lvSDX] as Button).Tag))
                        {
                            flowLayoutPanel_FormIndex.Controls.RemoveAt(lvSDX);
                            break;
                        }
                        lvSDX++;
                    }
                    aCloseOne.Close();
                    PV_Forms.RemoveAt(lvIdx);

                    break;
                }
                lvIdx++;
            }

            pt_让Form可见();
        }

        /// <summary>
        /// 仅仅保证实例化，且根据类型与行为标记，唯一出现。内容与展现。不在这里操作。
        /// </summary>
        /// <param name="t"></param>
        /// <param name="aParam"></param>
        /// <param name="FFuncText"></param>
        /// <param name="behavoir"></param>
        /// <returns></returns>
        public FormBase PT_New_Or_GetOldForm_in_Pages(Type t, string FFuncText, int behavoir = 0)
        {
            //bool lvNewForm = false;
            FormBase f = null;
            int index = -1;
            if (this.CheckFormIsExist(t, behavoir, out index))
            {
                f = PV_Forms[index] as FormBase;
            }
            else
            {
                //MethodInfo method;
                //method
                //     = t.GetMethod("GetForm", BindingFlags.Static | BindingFlags.Public);
                //FormBase f = (FormBase)method.Invoke(null, null);
                Assembly asembly = Assembly.GetExecutingAssembly();
                try
                {
                    asembly = Assembly.Load(t.Namespace);
                }
                catch
                {
                }

                object frmObj = asembly.CreateInstance(t.FullName);
                //lvNewForm = true;
                f = frmObj as FormBase;
                f.pv_Behavoir = behavoir;
                if (!string.IsNullOrEmpty(FFuncText) && (FFuncText.Trim() != ""))
                {
                    f.Text = FFuncText;
                }
                f.FormBorderStyle = FormBorderStyle.None;
                f.TopLevel = false;
                f.MdiParent = null;
                f.Parent = this.panel_Forms;
                f.Top = 0;
                f.Left = 0;
                f.Height = this.panel_Forms.Height;
                f.Width = this.panel_Forms.Width;
                f.PV_MsgForm = this.PV_MsgForm;
                f.Pv_func_name = FFuncText;
            }
            pt_整理顺序(f);
            pt_让Form可见();

            return f;
        }

        private void pt_让Form可见()
        {
            if (pv_nextShow.Count <= 0)
            {
                return;
            }
            FormBase moveTop = pv_nextShow[pv_nextShow.Count - 1];
            foreach (FormBase lvForm in PV_Forms)
            {
                bool lvVisible = Object.ReferenceEquals(moveTop, lvForm);
                if (lvForm.Visible != lvVisible)
                {
                    lvForm.Visible = lvVisible;
                }

                if (lvForm.Visible)
                {
                    lvForm.Top = 0;
                    lvForm.Left = 0;
                    lvForm.Height = this.panel_Forms.Height;
                    lvForm.Width = this.panel_Forms.Width;
                }
            }
        }

        /// <summary>
        /// 重要的是，设置要显示的form
        /// </summary>
        /// <param name="moveTop"></param>
        private void pt_整理顺序(FormBase moveTop)
        {
            // 先加入进去。
            int lvIdx = 0;
            bool lvFound = false;
            while (lvIdx < PV_Forms.Count)
            {
                if (Object.ReferenceEquals(moveTop, PV_Forms[lvIdx]))
                {
                    lvFound = true;
                    break;
                }
                lvIdx++;
            }
            if (!lvFound)
            {
                Button lvBtnForm = new Button();
                lvBtnForm.Text = moveTop.Text;
                lvBtnForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                lvBtnForm.AutoSize = true;
                lvBtnForm.Click += new System.EventHandler(this.button_Idx_Click);
                lvBtnForm.Tag = moveTop;
                flowLayoutPanel_FormIndex.Controls.Add(lvBtnForm);
                lvBtnForm.Visible = true;
                PV_Forms.Add(moveTop);
                flowLayoutPanel_FormIndex.Height = flowLayoutPanel_FormIndex.Height + 1;
                tableLayoutPanel1.Height = tableLayoutPanel1.Height + 1;
            }

            lvFound = false;
            lvIdx = 0;
            while (lvIdx < pv_nextShow.Count)
            {
                if (Object.ReferenceEquals(moveTop, pv_nextShow[lvIdx]))
                {
                    lvFound = true;
                    break;
                }
                lvIdx++;
            }
            if (!lvFound)
            {
                pv_nextShow.Add(moveTop);
            }
            else
            {
                pv_nextShow.RemoveAt(lvIdx);
                pv_nextShow.Add(moveTop);
            }
        }

        private void button_Idx_Click(object sender, EventArgs e)
        {
            FormBase lvf = (sender as Button).Tag as FormBase;
            pt_整理顺序(lvf);
            pt_让Form可见();
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            FormBase lvf = null;
            foreach (FormBase lvform in PV_Forms)
            {
                if (lvform.Visible)
                {
                    lvf = lvform;
                    break;
                }
            }
            if (lvf != null)
            {
                pt_CloseForm(lvf);
            }
        }

        private void panel_Forms_Resize(object sender, EventArgs e)
        {
            if (PV_Forms == null)
            {
                return;
            }
            foreach (FormBase lvForm in PV_Forms)
            {
                if (lvForm.Visible)
                {
                    lvForm.Top = 0;
                    lvForm.Left = 0;
                    lvForm.Height = this.panel_Forms.Height;
                    lvForm.Width = this.panel_Forms.Width;
                    break;
                }
            }
        }
    }
}