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

namespace ScancodeMapping
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainStatusBar = new System.Windows.Forms.StatusStrip();
            this.generalStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.keysDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.standard101MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standard102MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.winkey101MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.winkey102MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.standardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enhancedMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alignmentDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.usstandardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.germanyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolBar = new System.Windows.Forms.ToolStrip();
            this.mainExitButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mainSaveMapping = new System.Windows.Forms.ToolStripButton();
            this.mainRemoveMapping = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mainShowMapping = new System.Windows.Forms.ToolStripButton();
            this.mainExportMapping = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mainScanKeyboard = new System.Windows.Forms.ToolStripButton();
            this.mainKeyMapping = new System.Windows.Forms.ToolStripButton();
            this.mainStatusBar.SuspendLayout();
            this.mainToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainStatusBar
            // 
            this.mainStatusBar.BackColor = System.Drawing.Color.Transparent;
            this.mainStatusBar.GripMargin = new System.Windows.Forms.Padding(0);
            this.mainStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generalStatusLabel,
            this.keysDropDown,
            this.layoutDropDown,
            this.alignmentDropDown});
            this.mainStatusBar.Location = new System.Drawing.Point(0, 510);
            this.mainStatusBar.Name = "mainStatusBar";
            this.mainStatusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainStatusBar.Size = new System.Drawing.Size(977, 25);
            this.mainStatusBar.SizingGrip = false;
            this.mainStatusBar.TabIndex = 2;
            // 
            // generalStatusLabel
            // 
            this.generalStatusLabel.AutoSize = false;
            this.generalStatusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.generalStatusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.generalStatusLabel.Margin = new System.Windows.Forms.Padding(0, 3, 0, 1);
            this.generalStatusLabel.Name = "generalStatusLabel";
            this.generalStatusLabel.Size = new System.Drawing.Size(300, 21);
            this.generalStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keysDropDown
            // 
            this.keysDropDown.AutoSize = false;
            this.keysDropDown.AutoToolTip = false;
            this.keysDropDown.BackColor = System.Drawing.Color.Transparent;
            this.keysDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.keysDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standard101MenuItem,
            this.standard102MenuItem,
            this.winkey101MenuItem,
            this.winkey102MenuItem});
            this.keysDropDown.ForeColor = System.Drawing.SystemColors.ControlText;
            this.keysDropDown.Image = ((System.Drawing.Image)(resources.GetObject("keysDropDown.Image")));
            this.keysDropDown.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.keysDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.keysDropDown.Name = "keysDropDown";
            this.keysDropDown.Size = new System.Drawing.Size(180, 23);
            this.keysDropDown.Text = "???";
            // 
            // standard101MenuItem
            // 
            this.standard101MenuItem.Name = "standard101MenuItem";
            this.standard101MenuItem.Size = new System.Drawing.Size(232, 22);
            this.standard101MenuItem.Text = "Standard Keyboard (101 keys)";
            this.standard101MenuItem.Click += new System.EventHandler(this.standard101MenuItem_Click);
            // 
            // standard102MenuItem
            // 
            this.standard102MenuItem.Name = "standard102MenuItem";
            this.standard102MenuItem.Size = new System.Drawing.Size(232, 22);
            this.standard102MenuItem.Text = "Standard Keyboard (102 keys)";
            this.standard102MenuItem.Click += new System.EventHandler(this.standard102MenuItem_Click);
            // 
            // winkey101MenuItem
            // 
            this.winkey101MenuItem.Name = "winkey101MenuItem";
            this.winkey101MenuItem.Size = new System.Drawing.Size(232, 22);
            this.winkey101MenuItem.Text = "Windows Keyboard (101 keys)";
            this.winkey101MenuItem.Click += new System.EventHandler(this.winkey101MenuItem_Click);
            // 
            // winkey102MenuItem
            // 
            this.winkey102MenuItem.Name = "winkey102MenuItem";
            this.winkey102MenuItem.Size = new System.Drawing.Size(232, 22);
            this.winkey102MenuItem.Text = "Windows Keyboard (102 keys)";
            this.winkey102MenuItem.Click += new System.EventHandler(this.winkey102MenuItem_Click);
            // 
            // layoutDropDown
            // 
            this.layoutDropDown.AutoSize = false;
            this.layoutDropDown.AutoToolTip = false;
            this.layoutDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.layoutDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.standardMenuItem,
            this.enhancedMenuItem});
            this.layoutDropDown.Image = ((System.Drawing.Image)(resources.GetObject("layoutDropDown.Image")));
            this.layoutDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.layoutDropDown.Name = "layoutDropDown";
            this.layoutDropDown.Size = new System.Drawing.Size(85, 23);
            this.layoutDropDown.Text = "???";
            // 
            // standardMenuItem
            // 
            this.standardMenuItem.Name = "standardMenuItem";
            this.standardMenuItem.Size = new System.Drawing.Size(132, 22);
            this.standardMenuItem.Text = "Standard";
            this.standardMenuItem.Click += new System.EventHandler(this.standardToolStripMenuItem_Click);
            // 
            // enhancedMenuItem
            // 
            this.enhancedMenuItem.Name = "enhancedMenuItem";
            this.enhancedMenuItem.Size = new System.Drawing.Size(132, 22);
            this.enhancedMenuItem.Text = "Enhanced";
            this.enhancedMenuItem.Click += new System.EventHandler(this.enhancedToolStripMenuItem_Click);
            // 
            // alignmentDropDown
            // 
            this.alignmentDropDown.AutoSize = false;
            this.alignmentDropDown.AutoToolTip = false;
            this.alignmentDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.alignmentDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usstandardMenuItem,
            this.germanyMenuItem});
            this.alignmentDropDown.Image = ((System.Drawing.Image)(resources.GetObject("alignmentDropDown.Image")));
            this.alignmentDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.alignmentDropDown.Name = "alignmentDropDown";
            this.alignmentDropDown.Size = new System.Drawing.Size(85, 23);
            this.alignmentDropDown.Text = "???";
            // 
            // usstandardMenuItem
            // 
            this.usstandardMenuItem.Name = "usstandardMenuItem";
            this.usstandardMenuItem.Size = new System.Drawing.Size(146, 22);
            this.usstandardMenuItem.Text = "US-Standard";
            this.usstandardMenuItem.Click += new System.EventHandler(this.usstandardMenuItem_Click);
            // 
            // germanyMenuItem
            // 
            this.germanyMenuItem.Name = "germanyMenuItem";
            this.germanyMenuItem.Size = new System.Drawing.Size(146, 22);
            this.germanyMenuItem.Text = "Germany";
            this.germanyMenuItem.Click += new System.EventHandler(this.germanyMenuItem_Click);
            // 
            // mainToolBar
            // 
            this.mainToolBar.BackColor = System.Drawing.Color.Transparent;
            this.mainToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.mainToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainExitButton,
            this.toolStripSeparator1,
            this.mainSaveMapping,
            this.mainRemoveMapping,
            this.toolStripSeparator2,
            this.mainShowMapping,
            this.mainExportMapping,
            this.toolStripSeparator3,
            this.mainScanKeyboard,
            this.mainKeyMapping});
            this.mainToolBar.Location = new System.Drawing.Point(0, 0);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Padding = new System.Windows.Forms.Padding(4);
            this.mainToolBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainToolBar.Size = new System.Drawing.Size(977, 47);
            this.mainToolBar.TabIndex = 3;
            this.mainToolBar.Text = "mainToolBar";
            // 
            // mainExitButton
            // 
            this.mainExitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mainExitButton.Image = global::ScancodeMapping.Properties.Resources.ExitButton;
            this.mainExitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainExitButton.Name = "mainExitButton";
            this.mainExitButton.Size = new System.Drawing.Size(36, 36);
            this.mainExitButton.Text = "Exit";
            this.mainExitButton.ToolTipText = "Exit Scancode Mapping program.";
            this.mainExitButton.Click += new System.EventHandler(this.mainExitButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // mainSaveMapping
            // 
            this.mainSaveMapping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mainSaveMapping.Image = global::ScancodeMapping.Properties.Resources.SaveMapping;
            this.mainSaveMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainSaveMapping.Name = "mainSaveMapping";
            this.mainSaveMapping.Size = new System.Drawing.Size(36, 36);
            this.mainSaveMapping.Text = "Save";
            this.mainSaveMapping.ToolTipText = "Write current mappings into Windows Registry.";
            this.mainSaveMapping.Click += new System.EventHandler(this.mainSaveMapping_Click);
            // 
            // mainRemoveMapping
            // 
            this.mainRemoveMapping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mainRemoveMapping.Image = global::ScancodeMapping.Properties.Resources.RemoveMapping;
            this.mainRemoveMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainRemoveMapping.Name = "mainRemoveMapping";
            this.mainRemoveMapping.Size = new System.Drawing.Size(36, 36);
            this.mainRemoveMapping.Text = "Remove";
            this.mainRemoveMapping.ToolTipText = "Remove all mappings from Windows Registry.";
            this.mainRemoveMapping.Click += new System.EventHandler(this.mainRemoveMapping_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
            // 
            // mainShowMapping
            // 
            this.mainShowMapping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mainShowMapping.Image = global::ScancodeMapping.Properties.Resources.ShowMapping;
            this.mainShowMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainShowMapping.Name = "mainShowMapping";
            this.mainShowMapping.Size = new System.Drawing.Size(36, 36);
            this.mainShowMapping.Text = "Show";
            this.mainShowMapping.ToolTipText = "Show current mappings as raw data.";
            this.mainShowMapping.Click += new System.EventHandler(this.mainShowMapping_Click);
            // 
            // mainExportMapping
            // 
            this.mainExportMapping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mainExportMapping.Image = global::ScancodeMapping.Properties.Resources.ExportMapping;
            this.mainExportMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainExportMapping.Name = "mainExportMapping";
            this.mainExportMapping.Size = new System.Drawing.Size(36, 36);
            this.mainExportMapping.Text = "Export";
            this.mainExportMapping.ToolTipText = "Export current mappings to a REG file.";
            this.mainExportMapping.Click += new System.EventHandler(this.mainExportMapping_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 39);
            // 
            // mainScanKeyboard
            // 
            this.mainScanKeyboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mainScanKeyboard.Image = global::ScancodeMapping.Properties.Resources.ScanKeyboard;
            this.mainScanKeyboard.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainScanKeyboard.Name = "mainScanKeyboard";
            this.mainScanKeyboard.Size = new System.Drawing.Size(36, 36);
            this.mainScanKeyboard.Text = "Scan";
            this.mainScanKeyboard.ToolTipText = "Scan all keys of your keyboard and display some useful information.";
            this.mainScanKeyboard.Click += new System.EventHandler(this.mainScanKeyboard_Click);
            // 
            // mainKeyMapping
            // 
            this.mainKeyMapping.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mainKeyMapping.Image = global::ScancodeMapping.Properties.Resources.KeyMapping;
            this.mainKeyMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainKeyMapping.Name = "mainKeyMapping";
            this.mainKeyMapping.Size = new System.Drawing.Size(36, 36);
            this.mainKeyMapping.Text = "Mapping";
            this.mainKeyMapping.ToolTipText = "Change mapping for currently selected keyboard key.";
            this.mainKeyMapping.Click += new System.EventHandler(this.mainKeyMapping_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(977, 535);
            this.Controls.Add(this.mainToolBar);
            this.Controls.Add(this.mainStatusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Scancode Mapping 1.0";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.mainStatusBar.ResumeLayout(false);
            this.mainStatusBar.PerformLayout();
            this.mainToolBar.ResumeLayout(false);
            this.mainToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip mainStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel generalStatusLabel;
        private System.Windows.Forms.ToolStripDropDownButton keysDropDown;
        private System.Windows.Forms.ToolStripMenuItem winkey101MenuItem;
        private System.Windows.Forms.ToolStripMenuItem winkey102MenuItem;
        private System.Windows.Forms.ToolStripMenuItem standard102MenuItem;
        private System.Windows.Forms.ToolStripMenuItem standard101MenuItem;
        private System.Windows.Forms.ToolStripDropDownButton layoutDropDown;
        private System.Windows.Forms.ToolStripMenuItem standardMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enhancedMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton alignmentDropDown;
        private System.Windows.Forms.ToolStripMenuItem germanyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem usstandardMenuItem;
        private System.Windows.Forms.ToolStrip mainToolBar;
        private System.Windows.Forms.ToolStripButton mainExitButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton mainSaveMapping;
        private System.Windows.Forms.ToolStripButton mainRemoveMapping;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton mainShowMapping;
        private System.Windows.Forms.ToolStripButton mainExportMapping;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton mainScanKeyboard;
        private System.Windows.Forms.ToolStripButton mainKeyMapping;
    }
}

