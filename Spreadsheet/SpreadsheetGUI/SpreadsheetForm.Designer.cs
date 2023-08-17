/*
 * Cristian Tapieroo & Kevin Pineda
 * u1367608 & u1342770
 * CS 3500 Software Practice I
 * Fall 2021
 * PS6
 */

namespace SpreadsheetGUI
{
    partial class SpreadsheetForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuEmptySpreadsheet = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.EvalButton = new System.Windows.Forms.Button();
            this.ContentsBox = new System.Windows.Forms.TextBox();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.SpreadsheetPanel = new SS.SpreadsheetPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(952, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuNew,
            this.MenuOpen,
            this.MenuSave,
            this.MenuClose});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(37, 20);
            this.MenuFile.Text = "File";
            // 
            // MenuNew
            // 
            this.MenuNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuEmptySpreadsheet});
            this.MenuNew.Name = "MenuNew";
            this.MenuNew.Size = new System.Drawing.Size(103, 22);
            this.MenuNew.Text = "New";
            // 
            // MenuEmptySpreadsheet
            // 
            this.MenuEmptySpreadsheet.Name = "MenuEmptySpreadsheet";
            this.MenuEmptySpreadsheet.Size = new System.Drawing.Size(175, 22);
            this.MenuEmptySpreadsheet.Text = "Empty Spreadsheet";
            this.MenuEmptySpreadsheet.Click += new System.EventHandler(this.MenuEmptySpreadsheet_Click);
            // 
            // MenuOpen
            // 
            this.MenuOpen.Name = "MenuOpen";
            this.MenuOpen.Size = new System.Drawing.Size(103, 22);
            this.MenuOpen.Text = "Open";
            this.MenuOpen.Click += new System.EventHandler(this.MenuOpen_Click);
            // 
            // MenuSave
            // 
            this.MenuSave.Name = "MenuSave";
            this.MenuSave.Size = new System.Drawing.Size(103, 22);
            this.MenuSave.Text = "Save";
            this.MenuSave.Click += new System.EventHandler(this.MenuSave_Click);
            // 
            // MenuClose
            // 
            this.MenuClose.Name = "MenuClose";
            this.MenuClose.Size = new System.Drawing.Size(103, 22);
            this.MenuClose.Text = "Close";
            this.MenuClose.Click += new System.EventHandler(this.MenuClose_Click);
            // 
            // MenuHelp
            // 
            this.MenuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAbout});
            this.MenuHelp.Name = "MenuHelp";
            this.MenuHelp.Size = new System.Drawing.Size(44, 20);
            this.MenuHelp.Text = "Help";
            // 
            // MenuAbout
            // 
            this.MenuAbout.Name = "MenuAbout";
            this.MenuAbout.Size = new System.Drawing.Size(109, 22);
            this.MenuAbout.Text = "Details";
            this.MenuAbout.Click += new System.EventHandler(this.MenuAbout_Click);
            // 
            // EvalButton
            // 
            this.EvalButton.Location = new System.Drawing.Point(498, 30);
            this.EvalButton.Name = "EvalButton";
            this.EvalButton.Size = new System.Drawing.Size(72, 22);
            this.EvalButton.TabIndex = 2;
            this.EvalButton.Text = "Evaluate";
            this.EvalButton.UseVisualStyleBackColor = true;
            this.EvalButton.Click += new System.EventHandler(this.EvalButton_Click);
            // 
            // ContentsBox
            // 
            this.ContentsBox.Location = new System.Drawing.Point(300, 30);
            this.ContentsBox.Name = "ContentsBox";
            this.ContentsBox.Size = new System.Drawing.Size(192, 20);
            this.ContentsBox.TabIndex = 3;
            // 
            // ValueBox
            // 
            this.ValueBox.Location = new System.Drawing.Point(142, 29);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.ReadOnly = true;
            this.ValueBox.Size = new System.Drawing.Size(100, 20);
            this.ValueBox.TabIndex = 4;
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(44, 29);
            this.NameBox.Name = "NameBox";
            this.NameBox.ReadOnly = true;
            this.NameBox.Size = new System.Drawing.Size(50, 20);
            this.NameBox.TabIndex = 5;
            // 
            // SpreadsheetPanel
            // 
            this.SpreadsheetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpreadsheetPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SpreadsheetPanel.Location = new System.Drawing.Point(-1, 58);
            this.SpreadsheetPanel.Name = "SpreadsheetPanel";
            this.SpreadsheetPanel.Size = new System.Drawing.Size(954, 627);
            this.SpreadsheetPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "value:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(248, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "content:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "cell:";
            // 
            // SpreadsheetForm
            // 
            this.ClientSize = new System.Drawing.Size(952, 684);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.ContentsBox);
            this.Controls.Add(this.EvalButton);
            this.Controls.Add(this.SpreadsheetPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadsheetForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpreadsheetForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel SpreadsheetPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuNew;
        private System.Windows.Forms.ToolStripMenuItem MenuEmptySpreadsheet;
        private System.Windows.Forms.ToolStripMenuItem MenuOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuSave;
        private System.Windows.Forms.ToolStripMenuItem MenuClose;
        private System.Windows.Forms.ToolStripMenuItem MenuHelp;
        private System.Windows.Forms.ToolStripMenuItem MenuAbout;
        private System.Windows.Forms.Button EvalButton;
        private System.Windows.Forms.TextBox ContentsBox;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

