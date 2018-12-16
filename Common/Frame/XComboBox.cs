using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data;
using System.Text;

namespace DavidControls
{
    public delegate void BNDroppedDownEventHandler(object sender, EventArgs e);
    public delegate void BNDrawItemEventHandler(object sender, DrawItemEventArgs e);
    public delegate void BNMeasureItemEventHandler(object sender, MeasureItemEventArgs e);
    
    public class XComboBox : ListControl
    {
        #region Variables

        private bool hovered = false;
        private bool resize = false;

        private Color _backColor = Color.White;
        private Color _color1 = Color.White;
        private Color _color2 = Color.Gainsboro;
        private Color _color3 = Color.White;
        private Color _color4 = Color.PaleGoldenrod;
        private BNRadius _radius = new BNRadius();

        private int _dropDownHeight = 200;
        private int _dropDownWidth = 0;
        private int _maxDropDownItems = 8;
        
        private int _selectedIndex = -1;

        private bool _isDroppedDown = false;

        private ComboBoxStyle _dropDownStyle = ComboBoxStyle.DropDown;

        private Rectangle rectBtn = new Rectangle(0, 0, 1, 1);
        private Rectangle rectContent = new Rectangle(0, 0, 1, 1);

        private ToolStripControlHost _controlHost = null;
        private ListBox _listBox;
        //private ToolStripDropDown _popupControl = null;
        private TextBox _textBox;

        #endregion




        #region Delegates

        [Category("Behavior"), Description("Occurs when IsDroppedDown changed to True.")]
        public event BNDroppedDownEventHandler DroppedDown;

        [Category("Behavior"), Description("Occurs when the SelectedIndex property changes.")]
        public event EventHandler SelectedIndexChanged;

        [Category("Behavior"), Description("Occurs whenever a particular item/area needs to be painted.")]
        public event BNDrawItemEventHandler DrawItem;

        [Category("Behavior"), Description("Occurs whenever a particular item's height needs to be calculated.")]
        public event BNMeasureItemEventHandler MeasureItem;

        #endregion



        
        #region Properties

        public Color Color1
        {
            get { return _color1; }
            set { _color1 = value; Invalidate(true); }
        }

        public Color Color2
        {
            get { return _color2; }
            set { _color2 = value; Invalidate(true); }
        }
        
        public Color Color3
        {
            get { return _color3; }
            set { _color3 = value; Invalidate(true); }
        }

        public Color Color4
        {
            get { return _color4; }
            set { _color4 = value; Invalidate(true); }
        }

        public int DropDownHeight
        {
            get { return _dropDownHeight; }
            set { _dropDownHeight = value; }
        }
        /*
        public ListBox.ObjectCollection Items
        {
            get { return _listBox.Items; }
        }*/

        public int DropDownWidth
        {
            get { return _dropDownWidth; }
            set { _dropDownWidth = value; }
        }

        public int MaxDropDownItems
        {
            get { return _maxDropDownItems; }
            set { _maxDropDownItems = value; }
        }

        /*
        public new object DataSource
        {
            get { return base.DataSource; }
            set 
            { 
                _listBox.DataSource = value;
                base.DataSource = value;
                OnDataSourceChanged(System.EventArgs.Empty);
            }
        }*/

        public bool Soreted
        {
            get
            {
                return _listBox.Sorted;
            }
            set
            {
                _listBox.Sorted = value;
            }
        }

        [Category("Behavior"), Description("Indicates whether the code or the OS will handle the drawing of elements in the list.")]
        public DrawMode DrawMode
        {
            get { return _listBox.DrawMode; }
            set
            {
                _listBox.DrawMode = value;
            }
        }
        
        public ComboBoxStyle DropDownStyle
        {
            get { return _dropDownStyle; }
            set 
            { 
                _dropDownStyle = value; 
            
                if (_dropDownStyle == ComboBoxStyle.DropDownList)
                {
                    _textBox.Visible = false;
                }
                else
                {
                    _textBox.Visible = true;
                }
                Invalidate(true);
            }
        }


