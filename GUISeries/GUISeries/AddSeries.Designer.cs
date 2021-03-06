﻿namespace GUISeries
{
    partial class AddSeries
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
            this.lbl_Heading = new System.Windows.Forms.Label();
            this.txt_TimeStamp = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txt_EpisodesWatched = new System.Windows.Forms.TextBox();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.prgrsBr_UploadProgress = new System.Windows.Forms.ProgressBar();
            this.lbl_TimeStamp = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Heading
            // 
            this.lbl_Heading.AutoSize = true;
            this.lbl_Heading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lbl_Heading.Location = new System.Drawing.Point(12, 45);
            this.lbl_Heading.Name = "lbl_Heading";
            this.lbl_Heading.Size = new System.Drawing.Size(474, 25);
            this.lbl_Heading.TabIndex = 0;
            this.lbl_Heading.Text = "\"Name of series\", which episodes have you watched?";
            // 
            // txt_TimeStamp
            // 
            this.txt_TimeStamp.Location = new System.Drawing.Point(12, 161);
            this.txt_TimeStamp.Name = "txt_TimeStamp";
            this.txt_TimeStamp.Size = new System.Drawing.Size(147, 20);
            this.txt_TimeStamp.TabIndex = 1;
            this.txt_TimeStamp.TextChanged += new System.EventHandler(this.txt_TimeStamp_TextChanged);
            this.txt_TimeStamp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_EpisodeWatched_KeyDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(268, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = "The date when you watched the episode(s). Leaving blank will result in today\'s da" +
    "te and current time.";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(586, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.MenStrp_HelpClick);
            // 
            // txt_EpisodesWatched
            // 
            this.txt_EpisodesWatched.Location = new System.Drawing.Point(283, 161);
            this.txt_EpisodesWatched.Name = "txt_EpisodesWatched";
            this.txt_EpisodesWatched.Size = new System.Drawing.Size(147, 20);
            this.txt_EpisodesWatched.TabIndex = 4;
            this.txt_EpisodesWatched.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_EpisodeWatched_KeyDown);
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Location = new System.Drawing.Point(189, 224);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_Confirm.TabIndex = 5;
            this.btn_Confirm.Text = "OK";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // prgrsBr_UploadProgress
            // 
            this.prgrsBr_UploadProgress.Location = new System.Drawing.Point(77, 195);
            this.prgrsBr_UploadProgress.Name = "prgrsBr_UploadProgress";
            this.prgrsBr_UploadProgress.Size = new System.Drawing.Size(298, 23);
            this.prgrsBr_UploadProgress.TabIndex = 6;
            this.prgrsBr_UploadProgress.Visible = false;
            // 
            // lbl_TimeStamp
            // 
            this.lbl_TimeStamp.AutoSize = true;
            this.lbl_TimeStamp.Location = new System.Drawing.Point(14, 184);
            this.lbl_TimeStamp.Name = "lbl_TimeStamp";
            this.lbl_TimeStamp.Size = new System.Drawing.Size(0, 13);
            this.lbl_TimeStamp.TabIndex = 7;
            // 
            // AddSeries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 276);
            this.Controls.Add(this.lbl_TimeStamp);
            this.Controls.Add(this.prgrsBr_UploadProgress);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.txt_EpisodesWatched);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_TimeStamp);
            this.Controls.Add(this.lbl_Heading);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AddSeries";
            this.Text = "AddSeries";
            this.Load += new System.EventHandler(this.AddSeries_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Heading;
        private System.Windows.Forms.TextBox txt_TimeStamp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TextBox txt_EpisodesWatched;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.ProgressBar prgrsBr_UploadProgress;
        private System.Windows.Forms.Label lbl_TimeStamp;
    }
}