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
    partial class AdvancedKeyMapping
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedKeyMapping));
            this.closeButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.firstKeyGroup = new System.Windows.Forms.GroupBox();
            this.scancodeLabel1 = new System.Windows.Forms.Label();
            this.virtualKeyLabel1 = new System.Windows.Forms.Label();
            this.keyNameLabel1 = new System.Windows.Forms.Label();
            this.secondKeyGroup = new System.Windows.Forms.GroupBox();
            this.scancodeLabel2 = new System.Windows.Forms.Label();
            this.virtualKeyLabel2 = new System.Windows.Forms.Label();
            this.keyNameLabel2 = new System.Windows.Forms.Label();
            this.scancodeText2 = new System.Windows.Forms.Label();
            this.virtualKeyText2 = new System.Windows.Forms.Label();
            this.keyNameText2 = new System.Windows.Forms.Label();
            this.scancodeText1 = new System.Windows.Forms.Label();
            this.keyNameText1 = new System.Windows.Forms.Label();
            this.virtualKeyText1 = new System.Windows.Forms.Label();
            this.actionButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.firstKeyGroup.SuspendLayout();
            this.secondKeyGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(269, 225);
            this.closeButton.Margin = new System.Windows.Forms.Padding(5);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 11;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(184, 225);
            this.startButton.Margin = new System.Windows.Forms.Padding(5);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 10;
            this.startButton.Text = "&Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // firstKeyGroup
            // 
            this.firstKeyGroup.BackColor = System.Drawing.Color.Transparent;
            this.firstKeyGroup.Controls.Add(this.scancodeLabel1);
            this.firstKeyGroup.Controls.Add(this.virtualKeyLabel1);
            this.firstKeyGroup.Controls.Add(this.keyNameLabel1);
            this.firstKeyGroup.Location = new System.Drawing.Point(14, 14);
            this.firstKeyGroup.Margin = new System.Windows.Forms.Padding(5);
            this.firstKeyGroup.Name = "firstKeyGroup";
            this.firstKeyGroup.Size = new System.Drawing.Size(330, 94);
            this.firstKeyGroup.TabIndex = 0;
            this.firstKeyGroup.TabStop = false;
            this.firstKeyGroup.Text = "Press First Keyboard Key";
            this.firstKeyGroup.EnabledChanged += new System.EventHandler(this.firstKeyGroup_EnabledChanged);
            // 
            // scancodeLabel1
            // 
            this.scancodeLabel1.Location = new System.Drawing.Point(9, 67);
            this.scancodeLabel1.Margin = new System.Windows.Forms.Padding(5);
            this.scancodeLabel1.Name = "scancodeLabel1";
            this.scancodeLabel1.Size = new System.Drawing.Size(65, 13);
            this.scancodeLabel1.TabIndex = 2;
            this.scancodeLabel1.Text = "Scancode:";
            this.scancodeLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // virtualKeyLabel1
            // 
            this.virtualKeyLabel1.Location = new System.Drawing.Point(8, 44);
            this.virtualKeyLabel1.Margin = new System.Windows.Forms.Padding(5);
            this.virtualKeyLabel1.Name = "virtualKeyLabel1";
            this.virtualKeyLabel1.Size = new System.Drawing.Size(65, 13);
            this.virtualKeyLabel1.TabIndex = 1;
            this.virtualKeyLabel1.Text = "Virtual Key:";
            this.virtualKeyLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keyNameLabel1
            // 
            this.keyNameLabel1.Location = new System.Drawing.Point(8, 21);
            this.keyNameLabel1.Margin = new System.Windows.Forms.Padding(5);
            this.keyNameLabel1.Name = "keyNameLabel1";
            this.keyNameLabel1.Size = new System.Drawing.Size(65, 13);
            this.keyNameLabel1.TabIndex = 0;
            this.keyNameLabel1.Text = "Key Name:";
            this.keyNameLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // secondKeyGroup
            // 
            this.secondKeyGroup.BackColor = System.Drawing.Color.Transparent;
            this.secondKeyGroup.Controls.Add(this.scancodeLabel2);
            this.secondKeyGroup.Controls.Add(this.virtualKeyLabel2);
            this.secondKeyGroup.Controls.Add(this.keyNameLabel2);
            this.secondKeyGroup.Location = new System.Drawing.Point(14, 121);
            this.secondKeyGroup.Margin = new System.Windows.Forms.Padding(5);
            this.secondKeyGroup.Name = "secondKeyGroup";
            this.secondKeyGroup.Size = new System.Drawing.Size(330, 94);
            this.secondKeyGroup.TabIndex = 4;
            this.secondKeyGroup.TabStop = false;
            this.secondKeyGroup.Text = "Press Second Keyboard Key";
            this.secondKeyGroup.EnabledChanged += new System.EventHandler(this.secondKeyGroup_EnabledChanged);
            // 
            // scancodeLabel2
            // 
            this.scancodeLabel2.Location = new System.Drawing.Point(8, 68);
            this.scancodeLabel2.Margin = new System.Windows.Forms.Padding(5);
            this.scancodeLabel2.Name = "scancodeLabel2";
            this.scancodeLabel2.Size = new System.Drawing.Size(65, 13);
            this.scancodeLabel2.TabIndex = 2;
            this.scancodeLabel2.Text = "Scancode:";
            this.scancodeLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // virtualKeyLabel2
            // 
            this.virtualKeyLabel2.Location = new System.Drawing.Point(8, 45);
            this.virtualKeyLabel2.Margin = new System.Windows.Forms.Padding(5);
            this.virtualKeyLabel2.Name = "virtualKeyLabel2";
            this.virtualKeyLabel2.Size = new System.Drawing.Size(65, 13);
            this.virtualKeyLabel2.TabIndex = 1;
            this.virtualKeyLabel2.Text = "Virtual Key:";
            this.virtualKeyLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keyNameLabel2
            // 
            this.keyNameLabel2.Location = new System.Drawing.Point(8, 22);
            this.keyNameLabel2.Margin = new System.Windows.Forms.Padding(5);
            this.keyNameLabel2.Name = "keyNameLabel2";
            this.keyNameLabel2.Size = new System.Drawing.Size(65, 13);
            this.keyNameLabel2.TabIndex = 0;
            this.keyNameLabel2.Text = "Key Name:";
            this.keyNameLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scancodeText2
            // 
            this.scancodeText2.BackColor = System.Drawing.SystemColors.Window;
            this.scancodeText2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scancodeText2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scancodeText2.Location = new System.Drawing.Point(95, 186);
            this.scancodeText2.Margin = new System.Windows.Forms.Padding(5);
            this.scancodeText2.Name = "scancodeText2";
            this.scancodeText2.Size = new System.Drawing.Size(237, 20);
            this.scancodeText2.TabIndex = 7;
            this.scancodeText2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // virtualKeyText2
            // 
            this.virtualKeyText2.BackColor = System.Drawing.SystemColors.Window;
            this.virtualKeyText2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.virtualKeyText2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.virtualKeyText2.Location = new System.Drawing.Point(95, 163);
            this.virtualKeyText2.Margin = new System.Windows.Forms.Padding(5);
            this.virtualKeyText2.Name = "virtualKeyText2";
            this.virtualKeyText2.Size = new System.Drawing.Size(237, 20);
            this.virtualKeyText2.TabIndex = 6;
            this.virtualKeyText2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keyNameText2
            // 
            this.keyNameText2.BackColor = System.Drawing.SystemColors.Window;
            this.keyNameText2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyNameText2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyNameText2.Location = new System.Drawing.Point(95, 140);
            this.keyNameText2.Margin = new System.Windows.Forms.Padding(5);
            this.keyNameText2.Name = "keyNameText2";
            this.keyNameText2.Size = new System.Drawing.Size(237, 20);
            this.keyNameText2.TabIndex = 5;
            this.keyNameText2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scancodeText1
            // 
            this.scancodeText1.BackColor = System.Drawing.SystemColors.Window;
            this.scancodeText1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.scancodeText1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scancodeText1.Location = new System.Drawing.Point(95, 78);
            this.scancodeText1.Margin = new System.Windows.Forms.Padding(5);
            this.scancodeText1.Name = "scancodeText1";
            this.scancodeText1.Size = new System.Drawing.Size(237, 20);
            this.scancodeText1.TabIndex = 3;
            this.scancodeText1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // keyNameText1
            // 
            this.keyNameText1.BackColor = System.Drawing.SystemColors.Window;
            this.keyNameText1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keyNameText1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyNameText1.Location = new System.Drawing.Point(95, 32);
            this.keyNameText1.Margin = new System.Windows.Forms.Padding(5);
            this.keyNameText1.Name = "keyNameText1";
            this.keyNameText1.Size = new System.Drawing.Size(237, 20);
            this.keyNameText1.TabIndex = 1;
            this.keyNameText1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // virtualKeyText1
            // 
            this.virtualKeyText1.BackColor = System.Drawing.SystemColors.Window;
            this.virtualKeyText1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.virtualKeyText1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.virtualKeyText1.Location = new System.Drawing.Point(95, 55);
            this.virtualKeyText1.Margin = new System.Windows.Forms.Padding(5);
            this.virtualKeyText1.Name = "virtualKeyText1";
            this.virtualKeyText1.Size = new System.Drawing.Size(237, 20);
            this.virtualKeyText1.TabIndex = 2;
            this.virtualKeyText1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // actionButton
            // 
            this.actionButton.Location = new System.Drawing.Point(14, 225);
            this.actionButton.Margin = new System.Windows.Forms.Padding(5);
            this.actionButton.Name = "actionButton";
            this.actionButton.Size = new System.Drawing.Size(75, 23);
            this.actionButton.TabIndex = 8;
            this.actionButton.Text = "???";
            this.actionButton.UseVisualStyleBackColor = true;
            this.actionButton.Click += new System.EventHandler(this.actionButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(99, 225);
            this.applyButton.Margin = new System.Windows.Forms.Padding(5);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 9;
            this.applyButton.Text = "&Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // AdvancedKeyMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(360, 260);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.actionButton);
            this.Controls.Add(this.scancodeText2);
            this.Controls.Add(this.virtualKeyText2);
            this.Controls.Add(this.keyNameText2);
            this.Controls.Add(this.scancodeText1);
            this.Controls.Add(this.keyNameText1);
            this.Controls.Add(this.virtualKeyText1);
            this.Controls.Add(this.secondKeyGroup);
            this.Controls.Add(this.firstKeyGroup);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedKeyMapping";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Key Mapping";
            this.Load += new System.EventHandler(this.AdvancedKeyMapping_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdvancedKeyMapping_FormClosing);
            this.firstKeyGroup.ResumeLayout(false);
            this.secondKeyGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox firstKeyGroup;
        private System.Windows.Forms.GroupBox secondKeyGroup;
        private System.Windows.Forms.Label scancodeLabel1;
        private System.Windows.Forms.Label virtualKeyLabel1;
        private System.Windows.Forms.Label keyNameLabel1;
        private System.Windows.Forms.Label scancodeLabel2;
        private System.Windows.Forms.Label virtualKeyLabel2;
        private System.Windows.Forms.Label keyNameLabel2;
        private System.Windows.Forms.Label scancodeText2;
        private System.Windows.Forms.Label virtualKeyText2;
        private System.Windows.Forms.Label keyNameText2;
        private System.Windows.Forms.Label scancodeText1;
        private System.Windows.Forms.Label keyNameText1;
        private System.Windows.Forms.Label virtualKeyText1;
        private System.Windows.Forms.Button actionButton;
        private System.Windows.Forms.Button applyButton;
    }
}