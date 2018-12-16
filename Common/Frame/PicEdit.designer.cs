namespace TXD.CF
{
    partial class PicLoader
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox_Pic = new System.Windows.Forms.GroupBox();
            this.panel_Pic = new System.Windows.Forms.Panel();
            this.labelNoImage = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanelButton = new System.Windows.Forms.FlowLayoutPanel();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.checkBox_CanShow = new System.Windows.Forms.CheckBox();
            this.groupBox_Pic.SuspendLayout();
            this.panel_Pic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.flowLayoutPanelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Pic
            // 
            this.groupBox_Pic.Controls.Add(this.panel_Pic);
            this.groupBox_Pic.Controls.Add(this.flowLayoutPanelButton);
            this.groupBox_Pic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Pic.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Pic.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_Pic.Name = "groupBox_Pic";
            this.groupBox_Pic.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_Pic.Size = new System.Drawing.Size(305, 210);
            this.groupBox_Pic.TabIndex = 3;
            this.groupBox_Pic.TabStop = false;
            this.groupBox_Pic.SizeChanged += new System.EventHandler(this.groupBox1_SizeChanged);
            // 
            // panel_Pic
            // 
            this.panel_Pic.Controls.Add(this.labelNoImage);
            this.panel_Pic.Controls.Add(this.pictureBox);
            this.panel_Pic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Pic.Location = new System.Drawing.Point(2, 16);
            this.panel_Pic.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel_Pic.Name = "panel_Pic";
            this.panel_Pic.Size = new System.Drawing.Size(301, 167);
            this.panel_Pic.TabIndex = 8;
            // 
            // labelNoImage
            // 
            this.labelNoImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNoImage.Location = new System.Drawing.Point(0, 0);
            this.labelNoImage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNoImage.Name = "labelNoImage";
            this.labelNoImage.Size = new System.Drawing.Size(301, 167);
            this.labelNoImage.TabIndex = 7;
            this.labelNoImage.Text = "无照片";
            this.labelNoImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(301, 167);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 6;
            this.pictureBox.TabStop = false;
            this.pictureBox.DoubleClick += new System.EventHandler(this.btnShow_Click);
            // 
            // flowLayoutPanelButton
            // 
            this.flowLayoutPanelButton.Controls.Add(this.btnShow);
            this.flowLayoutPanelButton.Controls.Add(this.btnClear);
            this.flowLayoutPanelButton.Controls.Add(this.btnUpdate);
            this.flowLayoutPanelButton.Controls.Add(this.checkBox_CanShow);
            this.flowLayoutPanelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanelButton.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(2, 183);
            this.flowLayoutPanelButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.flowLayoutPanelButton.Name = "flowLayoutPanelButton";
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(301, 25);
            this.flowLayoutPanelButton.TabIndex = 5;
            // 
            // btnShow
            // 
            this.btnShow.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnShow.AutoSize = true;
            this.btnShow.Location = new System.Drawing.Point(250, 2);
            this.btnShow.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(51, 22);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "细节";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.AutoSize = true;
            this.btnClear.Location = new System.Drawing.Point(205, 2);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(45, 22);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "清除";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnUpdate.AutoSize = true;
            this.btnUpdate.Location = new System.Drawing.Point(166, 2);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(39, 22);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "上传";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // checkBox_CanShow
            // 
            this.checkBox_CanShow.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox_CanShow.AutoSize = true;
            this.checkBox_CanShow.Location = new System.Drawing.Point(92, 5);
            this.checkBox_CanShow.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.checkBox_CanShow.Name = "checkBox_CanShow";
            this.checkBox_CanShow.Size = new System.Drawing.Size(72, 16);
            this.checkBox_CanShow.TabIndex = 3;
            this.checkBox_CanShow.Text = "默认显示";
            this.checkBox_CanShow.UseVisualStyleBackColor = true;
            this.checkBox_CanShow.Visible = false;
            this.checkBox_CanShow.CheckedChanged += new System.EventHandler(this.checkBox_CanShow_CheckedChanged);
            // 
            // PicLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_Pic);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "PicLoader";
            this.Size = new System.Drawing.Size(305, 210);
            this.Load += new System.EventHandler(this.FlowerEditionPicture_Load);
            this.groupBox_Pic.ResumeLayout(false);
            this.panel_Pic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.flowLayoutPanelButton.ResumeLayout(false);
            this.flowLayoutPanelButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButton;
        public System.Windows.Forms.Button btnShow;
        public System.Windows.Forms.Button btnClear;
        public System.Windows.Forms.Button btnUpdate;
        public System.Windows.Forms.CheckBox checkBox_CanShow;
        public System.Windows.Forms.OpenFileDialog openFileDialog;
        public System.Windows.Forms.GroupBox groupBox_Pic;
        public System.Windows.Forms.Label labelNoImage;
        public System.Windows.Forms.Panel panel_Pic;

    }
}
