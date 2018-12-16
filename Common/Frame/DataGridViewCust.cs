using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Data;
namespace TXD.CF
{
    public class DataGridViewCust : DataGridView
    {
        ScrollEventHandler Pv_EH_FootScroll = null;
        DataGridViewColumnEventHandler Pv_EH_ColumnWithdChange = null;
        DataGridViewCellCancelEventHandler Pv_EH_CellBeginEdit = null;
        DataGridViewCellEventHandler Pv_EH_CellEndEdit = null;
        DataGridViewColumnEventHandler Pv_EH_ColumnAdded = null;
        EventHandler Pv_EH_RowHeadersWidthChanged = null;
        EventHandler Pv_EH_SizeChanged = null;
        DataGridViewBindingCompleteEventHandler Pv_EH_DataGridViewBindingCompleteEventHandler = null;
        DataGridViewCellFormattingEventHandler PV_EH_DataGridViewCellFormatting = null;
        private bool PV_isShowZero = true;

        public bool PvIsShowZero
        {
            get { return PV_isShowZero; }
            set
            {
                if (PV_isShowZero != value)
                {
                    if (value)
                    {

                        this.CellFormatting -= PV_EH_DataGridViewCellFormatting;
                    }
                    else
                    {
                        if (PV_EH_DataGridViewCellFormatting == null)
                        {
                            PV_EH_DataGridViewCellFormatting = new DataGridViewCellFormattingEventHandler(DataGridViewCust_CellFormatting);
                        }

                        this.CellFormatting += PV_EH_DataGridViewCellFormatting;

                    }
                }
                PV_isShowZero = value;
            }
        }
        public DataGridViewCust()
            : base()
        {
            //this.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            //this.AllowUserToOrderColumns = true;
            this.Dock = DockStyle.Fill;
            this.ReadOnlyChanged += new EventHandler(DataGridViewCust_ReadOnlyChanged);
            //为了汇总行
            //this.Controls.Add(this.pv_Footer);
            /*
             this.Scroll += new ScrollEventHandler(DataGridViewCust_Scroll);
             this.ColumnWidthChanged += new DataGridViewColumnEventHandler(DataGridViewCust_ColumnWidthChanged);
             this.CellBeginEdit += new DataGridViewCellCancelEventHandler(DataGridViewCust_CellBeginEdit);
             this.CellEndEdit += new DataGridViewCellEventHandler(DataGridViewCust_CellEndEdit);
             this.ColumnAdded += new DataGridViewColumnEventHandler(DataGridViewCust_ColumnAdded);
             this.RowHeadersWidthChanged += new EventHandler(DataGridViewCust_RowHeadersWidthChanged);
             this.SizeChanged += new EventHandler(DataGridViewCust_SizeChanged);
             * */
        }
        /// <summary>
        /// 所有check列的相关信息。
        /// </summary>
        public List<CheckColumTag> PV_checkColumnsTag;
        public List<DataGridViewFooterTag> PV_FooterColTag = new List<DataGridViewFooterTag>();
        public bool PV_IsHeaderCheckBoxClicked;

