namespace ImageCrawler
{
    partial class AboutBoxImageCrawler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBoxImageCrawler));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.richTextBoxLicence = new System.Windows.Forms.RichTextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelProductName = new System.Windows.Forms.Label();
            this.labelCopyright = new System.Windows.Forms.Label();
            this.linkLabelHomepage = new System.Windows.Forms.LinkLabel();
            this.listBoxLicenceHint = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.buttonOk, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.labelVersion, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelProductName, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.richTextBoxLicence, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.labelCopyright, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.linkLabelHomepage, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.listBoxLicenceHint, 0, 4);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 133F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(417, 458);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Location = new System.Drawing.Point(3, 431);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(411, 24);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // richTextBoxLicence
            // 
            this.richTextBoxLicence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxLicence.Location = new System.Drawing.Point(3, 216);
            this.richTextBoxLicence.Name = "richTextBoxLicence";
            this.richTextBoxLicence.ReadOnly = true;
            this.richTextBoxLicence.Size = new System.Drawing.Size(411, 209);
            this.richTextBoxLicence.TabIndex = 1;
            this.richTextBoxLicence.Text = resources.GetString("richTextBoxLicence.Text");
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelVersion.Location = new System.Drawing.Point(3, 20);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(411, 13);
            this.labelVersion.TabIndex = 2;
            this.labelVersion.Text = "Version";
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelProductName.Location = new System.Drawing.Point(3, 0);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(411, 13);
            this.labelProductName.TabIndex = 3;
            this.labelProductName.Text = "Product";
            // 
            // labelCopyright
            // 
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCopyright.Location = new System.Drawing.Point(3, 40);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(411, 13);
            this.labelCopyright.TabIndex = 4;
            this.labelCopyright.Text = "Copyright";
            // 
            // linkLabelHomepage
            // 
            this.linkLabelHomepage.AutoSize = true;
            this.linkLabelHomepage.Dock = System.Windows.Forms.DockStyle.Top;
            this.linkLabelHomepage.Location = new System.Drawing.Point(3, 60);
            this.linkLabelHomepage.Name = "linkLabelHomepage";
            this.linkLabelHomepage.Size = new System.Drawing.Size(411, 13);
            this.linkLabelHomepage.TabIndex = 5;
            this.linkLabelHomepage.TabStop = true;
            this.linkLabelHomepage.Text = "Homepage: https://sourceforge.net/projects/imagecrawler/";
            this.linkLabelHomepage.Click += new System.EventHandler(this.linkLabelHomepage_Click);
            // 
            // listBoxLicenceHint
            // 
            this.listBoxLicenceHint.BackColor = System.Drawing.SystemColors.Menu;
            this.listBoxLicenceHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLicenceHint.Enabled = false;
            this.listBoxLicenceHint.FormattingEnabled = true;
            this.listBoxLicenceHint.Items.AddRange(new object[] {
            "This program is free software: you can redistribute it and/or modify",
            "    it under the terms of the GNU General Public License as published by",
            "    the Free Software Foundation, either version 3 of the License, or",
            "    (at your option) any later version.",
            "",
            "    This program is distributed in the hope that it will be useful,",
            "    but WITHOUT ANY WARRANTY; without even the implied warranty of",
            "    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the",
            "    GNU General Public License for more details."});
            this.listBoxLicenceHint.Location = new System.Drawing.Point(3, 83);
            this.listBoxLicenceHint.Name = "listBoxLicenceHint";
            this.listBoxLicenceHint.Size = new System.Drawing.Size(411, 127);
            this.listBoxLicenceHint.TabIndex = 6;
            // 
            // AboutBoxImageCrawler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 476);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBoxImageCrawler";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Image Crawler";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.RichTextBox richTextBoxLicence;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.Label labelCopyright;
        private System.Windows.Forms.LinkLabel linkLabelHomepage;
        private System.Windows.Forms.ListBox listBoxLicenceHint;

    }
}
