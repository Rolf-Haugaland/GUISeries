using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUISeries
{
    public partial class RepairDB : Form
    {
        public RepairDB()
        {
            InitializeComponent();
            databases = manager.GetDBFromFile(StaticInfo.NonFuncDatabasesPath);
            PutItemsInLstBx();
        }
        List<Database> databases = new List<Database>();
        ConfigurationManager manager = new ConfigurationManager();
        void PutItemsInLstBx()
        {
            lstBx1.Items.Clear();
            foreach(Database db in databases)
            {
                lstBx1.Items.Add(db.DatabaseName);
            }
        }

        private void lstBx_SelectValChanged(object sender, EventArgs e)
        {
            if(lstBx1.SelectedItems.Count > 1)
            {
                MessageBox.Show("Please only select one item");
                return;
            }
            else if(lstBx1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an items");
                return;
            }
            Database removeme = databases[lstBx1.SelectedIndex];
            DialogResult Connect = MessageBox.Show("Attempt to make a connection to the current database?" + " Database name: " + removeme.DatabaseName + " database ip: " + removeme.DatabaseIP +
                " database port: " + removeme.DatabasePort, "Connect to this database?", MessageBoxButtons.YesNo);
            if(Connect == DialogResult.Yes)
            {
                if(manager.CheckDatabaseConnection(removeme))
                {
                    if (manager.CheckIfTableExists(removeme))
                    {
                        if (manager.CheckIfTableIsValid(removeme))
                        {
                            MessageBox.Show("Sucsess! The database is connectable and compatible. Adding it to functional databases...", "Sucsess!");
                            manager.RemoveDatabase(removeme, StaticInfo.NonFuncDatabasesPath);
                            manager.AddFuncDBToFile(removeme);
                        }
                        else
                        {
                            DialogResult overwrite = MessageBox.Show("It seems that the database provided contains a table called 'Series' that is incompatible with this program. " +
                            "You will need to backup and delete it and then try again. Or this program could overwrite it for you, clicking yes will make " +
                            "the program overwrite the current 'Series' table in the database " + removeme.DatabaseName, "Overwrite Series table?", MessageBoxButtons.YesNoCancel);
                            if (overwrite == DialogResult.Yes)
                            {
                                DialogResult Confirm = MessageBox.Show("Clicking yes will delete ALL data inside of table 'Series' in database " + removeme.DatabaseName + " " +
                                "and make it compatible with this program, proceed?", "Are you sure?", MessageBoxButtons.YesNoCancel);
                                if (Confirm == DialogResult.Yes)
                                {
                                    manager.CreateTable(removeme, "");
                                    manager.RemoveDatabase(removeme, StaticInfo.NonFuncDatabasesPath);
                                    manager.AddFuncDBToFile(removeme);
                                }
                            }
                        }
                    }
                    else
                    {
                        manager.CreateTable(removeme, "");
                        manager.RemoveDatabase(removeme, StaticInfo.NonFuncDatabasesPath);
                        manager.AddFuncDBToFile(removeme);
                    }
                }
                else
                {
                    DialogResult removeDB = MessageBox.Show("Unable to connect to the database, remove it?", "Unable to connect, remove?", MessageBoxButtons.YesNo);
                    if(removeDB == DialogResult.Yes)
                    {
                        manager.RemoveDatabase(removeme, StaticInfo.DatabaseConfPath);
                        manager.RemoveDatabase(removeme, StaticInfo.NonFuncDatabasesPath);
                        this.Close();
                        return;
                    }
                }
            }
        }
    }
}
