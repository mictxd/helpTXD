namespace TXD.CF
{
    partial class FormCautionOne
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCautionOne));
            this.enter = new System.Windows.Forms.Button();
            this.picInformation = new System.Windows.Forms.PictureBox();
            this.picWarning = new System.Windows.Forms.PictureBox();
            this.picError = new System.Windows.Forms.PictureBox();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picInformation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // enter
            // 
            this.enter.Location = new System.Drawing.Point(662, 298);
            this.enter.Margin = new System.Windows.Forms.Padding(4);
            this.enter.Name = "enter";
            this.enter.Size = new System.Drawing.Size(86, 28);
            this.enter.TabIndex = 22;
            this.enter.Text = "确定";
            this.enter.UseVisualStyleBackColor = true;
            this.enter.Click += new System.EventHandler(this.enter_Click);
            this.enter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.enter_KeyDown);
            // 
            // picInformation
            // 
            this.picInformation.Image = ((System.Drawing.Image)(resources.GetObject("picInformation.Image")));
            this.picInformation.Location = new System.Drawing.Point(60, 123);
            this.picInformation.Margin = new System.Windows.Forms.Padding(4);
            this.picInformation.Name = "picInformation";
            this.picInformation.Size = new System.Drawing.Size(48, 48);
            this.picInformation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picInformation.TabIndex = 30;
            this.picInformation.TabStop = false;
            this.picInformation.Visible = false;
            // 
            // picWarning
            // 
            this.picWarning.Image = ((System.Drawing.Image)(resources.GetObject("picWarning.Image")));
            this.picWarning.Location = new System.Drawing.Point(116, 123);
            this.picWarning.Margin = new System.Windows.Forms.Padding(4);
            this.picWarning.Name = "picWarning";
            this.picWarning.Size = new System.Drawing.Size(48, 48);
            this.picWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWarning.TabIndex = 29;
            this.picWarning.TabStop = false;
            this.picWarning.Visible = false;
            // 
            // picError
            // 
            this.picError.Image = ((System.Drawing.Image)(resources.GetObject("picError.Image")));
            this.picError.Location = new System.Drawing.Point(4, 123);
            this.picError.Margin = new System.Windows.Forms.Padding(4);
            this.picError.Name = "picError";
            this.picError.Size = new System.Drawing.Size(48, 48);
            this.picError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picError.TabIndex = 28;
            this.picError.TabStop = false;
            this.picError.Visible = false;
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanel1.SetColumnSpan(this.txtMessage, 2);
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(172, 4);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.txtMessage, 3);
            this.txtMessage.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(576, 286);
            this.txtMessage.TabIndex = 32;
            this.txtMessage.Text = "";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.picError, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.picWarning, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.picInformation, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtMessage, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.enter, 5, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(752, 330);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormCautionOne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 362);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormCautionOne";
            this.Padding = new System.Windows.Forms.Padding(16);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Information";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.picInformation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button enter;
        private System.Windows.Forms.PictureBox picInformation;
        private System.Windows.Forms.PictureBox picWarning;
        private System.Windows.Forms.PictureBox picError;
        private System.Windows.Forms.RichTextBox txtMessage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Timer timer1;
    }
}