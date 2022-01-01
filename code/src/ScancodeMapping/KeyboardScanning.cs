using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using ScancodeHook.LowLevel;

namespace ScancodeMapping
{
    public partial class KeyboardScanning : Form, IKeyScanSink
    {
        public KeyboardScanning()
        {
            InitializeComponent();
        }

        public bool Filtered
        {
            get { return this.cbFiltered.Checked; }
            set { this.cbFiltered.Checked = value; }
        }

        public bool Unique
        {
            get { return this.cbUnique.Checked; }
            set { this.cbUnique.Checked = value; }
        }

        public bool Pressed
        {
            get { return this.cbKeyPress.Checked; }
            set { this.cbKeyPress.Checked = value; }
        }

        public bool Released
        {
            get { return this.cbKeyRelease.Checked; }
            set { this.cbKeyRelease.Checked = value; }
        }

        public void HandleKeyScan(int vkeycode, int scancode, int flags, int time)
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
            string keyStoke = (Keyboard.IsKeyUp(flags) ? "released" : "pressed");

            // Only extended keys using additional information!
            int extended = (Keyboard.IsExtended(flags) ? Keyboard.EXTENDED : Keyboard.STANDARD);

            // Prepare list item tag handle
            string tagEntry = vkeycode.ToString("X4") + scancode.ToString("X4") + extended.ToString("X4") + flags.ToString("X8");

            // Don't search the list if "unique" has to be recorded!
            ListViewItem item = null;
            if (this.Unique)
            {
                for (int index = 0; index < this.lvScanData.Items.Count; index++)
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
                this.lvScanData.Items.Add(new ListViewItem(new string[] {
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

        private void KeyboardScanning_FormClosing(object sender, FormClosingEventArgs e)
        {
            Keyboard.Instance.Unhook();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            if (btnAction.Text == "Start")
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

        private void lvContextMenu_Opening(object sender, CancelEventArgs eventArgs)
        {
            eventArgs.Cancel = !(this.lvScanData.Items.Count > 0);

            this.emptyMenuItem.Enabled = (this.lvScanData.Items.Count > 0);
            this.removeMenuItem.Enabled = (this.lvScanData.SelectedItems.Count > 0);
            this.exportMenuItem.Enabled = (this.lvScanData.Items.Count > 0) && !Keyboard.Instance.Hooked;
        }

        private void emptyMenuItem_Click(object sender, EventArgs e)
        {
            while (this.lvScanData.Items.Count > 0)
            {
                this.lvScanData.Items[0].Remove();
            }
            this.lbCountValue.Text = this.lvScanData.Items.Count.ToString();
        }

        private void removeMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ListView.SelectedListViewItemCollection selected = this.lvScanData.SelectedItems;

            for (int index = 0; index < selected.Count; index++)
            {
                selected[index].Remove();
            }
            this.lbCountValue.Text = this.lvScanData.Items.Count.ToString();
        }

        private void exportMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Excel-CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            save.AutoUpgradeEnabled = true;
            save.CheckFileExists = false;
            save.RestoreDirectory = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                Cursor oldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                TextWriter outfile = new StreamWriter(save.FileName, false, Encoding.Unicode);
                string outstring = "";

                // Output header information.
                for (int index = 0; index < this.lvScanData.Columns.Count; index++)
                {
                    outstring += this.lvScanData.Columns[index].Text;
                    if (index < this.lvScanData.Columns.Count - 1)
                    {
                        outstring += "\t";
                    }
                }
                outfile.WriteLine(outstring);
                outstring = "";

                // Output data rows.
                for (int outer = 0; outer < this.lvScanData.Items.Count; outer++)
                {
                    ListViewItem item = this.lvScanData.Items[outer];

                    for (int inner = 0; inner < item.SubItems.Count; inner++)
                    {
                        outstring += item.SubItems[inner].Text;
                        if (inner < item.SubItems.Count - 1)
                        {
                            outstring += "\t";
                        }
                    }

                    outfile.WriteLine(outstring);
                    outstring = "";
                }
                outfile.Close();
                this.Cursor = oldCursor;
            }
        }
    }
}
