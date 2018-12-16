namespace TXD.CF
{
    partial class UC_FORMS
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel_FormIndex = new System.Windows.Forms.FlowLayoutPanel();
            this.button_close = new System.Windows.Forms.Button();
            this.panel_Forms = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel_FormIndex
            // 
            this.flowLayoutPanel_FormIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel_FormIndex.AutoSize = true;
            this.flowLayoutPanel_FormIndex.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel_FormIndex.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel_FormIndex.Name = "flowLayoutPanel_FormIndex";
            this.flowLayoutPanel_FormIndex.Size = new System.Drawing.Size(317, 22);
            this.flowLayoutPanel_FormIndex.TabIndex = 1;
            // 
            // button_close
            // 
            this.button_close.AutoSize = true;
            this.button_close.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button_close.Location = new System.Drawing.Point(326, 3);
            this.button_close.MinimumSize = new System.Drawing.Size(20, 20);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(21, 22);
            this.button_close.TabIndex = 0;
            this.button_close.Text = "X";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // panel_Forms
            // 
            this.panel_Forms.Location = new System.Drawing.Point(89, 179);
            this.panel_Forms.Name = "panel_Forms";
            this.panel_Forms.Size = new System.Drawing.Size(200, 149);
            this.panel_Forms.TabIndex = 1;
            this.panel_Forms.Resize += new System.EventHandler(this.panel_Forms_Resize);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel_FormIndex, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button_close, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(350, 28);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // UC_FORMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel_Forms);
            this.MinimumSize = new System.Drawing.Size(20, 400);
            this.Name = "UC_FORMS";
            this.Size = new System.Drawing.Size(350, 400);
            this.Load += new System.EventHandler(this.UC_FORMS_Load);
            this.Resize += new System.EventHandler(this.UC_FORMS_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_FormIndex;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.Panel panel_Forms;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
