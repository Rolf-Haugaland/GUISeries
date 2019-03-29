namespace GUISeries
{
    partial class DatabaseSelectRemove
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
            this.lstBx_Databases = new System.Windows.Forms.ListBox();
            this.lbl_Heading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstBx_Databases
            // 
            this.lstBx_Databases.FormattingEnabled = true;
            this.lstBx_Databases.Location = new System.Drawing.Point(195, 112);
            this.lstBx_Databases.Name = "lstBx_Databases";
            this.lstBx_Databases.Size = new System.Drawing.Size(383, 316);
            this.lstBx_Databases.TabIndex = 0;
            this.lstBx_Databases.SelectedValueChanged += new System.EventHandler(this.LstBxSelectedValueChanged);
            // 
            // lbl_Heading
            // 
            this.lbl_Heading.AutoSize = true;
            this.lbl_Heading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.lbl_Heading.Location = new System.Drawing.Point(190, 45);
            this.lbl_Heading.Name = "lbl_Heading";
            this.lbl_Heading.Size = new System.Drawing.Size(359, 25);
            this.lbl_Heading.TabIndex = 1;
            this.lbl_Heading.Text = "Select the database you want to remove";
            // 
            // RemoveDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbl_Heading);
            this.Controls.Add(this.lstBx_Databases);
            this.Name = "RemoveDatabase";
            this.Text = "RemoveDatabase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstBx_Databases;
        private System.Windows.Forms.Label lbl_Heading;
    }
}