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
        private Boolean scanningFiltered = true;
        private Boolean scanningUnique = true;
        private Boolean scanningKeyPress = true;
        private Boolean scanningKeyRelease = false;

        // XML file node and attribute names
        private const String xmlSettings = "settings";
        private const String xmlKeyboard = "keyboard";
        private const String xmlScanning = "scanning";
        private const String xmlAlignment = "alignment";
        private const String xmlLayout = "layout";
        private const String xmlKeys = "keys";
        private const String xmlFiltered = "filtered";
        private const String xmlUnique = "unique";
        private const String xmlKeyPress = "keypress";
        private const String xmlKeyRelease = "keyrelease";

        private const Int32 aboutCommandId = 0x100;

        private Boolean dirty = false;
        private Boolean reboot = false;

        private readonly KeyboardPanel keyboardPanel;
        private KeyButton selectedButton = null;

        private readonly ArrayList advancedMappings = new ArrayList();

        public MainForm()
        {
            this.InitializeComponent();

            this.SuspendLayout();

            this.keyboardPanel = new KeyboardPanel(TKeyboardAlignment.Default);

            this.Controls.Add(this.keyboardPanel);

            this.keyboardPanel.Location = new Point(0, this.mainToolBar.Size.Height);

            this.FitWindowLayout();

            this.ResumeLayout(false);

            // Don't do to much right here because the main form is not yet instantiated!
        }

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

        public Boolean Dirty
        {
            get { return this.dirty; }
            set { this.dirty = value; }
        }

        public Boolean Reboot
        {
            get { return this.reboot; }
            set { this.reboot = value; }
        }

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

        public void StatusbarChanged(String text)
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

            Int32 width = this.ClientSize.Width
                        - this.layoutDropDown.Width
                        - this.keysDropDown.Width
                        - this.alignmentDropDown.Width - 1;

            this.generalStatusLabel.Size = new Size(width, this.generalStatusLabel.Size.Height);
        }

        public KeyButton FindButtonByScancode(Int32 scnacode, Int32 extended)
        {
            return this.keyboardPanel.FindButtonByScancode(scnacode, extended);
        }

        public ArrayList CollectButtons()
        {
            ArrayList result = new ArrayList();

            for (Int32 panels = 0; panels < this.keyboardPanel.Controls.Count; panels++)
            {
                if (this.keyboardPanel.Controls[panels] is KeyPanel panel)
                {
                    for (Int32 buttons = 0; buttons < panel.Controls.Count; buttons++)
                    {
                        if (panel.Controls[buttons] is KeyButton button)
                        {
                            result.Add(button);
                        }
                    }
                }
            }

            return result;
        }

        private void LoadSettings(String file)
        {
            // Read XML file content.
            TextReader reader;
            try { reader = new StreamReader(file); }
            catch (Exception) { return; }
            XmlDocument document = new XmlDocument();
            document.Load(reader);
            reader.Close();
            reader.Dispose();

            // Load 'settings' root tag and process contained entries.
            XmlNodeList settings = document.GetElementsByTagName(xmlSettings);

            for (Int32 outer = 0; outer < settings.Count; outer++)
            {
                try
                {
                    XmlNodeList nodes = settings.Item(outer).ChildNodes;

                    // Deserialize data depending on table type.
                    for (Int32 inner = 0; inner < nodes.Count; inner++)
                    {
                        XmlNode entry = nodes.Item(inner);
                        String value;

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
                                this.scanningFiltered = Convert.ToBoolean(entry.Attributes.GetNamedItem(xmlFiltered).Value);
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                this.scanningUnique = Convert.ToBoolean(entry.Attributes.GetNamedItem(xmlUnique).Value);
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                this.scanningKeyPress = Convert.ToBoolean(entry.Attributes.GetNamedItem(xmlKeyPress).Value);
                            }
                            catch (Exception)
                            {
                                // Do nothing; use defaults instead!
                            }

                            try
                            {
                                this.scanningKeyRelease = Convert.ToBoolean(entry.Attributes.GetNamedItem(xmlKeyRelease).Value);
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

        private void SaveSettings(String file)
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
            writer.Dispose();
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
            // Source of that code came from (no longer available) http://www.geekpedia.com/code36_Shut-down-system-using-Csharp.html
            // Alternative source found on : https://stackoverflow.com/a/462409
            // Description of all flags: https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32shutdown-method-in-class-win32-operatingsystem
#if !DEBUG
            try
            {
                using (System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_OperatingSystem"))
                {
                    mc.Get();

                    // You can't shutdown without security privileges
                    mc.Scope.Options.EnablePrivileges = true;

                    using (System.Management.ManagementBaseObject args = mc.GetMethodParameters("Win32Shutdown"))
                    {
                        // Flag 6 means we want to "forced reboot" of the system!
                        args["Flags"] = "6";
                        args["Reserved"] = "0";

                        foreach (System.Management.ManagementObject mo in mc.GetInstances())
                        {
                            mo.InvokeMethod("Win32Shutdown", args, null);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, 
                    $"Trying to reboot the system has crashed unexpectedly!{Environment.NewLine}{Environment.NewLine}{exception.Message}",
                    "System Reboot", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
#else
            MessageBox.Show(this, "System reboot is disabled in debug mode!",
                "System Reboot", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
        }

        private String MakeMappingInfo(AdvancedMapping entry)
        {
            String result = String.Empty;

            Int32 helper = (entry.ExtendedKey1 << 8) | entry.ScancodeKey1;

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

        private void OnStandard101MenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys101;
        }

        private void OnStandard102MenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys102;
        }

        private void OnWinkey102MenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys106;
        }

        private void OnWinkey101MenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardKeys = TKeyboardKeys.Keys105;
        }

        private void OnEnhancedToolStripMenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardLayout = TKeyboardLayout.Enhanced;
        }

        private void OnStandardToolStripMenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardLayout = TKeyboardLayout.Standard;
        }

        private void OnGermanyMenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardAlignment = TKeyboardAlignment.Germany;
        }

        private void OnUsStandardMenuItemClick(Object sender, EventArgs args)
        {
            this.KeyboardAlignment = TKeyboardAlignment.USStandard;
        }

        private void OnMainFormLoad(Object sender, EventArgs args)
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

            String layout = Win32Wrapper.GetKeyboardLayoutName();

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

            String primaryLanguage = layout.Substring(4);

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

            this.LoadSettings(Path.ChangeExtension(Application.ExecutablePath, ".xml"));

            try
            {
                // Put About Box to the system menu.
                SystemMenu systemMenu = SystemMenu.GetSystemMenu(this);
                Int32 position = systemMenu.FindPositionByCommandID((Int32)SystemMenuCommand.SC_CLOSE);
                systemMenu.InsertSeparator(position);
                systemMenu.InsertMenu(position, MainForm.aboutCommandId, "About ...");

                // Get current scancode map from Windows Registry
                ScancodeMap mappings = ScancodeMap.RegistryLoad();

                // Update mapping state on every key panel.
                this.keyboardPanel.UpdateMappings(mappings);

                Int32 index = 0;
                Int32 oriScancode = 0;
                Int32 oriExtended = 0;
                Int32 mapScancode = 0;
                Int32 mapExtended = 0;

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

        private void OnMainFormFormClosing(Object sender, FormClosingEventArgs args)
        {
            this.Cursor = Cursors.WaitCursor;

            this.SaveSettings(Path.ChangeExtension(Application.ExecutablePath, ".xml"));

            if (this.Dirty)
            {
                DialogResult result = MessageBox.Show(this,
                    "Would you like to store your scancode mapping changes into the Windows Registry and reboot the system afterwards?",
                    "Save Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    this.SaveMappings();
                    this.RebootSystem();
                }
            }
            else if (this.Reboot)
            {
                DialogResult result = MessageBox.Show(this,
                    "Would you like to reboot your system to get your scancode mapping changes working?",
                    "System Reboot", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    this.RebootSystem();
                }
            }
        }

        private void OnMainExitButtonClick(Object sender, EventArgs args)
        {
            this.Close();
        }

        private void OnMainSaveMappingClick(Object sender, EventArgs args)
        {
            this.SaveMappings();
        }

        private void OnMainRemoveMappingClick(Object sender, EventArgs args)
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

        private void OnMainShowMappingClick(Object sender, EventArgs args)
        {
            ScancodeMap mappings = new ScancodeMap();
            this.keyboardPanel.CollectMappings(mappings);

            // Add advanced mappings to raw data list.
            foreach (AdvancedMapping entry in this.advancedMappings)
            {
                mappings.Append(entry.ScancodeKey1, entry.ExtendedKey1, entry.ScancodeKey2, entry.ExtendedKey2);
            }

            String[] rawData = mappings.GetRawDataList();

            Int32 index = 0;
            rawData[index++] += "\tVersion";
            rawData[index++] += "\tFlags";
            rawData[index++] += "\tElements";

            // Append assigned keys to the output.
            for (; index < rawData.Length - 1; index++)
            {
                Int32 mapScancode = Convert.ToInt32(rawData[index].Substring(0, 2), 16);
                Int32 mapExtended = Convert.ToInt32(rawData[index].Substring(3, 2), 16);
                Int32 oriScancode = Convert.ToInt32(rawData[index].Substring(6, 2), 16);
                Int32 oriExtended = Convert.ToInt32(rawData[index].Substring(9, 2), 16);

                String helper = KeyButton.MakeMappingInfo(
                    this.keyboardPanel.FindButtonByScancode(oriScancode, oriExtended),
                    this.keyboardPanel.FindButtonByScancode(mapScancode, mapExtended)
                );

                if (helper == String.Empty)
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

        private void OnMainExportMappingClick(Object sender, EventArgs args)
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                Filter = "Windows Registry files (*.reg)|*.reg|All files (*.*)|*.*",
                AutoUpgradeEnabled = true,
                CheckFileExists = false,
                RestoreDirectory = true
            };

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

        private void OnMainScanKeyboardClick(Object sender, EventArgs args)
        {
            Cursor oldCursor = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            KeyboardScanning dlg = new KeyboardScanning()
            {
                Filtered = this.scanningFiltered,
                Unique = this.scanningUnique,
                Pressed = this.scanningKeyPress,
                Released = this.scanningKeyRelease
            };

            dlg.ShowDialog(this);

            this.scanningFiltered = dlg.Filtered;
            this.scanningUnique = dlg.Unique;
            this.scanningKeyPress = dlg.Pressed;
            this.scanningKeyRelease = dlg.Released;

            this.Cursor = oldCursor;
        }

        private void OnMainKeyMappingClick(Object sender, EventArgs args)
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

        protected override void WndProc(ref Message msg)
        {
            if (msg.Msg == (Int32)WindowMessage.WM_SYSCOMMAND)
            {
                switch (msg.WParam.ToInt32())
                {
                    case MainForm.aboutCommandId:
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
