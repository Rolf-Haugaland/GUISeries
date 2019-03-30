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
    public partial class DatabaseSelectRemove : Form
    {
        public DatabaseSelectRemove()
        {
            InitializeComponent();
            UpdateLstBx();
        }

        string Action;

        List<Database> databases = new List<Database>();

        private void LstBxSelectedValueChanged(object sender, EventArgs e)
        {
            ConfigurationManager manager = new ConfigurationManager();
            if(lstBx_Databases.SelectedIndex != -1)
            {
                Database database = databases[lstBx_Databases.SelectedIndex];
                if (lstBx_Databases.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Please only select one database at the time.");
                    return;
                }
                else if(lstBx_Databases.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select a database");
                    return;
                }
                if(Action == "RemoveDatabase")
                {
                    DialogResult result = MessageBox.Show("Are you sure you wish to delete the selected database? Database Name: '" + database.DatabaseName + "' " +
    "Database IP: '" + database.DatabaseIP + "' Database Port: '" + database.DatabasePort + "'", "Remove the selected database?", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        databases.Remove(database);
                        manager.OverWriteDatabases(databases);
                        if (manager.DatabaseCheckEqual(database, StaticInfo.CurrentDatabase))
                        {
                            DialogResult SetFunctional = MessageBox.Show("You just removed the database that was currently being used. Do you wish to set another functional database?", "Current " +
                                "database was removed", MessageBoxButtons.YesNo);
                            if(SetFunctional == DialogResult.Yes)
                            {
                                List<Database> FuncDatabases = manager.GetFunctionalDatabases();
                                if (FuncDatabases.Count > 0)
                                    StaticInfo.CurrentDatabase = FuncDatabases[0];
                                else
                                {
                                    StaticInfo.CurrentDatabase = null;
                                    MessageBox.Show("Did not find a functional database, please add one in Configuration>Add Database");
                                }
                            }
                            else
                            {
                                StaticInfo.CurrentDatabase = null;
                            }
                        }
                        this.Close();
                    }
                }
                else if(Action == "SetDatabase")
                {
                    DialogResult result = MessageBox.Show("Database set, database IP: " + database.DatabaseIP + ", database name: " + database.DatabaseName);
                    StaticInfo.CurrentDatabase = database;
                    this.Close();
                }
            }
        }

        public void Initialize(string InitializeAction)
        {
            if (InitializeAction == "SetDatabase")
                lbl_Heading.Text = "Please select the database you want to use.";
            Action = InitializeAction;
        }

        void UpdateLstBx()
        {
            ConfigurationManager manager = new ConfigurationManager();
            databases = manager.GetDatabases();

            lstBx_Databases.Items.Clear();
            foreach (Database db in databases)
            {
                lstBx_Databases.Items.Add(db.DatabaseName);
            }
        }
    }
}
