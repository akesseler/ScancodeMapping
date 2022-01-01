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
using System.Drawing;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    public class FunctionKeyPanel : KeyPanel
    {
        private const int LEFT = 0;
        private const int TOP = 0;
        private const int WIDTH = 629;
        private const int HEIGHT = 60;

        #region Buttons' location, size and VK assignments.
        private KeyButtonData[] buttonData = new KeyButtonData[]{
            new KeyButtonData( 0,   0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_ESCAPE, 0x01, Keyboard.STANDARD)),
            new KeyButtonData( 84,  0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F1,     0x3B, Keyboard.STANDARD)),
            new KeyButtonData( 126, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F2,     0x3C, Keyboard.STANDARD)),
            new KeyButtonData( 168, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F3,     0x3D, Keyboard.STANDARD)),
            new KeyButtonData( 210, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F4,     0x3E, Keyboard.STANDARD)),
            new KeyButtonData( 272, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F5,     0x3F, Keyboard.STANDARD)),
            new KeyButtonData( 314, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F6,     0x40, Keyboard.STANDARD)),
            new KeyButtonData( 356, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F7,     0x41, Keyboard.STANDARD)),
            new KeyButtonData( 398, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F8,     0x42, Keyboard.STANDARD)),
            new KeyButtonData( 461, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F9,     0x43, Keyboard.STANDARD)),
            new KeyButtonData( 503, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F10,    0x44, Keyboard.STANDARD)),
            new KeyButtonData( 545, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F11,    0x57, Keyboard.STANDARD)),
            new KeyButtonData( 587, 0, 42, 42, true, false, new KeyScan(VirtualKeys.VK_F12,    0x58, Keyboard.STANDARD))
        };
        #endregion

        //
        // Summary:
        //
        public FunctionKeyPanel(string name, int index, Point offset, TKeyboardAlignment alignment, KeyboardPanel parent)
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

        //
        // Summary:
        //
        private void Initialize(TKeyboardAlignment alignment)
        {
            for (int index = 0; index < buttonData.Length; index++)
            {
                KeyButton button = new KeyButton("FK" + index, (index + 1).ToString(), index, this);
                button.Defaults = buttonData[index];
                button.PrepareTooltip();

                if (buttonData[index].Convert)
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
