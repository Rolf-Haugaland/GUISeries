namespace GUISeries
{
    partial class AddDatabase
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
            this.txt_DBName = new System.Windows.Forms.TextBox();
            this.txt_DBIP = new System.Windows.Forms.TextBox();
            this.txt_DBPW = new System.Windows.Forms.TextBox();
            this.txt_DBUname = new System.Windows.Forms.TextBox();
            this.txt_DBPWConfirm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Confirm = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_DBPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chckBx_DefaultDB = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_DBName
            // 
            this.txt_DBName.Location = new System.Drawing.Point(115, 60);
            this.txt_DBName.Margin = new System.Windows.Forms.Padding(2);
            this.txt_DBName.Name = "txt_DBName";
            this.txt_DBName.Size = new System.Drawing.Size(164, 20);
            this.txt_DBName.TabIndex = 0;
            // 
            // txt_DBIP
            // 
            this.txt_DBIP.Location = new System.Drawing.Point(115, 83);
            this.txt_DBIP.Margin = new System.Windows.Forms.Padding(2);
            this.txt_DBIP.Name = "txt_DBIP";
            this.txt_DBIP.Size = new System.Drawing.Size(164, 20);
            this.txt_DBIP.TabIndex = 1;
            // 
            // txt_DBPW
            // 
            this.txt_DBPW.Location = new System.Drawing.Point(115, 128);
            this.txt_DBPW.Margin = new System.Windows.Forms.Padding(2);
            this.txt_DBPW.Name = "txt_DBPW";
            this.txt_DBPW.Size = new System.Drawing.Size(164, 20);
            this.txt_DBPW.TabIndex = 3;
            this.txt_DBPW.UseSystemPasswordChar = true;
            // 
            // txt_DBUname
            // 
            this.txt_DBUname.Location = new System.Drawing.Point(115, 106);
            this.txt_DBUname.Margin = new System.Windows.Forms.Padding(2);
            this.txt_DBUname.Name = "txt_DBUname";
            this.txt_DBUname.Size = new System.Drawing.Size(164, 20);
            this.txt_DBUname.TabIndex = 2;
            // 
            // txt_DBPWConfirm
            // 
            this.txt_DBPWConfirm.Location = new System.Drawing.Point(115, 151);
            this.txt_DBPWConfirm.Margin = new System.Windows.Forms.Padding(2);
            this.txt_DBPWConfirm.Name = "txt_DBPWConfirm";
            this.txt_DBPWConfirm.Size = new System.Drawing.Size(164, 20);
            this.txt_DBPWConfirm.TabIndex = 4;
            this.txt_DBPWConfirm.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Database Navn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Database IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 128);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 152);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Confirm Password";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(111, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 40);
            this.label5.TabIndex = 11;
            this.label5.Text = "MySQL Database oppsett";
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Location = new System.Drawing.Point(164, 231);
            this.btn_Confirm.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(56, 19);
            this.btn_Confirm.TabIndex = 7;
            this.btn_Confirm.Text = "OK";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(345, 231);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(56, 19);
            this.btn_Cancel.TabIndex = 8;
            this.btn_Cancel.Text = "Avbryt";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 175);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Port";
            // 
            // txt_DBPort
            // 
            this.txt_DBPort.Location = new System.Drawing.Point(115, 174);
            this.txt_DBPort.Margin = new System.Windows.Forms.Padding(2);
            this.txt_DBPort.Name = "txt_DBPort";
            this.txt_DBPort.Size = new System.Drawing.Size(164, 20);
            this.txt_DBPort.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 199);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(163, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Set to default/standard database";
            // 
            // chckBx_DefaultDB
            // 
            this.chckBx_DefaultDB.AutoSize = true;
            this.chckBx_DefaultDB.Location = new System.Drawing.Point(178, 198);
            this.chckBx_DefaultDB.Name = "chckBx_DefaultDB";
            this.chckBx_DefaultDB.Size = new System.Drawing.Size(15, 14);
            this.chckBx_DefaultDB.TabIndex = 6;
            this.chckBx_DefaultDB.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(412, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.helpToolStripMenuItem.Text = "Default? Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.mnStrp_Help_Click);
            // 
            // AddDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 261);
            this.Controls.Add(this.chckBx_DefaultDB);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_DBPort);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_DBPWConfirm);
            this.Controls.Add(this.txt_DBPW);
            this.Controls.Add(this.txt_DBUname);
            this.Controls.Add(this.txt_DBIP);
            this.Controls.Add(this.txt_DBName);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AddDatabase";
            this.Text = "AddDatabase";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_DBName;
        private System.Windows.Forms.TextBox txt_DBIP;
        private System.Windows.Forms.TextBox txt_DBPW;
        private System.Windows.Forms.TextBox txt_DBUname;
        private System.Windows.Forms.TextBox txt_DBPWConfirm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_Confirm;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_DBPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chckBx_DefaultDB;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    }
}