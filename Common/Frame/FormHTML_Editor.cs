using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TXD.CF
{
    public partial class FormHTML_Editor : TXD.CF.FormDetail
    {
        private bool pv_isTimer处理中 = false;

        public FormHTML_Editor()
        {
            InitializeComponent();
        }

        public string pv_要编辑的内容 = "";
        public IDataObject pv_剪切板内容 = null;
        public bool PV_手工录入 = true;

        private void FormHTML_Editor_Load(object sender, EventArgs e)
        {
        }

        private void FormHTML_Editor_Shown(object sender, EventArgs e)
        {
            // webBrowser1.ScriptErrorsSuppressed = true;

            // Check if the main script used by the HTML page exists
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tinymce\js\tinymce\tinymce.min.js");
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tinymce\js\tinymce\tinymce.min.js")))
            {
                webBrowser1.Url = new Uri(@"file:///" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"tinymce/tinymce.htm").Replace('\\', '/'));
            }
            else
            {
                MessageBox.Show("Could not find the tinyMCE script directory. Please ensure the directory is in the same location as tinymce.htm", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            /*其他地方用到的技术
               string aaaa = @"https://docs.microsoft.com/zh-cn/previous-versions/visualstudio/visual-studio-2008/b0wes9a3(v%3dvs.90)";

            System.Diagnostics.Process.Start("firefox.exe", aaaa);
            //webBrowser1.ShowPrintDialog();
            */
        }

        public object ActiveXInstance
        {
            get { return webBrowser1.ActiveXInstance; }
        }

        /// <summary>
        /// 需要首次显示后才能使用
        /// </summary>
        public string HtmlContent
        {
            get
            {
                string content = string.Empty;
                if (webBrowser1.Document != null)
                {
                    object html = webBrowser1.Document.InvokeScript("GetContent");
                    content = html as string;
                }
                return content;
            }
            set { webBrowser1.Document?.InvokeScript("SetContent", new object[] {value}); }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            timer1.Start();
        }

        private void button_确定_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pv_isTimer处理中)
            {
                return;
            }

            try
            {
                pv_isTimer处理中 = true;
                if (webBrowser1.Document != null
                    && webBrowser1.Document.Body != null
                    && webBrowser1.Document.Body.All["Signer_Already_OK"] != null)
                {
                    string lvAllReadTag = webBrowser1.Document.Body.All["Signer_Already_OK"].GetAttribute("value");
                    if (lvAllReadTag == "InitOK")
                    {
                        this.HtmlContent = pv_要编辑的内容;
                        timer1.Stop();
                    }
                }
                pv_isTimer处理中 = false;
            }
            catch
            {
                pv_isTimer处理中 = false;
                throw;
            }
        }

        /// <summary>
        /// 弹出编辑html对话框。
        /// </summary>
        /// <param name="aInText"></param>
        /// <param name="HTML_result"></param>
        /// <returns></returns>
        public static bool pt_is成功获取html编辑器的结果(string aInText, ref string HTML_result)
        {
            bool lvRet = false;
            FormHTML_Editor lvf = new FormHTML_Editor();
            lvf.pv_要编辑的内容 = aInText;
            if (lvf.ShowDialog() == DialogResult.OK)
            {
                HTML_result = lvf.HtmlContent;
                lvRet = true;
            }
            return lvRet;
        }

        public static bool pt_is自动黏贴出html(IDataObject inData, ref string HTML_result)
        {
            bool lvRet = false;
            FormHTML_Editor lvf = new FormHTML_Editor();
            lvf.pv_剪切板内容 = inData;
            lvf.PV_手工录入 = false;
            if (lvf.ShowDialog() == DialogResult.OK)
            {
                HTML_result = lvf.HtmlContent;
                lvRet = true;
            }
            return lvRet;
        }

        private void button_取消_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}