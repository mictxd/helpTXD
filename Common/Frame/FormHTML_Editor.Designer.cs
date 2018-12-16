namespace TXD.CF
{
    partial class FormHTML_Editor
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button_取消 = new System.Windows.Forms.Button();
            this.button_确定 = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.button_取消);
            this.flowLayoutPanel1.Controls.Add(this.button_确定);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(917, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // button_取消
            // 
            this.button_取消.AutoSize = true;
            this.button_取消.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_取消.Location = new System.Drawing.Point(871, 3);
            this.button_取消.Name = "button_取消";
            this.button_取消.Size = new System.Drawing.Size(43, 23);
            this.button_取消.TabIndex = 1;
            this.button_取消.Text = "取消";
            this.button_取消.UseVisualStyleBackColor = true;
            this.button_取消.Click += new System.EventHandler(this.button_取消_Click);
            // 
            // button_确定
            // 
            this.button_确定.AutoSize = true;
            this.button_确定.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_确定.Location = new System.Drawing.Point(822, 3);
            this.button_确定.Name = "button_确定";
            this.button_确定.Size = new System.Drawing.Size(43, 23);
            this.button_确定.TabIndex = 0;
            this.button_确定.Text = "确定";
            this.button_确定.UseVisualStyleBackColor = true;
            this.button_确定.Click += new System.EventHandler(this.button_确定_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.Location = new System.Drawing.Point(8, 37);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(917, 447);
            this.webBrowser1.TabIndex = 1;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormHTML_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.ClientSize = new System.Drawing.Size(933, 492);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "FormHTML_Editor";
            this.Text = "html格式";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormHTML_Editor_Load);
            this.Shown += new System.EventHandler(this.FormHTML_Editor_Shown);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button_确定;
        private System.Windows.Forms.Button button_取消;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Timer timer1;
    }
}
