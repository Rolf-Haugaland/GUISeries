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
    public partial class RemoveDatabase : Form
    {
        public RemoveDatabase()
        {
            InitializeComponent();
            UpdateLstBx();
        }

        List<Database> databases = new List<Database>();

        private void LstBxSelectedValueChanged(object sender, EventArgs e)
        {
            ConfigurationManager manager = new ConfigurationManager();
            if(lstBx_Databases.SelectedIndex != -1)
            {
                Database remove = databases[lstBx_Databases.SelectedIndex];
                if (lstBx_Databases.SelectedItems.Count > 1)
                {
                    MessageBox.Show("Please only select one database to remove at the time.");
                    return;
                }
                DialogResult result = MessageBox.Show("Are you sure you wish to delete the selected database? Database Name: '" + remove.DatabaseName + "' " +
                    "Database IP: '" + remove.DatabaseIP + "' Database Port: '" + remove.DatabasePort + "'", "Remove the selected database?", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    databases.Remove(remove);
                    manager.OverWriteDatabases(databases);
                    UpdateLstBx();
                }
            }
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
