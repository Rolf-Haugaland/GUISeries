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
                if(Action == "RemoveDatabase")
                {
                    DialogResult result = MessageBox.Show("Are you sure you wish to delete the selected database? Database Name: '" + database.DatabaseName + "' " +
    "Database IP: '" + database.DatabaseIP + "' Database Port: '" + database.DatabasePort + "'", "Remove the selected database?", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        databases.Remove(database);
                        manager.OverWriteDatabases(databases);
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
