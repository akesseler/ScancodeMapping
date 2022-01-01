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
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace ScancodeMapping
{
    public partial class AdvancedKeyMapping : Form, IKeyScanSink
    {
        private const string ACTION_DISABLE = "Disable";
        private const string ACTION_RESTORE = "Restore";

        private int virtualKey1 = 0;
        private int scancodeKey1 = 0;
        private int extendedKey1 = 0;
        private int virtualKey2 = 0;
        private int scancodeKey2 = 0;
        private int extendedKey2 = 0;

        private ArrayList pendingList = null;

        public AdvancedKeyMapping(ArrayList pendingList)
        {
            InitializeComponent();

            this.pendingList = pendingList;
        }

        public void HandleKeyScan(int vkeycode, int scancode, int flags, int time)
        {
            // REMARK: Read comments in function IsSpecialLControl()!
            if (Keyboard.IsSpecialLControl(vkeycode, scancode))
            {
                return;
            }

            // Do nothing on key release.
            if (Keyboard.IsKeyUp(flags))
            {
                return;
            }

            // Only extended keys using additional information!
            int extended = (Keyboard.IsExtended(flags) ? Keyboard.EXTENDED : Keyboard.STANDARD);

            if (!this.secondKeyGroup.Enabled)
            {
                FirstGroupKeyMapping(vkeycode, scancode, extended);
            }
            else
            {
                SecondGroupKeyMapping(vkeycode, scancode, extended);
            }
        }

        private void FirstGroupKeyMapping(int vkeycode, int scancode, int extended)
        {
            this.keyNameText1.Text = VirtualKeys.Name(vkeycode);
            this.virtualKeyText1.Text = "0x" + vkeycode.ToString("X4");
            this.scancodeText1.Text = "0x" + extended.ToString("X2") + scancode.ToString("X2");

            // REMARK:  This virtual key code arrives when an already disabled 
            //          (put to the registry and rebooted afterwards) key on the 
            //          keyboard was pressed. In that case it is impossible to 
            //          find out the original key only from invomming scancode. 
            //          Such a key cannot be disabled, remapped or what ever!
            if (vkeycode == VirtualKeys.VK_RESERVED_0xFF && scancode == 0)
            {
                this.ResetLayout(); //Hook must be disabled beforehand!

                MessageBox.Show(
                    this,
                    "Sorry, but it is impossible to find out an appropriated scancode\n" +
                    "for keys which are already disabled through a registry mapping!",
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );

                return; // Give up.
            }

            // Save values.
            this.virtualKey1 = vkeycode;
            this.scancodeKey1 = scancode;
            this.extendedKey1 = extended;

            // Enable "action" button.
            this.actionButton.Enabled = true;

            // Prepare "action" button.
            KeyButton button = App.GetMainForm().FindButtonByScancode(this.scancodeKey1, this.extendedKey1);

            if (button != null) // If this button is zero it will be added to the pending list.
            {
                if (button.Keyscan.HasMapping)
                {
                    this.actionButton.Text = ACTION_RESTORE;
                    this.actionButton.Tag = KeyButton.RestoreButton;
                }
                else
                {
                    this.actionButton.Text = ACTION_DISABLE;
                    this.actionButton.Tag = KeyButton.DisabledButton;
                }
            }
            else
            {
                if (-1 != pendingList.IndexOf(new AdvancedMapping(this.virtualKey1, this.scancodeKey1, this.extendedKey1)))
                {
                    this.actionButton.Text = ACTION_RESTORE;
                    this.actionButton.Tag = KeyButton.RestoreButton;
                }
                else
                {
                    this.actionButton.Text = ACTION_DISABLE;
                    this.actionButton.Tag = KeyButton.DisabledButton;
                }
            }

            // Disable first group after catching first key.
            this.firstKeyGroup.Enabled = false;

            // Enable second group to catch second key scan.
            this.secondKeyGroup.Enabled = true;
        }

        private void SecondGroupKeyMapping(int vkeycode, int scancode, int extended)
        {
            string keyName = VirtualKeys.Name(vkeycode);
            string vkCode = "0x" + vkeycode.ToString("X4");
            string scanCode = "0x" + extended.ToString("X2") + scancode.ToString("X2");

            // Avoid mapping to same key.
            if (scanCode == this.scancodeText1.Text)
            {
                return;
            }

            this.keyNameText2.Text = keyName;
            this.virtualKeyText2.Text = vkCode;
            this.scancodeText2.Text = scanCode;

            this.ResetLayout(); //Hook must be disabled beforehand!

            // REMARK:  This virtual key code arrives when an already disabled 
            //          (put to the registry and rebooted afterwards) key on the 
            //          keyboard was pressed. In that case it is impossible to 
            //          find out the original key only from invomming scancode. 
            //          Such a key cannot be disabled, remapped or what ever!
            if (vkeycode == VirtualKeys.VK_RESERVED_0xFF && scancode == 0)
            {
                MessageBox.Show(
                    this,
                    "Sorry, but it is impossible to map a keyboard key onto keys\n" +
                    "which are already disabled through a registry mapping!",
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );

                return; // Give up.
            }

            // Enable "apply" button.
            this.applyButton.Enabled = true;

            // Save values.
            this.virtualKey2 = vkeycode;
            this.scancodeKey2 = scancode;
            this.extendedKey2 = extended;
        }

        private void UpdatePendingList(int virtualKey1, int scancodeKey1, int extendedKey1, int virtualKey2, int scancodeKey2, int extendedKey2)
        {
            AdvancedMapping entry = new AdvancedMapping(virtualKey1, scancodeKey1, extendedKey1);

            int index = pendingList.IndexOf(entry);

            if (index == -1) // Add entry to list.
            {
                entry.VirtualKey1 = virtualKey1;
                entry.VirtualKey2 = virtualKey2;
                entry.ScancodeKey2 = scancodeKey2;
                entry.ExtendedKey2 = extendedKey2;

                pendingList.Add(entry);
            }
            else // Update found entry.
            {
                ((AdvancedMapping)pendingList[index]).VirtualKey1 = virtualKey1;
                ((AdvancedMapping)pendingList[index]).VirtualKey2 = virtualKey2;
                ((AdvancedMapping)pendingList[index]).ScancodeKey2 = scancodeKey2;
                ((AdvancedMapping)pendingList[index]).ExtendedKey2 = extendedKey2;
            }

            // Set application dirty because of changes on keyboard key mapping.
            App.GetMainForm().Dirty = true;
        }

        private void UpdatePendingList(int virtualKey1, int scancodeKey1, int extendedKey1, KeyButton button)
        {
            if (button != null)
            {
                if (button.Equals(KeyButton.DisabledButton))
                {
                    pendingList.Add(new AdvancedMapping(virtualKey1, scancodeKey1, extendedKey1));
                }
                else if (button.Equals(KeyButton.RestoreButton))
                {
                    int index = pendingList.IndexOf(new AdvancedMapping(virtualKey1, scancodeKey1, extendedKey1));

                    if (index != -1)
                    {
                        pendingList.RemoveAt(index);
                    }
                }

                // Set application dirty because of changes on keyboard key mapping.
                App.GetMainForm().Dirty = true;
            }
        }

        private void ResetLayout()
        {
            // Disable hook.
            Keyboard.Instance.Unhook();

            // Disable first group.
            this.firstKeyGroup.Enabled = false;

            // Disable second group.
            this.secondKeyGroup.Enabled = false;

            // Re-enable start button.
            this.startButton.Enabled = true;

            // Disable "action" button.
            this.actionButton.Enabled = false;
            this.actionButton.Tag = null;

            // Disable "apply" button.
            this.applyButton.Enabled = false;
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            this.scancodeKey1 = 0;
            this.extendedKey1 = 0;

            this.keyNameText1.Text = "";
            this.virtualKeyText1.Text = "";
            this.scancodeText1.Text = "";

            this.scancodeKey2 = 0;
            this.extendedKey2 = 0;

            this.keyNameText2.Text = "";
            this.virtualKeyText2.Text = "";
            this.scancodeText2.Text = "";

            this.firstKeyGroup.Enabled = true;
            this.startButton.Enabled = false;
            this.applyButton.Enabled = false;
            this.actionButton.Enabled = false;
            this.actionButton.Tag = null;

            Keyboard.Instance.Hook(this, true);
        }

        private void actionButton_Click(object sender, EventArgs e)
        {
            KeyButton button = App.GetMainForm().FindButtonByScancode(this.scancodeKey1, this.extendedKey1);

            if (button != null)
            {
                // Tag content is set either to DISABLE or to RESTORE button.
                button.RemapButton((KeyButton)this.actionButton.Tag);
            }
            else
            {
                // Tag content is set either to DISABLE or to RESTORE button.
                this.UpdatePendingList(
                    this.virtualKey1,
                    this.scancodeKey1,
                    this.extendedKey1,
                    (KeyButton)this.actionButton.Tag
                );
            }

            this.ResetLayout();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            KeyButton button1 = App.GetMainForm().FindButtonByScancode(scancodeKey1, extendedKey1);
            KeyButton button2 = App.GetMainForm().FindButtonByScancode(scancodeKey2, extendedKey2);

            if (button1 != null && button2 != null)
            {
                button1.RemapButton(button2);
            }
            else if (button1 != null && button2 == null)
            {
                button1.RemapButton(
                    this.scancodeKey2,
                    this.extendedKey2
                );

            }
            else if (button1 == null && button2 != null)
            {
                this.UpdatePendingList(
                    this.virtualKey1,
                    this.scancodeKey1,
                    this.extendedKey1,
                    button2.Keyscan.VKeyCode,
                    button2.Keyscan.Scancode,
                    button2.Keyscan.Extended
                );
            }
            else
            {
                this.UpdatePendingList(
                    this.virtualKey1,
                    this.scancodeKey1,
                    this.extendedKey1,
                    this.virtualKey2,
                    this.scancodeKey2,
                    this.extendedKey2
                );
            }

            this.applyButton.Enabled = false;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void firstKeyGroup_EnabledChanged(object sender, EventArgs e)
        {
            if (this.firstKeyGroup.Enabled)
            {
                this.keyNameText1.BackColor = SystemColors.Window;
                this.scancodeText1.BackColor = SystemColors.Window;
                this.virtualKeyText1.BackColor = SystemColors.Window;
            }
            else
            {
                this.keyNameText1.BackColor = SystemColors.ControlLight;
                this.scancodeText1.BackColor = SystemColors.ControlLight;
                this.virtualKeyText1.BackColor = SystemColors.ControlLight;
            }
        }

        private void secondKeyGroup_EnabledChanged(object sender, EventArgs e)
        {
            if (this.secondKeyGroup.Enabled)
            {
                this.keyNameText2.BackColor = SystemColors.Window;
                this.scancodeText2.BackColor = SystemColors.Window;
                this.virtualKeyText2.BackColor = SystemColors.Window;
            }
            else
            {
                this.keyNameText2.BackColor = SystemColors.ControlLight;
                this.scancodeText2.BackColor = SystemColors.ControlLight;
                this.virtualKeyText2.BackColor = SystemColors.ControlLight;
            }
        }

        private void AdvancedKeyMapping_Load(object sender, EventArgs e)
        {
            this.firstKeyGroup.Enabled = false;
            this.secondKeyGroup.Enabled = false;
            this.applyButton.Enabled = false;
            this.actionButton.Text = ACTION_DISABLE;
            this.actionButton.Enabled = false;
            this.actionButton.Tag = null;
        }

        private void AdvancedKeyMapping_FormClosing(object sender, FormClosingEventArgs e)
        {
            Keyboard.Instance.Unhook(); // Ensure unhooking!
        }
    }

    public class AdvancedMapping
    {
        private int virtualKey1 = 0;
        private int scancodeKey1 = 0;
        private int extendedKey1 = 0;
        private int virtualKey2 = 0;
        private int scancodeKey2 = 0;
        private int extendedKey2 = 0;

        public AdvancedMapping()
        {
            this.virtualKey1 = 0;
            this.scancodeKey1 = 0;
            this.extendedKey1 = 0;
            this.virtualKey2 = 0;
            this.scancodeKey2 = 0;
            this.extendedKey2 = 0;
        }

        public AdvancedMapping(int virtualKey1, int scancodeKey1, int extendedKey1)
            : this()
        {
            this.virtualKey1 = virtualKey1;
            this.scancodeKey1 = scancodeKey1;
            this.extendedKey1 = extendedKey1;
        }

        public AdvancedMapping(int virtualKey1, int scancodeKey1, int extendedKey1, int virtualKey2, int scancodeKey2, int extendedKey2)
            : this(virtualKey1, scancodeKey1, extendedKey1)
        {
            this.virtualKey2 = virtualKey2;
            this.scancodeKey2 = scancodeKey2;
            this.extendedKey2 = extendedKey2;
        }

        public int VirtualKey1
        {
            get { return this.virtualKey1; }
            set { this.virtualKey1 = value; }
        }

        public int ScancodeKey1
        {
            get { return this.scancodeKey1; }
            set { this.scancodeKey1 = value; }
        }

        public int ExtendedKey1
        {
            get { return this.extendedKey1; }
            set { this.extendedKey1 = value; }
        }

        public int VirtualKey2
        {
            get { return this.virtualKey2; }
            set { this.virtualKey2 = value; }
        }

        public int ScancodeKey2
        {
            get { return this.scancodeKey2; }
            set { this.scancodeKey2 = value; }
        }

        public int ExtendedKey2
        {
            get { return this.extendedKey2; }
            set { this.extendedKey2 = value; }
        }

        public static bool operator ==(AdvancedMapping entry1, AdvancedMapping entry2)
        {
            if (((object)entry1) != null && ((object)entry2) != null)
            {
                return ((entry1.ScancodeKey1 == entry2.ScancodeKey2) && (entry1.ExtendedKey1 == entry2.ExtendedKey2));
            }
            else
            {
                if (((object)entry1) == null && ((object)entry2) == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool operator !=(AdvancedMapping entry1, AdvancedMapping entry2)
        {
            return !(entry1 == entry2);
        }

        public override int GetHashCode()
        {
            return ((this.ScancodeKey1 & 0xFFFF) << 16) | (this.ExtendedKey1 & 0xFFFF);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is AdvancedMapping)
            {
                return ((this.ScancodeKey1 == ((AdvancedMapping)obj).ScancodeKey1) && (this.ExtendedKey1 == ((AdvancedMapping)obj).ExtendedKey1));
            }
            else
            {
                return false;
            }
        }
    }
}
