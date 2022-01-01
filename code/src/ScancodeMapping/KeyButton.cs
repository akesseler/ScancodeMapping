using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    #region Key Button Implementation
    public class KeyButton : Button
    {
        private KeyScan keyscan;
        private KeyButtonData defaults;
        private string tooltip;

        private Color defaultColor = Color.Transparent;
        private Color activeColor = Color.FromArgb(232, 232, 232);
        private Color disableColor = Color.FromArgb(255, 128, 128);
        private Color mappingColor = Color.FromArgb(128, 255, 128);
        private KeyPanel parent = null;
        private bool active = false;

        private static KeyButton restoreButton = new KeyButton("RESTORE", "-- Restore --");
        private static KeyButton disabledButton = new KeyButton("DISABLE", "-- Disable --");

        //
        // Summary:
        //
        public KeyButton(string name, string text)
            : base()
        {
            this.keyscan = null;
            this.defaults = null;
            this.tooltip = "";
            this.parent = null;
            this.Name = name;
            this.Text = text;
            this.BackColor = defaultColor;
            this.TabIndex = 0;
            this.TabStop = false;

            // Property realy needed!
            this.Defaults = new KeyButtonData(0, 0, 0, 0, false, false, new KeyScan(VirtualKeys.VK_UNUSED, 0x00, 0x00));
        }

        //
        // Summary:
        //
        public KeyButton(string name, string text, int index, KeyPanel parent)
            : base()
        {
            this.keyscan = null;
            this.defaults = null;
            this.tooltip = "";
            this.parent = parent;
            this.Name = name;
            this.Text = text;
            this.BackColor = defaultColor;

            if (index == -1)
            {
                this.TabIndex = 0;
                this.TabStop = false;
            }
            else
            {
                this.TabIndex = index;
            }

            // Make font a bit smaller
            this.Font = new Font(this.Font.Name, this.Font.SizeInPoints, FontStyle.Bold);

            this.FlatStyle = FlatStyle.Standard;
            this.Margin = new Padding(0);
            this.UseVisualStyleBackColor = true;

            this.MouseMove += new MouseEventHandler(KeyButton_MouseMove);
            this.MouseEnter += new EventHandler(KeyboardButton_MouseEnter);
            this.MouseLeave += new EventHandler(KeyboardButton_MouseLeave);
            this.Enter += new EventHandler(KeyButton_Enter);
        }

        //
        // Summary:
        //
        public KeyButton(string name, string text, int index, string tooltip, KeyPanel parent) :
            this(name, text, index, parent)
        {
            this.tooltip = tooltip;
        }

        //
        // Summary:
        //
        public static KeyButton RestoreButton
        {
            get { return KeyButton.restoreButton; }
        }

        //
        // Summary:
        //
        public static KeyButton DisabledButton
        {
            get { return KeyButton.disabledButton; }
        }

        //
        // Summary:
        //
        public KeyScan Keyscan
        {
            get { return this.keyscan; }
            set { this.keyscan = value; }
        }

        //
        // Summary:
        //
        public KeyButtonData Defaults
        {
            get { return defaults; }
            set { this.defaults = value; RestoreDefaults(); }
        }

        //
        // Summary:
        //
        public string Tooltip
        {
            get { return this.tooltip; }
            set { this.tooltip = value; }
        }

        //
        // Summary:
        //
        public bool Active
        {
            get { return active; }
            set { active = value; this.AdaptLayout(); }
        }

        //
        // Summary:
        //
        public void PrepareTooltip()
        {
            if (this.parent != null)
            {
                this.Tooltip = KeyButton.MakeMappingInfo(
                    this,
                    this.parent.FindButtonByScancode(
                        this.Keyscan.MappedScancode,
                        this.Keyscan.MappedExtended
                    )
                );
                this.parent.Tooltip.SetToolTip(this, this.Tooltip);
            }
            else
            {
                this.Tooltip = KeyButton.MakeMappingInfo(this, null);
            }
        }

        //
        // Summary:
        //
        public void RestoreDefaults()
        {
            this.Location = defaults.Location;
            this.Size = defaults.Size;
            this.Keyscan = defaults.Keyscan;
            this.Visible = defaults.Visible;
        }

        //
        // Summary:
        //
        public void AdaptLayout()
        {
            if (this.Keyscan.IsDiabled)
            {
                this.BackColor = this.disableColor;
            }
            else if (this.Keyscan.HasMapping)
            {
                this.BackColor = this.mappingColor;
            }
            else if (this.Active)
            {
                this.BackColor = this.activeColor;
            }
            else
            {
                this.BackColor = this.defaultColor;
            }
        }

        //
        // Summary:
        //
        public void RemapButton(KeyButton button)
        {
            if (button.Equals(KeyButton.RestoreButton))
            {
                this.Keyscan.ResetMapping();
            }
            else if (button.Equals(KeyButton.DisabledButton))
            {
                this.Keyscan.DisableMapping();
            }
            else
            {
                this.Keyscan.RemapMapping(
                    button.Keyscan.Scancode,
                    button.Keyscan.Extended
                );
            }

            this.AdaptLayout();
            this.PrepareTooltip();

            // Set application dirty because of changes on keyboard key mapping.
            App.GetMainForm().Dirty = true;
        }

        //
        // Summary:
        //
        public void RemapButton(int scancode, int extended)
        {
            if (scancode == 0 && extended == 0)
            {
                this.Keyscan.DisableMapping();
            }
            else
            {
                this.Keyscan.RemapMapping(
                    scancode,
                    extended
                );
            }
            this.AdaptLayout();
            this.PrepareTooltip();

            // Set application dirty because of changes on keyboard key mapping.
            App.GetMainForm().Dirty = true;
        }
        //
        // Summary:
        //
        private void KeyButton_Enter(object sender, EventArgs e)
        {
            if (null != App.GetMainForm())
            {
                App.GetMainForm().SelectedButtonChanged(this);
            }
        }

        //
        // Summary:
        //
        private void KeyButton_MouseMove(object sender, MouseEventArgs mouseArgs)
        {
            // Use left mouse button only with CTRL key to start with Drag&Drop!
            if (((mouseArgs.Button & MouseButtons.Left) == MouseButtons.Left) &&
                ((Control.ModifierKeys == Keys.Control)))
            {
                KeyButtonDragDropHelper helper = new KeyButtonDragDropHelper(this);

                helper.DoDragDrop();

                if (helper.DropTarget != null)
                {
                    // Link this to drop targen.
                    this.RemapButton(helper.DropTarget);
                }
                else
                {
                    if (this.Keyscan.HasMapping)
                    {
                        // Remove button link.
                        this.RemapButton(KeyButton.RestoreButton);
                    }
                    else
                    {
                        // Disable button.
                        this.RemapButton(KeyButton.DisabledButton);
                    }
                }
            }
        }

        //
        // Summary:
        //
        private void KeyboardButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is KeyButton)
            {
                KeyButton button = (KeyButton)sender;

                string status = "Key: \"" + button.ToString().ToUpper() + "\"  " +
                                "(vk=0x" + button.Keyscan.VKeyCode.ToString("X2") +
                                ", sc=0x" + button.Keyscan.Scancode.ToString("X2") +
                                ", ex=0x" + button.Keyscan.Extended.ToString("X2") + ")";

                App.GetMainForm().StatusbarChanged(status);
            }
        }

        //
        // Summary:
        //
        private void KeyboardButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is KeyButton)
            {
                App.GetMainForm().StatusbarChanged("");
            }
        }

        //
        // Summary:
        //
        protected override void OnKeyUp(KeyEventArgs evt)
        {
            // Call base class event handler.
            base.OnKeyUp(evt);

            // Escalate incoming event to the parent panel.
            if (this.Parent is KeyPanel)
            {
                ((KeyPanel)this.Parent).HandleKeyUpEvent(evt);
            }
        }

        //
        // Summary:
        //
        public static string MakeMappingInfo(KeyButton oriButton, KeyButton mapButton)
        {
            string result = "";

            if (oriButton != null)
            {
                int helper = (oriButton.Keyscan.Extended << 8) | oriButton.Keyscan.Scancode;
                result = VirtualKeys.Name(oriButton.Keyscan.VKeyCode) + " (0x" + helper.ToString("X4") + ")";

                if (oriButton.Keyscan.IsDiabled)
                {
                    result += " -> disabled";
                }
                else if (oriButton.Keyscan.HasMapping)
                {
                    if (mapButton != null)
                    {
                        helper = (mapButton.Keyscan.Extended << 8) | mapButton.Keyscan.Scancode;
                        result += " -> " + VirtualKeys.Name(mapButton.Keyscan.VKeyCode) + " (0x" + helper.ToString("X4") + ")";
                    }
                    else
                    {
                        helper = (oriButton.Keyscan.MappedExtended << 8) | oriButton.Keyscan.MappedScancode;
                        result += " -> mapped (0x" + helper.ToString("X4") + ")";
                    }
                }
            }
            return result;
        }

        //
        // Summary:
        //
        public override string ToString()
        {
            string result;

            if (this.Keyscan.VKeyCode == VirtualKeys.VK_UNUSED)
            {
                result = this.Text;
            }
            else
            {
                if (VirtualKeys.Fulltext(this.Keyscan.VKeyCode) != "")
                {
                    result = VirtualKeys.Fulltext(this.Keyscan.VKeyCode);
                }
                else
                {
                    result = this.Text;
                }

                if (this.Keyscan.VKeyCode == VirtualKeys.VK_RETURN && this.Keyscan.Extended != 0)
                {
                    result += " (NUM Pad)";
                }
            }

            return result;
        }
    }
    #endregion

    #region Key Button Data Implementation
    public class KeyButtonData
    {
        private Point location;
        private Size size;
        private bool visible;
        private bool convert;
        private KeyScan keyscan;

        //
        // Summary:
        //
        public KeyButtonData(int left, int top, int width, int height, bool visible, bool convert, KeyScan keyscan)
        {
            this.location = new Point(left, top);
            this.size = new Size(width, height);
            this.visible = visible;
            this.convert = convert;
            this.keyscan = keyscan;
        }

        //
        // Summary:
        //
        public Point Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        //
        // Summary:
        //
        public Size Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        //
        // Summary:
        //
        public bool Visible
        {
            get { return visible; }
            set { this.visible = value; }
        }

        //
        // Summary:
        //
        public bool Convert
        {
            get { return convert; }
            set { this.convert = value; }
        }

        //
        // Summary:
        //
        public KeyScan Keyscan
        {
            get { return this.keyscan; }
            set { this.keyscan = value; }
        }
    }
    #endregion

    #region Key Button Sorter Implementation
    public class KeyButtonSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            return x.ToString().ToLower().CompareTo(y.ToString().ToLower());
        }
    }
    #endregion

    #region Key Button Drag & Drop Helper Implementation
    public class KeyButtonDragDropHelper : UserControl
    {
        // Required designer variable.
        private System.ComponentModel.IContainer components = null;

        // Drag source reference.
        private KeyButton dragSource = null;
        private KeyButton dropTarget = null;

        //
        // Summary:
        //      Standard constructor
        //
        public KeyButtonDragDropHelper()
        {
            this.SuspendLayout();

            this.AllowDrop = true;
            this.BackColor = Color.FromArgb(224, 224, 255);
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Name = "KeyButtonDragDropHelper";
            this.Size = new Size(32, 12);
            this.Margin = new Padding(0);
            this.Visible = false;

            this.DragOver += new System.Windows.Forms.DragEventHandler(this.KeyButtonDragDropHelper_DragOver);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.KeyButtonDragDropHelper_DragDrop);
            this.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.KeyButtonDragDropHelper_GiveFeedback);

            this.dragSource = null;

            this.ResumeLayout(false);
        }

        //
        // Summary:
        //      Constructor with drag source parameter.
        //
        public KeyButtonDragDropHelper(KeyButton dragSource)
            : this()
        {
            this.dragSource = dragSource;
        }

        //
        // Summary:
        //      Clean up any resources being used.
        //
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //
        // Summary:
        //
        public KeyButton DragSource
        {
            get { return this.dragSource; }
        }


        //
        // Summary:
        //
        public KeyButton DropTarget
        {
            get { return this.dropTarget; }
        }

        //
        // Summary:
        //
        // Exceptions:
        //      ArgumentNullException
        public void DoDragDrop()
        {
            if (this.DragSource == null)
            {
                throw new ArgumentNullException("Drag Source");
            }

            Form dragForm = this.DragSource.FindForm();
            if (dragForm == null)
            {
                throw new ArgumentNullException("Drag Source Form");
            }

            // Add control to parent form.
            dragForm.Controls.Add(this);

            // Set control top-most!
            dragForm.FindForm().Controls.SetChildIndex(this, 0);

            // Show control.
            this.Visible = true;

            // Begin drag & drop operation.
            DragDropEffects dropEffects = this.DoDragDrop(this, DragDropEffects.Link);

            // Hide control.
            this.Visible = false;

            // Remove control from parent form.
            dragForm.Controls.Remove(this);
            this.Dispose();
        }

        //
        // Summary:
        //
        private void KeyButtonDragDropHelper_DragDrop(object sender, DragEventArgs dragArgs)
        {
            // Prepare result effects.
            dragArgs.Effect = DragDropEffects.None;

            KeyButtonDragDropHelper me = sender as KeyButtonDragDropHelper;

            if (me != null)
            {
                // Hide control to get underling component.
                me.Visible = false;

                // Get control under current mouse position.
                KeyButton target = Control.FromHandle(
                    WindowFromPoint(Control.MousePosition)
                ) as KeyButton;

                if (target != null && !target.Equals(me.DragSource))
                {
                    // Assign target.
                    me.dropTarget = target;

                    // Set result effects.
                    dragArgs.Effect = DragDropEffects.Link;
                }

                // Re-show control.
                me.Visible = true;
            }
        }

        //
        // Summary:
        //
        private void KeyButtonDragDropHelper_GiveFeedback(object sender, GiveFeedbackEventArgs feedbackArgs)
        {
            KeyButtonDragDropHelper me = sender as KeyButtonDragDropHelper;

            if (me != null && me.Parent != null)
            {
                // Get current mouse position.
                Point location = Control.MousePosition;

                // Calculatte and set control's new location.
                location.X -= me.Size.Width / 2;
                location.Y -= me.Size.Height / 2;

                me.Location = me.Parent.PointToClient(location);
            }
        }

        //
        // Summary:
        //
        private void KeyButtonDragDropHelper_DragOver(object sender, DragEventArgs dragArgs)
        {
            if (!dragArgs.Data.GetDataPresent(typeof(KeyButtonDragDropHelper)))
            {
                dragArgs.Effect = DragDropEffects.None;
                return;
            }
            else if ((dragArgs.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
            {
                dragArgs.Effect = DragDropEffects.Link;
            }
        }

        //
        // Summary:
        //
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point point);
    }
    #endregion

    #region Key Scan Implementation
    public class KeyScan
    {
        private int vkeycode;
        private int scancode;
        private int extended;
        private int mappedScancode;
        private int mappedExtended;
        private bool hasMapping;

        //
        // Summary:
        //
        public KeyScan()
        {
            this.vkeycode = 0;
            this.scancode = 0;
            this.extended = 0;
            this.mappedScancode = 0;
            this.mappedExtended = 0;
            this.hasMapping = false;
        }

        //
        // Summary:
        //
        public KeyScan(int vkeycode, int scancode, int extended)
            : this()
        {
            this.vkeycode = vkeycode;
            this.scancode = scancode;
            this.extended = extended;
        }

        //
        // Summary:
        //
        public int VKeyCode
        {
            get { return this.vkeycode; }
            set { this.vkeycode = value; }
        }

        //
        // Summary:
        //
        public int Scancode
        {
            get { return this.scancode; }
            set { this.scancode = value; }
        }

        //
        // Summary:
        //
        public int Extended
        {
            get { return this.extended; }
            set { this.extended = value; }
        }

        //
        // Summary:
        //
        public int MappedScancode
        {
            get { return this.mappedScancode; }
            set { this.mappedScancode = value; }
        }

        //
        // Summary:
        //
        public int MappedExtended
        {
            get { return mappedExtended; }
            set { mappedExtended = value; }
        }

        //
        // Summary:
        //
        public bool HasMapping
        {
            get { return this.hasMapping; }
            set { this.hasMapping = value; }
        }

        //
        // Summary:
        //
        public bool IsDiabled
        {
            get
            {
                // Can only be disabled if mepping is used!
                if (this.HasMapping)
                {
                    return (this.MappedScancode == 0 && this.MappedExtended == 0);
                }
                else
                {
                    return false;
                }
            }
        }

        //
        // Summary:
        //
        public void ResetMapping()
        {
            this.MappedScancode = 0;
            this.MappedExtended = 0;
            this.HasMapping = false;
        }

        //
        // Summary:
        //
        public void DisableMapping()
        {
            this.MappedScancode = 0;
            this.MappedExtended = 0;
            this.HasMapping = true;
        }

        //
        // Summary:
        //
        public void RemapMapping(int mapScancode, int mapExtended)
        {
            this.MappedScancode = mapScancode;
            this.MappedExtended = mapExtended;
            this.HasMapping = true;
        }

        //
        // Summary:
        //
        public override string ToString()
        {
            return "VK: 0x" + this.vkeycode.ToString("X2") + ", " +
                   "SC: 0x" + this.scancode.ToString("X2") + ", " +
                   "EX: 0x" + this.extended.ToString("X2");
        }
    }
    #endregion
}
