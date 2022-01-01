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
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ScancodeMapping
{
    public partial class MainForm : Form
    {
        // Main form settings
        private TKeyboardLayout currentLayout = TKeyboardLayout.Default;
        private TKeyboardKeys currentKeys = TKeyboardKeys.Default;
        private TKeyboardAlignment currentAlignment = TKeyboardAlignment.Default;

        // Scanning form settings
        private bool scanningFiltered = true;
        private bool scanningUnique = true;
        private bool scanningKeyPress = true;
        private bool scanningKeyRelease = false;

        // XML file node and attribute names
        private const string xmlSettings = "settings";
        private const string xmlKeyboard = "keyboard";
        private const string xmlScanning = "scanning";
        private const string xmlAlignment = "alignment";
        private const string xmlLayout = "layout";
        private const string xmlKeys = "keys";
        private const string xmlFiltered = "filtered";
        private const string xmlUnique = "unique";
        private const string xmlKeyPress = "keypress";
        private const string xmlKeyRelease = "keyrelease";

        private const int aboutCommandID = 0x100;

        private bool dirty = false;
        private bool reboot = false;

        private KeyboardPanel keyboardPanel;
        private KeyButton selectedButton = null;

        private ArrayList advancedMappings = new ArrayList();

        public MainForm()
        {
            InitializeComponent();

            this.SuspendLayout();

            this.keyboardPanel = new KeyboardPanel(TKeyboardAlignment.Default);

            this.Controls.Add(this.keyboardPanel);

            this.keyboardPanel.Location = new Point(0, mainToolBar.Size.Height);

            FitWindowLayout();

            this.ResumeLayout(false);

            // Don't do to much right here because the main form is not yet instantiated!
        }

        //
        // Propertires
        //
        public TKeyboardLayout KeyboardLayout
        {
            get { return this.currentLayout; }
            set
            {
                if (this.currentLayout != value)
                {
                    this.currentLayout = value;

                    // Keyboard layout update.
                    this.keyboardPanel.KeyboardLayoutChanged(this.currentLayout);

                    // Status bar value update.
                    this.StatusbarChanged(this.currentLayout);
                }
            }
        }

        public TKeyboardKeys KeyboardKeys
        {
            get { return this.currentKeys; }
            set
            {
                if (this.currentKeys != value)
                {
                    this.currentKeys = value;

                    // Keyboard layout update.
                    this.keyboardPanel.KeyboardLayoutChanged(this.currentKeys);

                    // Status bar value update.
                    this.StatusbarChanged(this.currentKeys);
                }
            }
        }

        public TKeyboardAlignment KeyboardAlignment
        {
            get { return this.currentAlignment; }
            set
            {
                if (this.currentAlignment != value)
                {
                    this.currentAlignment = value;

                    // Keyboard layout update.
                    this.keyboardPanel.KeyboardLayoutChanged(this.currentAlignment);

                    // Status bar value update.
                    this.StatusbarChanged(this.currentAlignment);
                }
            }
        }

        public bool Dirty
        {
            get { return this.dirty; }
            set { this.dirty = value; }
        }

        public bool Reboot
        {
            get { return this.reboot; }
            set { this.reboot = value; }
        }

        //
        // Member functions
        //

        public void StatusbarChanged(TKeyboardLayout layout)
        {
            // Uncheck all items
            this.enhancedMenuItem.Checked = false;
            this.standardMenuItem.Checked = false;

            switch (layout)
            {
                case TKeyboardLayout.Enhanced:
                    this.layoutDropDown.Text = this.enhancedMenuItem.Text;
                    this.enhancedMenuItem.Checked = true;
                    break;
                case TKeyboardLayout.Standard:
                    this.layoutDropDown.Text = this.standardMenuItem.Text;
                    this.standardMenuItem.Checked = true;
                    break;
            }
            this.SelectedButtonChanged(this.keyboardPanel.GetSelectedButton());
        }

        private void StatusbarChanged(TKeyboardKeys keys)
        {
            // Uncheck all items
            this.standard101MenuItem.Checked = false;
            this.standard102MenuItem.Checked = false;
            this.winkey101MenuItem.Checked = false;
            this.winkey102MenuItem.Checked = false;

            switch (keys)
            {
                case TKeyboardKeys.Keys101:
                    this.keysDropDown.Text = this.standard101MenuItem.Text;
                    this.standard101MenuItem.Checked = true;
                    break;
                case TKeyboardKeys.Keys102:
                    this.keysDropDown.Text = this.standard102MenuItem.Text;
                    this.standard102MenuItem.Checked = true;
                    break;
                case TKeyboardKeys.Keys105:
                    this.keysDropDown.Text = this.winkey101MenuItem.Text;
                    this.winkey101MenuItem.Checked = true;
                    break;
                case TKeyboardKeys.Keys106:
                    this.keysDropDown.Text = this.winkey102MenuItem.Text;
                    this.winkey102MenuItem.Checked = true;
                    break;
            }
            this.SelectedButtonChanged(this.keyboardPanel.GetSelectedButton());
        }

        private void StatusbarChanged(TKeyboardAlignment alignment)
        {
            // Uncheck all items
            this.germanyMenuItem.Checked = false;
            this.usstandardMenuItem.Checked = false;

            switch (alignment)
            {
                case TKeyboardAlignment.Germany:
                    this.alignmentDropDown.Text = this.germanyMenuItem.Text;
                    this.germanyMenuItem.Checked = true;
                    break;
                case TKeyboardAlignment.USStandard:
                    this.alignmentDropDown.Text = this.usstandardMenuItem.Text;
                    this.usstandardMenuItem.Checked = true;
                    break;
            }
            this.SelectedButtonChanged(this.keyboardPanel.GetSelectedButton());
        }

        public void StatusbarChanged(string text)
        {
            this.generalStatusLabel.Text = text;
        }

        public void SelectedButtonChanged(KeyButton button)
        {
            this.selectedButton = button;

            if (this.selectedButton == null)
            {
                this.mainKeyMapping.Enabled = false;
            }
            else
            {
                this.mainKeyMapping.Enabled = true;
            }
        }

        public void FitWindowLayout()
        {
            this.ClientSize = new Size(
                this.keyboardPanel.Size.Width,
                this.keyboardPanel.Size.Height + this.mainToolBar.Size.Height + this.mainStatusBar.Size.Height);

            int width = this.ClientSize.Width
                        - this.layoutDropDown.Width
                        - this.keysDropDown.Width
                        - this.alignmentDropDown.Width - 1;

            this.generalStatusLabel.Size = new Size(width, this.generalStatusLabel.Size.Height);
        }

        public KeyButton FindButtonByScancode(int scnacode, int extended)
        {
            return this.keyboardPanel.FindButtonByScancode(scnacode, extended);
        }

        public ArrayList CollectButtons()
        {
            ArrayList result = new ArrayList();

            for (int panels = 0; panels < this.keyboardPanel.Controls.Count; panels++)
            {
                if (this.keyboardPanel.Controls[panels] is KeyPanel)
                {
                    KeyPanel panel = (KeyPanel)this.keyboardPanel.Controls[panels];

                    for (int buttons = 0; buttons < panel.Controls.Count; buttons++)
                    {
                        if (panel.Controls[buttons] is KeyButton)
                        {
                            result.Add((KeyButton)panel.Controls[buttons]);
                        }
                    }
                }
            }
            return result;
        }

        private void LoadSettings(string file)
        {
            // Read XML file content.
            TextReader reader;
            try { reader = new StreamReader(file); }
            catch (Exception) { return; }
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            reader.Close();

            // Load 'settings' root tag and process contained entries.
            XmlNodeList settings = document.GetElementsByTagName(xmlSettings);

            for (int outer = 0; outer < settings.Count; outer++)
            {
                try
                {
                    XmlNodeList nodes = settings.Item(outer).ChildNodes;

                    // Deserialize data depending on table type.
                    for (int inner = 0; inner < nodes.Count; inner++)
                    {
                        XmlNode entry = nodes.Item(inner);
                        string value;

                        if (entry.Name.ToLower().Equals(xmlKeyboard) && entry.Attributes.Count > 0)
                        {
                            try
                            {
                                value = entry.Attributes.GetNamedItem(xmlAlignment).Value;

                                if (value.ToLower() == TKeyboardAlignment.Germany.ToString().ToLower())
                                {
                                    this.KeyboardAlignment = TKeyboardAlignment.Germany;
                                }
                                else if (value.ToLower() == TKeyboardAlignment.USStandard.ToString().ToLower())
                                {
                                    this.KeyboardAlignment = TKeyboardAlignment.USStandard;
                                }
                                else
                                {
                                    this.KeyboardAlignment = TKeyboardAlignment.Default;
                                }
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                value = entry.Attributes.GetNamedItem(xmlLayout).Value;

                                if (value.ToLower() == TKeyboardLayout.Enhanced.ToString().ToLower())
                                {
                                    this.KeyboardLayout = TKeyboardLayout.Enhanced;
                                }
                                else if (value.ToLower() == TKeyboardLayout.Standard.ToString().ToLower())
                                {
                                    this.KeyboardLayout = TKeyboardLayout.Standard;
                                }
                                else
                                {
                                    this.KeyboardLayout = TKeyboardLayout.Default;
                                }
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                value = entry.Attributes.GetNamedItem(xmlKeys).Value;

                                if (value.ToLower() == TKeyboardKeys.Keys101.ToString().ToLower())
                                {
                                    this.KeyboardKeys = TKeyboardKeys.Keys101;
                                }
                                else if (value.ToLower() == TKeyboardKeys.Keys102.ToString().ToLower())
                                {
                                    this.KeyboardKeys = TKeyboardKeys.Keys102;
                                }
                                else if (value.ToLower() == TKeyboardKeys.Keys105.ToString().ToLower())
                                {
                                    this.KeyboardKeys = TKeyboardKeys.Keys105;
                                }
                                else if (value.ToLower() == TKeyboardKeys.Keys106.ToString().ToLower())
                                {
                                    this.KeyboardKeys = TKeyboardKeys.Keys106;
                                }
                                else
                                {
                                    this.KeyboardKeys = TKeyboardKeys.Default;
                                }
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }
                        }
                        else if (entry.Name.ToLower().Equals(xmlScanning))
                        {
                            try
                            {
                                this.scanningFiltered = Convert.ToBoolean(
                                    entry.Attributes.GetNamedItem(xmlFiltered).Value
                                );
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                this.scanningUnique = Convert.ToBoolean(
                                    entry.Attributes.GetNamedItem(xmlUnique).Value
                                );
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                this.scanningKeyPress = Convert.ToBoolean(
                                    entry.Attributes.GetNamedItem(xmlKeyPress).Value
                                );
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                this.scanningKeyRelease = Convert.ToBoolean(
                                    entry.Attributes.GetNamedItem(xmlKeyRelease).Value
                                );
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, "Invalid input file!\n\n" + exception.Message, "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveSettings(string file)
        {
            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("ScancodeMapping");
            XmlElement settings = document.CreateElement(xmlSettings);
            XmlElement keyboard = document.CreateElement(xmlKeyboard);
            XmlElement scanning = document.CreateElement(xmlScanning);

            keyboard.SetAttribute(xmlAlignment, this.KeyboardAlignment.ToString().ToLower());
            keyboard.SetAttribute(xmlLayout, this.KeyboardLayout.ToString().ToLower());
            keyboard.SetAttribute(xmlKeys, this.KeyboardKeys.ToString().ToLower());
            settings.AppendChild(keyboard);

            scanning.SetAttribute(xmlFiltered, this.scanningFiltered.ToString().ToLower());
            scanning.SetAttribute(xmlUnique, this.scanningUnique.ToString().ToLower());
            scanning.SetAttribute(xmlKeyPress, this.scanningKeyPress.ToString().ToLower());
            scanning.SetAttribute(xmlKeyRelease, this.scanningKeyRelease.ToString().ToLower());
            settings.AppendChild(scanning);

            root.AppendChild(settings);
            document.AppendChild(root);

            // Write XML file content.
            XmlSerializer serializer = new XmlSerializer(typeof(XmlNode));
            TextWriter writer = new StreamWriter(file);
            serializer.Serialize(writer, document);
            writer.Close();
        }

        private void SaveMappings()
        {
            ScancodeMap mappings = new ScancodeMap();

            this.keyboardPanel.CollectMappings(mappings);

            // Add advanced mappings to raw data list.
            foreach (AdvancedMapping entry in this.advancedMappings)
            {
                mappings.Append(entry.ScancodeKey1, entry.ExtendedKey1, entry.ScancodeKey2, entry.ExtendedKey2);
            }

            if (mappings.HasEntries())
            {
                if (ScancodeMap.RegistrySave(mappings))
                {
                    this.Dirty = false;
                    this.Reboot = true;
                }
            }
        }

        private void RebootSystem()
        {
#if !DEBUG
            // Source of that code came from http://www.geekpedia.com/code36_Shut-down-system-using-Csharp.html
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();
            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams = mcWin32.GetMethodParameters("Win32Shutdown");
            // Flag 6 means we want to forced reboot of the system!
            mboShutdownParams["Flags"] = "6";
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
            }
#endif
        }

        private string MakeMappingInfo(AdvancedMapping entry)
        {
            string result = "";

            int helper = (entry.ExtendedKey1 << 8) | entry.ScancodeKey1;
            result += VirtualKeys.Name(entry.VirtualKey1) + " (0x" + helper.ToString("X4") + ")";

            if (entry.ScancodeKey2 == 0 && entry.ExtendedKey2 == 0)
            {
                result += " -> disabled";
            }
            else
            {
                helper = (entry.ExtendedKey2 << 8) | entry.ScancodeKey2;
                result += " -> " + VirtualKeys.Name(entry.VirtualKey2) + " (0x" + helper.ToString("X4") + ")";
            }

            return result;
        }

        //
        // Status bar event handlers.
        //
        private void standard101MenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys101;
        }

        private void standard102MenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys102;
        }

        private void winkey102MenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys106;
        }

        private void winkey101MenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys105;
        }

        private void enhancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardLayout = TKeyboardLayout.Enhanced;
        }

        private void standardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardLayout = TKeyboardLayout.Standard;
        }

        private void germanyMenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardAlignment = TKeyboardAlignment.Germany;
        }

        private void usstandardMenuItem_Click(object sender, EventArgs e)
        {
            this.KeyboardAlignment = TKeyboardAlignment.USStandard;
        }

        //
        // Main form depending event handlers.
        //
        private void MainForm_Load(object sender, EventArgs e)
        {
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            // Result this function is formatted as a hexadecimal string
            // with the parts of the used sub-language followed by the 
            // used primary language. Both parts are devided as shown 
            // as follows:
            // 
            // +-------------------------+-------------------------+
            // |     SubLanguage ID      |   Primary Language ID   |
            // +-------------------------+-------------------------+
            // 15                    10  9                         0 
            // 
            string layout = Win32Wrapper.GetKeyboardLayoutName();

            // With US keyboard layout function GetKeyboardLayoutName() 
            // returns the string "00000409". This means primary language 
            // is set to "English (United States)" and sub-language is 
            // not used. For german keyboard layouts the function returns 
            // "00000407" and this means primary language is set to "German 
            // (Germany)". The sub-language is also not used. Thus, US or 
            // international keyboards can be identified by the comparison 
            // of last four characters of the returned string. If the result 
            // string contains "0409" then the US keyboard layout should 
            // be usedand if not the international keyboard layout will be 
            // used instead!
            //
            // But keep in mind, following code should be executed AFTER 
            // the keyboard panel was created. Otherwise the triggered 
            // property KeyboardAlignment will not be able to force a 
            // change of current/default layout.
            //
            string primaryLanguage = layout.Substring(4);
            if (primaryLanguage == "0409")
            {
                this.KeyboardAlignment = TKeyboardAlignment.USStandard;
            }
            else
            {
                this.KeyboardAlignment = TKeyboardAlignment.Germany;
            }

            // Status bar value update.
            this.StatusbarChanged(this.currentLayout);
            this.StatusbarChanged(this.currentKeys);
            this.StatusbarChanged(this.currentAlignment); // Not really needed because it's already done by setting the property (see above)

            //
            // Load settings...
            //
            LoadSettings(Path.ChangeExtension(Application.ExecutablePath, ".xml"));

            try
            {
                // Put About Box to the system menu.
                SystemMenu systemMenu = SystemMenu.GetSystemMenu(this);
                int position = systemMenu.FindPositionByCommandID((int)SystemMenuCommand.SC_CLOSE);
                systemMenu.InsertSeparator(position);
                systemMenu.InsertMenu(position, aboutCommandID, "About ...");

                // Get current scancode map from Windows Registry
                ScancodeMap mappings = ScancodeMap.RegistryLoad();

                // Update mapping state on every key panel.
                this.keyboardPanel.UpdateMappings(mappings);

                int index = 0;
                int oriScancode = 0;
                int oriExtended = 0;
                int mapScancode = 0;
                int mapExtended = 0;

                // Add every unknown keyboard key mapping to the advanced mappings list.
                while (mappings.GetAt(index, out oriScancode, out oriExtended, out mapScancode, out mapExtended))
                {
                    // Avoid usage of already assigned keyboard keys!
                    if (null == this.FindButtonByScancode(oriScancode, oriExtended))
                    {
                        KeyButton mapButton = this.FindButtonByScancode(mapScancode, mapExtended);

                        if (mapButton == null)
                        {
                            this.advancedMappings.Add(
                                new AdvancedMapping(
                                    VirtualKeys.VK_UNKNOWN, oriScancode, oriExtended,
                                    VirtualKeys.VK_UNKNOWN, mapScancode, mapExtended
                                )
                            );
                        }
                        else
                        {
                            this.advancedMappings.Add(
                                new AdvancedMapping(
                                    VirtualKeys.VK_UNKNOWN, oriScancode, oriExtended,
                                    mapButton.Keyscan.VKeyCode, mapScancode, mapExtended
                                )
                            );
                        }
                    }

                    index++;
                }
            }
            catch (Exception)
            {
                // Do some error handling
            }

            this.Cursor = oldCursor;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            SaveSettings(Path.ChangeExtension(Application.ExecutablePath, ".xml"));

            if (this.Dirty)
            {
#if !DEBUG
                DialogResult result = MessageBox.Show(
                    this,
                    "Would you like to store your scancode mapping changes into the Windows Registry and reboot the system afterwards?",
                    "Save Changes",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    this.SaveMappings();
                    this.RebootSystem();
                }
#endif
            }
            else if (this.Reboot)
            {
#if !DEBUG
                DialogResult result = MessageBox.Show(
                    this,
                    "Would you like to reboot your system to get your scancode mapping changes working?",
                    "System Reboot",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    this.RebootSystem();
                }
#endif
            }
        }

        //
        // Menu bar event handlers.
        //

        private void mainExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mainSaveMapping_Click(object sender, EventArgs e)
        {
            this.SaveMappings();
        }

        private void mainRemoveMapping_Click(object sender, EventArgs e)
        {
            // Remove mappings from the registry.
            ScancodeMap.RegistryRemove();

            // Update mapping state on every key panel.
            this.keyboardPanel.UpdateMappings(null);

            // Also remove every thing from advanced maping list.
            this.advancedMappings.RemoveRange(0, this.advancedMappings.Count);

            // Need to reboot instead of dirty usage.
            this.Reboot = true;
        }

        private void mainShowMapping_Click(object sender, EventArgs e)
        {
            ScancodeMap mappings = new ScancodeMap();
            this.keyboardPanel.CollectMappings(mappings);

            // Add advanced mappings to raw data list.
            foreach (AdvancedMapping entry in this.advancedMappings)
            {
                mappings.Append(entry.ScancodeKey1, entry.ExtendedKey1, entry.ScancodeKey2, entry.ExtendedKey2);
            }

            string[] rawData = mappings.GetRawDataList();

            int index = 0;
            rawData[index++] += "\tVersion";
            rawData[index++] += "\tFlags";
            rawData[index++] += "\tElements";

            // Append assigned keys to the output.
            for (; index < rawData.Length - 1; index++)
            {
                int mapScancode = Convert.ToInt32(rawData[index].Substring(0, 2), 16);
                int mapExtended = Convert.ToInt32(rawData[index].Substring(3, 2), 16);
                int oriScancode = Convert.ToInt32(rawData[index].Substring(6, 2), 16);
                int oriExtended = Convert.ToInt32(rawData[index].Substring(9, 2), 16);

                string helper = KeyButton.MakeMappingInfo(
                    keyboardPanel.FindButtonByScancode(oriScancode, oriExtended),
                    keyboardPanel.FindButtonByScancode(mapScancode, mapExtended)
                );

                if (helper == "")
                {
                    // Try to append advanced mappings to the output.
                    foreach (AdvancedMapping entry in this.advancedMappings)
                    {
                        if (entry.ScancodeKey1 == oriScancode && entry.ExtendedKey1 == oriExtended)
                        {
                            helper = this.MakeMappingInfo(entry);
                            break;
                        }
                    }
                }

                rawData[index] += "\t" + helper;
            }

            rawData[index++] += "\tTermination";

            MappingRawData dlg = new MappingRawData(rawData);
            dlg.ShowDialog();
        }

        private void mainExportMapping_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Windows Registry files (*.reg)|*.reg|All files (*.*)|*.*";
            save.AutoUpgradeEnabled = true;
            save.CheckFileExists = false;
            save.RestoreDirectory = true;

            if (save.ShowDialog() == DialogResult.OK)
            {
                Cursor oldCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;

                // Update mapping state on every key panel.
                ScancodeMap mappings = new ScancodeMap();
                this.keyboardPanel.CollectMappings(mappings);

                // Add advanced mappings to raw data list.
                foreach (AdvancedMapping entry in this.advancedMappings)
                {
                    mappings.Append(entry.ScancodeKey1, entry.ExtendedKey1, entry.ScancodeKey2, entry.ExtendedKey2);
                }

                mappings.ExportAsFile(save.FileName);

                this.Cursor = oldCursor;
            }
        }

        private void mainScanKeyboard_Click(object sender, EventArgs e)
        {
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            KeyboardScanning dlg = new KeyboardScanning();

            dlg.Filtered = this.scanningFiltered;
            dlg.Unique = this.scanningUnique;
            dlg.Pressed = this.scanningKeyPress;
            dlg.Released = this.scanningKeyRelease;

            dlg.ShowDialog(this);

            this.scanningFiltered = dlg.Filtered;
            this.scanningUnique = dlg.Unique;
            this.scanningKeyPress = dlg.Pressed;
            this.scanningKeyRelease = dlg.Released;

            this.Cursor = oldCursor;
        }

        private void mainKeyMapping_Click(object sender, EventArgs e)
        {
            if (this.selectedButton == null)
            {
                MessageBox.Show(
                    this,
                    "Select a keyboard button on displayed keyboard and try again!",
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk
                );
            }
            else
            {
                KeyMapping dlgMapping = new KeyMapping(this.selectedButton);
                DialogResult result = dlgMapping.ShowDialog();

                if (result == DialogResult.Retry) // Advanced button selected.
                {
                    AdvancedKeyMapping dlgAdvanced = new AdvancedKeyMapping(this.advancedMappings);
                    result = dlgAdvanced.ShowDialog();
                }
            }
        }

        //
        // Overwritten windows message handler.
        //
        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == (int)WindowMessage.WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case aboutCommandID:
                        AboutBox dlg = new AboutBox();
                        dlg.ShowDialog(this);
                        break;
                }
            }
            // Call base class function
            base.WndProc(ref msg);
        }
    }
}
