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
using System.Drawing;
using System.Windows.Forms;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    public partial class KeyboardPanel : UserControl
    {
        private readonly FunctionKeyPanel functionKeyPanel = null;
        private readonly StandardKeyPanel standardKeyPanel = null;
        private readonly ControlKeyPanel controlKeyPanel = null;
        private readonly NumericKeyPanel numericKeyPanel = null;

        public KeyboardPanel(TKeyboardAlignment alignment)
        {
            Point offset = new Point(10, 10);

            this.InitializeComponent();

            this.SuspendLayout();

            this.functionKeyPanel = new FunctionKeyPanel("FunctionKeyPanel", 0, offset, alignment, this);
            this.standardKeyPanel = new StandardKeyPanel("StandardKeyPanel", 1, offset, alignment, this);
            this.controlKeyPanel = new ControlKeyPanel("ControlKeyPanel", 2, offset, alignment, this);
            this.numericKeyPanel = new NumericKeyPanel("NumericKeyPanel", 3, offset, alignment, this);

            this.Controls.Add(this.functionKeyPanel);
            this.Controls.Add(this.standardKeyPanel);
            this.Controls.Add(this.controlKeyPanel);
            this.Controls.Add(this.numericKeyPanel);

            // Panel offset: 2x(X/Y) for the space around this panel plus 2pt for the border!
            this.Size = new Size(
                2 * offset.X + 2 + this.functionKeyPanel.Size.Width + this.controlKeyPanel.Size.Width + this.numericKeyPanel.Size.Width,
                2 * offset.Y + 2 + this.functionKeyPanel.Size.Height + this.standardKeyPanel.Size.Height
            );

            this.functionKeyPanel.ResumeLayout(false);
            this.standardKeyPanel.ResumeLayout(false);
            this.controlKeyPanel.ResumeLayout(false);
            this.numericKeyPanel.ResumeLayout(false);
            this.ResumeLayout(false);

            // Set LED's initial state.
            this.NumLock(KeyStates.NumLock);
            this.CapsLock(KeyStates.CapsLock);
            this.ScrollLock(KeyStates.ScrollLock);
        }

        public void NumLock(Boolean ledOn)
        {
            this.numericKeyPanel.NumLock(ledOn);
        }

        public void CapsLock(Boolean ledOn)
        {
            this.numericKeyPanel.CapsLock(ledOn);
        }

        public void ScrollLock(Boolean ledOn)
        {
            this.numericKeyPanel.ScrollLock(ledOn);
        }

        public void KeyboardLayoutChanged(TKeyboardLayout layout)
        {
            if (layout == TKeyboardLayout.Standard)
            {
                if (this.numericKeyPanel.Visible || this.controlKeyPanel.Visible)
                {
                    this.numericKeyPanel.Visible = false;
                    this.controlKeyPanel.Visible = false;

                    // Calculate new layout dimensions.
                    Int32 width = this.Size.Width - (this.numericKeyPanel.Size.Width + this.controlKeyPanel.Size.Width);
                    Int32 height = this.Size.Height; // Don't change the height!
                    this.Size = new Size(width, height);
                }
            }
            else if (layout == TKeyboardLayout.Enhanced)
            {
                if (!this.numericKeyPanel.Visible || !this.controlKeyPanel.Visible)
                {
                    this.numericKeyPanel.Visible = true;
                    this.controlKeyPanel.Visible = true;

                    // Restore original layout dimensions.
                    Int32 width = this.Size.Width + (this.numericKeyPanel.Size.Width + this.controlKeyPanel.Size.Width);
                    Int32 height = this.Size.Height; // Don't change the height!
                    this.Size = new Size(width, height);
                }
            }

            App.GetMainForm().FitWindowLayout();
        }

        public void KeyboardLayoutChanged(TKeyboardKeys keys)
        {
            this.standardKeyPanel.SetKeys(keys);
        }

        public void KeyboardLayoutChanged(TKeyboardAlignment alignment)
        {
            this.standardKeyPanel.SetAlignment(alignment);
        }

        public void UpdateMappings(ScancodeMap mappings)
        {
            for (Int32 index = 0; index < this.Controls.Count; index++)
            {
                if (this.Controls[index] is KeyPanel panel)
                {
                    panel.UpdateMappings(mappings);
                }
            }
        }

        public void CollectMappings(ScancodeMap mappings)
        {
            if (mappings == null) { return; }

            for (Int32 index = 0; index < this.Controls.Count; index++)
            {
                if (this.Controls[index] is KeyPanel panel)
                {
                    panel.CollectMappings(mappings);
                }
            }
        }

        public KeyButton FindButtonByScancode(Int32 scancode, Int32 extended)
        {
            for (Int32 outer = 0; outer < this.Controls.Count; outer++)
            {
                if (this.Controls[outer] is KeyPanel panel)
                {
                    for (Int32 inner = 0; inner < panel.Controls.Count; inner++)
                    {
                        if (panel.Controls[inner] is KeyButton button)
                        {
                            if (button.Keyscan.Scancode == scancode && button.Keyscan.Extended == extended)
                            {
                                return button;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public KeyButton GetSelectedButton()
        {
            for (Int32 outer = 0; outer < this.Controls.Count; outer++)
            {
                if (this.Controls[outer] is KeyPanel panel)
                {
                    for (Int32 inner = 0; inner < panel.Controls.Count && panel.Visible; inner++)
                    {
                        if (panel.Controls[inner] is KeyButton button)
                        {
                            if (button.Focused)
                            {
                                return button;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
