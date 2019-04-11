namespace GUISeries
{
    partial class RepairDB
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
            this.lstBx1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lstBx1
            // 
            this.lstBx1.FormattingEnabled = true;
            this.lstBx1.Location = new System.Drawing.Point(225, 93);
            this.lstBx1.Name = "lstBx1";
            this.lstBx1.Size = new System.Drawing.Size(327, 277);
            this.lstBx1.TabIndex = 0;
            this.lstBx1.SelectedValueChanged += new System.EventHandler(this.lstBx_SelectValChanged);
            // 
            // RepairDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lstBx1);
            this.Name = "RepairDB";
            this.Text = "RepairDB";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstBx1;
    }
}