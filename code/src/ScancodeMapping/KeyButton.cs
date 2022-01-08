/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!

    #region Key Button Implementation

    public class KeyButton : Button
    {
        private KeyScan keyscan;
        private KeyButtonData defaults;
        private String tooltip;

        private readonly Color defaultColor = Color.Transparent;
        private readonly Color activeColor = Color.FromArgb(232, 232, 232);
        private readonly Color disableColor = Color.FromArgb(255, 128, 128);
        private readonly Color mappingColor = Color.FromArgb(128, 255, 128);
        private readonly KeyPanel parent = null;
        private Boolean active = false;

        private static readonly KeyButton restoreButton = new KeyButton("RESTORE", "-- Restore --");
        private static readonly KeyButton disabledButton = new KeyButton("DISABLE", "-- Disable --");

        public KeyButton(String name, String text)
            : base()
        {
            this.keyscan = null;
            this.defaults = null;
            this.tooltip = String.Empty;
            this.parent = null;
            this.Name = name;
            this.Text = text;
            this.BackColor = this.defaultColor;
            this.TabIndex = 0;
            this.TabStop = false;

            // Property realy needed!
            this.Defaults = new KeyButtonData(0, 0, 0, 0, false, false, new KeyScan(VirtualKeys.VK_UNUSED, 0x00, 0x00));
        }

        public KeyButton(String name, String text, Int32 index, KeyPanel parent)
            : base()
        {
            this.keyscan = null;
            this.defaults = null;
            this.tooltip = String.Empty;
            this.parent = parent;
            this.Name = name;
            this.Text = text;
            this.BackColor = this.defaultColor;

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

            this.MouseMove += new MouseEventHandler(this.OnKeyButtonMouseMove);
            this.MouseEnter += new EventHandler(this.OnKeyboardButtonMouseEnter);
            this.MouseLeave += new EventHandler(this.OnKeyboardButtonMouseLeave);
            this.Enter += new EventHandler(this.OnKeyButtonEnter);
        }

        public KeyButton(String name, String text, Int32 index, String tooltip, KeyPanel parent) :
            this(name, text, index, parent)
        {
            this.tooltip = tooltip;
        }

        public static KeyButton RestoreButton
        {
            get { return KeyButton.restoreButton; }
        }

        public static KeyButton DisabledButton
        {
            get { return KeyButton.disabledButton; }
        }

        public KeyScan Keyscan
        {
            get { return this.keyscan; }
            set { this.keyscan = value; }
        }

        public KeyButtonData Defaults
        {
            get { return this.defaults; }
            set { this.defaults = value; this.RestoreDefaults(); }
        }

        public String Tooltip
        {
            get { return this.tooltip; }
            set { this.tooltip = value; }
        }

        public Boolean Active
        {
            get { return this.active; }
            set { this.active = value; this.AdaptLayout(); }
        }

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

        public void RestoreDefaults()
        {
            this.Location = this.defaults.Location;
            this.Size = this.defaults.Size;
            this.Keyscan = this.defaults.Keyscan;
            this.Visible = this.defaults.Visible;
        }

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
                this.Keyscan.RemapMapping(button.Keyscan.Scancode, button.Keyscan.Extended);
            }

            this.AdaptLayout();
            this.PrepareTooltip();

            // Set application dirty because of changes on keyboard key mapping.
            App.GetMainForm().Dirty = true;
        }

        public void RemapButton(Int32 scancode, Int32 extended)
        {
            if (scancode == 0 && extended == 0)
            {
                this.Keyscan.DisableMapping();
            }
            else
            {
                this.Keyscan.RemapMapping(scancode, extended);
            }
            this.AdaptLayout();
            this.PrepareTooltip();

            // Set application dirty because of changes on keyboard key mapping.
            App.GetMainForm().Dirty = true;
        }

        private void OnKeyButtonEnter(Object sender, EventArgs args)
        {
            if (null != App.GetMainForm())
            {
                App.GetMainForm().SelectedButtonChanged(this);
            }
        }

        private void OnKeyButtonMouseMove(Object sender, MouseEventArgs args)
        {
            // Use left mouse button only with CTRL key to start with Drag&Drop!
            if (((args.Button & MouseButtons.Left) == MouseButtons.Left) &&
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

        private void OnKeyboardButtonMouseEnter(Object sender, EventArgs args)
        {
            if (sender is KeyButton button)
            {
                String status = "Key: \"" + button.ToString().ToUpper() + "\"  " +
                                "(vk=0x" + button.Keyscan.VKeyCode.ToString("X2") +
                                ", sc=0x" + button.Keyscan.Scancode.ToString("X2") +
                                ", ex=0x" + button.Keyscan.Extended.ToString("X2") + ")";

                App.GetMainForm().StatusbarChanged(status);
            }
        }

        private void OnKeyboardButtonMouseLeave(Object sender, EventArgs args)
        {
            if (sender is KeyButton)
            {
                App.GetMainForm().StatusbarChanged(String.Empty);
            }
        }

        protected override void OnKeyUp(KeyEventArgs args)
        {
            // Call base class event handler.
            base.OnKeyUp(args);

            // Escalate incoming event to the parent panel.
            if (this.Parent is KeyPanel panel)
            {
                panel.HandleKeyUpEvent(args);
            }
        }

        public static String MakeMappingInfo(KeyButton oriButton, KeyButton mapButton)
        {
            String result = String.Empty;

            if (oriButton != null)
            {
                Int32 helper = (oriButton.Keyscan.Extended << 8) | oriButton.Keyscan.Scancode;
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

        public override String ToString()
        {
            String result;

            if (this.Keyscan.VKeyCode == VirtualKeys.VK_UNUSED)
            {
                result = this.Text;
            }
            else
            {
                if (VirtualKeys.Fulltext(this.Keyscan.VKeyCode) != String.Empty)
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
        private Boolean visible;
        private Boolean convert;
        private KeyScan keyscan;

        public KeyButtonData(Int32 left, Int32 top, Int32 width, Int32 height, Boolean visible, Boolean convert, KeyScan keyscan)
        {
            this.location = new Point(left, top);
            this.size = new Size(width, height);
            this.visible = visible;
            this.convert = convert;
            this.keyscan = keyscan;
        }

        public Point Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public Size Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        public Boolean Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        public Boolean Convert
        {
            get { return this.convert; }
            set { this.convert = value; }
        }

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
        public Int32 Compare(Object x, Object y)
        {
            return x.ToString().ToLower().CompareTo(y.ToString().ToLower());
        }
    }

    #endregion

    #region Key Button Drag & Drop Helper Implementation

    public class KeyButtonDragDropHelper : UserControl
    {
        // Required designer variable.
        private readonly System.ComponentModel.IContainer components = null;

        // Drag source reference.
        private readonly KeyButton dragSource = null;
        private KeyButton dropTarget = null;

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

            this.DragOver += new DragEventHandler(this.OnKeyButtonDragDropHelperDragOver);
            this.DragDrop += new DragEventHandler(this.OnKeyButtonDragDropHelperDragDrop);
            this.GiveFeedback += new GiveFeedbackEventHandler(this.OnKeyButtonDragDropHelperGiveFeedback);

            this.dragSource = null;

            this.ResumeLayout(false);
        }

        public KeyButtonDragDropHelper(KeyButton dragSource)
            : this()
        {
            this.dragSource = dragSource;
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public KeyButton DragSource
        {
            get { return this.dragSource; }
        }

        public KeyButton DropTarget
        {
            get { return this.dropTarget; }
        }

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

        private void OnKeyButtonDragDropHelperDragDrop(Object sender, DragEventArgs args)
        {
            // Prepare result effects.
            args.Effect = DragDropEffects.None;

            if (sender is KeyButtonDragDropHelper me)
            {
                // Hide control to get underling component.
                me.Visible = false;

                // Get control under current mouse position.

                if (Control.FromHandle(KeyButtonDragDropHelper.WindowFromPoint(Control.MousePosition)) is KeyButton target && !target.Equals(me.DragSource))
                {
                    // Assign target.
                    me.dropTarget = target;

                    // Set result effects.
                    args.Effect = DragDropEffects.Link;
                }

                // Re-show control.
                me.Visible = true;
            }
        }

        private void OnKeyButtonDragDropHelperGiveFeedback(Object sender, GiveFeedbackEventArgs args)
        {
            if (sender is KeyButtonDragDropHelper me && me.Parent != null)
            {
                // Get current mouse position.
                Point location = Control.MousePosition;

                // Calculatte and set control's new location.
                location.X -= me.Size.Width / 2;
                location.Y -= me.Size.Height / 2;

                me.Location = me.Parent.PointToClient(location);
            }
        }

        private void OnKeyButtonDragDropHelperDragOver(Object sender, DragEventArgs args)
        {
            if (!args.Data.GetDataPresent(typeof(KeyButtonDragDropHelper)))
            {
                args.Effect = DragDropEffects.None;
            }
            else if ((args.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
            {
                args.Effect = DragDropEffects.Link;
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point point);
    }

    #endregion

    #region Key Scan Implementation

    public class KeyScan
    {
        private Int32 vkeycode;
        private Int32 scancode;
        private Int32 extended;
        private Int32 mappedScancode;
        private Int32 mappedExtended;
        private Boolean hasMapping;

        public KeyScan()
        {
            this.vkeycode = 0;
            this.scancode = 0;
            this.extended = 0;
            this.mappedScancode = 0;
            this.mappedExtended = 0;
            this.hasMapping = false;
        }

        public KeyScan(Int32 vkeycode, Int32 scancode, Int32 extended)
            : this()
        {
            this.vkeycode = vkeycode;
            this.scancode = scancode;
            this.extended = extended;
        }

        public Int32 VKeyCode
        {
            get { return this.vkeycode; }
            set { this.vkeycode = value; }
        }

        public Int32 Scancode
        {
            get { return this.scancode; }
            set { this.scancode = value; }
        }

        public Int32 Extended
        {
            get { return this.extended; }
            set { this.extended = value; }
        }

        public Int32 MappedScancode
        {
            get { return this.mappedScancode; }
            set { this.mappedScancode = value; }
        }

        public Int32 MappedExtended
        {
            get { return this.mappedExtended; }
            set { this.mappedExtended = value; }
        }

        public Boolean HasMapping
        {
            get { return this.hasMapping; }
            set { this.hasMapping = value; }
        }

        public Boolean IsDiabled
        {
            get
            {
                // Can only be disabled if mapping is used!
                if (this.HasMapping)
                {
                    return this.MappedScancode == 0 && this.MappedExtended == 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public void ResetMapping()
        {
            this.MappedScancode = 0;
            this.MappedExtended = 0;
            this.HasMapping = false;
        }

        public void DisableMapping()
        {
            this.MappedScancode = 0;
            this.MappedExtended = 0;
            this.HasMapping = true;
        }

        public void RemapMapping(Int32 mapScancode, Int32 mapExtended)
        {
            this.MappedScancode = mapScancode;
            this.MappedExtended = mapExtended;
            this.HasMapping = true;
        }

        public override String ToString()
        {
            return "VK: 0x" + this.vkeycode.ToString("X2") + ", " +
                   "SC: 0x" + this.scancode.ToString("X2") + ", " +
                   "EX: 0x" + this.extended.ToString("X2");
        }
    }

    #endregion
}
