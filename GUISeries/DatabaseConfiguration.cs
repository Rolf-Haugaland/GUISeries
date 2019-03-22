using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace GUISeries
{
    public partial class DatabaseConfiguration : Form
    {
        public DatabaseConfiguration()
        {
            InitializeComponent();
            Startup();
        }

        void Startup()
        {
            ConfigurationManager manager = new ConfigurationManager();

            string DefaultDatabase = manager.GetSetting("Default Database");

            //Foreach statement that fills in the information in the textbox-es if the user already has database configuration
            foreach (List<string> ls in manager.GetAllSettings())
            {
                if (("Database" + DefaultDatabase + " name").Equals(ls[0], StringComparison.CurrentCultureIgnoreCase))
                    txt_DBName.Text = ls[1];
                else if (("Database" + DefaultDatabase + " IP").Equals(ls[0], StringComparison.CurrentCultureIgnoreCase))
                    txt_DBIP.Text = ls[1];
                else if (("Database" + DefaultDatabase + " Username").Equals(ls[0], StringComparison.CurrentCultureIgnoreCase))
                    txt_DBUname.Text = ls[1];
                else if (("Database" + DefaultDatabase + " Password").Equals(ls[0], StringComparison.CurrentCultureIgnoreCase))
                {
                    txt_DBPW.Text = ls[1];
                    txt_DBPWConfirm.Text = ls[1];
                }
                else if (("Database" + DefaultDatabase + " Port").Equals(ls[0], StringComparison.CurrentCultureIgnoreCase))
                    txt_DBPort.Text = ls[1];
            }
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            //Confirms that the password textboxes match
            if(txt_DBPW.Text != txt_DBPWConfirm.Text)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            ConfigurationManager manager = new ConfigurationManager();

            string DefaultDatabase = manager.GetSetting("Default Database");

            if (!string.IsNullOrWhiteSpace(txt_DBName.Text) && manager.GetSetting("Database" + DefaultDatabase + " Name") != "")
                manager.SetSetting("Database" + DefaultDatabase + " Name", txt_DBName.Text);
            else
                manager.AddSetting("Database" + DefaultDatabase + " Name", txt_DBName.Text);

            if (!string.IsNullOrWhiteSpace(txt_DBIP.Text) && manager.GetSetting("Database" + DefaultDatabase + " IP") != "")
                manager.SetSetting("Database" + DefaultDatabase + " IP", txt_DBIP.Text);
            else
                manager.AddSetting("Database" + DefaultDatabase + " IP", txt_DBIP.Text);

            if (!string.IsNullOrWhiteSpace(txt_DBUname.Text) && manager.GetSetting("Database" + DefaultDatabase + " Username") != "")
                manager.SetSetting("Database" + DefaultDatabase + " Username", txt_DBUname.Text);
            else
                manager.AddSetting("Database" + DefaultDatabase + " Username", txt_DBUname.Text);

            if (!string.IsNullOrWhiteSpace(txt_DBPW.Text) && manager.GetSetting("Database" + DefaultDatabase + " Password") != "")
                manager.SetSetting("Database" + DefaultDatabase + " Password", txt_DBPW.Text);
            else
                manager.AddSetting("Database" + DefaultDatabase + " Password", txt_DBPW.Text);
            if (!string.IsNullOrEmpty(txt_DBPort.Text) && manager.GetSetting("Database" + DefaultDatabase + " Port") != "")
                manager.SetSetting("Database" + DefaultDatabase + " Port", txt_DBPort.Text);
            else
                manager.AddSetting("Database" + DefaultDatabase + " Port", txt_DBPort.Text);

            if (manager.CheckDatabaseFromFile())
            {
                MessageBox.Show("Succsess! A connection was succsessfully made to the database.", "Succsess");
                this.Close();
            }
            else
            {
                MessageBox.Show("Failure. Please check your internet connection and that the database is accessible, then try again", "Failure");
            }
        }
    }
}
