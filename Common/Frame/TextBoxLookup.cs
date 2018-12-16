using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sunlord.CommonFrame
{
    [System.ComponentModel.LookupBindingProperties("DataSource", "DisplayMember", "ValueMember", "SelectedValue")]
    public partial class txbLookup : TextBox
    {
        private object _DataSource;
        private string _DisplayMember;
        private string _ValueMember;
        private string _SelectedValue;
        [AttributeProvider(typeof(IListSource))]
        public object DataSource
        {
            get { return _DataSource; }
            set { _DataSource = value; }
        }
        [TypeConverter("System.Windows.Forms.Design.DataMemberFieldConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string DisplayMember
        {
            get { return _DisplayMember; }
            set { _DisplayMember = value; }
        }
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string ValueMember
        {
            get { return _ValueMember; }
            set { _ValueMember = value; }
        }
        [DesignerSerializationVisibility(0)]
        [Browsable(false)]
        [Bindable(true)]
        [DefaultValue("")]
        public string SelectedValue
        {
            get
            {
                if (_SelectedValue != null) return _SelectedValue.ToString();
                else return "";
            }
            set { _SelectedValue = value; }
        }
        public txbLookup()
        {
            InitializeComponent();
        }
    }
}
