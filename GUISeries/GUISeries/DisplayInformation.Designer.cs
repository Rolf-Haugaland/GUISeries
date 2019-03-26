namespace GUISeries
{
    partial class DisplayInformation
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
            this.lbl_Info = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Heading
            // 
            this.lbl_Heading.AutoSize = true;
            this.lbl_Heading.Location = new System.Drawing.Point(12, 30);
            this.lbl_Heading.Name = "lbl_Heading";
            this.lbl_Heading.Size = new System.Drawing.Size(146, 13);
            this.lbl_Heading.TabIndex = 0;
            this.lbl_Heading.Text = "Program broken, contact Rolf";
            // 
            // lbl_Info
            // 
            this.lbl_Info.AutoSize = true;
            this.lbl_Info.Location = new System.Drawing.Point(12, 77);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(146, 13);
            this.lbl_Info.TabIndex = 1;
            this.lbl_Info.Text = "Program broken, contact Rolf";
            // 
            // DisplayInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 226);
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.lbl_Heading);
            this.KeyPreview = true;
            this.Name = "DisplayInformation";
            this.Text = "DisplayInformation";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Heading;
        private System.Windows.Forms.Label lbl_Info;
    }
}