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
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ScancodeMapping
{
    public partial class KeyboardScanning : Form, IKeyScanSink
    {
        public KeyboardScanning()
        {
            this.InitializeComponent();
        }

        public Boolean Filtered
        {
            get { return this.cbFiltered.Checked; }
            set { this.cbFiltered.Checked = value; }
        }

        public Boolean Unique
        {
            get { return this.cbUnique.Checked; }
            set { this.cbUnique.Checked = value; }
        }

        public Boolean Pressed
        {
            get { return this.cbKeyPress.Checked; }
            set { this.cbKeyPress.Checked = value; }
        }

        public Boolean Released
        {
            get { return this.cbKeyRelease.Checked; }
            set { this.cbKeyRelease.Checked = value; }
        }

        public void HandleKeyScan(Int32 vkeycode, Int32 scancode, Int32 flags, Int32 time)
        {
            // Filter messages only if requested.
            if (this.cbFiltered.Checked)
            {
                // REMARK: Read comments in function IsSpecialLControl()!
                if (Keyboard.IsSpecialLControl(vkeycode, scancode))
                {
                    return;
                }
            }

            // No check box is selected!
            if (!this.cbKeyPress.Checked && !this.cbKeyRelease.Checked)
            {
                return;
            }

            // Ignore key release if key press is the only requested one.
            if (this.cbKeyPress.Checked && !this.cbKeyRelease.Checked && Keyboard.IsKeyUp(flags))
            {
                return;
            }

            // Ignore key press if key release is the only requested one.
            if (!this.cbKeyPress.Checked && this.cbKeyRelease.Checked && !Keyboard.IsKeyUp(flags))
            {
                return;
            }

            // Prepare key stoke name (keys can only be pressed or released)
            String keyStoke = Keyboard.IsKeyUp(flags) ? "released" : "pressed";

            // Only extended keys using additional information!
            Int32 extended = Keyboard.IsExtended(flags) ? Keyboard.EXTENDED : Keyboard.STANDARD;

            // Prepare list item tag handle
            String tagEntry = vkeycode.ToString("X4") + scancode.ToString("X4") + extended.ToString("X4") + flags.ToString("X8");

            // Don't search the list if "unique" has to be recorded!
            ListViewItem item = null;
            if (this.Unique)
            {
                for (Int32 index = 0; index < this.lvScanData.Items.Count; index++)
                {
                    if (tagEntry.ToString() == this.lvScanData.Items[index].Tag.ToString())
                    {
                        item = this.lvScanData.Items[index];
                        break;
                    }
                }
            }

            if (item == null) // This happens either if an entry doesn't yet exist or in "everything" mode!
            {
                this.lvScanData.Items.Add(new ListViewItem(new String[] {
                    VirtualKeys.Name(vkeycode),
                    Win32Wrapper.GetKeyNameText(new KeyScan(vkeycode, scancode, extended)),
                    "0x" + vkeycode.ToString("X4"),
                    "0x" + scancode.ToString("X4"),
                    "0x" + extended.ToString("X4"),
                    "0x" + flags.ToString("X8"),
                    keyStoke}, -1));

                this.lvScanData.Items[this.lvScanData.Items.Count - 1].Tag = tagEntry.ToString();
                this.lvScanData.Items[this.lvScanData.Items.Count - 1].EnsureVisible();
                this.lvScanData.Items[this.lvScanData.Items.Count - 1].Selected = true;
            }
            else
            {
                item.SubItems[0].Text = VirtualKeys.Name(vkeycode);
                item.SubItems[1].Text = Win32Wrapper.GetKeyNameText(new KeyScan(vkeycode, scancode, extended));
                item.SubItems[2].Text = "0x" + vkeycode.ToString("X4");
                item.SubItems[3].Text = "0x" + scancode.ToString("X4");
                item.SubItems[4].Text = "0x" + extended.ToString("X4");
                item.SubItems[5].Text = "0x" + flags.ToString("X8");

                item.EnsureVisible();
                item.Selected = true;
            }
            this.lbCountValue.Text = this.lvScanData.Items.Count.ToString();
        }

        private void OnKeyboardScanningFormClosing(Object sender, FormClosingEventArgs args)
        {
            Keyboard.Instance.Unhook();
        }

        private void OnButtonCloseClick(Object sender, EventArgs args)
        {
            this.Close();
        }

        private void OnButtonActionClick(Object sender, EventArgs args)
        {
            if (this.btnAction.Text == "Start")
            {
                Keyboard.Instance.Hook(this, true);
                this.btnAction.Text = "Stop";
            }
            else
            {
                this.btnAction.Text = "Start";
                Keyboard.Instance.Unhook();
            }
        }

        private void OnListViewContextMenuOpening(Object sender, CancelEventArgs args)
        {
            args.Cancel = !(this.lvScanData.Items.Count > 0);

            this.emptyMenuItem.Enabled = (this.lvScanData.Items.Count > 0);
            this.removeMenuItem.Enabled = (this.lvScanData.SelectedItems.Count > 0);
            this.exportMenuItem.Enabled = (this.lvScanData.Items.Count > 0) && !Keyboard.Instance.Hooked;
        }

        private void OnEmptyMenuItemClick(Object sender, EventArgs args)
        {
            while (this.lvScanData.Items.Count > 0)
            {
                this.lvScanData.Items[0].Remove();
            }
            this.lbCountValue.Text = this.lvScanData.Items.Count.ToString();
        }

        private void OnRemoveMenuItemClick(Object sender, EventArgs args)
        {
            ListView.SelectedListViewItemCollection selected = this.lvScanData.SelectedItems;

            for (Int32 index = 0; index < selected.Count; index++)
            {
                selected[index].Remove();
            }
            this.lbCountValue.Text = this.lvScanData.Items.Count.ToString();
        }

        private void OnExportMenuItemClick(Object sender, EventArgs args)
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                Filter = "Excel-CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                AutoUpgradeEnabled = true,
                CheckFileExists = false,
                RestoreDirectory = true
            };

            if (save.ShowDialog() == DialogResult.OK)
            {
                Cursor oldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                TextWriter outfile = new StreamWriter(save.FileName, false, Encoding.Unicode);
                String outstring = String.Empty;

                // Output header information.
                for (Int32 index = 0; index < this.lvScanData.Columns.Count; index++)
                {
                    outstring += this.lvScanData.Columns[index].Text;
                    if (index < this.lvScanData.Columns.Count - 1)
                    {
                        outstring += "\t";
                    }
                }
                outfile.WriteLine(outstring);
                outstring = String.Empty;

                // Output data rows.
                for (Int32 outer = 0; outer < this.lvScanData.Items.Count; outer++)
                {
                    ListViewItem item = this.lvScanData.Items[outer];

                    for (Int32 inner = 0; inner < item.SubItems.Count; inner++)
                    {
                        outstring += item.SubItems[inner].Text;
                        if (inner < item.SubItems.Count - 1)
                        {
                            outstring += "\t";
                        }
                    }

                    outfile.WriteLine(outstring);
                    outstring = String.Empty;
                }
                outfile.Close();
                this.Cursor = oldCursor;
            }
        }
    }
}
