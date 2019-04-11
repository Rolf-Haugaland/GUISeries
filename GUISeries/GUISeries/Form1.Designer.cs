namespace GUISeries
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
            this.removeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDefaultDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.latestEpisodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lbl_CurrentDatabase = new System.Windows.Forms.Label();
            this.lstVw_UploadSuggestions = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.mnStrp_RepairDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_Search
            // 
            this.txt_Search.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_Search.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_Search.Location = new System.Drawing.Point(219, 41);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(237, 20);
            this.txt_Search.TabIndex = 0;
            this.txt_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Search_KeyDown);
            // 
            // btn_ConfirmSearch
            // 
            this.btn_ConfirmSearch.Location = new System.Drawing.Point(481, 38);
            this.btn_ConfirmSearch.Name = "btn_ConfirmSearch";
            this.btn_ConfirmSearch.Size = new System.Drawing.Size(75, 23);
            this.btn_ConfirmSearch.TabIndex = 1;
            this.btn_ConfirmSearch.Text = "OK";
            this.btn_ConfirmSearch.UseVisualStyleBackColor = true;
            this.btn_ConfirmSearch.Click += new System.EventHandler(this.btn_ConfirmSearch_Click);
            // 
            // lstView_SeriesFromAPI
            // 
            this.lstView_SeriesFromAPI.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.lstView_SeriesFromAPI.Location = new System.Drawing.Point(152, 101);
            this.lstView_SeriesFromAPI.Name = "lstView_SeriesFromAPI";
            this.lstView_SeriesFromAPI.Size = new System.Drawing.Size(451, 189);
            this.lstView_SeriesFromAPI.TabIndex = 2;
            this.lstView_SeriesFromAPI.UseCompatibleStateImageBehavior = false;
            this.lstView_SeriesFromAPI.View = System.Windows.Forms.View.Tile;
            this.lstView_SeriesFromAPI.ItemActivate += new System.EventHandler(this.lstVIew_ItemActivated);
            this.lstView_SeriesFromAPI.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GeneralKeyDown);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.myDataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureChangeDatabaseToolStripMenuItem,
            this.removeDatabaseToolStripMenuItem,
            this.setDatabaseToolStripMenuItem,
            this.changeDefaultDatabaseToolStripMenuItem,
            this.mnStrp_RepairDB});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configurationToolStripMenuItem.Text = "Configuration";
            // 
            // configureChangeDatabaseToolStripMenuItem
            // 
            this.configureChangeDatabaseToolStripMenuItem.Name = "configureChangeDatabaseToolStripMenuItem";
            this.configureChangeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.configureChangeDatabaseToolStripMenuItem.Text = "Add database";
            this.configureChangeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.MnStrp_ConfigureDB);
            // 
            // removeDatabaseToolStripMenuItem
            // 
            this.removeDatabaseToolStripMenuItem.Name = "removeDatabaseToolStripMenuItem";
            this.removeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.removeDatabaseToolStripMenuItem.Text = "Remove database";
            this.removeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.MnStrp_RemoveDB);
            // 
            // setDatabaseToolStripMenuItem
            // 
            this.setDatabaseToolStripMenuItem.Name = "setDatabaseToolStripMenuItem";
            this.setDatabaseToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.setDatabaseToolStripMenuItem.Text = "Set database";
            this.setDatabaseToolStripMenuItem.Click += new System.EventHandler(this.mnStrp_SetDB);
            // 
            // changeDefaultDatabaseToolStripMenuItem
            // 
            this.changeDefaultDatabaseToolStripMenuItem.Name = "changeDefaultDatabaseToolStripMenuItem";
            this.changeDefaultDatabaseToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.changeDefaultDatabaseToolStripMenuItem.Text = "Change default database";
            this.changeDefaultDatabaseToolStripMenuItem.Click += new System.EventHandler(this.ChangeDefaultDB_Click);
            // 
            // myDataToolStripMenuItem
            // 
            this.myDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.latestEpisodeToolStripMenuItem});
            this.myDataToolStripMenuItem.Name = "myDataToolStripMenuItem";
            this.myDataToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.myDataToolStripMenuItem.Text = "My data";
            // 
            // latestEpisodeToolStripMenuItem
            // 
            this.latestEpisodeToolStripMenuItem.Name = "latestEpisodeToolStripMenuItem";
            this.latestEpisodeToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.latestEpisodeToolStripMenuItem.Text = "Latest episode";
            this.latestEpisodeToolStripMenuItem.Click += new System.EventHandler(this.mnStrp_LatestEpsWatched_Click);
            // 
            // lbl_CurrentDatabase
            // 
            this.lbl_CurrentDatabase.AutoSize = true;
            this.lbl_CurrentDatabase.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lbl_CurrentDatabase.Location = new System.Drawing.Point(12, 424);
            this.lbl_CurrentDatabase.Name = "lbl_CurrentDatabase";
            this.lbl_CurrentDatabase.Size = new System.Drawing.Size(118, 17);
            this.lbl_CurrentDatabase.TabIndex = 4;
            this.lbl_CurrentDatabase.Text = "Current datbase: ";
            // 
            // lstVw_UploadSuggestions
            // 
            this.lstVw_UploadSuggestions.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.lstVw_UploadSuggestions.Location = new System.Drawing.Point(236, 332);
            this.lstVw_UploadSuggestions.Name = "lstVw_UploadSuggestions";
            this.lstVw_UploadSuggestions.Size = new System.Drawing.Size(291, 109);
            this.lstVw_UploadSuggestions.TabIndex = 5;
            this.lstVw_UploadSuggestions.UseCompatibleStateImageBehavior = false;
            this.lstVw_UploadSuggestions.View = System.Windows.Forms.View.Tile;
            this.lstVw_UploadSuggestions.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GeneralKeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(331, 316);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(428, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Upload suggestions: (Have like naruto episode 5 here, if the user has watched epi" +
    "sode 4)";
            // 
            // mnStrp_RepairDB
            // 
            this.mnStrp_RepairDB.Name = "mnStrp_RepairDB";
            this.mnStrp_RepairDB.Size = new System.Drawing.Size(205, 22);
            this.mnStrp_RepairDB.Text = "Configure database";
            this.mnStrp_RepairDB.Click += new System.EventHandler(this.mnStrp_RepairDB_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstVw_UploadSuggestions);
            this.Controls.Add(this.lbl_CurrentDatabase);
            this.Controls.Add(this.lstView_SeriesFromAPI);
            this.Controls.Add(this.btn_ConfirmSearch);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GeneralKeyDown);
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
        private System.Windows.Forms.ToolStripMenuItem removeDatabaseToolStripMenuItem;
        private System.Windows.Forms.Label lbl_CurrentDatabase;
        private System.Windows.Forms.ToolStripMenuItem setDatabaseToolStripMenuItem;
        private System.Windows.Forms.ListView lstVw_UploadSuggestions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem changeDefaultDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem latestEpisodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnStrp_RepairDB;
    }
}

