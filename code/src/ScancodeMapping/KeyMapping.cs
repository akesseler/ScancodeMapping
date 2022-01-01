using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
