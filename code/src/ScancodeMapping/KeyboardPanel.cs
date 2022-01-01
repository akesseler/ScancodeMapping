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

using System.Drawing;
using System.Windows.Forms;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    public partial class KeyboardPanel : UserControl
    {
        private FunctionKeyPanel functionKeyPanel = null;
        private StandardKeyPanel standardKeyPanel = null;
        private ControlKeyPanel controlKeyPanel = null;
        private NumericKeyPanel numericKeyPanel = null;

        public KeyboardPanel(TKeyboardAlignment alignment)
        {
            Point offset = new Point(10, 10);

            InitializeComponent();

            this.SuspendLayout();

            this.functionKeyPanel = new FunctionKeyPanel("FunctionKeyPanel", 0, offset, alignment, this);
            this.standardKeyPanel = new StandardKeyPanel("StandardKeyPanel", 1, offset, alignment, this);
            this.controlKeyPanel = new ControlKeyPanel("ControlKeyPanel", 2, offset, alignment, this);
            this.numericKeyPanel = new NumericKeyPanel("NumericKeyPanel", 3, offset, alignment, this);

            this.Controls.Add(this.functionKeyPanel);
            this.Controls.Add(this.standardKeyPanel);
            this.Controls.Add(this.controlKeyPanel);
            this.Controls.Add(this.numericKeyPanel);

            // Use a border around this panel.
            // this.BorderStyle = BorderStyle.FixedSingle;

            // Panel offset: 2x(X/Y) for the space around this panel plus 2pt for the border!
            this.Size = new Size(
                2 * offset.X + 2 + functionKeyPanel.Size.Width + controlKeyPanel.Size.Width + numericKeyPanel.Size.Width,
                2 * offset.Y + 2 + functionKeyPanel.Size.Height + standardKeyPanel.Size.Height
            );

            this.functionKeyPanel.ResumeLayout(false);
            this.standardKeyPanel.ResumeLayout(false);
            this.controlKeyPanel.ResumeLayout(false);
            this.numericKeyPanel.ResumeLayout(false);
            this.ResumeLayout(false);

            // Set LED's initial state.
            NumLock(KeyStates.NumLock);
            CapsLock(KeyStates.CapsLock);
            ScrollLock(KeyStates.ScrollLock);
        }

        public void NumLock(bool ledOn)
        {
            numericKeyPanel.NumLock(ledOn);
        }

        public void CapsLock(bool ledOn)
        {
            numericKeyPanel.CapsLock(ledOn);
        }

        public void ScrollLock(bool ledOn)
        {
            numericKeyPanel.ScrollLock(ledOn);
        }

        public void KeyboardLayoutChanged(TKeyboardLayout layout)
        {
            if (layout == TKeyboardLayout.Standard)
            {
                if (numericKeyPanel.Visible || controlKeyPanel.Visible)
                {
                    numericKeyPanel.Visible = false;
                    controlKeyPanel.Visible = false;

                    // Calculate new layout dimensions.
                    int width = this.Size.Width - (numericKeyPanel.Size.Width + controlKeyPanel.Size.Width);
                    int height = this.Size.Height; // Don't change the height!
                    this.Size = new Size(width, height);
                }
            }
            else if (layout == TKeyboardLayout.Enhanced)
            {
                if (!numericKeyPanel.Visible || !controlKeyPanel.Visible)
                {
                    numericKeyPanel.Visible = true;
                    controlKeyPanel.Visible = true;

                    // Restore original layout dimensions.
                    int width = this.Size.Width + (numericKeyPanel.Size.Width + controlKeyPanel.Size.Width);
                    int height = this.Size.Height; // Don't change the height!
                    this.Size = new Size(width, height);
                }
            }

            App.GetMainForm().FitWindowLayout();
        }

        public void KeyboardLayoutChanged(TKeyboardKeys keys)
        {
            standardKeyPanel.SetKeys(keys);
        }

        public void KeyboardLayoutChanged(TKeyboardAlignment alignment)
        {
            standardKeyPanel.SetAlignment(alignment);
        }

        public void UpdateMappings(ScancodeMap mappings)
        {
            for (int index = 0; index < this.Controls.Count; index++)
            {
                if (this.Controls[index] is KeyPanel)
                {
                    ((KeyPanel)this.Controls[index]).UpdateMappings(mappings);
                }
            }
        }

        public void CollectMappings(ScancodeMap mappings)
        {
            if (mappings == null) { return; }

            for (int index = 0; index < this.Controls.Count; index++)
            {
                if (this.Controls[index] is KeyPanel)
                {
                    ((KeyPanel)this.Controls[index]).CollectMappings(mappings);
                }
            }
        }

        public KeyButton FindButtonByScancode(int scancode, int extended)
        {
            for (int outer = 0; outer < this.Controls.Count; outer++)
            {
                if (this.Controls[outer] is KeyPanel)
                {
                    KeyPanel panel = (KeyPanel)this.Controls[outer];
                    for (int inner = 0; inner < panel.Controls.Count; inner++)
                    {
                        if (panel.Controls[inner] is KeyButton)
                        {
                            KeyButton button = (KeyButton)panel.Controls[inner];
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
            for (int outer = 0; outer < this.Controls.Count; outer++)
            {
                if (this.Controls[outer] is KeyPanel)
                {
                    KeyPanel panel = (KeyPanel)this.Controls[outer];
                    for (int inner = 0; inner < panel.Controls.Count && panel.Visible; inner++)
                    {
                        if (panel.Controls[inner] is KeyButton)
                        {
                            KeyButton button = (KeyButton)panel.Controls[inner];
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
