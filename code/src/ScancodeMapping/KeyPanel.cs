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
using System.Windows.Forms;

namespace ScancodeMapping
{
    // The designer requires this class as the first part of this file!
    public class KeyPanel : Panel
    {
        private readonly ToolTip buttonToolTip;
        protected KeyboardPanel parent = null;

        public KeyPanel(KeyboardPanel parent)
            : base()
        {
            this.buttonToolTip = new ToolTip();
            this.parent = parent;
        }

        public ToolTip Tooltip
        {
            get { return this.buttonToolTip; }
        }

        public void HandleKeyUpEvent(KeyEventArgs evt)
        {
            if (this.Parent is KeyboardPanel panel)
            {
                switch (evt.KeyValue)
                {
                    case VirtualKeys.VK_NUMLOCK:
                        panel.NumLock(KeyStates.NumLock);
                        break;
                    case VirtualKeys.VK_CAPITAL:
                        panel.CapsLock(KeyStates.CapsLock);
                        break;
                    case VirtualKeys.VK_SCROLL:
                        panel.ScrollLock(KeyStates.ScrollLock);
                        break;
                }
            }
        }

        public Boolean UpdateScancode(Int32 vkeycode, Int32 scancode, Int32 extended)
        {
            try
            {
                KeyButton button = this.FindButtonUseVK(vkeycode);

                button.Keyscan.Scancode = scancode;
                button.Keyscan.Extended = extended;
                button.PrepareTooltip();

                App.GetMainForm().StatusbarChanged(
                    "Keyscan" +
                    ": virtual key=0x" + vkeycode.ToString("X2") +
                    "; scancode=0x" + scancode.ToString("X2") +
                    "; extended=0x" + extended.ToString("X2")
                );

                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public void UpdateMappings(ScancodeMap mappings)
        {
            for (Int32 index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton button)
                {
                    Int32 oriScancode = button.Keyscan.Scancode;
                    Int32 oriExtended = button.Keyscan.Extended;
                    Int32 mapScancode = 0;
                    Int32 mapExtended = 0;

                    if (mappings != null)
                    {
                        Int32 position = mappings.IndexOf(oriScancode, oriExtended, mapScancode, mapExtended);

                        if (position != -1)
                        {
                            if (mappings.GetAt(position, out oriScancode, out oriExtended, out mapScancode, out mapExtended))
                            {
                                button.Keyscan.RemapMapping(mapScancode, mapExtended);
                            }
                        }
                        else
                        {
                            button.Keyscan.ResetMapping();
                        }
                    }
                    else
                    {
                        button.Keyscan.ResetMapping();
                    }
                    button.AdaptLayout();
                    button.PrepareTooltip();
                }
            }
        }

        public void CollectMappings(ScancodeMap mappings)
        {
            if (mappings == null) { return; }

            for (Int32 index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton button)
                {
                    if (!button.Keyscan.HasMapping) { continue; } // Go on with next button if this button has no mapping.

                    Int32 oriScancode = button.Keyscan.Scancode;
                    Int32 oriExtended = button.Keyscan.Extended;
                    Int32 mapScancode = button.Keyscan.MappedScancode;
                    Int32 mapExtended = button.Keyscan.MappedExtended;

                    Int32 position = mappings.IndexOf(oriScancode, oriExtended, 0, 0);

                    if (position == -1)
                    {
                        mappings.Append(oriScancode, oriExtended, mapScancode, mapExtended);
                    }
                    else
                    {
                        mappings.SetAt(position, oriScancode, oriExtended, mapScancode, mapExtended);
                    }
                }
            }
        }

        public KeyButton FindButtonByScancode(Int32 scancode, Int32 extended)
        {
            if (this.parent != null)
            {
                return this.parent.FindButtonByScancode(scancode, extended);
            }
            else
            {
                return null;
            }
        }

        protected KeyButton FindButtonUseVK(Int32 vkeycode)
        {
            for (Int32 index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton button)
                {
                    // Check curennt button's virtual key code.
                    if (button.Keyscan.VKeyCode == vkeycode)
                    {
                        return button;
                    }
                }
            }

            throw new ArgumentOutOfRangeException("Button for virtual key " + vkeycode + " not found");
        }

        protected KeyButton FindButtonUseSC(Int32 scancode)
        {
            for (Int32 index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton button)
                {
                    // Check curennt button's scan code.
                    if (button.Keyscan.Scancode == scancode)
                    {
                        return button;
                    }
                }
            }

            throw new ArgumentOutOfRangeException("Button with scan code " + scancode + " not found");
        }
    }
}
