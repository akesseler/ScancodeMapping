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
        private ToolTip buttonToolTip;
        protected KeyboardPanel parent = null;

        //
        // Summary:
        //      Standard constructor.
        //
        public KeyPanel(KeyboardPanel parent)
            : base()
        {
            buttonToolTip = new ToolTip();
            this.parent = parent;
        }

        //
        // Properties
        //

        //
        // Summary:
        //      The panel tooltip used by panel's buttons.
        //
        public ToolTip Tooltip
        {
            get { return this.buttonToolTip; }
        }

        //
        // Event handlers
        //

        //
        // Summary:
        //      General KeyUp event handler.
        //
        public void HandleKeyUpEvent(KeyEventArgs evt)
        {
            if (this.Parent is KeyboardPanel)
            {
                switch (evt.KeyValue)
                {
                    case VirtualKeys.VK_NUMLOCK:
                        ((KeyboardPanel)this.Parent).NumLock(KeyStates.NumLock);
                        break;
                    case VirtualKeys.VK_CAPITAL:
                        ((KeyboardPanel)this.Parent).CapsLock(KeyStates.CapsLock);
                        break;
                    case VirtualKeys.VK_SCROLL:
                        ((KeyboardPanel)this.Parent).ScrollLock(KeyStates.ScrollLock);
                        break;
                }
            }
        }

        //
        // Member functions
        //

        //
        // Summary:
        //     Handles incoming virtual key, its scancode and its extended code.
        //
        // Returns:
        //     True if value combination was handled and false if not.
        //
        public bool UpdateScancode(int vkeycode, int scancode, int extended)
        {
            try
            {
                KeyButton button = FindButtonUseVK(vkeycode);

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

        //
        // Summary:
        //
        public void UpdateMappings(ScancodeMap mappings)
        {
            for (int index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton)
                {
                    KeyButton button = (KeyButton)this.Controls[index];

                    int oriScancode = button.Keyscan.Scancode;
                    int oriExtended = button.Keyscan.Extended;
                    int mapScancode = 0;
                    int mapExtended = 0;

                    if (mappings != null)
                    {
                        int position = mappings.IndexOf(oriScancode, oriExtended, mapScancode, mapExtended);

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

        // Summary:
        //
        public void CollectMappings(ScancodeMap mappings)
        {
            if (mappings == null) { return; }

            for (int index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton)
                {
                    KeyButton button = (KeyButton)this.Controls[index];

                    if (!button.Keyscan.HasMapping) { continue; } // Go on with next button if this button has no mapping.

                    int oriScancode = button.Keyscan.Scancode;
                    int oriExtended = button.Keyscan.Extended;
                    int mapScancode = button.Keyscan.MappedScancode;
                    int mapExtended = button.Keyscan.MappedExtended;

                    int position = mappings.IndexOf(oriScancode, oriExtended, 0, 0);

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

        //
        // Summary:
        //
        public KeyButton FindButtonByScancode(int scancode, int extended)
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

        //
        // Summary:
        //
        protected KeyButton FindButtonUseVK(int vkeycode)
        {
            for (int index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton)
                {
                    // Check curennt button's virtual key code.
                    if (((KeyButton)this.Controls[index]).Keyscan.VKeyCode == vkeycode)
                    {
                        return (KeyButton)this.Controls[index];
                    }
                }
            }

            throw new ArgumentOutOfRangeException("Button for virtual key " + vkeycode + " not found");
        }

        //
        // Summary:
        //
        protected KeyButton FindButtonUseSC(int scancode)
        {
            for (int index = 0; index < this.Controls.Count; index++)
            {
                // Ensure usage of keyboard buttons only.
                if (this.Controls[index] is KeyButton)
                {
                    // Check curennt button's scan code.
                    if (((KeyButton)this.Controls[index]).Keyscan.Scancode == scancode)
                    {
                        return (KeyButton)this.Controls[index];
                    }
                }
            }

            throw new ArgumentOutOfRangeException("Button with scan code " + scancode + " not found");
        }
    }
}
