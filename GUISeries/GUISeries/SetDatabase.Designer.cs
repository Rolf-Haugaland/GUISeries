namespace GUISeries
{
    partial class SetDatabase
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
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstBx_Databases
            // 
            this.lstBx_Databases.FormattingEnabled = true;
            this.lstBx_Databases.Location = new System.Drawing.Point(46, 55);
            this.lstBx_Databases.Name = "lstBx_Databases";
            this.lstBx_Databases.Size = new System.Drawing.Size(193, 173);
            this.lstBx_Databases.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(42, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 43);
            this.label1.TabIndex = 1;
            this.label1.Text = "Here are the currently avaiable connectable databases";
            // 
            // SetDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 284);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstBx_Databases);
            this.Name = "SetDatabase";
            this.Text = "SetDatabase";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstBx_Databases;
        private System.Windows.Forms.Label label1;
    }
}