        /// <summary>
        /// 汇总区
        /// </summary>
        private DataGridViewFooter pv_Footer = null;
        /// <summary>
        /// 获取和设置是否包含汇总区
        /// </summary>
        public bool PV_HasFooter
        {
            get
            {
                return pv_Footer != null;
            }
            set
            {
                //pv_Footer.Visible = value;
                if (value)
                {

                    if (pv_Footer == null)
                    {
                        pv_Footer = new DataGridViewFooter(this);
                        this.Controls.Add(pv_Footer);
                        //初始化col关联  可以单独做 也可放在这里执行
                        //pv_Footer.pt
                        pv_Footer.InitFooter();
                        pv_Footer.Visible = true;

                        //初始化事件处理
                        if (Pv_EH_FootScroll == null)
                        { Pv_EH_FootScroll = new ScrollEventHandler(DataGridViewCust_Scroll); }
                        this.Scroll += Pv_EH_FootScroll;
                        if (Pv_EH_ColumnWithdChange == null)
                        { Pv_EH_ColumnWithdChange = new DataGridViewColumnEventHandler(DataGridViewCust_ColumnWidthChanged); }
                        this.ColumnWidthChanged += Pv_EH_ColumnWithdChange;
                        if (Pv_EH_CellBeginEdit == null)
                        { Pv_EH_CellBeginEdit = new DataGridViewCellCancelEventHandler(DataGridViewCust_CellBeginEdit); }
                        this.CellBeginEdit += Pv_EH_CellBeginEdit;
                        if (Pv_EH_CellEndEdit == null)
                        { Pv_EH_CellEndEdit = new DataGridViewCellEventHandler(DataGridViewCust_CellEndEdit); }
                        this.CellEndEdit += Pv_EH_CellEndEdit;
                        //if (Pv_EH_ColumnAdded == null)
                        //{ Pv_EH_ColumnAdded = new DataGridViewColumnEventHandler(DataGridViewCust_ColumnAdded); }
                        this.ColumnAdded += Pv_EH_ColumnAdded;
                        if (Pv_EH_RowHeadersWidthChanged == null)
                        { Pv_EH_RowHeadersWidthChanged = new EventHandler(DataGridViewCust_RowHeadersWidthChanged); }
                        this.RowHeadersWidthChanged += Pv_EH_RowHeadersWidthChanged;
                        if (Pv_EH_SizeChanged == null)
                        { Pv_EH_SizeChanged = new EventHandler(DataGridViewCust_SizeChanged); }
                        this.SizeChanged += Pv_EH_SizeChanged;
                        if (Pv_EH_DataGridViewBindingCompleteEventHandler == null)
                        { Pv_EH_DataGridViewBindingCompleteEventHandler = new DataGridViewBindingCompleteEventHandler(DataGridViewCust_DataBindingComplete); }
                        this.DataBindingComplete += Pv_EH_DataGridViewBindingCompleteEventHandler;
                        RefreshFooter();
                    }
                }
                else
                {
                    if (pv_Footer != null)
                    {
                        pv_Footer.Visible = false;
                        if (Pv_EH_FootScroll != null)
                        { this.Scroll -= Pv_EH_FootScroll; }
                        this.ColumnWidthChanged -= Pv_EH_ColumnWithdChange;
                        this.CellBeginEdit -= Pv_EH_CellBeginEdit;
                        this.CellEndEdit -= Pv_EH_CellEndEdit;
                        this.ColumnAdded -= Pv_EH_ColumnAdded;
                        this.RowHeadersWidthChanged -= Pv_EH_RowHeadersWidthChanged;
                        this.SizeChanged -= Pv_EH_SizeChanged;
                        this.DataBindingComplete -= Pv_EH_DataGridViewBindingCompleteEventHandler;
                    }
                }
            }
        }
        /// <summary>
        /// 数据绑定后计算汇总区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewCust_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.pv_Footer.pt_ReCaculate();
        }
        /// <summary>
        /// 汇总区显示变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewCust_SizeChanged(object sender, EventArgs e)
        {
            RefreshFooter();
        }
        /// <summary>
        /// 汇总去跟着变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewCust_RowHeadersWidthChanged(object sender, EventArgs e)
        {
            //pv_Footer.RowHeaderWidth = this.RowHeadersWidth;
            RefreshFooter();
        }

        //void DataGridViewCust_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        //{
        //    pv_Footer.Add(new DataGridViewFooterCell(DataGridViewFooterSumMode.None));

