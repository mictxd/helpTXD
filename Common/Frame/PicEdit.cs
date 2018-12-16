using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace TXD.CF
{
    public enum PicEditState { psInit, psChanged }

    public partial class PicLoader : System.Windows.Forms.UserControl
    {
        #region Property

        FrmFlowerEditionPurePic frmFlowerEditionPurePic = null;
        //public  MSN.Practices.Framework.DataAccess.IDataAccess dataAccess = null;
        public bool PT_PicDBChange =false;
        private bool JustFromFile = false;
        //private bool JustFromObject = false;
        //private  bool JustClear = false;
        //private string lv_ModeID = "";//花版ID

        [Browsable(true), Category("Appearance"), Description("button panel visible"), DefaultValue(false)]
        public bool PV_isImageOnly
        {
            get { return !this.flowLayoutPanelButton.Visible; }
            set
            {
                this.flowLayoutPanelButton.Visible = !value;
                if (this.flowLayoutPanelButton.Visible)
                {
                    this.groupBox_Pic.Text = "";
                    this.groupBox_Pic.Top = 8;
                    this.groupBox_Pic.Left = 8;
                    this.groupBox_Pic.Width = this.Width - 16;
                    this.groupBox_Pic.Height = this.Height - 16;
                }
                else
                {
                    this.groupBox_Pic.Text = "";
                    this.groupBox_Pic.Top = 0;
                    this.groupBox_Pic.Left = 0;
                    this.groupBox_Pic.Width = this.Width;
                    this.groupBox_Pic.Height = this.Height;
                }
            }
        }


        #endregion

        #region Construct

        public PicLoader()
        {
            InitializeComponent();
        }

        #endregion

        #region System Event

        /// <summary>
        /// 设置加载图片文件对话框的起始目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FlowerEditionPicture_Load(object sender, EventArgs e)
        {
            this.openFileDialog.InitialDirectory = Application.StartupPath;
            //this.getPicSize();
        }

        /// <summary>
        /// 清除图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (this.pictureBox.Image != null)
            {
                this.pictureBox.Image.Dispose();
                this.pictureBox.Image = null;
                this.PT_PicDBChange = true;
            }
            this.labelNoImage.Visible = true;
            this.JustFromFile = false;
        }
        /// <summary>
        /// 显示原大小的图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click(object sender, EventArgs e)
        {
            frmFlowerEditionPurePic = new FrmFlowerEditionPurePic();
            frmFlowerEditionPurePic.pictureBox1.Image = this.pictureBox.Image;
            frmFlowerEditionPurePic.ShowDialog();
            frmFlowerEditionPurePic.Dispose();
        }
        /// <summary>
        /// 使用图像文件更新当前图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {

            //openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "jpg(*.jpg)|*.jpg;*.jpeg|bmp(*.bmp)|*.bmp|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadPicureBoxFromFile(openFileDialog.FileName);
                this.PT_PicDBChange = true;
            }

        }
        /// <summary>
        /// 是否显示图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_CanShow_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_CanShow.Checked)
            {

            }
            else
            {
                //this.pictureBox.Image.Dispose();
                this.pictureBox.Image = null;
            }
        }

        private void groupBox1_SizeChanged(object sender, EventArgs e)
        {
            this.getPicSize();
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 利用数据源的栏位进行图片显示
        /// </summary>
        /// <param name="dr">类似 DataRow[0]</param>
        public void PT_FromObject(Object dr)
        {
            labelNoImage.Visible = true;
            if (dr == null) { return; }
            if (dr.ToString() == "") { return; }
            if (((byte[])dr).Length == 0) { return; }
            MemoryStream ms = new MemoryStream((byte[])dr);
            Image image = Image.FromStream(ms);
            pictureBox.Image = image;
            labelNoImage.Visible = false;
            ms.Close();
            ms.Dispose();
            this.JustFromFile = false;//表示图片是从数据库来的，一旦要保存，需要这个来指示是否更新栏位
            this.PT_PicDBChange = false;
        }

        public void PT_UpdateField(DataRow dataRow, string FieldName)
        {
            if (pictureBox.Image != null)
            {
                if (JustFromFile)
                {
                    FileStream fs = new FileStream(this.openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    byte[] content = new byte[fs.Length];
                    fs.Read(content, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    dataRow[FieldName] = content;
                }
            }
            else
            {
                dataRow[FieldName] = DBNull.Value;
            }
        }


        #endregion

        #region Private Method

        private void LoadPicureBoxFromFile(string pathFile)
        {
            Image image = Image.FromFile(pathFile);
            this.pictureBox.Image = image;
            labelNoImage.Visible = (this.pictureBox.Image == null);
            this.JustFromFile = true;
            this.PT_PicDBChange = true;
        }



        //public void reloadPic(string A_ModelID)
        //{
        //    this.pictureBox.Image.Dispose();
        //    if (!this.checkBox_CanShow.Checked)
        //    {

        //        return;
        //    }

        //    System.Data.IDbDataParameter[] sp;
        //    this.lv_ModeID = A_ModelID;
        //    string sqlStr = "select design from d_design_model where model_ID=@model_ID";
        //    sp = new IDbDataParameter[] 
        //       { 
        //        dataAccess.CreatParameter("@model_ID", DbType.String, 20)
        //       };
        //    sp[0].Value = this.lv_ModeID;
        //    //dataAccess.RunSql()
        //    //IDataReader dr = dataAccess.(CommandType.Text, sqlStr, sp);
        //    //if (dr.Read())
        //    //{
        //    //    MemoryStream ms = new MemoryStream((byte[])dr["design"], true);
        //    //    Image lv_image = Image.FromStream(ms, true);
        //    //    this.pictureBox.Image = lv_image;
        //    //}

        //}

        private void getPicSize()
        {
            int lvParentWidth = this.panel_Pic.Width;
            int lvParentHeight = this.panel_Pic.Height;

            int lvMinLength = lvParentWidth;

            if (lvParentWidth > lvParentHeight)
            {
                lvMinLength = lvParentHeight;
            }
            this.pictureBox.Width = lvMinLength;
            this.pictureBox.Height = lvMinLength;
        }
        #endregion






    }
}