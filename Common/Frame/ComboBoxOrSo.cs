using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TXD.CF
{
    public class ComboBoxOrSo : ComboBox
    {
        private List<string> pv_MatchedList = new List<string>();
        private List<string> __checklist = new List<string>();
        public List<string> PV_checklist
        {
            get { return __checklist; }
            set
            {
                __checklist = value;
                this.Items.Clear();
                this.Items.AddRange(__checklist.ToArray());
            }
        }

        //public ComboBoxOrSo():base()
        //{
        //    //this.OnTextUpdate += new EventHandler(OrsoTextUpdate);
        //}
        protected override void OnTextUpdate(EventArgs e)
        {
            base.OnTextUpdate(e);

            this.Items.Clear();
            pv_MatchedList.Clear();
            foreach (string lvs in PV_checklist)
            {
                if (lvs.Contains(this.Text))
                { pv_MatchedList.Add(lvs); }
            }
            this.Items.AddRange(pv_MatchedList.ToArray());
            this.SelectionStart = this.Text.Length;
            Cursor = Cursors.Default;
            this.DroppedDown = true;
        }
    }
}
