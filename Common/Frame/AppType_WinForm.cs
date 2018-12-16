using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace TXD.CF
{
    public static partial class HelpTXD
    {
        //��Ϣ����API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd, // ��Ϣ�����Ĵ��ڵľ��
            int Msg, // ��ϢID
            int wParam, // ����1
            IntPtr lParam
        );

        public const int WM_USER = 0x0400;
        public const int WM_�ؼ��䴰��ر� = WM_USER + 1;
        public const int WM_�ؼ��䴰��ѡ�� = WM_USER + 2;
        public const int WM_�ؼ��䴰��_�ϲ�����һ�� = WM_USER + 3;
        public const int WM_�﷨��������ر� = WM_USER + 4;

        public const int WM_ѡ���˱��һ��_���� = WM_USER + 5;
         public const int WM_���ӷ����仯 = WM_USER + 6;
        public const int WM_ˢ��Ԥ�� = WM_USER + 7;


        //public static System.Drawing.Color ColorReadOnly = System.Drawing.Color.WhiteSmoke;
        public static System.Drawing.Color ColorWriteable = System.Drawing.Color.PeachPuff;
        public static System.Drawing.Color ColorWarning = System.Drawing.Color.HotPink;
        public static System.Drawing.Color ColorBackModified = System.Drawing.Color.Gold;

        //public static string CombineName(string last_name, string first_name)
        //{ return last_name + ", " + first_name; }
        //public static string NewMaxID(IDataAccCover dataAccess,
        //    string tableName, string fieldName)
        //{
        //    string lvPreFix = "";
        //    if (tableName.ToLower() == "Tradings".ToLower())
        //    { lvPreFix = "I" + DateTime.Now.Date.ToString("MMddyy") + '-'; }
        //    if (tableName.ToLower() == "Students".ToLower())
        //    { lvPreFix = "C" + DateTime.Now.Date.ToString("MMddyy") + '-'; }
        //    if (tableName.ToLower() == "Vendors".ToLower())
        //    { lvPreFix = "V" + DateTime.Now.Date.ToString("MMddyy") + '-'; }
        //    if (tableName.ToLower() == "Inventory".ToLower())
        //    { lvPreFix = "SSI" + '-'; }
        //    if (lvPreFix == "")
        //    { lvPreFix = DateTime.Now.Date.ToString("yyMMdd") + '-'; }
        //    return NewMaxID(dataAccess, tableName, fieldName, lvPreFix, 11);
        //}

        //public static string NewMaxID(IDataAccCover dataAccess,
        //    string tableName, string fieldName, string preFix, int fieldWith)
        //{
        //    int lv_sn = 0;//��ˮ��
        //    string lv_MaxId = "";
        //    //string strSQL = "Select " + fieldName + " From " + tableName
        //    //    + " where " + fieldName + " like '" + preFix + "%'"
        //    //    + " order by  " + fieldName;
        //    //DataSet ds = new DataSet();
        //    //int lv_tmpsn = 0;
        //    //dataAccess.RunSql(strSQL, null, ref ds);
        //    //foreach (DataRow dr in ds.Tables[0].Rows)
        //    //{
        //    //    try
        //    //    {
        //    //        lv_MaxId = dr[0].ToString().Substring(preFix.Length,
        //    //            fieldWith - preFix.Length);
        //    //        lv_tmpsn = Convert.ToInt32(lv_MaxId);
        //    //        if (lv_sn < lv_tmpsn) { lv_sn = lv_tmpsn; }
        //    //    }
        //    //    catch (Exception)
        //    //    {
        //    //    }
        //    //}
        //    //lv_sn++;

        //    //lv_MaxId = lv_sn.ToString("000000000000000000000000000000000");
        //    //lv_MaxId = lv_MaxId.Substring(lv_MaxId.Length - (fieldWith - preFix.Length));
        //    return (preFix + lv_MaxId);
        //}

        ////public static DataSet Pvt_GetCompany(IDataAccCover dataAccess)
        ////{
        ////    DataSet ds = new DataSet();
        ////    dataAccess.RunSql("select * from zsys_company_info", null, ref ds);
        ////    return ds;
        ////}

        //public static bool Pvt_isKeyExist(IDataAccCover dataAccess,
        //    string tableName, string fieldName, string keyValue)
        //{

        //    //ArrayList paras = new ArrayList();
        //    List<IDbDataParameter> paras = new List<IDbDataParameter>();
        //    int lv_hasID = 0;
        //    string lv_sql = "select count(*) from " + tableName + " where " + fieldName + "=@" + fieldName;
        //    paras.Add(dataAccess.CreatParameter(fieldName, keyValue));
        //    object lv_oid = dataAccess.GetOne(lv_sql, paras);
        //    if (lv_oid != null)
        //    {
        //        if (lv_oid.ToString() != "") { lv_hasID = Convert.ToInt32(lv_oid); }
        //    }
        //    return (lv_hasID > 0);
        //}
        ///// <summary>
        ///// �˴��D��R��  �s�W���ɭԨϥ�
        ///// </summary>
        ///// <param name="newPKeyValue"></param>
        ///// <returns></returns>
        //protected bool Pvt_isNewKeyConflict(IDataAccCover dataAccess,
        //    string tableName, string fieldName, string keyValue)
        //{
        //    ArrayList paras = new ArrayList();
        //    int lv_hasID = 0;
        //    string lv_sql = "select count(*) from " + Pv_tbMain + " where " + Pv_IdField + "=@" + Pv_IdField;
        //    paras.Add(Pv_DataAccess.CreatParameter(Pv_IdField, newPKeyValue));
        //    object lv_oid = Pv_DataAccess.GetOne(lv_sql, paras);
        //    if (lv_oid != null)
        //    {
        //        if (lv_oid.ToString() != "") { lv_hasID = Convert.ToInt32(lv_oid); }
        //    }
        //    return (lv_hasID > 0);
        //}

        public static string Pv_DateFormat = "MM/dd/yyyy";
        public static string Pv_AmountFormat = "#,##0.##";
        public static string Pv_PriceFormat = "#,##0.####";
        public static string Pv_NumFormat = "#,###.######";
        public static string Pv_NumFormat_Fix = "#,###.000";
        public static string Pv_CountFormat = "#,###";

        //public static void LogSignToServer(ref LoginUserWcf aLoginUser, string Mesg)
        //{
        //    try
        //    {

        //        aLoginUser.MSG = "";
        //        System.ServiceModel.ChannelFactory<ILogin> channelFactoryDS = new System.ServiceModel.ChannelFactory<ILogin>("TxdCfClient");
        //        ILogin proxy = channelFactoryDS.CreateChannel();
        //        using (proxy as IDisposable)
        //        {
        //            proxy.Login(ref aLoginUser);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log4net.ILog logsys = log4net.LogManager.GetLogger("SysRun");
        //        logsys.Error(Mesg + Environment.NewLine + "   ----------uplog error(" + ex.Message + Environment.NewLine + ex.StackTrace + ")");
        //    }
        //}

        /// <summary>
        /// ��������Դ�Ϳؼ���
        /// </summary>
        /// <param name="ctl">�ؼ�</param>
        /// <param name="propertyName">��������</param>
        /// <param name="BindingSource">����Դ</param>
        /// <param name="fieldName">���ֶ�</param>
        public static void SetBinding(Control ctl, string propertyName, object BindingSource, string fieldName)
        {
            if (ctl.DataBindings[propertyName] != null) ctl.DataBindings.Remove(ctl.DataBindings[propertyName]);
            //Binding lvbd = new Binding(propertyName, BindingSource, fieldName, true, DataSourceUpdateMode.OnPropertyChanged);
            //ctl.DataBindings.Add(lvbd);
            //ctl.DataBindings.Add(propertyName, BindingSource, fieldName, true, DataSourceUpdateMode.OnPropertyChanged);
            string lvFormat = "";
            object lvNullValue = null;
            if (fieldName.ToLower().Contains("fqty"))
            {
                lvFormat = Pv_NumFormat;
                lvNullValue = 0;
            }
            else if (fieldName.ToLower().Contains("fprice"))
            {
                lvFormat = Pv_PriceFormat;
                lvNullValue = 0;
            }
            else if (fieldName.ToLower().Contains("famt") || fieldName.ToLower().Contains("famount"))
            {
                lvFormat = Pv_AmountFormat;
                lvNullValue = 0;
            }
            else if (fieldName.ToLower().Contains("fweight"))
            {
                lvFormat = Pv_NumFormat;
                lvNullValue = 0;
            }
            ctl.DataBindings.Add(propertyName, BindingSource, fieldName, true, DataSourceUpdateMode.OnPropertyChanged, lvNullValue, lvFormat);
        }

        /// <summary>
        /// ��������Դ�Ϳؼ���,Ĭ�ϰ�Text����
        /// </summary>
        /// <param name="ctl">�ؼ�</param>
        /// <param name="BindingSource">����Դ</param>
        /// <param name="fieldName">���ֶ�</param>
        public static void SetBinding(Control ctl, object BindingSource, string fieldName)
        {
            SetBinding(ctl, "Text", BindingSource, fieldName);
        }

        public static void ClearBindings(Control ctl, string propertyName)
        {
            if (ctl.DataBindings[propertyName] != null) ctl.DataBindings.Remove(ctl.DataBindings[propertyName]);
        }

        /// <summary>
        /// ���տؼ��������������Դ
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="bindingSource"></param>
        public static void BindTextBox(TextBox textBox,
            BindingSource bindingSource)
        {
            string lv_fieldName = textBox.Name;
            if (lv_fieldName.Contains("_"))
            {
                lv_fieldName = lv_fieldName.Substring(lv_fieldName.IndexOf("_") + 1);
                BindTextBox(textBox, bindingSource, lv_fieldName);
            }
        }

        /// <summary>
        /// ������Դ
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="bindingSource"></param>
        /// <param name="dataMember"></param>
        public static void BindTextBox(TextBox textBox,
            BindingSource bindingSource, string dataMember)
        {
            if (string.IsNullOrEmpty(dataMember))
            {
                return;
            }
            textBox.DataBindings.Clear();
            textBox.DataBindings.Add("Text", bindingSource, dataMember);
            if (((DataTable) (bindingSource).DataSource).Columns[dataMember].DataType == typeof(System.String))
            {
                int lvmaxlenth = ((DataTable) (bindingSource).DataSource).Columns[dataMember].MaxLength;
                if (lvmaxlenth > 0)
                {
                    textBox.MaxLength = lvmaxlenth;
                }
            }
        }

        //public static void initComboBox_items(ComboBox comboBox, DataTable ListSource, string aValueMember, string aDisplayMember, string sFirstRowText, object oFirstValue)
        //{
        //    if (comboBox.DataSource != null)
        //    {
        //        ((DataTable)comboBox.DataSource).Dispose();
        //        comboBox.DataSource = null;
        //    }
        //    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        //    if (!((sFirstRowText == null) || (oFirstValue == null)))
        //    {
        //        DataRow dr = ListSource.NewRow();
        //        dr[aValueMember] = oFirstValue;
        //        dr[aDisplayMember] = sFirstRowText;
        //        ListSource.Rows.InsertAt(dr, 0);
        //    }

        //    comboBox.ValueMember =aValueMember;
        //    comboBox.DisplayMember = aDisplayMember;
        //    comboBox.DataSource = ListSource;
        //    comboBox.SelectedIndex = 0;
        //}
        public static void initComboBox_items(ComboBox comboBox, BindingSource bSource, string aValueMember, string aDisplayMember, string sFirstRowText, object oFirstValue)
        {
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!((sFirstRowText == null) || (oFirstValue == null)))
            {
                DataTable lvdt = bSource.DataSource as DataTable;
                DataRow dr = lvdt.NewRow();
                dr[aValueMember] = oFirstValue;
                dr[aDisplayMember] = sFirstRowText;
                dr.EndEdit();
                lvdt.Rows.InsertAt(dr, 0);
            }

            comboBox.ValueMember = aValueMember;
            comboBox.DisplayMember = aDisplayMember;
            comboBox.DataSource = bSource;
            comboBox.SelectedIndex = 0;
        }


        public static void BindingContainer(Control ctl, object aBindingSource)
        {
            string lv����Դ������;
            string lv�ؼ�����׺;
            Type type = aBindingSource.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (PropertyInfo i in ps)
            {
                lv����Դ������ = i.Name;
                foreach (Control sub in ctl.Controls)
                {
                    lv�ؼ�����׺ = sub.Name.Substring(sub.Name.IndexOf("_") + 1);
                    if (lv����Դ������ == lv�ؼ�����׺)
                    {
                        if (sub.GetType().GetProperty("Text") != null)
                        {
                            SetBinding(sub, aBindingSource, lv����Դ������);
                        }
                        else if (sub.GetType().GetProperty("Value") != null)
                        {
                            SetBinding(sub, "Value", aBindingSource, lv����Դ������);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// �󶨵�Դͷ,һ���ǿɱ༭����datatable
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="aBindingSource"></param>
        public static void BindingContainerWithBS(Control ctl, BindingSource aBindingSource)
        {
            DataTable lvdt = aBindingSource.DataSource as DataTable;

            string lv����Դ������;
            string lv�ؼ�����׺;
            Type type = aBindingSource.GetType();
            System.Reflection.PropertyInfo[] ps = type.GetProperties();
            foreach (DataColumn lvc in lvdt.Columns)
            {
                lv����Դ������ = lvc.ColumnName;
                foreach (Control sub in ctl.Controls)
                {
                    lv�ؼ�����׺ = sub.Name.Substring(sub.Name.IndexOf("_") + 1);
                    if (lv����Դ������ == lv�ؼ�����׺)
                    {
                        if (sub is DateTimePicker)
                        {
                            DateTimePicker lvp = sub as DateTimePicker;
                            if (!lvp.ShowCheckBox)
                                BindingDateTimePicker(lvp, lv����Դ������, aBindingSource);
                        }
                        //else if (sub is DatetimePickerUTXD)
                        //{
                        //    DatetimePickerUTXD lvp = sub as DatetimePickerUTXD;
                        //    BindingDateTimePicker(lvp, lv����Դ������, aBindingSource);
                        //}
                        else if (sub is CheckBox)
                        {
                            CheckBox lvchk = sub as CheckBox;
                            BindingCheckBox(lvchk, lv����Դ������, aBindingSource);
                        }
                        else if (sub is ComboBox)
                        {
                            ComboBox lvCom = sub as ComboBox;
                            if (lvCom.DataSource != null)
                            {
                                SetBinding(sub, "SelectedValue", aBindingSource, lv����Դ������);
                            }
                            else
                            {
                                SetBinding(sub, "Text", aBindingSource, lv����Դ������);
                            }
                        }
                        else if (sub.GetType().GetProperty("Value") != null)
                        {
                            SetBinding(sub, "Value", aBindingSource, lv����Դ������);
                        }
                        else if (sub.GetType().GetProperty("Text") != null)
                        {
                            SetBinding(sub, aBindingSource, lv����Դ������);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// �󶨸���ThreeState��������Ӧ����
        /// </summary>
        /// <param name="source">Binding Source</param> www.it165.net
        /// <param name="checkBox">The Combo Box</param>
        /// <param name="bindingField">The binding field name</param>
        public static void BindingCheckBox(CheckBox checkBox, string bindingField, BindingSource source)
        {
            string lvProBind = "Checked";
            if (checkBox.ThreeState)
            {
                lvProBind = "CheckState";
            }

            Binding binding = checkBox.DataBindings[lvProBind];
            if (binding != null)
            {
                checkBox.DataBindings.Remove(binding);
            }
            Binding checkBoxBinding = new Binding(lvProBind, source, bindingField, true, DataSourceUpdateMode.OnPropertyChanged);
            checkBoxBinding.Format += BindingCheckBox_Format;
            checkBoxBinding.Parse += BindingCheckBox_Parse_DB;
            checkBox.DataBindings.Add(checkBoxBinding);
        }


        /// <summary>
        /// Format event for checkBox binding
        /// </summary>
        /// <param name="sender">The source of the event. Here is Binding.</param>
        /// <param name="e">Provides data for the Binding.Format and Binding.Parse events.</param>
        private static void BindingCheckBox_Format(object sender, ConvertEventArgs e)
        {
            CheckBox lvchk = (sender as Binding).Control as CheckBox;
            if (lvchk.ThreeState)
            {
                if (e.Value != null && (e.Value != DBNull.Value))
                {
                    if (Convert.ToBoolean(e.Value))
                    {
                        e.Value = CheckState.Checked;
                    }
                    else
                    {
                        e.Value = CheckState.Unchecked;
                    }
                }
                else
                {
                    e.Value = CheckState.Indeterminate;
                }
            }
            else
            {
                if ((e.Value == null) || (e.Value == DBNull.Value))
                {
                    e.Value = false;
                }
                else
                {
                    e.Value = Convert.ToBoolean(e.Value);
                }
            }
            //if (e.DesiredType == typeof(CheckState))
            //{
            //    if (e.Value != null && Convert.ToBoolean(e.Value))
            //    {
            //        e.Value = CheckState.Checked;
            //    }
            //    else if (e.Value != null && !(Convert.ToBoolean(e.Value)))
            //    {
            //        e.Value = CheckState.Unchecked;
            //    }
            //    else
            //    {
            //        e.Value = CheckState.Indeterminate;
            //    }
            //}
            //else if (e.DesiredType == typeof(bool))
            //{
            //    if ((e.Value == null) || (e.Value == DBNull.Value))
            //    { e.Value = false; }
            //    else
            //    {
            //        e.Value = Convert.ToBoolean(e.Value);
            //    }


            //}
        }

        /// <summary>
        /// Parse event for checkBox binding
        /// </summary>
        /// <param name="sender">The source of the event. Here is Binding.</param>
        /// <param name="e">Provides data for the Binding.Format and Binding.Parse events.</param>
        private static void BindingCheckBox_Parse_DB(object sender, ConvertEventArgs e)
        {
            CheckBox lvchk = (sender as Binding).Control as CheckBox;
            if (lvchk.ThreeState)
            {
                switch ((CheckState) e.Value)
                {
                    case CheckState.Checked:
                        e.Value = true;
                        break;
                    case CheckState.Indeterminate:
                        e.Value = DBNull.Value;
                        break;
                    default:
                        e.Value = false;
                        break;
                }
            }
            else
            {
                e.Value = lvchk.Checked;
            }
            //if (e.DesiredType == typeof(bool?))
            //{
            //    switch ((CheckState)e.Value)
            //    {
            //        case CheckState.Checked:
            //            e.Value = true;
            //            break;
            //        case CheckState.Indeterminate:
            //            e.Value = null;
            //            break;
            //        default:
            //            e.Value = false;
            //            break;
            //    }
            //}
            //else if (e.DesiredType == typeof(bool))
            //{ e.Value = Convert.ToBoolean(e.Value); }
        }

        private static void RemoveEvent<T>(T c, string name)
        {
            Delegate[] invokeList = GetObjectEventList_txd(c, name);
            if (invokeList == null)
            {
                return;
            }
            foreach (Delegate del in invokeList)
            {
                typeof(T).GetEvent(name).RemoveEventHandler(c, del);
            }
        }

        ///  <summary>     
        /// ��ȡ�����¼� zgke@sina.com qq:116149     
        ///  </summary>     
        ///  <param name="p_Object">���� </param>     
        ///  <param name="p_EventName">�¼��� </param>     
        ///  <returns>ί���� </returns>     
        public static Delegate[] GetObjectEventList(object p_Object, string p_EventName)
        {
            FieldInfo _Field = p_Object.GetType().GetField(p_EventName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            if (_Field == null)
            {
                return null;
            }
            object _FieldValue = _Field.GetValue(p_Object);
            if (_FieldValue != null && _FieldValue is Delegate)
            {
                Delegate _ObjectDelegate = (Delegate) _FieldValue;
                return _ObjectDelegate.GetInvocationList();
            }
            return null;
        }

        public static Delegate[] GetObjectEventList_txd(object p_Object, string p_EventName)
        {
// BindingFlags.NonPublic || BindingFlags.Static | BindingFlags.Public
            FieldInfo _Field = p_Object.GetType().GetField(p_EventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
            FieldInfo _Fieldttt = p_Object.GetType().GetField("MinDateTime", BindingFlags.Instance | BindingFlags.Static);
            FieldInfo _Fieldttttt = p_Object.GetType().GetField("ValueChanged", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            FieldInfo[] sssss = p_Object.GetType().GetFields();
            if (_Field == null)
            {
                return null;
            }
            object _FieldValue = _Field.GetValue(p_Object);
            if (_FieldValue != null && _FieldValue is Delegate)
            {
                Delegate _ObjectDelegate = (Delegate) _FieldValue;
                return _ObjectDelegate.GetInvocationList();
            }
            return null;
        }

        /// <summary>
        /// ��ʱ����԰�,��check.д��ֵ��.����
        /// </summary>
        /// <param name="dtp"></param>
        /// <param name="bindingField"></param>
        /// <param name="source"></param>
        public static void BindingDateTimePicker(DateTimePicker dtp, string bindingField, BindingSource source)
        {
            string lvProBind = "Value";

            Binding binding = dtp.DataBindings[lvProBind];
            if (binding != null)
            {
                dtp.DataBindings.Remove(binding);
            }
            Binding dtp_Binding = new Binding(lvProBind, source, bindingField, true, DataSourceUpdateMode.OnPropertyChanged);

            dtp_Binding.Format += BindingDateTimePicker_Format;
            //dtp_Binding.Parse += BindingDateTimePicker_Parse_DB;
            //RemoveEvent(dtp, "ValueChanged"); //remove����.������. 

            //dtp.ValueChanged += new EventHandler(dateTimePicker_FDate_CertRegStart_ValueChanged);
            //RemoveEvent(dtp, "ValueChanged");

            dtp.DataBindings.Add(dtp_Binding);
        }

        private static void BindingDateTimePicker_Format(object sender, ConvertEventArgs e)
        {
            Binding lvb = sender as Binding;
            DateTimePicker lvDTP = (sender as Binding).Control as DateTimePicker;
            if (lvDTP.ShowCheckBox)
            {
                if ((e.Value == null) || (e.Value == DBNull.Value))
                {
                    //e.Value = DateTime.Now;
                    if (lvDTP.Checked)
                    {
                        lvDTP.Checked = false;
                    }
                }
                else
                {
                    if (!lvDTP.Checked)
                    {
                        lvDTP.Checked = true;
                    }
                }
            }
            else
            {
                if ((e.Value == null) || (e.Value == DBNull.Value))
                {
                    e.Value = (lvb.BindableComponent as DateTimePicker).Value;
                }
            }
        }

        ///// <summary>
        ///// �ǿ�ʱ��ֵ�� ʹ�����  ���ֻ��һ��.
        ///// </summary>
        ///// <param name="dtp"></param>
        ///// <param name="bindingField"></param>
        ///// <param name="source"></param>
        //public static void BindingDateTimePicker(DateTimePicker dtp, string bindingField, BindingSource source)
        //{
        //    string lvProBind = "Tag";

        //    Binding binding = dtp.DataBindings[lvProBind];
        //    if (binding != null)
        //    {
        //        dtp.DataBindings.Remove(binding);

        //    }
        //    Binding dtp_Binding = new Binding(lvProBind, source, bindingField, true, DataSourceUpdateMode.OnPropertyChanged);

        //    dtp_Binding.Format += BindingDateTimePicker_Format; //���Է���  ������2���¼���,���Զ���ʾ����.û�,Ϊ�ΰ󶨵�Tag��,��Ӱ��value
        //    dtp_Binding.Parse += BindingDateTimePicker_Parse_DB;//���Է���  ������2���¼���,���Զ���ʾ����.û�,Ϊ�ΰ󶨵�Tag��,��Ӱ��value
        //    //RemoveEvent(dtp, "ValueChanged"); //remove����.������. 

        //    dtp.ValueChanged += new EventHandler(dateTimePicker_FDate_CertRegStart_ValueChanged);
        //    //RemoveEvent(dtp, "ValueChanged");

        //    dtp.DataBindings.Add(dtp_Binding);
        //}
        //static void dateTimePicker_FDate_CertRegStart_ValueChanged(object sender, EventArgs e)
        //{
        //    DateTimePicker lvDTP = sender as DateTimePicker;
        //    if (lvDTP.ShowCheckBox)
        //    {
        //        if (lvDTP.Checked)
        //        {
        //            DateTime lvdt = DateTime.Now;
        //            try
        //            {
        //                lvdt = Convert.ToDateTime(lvDTP.Tag);
        //                if (lvdt != lvDTP.Value)
        //                { lvDTP.Tag = lvDTP.Value; }
        //            }
        //            catch { }
        //        }
        //        else
        //        {
        //            if (lvDTP.Tag != DBNull.Value)
        //            { lvDTP.Tag = DBNull.Value; }
        //        }
        //    }
        //}
        //static void BindingDateTimePicker_Format(object sender, ConvertEventArgs e)
        //{
        //    DateTimePicker lvDTP = (sender as Binding).Control as DateTimePicker;
        //    if (lvDTP.ShowCheckBox)
        //    {
        //        if ((e.Value == null) || (e.Value == DBNull.Value))
        //        {
        //            //e.Value = DateTime.Now;
        //            if (lvDTP.Checked) { lvDTP.Checked = false; }
        //        }
        //        else
        //        {
        //            if (!lvDTP.Checked) { lvDTP.Checked = true; }
        //        }
        //    }
        //}
        //static void BindingDateTimePicker_Parse_DB(object sender, ConvertEventArgs e)
        //{
        //    DateTimePicker lvDTP = (sender as Binding).Control as DateTimePicker;

        //    e.Value = lvDTP.Value;
        //    if ((lvDTP.ShowCheckBox) && (!lvDTP.Checked) && (e.Value != DBNull.Value))
        //    {
        //        e.Value = DBNull.Value;
        //    }
        //}
        /////// <summary>
        /////// �߱���ֵ������ ʹ�����
        /////// </summary>
        /////// <param name="dtp">�Զ���ɴ�����ֵ��</param>
        /////// <param name="bindingField"></param>
        /////// <param name="source"></param>
        ////public static void BindingDateTimePicker(DatetimePickerUTXD dtp, string bindingField, BindingSource source)
        ////{
        ////    string lvProBind = "Value";

        ////    Binding binding = dtp.DataBindings[lvProBind];
        ////    if (binding != null)
        ////    {
        ////        dtp.DataBindings.Remove(binding);
        ////    }
        ////    Binding dtp_Binding = new Binding(lvProBind, source, bindingField, true, DataSourceUpdateMode.OnPropertyChanged);
        ////    dtp_Binding.Format += BindingDateTimePickerUTXD_Format;
        ////    dtp_Binding.Parse += BindingDateTimePickerUTXD_Parse_DB;
        ////    dtp.DataBindings.Add(dtp_Binding);
        ////}

        ////static void BindingDateTimePickerUTXD_Format(object sender, ConvertEventArgs e)
        ////{
        ////    DatetimePickerUTXD lvDTP = (sender as Binding).Control as DatetimePickerUTXD;
        ////    if ((e.Value == null) || (e.Value == DBNull.Value))
        ////    { if (lvDTP.Checked) { lvDTP.Checked = false; } }
        ////    else
        ////    { lvDTP.Checked = true; }
        ////}
        ////static void BindingDateTimePickerUTXD_Parse_DB(object sender, ConvertEventArgs e)
        ////{
        ////    DatetimePickerUTXD lvDTP = (sender as Binding).Control as DatetimePickerUTXD;
        ////    //e.Value = lvDTP.Value;
        ////    if (!lvDTP.Checked)
        ////    {
        ////        e.Value = DBNull.Value;
        ////    }
        ////}


        /// <summary>
        /// �����󶨲��� ����.��ǰ,ÿ��tableҳ�� ������.�ر������datetimepickerd��ʱ��
        /// </summary>
        /// <param name="tabControl"></param>
        /// 
        public static void Fix(TabControl tabControl)
        {
            TabPage selected = tabControl.SelectedTab;

            foreach (TabPage tab in tabControl.TabPages)
            {
                if (tab != selected)
                    tabControl.SelectTab(tab);
            }
            tabControl.SelectTab(selected);
        }

        public static void BindingClearContainer(Control ctl)
        {
            foreach (Control sub in ctl.Controls)
            {
                if (sub.GetType().GetProperty("Text") != null)
                {
                    ClearBindings(sub, "Text");
                }
                else if (sub.GetType().GetProperty("Value") != null)
                {
                    ClearBindings(sub, "Value");
                }
            }
        }

        public static DataTable loadExcelToDS(string ExcelFile)
        {
            DataTable ExcelDS = null;
            string lv_exceltbName = "Sheet1";
            //Excel.Application xlApp = new Excel.ApplicationClass();
            //if (xlApp == null)
            //{
            //    return "Excel Application not found.";
            //}

            //Excel.Workbook xlBook = null;
            //Excel.Worksheet xlSheet = null;

            //try
            //{
            //    xlBook = xlApp.Workbooks.Open(ExcelFile, Type.Missing, Type.Missing, Type.Missing,
            //        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing
            //        , Type.Missing, Type.Missing, Type.Missing);
            //    xlSheet = (Excel.Worksheet)xlBook.Worksheets[1];
            //    lv_exceltbName = xlSheet.CustName;
            //    xlApp.Quit();
            //}
            //catch (Exception ex) { lvmsg = ex.Message; }
            //if (lvmsg != "")
            //{
            //    xlApp.Quit();
            //    return lvmsg;
            //}

            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelFile + ";Extended Properties=Excel 8.0;";
            string sql = "";
            System.Data.OleDb.OleDbDataAdapter dac;

            //var strConn = "Provider=Microsoft.Jet.OLEDB.4.0; " +
            //                 "Data Source=" + f1.FileName + "; " +
            //                 "Extended Properties='Excel 8.0;IMEX=1'";
            bool lvgoon = false;
            var conn = new System.Data.OleDb.OleDbConnection(strConn);
            try
            {
                conn.Open();
                var tblSchema = conn.GetSchema("Tables");
                if (tblSchema.Rows.Count > 0)
                {
                    lv_exceltbName = tblSchema.Rows[0]["TABLE_NAME"].ToString();
                    var oledbadp = new System.Data.OleDb.OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", lv_exceltbName), conn);
                    //var data = new DataTable(lv_exceltbName);
                    //oledbadp.Fill(data);
                    //gridView1.Columns.Clear();
                    //gridControl1.DataSource = data;
                }


                conn.Close();
                sql = "select * from [" + lv_exceltbName + "$]";
                dac = new System.Data.OleDb.OleDbDataAdapter(sql, strConn);
                ExcelDS = new DataTable();


                dac.Fill(ExcelDS);
                lvgoon = true;
            }
            catch (Exception e)
            {
#if (DEBUG)
                MessageBox.Show("Microsoft.Jet.OLEDB.4.0\r\n" + e.Message);
#endif
            }
            if (lvgoon)
            {
                return ExcelDS;
            }
            strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ExcelFile + ";Extended Properties=Excel 8.0;";
            try
            {
                conn.Close();
                conn.Open();
                var tblSchema = conn.GetSchema("Tables");
                if (tblSchema.Rows.Count > 0)
                {
                    lv_exceltbName = tblSchema.Rows[0]["TABLE_NAME"].ToString();
                    sql = "select * from [" + lv_exceltbName + "$]";
                    dac = new System.Data.OleDb.OleDbDataAdapter(sql, strConn);
                    ExcelDS = new DataTable();

                    dac.Fill(ExcelDS);
                }
            }
            catch (Exception e)
            {
#if (DEBUG)
                MessageBox.Show("Microsoft.JACE.OLEDB.12.0\r\n" + e.Message);
#endif
            }
            return ExcelDS;
        }


        public static List<T> Table2Entities<T>(DataTable table, bool ignoreMiss = false) where T : new()
        {
            List<T> entities = new List<T>();
            if (table == null)
            {
                return entities;
            }
            PropertyInfo[] lvOrgin_info = typeof(T).GetProperties();
            if (ignoreMiss)
            {
                List<PropertyInfo> lvNewInfos = new List<PropertyInfo>();
                foreach (PropertyInfo lvpro_inf in lvOrgin_info)
                {
                    if (table.Columns[lvpro_inf.Name] != null)
                    {
                        lvNewInfos.Add(lvpro_inf);
                    }
                }
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                    T entity = new T();
                    foreach (var item in lvNewInfos)
                    {
                        if (!item.CanWrite)
                        {
                            continue;
                        }
                        if (!Convert.IsDBNull(row[item.Name]))
                        {
                            item.SetValue(entity, row[item.Name], null);
                        }
                        //item.SetValue(entity, Convert.ChangeType(row[item.Name] == DBNull.Value ? null : row[item.Name], item.PropertyType), null);
                    }
                    entities.Add(entity);
                }
            }
            else
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }
                    T entity = new T();
                    foreach (var item in lvOrgin_info)
                    {
                        if (!item.CanWrite)
                        {
                            continue;
                        }
                        if (!Convert.IsDBNull(row[item.Name]))
                        {
                            item.SetValue(entity, row[item.Name], null);
                        }
                        //item.SetValue(entity, Convert.ChangeType(row[item.Name] == DBNull.Value ? null : row[item.Name], item.PropertyType), null);
                    }
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public static T Row2Entitie<T>(DataRow lvdr) where T : new()
        {
            T entity = new T();
            foreach (var item in entity.GetType().GetProperties())
            {
                if (!item.CanWrite)
                {
                    continue;
                }
                if (!Convert.IsDBNull(lvdr[item.Name]))
                {
                    item.SetValue(entity, lvdr[item.Name], null);
                }
                //item.SetValue(entity, Convert.ChangeType(lvdr[item.Name] == DBNull.Value ? null : lvdr[item.Name], item.PropertyType), null);
            }
            return entity;
        }

        public static void Entitie2Row<T>(T rec, ref DataRow lvdrv) where T : new()
        {
            PropertyInfo[] oProps = typeof(T).GetProperties();

            foreach (PropertyInfo pi in oProps)
            {
                lvdrv[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
            }
            lvdrv.EndEdit();
            //lvdrv.AcceptChanges();
        }

        /* public static IList<T> ConvertToModel(DataTable dt)
         {
             // ���弯��    
             IList<T> ts = new List<T>();

             // ��ô�ģ�͵�����   
             Type type = typeof(T);
             string tempName = "";

             foreach (DataRow dr in dt.Rows)
             {
                 T t = new T();
                 // ��ô�ģ�͵Ĺ�������      
                 PropertyInfo[] propertys = t.GetType().GetProperties();
                 foreach (PropertyInfo pi in propertys)
                 {
                     tempName = pi.Name;  // ���DataTable�Ƿ��������    

                     if (dt.Columns.Contains(tempName))
                     {
                         // �жϴ������Ƿ���Setter      
                         if (!pi.CanWrite) continue;

                         object value = dr[tempName];
                         if (value != DBNull.Value)
                             pi.SetValue(t, value, null);
                     }
                 }
                 ts.Add(t);
             }
             return ts;
         }*/

        public static DataTable EntitiesToDataTable<T>(this IEnumerable<T> varlist) where T : class
        {
            DataTable dtReturn = new DataTable(typeof(T).Name); //���modelʱ������ʹ��Name ����T
            List<PropertyInfo> oProps = typeof(T).GetProperties().ToList();
            //���˸������ݽṹ
            for (int lvi = oProps.Count - 1; lvi >= 0; lvi--)
            {
                if (oProps[lvi].PropertyType.FullName.Contains("System.Collections.Generic.List"))
                {
                    oProps.RemoveAt(lvi);
                }
            }
            foreach (PropertyInfo pi in oProps)
            {
                Type colType = pi.PropertyType;

                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
            }
            //Ϊ�˼򻯱�����ݵı��棬���༭״̬��UI���õ����ֶΡ��ύ����ʱ������״̬��ѡ����ɾ�Ĳ�����
            //DataColumn acol = new DataColumn(PC�������.Col��ɾ��״̬��, typeof(int));
            //acol.DefaultValue = PC�������.i_browse_unchange;
            //acol.AllowDBNull = false;
            //dtReturn.Columns.Add(acol);
            if (varlist != null)
            {
                foreach (T rec in varlist)
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
            return (dtReturn);
        }

        public static void InitComboBoxByListString(ComboBox aComboBox, List<string> aSource)
        {
            aComboBox.Items.Clear();
            foreach (string lvs in aSource)
            {
                aComboBox.Items.Add(lvs);
            }
        }

        public static void InitComboBoxByListString(ComboBox aComboBox, List<string> aSource, string aFirstRow)
        {
            aComboBox.Items.Clear();
            aComboBox.Items.Add(aFirstRow);
            foreach (string lvs in aSource)
            {
                aComboBox.Items.Add(lvs);
            }
        }

        /// <summary>
        /// һ����������ʹ�ã����뽫��PC_combo������Ϣ����Ϊtagʹ�á�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void comboBox��������_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox lvCombo = sender as ComboBox;
            if (lvCombo == null)
            {
                return;
            }
            PC_combo������Ϣ lvinfo = lvCombo.Tag as PC_combo������Ϣ;
            string valuefilter = lvCombo.Text;
            if (lvinfo.���ֶΰ�)
            {
                DataTable lvdt = (lvCombo.DataSource as BindingSource).DataSource as DataTable;
                bool lv���� = false;
                if (lvCombo.SelectedValue == null)
                {
                    lvinfo.�¼��ؼ�������.Filter = "1=2";
                    return;
                }
                foreach (DataRow lvdr in lvdt.Rows)
                {
                    if (lvdr[lvCombo.ValueMember].ToString() == lvCombo.SelectedValue.ToString())
                    {
                        lv���� = true;
                        if (lvdt.Columns[lvinfo.��������ֵ_����ֵ].DataType == typeof(System.String))
                        {
                            valuefilter = "'" + lvdr[lvinfo.��������ֵ_����ֵ].ToString() + "'";
                        }
                        else
                        {
                            valuefilter = lvdr[lvinfo.��������ֵ_����ֵ].ToString();
                        }
                        lvinfo.�¼��ؼ�������.Filter = lvinfo.�¼����_������������ֵ + "=" + valuefilter;
                        break;
                    }
                }
                if (!lv����)
                {
                    lvinfo.�¼��ؼ�������.Filter = "1=2";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(lvCombo.Text.Trim()))
                {
                    lvinfo.�¼��ؼ�������.Filter = "1=2";
                }
                else
                {
                    lvinfo.�¼��ؼ�������.Filter = lvinfo.�¼����_������������ֵ + "=" + "'" + lvCombo.Text.Trim() + "'";
                }
            }
        }

        /// <summary>
        /// IE�汾��
        /// </summary>
        /// <returns></returns>
        public static int ieVersion()
        {
            //IE�汾��
            RegistryKey mreg = Registry.LocalMachine;
            mreg = mreg.CreateSubKey("SOFTWARE\\Microsoft\\Internet Explorer");

            //���°汾
            var svcVersion = mreg.GetValue("svcVersion");
            if (svcVersion != null)
            {
                mreg.Close();
                var v = svcVersion.ToString().Split('.')[0];
                return int.Parse(v);
            }
            else
            {
                //Ĭ�ϰ汾
                var ieVersion = mreg.GetValue("Version");
                mreg.Close();
                if (ieVersion != null)
                {
                    var v = ieVersion.ToString().Split('.')[0];
                    return int.Parse(v);
                }
            }

            return 0;
        }

        /// <summary>
        /// ����IE�汾�� ����Emulationֵ
        /// </summary>
        /// <param name="ieVersion"></param>
        /// <returns></returns>
        public static int ieVersionEmulation(int ieVersion)
        {
            //IE7 7000 (0x1B58)
            if (ieVersion < 8)
            {
                return 0;
            }

            if (ieVersion == 8)
            {
                return 0x1F40; //8000 (0x1F40)��8888 (0x22B8)
            }

            if (ieVersion == 9)
            {
                return 0x2328; //9000 (0x2328)��9999 (0x270F)
            }
            else if (ieVersion == 10)
            {
                return 0x02710; //10000 (0x02710)��10001 (0x2711)
            }
            else if (ieVersion == 11)
            {
                return 0x2AF8; //11000 (0x2AF8)��11001 (0x2AF9
            }

            return 0;
        }

        public static void pt_���������п�_��һ�����(DataGridView lvDGV)
        {
            lvDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            int lv�����п�� = 0;
            for (int lvIndex = 1; lvIndex < lvDGV.Columns.Count - 1; lvIndex++)
            {
                lv�����п�� += lvDGV.Columns[lvIndex].Width;
            }
            lvDGV.Columns[0].Width = lvDGV.ClientSize.Width - lv�����п��- 56;
            lvDGV.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            lvDGV.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

        }
    }

    public class Format_checkStatu : IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(CheckState))
                return this;
            else
                return null;
        }

        //public bool? Format(string fmt, object arg, IFormatProvider formatProvider)
        //{

        //    if (arg == null)
        //    { return null;}
        //    else if (!(arg is CheckBox))
        //    { return null; }
        //    else 
        //    {
        //        CheckBox lvc = arg as CheckBox;
        //        if (lvc.CheckState ==CheckState.Checked)
        //        {return true;}
        //        else if (lvc.CheckState ==CheckState.Unchecked)
        //        { return false; }
        //        else if ()
        //    }
        //}
    }

    public class LocalLoInfo
    {
        public bool ��ס�˺�;
        public bool �Զ���¼;
        public string user;
        public string pws;
    }

    public class PC_combo������Ϣ
    {
        public bool ���ֶΰ� = false;
        public BindingSource �����ؼ�������;
        public string ��������ֵ_����ֵ;
        public string ������ʾֵ;
        public string �������ֶ�;
        public string �¼����_������������ֵ;
        public string �¼�����_����ֵ;
        public string �¼���ʾֵ;
        public string �¼����ֶ�;
        public BindingSource �¼��ؼ�������;
        public string ѡ��ʱ���������ʾ;
        public object tag;
        public BindingSource ����Դ;
    }

    internal class UpDownLoadFile
    {
        /// <summary>
        /// WebClient�ϴ��ļ�����������������������
        /// </summary>
        /// <param name="fileNameFullPath">Ҫ�ϴ����ļ���ȫ·����ʽ��</param>
        /// <param name="strUrlDirPath">Web�������ļ���·��</param>
        /// <returns>True/False�Ƿ��ϴ��ɹ�</returns>
        public bool UpLoadFile(string fileNameFullPath, string strUrlDirPath)
        {
            //�õ�Ҫ�ϴ����ļ��ļ���
            string fileName = fileNameFullPath.Substring(fileNameFullPath.LastIndexOf("\\") + 1);
            //���ļ�����������ʱ���뼰�������
            string NewFileName = DateTime.Now.ToString("yyyyMMddhhmmss")
                                 + DateTime.Now.Millisecond.ToString()
                                 + fileNameFullPath.Substring(fileNameFullPath.LastIndexOf("."));
            //�õ��ļ���չ��
            string fileNameExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
            if (strUrlDirPath.EndsWith("/") == false) strUrlDirPath = strUrlDirPath + "/";
            //�����ڷ�������ʱ�����ļ�������ʾҵ����Ҫ��
            strUrlDirPath = strUrlDirPath + NewFileName;
            // ����WebClientʵ��
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            // ��Ҫ�ϴ����ļ��򿪶����ļ���
            FileStream myFileStream = new FileStream(fileNameFullPath, FileMode.Open, FileAccess.Read);
            BinaryReader myBinaryReader = new BinaryReader(myFileStream);
            try
            {
                byte[] postArray = myBinaryReader.ReadBytes((int) myFileStream.Length);
                //��Զ��Web��ַ�����ļ���д��
                Stream postStream = myWebClient.OpenWrite(strUrlDirPath, "PUT");
                if (postStream.CanWrite)
                {
                    postStream.Write(postArray, 0, postArray.Length);
                }
                else
                {
                    //MessageBox.Show("Web�������ļ�Ŀǰ����д�룬����Web������Ŀ¼Ȩ�����ã�","ϵͳ��ʾ",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                postStream.Close(); //�ر���
                return true;
            }
            catch
            {
                //MessageBox.Show("�ļ��ϴ�ʧ�ܣ�" + exp.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// ���ط������ļ����ͻ��ˣ�������������
        /// </summary>
        /// <param name="strUrlFilePath">Ҫ���ص�Web�������ϵ��ļ���ַ��ȫ·�����磺http://www.dzbsoft.com/test.rar��</param>
        /// <param name="Dir">���ص���Ŀ¼�����λ�ã����ػ����ļ��У�</param>
        /// <returns>True/False�Ƿ��ϴ��ɹ�</returns>
        public bool DownLoadFile(string strUrlFilePath, string strLocalDirPath)
        {
            // ����WebClientʵ��
            WebClient client = new WebClient();
            //�����ص��ļ���
            string fileName = strUrlFilePath.Substring(strUrlFilePath.LastIndexOf("/"));
            //���Ϊ�ľ���·�����ļ���
            string Path = strLocalDirPath + fileName;
            try
            {
                WebRequest myWebRequest = WebRequest.Create(strUrlFilePath);
            }
            catch (Exception exp)
            {
                MessageBox.Show("�ļ�����ʧ�ܣ�" + exp.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            try
            {
                client.DownloadFile(strUrlFilePath, Path);
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show("�ļ�����ʧ�ܣ�" + exp.Message, "ϵͳ��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }


        /// <summary>
        /// ���ش����������루��ͨ��������
        /// </summary>
        /// <param name="URL">��ַ</param>
        /// <param name="Filename">�ļ���</param>
        /// <param name="Prog">��ͨ������ProgressBar</param>
        /// <returns>True/False�Ƿ����سɹ�</returns>
        public bool DownLoadFile(string URL, string Filename, ProgressBar Prog)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest) System.Net.HttpWebRequest.Create(URL); //��URL��ַ�õ�һ��WEB����   
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse) Myrq.GetResponse(); //��WEB����õ�WEB��Ӧ   
                long totalBytes = myrp.ContentLength; //��WEB��Ӧ�õ����ֽ���   
                Prog.Maximum = (int) totalBytes; //�����ֽ����õ������������ֵ   
                System.IO.Stream st = myrp.GetResponseStream(); //��WEB���󴴽���������   
                System.IO.Stream so = new System.IO.FileStream(Filename, System.IO.FileMode.Create); //�����ļ�����д��   
                long totalDownloadedByte = 0; //�����ļ���С   
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int) by.Length); //����   
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte; //�����ļ���С   
                    Application.DoEvents();
                    so.Write(by, 0, osize); //д��   
                    Prog.Value = (int) totalDownloadedByte; //���½�����   
                    osize = st.Read(by, 0, (int) by.Length); //����   
                }
                so.Close(); //�ر���
                st.Close(); //�ر���
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// ���ش�����������(״̬��ʽ��������
        /// </summary>
        /// <param name="URL">��ַ</param>
        /// <param name="Filename">�ļ���</param>
        /// <param name="Prog">״̬��ʽ������ToolStripProgressBar</param>
        /// <returns>True/False�Ƿ����سɹ�</returns>
        public bool DownLoadFile(string URL, string Filename, ToolStripProgressBar Prog)
        {
            try
            {
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest) System.Net.HttpWebRequest.Create(URL); //��URL��ַ�õ�һ��WEB����   
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse) Myrq.GetResponse(); //��WEB����õ�WEB��Ӧ   
                long totalBytes = myrp.ContentLength; //��WEB��Ӧ�õ����ֽ���   
                Prog.Maximum = (int) totalBytes; //�����ֽ����õ������������ֵ   
                System.IO.Stream st = myrp.GetResponseStream(); //��WEB���󴴽���������   
                System.IO.Stream so = new System.IO.FileStream(Filename, System.IO.FileMode.Create); //�����ļ�����д��   
                long totalDownloadedByte = 0; //�����ļ���С   
                byte[] by = new byte[1024];
                int osize = st.Read(by, 0, (int) by.Length); //����   
                while (osize > 0)
                {
                    totalDownloadedByte = osize + totalDownloadedByte; //�����ļ���С   
                    Application.DoEvents();
                    so.Write(by, 0, osize); //д��   
                    Prog.Value = (int) totalDownloadedByte; //���½�����   
                    osize = st.Read(by, 0, (int) by.Length); //����   
                }
                so.Close(); //�ر���   
                st.Close(); //�ر���   
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// �������ݵĸ��ڵ��ж���ʽ
    /// </summary>
    public enum ParentStyle
    {
        /// <summary>
        /// �ϼ�Ϊ0
        /// </summary>
        isZero = 0,

        /// <summary>
        /// ������ֵ���ڸ�����ֵ
        /// </summary>
        isSame = 1,

        /// <summary>
        /// ���������У��ж�
        /// </summary>
        Iteration = 2
    };

    /// <summary>
    /// �����ڵ�ƥ��״��
    /// </summary>
    public static class PC_Tri_State
    {
        /// <summary>
        /// ���κ�ƥ��
        /// </summary>
        public static int NON = 0;

        /// <summary>
        /// ����ƥ��
        /// </summary>
        public static int part = 1;

        /// <summary>
        /// ȫ��ƥ��
        /// </summary>
        public static int all = 2;
    };

    /// <summary>
    /// �����tree����
    /// </summary>
    public class PC������ش���
    {
        bool isDataloading = false;

        //todo ���ڵ���̬����
        /// <summary>
        /// check������̬��.
        /// </summary>
        public bool pv_isCheck3State { get; set; }

        /// <summary>
        /// ����Դ ����datatable��������䷺�͡��򻯴��롣���ƣ�ֻ�ܵ��ֶ���������
        /// </summary>
        public BindingSource pv_bs { get; set; }

        /// <summary>
        /// ��Ҫ����Ŀؼ�
        /// </summary>
        public TreeView pv_Tree { get; set; }

        /// <summary>
        /// �������ݣ������ݵ��ֶ���
        /// </summary>
        public string pv_ParentKey { get; set; }

        /// <summary>
        /// �����ݵ�����
        /// </summary>
        public string pv_SelfKey { get; set; }

        /// <summary>
        /// ��ʾ�ַ����ֶ�
        /// </summary>
        public string pv_TextKey { get; set; }

        public ParentStyle pv_ParentStyle { get; set; }

        /// <summary>
        /// �Ƚ��Ӽ�ʱ��ƥ���Ӽ��������ֶ�����
        /// </summary>
        public string pv_CompareKey { get; set; }

        public BindingSource pv_CompareBS { get; set; }

        public ImageList pv_CheckPic = new ImageList();

        /// <summary>
        /// ����aBS��ʼ������չʾ��
        /// </summary>
        /// <param name="aBS"></param>
        /// <param name="aTreeView"></param>
        /// <param name="aParentKey"></param>
        /// <param name="aPvSelfKey"></param>
        /// <param name="aTextKey"></param>
        public PC������ش���(BindingSource aBS, TreeView aTreeView, string aParentKey, string aPvSelfKey, string aTextKey, string CompareKey, BindingSource aCompBS, ParentStyle aHowFindRoot = ParentStyle.isZero)
        {
            pv_bs = aBS;
            pv_Tree = aTreeView;
            pv_ParentKey = aParentKey;
            pv_SelfKey = aPvSelfKey;
            pv_TextKey = aTextKey;
            pv_CompareBS = aCompBS;
            pv_isCheck3State = false;
            pv_ParentStyle = aHowFindRoot;
            pv_CompareKey = CompareKey;
            aTreeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(TreeViewNodeMouseClick);
            aTreeView.AfterCheck += new TreeViewEventHandler(TreeViewNodeAfterCheck);
            InitTree();
        }

        private void InitTree()
        {
            DataRowView lvdrv = null;
            DataRowView lvѰ���� = null;
            TreeNode lvtn = null;
            pv_Tree.Nodes.Clear();

            pv_Tree.CheckBoxes = !pv_isCheck3State;
            if (!pv_isCheck3State)
            {
                pv_Tree.ImageList = null;
            }
            //List<TreeNode> lv������ = new List<TreeNode>();
            // for (int lvi = 0; lvi < pv_bs.Count; lvi++)
            //{

            // }

            //���ҳ����и��ڵ�
            string lv��ǰ�ڵ�ĸ� = "";
            string lv��ǰ�ڵ� = "";
            bool lv��ǰ�ڵ��Ǹ� = false;
            for (int lvi = 0; lvi < pv_bs.Count; lvi++)
            {
                lv��ǰ�ڵ��Ǹ� = false;
                lvdrv = pv_bs[lvi] as DataRowView;
                lv��ǰ�ڵ� = lvdrv[pv_SelfKey].ToString().ToLower();
                lv��ǰ�ڵ�ĸ� = lvdrv[pv_ParentKey].ToString().ToLower();
                if (pv_ParentStyle == ParentStyle.Iteration)
                {
                    pv_bs.Filter = pv_ParentKey + " = '" + lv��ǰ�ڵ� + "' ";
                    pv_bs.MoveFirst();
                    for (int lvj = 0; lvj < pv_bs.Count; lvj++)
                    {
                        lvѰ���� = pv_bs[lvj] as DataRowView;
                        if (lv��ǰ�ڵ� == lvѰ����[pv_SelfKey].ToString().ToLower())
                        {
                            continue;
                        }
                        if (lv��ǰ�ڵ�ĸ� == lvѰ����[pv_SelfKey].ToString().ToLower())
                        {
                            lv��ǰ�ڵ��Ǹ� = true;
                            break;
                        }
                    }
                }
                else if (pv_ParentStyle == ParentStyle.isSame)
                {
                    lv��ǰ�ڵ��Ǹ� = lv��ǰ�ڵ� != lv��ǰ�ڵ�ĸ�;
                }
                else
                {
                    lv��ǰ�ڵ��Ǹ� = lv��ǰ�ڵ�ĸ� == "0";
                }

                if (lv��ǰ�ڵ��Ǹ�)
                {
                    lvdrv = pv_bs[lvi] as DataRowView;
                    lvtn = new TreeNode();
                    lvtn.Text = lvdrv[pv_TextKey].ToString();
                    lvtn.Tag = lvdrv;
                    if (!pv_isCheck3State)
                    {
                        lvtn.ImageIndex = 0;
                    }
                    pv_Tree.Nodes.Add(lvtn);
                }
            }
            //ÿ�����ڵ���ӽڵ��ҵ���
            foreach (TreeNode lvttn in pv_Tree.Nodes)
            {
                SetChild(lvttn);
            }
            pv_bs.RemoveFilter();
        }

        private void SetChild(TreeNode ParentNode)
        {
            DataRowView lvdrv = (ParentNode.Tag as DataRowView);
            string lvParentValue = lvdrv[pv_SelfKey].ToString();


            if (lvdrv[pv_ParentKey].GetType().FullName.ToString() == "System.String")
            {
                pv_bs.Filter = pv_ParentKey + "='" + lvParentValue + "'";
            }
            else
            {
                pv_bs.Filter = pv_ParentKey + "=" + lvParentValue + "";
            }
            pv_bs.MoveFirst();
            TreeNode lvtn = null;
            for (int lvi = 0; lvi < pv_bs.Count; lvi++)
            {
                lvdrv = pv_bs[lvi] as DataRowView;
                if (lvdrv[pv_ParentKey].ToString() == lvdrv[pv_SelfKey].ToString())
                {
                    continue;
                }
                if (lvdrv[pv_ParentKey].ToString() == lvParentValue)
                {
                    lvtn = new TreeNode();

                    lvtn.Text = lvdrv[pv_TextKey].ToString();
                    lvtn.Tag = lvdrv;
                    lvtn.ImageIndex = 0;
                    ParentNode.Nodes.Add(lvtn);
                }
            }
            foreach (TreeNode lvSon in ParentNode.Nodes)
            {
                SetChild(lvSon);
            }
        }

        /// <summary>
        /// �򹴵�ǰѡ������нڵ�
        /// </summary>
        public void CompareShowCheck()
        {
            if (isDataloading)
            {
                return;
            }
            if (pv_CompareBS == null)
            {
                return;
            }
            try
            {
                isDataloading = true;

                List<int> lv����ƥ���� = new List<int>();
                foreach (TreeNode lvTopNode in pv_Tree.Nodes)
                {
                    CompareChild(lvTopNode);
                }
                isDataloading = false;
            }
            catch
            {
                isDataloading = false;
                throw;
            }
        }

        private int CompareChild(TreeNode lvTRN)
        {
            int lvRet = PC_Tri_State.NON;
            List<int> lvSubMR = new List<int>();
            //�����ȱȽϺ��ӽ��״̬��
            foreach (TreeNode lvTopNode in lvTRN.Nodes)
            {
                int lvCurMr = CompareChild(lvTopNode);
                lvSubMR.Add(lvCurMr);
            }
            bool lvNON = true;
            bool lvAll = false;
            if (lvSubMR.Count > 0) //�����ӽڵ�ʱ��ѡ��״̬������������Դ�������������ӽڵ�Ĺ�ѡ״����
            {
                lvNON = true;
                lvAll = true;
                foreach (int lvCurMr in lvSubMR)
                {
                    if ((lvCurMr == PC_Tri_State.all) || (lvCurMr == PC_Tri_State.part))
                    {
                        lvNON = false;
                    }

                    if ((lvCurMr == PC_Tri_State.NON) || (lvCurMr == PC_Tri_State.part))
                    {
                        lvAll = false;
                    }
                }
                if ((!lvNON) && (!lvAll))
                {
                    lvRet = PC_Tri_State.part;
                }
                else if (lvNON && lvAll)
                {
                    lvRet = PC_Tri_State.NON;
                }
                else if (lvNON)
                {
                    lvRet = PC_Tri_State.NON;
                }
                else
                {
                    lvRet = PC_Tri_State.all;
                }
            }
            else //û���ӽڵ㣬��������ӽڵ㡣��ֻ��������Դ���ж���
            {
                lvNON = true;
                lvAll = true;
                foreach (var lvCB in pv_CompareBS)
                {
                    DataRowView lvCBS = lvCB as DataRowView;
                    if (lvCBS[pv_CompareKey].ToString() == ((DataRowView) lvTRN.Tag)[pv_SelfKey].ToString())
                    {
                        lvNON = false;
                    }
                    else
                    {
                        lvAll = false;
                    }
                }
                if ((!lvNON) && (!lvAll))
                {
                    lvRet = PC_Tri_State.part;
                }
                else if (lvNON && lvAll)
                {
                    lvRet = PC_Tri_State.NON;
                }
                else if (lvNON)
                {
                    lvRet = PC_Tri_State.NON;
                }
                else
                {
                    lvRet = PC_Tri_State.all;
                }
            }
            lvTRN.Checked = (lvRet != PC_Tri_State.NON);
            if (lvTRN.TreeView.ImageList != null)
            {
                if (lvTRN.TreeView.ImageList.Images.Count > 2)
                {
                    lvTRN.ImageIndex = lvRet;
                }
            }

            return lvRet;
        }

        /// <summary>
        /// ֻҪ�¼��й�ѡ�������ڵ���㹴ѡ��������ѡҲ�㹴ѡ��
        /// </summary>
        /// <returns></returns>
        public List<DataRowView> GetSeleted()
        {
            List<DataRowView> lvRet = new List<DataRowView>();
            if (pv_isCheck3State)
            {
                foreach (TreeNode lvRoot in pv_Tree.Nodes)
                {
                    if (lvRoot.ImageIndex != (int) PC_Tri_State.NON)
                    {
                        if (lvRoot.Tag != null)
                        {
                            lvRet.Add((DataRowView) lvRoot.Tag);
                        }
                    }
                    getSelectedSubLeaves(lvRoot, lvRet);
                }
            }
            else
            {
                foreach (TreeNode lvRoot in pv_Tree.Nodes)
                {
                    if (lvRoot.Checked)
                    {
                        if (lvRoot.Tag != null)
                        {
                            lvRet.Add((DataRowView) lvRoot.Tag);
                        }
                    }
                    getSelectedSubLeaves(lvRoot, lvRet);
                }
            }
            return lvRet;
        }

        private void GetSelectedSub(TreeNode parent, ref List<DataRowView> lvRet)
        {
            if (pv_isCheck3State)
            {
                foreach (TreeNode lvtn in parent.Nodes)
                {
                    if (lvtn.ImageIndex != PC_Tri_State.NON)
                    {
                        if (lvtn.Tag != null)
                        {
                            lvRet.Add((DataRowView) lvtn.Tag);
                        }
                    }
                    GetSelectedSub(lvtn, ref lvRet);
                }
            }
            else
            {
                foreach (TreeNode lvtn in parent.Nodes)
                {
                    if (lvtn.Checked)
                    {
                        if (lvtn.Tag != null)
                        {
                            lvRet.Add((DataRowView) lvtn.Tag);
                        }
                        GetSelectedSub(lvtn, ref lvRet);
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ���б�ѡ���Ҷ�ӽڵ㡣���ڵ�İ���������datarowview[]
        /// </summary>
        public List<DataRowView> GetSelectedLeaves()
        {
            List<DataRowView> lvRet = new List<DataRowView>();
            if (pv_isCheck3State)
            {
                foreach (TreeNode lvRoot in pv_Tree.Nodes)
                {
                    if (lvRoot.Nodes.Count > 0)
                    {
                        getSelectedSubLeaves(lvRoot, lvRet);
                    }
                    else
                    {
                        if (lvRoot.ImageIndex != (int) PC_Tri_State.NON)
                        {
                            if (lvRoot.Tag != null)
                            {
                                lvRet.Add((DataRowView) lvRoot.Tag);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (TreeNode lvRoot in pv_Tree.Nodes)
                {
                    if (lvRoot.Nodes.Count > 0)
                    {
                        getSelectedSubLeaves(lvRoot, lvRet);
                    }
                    else
                    {
                        if (lvRoot.Checked)
                        {
                            if (lvRoot.Tag != null)
                            {
                                lvRet.Add((DataRowView) lvRoot.Tag);
                            }
                        }
                    }
                }
            }
            return lvRet;
        }

        private void getSelectedSubLeaves(TreeNode lvTRN, List<DataRowView> aResult)
        {
            if (lvTRN.Nodes.Count > 0) //�Ǳ��ڵ�Ͳ�������
            {
                foreach (TreeNode lvsub in lvTRN.Nodes)
                {
                    getSelectedSubLeaves(lvsub, aResult);
                }
            }
            else
            {
                if (pv_isCheck3State)
                {
                    if (lvTRN.ImageIndex != (int) PC_Tri_State.NON)
                    {
                        if (lvTRN.Tag != null)
                        {
                            aResult.Add((DataRowView) lvTRN.Tag);
                        }
                    }
                }
                else
                {
                    if (lvTRN.Checked)
                    {
                        if (lvTRN.Tag != null)
                        {
                            aResult.Add((DataRowView) lvTRN.Tag);
                        }
                    }
                }
            }
        }

        private void TreeViewNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e == null)
            {
                return;
            }
            if (e.Node == null)
            {
                return;
            }
            TreeView lvTR = sender as TreeView;
            TreeViewHitTestInfo info = lvTR.HitTest(e.Location);
            //todo �����Ҫ��ԭ����ѡ��̬����Ҫ��ԭ֮ǰ��״̬�������Ҫ���档�Ժ�ʵ��
            if ((info.Location == TreeViewHitTestLocations.Image))
            {
                if ((e.Node.ImageIndex == PC_Tri_State.NON) || (e.Node.ImageIndex == PC_Tri_State.part))
                {
                    e.Node.ImageIndex = PC_Tri_State.all;
                }
                else
                {
                    e.Node.ImageIndex = PC_Tri_State.NON;
                }
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
                Application.DoEvents();

                UpdateChildrenWithParentValue(e.Node);
                UpdateParent(e.Node);
            }
        }

        private void TreeViewNodeAfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isDataloading)
            {
                return;
            }
            isDataloading = true;
            try
            {
                if (e == null)
                {
                    return;
                }
                if (e.Node == null)
                {
                    return;
                }
                TreeView lvTR = sender as TreeView;

                UpdateChildrenWithParentValue(e.Node);
                UpdateParent(e.Node);
                isDataloading = false;
            }
            catch
            {
                isDataloading = false;
                throw;
            }
        }

        /// <summary>
        /// ���ݵ�ǰ���Ĺ�ѡ״̬���ݹ����������ӽڵ��״̬
        /// </summary>
        /// <param name="parent"></param>
        private void UpdateChildrenWithParentValue(TreeNode parent)
        {
            int state = parent.ImageIndex;
            if ((state == (int) PC_Tri_State.NON)
                || (state == (int) PC_Tri_State.all))
            {
                foreach (TreeNode node in parent.Nodes)
                {
                    node.ImageIndex = state;
                    if (node.Nodes.Count != 0)
                    {
                        UpdateChildrenWithParentValue(node);
                    }
                }
            }
            foreach (TreeNode node in parent.Nodes)
            {
                node.Checked = parent.Checked;
                if (node.Nodes.Count != 0)
                {
                    UpdateChildrenWithParentValue(node);
                }
            }
        }

        /// <summary>
        /// ���ݵ�ǰ�ڵ��״̬�����µݹ�����ϼ��ڵ㡣
        /// </summary>
        /// <param name="child"></param>
        private void UpdateParent(TreeNode child)
        {
            TreeNode parent = child.Parent;
            if (parent == null)
            {
                return;
            }

            if (child.ImageIndex == (int) PC_Tri_State.part)
            {
                parent.ImageIndex = child.ImageIndex;
            }
            else
            {
                bool allCheck = true;
                bool allNon = true;
                int lvPartCheck = (int) PC_Tri_State.part;
                int lvAllCheck = (int) PC_Tri_State.all;
                int lvNonCheck = (int) PC_Tri_State.NON;
                foreach (TreeNode lvChild in parent.Nodes)
                {
                    if (lvChild.ImageIndex == lvAllCheck)
                    {
                        allNon = false;
                    }
                    if (lvChild.ImageIndex == lvNonCheck)
                    {
                        allCheck = false;
                    }
                }
                if (!(allCheck || allNon))
                {
                    parent.ImageIndex = lvPartCheck;
                }
                else if (allCheck)
                {
                    parent.ImageIndex = lvAllCheck;
                }
                else
                {
                    parent.ImageIndex = lvNonCheck;
                }
            }
            if (child.Checked)
            {
                parent.Checked = true;
            }
            else
            {
                bool lvFound = false;
                foreach (TreeNode lvChild in parent.Nodes)
                {
                    if (lvChild.Checked)
                    {
                        lvFound = true;
                        break;
                    }
                }
                parent.Checked = lvFound;
            }
            UpdateParent(parent);
        }

        public List<DataRowView> GetAllLeaves()
        {
            List<DataRowView> lvRet = new List<DataRowView>();
            foreach (TreeNode lvRoot in pv_Tree.Nodes)
            {
                if (lvRoot.Nodes.Count <= 0)
                {
                    lvRet.Add(lvRoot.Tag as DataRowView);
                }
                else
                {
                    GetSubLeaves(lvRoot, ref lvRet);
                }
            }
            return lvRet;
        }

        private void GetSubLeaves(TreeNode lvParent, ref List<DataRowView> lvRet)
        {
            foreach (TreeNode lvSon in lvParent.Nodes)
            {
                if (lvSon.Nodes.Count <= 0)
                {
                    lvRet.Add(lvSon.Tag as DataRowView);
                }
                else
                {
                    GetSubLeaves(lvSon, ref lvRet);
                }
            }
        }

        ////check if user click on the state image
        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    base.OnMouseClick(e);
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        TreeViewHitTestInfo info = base.HitTest(e.Location);
        //        if (info.Node != null && info.Location == TreeViewHitTestLocations.StateImage)
        //        {
        //            TreeNode node = info.Node;
        //            switch (node.ImageIndex)
        //            {
        //                case STATE_UNCHECKED:
        //                case STATE_MIXED:
        //                    node.ImageIndex = STATE_CHECKED;
        //                    break;
        //                case STATE_CHECKED:
        //                    node.ImageIndex = STATE_UNCHECKED;
        //                    break;
        //            }
        //            UpdateChildren(node);
        //            UpdateParent(node);
        //        }
        //    }
        //}

        ////check if user press the space key
        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);
        //    if (e.KeyCode == Keys.Space)
        //    {
        //        if (base.SelectedNode != null)
        //        {
        //            TreeNode node = base.SelectedNode;
        //            switch (node.ImageIndex)
        //            {
        //                case STATE_UNCHECKED:
        //                case STATE_MIXED:
        //                    node.ImageIndex = STATE_CHECKED;
        //                    break;
        //                case STATE_CHECKED:
        //                    node.ImageIndex = STATE_UNCHECKED;
        //                    break;
        //            }
        //            UpdateChildren(node);
        //            UpdateParent(node);
        //        }
        //    }
        //}

        ////swap between enabled and disabled images
        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    base.OnEnabledChanged(e);

        //    for (int i = 0; i < 3; i++)
        //    {
        //        Image img = this.StateImageList.Images[0];
        //        this.StateImageList.Images.RemoveAt(0);
        //        this.StateImageList.Images.Add(img);
        //    }
        //}
    }


    ///// <summary>
    ///// Я���������ݵ�ͨ����
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class TreeContainer_txd <T> where T:class,new()
    //{

    //    private List<TreeNode_txd<T>> NodeRoots = new List<TreeNode_txd<T>>();
    //    public void AddNode(TreeNode_txd<T> anode)
    //    {
    //        NodeRoots.Add(anode);
    //    }

    //    public void AddSon(TreeNode_txd<T> Parent, TreeNode_txd<T> sub)
    //    {
    //        if ((object.ReferenceEquals(Parent,sub))||(object.ReferenceEquals(Parent.Data, sub.Data)))
    //        {
    //            throw new Exception("��ֹ���ڵ�ĸ�ָ���Լ�,����Ҳ�����ص���");
    //        }

    //        foreach (TreeNode_txd<T> lvNode in  NodeRoots)
    //        {
    //            //if(Parent)
    //        }
    //    }
    //}

    //public class TreeNode_txd<T>:Object
    //{
    //    private List<TreeNode_txd<T>> NodeRoots = new List<TreeNode_txd<T>>();
    //    public TreeNode_txd(T data)
    //    {
    //        this.Data = data;
    //        nodes = new List<TreeNode_txd<T>>();
    //    }


    //    /// <summary>
    //    /// �����
    //    /// </summary>
    //    public TreeNode_txd<T> Parent
    //    {
    //        get ; 
    //    }
    //    /// <summary>
    //    /// �������
    //    /// </summary>
    //    public T Data { get; set; }

    //    private List<TreeNode_txd<T>> nodes;
    //    /// <summary>
    //    /// �ӽ��
    //    /// </summary>
    //    public List<TreeNode_txd<T>> Nodes
    //    {
    //        get { return nodes; }
    //    }
    //    /// <summary>
    //    /// ��ӽ��
    //    /// </summary>
    //    /// <param name="nodeTxd">���</param>
    //    public void AddNode(TreeNode_txd<T> nodeTxd)
    //    {
    //        if (!nodes.Contains(nodeTxd))
    //        {
    //            nodeTxd.parent = this;
    //            nodes.Add(nodeTxd);
    //        }
    //    }
    //    /// <summary>
    //    /// ��ӽ��
    //    /// </summary>
    //    /// <param name="nodes">��㼯��</param>
    //    public void AddNode(List<TreeNode_txd<T>> nodes)
    //    {
    //        foreach (var node in nodes)
    //        {
    //            if (!nodes.Contains(node))
    //            {
    //                node.parent = this;
    //                nodes.Add(node);
    //            }
    //        }
    //    }
    //    /// <summary>
    //    /// �Ƴ����
    //    /// </summary>
    //    /// <param name="nodeTxd"></param>
    //    public void Remove(TreeNode_txd<T> nodeTxd)
    //    {
    //        if (nodes.Contains(nodeTxd))
    //            nodes.Remove(nodeTxd);
    //    }
    //    /// <summary>
    //    /// ��ս�㼯��
    //    /// </summary>
    //    public void RemoveAll()
    //    {
    //        nodes.Clear();
    //    }
    //}
}