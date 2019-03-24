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
    public partial class SetDatabase : Form
    {
        public SetDatabase()
        {
            InitializeComponent();
            UpdateListBox();
        }

        void UpdateListBox()
        {
            lstBx_Databases.Items.Clear();
            ConfigurationManager manager = new ConfigurationManager();

            //foreach(Database database in manager.data)
        }
    }
}
