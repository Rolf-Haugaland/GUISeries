namespace GUISeries
{
    partial class DatabaseConfiguration
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
            this.SuspendLayout();
            // 
            // txt_DBName
            // 
            this.txt_DBName.Location = new System.Drawing.Point(153, 74);
            this.txt_DBName.Name = "txt_DBName";
            this.txt_DBName.Size = new System.Drawing.Size(218, 22);
            this.txt_DBName.TabIndex = 0;
            // 
            // txt_DBIP
            // 
            this.txt_DBIP.Location = new System.Drawing.Point(153, 102);
            this.txt_DBIP.Name = "txt_DBIP";
            this.txt_DBIP.Size = new System.Drawing.Size(218, 22);
            this.txt_DBIP.TabIndex = 1;
            // 
            // txt_DBPW
            // 
            this.txt_DBPW.Location = new System.Drawing.Point(153, 158);
            this.txt_DBPW.Name = "txt_DBPW";
            this.txt_DBPW.Size = new System.Drawing.Size(218, 22);
            this.txt_DBPW.TabIndex = 3;
            this.txt_DBPW.UseSystemPasswordChar = true;
            // 
            // txt_DBUname
            // 
            this.txt_DBUname.Location = new System.Drawing.Point(153, 130);
            this.txt_DBUname.Name = "txt_DBUname";
            this.txt_DBUname.Size = new System.Drawing.Size(218, 22);
            this.txt_DBUname.TabIndex = 2;
            // 
            // txt_DBPWConfirm
            // 
            this.txt_DBPWConfirm.Location = new System.Drawing.Point(153, 186);
            this.txt_DBPWConfirm.Name = "txt_DBPWConfirm";
            this.txt_DBPWConfirm.Size = new System.Drawing.Size(218, 22);
            this.txt_DBPWConfirm.TabIndex = 4;
            this.txt_DBPWConfirm.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Database Navn";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Database IP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Username";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 187);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "Confirm Password";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label5.Location = new System.Drawing.Point(129, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(242, 49);
            this.label5.TabIndex = 11;
            this.label5.Text = "MySQL Database oppsett";
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Location = new System.Drawing.Point(222, 245);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_Confirm.TabIndex = 12;
            this.btn_Confirm.Text = "OK";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(463, 272);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 13;
            this.btn_Cancel.Text = "Avbryt";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Port";
            // 
            // txt_DBPort
            // 
            this.txt_DBPort.Location = new System.Drawing.Point(153, 214);
            this.txt_DBPort.Name = "txt_DBPort";
            this.txt_DBPort.Size = new System.Drawing.Size(218, 22);
            this.txt_DBPort.TabIndex = 14;
            // 
            // DatabaseConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 307);
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
            this.Name = "DatabaseConfiguration";
            this.Text = "DatabaseConfiguration";
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
    }
}