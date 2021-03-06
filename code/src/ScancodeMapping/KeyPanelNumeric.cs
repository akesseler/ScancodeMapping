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

using ScancodeHook.LowLevel;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    public class NumericKeyPanel : KeyPanel
    {
        private const Int32 LEFT = 797;
        private const Int32 TOP = 0;
        private const Int32 WIDTH = 168;
        private const Int32 HEIGHT = 270;

        private PictureBox numLockBox;
        private PictureBox capsLockBox;
        private PictureBox scrollLockBox;

        #region Buttons' location, size and VK assignments.

        private readonly KeyButtonData[] buttonData = new KeyButtonData[]
        {
            new KeyButtonData(0,   60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMLOCK,  0x45, Keyboard.EXTENDED)),
            new KeyButtonData(42,  60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_DIVIDE,   0x35, Keyboard.EXTENDED)),
            new KeyButtonData(84,  60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_MULTIPLY, 0x37, Keyboard.STANDARD)),
            new KeyButtonData(126, 60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_SUBTRACT, 0x4A, Keyboard.STANDARD)),
            new KeyButtonData(0,   102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD7,  0x47, Keyboard.STANDARD)),
            new KeyButtonData(42,  102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD8,  0x48, Keyboard.STANDARD)),
            new KeyButtonData(84,  102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD9,  0x49, Keyboard.STANDARD)),
            new KeyButtonData(126, 102, 42, 84, true, false, new KeyScan(VirtualKeys.VK_ADD,      0x4E, Keyboard.STANDARD)),
            new KeyButtonData(0,   144, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD4,  0x4B, Keyboard.STANDARD)),
            new KeyButtonData(42,  144, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD5,  0x4C, Keyboard.STANDARD)),
            new KeyButtonData(84,  144, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD6,  0x4D, Keyboard.STANDARD)),
            new KeyButtonData(0,   186, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD1,  0x4F, Keyboard.STANDARD)),
            new KeyButtonData(42,  186, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD2,  0x50, Keyboard.STANDARD)),
            new KeyButtonData(84,  186, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD3,  0x51, Keyboard.STANDARD)),
            new KeyButtonData(126, 186, 42, 84, true, false, new KeyScan(VirtualKeys.VK_RETURN,   0x1C, Keyboard.EXTENDED)),
            new KeyButtonData(0,   228, 84, 42, true, false, new KeyScan(VirtualKeys.VK_NUMPAD0,  0x52, Keyboard.STANDARD)),
            new KeyButtonData(84,  228, 42, 42, true, false, new KeyScan(VirtualKeys.VK_DECIMAL,  0x53, Keyboard.STANDARD))
        };

        #endregion

        public NumericKeyPanel(String name, Int32 index, Point offset, TKeyboardAlignment alignment, KeyboardPanel parent)
            : base(parent)
        {
            this.SuspendLayout();

            this.TabIndex = index;
            this.Name = name;
            this.BackColor = Color.Transparent;
            this.Location = new Point(LEFT + offset.X, TOP + offset.Y);
            this.Size = new Size(WIDTH, HEIGHT);

            this.Initialize(alignment);
        }

        private void Initialize(TKeyboardAlignment alignment)
        {
            for (Int32 index = 0; index < this.buttonData.Length; index++)
            {
                KeyButton button = new KeyButton("NK" + index, (index + 1).ToString(), index, this)
                {
                    Defaults = this.buttonData[index]
                };

                button.PrepareTooltip();

                if (this.buttonData[index].Convert)
                {
                    button.Text = Win32Wrapper.KeyText.GetKeyTextAtOnce(alignment, button.Keyscan, button.Text);
                }
                else
                {
                    button.Text = Win32Wrapper.GetKeyNameText(button.Keyscan, button.Text);
                }
                this.Controls.Add(button);
            }

            #region Create NUM_LOCK_LED component.

            this.numLockBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.numLockBox).BeginInit();
            this.numLockBox.Location = new Point(21, 0);
            this.numLockBox.Size = new Size(42, 7);
            this.numLockBox.SizeMode = PictureBoxSizeMode.CenterImage;

            if (KeyStates.NumLock)
            {
                this.numLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDon;
            }
            else
            {
                this.numLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDoff;
            }

            this.Controls.Add(this.numLockBox);

            ((System.ComponentModel.ISupportInitialize)this.numLockBox).EndInit();

            #endregion

            #region Create CAPS_LOCK_LED component.

            this.capsLockBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.capsLockBox).BeginInit();
            this.capsLockBox.Location = new Point(63, 0);
            this.capsLockBox.Size = new Size(42, 7);
            this.capsLockBox.SizeMode = PictureBoxSizeMode.CenterImage;

            if (KeyStates.CapsLock)
            {
                this.capsLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDon;
            }
            else
            {
                this.capsLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDoff;
            }

            this.Controls.Add(this.capsLockBox);

            ((System.ComponentModel.ISupportInitialize)this.capsLockBox).EndInit();

            #endregion

            #region Create SCROLL_LOCK_LED component.

            this.scrollLockBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.scrollLockBox).BeginInit();
            this.scrollLockBox.Location = new Point(105, 0);
            this.scrollLockBox.Size = new Size(42, 7);
            this.scrollLockBox.SizeMode = PictureBoxSizeMode.CenterImage;

            if (KeyStates.ScrollLock)
            {
                this.scrollLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDon;
            }
            else
            {
                this.scrollLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDoff;
            }

            this.Controls.Add(this.scrollLockBox);

            ((System.ComponentModel.ISupportInitialize)this.scrollLockBox).EndInit();

            #endregion
        }

        public void NumLock(Boolean ledOn)
        {
            if (ledOn)
            {
                this.numLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDon;
            }
            else
            {
                this.numLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDoff;
            }
        }

        public void CapsLock(Boolean ledOn)
        {
            if (ledOn)
            {
                this.capsLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDon;
            }
            else
            {
                this.capsLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDoff;
            }
        }

        public void ScrollLock(Boolean ledOn)
        {
            if (ledOn)
            {
                this.scrollLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDon;
            }
            else
            {
                this.scrollLockBox.Image = global::ScancodeMapping.Properties.Resources.LEDoff;
            }
        }
    }
}
