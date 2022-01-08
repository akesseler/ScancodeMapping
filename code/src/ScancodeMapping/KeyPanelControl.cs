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

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    public class ControlKeyPanel : KeyPanel
    {
        private const Int32 LEFT = 629;
        private const Int32 TOP = 0;
        private const Int32 WIDTH = 168;
        private const Int32 HEIGHT = 270;

        #region Buttons' location, size and VK assignments.

        private readonly KeyButtonData[] buttonData = new KeyButtonData[]
        {
            new KeyButtonData(21,  0,   42, 42, true, false, new KeyScan(VirtualKeys.VK_SNAPSHOT, 0x37, Keyboard.EXTENDED)),
            new KeyButtonData(63,  0,   42, 42, true, false, new KeyScan(VirtualKeys.VK_SCROLL,   0x46, Keyboard.STANDARD)),
            new KeyButtonData(105, 0,   42, 42, true, false, new KeyScan(VirtualKeys.VK_PAUSE,    0x45, Keyboard.STANDARD)),
            new KeyButtonData(21,  60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_INSERT,   0x52, Keyboard.EXTENDED)),
            new KeyButtonData(63,  60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_HOME,     0x47, Keyboard.EXTENDED)),
            new KeyButtonData(105, 60,  42, 42, true, false, new KeyScan(VirtualKeys.VK_PRIOR,    0x49, Keyboard.EXTENDED)),
            new KeyButtonData(21,  102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_DELETE,   0x53, Keyboard.EXTENDED)),
            new KeyButtonData(63,  102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_END,      0x4F, Keyboard.EXTENDED)),
            new KeyButtonData(105, 102, 42, 42, true, false, new KeyScan(VirtualKeys.VK_NEXT,     0x51, Keyboard.EXTENDED)),
            new KeyButtonData(63,  186, 42, 42, true, false, new KeyScan(VirtualKeys.VK_UP,       0x48, Keyboard.EXTENDED)),
            new KeyButtonData(21,  228, 42, 42, true, false, new KeyScan(VirtualKeys.VK_LEFT,     0x4B, Keyboard.EXTENDED)),
            new KeyButtonData(63,  228, 42, 42, true, false, new KeyScan(VirtualKeys.VK_DOWN,     0x50, Keyboard.EXTENDED)),
            new KeyButtonData(105, 228, 42, 42, true, false, new KeyScan(VirtualKeys.VK_RIGHT,    0x4D, Keyboard.EXTENDED))
        };

        #endregion

        public ControlKeyPanel(String name, Int32 index, Point offset, TKeyboardAlignment alignment, KeyboardPanel parent)
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
                KeyButton button = new KeyButton("CK" + index, (index + 1).ToString(), index, this)
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
        }
    }
}
