namespace GUISeries
{
    partial class LatestWatched
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.txt_Filter = new System.Windows.Forms.TextBox();
            this.lstVw_ShowHistory = new System.Windows.Forms.ListView();
            this.ShowName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EpNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SeasonNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TimeStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rdBtn_OrderByName = new System.Windows.Forms.RadioButton();
            this.rdBtn_OrderByDate = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dtPicker = new System.Windows.Forms.DateTimePicker();
            this.cmbBox_AscOrDesc = new System.Windows.Forms.ComboBox();
            this.lbl_LstItemCount = new System.Windows.Forms.Label();
            this.chkBx_DateOrAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(62, 53);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(640, 104);
            this.listBox1.TabIndex = 1;
            // 
            // txt_Filter
            // 
            this.txt_Filter.Location = new System.Drawing.Point(263, 20);
            this.txt_Filter.Margin = new System.Windows.Forms.Padding(2);
            this.txt_Filter.Name = "txt_Filter";
            this.txt_Filter.Size = new System.Drawing.Size(242, 20);
            this.txt_Filter.TabIndex = 2;
            this.txt_Filter.TextChanged += new System.EventHandler(this.txt_Filter_TextChanged);
            // 
            // lstVw_ShowHistory
            // 
            this.lstVw_ShowHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstVw_ShowHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ShowName,
            this.EpNum,
            this.SeasonNumber,
            this.TimeStamp});
            this.lstVw_ShowHistory.Location = new System.Drawing.Point(62, 209);
            this.lstVw_ShowHistory.Margin = new System.Windows.Forms.Padding(2);
            this.lstVw_ShowHistory.Name = "lstVw_ShowHistory";
            this.lstVw_ShowHistory.Size = new System.Drawing.Size(640, 198);
            this.lstVw_ShowHistory.TabIndex = 3;
            this.lstVw_ShowHistory.UseCompatibleStateImageBehavior = false;
            this.lstVw_ShowHistory.View = System.Windows.Forms.View.Details;
            // 
            // ShowName
            // 
            this.ShowName.Text = "ShowName";
            this.ShowName.Width = 167;
            // 
            // EpNum
            // 
            this.EpNum.Text = "Episode Number";
            this.EpNum.Width = 93;
            // 
            // SeasonNumber
            // 
            this.SeasonNumber.Text = "Season";
            this.SeasonNumber.Width = 49;
            // 
            // TimeStamp
            // 
            this.TimeStamp.Text = "Watched";
            this.TimeStamp.Width = 70;
            // 
            // rdBtn_OrderByName
            // 
            this.rdBtn_OrderByName.AutoSize = true;
            this.rdBtn_OrderByName.Location = new System.Drawing.Point(107, 182);
            this.rdBtn_OrderByName.Margin = new System.Windows.Forms.Padding(2);
            this.rdBtn_OrderByName.Name = "rdBtn_OrderByName";
            this.rdBtn_OrderByName.Size = new System.Drawing.Size(96, 17);
            this.rdBtn_OrderByName.TabIndex = 4;
            this.rdBtn_OrderByName.TabStop = true;
            this.rdBtn_OrderByName.Text = "Order by Name";
            this.rdBtn_OrderByName.UseVisualStyleBackColor = true;
            this.rdBtn_OrderByName.CheckedChanged += new System.EventHandler(this.rdBtn_CheckChanged);
            // 
            // rdBtn_OrderByDate
            // 
            this.rdBtn_OrderByDate.AutoSize = true;
            this.rdBtn_OrderByDate.Location = new System.Drawing.Point(206, 182);
            this.rdBtn_OrderByDate.Margin = new System.Windows.Forms.Padding(2);
            this.rdBtn_OrderByDate.Name = "rdBtn_OrderByDate";
            this.rdBtn_OrderByDate.Size = new System.Drawing.Size(91, 17);
            this.rdBtn_OrderByDate.TabIndex = 5;
            this.rdBtn_OrderByDate.TabStop = true;
            this.rdBtn_OrderByDate.Text = "Order by Date";
            this.rdBtn_OrderByDate.UseVisualStyleBackColor = true;
            this.rdBtn_OrderByDate.CheckedChanged += new System.EventHandler(this.rdBtn_CheckChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(301, 167);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(236, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "How many episodes have i watched at this date:";
            // 
            // dtPicker
            // 
            this.dtPicker.Location = new System.Drawing.Point(301, 182);
            this.dtPicker.Margin = new System.Windows.Forms.Padding(2);
            this.dtPicker.Name = "dtPicker";
            this.dtPicker.Size = new System.Drawing.Size(182, 20);
            this.dtPicker.TabIndex = 11;
            this.dtPicker.ValueChanged += new System.EventHandler(this.dtPckrValChanged);
            // 
            // cmbBox_AscOrDesc
            // 
            this.cmbBox_AscOrDesc.FormattingEnabled = true;
            this.cmbBox_AscOrDesc.Location = new System.Drawing.Point(3, 181);
            this.cmbBox_AscOrDesc.Margin = new System.Windows.Forms.Padding(2);
            this.cmbBox_AscOrDesc.Name = "cmbBox_AscOrDesc";
            this.cmbBox_AscOrDesc.Size = new System.Drawing.Size(88, 21);
            this.cmbBox_AscOrDesc.TabIndex = 12;
            this.cmbBox_AscOrDesc.SelectedIndexChanged += new System.EventHandler(this.cmBoxIndexChanged);
            // 
            // lbl_LstItemCount
            // 
            this.lbl_LstItemCount.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_LstItemCount.Location = new System.Drawing.Point(0, 409);
            this.lbl_LstItemCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_LstItemCount.Name = "lbl_LstItemCount";
            this.lbl_LstItemCount.Padding = new System.Windows.Forms.Padding(58, 0, 0, 28);
            this.lbl_LstItemCount.Size = new System.Drawing.Size(800, 41);
            this.lbl_LstItemCount.TabIndex = 13;
            this.lbl_LstItemCount.Text = "The list is currently showing x items";
            this.lbl_LstItemCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkBx_DateOrAll
            // 
            this.chkBx_DateOrAll.AutoSize = true;
            this.chkBx_DateOrAll.Location = new System.Drawing.Point(497, 185);
            this.chkBx_DateOrAll.Margin = new System.Windows.Forms.Padding(2);
            this.chkBx_DateOrAll.Name = "chkBx_DateOrAll";
            this.chkBx_DateOrAll.Size = new System.Drawing.Size(141, 17);
            this.chkBx_DateOrAll.TabIndex = 14;
            this.chkBx_DateOrAll.Text = "Show only from this date";
            this.chkBx_DateOrAll.UseVisualStyleBackColor = true;
            this.chkBx_DateOrAll.CheckedChanged += new System.EventHandler(this.chkBx_DateOrAll_CheckedChanged);
            // 
            // LatestWatched
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chkBx_DateOrAll);
            this.Controls.Add(this.lbl_LstItemCount);
            this.Controls.Add(this.cmbBox_AscOrDesc);
            this.Controls.Add(this.dtPicker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdBtn_OrderByDate);
            this.Controls.Add(this.rdBtn_OrderByName);
            this.Controls.Add(this.lstVw_ShowHistory);
            this.Controls.Add(this.txt_Filter);
            this.Controls.Add(this.listBox1);
            this.Name = "LatestWatched";
            this.Text = "LatestWatched";
            this.Load += new System.EventHandler(this.LatestWatched_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TextBox txt_Filter;
        private System.Windows.Forms.ListView lstVw_ShowHistory;
        private System.Windows.Forms.ColumnHeader ShowName;
        private System.Windows.Forms.ColumnHeader EpNum;
        private System.Windows.Forms.ColumnHeader SeasonNumber;
        private System.Windows.Forms.ColumnHeader TimeStamp;
        private System.Windows.Forms.RadioButton rdBtn_OrderByName;
        private System.Windows.Forms.RadioButton rdBtn_OrderByDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtPicker;
        private System.Windows.Forms.ComboBox cmbBox_AscOrDesc;
        private System.Windows.Forms.Label lbl_LstItemCount;
        private System.Windows.Forms.CheckBox chkBx_DateOrAll;
    }
}