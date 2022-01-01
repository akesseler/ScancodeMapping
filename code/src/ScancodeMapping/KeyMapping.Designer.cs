namespace ScancodeMapping
{
    partial class KeyMapping
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyMapping));
            this.closeButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.advancedButton = new System.Windows.Forms.Button();
            this.keyMappingGroup = new System.Windows.Forms.GroupBox();
            this.buttonsCombo = new System.Windows.Forms.ComboBox();
            this.remapRadioButton = new System.Windows.Forms.RadioButton();
            this.restoreRadioButton = new System.Windows.Forms.RadioButton();
            this.disableRadioButton = new System.Windows.Forms.RadioButton();
            this.keyMappingGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(184, 128);
            this.closeButton.Margin = new System.Windows.Forms.Padding(5);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(99, 128);
            this.applyButton.Margin = new System.Windows.Forms.Padding(5);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 2;
            this.applyButton.Text = "&Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // advancedButton
            // 
            this.advancedButton.Location = new System.Drawing.Point(14, 128);
            this.advancedButton.Margin = new System.Windows.Forms.Padding(5);
            this.advancedButton.Name = "advancedButton";
            this.advancedButton.Size = new System.Drawing.Size(75, 23);
            this.advancedButton.TabIndex = 1;
            this.advancedButton.Text = "A&dvanced";
            this.advancedButton.UseVisualStyleBackColor = true;
            this.advancedButton.Click += new System.EventHandler(this.advancedButton_Click);
            // 
            // keyMappingGroup
            // 
            this.keyMappingGroup.Controls.Add(this.buttonsCombo);
            this.keyMappingGroup.Controls.Add(this.remapRadioButton);
            this.keyMappingGroup.Controls.Add(this.restoreRadioButton);
            this.keyMappingGroup.Controls.Add(this.disableRadioButton);
            this.keyMappingGroup.Location = new System.Drawing.Point(14, 14);
            this.keyMappingGroup.Margin = new System.Windows.Forms.Padding(5);
            this.keyMappingGroup.Name = "keyMappingGroup";
            this.keyMappingGroup.Size = new System.Drawing.Size(245, 104);
            this.keyMappingGroup.TabIndex = 0;
            this.keyMappingGroup.TabStop = false;
            this.keyMappingGroup.Text = "Key Mapping Actions";
            // 
            // buttonsCombo
            // 
            this.buttonsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.buttonsCombo.Enabled = false;
            this.buttonsCombo.FormattingEnabled = true;
            this.buttonsCombo.Location = new System.Drawing.Point(92, 68);
            this.buttonsCombo.Name = "buttonsCombo";
            this.buttonsCombo.Size = new System.Drawing.Size(137, 21);
            this.buttonsCombo.TabIndex = 3;
            // 
            // remapRadioButton
            // 
            this.remapRadioButton.AutoSize = true;
            this.remapRadioButton.Location = new System.Drawing.Point(7, 68);
            this.remapRadioButton.Name = "remapRadioButton";
            this.remapRadioButton.Size = new System.Drawing.Size(79, 17);
            this.remapRadioButton.TabIndex = 2;
            this.remapRadioButton.Text = "&Map Key to";
            this.remapRadioButton.UseVisualStyleBackColor = true;
            this.remapRadioButton.CheckedChanged += new System.EventHandler(this.remapRadioButton_CheckedChanged);
            // 
            // restoreRadioButton
            // 
            this.restoreRadioButton.AutoSize = true;
            this.restoreRadioButton.Location = new System.Drawing.Point(7, 44);
            this.restoreRadioButton.Name = "restoreRadioButton";
            this.restoreRadioButton.Size = new System.Drawing.Size(83, 17);
            this.restoreRadioButton.TabIndex = 1;
            this.restoreRadioButton.Text = "&Restore Key";
            this.restoreRadioButton.UseVisualStyleBackColor = true;
            this.restoreRadioButton.CheckedChanged += new System.EventHandler(this.restoreRadioButton_CheckedChanged);
            // 
            // disableRadioButton
            // 
            this.disableRadioButton.AutoSize = true;
            this.disableRadioButton.Checked = true;
            this.disableRadioButton.Location = new System.Drawing.Point(7, 20);
            this.disableRadioButton.Name = "disableRadioButton";
            this.disableRadioButton.Size = new System.Drawing.Size(81, 17);
            this.disableRadioButton.TabIndex = 0;
            this.disableRadioButton.TabStop = true;
            this.disableRadioButton.Text = "&Disable Key";
            this.disableRadioButton.UseVisualStyleBackColor = true;
            this.disableRadioButton.CheckedChanged += new System.EventHandler(this.disableRadioButton_CheckedChanged);
            // 
            // KeyMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Ivory;
            this.ClientSize = new System.Drawing.Size(272, 164);
            this.Controls.Add(this.keyMappingGroup);
            this.Controls.Add(this.advancedButton);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyMapping";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Key Mapping";
            this.Load += new System.EventHandler(this.KeyMapping_Load);
            this.keyMappingGroup.ResumeLayout(false);
            this.keyMappingGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button advancedButton;
        private System.Windows.Forms.GroupBox keyMappingGroup;
        private System.Windows.Forms.RadioButton remapRadioButton;
        private System.Windows.Forms.RadioButton restoreRadioButton;
        private System.Windows.Forms.RadioButton disableRadioButton;
        private System.Windows.Forms.ComboBox buttonsCombo;
    }
}