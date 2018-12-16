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
    public partial class UC_CheckList : UserControl
    {
        public event PV_UCList_CurrentChangedHandle List_CurrentChanged;
        private string FSELE = "FSELE";

        //public string  PVSupperSetClass="";
        //public List<MatchFieldsPair> PV_MatchFieldsPair = new List<MatchFieldsPair>();
        //多数情况 单个字段对比。多个字段对比的情况，请将数据合并为单字段。
        private string PV_MatchField_SupperSet { get; set; }

        private string PV_MatchField_SubSet { get; set; }

        public bool PV_SelectLock
        {
            get { return (bindingSourceList.Filter != null); }
            set
            {
                if (value)
                {
                    bindingSourceList.Filter = FSELE + "=" + true;
                }
                else
                {
                    bindingSourceList.RemoveFilter();
                }
                dataGridView1.ReadOnly = value;
            }
        }

        public UC_CheckList()
        {
            InitializeComponent();
        }

        //public UC_CheckList(string MatchField_SupperSet, string MatchField_SubSet) : this();
        //{

        //    PV_MatchField_SupperSet = MatchField_SupperSet;
        //    PV_MatchField_SubSet = MatchField_SubSet;
        //}

        public List<T> PT_GetSele<T>() where T : class, new()
        {
            List<T> lvRet = new List<T>();
            DataTable lvdt = bindingSourceList.DataSource as DataTable;

            foreach (DataRow lvdr in lvdt.Rows)
            {
                if (Convert.ToBoolean(lvdr[FSELE]))
                {
                    T entity = new T();
                    foreach (var item in entity.GetType().GetProperties())
                    {
                        if (!Convert.IsDBNull(lvdr[item.Name]))
                        {
                            item.SetValue(entity, lvdr[item.Name], null);
                        }
                        //item.SetValue(entity, Convert.ChangeType(lvdr[item.Name] == DBNull.Value ? null : lvdr[item.Name], item.PropertyType), null);
                    }
                    lvRet.Add(entity);
                }
            }
            return lvRet;
        }

        public T PT_Current<T>() where T : class, new()
        {
            if (bindingSourceList.Current == null)
            {
                return null;
            }
            DataRow lvdr = (bindingSourceList.Current as DataRowView).Row;
            T entity = new T();
            foreach (var item in entity.GetType().GetProperties())
            {
                if (!Convert.IsDBNull(lvdr[item.Name]))
                {
                    item.SetValue(entity, lvdr[item.Name], null);
                }
                //item.SetValue(entity, Convert.ChangeType(lvdr[item.Name] == DBNull.Value ? null : lvdr[item.Name], item.PropertyType), null);
            }
            return entity;
        }

        /// <summary>
        /// 参照4参数的。这里就是分拆了。在多数情况下，对比字段是重名的。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="BaseData"></param>
        /// <param name="ShowColumns"></param>
        /// <param name="MatchField"></param>
        public void PT_ResetList<T>(List<T> BaseData, List<TS_GRID_COL> ShowColumns, string MatchField) where T : class
        {
            PT_ResetList(BaseData, ShowColumns, MatchField, MatchField);
        }

        /// <summary>
        /// 初始化数据勾选表格。行，列。跟对比字段。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="BaseData">要对比的行</param>
        /// <param name="ShowColumns">展示行的列显示</param>
        /// <param name="MatchField_SupperSet">可选项（超集）里的关联字段，以后做成以逗号分割的</param>
        /// <param name="MatchField_SubSet">已选项（子集）里的关联字段，以后做成以逗号好分割的</param>
        public void PT_ResetList<T>(List<T> BaseData, List<TS_GRID_COL> ShowColumns, string MatchField_SupperSet, string MatchField_SubSet) where T : class
        {
            PV_MatchField_SupperSet = MatchField_SupperSet;
            PV_MatchField_SubSet = MatchField_SubSet;


            DataTable lvDT = HelpTXD.EntitiesToDataTable(BaseData);
            DataColumn lvc = new DataColumn();
            lvc.ColumnName = FSELE;
            lvc.DataType = typeof(System.Boolean);
            lvDT.Columns.Add(lvc);
            bindingSourceList.DataSource = lvDT;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();
            DataGridViewCheckBoxColumn lvCheckCol = new DataGridViewCheckBoxColumn();
            lvCheckCol.HeaderText = "选择";
            lvCheckCol.DataPropertyName = FSELE;
            dataGridView1.Columns.Add(lvCheckCol);
            foreach (TS_GRID_COL lvShow in ShowColumns)
            {
                DataGridViewTextBoxColumn lvTextCol = new DataGridViewTextBoxColumn();
                lvTextCol.DataPropertyName = lvShow.COL_DATA_NAME;
                lvTextCol.HeaderText = lvShow.COL_TEXT;
                lvTextCol.ReadOnly = true;
                dataGridView1.Columns.Add(lvTextCol);
            }
            dataGridView1.DataSource = bindingSourceList;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        }

        /// <summary>
        /// 传入子集，匹配全集，匹配上的将勾选状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Subset"></param>
        public void Pt_MatchSubset<T>(List<T> Subset)
        {
            DataTable dtReturn = new DataTable(typeof(T).Name); //逆回model时，可以使用Name 来找T
            PropertyInfo[] oProps = typeof(T).GetProperties();
            foreach (PropertyInfo pi in oProps)
            {
                Type colType = pi.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
            }
            //为了简化表格数据的保存，将编辑状态从UI设置到此字段。提交数据时，检查此状态，选择增删改操作。
            //DataColumn acol = new DataColumn(PC操作标记.Col增删改状态列, typeof(int));
            //acol.DefaultValue = PC操作标记.i_browse_unchange;
            //acol.AllowDBNull = false;
            //dtReturn.Columns.Add(acol);
            if (Subset != null)
            {
                foreach (T rec in Subset)
                {
                    DataRow dr = dtReturn.NewRow();
                    foreach (PropertyInfo pi in oProps)
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                    }
                    dtReturn.Rows.Add(dr);
                }
            }
            dtReturn.AcceptChanges();
            DataTable Dest = bindingSourceList.DataSource as DataTable;
            foreach (DataRow lvdr in Dest.Rows)
            {
                lvdr[FSELE] = false;
            }
            for (int lvy = 0; lvy < Dest.Rows.Count; lvy++)
            {
                bool FoundNotMatch = false;

                for (int lvi = 0; lvi < dtReturn.Rows.Count; lvi++)
                {
                    ////foreach (MatchFieldsPair lvpairy in this.PV_MatchFieldsPair)
                    //{
                    //    if (dtReturn.Rows[lvi][lvpairy.Field_SubSet].ToString() != Dest.Rows[lvy][lvpairy.Field_FullSet].ToString())
                    //    {
                    //        FoundNotMatch = true;
                    //        break;
                    //    }
                    //}

                    if (dtReturn.Rows[lvi][PV_MatchField_SubSet].ToString() == Dest.Rows[lvy][PV_MatchField_SupperSet].ToString())
                    {
                        FoundNotMatch = true;
                        break;
                    }
                }
                if (FoundNotMatch)
                {
                    Dest.Rows[lvy][FSELE] = true;
                    Dest.Rows[lvy].AcceptChanges();
                }
            }
            Dest.AcceptChanges();
            for (int lvi = 0; lvi < dtReturn.Rows.Count; lvi++)
            {
                if (Convert.ToBoolean(Dest.Rows[lvi][FSELE]))
                {
                    bindingSourceList.Position = lvi;
                    break;
                }
            }
        }

        private void bindingSourceList_CurrentChanged(object sender, EventArgs e)
        {
            if (List_CurrentChanged != null)
            {
                List_CurrentChanged(sender, e);
            }
        }

        public object Current()
        {
            return bindingSourceList.Current;
        }
    }

    public delegate void PV_UCList_CurrentChangedHandle(object sender, EventArgs e);
}