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
using System.Collections.Generic;
using System.Windows.Forms;

namespace ScancodeMapping
{
    public partial class KeyMapping : Form
    {
        private readonly KeyButton currentButton = null;
        private Dictionary<String, KeyButton> usedButtons = null;

        public KeyMapping(KeyButton currentButton)
        {
            this.InitializeComponent();
            this.currentButton = currentButton;
        }

        private void OnKeyMappingLoad(Object sender, EventArgs args)
        {
            ArrayList buttons = App.GetMainForm().CollectButtons();
            buttons.Sort(new KeyButtonSorter());

            this.usedButtons = new Dictionary<String, KeyButton>(buttons.Count);

            for (Int32 index = 0; index < buttons.Count; index++)
            {
                this.usedButtons.Add(buttons[index].ToString(), (KeyButton)buttons[index]);
                this.buttonsCombo.Items.Add(buttons[index].ToString());
            }

            try
            {
                this.buttonsCombo.SelectedItem = this.currentButton.ToString();
            }
            catch (Exception)
            {
                this.buttonsCombo.SelectedIndex = 0;
            }
        }

        private void OnCloseButtonClick(Object sender, EventArgs args)
        {
            this.Close();
        }

        private void OnApplyButtonClick(Object sender, EventArgs args)
        {
            if (this.restoreRadioButton.Checked)
            {
                this.currentButton.RemapButton(KeyButton.RestoreButton);
            }
            else if (this.disableRadioButton.Checked)
            {
                this.currentButton.RemapButton(KeyButton.DisabledButton);
            }
            else if (this.remapRadioButton.Checked)
            {
                if (this.usedButtons.TryGetValue(this.buttonsCombo.SelectedItem.ToString(), out KeyButton remapButton))
                {
                    if (!this.currentButton.Equals(remapButton))
                    {
                        this.currentButton.RemapButton(remapButton);
                    }
                    else
                    {
                        this.currentButton.RemapButton(KeyButton.RestoreButton);
                    }
                }
            }
        }

        private void OnAdvancedButtonClick(Object sender, EventArgs args)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void OnDisableRadioButtonCheckedChanged(Object sender, EventArgs args)
        {
            this.buttonsCombo.Enabled = false;
        }

        private void OnRestoreRadioButtonCheckedChanged(Object sender, EventArgs args)
        {
            this.buttonsCombo.Enabled = false;
        }

        private void OnRemapRadioButtonCheckedChanged(Object sender, EventArgs args)
        {
            this.buttonsCombo.Enabled = true;
        }
    }
}
