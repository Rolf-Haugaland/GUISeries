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
            // 
            // btn_ConfirmSearch
            // 
            this.btn_ConfirmSearch.Location = new System.Drawing.Point(481, 38);
            this.btn_ConfirmSearch.Name = "btn_ConfirmSearch";
            this.btn_ConfirmSearch.Size = new System.Drawing.Size(75, 23);
            this.btn_ConfirmSearch.TabIndex = 1;
            this.btn_ConfirmSearch.Text = "OK";
            this.btn_ConfirmSearch.UseVisualStyleBackColor = true;
            // 
            // lstView_SeriesFromAPI
            // 
            this.lstView_SeriesFromAPI.Location = new System.Drawing.Point(255, 172);
            this.lstView_SeriesFromAPI.Name = "lstView_SeriesFromAPI";
            this.lstView_SeriesFromAPI.Size = new System.Drawing.Size(279, 149);
            this.lstView_SeriesFromAPI.TabIndex = 2;
            this.lstView_SeriesFromAPI.UseCompatibleStateImageBehavior = false;
            this.lstView_SeriesFromAPI.ItemActivate += new System.EventHandler(this.lstVIew_ItemClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstView_SeriesFromAPI);
            this.Controls.Add(this.btn_ConfirmSearch);
            this.Controls.Add(this.txt_Search);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Button btn_ConfirmSearch;
        private System.Windows.Forms.ListView lstView_SeriesFromAPI;
    }
}