        //    RefreshFooter();
        //}
        /// <summary>
        /// 进入编辑状态前，该CELL的值。
        /// </summary>
        private object cellTempValue = 0;
        /// <summary>
        /// 可能影响到汇总区的计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewCust_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            cellTempValue = this[e.ColumnIndex, e.RowIndex].Value;
        }
        /// <summary>
        /// 完成编辑后，重算汇总区对应.目前只做sum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewCust_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((this[e.ColumnIndex, e.RowIndex].Value != cellTempValue) && (pv_Footer[e.ColumnIndex] != null))
            {
                //由于未知原因，提交的数据不能更新到当前值，所以，所有汇总行刷新一遍。

                //DataGridViewFooterCell lvc = null;
                //lvc = pv_Footer[e.ColumnIndex];
                //if (lvc.PV_SumMode == DGVCFooterCellStyle.Sum)
                //{
                //    decimal v = 0, t = 0;
                //    try { t = Convert.ToDecimal(cellTempValue); }
                //    catch { }
                //    try { v = Convert.ToDecimal(this[e.ColumnIndex, e.RowIndex].Value); }
                //    catch { }
                //    decimal org = 0;
                //    try { org = Convert.ToDecimal(lvc.Text); }
                //    catch { }
                //    lvc.Text = (org - t + v).ToString((sender as DataGridViewCust).Columns[e.ColumnIndex].DefaultCellStyle.Format);
                //}

                this.pv_Footer.pt_ReCaculate();
                cellTempValue = 0;
            }
        }
        /// <summary>
        /// 汇总区跟着变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewCust_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                pv_Footer[e.Column.Index].Width = e.Column.Width;
            }
            catch { }
            RefreshFooter();
        }

        private int BorderWidth
        {
            get { return 1; }
        }
        /// <summary>
        /// 汇总行的大小跟位置
        /// </summary>
        private void RefreshFooter()
        {
            if (pv_Footer == null) { return; }
            Rectangle rect = new Rectangle();
            if (HorizontalScrollBar.Visible)
            {
                rect.X = BorderWidth;

                rect.Y = this.Height - pv_Footer.Height - HorizontalScrollBar.Height - BorderWidth;
                rect.Width = this.Width - BorderWidth * 2;
                rect.Height = pv_Footer.Height;
            }
            else
            {
               
                rect.X = BorderWidth;
                rect.Y = this.Height - pv_Footer.Height - BorderWidth;
                rect.Width = this.Width - BorderWidth * 2;
                rect.Height = pv_Footer.Height;
            }
            pv_Footer.LayoutControl(rect);
            pv_Footer.Refresh();
        }
        /// <summary>
        /// 相应横滚，汇总区变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataGridViewCust_Scroll(object sender, ScrollEventArgs e)
        {
            pv_Footer.ScrollWithParent(this.HorizontalScrollBar.Value);
        }
        /// <summary>
        /// 通过grid的变化 调用汇总区的布局变化
        /// </summary>
        /// <param name="levent"></param>
        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);
            if (!this.PV_HasFooter) { return; }
            Type t = this.GetType();
            FieldInfo tLayout = t.BaseType.GetField("layout", BindingFlags.NonPublic | BindingFlags.Instance);
            if (tLayout == null)
                return;

            Object layout = tLayout.GetValue(this);

            if (layout == null)
                return;
            Graphics g = this.CreateGraphics();
            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString("合计", this.Font);

            FieldInfo tDataClient = layout.GetType().GetField("Data");
            Rectangle dataClient = (Rectangle)tDataClient.GetValue(layout);

            if (this.ColumnHeadersHeight + (this.HorizontalScrollBar.Visible ? this.HorizontalScrollBar.Height : 0) + dataClient.Height > this.Height - this.HorizontalScrollBar.Height)
            {
                dataClient = new Rectangle(dataClient.X, dataClient.Y, dataClient.Width, dataClient.Height - this.HorizontalScrollBar.Height);
                tDataClient.SetValue(layout, dataClient);
                RefreshFooter();
            }
        }
        /// <summary>
        /// 对于0不显示的设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridViewCust_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (PV_isShowZero) { return; }
            if (e.ColumnIndex < 0 || e.RowIndex < 0) { return; }

            if ((e.Value == null) || (e.Value.ToString() == "") || (e.Value == DBNull.Value)) { return; }
            decimal lvt = 0;
            if (!decimal.TryParse(e.Value.ToString(), out lvt)) { return; }
            if (lvt == 0)
            {
                e.Value = DBNull.Value;
            }
        }
        public void pt_HeadText2ClipBoard()
        {
            string tex = "";
            //DataGridViewColumn lvc = null;
            DataGridViewColumnCollection columnCollection = this.Columns;

            string[] array = new string[this.Columns.Count];
            foreach (DataGridViewColumn column in this.Columns)
            { array[column.DisplayIndex] = column.HeaderText; }
            for (int lvi = 0; lvi < this.Columns.Count; lvi++)
            {

                tex += array[lvi] + "\t";
            }

            Clipboard.SetDataObject(tex);
        }
        public void pt_setHeadCheckBoxVisible()
        {
            if (this.PV_checkColumnsTag == null) { return; }
            CheckColumTag lvcct=null;

            for (int lvi = 0; lvi < this.PV_checkColumnsTag.Count; lvi++)
            {
                lvcct = this.PV_checkColumnsTag[lvi];
                lvcct.HeaderCheckBox.Visible = (this.Columns[lvcct.PV_ColumnIndex].ReadOnly != true) && (this.ReadOnly != true);
            }
        }
        private void DataGridViewCust_ReadOnlyChanged(object sender, EventArgs e)
        {
            pt_setHeadCheckBoxVisible();
        }
    }

    public class DataGridViewFooterTag
    {
        public int ColumnIndex_Footer = -1;
        public string FooterStyle = "";
    }
    /// <summary>
    /// 汇总区的panel实现。
    /// </summary>
    public class DataGridViewFooter : Panel
    {
        int pv_FrozenCount = 0;
        /// <summary>
        /// 代表冻结列对应的区域
        /// </summary>
        private Panel pv_PanelFrozen = new Panel();
        /// <summary>
        /// 代表非冻结列对应的区域
        /// </summary>
        private Panel pv_PanelScroll = new Panel();
        //internal ListView _LeftView = new ListView();
        //internal ListView _RightView = new ListView();
        //private int pv_FrozenCount = 0;
        /// <summary>
        /// 对应datagridview
        /// </summary>
        private DataGridViewCust pv_DataGridViewCust = null;
        //public void Add()
        //{

        //}
        //   public DataGridFooterColumnCollection Columns = null;
        /// <summary>
        /// 汇总区，对应grid汇总列的格子们。
        /// </summary>
        List<DataGridViewFooterCell> Cells = new List<DataGridViewFooterCell>();
        /// <summary>
        /// 汇总区带的汇总格子，按索引来查
        /// </summary>
        /// <param name="ColumnsIndex">索引是grid的column的index值</param>
        /// <returns>汇总数据格子。有可能为空（如果grid该列没有汇总要求）</returns>
        public DataGridViewFooterCell this[int ColumnsIndex]
        {
            get
            {
                DataGridViewFooterCell lvcell = null;
                foreach (DataGridViewFooterCell lvc in Cells)
                {
                    if (ColumnsIndex == lvc.pv_ColumnIndex)
                    { lvcell = lvc; }
                }
                return lvcell;
            }
        }
        /// <summary>
        /// 对应的datagridview.暂时用自定义的。
        /// </summary>
        public DataGridViewCust DataGridViewGroup
        {
            get { return pv_DataGridViewCust; }
            set { pv_DataGridViewCust = value; }
        }

        //public string RowHeaderText
        //{
        //    get { return this._LeftView.Items[0].SubItems[0].Text; }
        //    set
        //    {
        //        this._LeftView.Items[0].SubItems[0].Text = value;
        //        this._RightView.Items[0].SubItems[0].Text = value;
        //    }
        //}

        //public int RowHeaderWidth
        //{
        //    get { return this._LeftView.Columns[0].Width; }
        //    set
        //    {
        //        this._LeftView.Columns[0].Width = value;
        //        this._RightView.Columns[0].Width = value;
        //    }
        //}
        /// <summary>
        /// 构造汇总区时，要传入被汇总的gridview
        /// </summary>
        /// <param name="dgvGroup"></param>
        public DataGridViewFooter(DataGridViewCust dgvGroup)
            : base()
        {

            this.pv_DataGridViewCust = dgvGroup;
            //this.Columns = new DataGridFooterColumnCollection();
            //this.Visible = false;
            //this._ImgList.ImageSize = new Size(1, 18);
            //this.Columns = new DataGridFooterColumnCollection(this);

            this.BorderStyle = BorderStyle.None;
            this.pv_PanelFrozen.BorderStyle = BorderStyle.None;
            this.pv_PanelFrozen.BackColor = SystemColors.GradientActiveCaption;
            this.pv_PanelScroll.BorderStyle = BorderStyle.None;
            this.pv_PanelScroll.BackColor = SystemColors.AppWorkspace;
            //this._LeftView.BorderStyle = BorderStyle.None;
            //this._RightView.BorderStyle = BorderStyle.None;

            this.SuspendLayout();
            Graphics g = this.CreateGraphics();
            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString("合计", this.Font);
            this.Height = (int)stringSize.Height + 2;
            //
            //pv_PanelFrozen
            //
            this.pv_PanelFrozen.Margin = new Padding(0);
            this.pv_PanelFrozen.Height = this.Height;

            //
            //pv_PanelScroll
            //
            this.pv_PanelScroll.Margin = new Padding(0);
            this.pv_PanelScroll.Height = this.Height;

            this.Controls.Add(pv_PanelFrozen);
            this.Controls.Add(pv_PanelScroll);

            this.ResumeLayout(false);
            this.ParentChanged += new EventHandler(DataGridFooter_ParentChanged);
        }
        /// <summary>
        /// 汇总行 对应grid的列进行控件初始化。产生对象，且计算值。
        /// </summary>
        public void InitFooter()
        {
            DataGridViewFooterCell cellFooter = null;
            this.Cells.Clear();
            bool frozen = false;
            
            foreach (DataGridViewFooterTag lvFTag in pv_DataGridViewCust.PV_FooterColTag)
            {
                cellFooter = null;
                frozen = pv_DataGridViewCust.Columns[lvFTag.ColumnIndex_Footer].Frozen;
                if (lvFTag.FooterStyle == DGVCFooterCellStyle.None) { cellFooter = new DataGridViewFooterCell(DGVCFooterCellStyle.None, lvFTag.ColumnIndex_Footer, frozen); }

                if (lvFTag.FooterStyle.ToLower() == DGVCFooterCellStyle.Sum.ToLower())
                {
                    cellFooter = new DataGridViewFooterCell(DGVCFooterCellStyle.Sum, lvFTag.ColumnIndex_Footer, frozen);
                }
                if (cellFooter != null)
                {
                    if (frozen) { this.pv_PanelFrozen.Controls.Add(cellFooter); }
                    else { this.pv_PanelScroll.Controls.Add(cellFooter); }
                    this.Cells.Add(cellFooter);
                }
            }
            this.pt_ReCaculate();

            //this.ResumeLayout(false);
        }
        void DataGridFooter_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent != null && !(this.Parent is DataGridViewCust))
                throw new Exception("不能将DataGridView添加到非DataGridViewCust控件之中");
            pv_DataGridViewCust = (DataGridViewCust)this.Parent;
        }
        /// <summary>
        /// 主要是横滚。有点小问题，最右下角会遮住竖滚条
        /// </summary>
        /// <param name="scrollValue"></param>
        internal void ScrollWithParent(int scrollValue)
        {
            int leftw = pv_DataGridViewCust.RowHeadersVisible ? pv_DataGridViewCust.RowHeadersWidth : 0;
            for (int i = 0; i < this.pv_DataGridViewCust.Columns.Count; i++)
            {
                if (this.pv_DataGridViewCust.Columns[i].Frozen)//对应列冻结 
                {
                    //pv_FrozenCount++;
                    leftw += this.pv_DataGridViewCust.Columns[i].Width;
                }
            }
            this.pv_PanelScroll.Left = leftw - scrollValue + 1;
        }
        /// <summary>
        /// 等着外部OnLayout调用
        /// </summary>
        /// <param name="rect"></param>
        public void LayoutControl(Rectangle rect)
        {
            DataGridViewFooterCell lvc = null;
            pv_FrozenCount = 0;//冻结列数量
            //最左边其实点
            int MostLeft = pv_DataGridViewCust.RowHeadersVisible ? pv_DataGridViewCust.RowHeadersWidth : 0;

            int leftw = MostLeft;
            int sumr = 0;
            for (int i = 0; i < this.pv_DataGridViewCust.Columns.Count; i++)
            {
                if (this.pv_DataGridViewCust.Columns[i].Frozen)//对应列冻结 
                {
                    pv_FrozenCount++;
                    leftw += this.pv_DataGridViewCust.Columns[i].Width;

                }
                else { sumr += this.pv_DataGridViewCust.Columns[i].Width; }
                lvc = this[i];
                if (lvc == null) { continue; }
                lvc.Width = this.pv_DataGridViewCust.Columns[i].Width - 3;
                if (this.pv_DataGridViewCust.Columns[i].Frozen)//对应列冻结 
                {
                    lvc.Left = this.pv_DataGridViewCust.GetCellDisplayRectangle(i, -1, false).Left - 2;
                    //leftw += this.pv_DataGridViewCust.Columns[lvib].Width;
                }
                else
                { lvc.Left = this.pv_DataGridViewCust.GetCellDisplayRectangle(i, -1, false).Left - leftw - 2; }
            }

            //冻结列

            ////this.RowHeaderWidth = this.pv_DataGridViewCust.RowHeadersWidth - 3;
            //int sumw = 0;

            //for (int lvib = 0;  lvib < this.pv_DataGridViewCust.Columns.Count; lvib++)
            //{
            //    sumw += this.pv_DataGridViewCust.Columns[lvib].Width;
            //    lvc= this[lvib];
            //    if (lvc==null){continue;}
            //    lvc.Width = this.pv_DataGridViewCust.Columns[lvib].Width;

            //}
            ////sumw += RowHeaderWidth;

            ////this._LeftView.Width = this._RightView.Width = sumw;

            //for (int lvib = pv_DataGridViewCust.Columns.Count - 1; lvib >= 0; lvib--)
            //{
            //    if (pv_DataGridViewCust.Columns[lvib].Frozen)
            //    {
            //        pv_FrozenCount = ++lvib;
            //        break;
            //    }
            //}
            ////最左侧行标题列可见时，要注意

            //if (pv_FrozenCount > 0)
            //{
            //    for (int lvib = 0; lvib < pv_FrozenCount; lvib++)
            //        leftw += pv_DataGridViewCust.Columns[lvib].Width;
            //}
            pv_PanelFrozen.Width = leftw;
            pv_PanelFrozen.Left = rect.Left;

            pv_PanelScroll.Left = leftw + rect.Left;
            pv_PanelScroll.Width = sumr;// rect.Width - leftw;

            //_RightView.Left = -(pv_DataGridViewCust.RowHeadersVisible ? pv_DataGridViewCust.RowHeadersWidth : 0) + 1;


            this.Width = rect.Width;
            this.Top = rect.Top;
            this.Left = rect.Left;

        }
        /// <summary>
        /// 计算所有汇总列的汇总值。目前支持sum avg count。仅对绑定数据源时有效。
        /// </summary>
        public void pt_ReCaculate()
        {

            foreach (DataGridViewFooterCell dgvfc in Cells)
            { dgvfc.Text = "0"; }
            DataTable lvdt = null;
            string lvFilter = "";
            if (pv_DataGridViewCust.DataSource == null) { return; }
            if (pv_DataGridViewCust.DataSource is DataTable)
            { lvdt = pv_DataGridViewCust.DataSource as DataTable; }
            else if (pv_DataGridViewCust.DataSource is BindingSource)
            {
                BindingSource ll = pv_DataGridViewCust.DataSource as BindingSource;
                try { lvFilter = ll.Filter; }
                catch { }
                if ((ll.DataSource != null) && (ll.DataSource is DataTable))
                {
                    lvdt = ll.DataSource as DataTable;
                }
            }
            if (lvdt != null)
            {
                //decimal lvd = 0;
                string expression = "";
                foreach (DataGridViewFooterCell dgvfc in Cells)
                {
                    try
                    {

                        if (dgvfc.PV_SumMode.ToLower() == DGVCFooterCellStyle.Sum.ToLower())
                        {
                            expression = "Sum(" + lvdt.Columns[pv_DataGridViewCust.Columns[dgvfc.pv_ColumnIndex].DataPropertyName] + ")";
                        }
                        if (dgvfc.PV_SumMode.ToLower() == DGVCFooterCellStyle.Average.ToLower())
                        {
                            expression = "Avg(" + lvdt.Columns[pv_DataGridViewCust.Columns[dgvfc.pv_ColumnIndex].DataPropertyName] + ")";
                        }
                        if (dgvfc.PV_SumMode.ToLower() == DGVCFooterCellStyle.Count.ToLower())
                        {
                            expression = "Count(" + lvdt.Columns[pv_DataGridViewCust.Columns[dgvfc.pv_ColumnIndex].DataPropertyName] + ")";
                        }
                        if (pv_DataGridViewCust.Columns[dgvfc.pv_ColumnIndex].DefaultCellStyle.Format != "")
                        {
                            object lvo = lvdt.Compute(expression, lvFilter);
                            if (lvo != null)
                            {
                                if (lvo is int)
                                {
                                    dgvfc.Text = Convert.ToInt32(lvo).ToString(pv_DataGridViewCust.Columns[dgvfc.pv_ColumnIndex].DefaultCellStyle.Format);
                                }
                                else if (lvo is decimal)
                                {
                                    dgvfc.Text = Convert.ToDecimal(lvo).ToString(pv_DataGridViewCust.Columns[dgvfc.pv_ColumnIndex].DefaultCellStyle.Format);
                                }
                                else { dgvfc.Text = lvo.ToString(); }
                            }
                        }
                        else
                        {
                            dgvfc.Text = lvdt.Compute(expression, lvFilter).ToString();
                        }
                    }
                    catch { }
                }

            }
        }
    }

    /// <summary>
    /// 汇总行的单元格实现
    /// </summary>
    public class DataGridViewFooterCell : TextBox
    {
        private string _SumMode = DGVCFooterCellStyle.None;
        private DataGridViewFooter _DGFooter;
        internal int pv_ColumnIndex = -1;//对应grid的column的index属性 这个是检索grid列的关键属性 
        //internal int pv_DisplayIndex = -1;//对应grid的column的DisplayIndex属性
        internal bool pv_Frozen = false;
        /// <summary>
        /// 汇总计算方法，配置在XML档，所以用字符串。取值参考DGVCFooterCellStyle
        /// </summary>
        public string PV_SumMode
        {
            get { return _SumMode; }
            set
            {
                _SumMode = value;
                if (_SumMode == DGVCFooterCellStyle.Sum || _SumMode == DGVCFooterCellStyle.Count || _SumMode == DGVCFooterCellStyle.Average)
                    this.Text = "0";
            }
        }
        /// <summary>
        /// 所属汇总行
        /// </summary>
        public DataGridViewFooter DGFooter
        {
            get { return _DGFooter; }
            set { _DGFooter = value; }
        }

        public DataGridViewFooterCell()
        {
            this.Top = 1;
            this.BorderStyle = BorderStyle.None;
        }

        public DataGridViewFooterCell(string summode, int ColumnIndex, bool aFrozen)
        {
            //因为太简单了就没调用不带参数的
            this.PV_SumMode = summode;
            pv_ColumnIndex = ColumnIndex;
            //pv_DisplayIndex = DisplayIndex;
            pv_Frozen = aFrozen;
            this.BorderStyle = BorderStyle.None;
            this.Top = 1;
            this.TextAlign = HorizontalAlignment.Right;
            this.ReadOnly = true;
        }
    }


    public class CheckColumTag
    {
        public bool IsHeaderCheckBoxClicked = false;
        public string DataPropertyName;
        public CheckBox HeaderCheckBox;
        public int PV_TotalCheckedCheckBoxes = 0;
        public int PV_ColumnIndex = -1;
        public int TrueValue = Convert.ToInt32(true);
        public int FalseValue = Convert.ToInt32(false);
    }
    public static class DGVCFooterCellStyle
    {
        public const string None = "";
        public const string Sum = "sum";
        public const string Average = "avg";
        public const string Count = "Count";
    }

    /// <summary>
    /// grid相关方法
    /// </summary>
    public static partial class Grid
    {
        /// <summary>
        /// 选择一行check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void dgvSelectAll_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridViewCust)) { return; }
            DataGridViewCust dgvC = sender as DataGridViewCust;
            if (e.RowIndex < 0) { return; }
            if (!dgvC.PV_IsHeaderCheckBoxClicked)
            {
                if (dgvC[e.ColumnIndex, e.RowIndex] is DataGridViewCheckBoxCell)
                {
                    RowCheckBoxClick((DataGridViewCheckBoxCell)dgvC[e.ColumnIndex, e.RowIndex]);
                }
                //dgvC.EndEdit();堆栈溢出  若要更新怎么办？ 溢出原因，因为true false对应的值跟数据源不匹配。导致点check无效，引起check值变化 又回来这个过程。所以，仅仅用标题栏的checkbox后
            }
        }
        /// <summary>
        /// check值变化，需要自行提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void dgvSelectAll_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (!(sender is DataGridViewCust)) { return; }
            DataGridViewCust dgvC = sender as DataGridViewCust;
            if (dgvC.CurrentCell is DataGridViewCheckBoxCell)
            {
                DataGridViewCheckBoxCell cell = dgvC.CurrentCell as DataGridViewCheckBoxCell;
                dgvC.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        /// <summary>
        /// 根据逻辑是否的值来设定值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void dataGridView_ContactCustDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (!(sender is DataGridViewCust)) { return; }
            DataGridViewCheckBoxCell lvc = null;
            DataGridViewCell lc = null;
            DataGridViewCust dgvC = sender as DataGridViewCust;
            foreach (CheckColumTag lvcct in dgvC.PV_checkColumnsTag)
            {
                if (e.ColumnIndex == lvcct.PV_ColumnIndex)
                {

                    lc = dgvC.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (lc is DataGridViewCheckBoxCell)
                    {
                        lvc = lc as DataGridViewCheckBoxCell;

                        if (lvc.Value != null)
                        {
                            if (lvc.ValueType != typeof(System.Boolean))
                            { e.Value = (lvc.Value.ToString() == lvcct.TrueValue.ToString()); }
                        }
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 点标题的check框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HeaderCheckBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender is CheckBox)
            {
                HeaderCheckBoxClick((CheckBox)sender);
            }
        }
        /// <summary>
        /// 相应空格 点标题check
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HeaderCheckBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((sender is CheckBox) && (e.KeyCode == Keys.Space))
                HeaderCheckBoxClick((CheckBox)sender);
        }
        /// <summary>
        /// 标题check位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void dgvSelectAll_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if ((e.RowIndex == -1) && (e.ColumnIndex >= 0))
            {
                if (!(sender is DataGridViewCust)) { return; }
                DataGridViewCust dgvC = sender as DataGridViewCust;
                dgvC.SuspendLayout();
                foreach (CheckColumTag lvcct in dgvC.PV_checkColumnsTag)
                {
                    if (e.ColumnIndex == lvcct.PV_ColumnIndex)
                    {
                        ResetHeaderCheckBoxLocation(dgvC, lvcct.HeaderCheckBox, lvcct.PV_ColumnIndex);
                    }
                }
                dgvC.ResumeLayout();
            }
        }

        private static void dgvSelectAll_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (!(sender is DataGridViewCust)) { return; }
            DataGridViewCust dgvC = sender as DataGridViewCust;
            foreach (CheckColumTag lvcct in dgvC.PV_checkColumnsTag)
            {
                //if (e.Column.Index == lvcct.PV_ColumnIndex)
                //{
                ResetHeaderCheckBoxLocation(dgvC, lvcct.HeaderCheckBox, lvcct.PV_ColumnIndex);
                //}
            }
        }

        private static void ResetHeaderCheckBoxLocation(DataGridViewCust dgvC, CheckBox HeaderCheckBox, int ColumnIndex)
        {
            //Get the column header cell bounds
            Rectangle oRectangle = dgvC.GetCellDisplayRectangle(ColumnIndex, -1, true);
            HeaderCheckBox.Height = oRectangle.Height - 4;
            if (oRectangle.Width > 4)
            {
                HeaderCheckBox.Visible = (dgvC.Columns[ColumnIndex].ReadOnly != true) && (dgvC.ReadOnly != true); ;
                HeaderCheckBox.Width = oRectangle.Width - 4;
            }
            else { HeaderCheckBox.Visible = false; }
            //HeaderCheckBox.Location = oRectangle.Location;
            Point oPoint = new Point();

            oPoint.X = oRectangle.Location.X + 2;
            oPoint.Y = oRectangle.Location.Y + 1;

            ////Change the location of the CheckBox to make it stay on the header
            HeaderCheckBox.Location = oPoint;
        }

        private static void HeaderCheckBoxClick(CheckBox HCheckBox)
        {
            if (HCheckBox.Tag == null) { return; }

            DataGridViewCust dgvC = HCheckBox.Tag as DataGridViewCust;
            dgvC.PV_IsHeaderCheckBoxClicked = true;
            DataGridViewCheckBoxCell dgvcbc = null;

            foreach (CheckColumTag cct in dgvC.PV_checkColumnsTag)
            {
                if (dgvC.Columns[cct.PV_ColumnIndex].ReadOnly) { continue; }
                if (Object.ReferenceEquals(cct.HeaderCheckBox, HCheckBox))
                {
                    foreach (DataGridViewRow Row in dgvC.Rows)
                    {
                        dgvcbc = Row.Cells[cct.PV_ColumnIndex] as DataGridViewCheckBoxCell;
                        dgvcbc.Value = HCheckBox.Checked ? dgvcbc.TrueValue : dgvcbc.FalseValue;
                    }
                    cct.PV_TotalCheckedCheckBoxes = HCheckBox.Checked ? dgvC.RowCount : 0;
                }
            }
            dgvC.EndEdit();
            dgvC.PV_IsHeaderCheckBoxClicked = false;
        }

        private static void RowCheckBoxClick(DataGridViewCheckBoxCell RCheckBox)
        {
            if ((RCheckBox != null) && (RCheckBox.DataGridView is DataGridViewCust))
            {
                DataGridViewCust dgvC = RCheckBox.DataGridView as DataGridViewCust;
                //Modifiy Counter;    
                foreach (CheckColumTag cct in dgvC.PV_checkColumnsTag)
                {
                    if (cct.PV_ColumnIndex == RCheckBox.ColumnIndex)
                    {
                        if ((bool)(RCheckBox.Value.Equals(RCheckBox.TrueValue)) && (cct.PV_TotalCheckedCheckBoxes < dgvC.RowCount))
                            cct.PV_TotalCheckedCheckBoxes++;
                        else if (cct.PV_TotalCheckedCheckBoxes > 0)
                            cct.PV_TotalCheckedCheckBoxes--;

                        //Change state of the header CheckBox.
                        if (cct.PV_TotalCheckedCheckBoxes < dgvC.RowCount)
                            cct.HeaderCheckBox.Checked = false;
                        else if (cct.PV_TotalCheckedCheckBoxes == dgvC.RowCount)
                            cct.HeaderCheckBox.Checked = true;
                        break;
                    }
                }
            }
        }
        //private void dataGridView_入库_ReadOnlyChanged(object sender, EventArgs e)
        //{

        //}
        /// <summary>
        /// 格式化显示 日期格式  浮点数，整数,并尝试设置可输入字符串最大字符数
        /// </summary>
        /// <param name="dataGridView"></param>
        public static void Format(DataGridView dataGridView, bool includeDecimalCol = false)
        {

            dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            DataTable lvdt = (DataTable)((BindingSource)dataGridView.DataSource).DataSource;

            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                if ((!dataGridView.ReadOnly) && (!col.ReadOnly)) { col.DefaultCellStyle.BackColor = HelpTXD.ColorWriteable; }
                try
                {
                    if (lvdt.Columns[col.DataPropertyName].DataType == typeof(System.DateTime))
                    { col.DefaultCellStyle.Format = HelpTXD.Pv_DateFormat; }
                }
                catch { }
                try
                {
                    if (lvdt.Columns[col.DataPropertyName].DataType == typeof(System.Int32))
                    { col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; }
                }
                catch { }
                if (includeDecimalCol)
                {
                    try
                    {
                        if (lvdt.Columns[col.DataPropertyName].DataType == typeof(System.Decimal))
                        {
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            col.DefaultCellStyle.Format = HelpTXD.Pv_NumFormat_Fix;
                        }
                    }

                    catch { }
                }
                try
                {
                    if (lvdt.Columns[col.DataPropertyName].DataType == typeof(System.String))
                    {
                        if (((DataGridViewTextBoxColumn)col).Width > 120)
                        { ((DataGridViewTextBoxColumn)col).Width = 120; }
                        ((DataGridViewTextBoxColumn)col).MaxInputLength = ((DataTable)((System.Windows.Forms.BindingSource)col.DataGridView.DataSource).DataSource).Columns[col.DataPropertyName].MaxLength;

                    }
                }
                catch { }
            }
        }


        /// <summary>
        /// 获取鼠标选择区域地 起始行结束行
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="RowIndexMin"></param>
        /// <param name="RowIndexMax"></param>
        /// <returns></returns>
        public static bool RowRangeSelected(DataGridView dataGridView, ref int RowIndexMin, ref int RowIndexMax)
        {
            if (dataGridView.SelectedCells.Count <= 0)
            { return false; }

            int lvMinRow = 200000000;
            int lvMaxRow = 0;

            foreach (DataGridViewCell lvc in dataGridView.SelectedCells)
            {
                if (lvMinRow > lvc.RowIndex) { lvMinRow = lvc.RowIndex; }
                if (lvMaxRow < lvc.RowIndex) { lvMaxRow = lvMinRow; }
            }
            RowIndexMax = lvMaxRow;
            RowIndexMin = lvMinRow;
            return true;
        }
    }


    public static class DataFormatTypeTXD
    {
        public static string 计数器 = "计数器";
        public static string 计量数量 = "计量数量";
        public static string 字符 = "字符";
        public static string 段落 = "段落";
        public static string 价格 = "价格";
        public static string 金额 = "金额";
        public static string 日期 = "日期";
        public static string 时钟 = "时钟";
        public static string 长时间 = "长时钟";
    }
}