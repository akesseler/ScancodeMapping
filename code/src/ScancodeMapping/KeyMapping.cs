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
        private KeyButton currentButton = null;
        private Dictionary<string, KeyButton> usedButtons = null;

        public KeyMapping(KeyButton currentButton)
        {
            InitializeComponent();
            this.currentButton = currentButton;
        }

        private void KeyMapping_Load(object sender, EventArgs e)
        {
            ArrayList buttons = App.GetMainForm().CollectButtons();
            buttons.Sort(new KeyButtonSorter());

            this.usedButtons = new Dictionary<string, KeyButton>(buttons.Count);

            for (int index = 0; index < buttons.Count; index++)
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

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void applyButton_Click(object sender, EventArgs e)
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
                KeyButton remapButton = null;
                if (this.usedButtons.TryGetValue(this.buttonsCombo.SelectedItem.ToString(), out remapButton))
                {
                    if (!currentButton.Equals(remapButton))
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

        private void advancedButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void disableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonsCombo.Enabled = false;
        }

        private void restoreRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonsCombo.Enabled = false;
        }

        private void remapRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonsCombo.Enabled = true;
        }
    }
}
