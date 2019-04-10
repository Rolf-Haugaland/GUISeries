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
                database.DefaultDB = true;
            else
                database.DefaultDB = false;
            if (!manager.CheckDatabaseConnection(database))
            {
                DialogResult CantConnect = MessageBox.Show("Cannot connect to this database, do you want to keep it? You will have to attempt " +
                    "to reconfigure it later. If you believe the information is right, please check your internet connection and " +
                    "try again.", "Keep this database?", MessageBoxButtons.YesNo);
                if(CantConnect == DialogResult.Yes)
                {
                    int resultInt = manager.AddDatabase(database);
                    if (resultInt == 1)
                    {
                        MessageBox.Show("This database already exists", "The database already exists");
                        return;
                    }
                    else if(resultInt == 0)
                    {
                        MessageBox.Show("Database added succsessfully, however, you may not use this database before you configure it in " +
                            "Configuration>Fix Databases");
                        this.Close();
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                if(!manager.CheckIfTableExists(database))
                {
                    CheckDefault(database);
                    manager.CreateTable(database, "");
                    manager.AddDatabase(database);
                    manager.AddFuncDBToFile(database);
                    MessageBox.Show("Database added sucsessfully", "Sucsess");
                    UseDatabase(database);
                    this.Close();
                    return;
                }
                else
                {
                    bool valid = manager.CheckIfTableIsValid(database);
                    if(valid)
                    {
                        CheckDefault(database);
                        manager.AddDatabase(database);
                        manager.AddFuncDBToFile(database);
                        MessageBox.Show("Database added sucsessfully", "Sucsess");
                        UseDatabase(database);
                        this.Close();
                        return;
                    }
                    else
                    {
                        DialogResult OverWrite = MessageBox.Show("It seems that the database provided contains a table called 'Series' that is incompatible with this program. " +
                            "You will need to backup and delete it and then try again. Or this program could overwrite it for you, clicking yes will make " +
                            "the program overwrite the current 'Series' table in the database " + database.DatabaseName,"Overwrite Series table?", 
                            MessageBoxButtons.YesNoCancel);
                        if(OverWrite == DialogResult.Yes)
                        {
                            DialogResult Confirm = MessageBox.Show("Clicking yes will delete ALL data inside of table 'Series' in database " + database.DatabaseName + " " +
                                "and make it compatible with this program, proceed?", "Are you sure?", MessageBoxButtons.YesNoCancel);
                            if(Confirm == DialogResult.Yes)
                            {
                                CheckDefault(database);
                                manager.CreateTable(database, "");
                                manager.AddDatabase(database);
                                manager.AddFuncDBToFile(database);
                                MessageBox.Show("Sucsessfully added the database", "Sucsess!");
                                UseDatabase(database);
                                this.Close();
                                return;
                            }
                        }
                    }
                }
            }

        }

        void UseDatabase(Database database)
        {
            DialogResult result = MessageBox.Show("Do you wish to use this database now?", "Use the database", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            {
                StaticInfo.CurrentDatabase = database;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        void CheckDefault(Database database)
        {
            ConfigurationManager manager = new ConfigurationManager();
            List<Database> databases = manager.GetAllInFunctionalDBFile();
            Database DefaultDB = databases.Find(x => x.DefaultDB);
            if(DefaultDB != null)
            {
                DialogResult result = MessageBox.Show("Det ser ut til at en annen standard/default database allerede finnes, vil du gjøre denne til den nye standar " +
                    "databasen", "Sette denne til default database?", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    manager.SetDefaultDB(database);
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
