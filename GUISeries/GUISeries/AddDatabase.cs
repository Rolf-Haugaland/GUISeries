using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

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
            if(txt_DBName.Text.Contains(","))
            {
                MessageBox.Show("The database name cannot contain a comma");
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
                            manager.OverWriteDatabases(databases, StaticInfo.DatabaseConfPath);
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
            else
            {
                if(!manager.CheckIfTableExists(database))
                {
                    manager.CreateTable(database, "");
                }
                else
                {
                    if(!manager.CheckIfTableIsValid(database))
                    {
                        DialogResult OverwriteDB = MessageBox.Show("It seems like you already have a Series table in your database and it seems uncompatible with this program. Click " +
                            "yes if you want to delete it and create a new one that works with this program, click no if you want to take a backup first " +
                            "or use another database.", "Overwrite current database?", MessageBoxButtons.YesNoCancel);
                        if (OverwriteDB == DialogResult.Yes)
                        {
                            DialogResult Confirm = MessageBox.Show("Overwriting the current Series table with a fresh table...", "Are you sure?",
                                MessageBoxButtons.YesNoCancel);
                            if (Confirm == DialogResult.Yes)
                            {
                                manager.CreateTable(database, "");
                                List<Database> FuncDBToFile = new List<Database>();
                                string jsonstring = File.ReadAllText(StaticInfo.FuncDatabasesPath);
                                if (!string.IsNullOrWhiteSpace(jsonstring))
                                {
                                    FuncDBToFile = manager.GetDBFromJsonString(jsonstring);
                                }
                                FuncDBToFile.Add(database);

                                manager.OverWriteDatabases(FuncDBToFile, StaticInfo.FuncDatabasesPath);
                            }
                        }
                    }
                    else
                    {
                        List<Database> FuncDBToFile = manager.GetDBFromJsonString(File.ReadAllText(StaticInfo.FuncDatabasesPath));

                        FuncDBToFile.Add(database);

                        manager.OverWriteDatabases(FuncDBToFile, StaticInfo.FuncDatabasesPath);
                    }
                }
                DialogResult result = MessageBox.Show("Do you wish to use this database?", "Use database?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    StaticInfo.CurrentDatabase = database;
            }
        }

        private void mnStrp_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The default parameter means if the program should always prioritize this database first. That means if this database is avaiable it will always choose this one over any other. " +
                "May only have one database as default at a time.", "Default parameter help");
        }
    }
}