        public new Color BackColor
        {
            get { return _backColor; }
            set 
            { 
                this._backColor = value;
                _textBox.BackColor = value;
                Invalidate(true);
            }
        }
        //private bool _poped = false;
        public bool IsDroppedDown
        {
            get { return _isDroppedDown; }
            set 
            {
                _isDroppedDown = value;

                if (_isDroppedDown)
                {
                    _listBox.Items.Clear();
                    foreach (string s in items)
                    {
                        _listBox.Items.Add(s);
                        
                    }
                    showList();
                }
                else
                {
                    _listBox.Hide();
                }
            }
        }

        public BNRadius Radius
        {
            get { return _radius; }
        }

        #endregion




        #region Constructor
        public XComboBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ContainerControl, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Selectable, true);

            base.BackColor = Color.Transparent;
            /*_radius.BottomLeft = 2;
            _radius.BottomRight = 2;
            _radius.TopLeft = 2;
            _radius.TopRight = 6;*/
            _radius.BottomLeft = 0;
            _radius.BottomRight = 0;
            _radius.TopLeft = 0;
            _radius.TopRight = 0;

            this.Height = 21;
            this.Width = 95;

            this.SuspendLayout();
            _textBox = new TextBox();
            _textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _textBox.Location = new System.Drawing.Point(3, 4);
            _textBox.Size = new System.Drawing.Size(60, 13);
            _textBox.TabIndex = 0;
            _textBox.WordWrap = false;
            _textBox.Margin = new Padding(0);
            _textBox.Padding = new Padding(0);
            _textBox.TextAlign = HorizontalAlignment.Left;
            this.Controls.Add(_textBox);
            this.ResumeLayout(false);

            AdjustControls();

            _listBox = new ListBox();
            _listBox.IntegralHeight = true;
            _listBox.BorderStyle = BorderStyle.FixedSingle;
            _listBox.SelectionMode = SelectionMode.One;
            _listBox.BindingContext = new BindingContext();
            _listBox.Visible = false;

            /*
            _controlHost = new ToolStripControlHost(_listBox);
            _controlHost.Padding = new Padding(0);
            _controlHost.Margin = new Padding(0);
            _controlHost.AutoSize = false;

            _popupControl = new ToolStripDropDown();
            _popupControl.Padding = new Padding(0);
            _popupControl.Margin = new Padding(0);
            _popupControl.AutoSize = true;
            _popupControl.DropShadowEnabled = false;
            _popupControl.Items.Add(_controlHost);*/

            _dropDownWidth = this.Width;

            _listBox.MeasureItem += new MeasureItemEventHandler(_listBox_MeasureItem);
            _listBox.DrawItem += new DrawItemEventHandler(_listBox_DrawItem);
            _listBox.MouseClick += new MouseEventHandler(_listBox_MouseClick);
            _listBox.MouseMove += new MouseEventHandler(_listBox_MouseMove);

            //_popupControl.Closed += new ToolStripDropDownClosedEventHandler(_popupControl_Closed);

            //_popupControl.Click += new EventHandler(_popup_clicked);

            _textBox.Resize += new EventHandler(_textBox_Resize);
            _textBox.TextChanged += new EventHandler(_textBox_TextChanged);

            _textBox.KeyDown += new  KeyEventHandler  ( _textBox_KeyPress);

