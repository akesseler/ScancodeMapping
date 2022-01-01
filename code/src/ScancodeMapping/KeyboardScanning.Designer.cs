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
    partial class KeyboardScanning
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyboardScanning));
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.lbCountValue = new System.Windows.Forms.Label();
            this.lbCount = new System.Windows.Forms.Label();
            this.cbKeyRelease = new System.Windows.Forms.CheckBox();
            this.cbKeyPress = new System.Windows.Forms.CheckBox();
            this.cbUnique = new System.Windows.Forms.CheckBox();
            this.cbFiltered = new System.Windows.Forms.CheckBox();
            this.btnAction = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.lbAttentionMessage = new System.Windows.Forms.Label();
            this.lbAttention = new System.Windows.Forms.Label();
            this.lblMainMessage = new System.Windows.Forms.Label();
            this.fillPanel = new System.Windows.Forms.Panel();
            this.lvScanData = new System.Windows.Forms.ListView();
            this.lvScanDataName = new System.Windows.Forms.ColumnHeader();
            this.lvScanDataKeyName = new System.Windows.Forms.ColumnHeader();
            this.lvScanDataVKCode = new System.Windows.Forms.ColumnHeader();
            this.lvScanDataScancode = new System.Windows.Forms.ColumnHeader();
            this.lvScanDataExtended = new System.Windows.Forms.ColumnHeader();
            this.lvScanDataFlags = new System.Windows.Forms.ColumnHeader();
            this.lvScanDataKeyStoke = new System.Windows.Forms.ColumnHeader();
            this.lvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.emptyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separatorMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.fillPanel.SuspendLayout();
            this.lvContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomPanel
            // 
            this.bottomPanel.Controls.Add(this.lbCountValue);
            this.bottomPanel.Controls.Add(this.lbCount);
            this.bottomPanel.Controls.Add(this.cbKeyRelease);
            this.bottomPanel.Controls.Add(this.cbKeyPress);
            this.bottomPanel.Controls.Add(this.cbUnique);
            this.bottomPanel.Controls.Add(this.cbFiltered);
            this.bottomPanel.Controls.Add(this.btnAction);
            this.bottomPanel.Controls.Add(this.btnClose);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 422);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Padding = new System.Windows.Forms.Padding(10);
            this.bottomPanel.Size = new System.Drawing.Size(692, 44);
            this.bottomPanel.TabIndex = 2;
            // 
            // lbCountValue
            // 
            this.lbCountValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbCountValue.Location = new System.Drawing.Point(430, 10);
            this.lbCountValue.Name = "lbCountValue";
            this.lbCountValue.Size = new System.Drawing.Size(60, 24);
            this.lbCountValue.TabIndex = 5;
            this.lbCountValue.Text = "0";
            this.lbCountValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCount
            // 
            this.lbCount.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbCount.Location = new System.Drawing.Point(370, 10);
            this.lbCount.Name = "lbCount";
            this.lbCount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbCount.Size = new System.Drawing.Size(60, 24);
            this.lbCount.TabIndex = 4;
            this.lbCount.Text = "Count:";
            this.lbCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbKeyRelease
            // 
            this.cbKeyRelease.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbKeyRelease.Location = new System.Drawing.Point(280, 10);
            this.cbKeyRelease.Name = "cbKeyRelease";
            this.cbKeyRelease.Size = new System.Drawing.Size(90, 24);
            this.cbKeyRelease.TabIndex = 3;
            this.cbKeyRelease.Text = "Key Release";
            this.cbKeyRelease.UseVisualStyleBackColor = true;
            // 
            // cbKeyPress
            // 
            this.cbKeyPress.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbKeyPress.Location = new System.Drawing.Point(190, 10);
            this.cbKeyPress.Name = "cbKeyPress";
            this.cbKeyPress.Size = new System.Drawing.Size(90, 24);
            this.cbKeyPress.TabIndex = 2;
            this.cbKeyPress.Text = "Key Press";
            this.cbKeyPress.UseVisualStyleBackColor = true;
            // 
            // cbUnique
            // 
            this.cbUnique.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbUnique.Location = new System.Drawing.Point(100, 10);
            this.cbUnique.Name = "cbUnique";
            this.cbUnique.Size = new System.Drawing.Size(90, 24);
            this.cbUnique.TabIndex = 1;
            this.cbUnique.Text = "Unique";
            this.cbUnique.UseVisualStyleBackColor = true;
            // 
            // cbFiltered
            // 
            this.cbFiltered.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbFiltered.Location = new System.Drawing.Point(10, 10);
            this.cbFiltered.Name = "cbFiltered";
            this.cbFiltered.Size = new System.Drawing.Size(90, 24);
            this.cbFiltered.TabIndex = 0;
            this.cbFiltered.Text = "Filtered";
            this.cbFiltered.UseVisualStyleBackColor = true;
            // 
            // btnAction
            // 
            this.btnAction.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAction.Location = new System.Drawing.Point(532, 10);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(75, 24);
            this.btnAction.TabIndex = 6;
            this.btnAction.Text = "Start";
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(607, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 24);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.lbAttentionMessage);
            this.topPanel.Controls.Add(this.lbAttention);
            this.topPanel.Controls.Add(this.lblMainMessage);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Padding = new System.Windows.Forms.Padding(10);
            this.topPanel.Size = new System.Drawing.Size(692, 136);
            this.topPanel.TabIndex = 0;
            // 
            // lbAttentionMessage
            // 
            this.lbAttentionMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAttentionMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAttentionMessage.Location = new System.Drawing.Point(85, 73);
            this.lbAttentionMessage.Name = "lbAttentionMessage";
            this.lbAttentionMessage.Size = new System.Drawing.Size(597, 53);
            this.lbAttentionMessage.TabIndex = 2;
            this.lbAttentionMessage.Text = "This dialog box will reflect your keyboard activities during the keyboard scan. B" +
                "ut keep in mind you are only able to stop with scanning your keyboard by clickin" +
                "g the Stop button with your mouse!";
            // 
            // lbAttention
            // 
            this.lbAttention.Dock = System.Windows.Forms.DockStyle.Left;
            this.lbAttention.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAttention.Location = new System.Drawing.Point(10, 73);
            this.lbAttention.Name = "lbAttention";
            this.lbAttention.Size = new System.Drawing.Size(75, 53);
            this.lbAttention.TabIndex = 1;
            this.lbAttention.Text = "Attention:";
            // 
            // lblMainMessage
            // 
            this.lblMainMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMainMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainMessage.Location = new System.Drawing.Point(10, 10);
            this.lblMainMessage.Name = "lblMainMessage";
            this.lblMainMessage.Size = new System.Drawing.Size(672, 63);
            this.lblMainMessage.TabIndex = 0;
            this.lblMainMessage.Text = resources.GetString("lblMainMessage.Text");
            // 
            // fillPanel
            // 
            this.fillPanel.Controls.Add(this.lvScanData);
            this.fillPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillPanel.Location = new System.Drawing.Point(0, 136);
            this.fillPanel.Name = "fillPanel";
            this.fillPanel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.fillPanel.Size = new System.Drawing.Size(692, 286);
            this.fillPanel.TabIndex = 1;
            // 
            // lvScanData
            // 
            this.lvScanData.AutoArrange = false;
            this.lvScanData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvScanDataName,
            this.lvScanDataKeyName,
            this.lvScanDataVKCode,
            this.lvScanDataScancode,
            this.lvScanDataExtended,
            this.lvScanDataFlags,
            this.lvScanDataKeyStoke});
            this.lvScanData.ContextMenuStrip = this.lvContextMenu;
            this.lvScanData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvScanData.FullRowSelect = true;
            this.lvScanData.GridLines = true;
            this.lvScanData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvScanData.HideSelection = false;
            this.lvScanData.Location = new System.Drawing.Point(10, 0);
            this.lvScanData.MultiSelect = false;
            this.lvScanData.Name = "lvScanData";
            this.lvScanData.ShowGroups = false;
            this.lvScanData.Size = new System.Drawing.Size(672, 286);
            this.lvScanData.TabIndex = 0;
            this.lvScanData.UseCompatibleStateImageBehavior = false;
            this.lvScanData.View = System.Windows.Forms.View.Details;
            // 
            // lvScanDataName
            // 
            this.lvScanDataName.Text = "Virtual Name";
            this.lvScanDataName.Width = 120;
            // 
            // lvScanDataKeyName
            // 
            this.lvScanDataKeyName.Text = "Sytem Name";
            this.lvScanDataKeyName.Width = 130;
            // 
            // lvScanDataVKCode
            // 
            this.lvScanDataVKCode.Text = "Virtual Key";
            this.lvScanDataVKCode.Width = 80;
            // 
            // lvScanDataScancode
            // 
            this.lvScanDataScancode.Text = "Scancode";
            this.lvScanDataScancode.Width = 80;
            // 
            // lvScanDataExtended
            // 
            this.lvScanDataExtended.Text = "Extended";
            this.lvScanDataExtended.Width = 80;
            // 
            // lvScanDataFlags
            // 
            this.lvScanDataFlags.Text = "Key Flags";
            this.lvScanDataFlags.Width = 80;
            // 
            // lvScanDataKeyStoke
            // 
            this.lvScanDataKeyStoke.Text = "Key Stoke";
            this.lvScanDataKeyStoke.Width = 80;
            // 
            // lvContextMenu
            // 
            this.lvContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyMenuItem,
            this.removeMenuItem,
            this.separatorMenuItem1,
            this.exportMenuItem});
            this.lvContextMenu.Name = "lvContextMenu";
            this.lvContextMenu.Size = new System.Drawing.Size(125, 76);
            this.lvContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.lvContextMenu_Opening);
            // 
            // emptyMenuItem
            // 
            this.emptyMenuItem.Name = "emptyMenuItem";
            this.emptyMenuItem.Size = new System.Drawing.Size(124, 22);
            this.emptyMenuItem.Text = "Empty";
            this.emptyMenuItem.Click += new System.EventHandler(this.emptyMenuItem_Click);
            // 
            // removeMenuItem
            // 
            this.removeMenuItem.Name = "removeMenuItem";
            this.removeMenuItem.Size = new System.Drawing.Size(124, 22);
            this.removeMenuItem.Text = "Remove";
            this.removeMenuItem.Click += new System.EventHandler(this.removeMenuItem_Click);
            // 
            // separatorMenuItem1
            // 
            this.separatorMenuItem1.Name = "separatorMenuItem1";
            this.separatorMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // exportMenuItem
            // 
            this.exportMenuItem.Name = "exportMenuItem";
            this.exportMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exportMenuItem.Text = "Export";
            this.exportMenuItem.Click += new System.EventHandler(this.exportMenuItem_Click);
            // 
            // KeyboardScanning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(692, 466);
            this.Controls.Add(this.fillPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "KeyboardScanning";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keyboard Scanning";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.KeyboardScanning_FormClosing);
            this.bottomPanel.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.fillPanel.ResumeLayout(false);
            this.lvContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel fillPanel;
        private System.Windows.Forms.Label lblMainMessage;
        private System.Windows.Forms.ListView lvScanData;
        private System.Windows.Forms.ColumnHeader lvScanDataName;
        private System.Windows.Forms.ColumnHeader lvScanDataKeyName;
        private System.Windows.Forms.ColumnHeader lvScanDataVKCode;
        private System.Windows.Forms.ColumnHeader lvScanDataScancode;
        private System.Windows.Forms.ColumnHeader lvScanDataExtended;
        private System.Windows.Forms.ColumnHeader lvScanDataFlags;
        private System.Windows.Forms.ColumnHeader lvScanDataKeyStoke;
        private System.Windows.Forms.Label lbAttention;
        private System.Windows.Forms.Label lbAttentionMessage;
        private System.Windows.Forms.Label lbCountValue;
        private System.Windows.Forms.Label lbCount;
        private System.Windows.Forms.CheckBox cbFiltered;
        private System.Windows.Forms.ContextMenuStrip lvContextMenu;
        private System.Windows.Forms.ToolStripMenuItem emptyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeMenuItem;
        private System.Windows.Forms.ToolStripSeparator separatorMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportMenuItem;
        private System.Windows.Forms.CheckBox cbUnique;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox cbKeyRelease;
        private System.Windows.Forms.CheckBox cbKeyPress;
    }
}