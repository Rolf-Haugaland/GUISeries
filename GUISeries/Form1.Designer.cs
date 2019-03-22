﻿namespace GUISeries
{
    partial class Form1
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
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.btn_ConfirmSearch = new System.Windows.Forms.Button();
            this.lstView_SeriesFromAPI = new System.Windows.Forms.ListView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureChangeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Search
            // 
            this.txt_Search.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_Search.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_Search.Location = new System.Drawing.Point(292, 50);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(315, 22);
            this.txt_Search.TabIndex = 0;
            // 
            // btn_ConfirmSearch
            // 
            this.btn_ConfirmSearch.Location = new System.Drawing.Point(641, 47);
            this.btn_ConfirmSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ConfirmSearch.Name = "btn_ConfirmSearch";
            this.btn_ConfirmSearch.Size = new System.Drawing.Size(100, 28);
            this.btn_ConfirmSearch.TabIndex = 1;
            this.btn_ConfirmSearch.Text = "OK";
            this.btn_ConfirmSearch.UseVisualStyleBackColor = true;
            this.btn_ConfirmSearch.Click += new System.EventHandler(this.btn_ConfirmSearch_Click);
            // 
            // lstView_SeriesFromAPI
            // 
            this.lstView_SeriesFromAPI.Location = new System.Drawing.Point(292, 174);
            this.lstView_SeriesFromAPI.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstView_SeriesFromAPI.Name = "lstView_SeriesFromAPI";
            this.lstView_SeriesFromAPI.Size = new System.Drawing.Size(448, 182);
            this.lstView_SeriesFromAPI.TabIndex = 2;
            this.lstView_SeriesFromAPI.UseCompatibleStateImageBehavior = false;
            this.lstView_SeriesFromAPI.ItemActivate += new System.EventHandler(this.lstVIew_ItemActivated);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1067, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureChangeDatabaseToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // configureChangeDatabaseToolStripMenuItem
            // 
            this.configureChangeDatabaseToolStripMenuItem.Name = "configureChangeDatabaseToolStripMenuItem";
            this.configureChangeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
            this.configureChangeDatabaseToolStripMenuItem.Text = "Configure/Change Database";
            this.configureChangeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.MnStrp_ConfigureDB);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.lstView_SeriesFromAPI);
            this.Controls.Add(this.btn_ConfirmSearch);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Button btn_ConfirmSearch;
        private System.Windows.Forms.ListView lstView_SeriesFromAPI;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureChangeDatabaseToolStripMenuItem;
    }
}