            _listBox.KeyDown += new KeyEventHandler(_listBox_KeyPress);
        }

        

        #endregion

        private void _textBox_KeyPress(object sender, KeyEventArgs e)
        {
            //int keyValue = e.KeyValue;
            //if(e.KeyChar
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.PageDown)
            {
                showList();
                _listBox.Focus();
                if (_listBox.Items.Count > 0 && _listBox.SelectedIndex < 0)
                {

                    _listBox.SelectedIndex = 0;

                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                _textBox.Focus();
                _listBox.Hide();
            }
        }

        private void _listBox_KeyPress(object sender, KeyEventArgs e)
        {
            //int keyValue = e.KeyValue;
            //if(e.KeyChar
            if (e.KeyCode == Keys.Back)
            {
                _textBox.Focus();
                //_textBox.Text = 
            }
            if (e.KeyCode == Keys.Up)
            {
                if (_listBox.Items.Count > 0 && _listBox.SelectedIndex == 0)
                {
                    //_listBox.SelectedIndex = -1;
                    //_listBox.SelectedItem = null;
                    _listBox.Hide();
                    _textBox.Focus();
                }
                //_textBox.Text = 
            }
            if (e.KeyCode == Keys.Escape)
            {
                _textBox.Focus();
                _listBox.Hide();
            }
            if (e.KeyCode == Keys.Enter)
            {
                //Selected
                //_listBox_MouseClick(null, null);
                this.SelectedIndex = _listBox.SelectedIndex;
                _listBox.Visible = false;
                _textBox.Focus();
            }
        }


        #region Overrides

        protected override void OnDataSourceChanged(EventArgs e)
        {
            this.SelectedIndex = 0;
            base.OnDataSourceChanged(e);
        }

        protected override void OnDisplayMemberChanged(EventArgs e)
        {
            _listBox.DisplayMember = this.DisplayMember;
            this.SelectedIndex = this.SelectedIndex;
            base.OnDisplayMemberChanged(e);
        }

        private Color _old_back_color = Color.Violet;
        protected override void OnEnabledChanged(EventArgs e)
        {
            _textBox.Enabled = this.Enabled;
            if (!_textBox.Enabled)
            {
                _old_back_color = _textBox.BackColor;
                _textBox.BackColor = SystemColors.Control;
            }
            else
            {
                if (_old_back_color != Color.Violet)
                    _textBox.BackColor = _old_back_color;
            }
            _textBox.Invalidate(true);
            Invalidate(true);
            base.OnEnabledChanged(e);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            _textBox.ForeColor = this.ForeColor;
            base.OnForeColorChanged(e);
        }

        protected override void OnFormatInfoChanged(EventArgs e)
        {
            _listBox.FormatInfo = this.FormatInfo;
            base.OnFormatInfoChanged(e);
        }

        protected override void OnFormatStringChanged(EventArgs e)
        {
            _listBox.FormatString = this.FormatString;
            base.OnFormatStringChanged(e);
        }

        protected override void OnFormattingEnabledChanged(EventArgs e)
        {
            _listBox.FormattingEnabled = this.FormattingEnabled;
            base.OnFormattingEnabledChanged(e);
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                resize = true;
                _textBox.Font = value;
                base.Font = value;
                Invalidate(true);
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.MouseDown += new MouseEventHandler(Control_MouseDown);
            e.Control.MouseEnter += new EventHandler(Control_MouseEnter);
            e.Control.MouseLeave += new EventHandler(Control_MouseLeave);
            e.Control.GotFocus += new EventHandler(Control_GotFocus);
            e.Control.LostFocus += new EventHandler(Control_LostFocus);
            base.OnControlAdded(e);
        }        

        protected override void OnMouseEnter(EventArgs e)
        {
            hovered = true;
            this.Invalidate(true);
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition))
            {
                hovered = false;
                Invalidate(true);
            }

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            
            //_textBox.Focus();
            if ((this.RectangleToScreen(rectBtn).Contains(MousePosition) || (DropDownStyle == ComboBoxStyle.DropDownList)) )
            {
                if (_listBox.Visible)
                {
                    //showList();
                    this.IsDroppedDown = false;
                    return;
                }
                /*else
                {
                    _listBox.Hide();
                }*/

                
                this.Invalidate(true);
                if (this.IsDroppedDown) 
                {
                    this.IsDroppedDown = false;
                }
                this.IsDroppedDown = true;
            }

            

            //this.IsDroppedDown = !this.IsDroppedDown;
            
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {

            if (! this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition) )
                hovered = false;
            else
                hovered = true;

            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta < 0)
                this.SelectedIndex = this.SelectedIndex + 1;
            else if (e.Delta > 0)
            {
                if (this.SelectedIndex > 0)
                    this.SelectedIndex = this.SelectedIndex - 1;
            }

            base.OnMouseWheel(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate(true);
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!this.ContainsFocus)
            {
                Invalidate();
            }

            base.OnLostFocus(e);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if(SelectedIndexChanged!=null)
                SelectedIndexChanged(this, e);

            base.OnSelectedIndexChanged(e);
        }

        protected override void OnValueMemberChanged(EventArgs e)
        {
            _listBox.ValueMember = this.ValueMember;
            this.SelectedIndex = this.SelectedIndex;
            base.OnValueMemberChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            if (resize)
            {

                resize = false;
                AdjustControls();

                Invalidate(true);
            }
            else
                Invalidate(true);

            if (DesignMode)
                _dropDownWidth = this.Width;
        }

        public override string Text
        {
            get
            {
                return _textBox.Text;
            }
            set
            {
                _textBox.Text = value;
                base.Text = _textBox.Text;
                OnTextChanged(EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //content border
            Rectangle rectCont = rectContent;
            rectCont.X += 1;
            rectCont.Y += 1;
            rectCont.Width -= 3;
            rectCont.Height -= 3;
            GraphicsPath pathContentBorder = CreateRoundRectangle(rectCont, Radius.TopLeft, Radius.TopRight, Radius.BottomRight,
                Radius.BottomLeft);

            
            //button border
            Rectangle rectButton = rectBtn;
            rectButton.X += 1;
            rectButton.Y += 1;
            rectButton.Width -= 3;
            rectButton.Height -= 3;
            GraphicsPath pathBtnBorder = CreateRoundRectangle(rectButton, 0, Radius.TopRight, Radius.BottomRight, 0);

            //outer border
            Rectangle rectOuter = rectContent;
            rectOuter.Width -= 1;
            rectOuter.Height -= 1;
            GraphicsPath pathOuterBorder = CreateRoundRectangle(rectOuter, Radius.TopLeft, Radius.TopRight, Radius.BottomRight,
                Radius.BottomLeft);

            //inner border
            Rectangle rectInner = rectContent;
            rectInner.X += 1;
            rectInner.Y += 1;
            rectInner.Width -= 3;
            rectInner.Height -= 3;
            GraphicsPath pathInnerBorder = CreateRoundRectangle(rectInner, Radius.TopLeft, Radius.TopRight, Radius.BottomRight,
                Radius.BottomLeft);

            //brushes and pens
            /*Brush brInnerBrush = new LinearGradientBrush(
                new Rectangle(rectInner.X,rectInner.Y,rectInner.Width,rectInner.Height+1), 
                (hovered || IsDroppedDown || ContainsFocus)?Color4:Color2, Color.Transparent,
                LinearGradientMode.Vertical);*/
            Brush brInnerBrush = new LinearGradientBrush(
                new Rectangle(rectInner.X, rectInner.Y, rectInner.Width, rectInner.Height + 1),
                Color.DodgerBlue, Color.DodgerBlue,
                LinearGradientMode.Vertical);
            Brush brBackground;
            if (this.DropDownStyle == ComboBoxStyle.DropDownList)
            {
                brBackground = new LinearGradientBrush(pathInnerBorder.GetBounds(), 
                    Color.FromArgb(IsDroppedDown ? 100 : 255, Color.White), 
                    Color.FromArgb(IsDroppedDown?255:100, BackColor),
                    LinearGradientMode.Vertical);
            }
            else
            {
                brBackground = new SolidBrush(BackColor);
            }
            if(!this.Enabled)
                brBackground = new SolidBrush( SystemColors.Control );
            Pen penOuterBorder = new Pen(Color1, 0);
            Pen penInnerBorder = new Pen(brInnerBrush, 0);
            LinearGradientBrush brButtonLeft = new LinearGradientBrush(rectBtn, Color1, Color2, LinearGradientMode.Vertical);
            ColorBlend blend = new ColorBlend();
            blend.Colors = new Color[] { Color.Transparent, Color2, Color.Transparent };
            blend.Positions = new float[] { 0.0f, 0.5f, 1.0f};
            brButtonLeft.InterpolationColors = blend;
            Pen penLeftButton = new Pen(brButtonLeft, 0);
            /*
            Brush brButton = new LinearGradientBrush(pathBtnBorder.GetBounds(),
                Color.FromArgb(100, IsDroppedDown? Color2:Color.White),
                    Color.FromArgb(100, IsDroppedDown ? Color.White : Color2),
                    LinearGradientMode.Vertical);*/
            /*
            Brush brButton = new LinearGradientBrush(pathBtnBorder.GetBounds(),
                Color.FromArgb(100, IsDroppedDown ? Color2 : Color.White),
                    Color.FromArgb(100, IsDroppedDown ? Color.White : Color.DodgerBlue),
                    LinearGradientMode.Vertical);*/
            Brush brButton = new LinearGradientBrush(pathBtnBorder.GetBounds(),
                Color.FromArgb(100,  Color.White),
                    Color.FromArgb(100, Color.DodgerBlue),
                    LinearGradientMode.Vertical);

            //draw
            e.Graphics.FillPath(brBackground, pathContentBorder);
            if (DropDownStyle != ComboBoxStyle.DropDownList)
            {
                e.Graphics.FillPath(brButton, pathBtnBorder);
            }
            //e.Graphics.DrawPath(penOuterBorder, pathOuterBorder);
            //e.Graphics.DrawPath(penInnerBorder, pathInnerBorder);

            GraphicsPath path_text = new GraphicsPath();
            path_text.AddLine(0, rectOuter.Bottom - 3, 0, rectOuter.Bottom);
            path_text.AddLine(0, rectOuter.Bottom , rectOuter.Width, rectOuter.Bottom);
            path_text.AddLine(rectOuter.Width, rectOuter.Bottom, rectOuter.Width, rectOuter.Bottom - 3);

            e.Graphics.DrawPath(penInnerBorder, path_text);
            //e.Graphics.DrawPath(penInnerBorder, pathOuterBorder);

            //e.Graphics.DrawLine(penLeftButton, rectBtn.Left + 1, rectInner.Top+1, rectBtn.Left + 1, rectInner.Bottom-1);
            

            //Glimph
            Rectangle rectGlimph = rectButton;
            rectButton.Width -= 4;
            e.Graphics.TranslateTransform(rectGlimph.Left + rectGlimph.Width / 2.0f, rectGlimph.Top + rectGlimph.Height / 2.0f);
            GraphicsPath path = new GraphicsPath();
            PointF[] points = new PointF[3];
            points[0] = new PointF(-6 / 2.0f, -3 / 2.0f);
            points[1] = new PointF(6 / 2.0f, -3 / 2.0f);
            points[2] = new PointF(0, 6 / 2.0f);
            path.AddLine(points[0], points[1]);
            path.AddLine(points[1], points[2]);
            path.CloseFigure();
            e.Graphics.RotateTransform(0);

            //SolidBrush br = new SolidBrush(Enabled?Color.Gray:Color.Gainsboro);
            SolidBrush br = new SolidBrush(Color.Gray);
            e.Graphics.FillPath(br, path);
            e.Graphics.ResetTransform();
            br.Dispose();
            path.Dispose();


            //text
            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                StringFormat sf  = new StringFormat(StringFormatFlags.NoWrap);
                sf.Alignment = StringAlignment.Near;

                Rectangle rectText = _textBox.Bounds;
                rectText.Offset(-3, 0);

                SolidBrush foreBrush = new SolidBrush(ForeColor);
                if (Enabled)
                {
                    e.Graphics.DrawString(_textBox.Text, this.Font, foreBrush, rectText.Location);
                }
                else
                {
                    ControlPaint.DrawStringDisabled(e.Graphics, _textBox.Text, Font, BackColor, rectText, sf);
                }
            }
            /*
            Dim foreBrush As SolidBrush = New SolidBrush(color)
            If (enabled) Then
                g.DrawString(text, font, foreBrush, rect, sf)
            Else
                ControlPaint.DrawStringDisabled(g, text, font, backColor, _
                     rect, sf)
            End If
            foreBrush.Dispose()*/


            pathContentBorder.Dispose();
            pathOuterBorder.Dispose();
            pathInnerBorder.Dispose();
            pathBtnBorder.Dispose();

            penOuterBorder.Dispose();
            penInnerBorder.Dispose();
            penLeftButton.Dispose();

            brBackground.Dispose();
            brInnerBrush.Dispose();
            brButtonLeft.Dispose();
            brButton.Dispose();
        }

        #endregion




        #region ListControlOverrides

        public override int SelectedIndex
        {
             get {
                 if (SelectedValue == null)
                     return -1;
                 return _selectedIndex; 
             }
            set 
            { 
                if(_listBox != null)
                {
                    if (_listBox.Items.Count == 0)
                        return;
                    if (value == -1)
                    {
                        _textBox.Text = "";
                        _listBox.SelectedValue = value;
                    }

                    if ((this.DataSource != null) && value == -1)
                    {
                        OnSelectedIndexChanged(EventArgs.Empty);
                        return;
                    }

                    if (value <= (_listBox.Items.Count - 1) && value >= -1)
                    {
                        _listBox.SelectedIndex = value;
                        _selectedIndex = value;
                        _textBox.Text = _listBox.GetItemText(_listBox.SelectedItem);
                        OnSelectedIndexChanged(EventArgs.Empty);
                    }
                }
            }
        }

        public object SelectedItem
        {
            get { return _listBox.SelectedItem;  }
            set 
            { 
                _listBox.SelectedItem = value;
                this.SelectedIndex = _listBox.SelectedIndex;
            }
        }
        private string _displaymember = null;
        int max_str_len = 0;
        private void _add_dt_to_item()
        {
            max_str_len = 0;
            if (_dt != null && _displaymember != null)
            {
                items.Clear();
                foreach (DataRow row in _dt.Rows)
                {
                    items.Add(row[_displaymember].ToString());
                }
                _listBox.Items.Clear();
                foreach (string s in items)
                {
                    int l = Encoding.UTF8.GetByteCount(s);
                    if (l > max_str_len)
                        max_str_len = l;
                    _listBox.Items.Add(s);
                }

            }
        }
        new public string DisplayMember
        {
            get { return _displaymember; }
            set
            {
                _displaymember = value;
                _add_dt_to_item();
            }
        }
        private string _valuemember = null;
        new public string ValueMember
        {
            get { return _valuemember; }
            set
            {
                _valuemember = value;
            }
        }

        public new object SelectedValue
        {
            get {
                if (_dt != null)
                {
                    DataRow [] drs = null;
                    if (_listBox.SelectedValue != null)
                        drs = _dt.Select(DisplayMember + "='" + _listBox.SelectedValue + "'");
                    else
                        drs = _dt.Select(DisplayMember + "='" + _textBox.Text  + "'");
                    if (drs != null && drs.Length == 1)
                        return drs[0][ValueMember];
                    return null;
                }
                else
                    return null;
                //return base.SelectedValue; 
            }
            set {
                if (value != null)
                {
                    DataRow[] drs = null;
                    drs = _dt.Select(ValueMember + "='" + value + "'");
                    if (drs != null && drs.Length == 1)
                    {
                        _textBox.Text = drs[0][DisplayMember].ToString();
                        _listBox.SelectedValue = value;
                    }
                    return;
                }
                else
                {
                    _textBox.Text = "";
                    _listBox.SelectedValue = value;
                }
                //return base.SelectedValue; 
            }
            /*
            set
            {
                base.SelectedValue = value;
            }*/
        }

        protected override void RefreshItem(int index)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void RefreshItems()
        {
            //base.RefreshItems();
        }

        public List<string> items = new List<string>();
        private DataTable _dt = null;
        public new DataTable DataSource
        {
            set
            {
                _dt = value;
                _add_dt_to_item();
            }
            get
            {
                return _dt;
            }
        }

        protected override void SetItemCore(int index, object value)
        {
            //base.SetItemCore(index, value);
        }

        protected override void SetItemsCore(System.Collections.IList items)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        #endregion




        #region NestedControlsEvents

        void Control_LostFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }

        void Control_GotFocus(object sender, EventArgs e)
        {
            OnGotFocus(e);
        }

        void Control_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        void Control_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        void Control_MouseDown(object sender, MouseEventArgs e)
        {
            OnMouseDown(e);
        }


        void _listBox_MouseMove(object sender, MouseEventArgs e)
        {
            int i;
            for (i = 0; i < (_listBox.Items.Count); i++)
            {
                if (_listBox.GetItemRectangle(i).Contains(_listBox.PointToClient(MousePosition)))
                {
                    _listBox.SelectedIndex = i;
                    return;
                }
            }
        }

        void _listBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (_listBox.Items.Count == 0)
            {
                return;
            }

            if (_listBox.SelectedItems.Count != 1)
            {
                return;
            }

            this.SelectedIndex = _listBox.SelectedIndex;

            if (DropDownStyle == ComboBoxStyle.DropDownList)
            {
                this.Invalidate(true);
            }

            IsDroppedDown = false;
        }

        void _listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                if (DrawItem != null)
                {
                    DrawItem(this, e);
                }
            }
        }

        void _listBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (MeasureItem != null)
            {
                MeasureItem(this, e);
            }
        }


        void _popupControl_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _isDroppedDown = false;
            //_poped = false;
            if (!this.RectangleToScreen(this.ClientRectangle).Contains(MousePosition))
            {
                hovered = false;
            }
            Invalidate(true);
        }



        void _textBox_Resize(object sender, EventArgs e)
        {
            this.AdjustControls();
        }
        public class WinApiHelper
        {
            private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
            private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;

            [DllImport("user32.dll")]
            private static extern void mouse_event(
                UInt32 dwFlags, // motion and click options
                UInt32 dx, // horizontal position or change
                UInt32 dy, // vertical position or change
                UInt32 dwData, // wheel movement
                IntPtr dwExtraInfo // application-defined information
                );

            public static void SendClick(Point location)
            {
                Cursor.Position = location;
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
            }
        }
    private void  hookControlsRecursive(ControlCollection coll)
    { 
           
        foreach (Control c in coll)  
        {
            if (c != _listBox && c != _textBox)
            {
                c.MouseClick += (sender, e) =>
                {
                    if (!this.RectangleToScreen(rectBtn).Contains(MousePosition) && !this.RectangleToScreen(rectContent).Contains(MousePosition))
                    {

                        _listBox.Hide();
                    }
                };
            }
            hookControlsRecursive(c.Controls);
        }
    }
    private void hookFormClick(Form f)
    {

                f.MouseClick += (sender, e) =>
                {
                    if (!this.RectangleToScreen(rectBtn).Contains(MousePosition) && !this.RectangleToScreen(rectContent).Contains(MousePosition))
                    {

                        _listBox.Hide();
                    }
                };
    }
        private bool hooked_clicked = false;

        private void showList()
        {
            if (_listBox.Visible)
            {
                //_listBox.Width = (max_str_len * 7 > _textBox.Width ? max_str_len * 7 : _textBox.Width) + 25;
                //_listBox.Invalidate(true);
                return;
            }
            Form f = _textBox.FindForm();
            if (f != null)
            {
                

                _listBox.Parent = f;

                Point _offset_text = _textBox.PointToScreen(_textBox.Location);
                Point _offset_form = f.PointToScreen(new Point(0, 0));
                _listBox.HorizontalScrollbar = true;
                _listBox.Left = _offset_text.X - _offset_form.X - 10;
                _listBox.Top = _offset_text.Y - _offset_form.Y + _textBox.Height - 1 ;
                _listBox.Width = (max_str_len * 6 > _textBox.Width ?  max_str_len * 6: _textBox.Width) + 25;
                _listBox.Height = _textBox.Height * ((_listBox.Items.Count > 10) ? 10 : _listBox.Items.Count) + 50;
                _listBox.Visible = true;
                _listBox.BringToFront();
                if (!hooked_clicked)
                {
                    hookControlsRecursive(f.Controls);
                    hookFormClick(f);
                    hooked_clicked = true;
                }

                //SetChildIndex
                //_listBox.Top = _textBox.Top + 30;
            }
        }

        void _textBox_TextChanged(object sender, EventArgs e)
        {
            
            //this.Text = _textBox.Text;
            //MessageBox.Show(_textBox.Text);
            //_listBox.Items
            _listBox.Items.Clear();
            //max_str_len = 0;
            foreach (string s in items)
            {
                
                if(s.ToUpper().Contains(_textBox.Text.ToUpper())){
                    //int l = Encoding.UTF8.GetByteCount(s);
                    //if (l > max_str_len)
                    //    max_str_len = l;
                    _listBox.Items.Add(s);
                }
            }
            if (_textBox.Text == "")
                _listBox.Hide();
            else
                showList();
        }

        #endregion




        #region PrivateMethods

        private void AdjustControls()
        {
            this.SuspendLayout();

            resize = true;
            _textBox.Top = 4;
            _textBox.Left = 5;
            this.Height = _textBox.Top + _textBox.Height + _textBox.Top;

            rectBtn =
                    new System.Drawing.Rectangle(this.ClientRectangle.Width - 18,
                    this.ClientRectangle.Top, 18, _textBox.Height + 2 * _textBox.Top);


            _textBox.Width = rectBtn.Left - 1 - _textBox.Left;

            rectContent = new Rectangle(ClientRectangle.Left, ClientRectangle.Top,
                ClientRectangle.Width, _textBox.Height + 2 * _textBox.Top);

            this.ResumeLayout();

            Invalidate(true);
        }

        private Point CalculateDropPosition()
        {
            Point point = new Point(0, this.Height);
            if ((this.PointToScreen(new Point(0, 0)).Y + this.Height + _controlHost.Height) > Screen.PrimaryScreen.WorkingArea.Height)
            {
                point.Y = -this._controlHost.Height - 7;
            }
            return point;
        }

        private Point CalculateDropPosition(int myHeight, int controlHostHeight)
        {
            Point point = new Point(0, myHeight);
            if ((this.PointToScreen(new Point(0, 0)).Y + this.Height + controlHostHeight) > Screen.PrimaryScreen.WorkingArea.Height)
            {
                point.Y = -controlHostHeight - 7;
            }
            return point;
        }

        #endregion      



        
        #region VirtualMethods

        public virtual void OnDroppedDown(object sender, EventArgs e)
        {
            if (DroppedDown != null)
            {
                DroppedDown(this, e);
            }
        }

        #endregion

        #region Render

        public static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int topLeftRadius, int topRightRadius,
            int bottomRightRadius, int bottomLeftRadius)
        {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;

            if(topLeftRadius > 0)
            {
                path.AddArc(l, t, topLeftRadius * 2, topLeftRadius * 2, 180, 90);
            }
            path.AddLine(l + topLeftRadius, t, l + w - topRightRadius, t);
            if (topRightRadius > 0)
            {
                path.AddArc(l + w - topRightRadius * 2, t, topRightRadius * 2, topRightRadius * 2, 270, 90);
            }
            path.AddLine(l + w, t + topRightRadius, l + w, t + h - bottomRightRadius);
            if (bottomRightRadius > 0)
            {
                path.AddArc(l + w - bottomRightRadius * 2, t + h - bottomRightRadius * 2,
                    bottomRightRadius * 2, bottomRightRadius * 2, 0, 90);
            }
            path.AddLine(l + w - bottomRightRadius, t + h, l + bottomLeftRadius, t + h);
            if(bottomLeftRadius >0)
            {
                path.AddArc(l, t + h - bottomLeftRadius * 2, bottomLeftRadius * 2, bottomLeftRadius * 2, 90, 90);
            }
            path.AddLine(l, t + h - bottomLeftRadius, l, t + topLeftRadius);
            path.CloseFigure();
            return path;
        }

        #endregion
    }
    public class BNRadius
    {
        private int _topLeft = 0;

        public int TopLeft
        {
            get { return _topLeft; }
            set { _topLeft = value; }
        }

        private int _topRight = 0;

        public int TopRight
        {
            get { return _topRight; }
            set { _topRight = value; }
        }

        private int _bottomLeft = 0;

        public int BottomLeft
        {
            get { return _bottomLeft; }
            set { _bottomLeft = value; }
        }

        private int _bottomRight = 0;

        public int BottomRight
        {
            get { return _bottomRight; }
            set { _bottomRight = value; }
        }
    }
    public class ItemList : List<object>
    {
    }

    
}
