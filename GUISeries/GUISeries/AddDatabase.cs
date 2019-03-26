using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Net;

namespace GUISeries
{
    public partial class AddDatabase : Form
    {
        public AddDatabase()
        {
            InitializeComponent();
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            //Confirms that the password textboxes match
            if(txt_DBPW.Text != txt_DBPWConfirm.Text)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            if (!int.TryParse(txt_DBPort.Text, out int NotUsed))
            {
                MessageBox.Show("Please type a valid port", "Invalid port");
                return;
            }

            ConfigurationManager manager = new ConfigurationManager();
            Database database = new Database()
            {
                DatabaseName = txt_DBName.Text,
                DatabaseIP = txt_DBIP.Text,
                DatabasePort = txt_DBPort.Text,
                DatabasePW = txt_DBPW.Text,
                DatabaseUname = txt_DBUname.Text
            };

            if (chckBx_DefaultDB.Checked)
            {
                database.DefaultDB = true;
                List<Database> databases = manager.GetDatabases();
                if(databases.Count > 0)
                {
                    int Index = databases.FindIndex(x => x.DefaultDB == true);
                    if(Index != -1)
                    {
                        DialogResult ChangeDefaultDB = MessageBox.Show("A default database already exists, do you want to make this database the new default database?", "Change default database?", MessageBoxButtons.YesNo);
                        if(ChangeDefaultDB == DialogResult.Yes)
                        {
                            databases[Index].DefaultDB = false;
                            databases.Add(database);
                            manager.OverWriteDatabases(databases);
                            ConfirmDatabase(database);
                            this.Close();
                            return;//return since this.close does not stop method execution
                        }
                    }
                }
            }
            else
                database.DefaultDB = false;

            int result = manager.AddDatabase(database);
            
            if (result == 1)
            {
                MessageBox.Show("This database already exists");
                return;
            }
            else if (result == 0)
            {
                ConfirmDatabase(database);
                /*this.*/Close();
                return;
            }
        }

        /// <summary>
        /// I need to check if the database is valid if another default database exists and if it doesent. So instead of coding this twice, i just made a function for it. 
        /// I will probably restructure this later so that it makes more sense. Which is why im setting a bookmark here
        /// </summary>
        /// <param name="database"></param>
        void ConfirmDatabase(Database database)
        {
            ConfigurationManager manager = new ConfigurationManager();
            MessageBox.Show("Database added succsessfully", "Succsess");
            bool Connectable = manager.CheckDatabaseConnection(database);
            if (!Connectable)
            {
                DialogResult ConnectableResult = MessageBox.Show("Cannot connect to the database, do you wish to keep the database?", "Unable to connect to the database", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (ConnectableResult == DialogResult.No)
                {
                    manager.RemoveDatabase(database);
                    MessageBox.Show("Database removed succsessfully");
                }
            }
        }

        private void mnStrp_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The default parameter means if the program should always prioritize this database first. That means if this database is avaiable it will always choose this one over any other. " +
                "May only have one database as default at a time.", "Default parameter help");
        }
    }
}
