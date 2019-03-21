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
    public partial class DisplayInformation : Form
    {
        public DisplayInformation()
        {
            InitializeComponent();
        }

        public void setup(string Action)
        {
            if(Action == "AddSeries.Help")
            {
                lbl_Heading.Text = "Help for Adding series window";
                lbl_Info.Text = "Date and timestamp for when you watched the series can be left blank." + Environment.NewLine
                    + " It will then be automatically set as current time and date. " + Environment.NewLine + " If you type in a timestamp alone it will " +
                    "be set to the current date(or the date you set in your settings file). " + Environment.NewLine + " Typing in a date only will take that date " +
                    "and set it to current time. " + Environment.NewLine + "The format has to be like this: 01.01.2001 23:59 " + Environment.NewLine +
                    "The clock format is 24 hour and the date comes before the month.";
            }
        }

        private void Frm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.W)
                this.Close();
        }
    }
}
