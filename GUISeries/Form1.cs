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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> GetSerieNames()
        {
            return new List<string>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateTextBoxAutoComplete();
        }

        void UpdateTextBoxAutoComplete()
        {
            //#1 start: Adds the autocompletion options to the textbox. They are taken from the GetSerieNames method which gets it from 
            //the database
            AutoCompleteStringCollection COLSeries = new AutoCompleteStringCollection();
            List<string> serieNames = GetSerieNames();

            foreach (string name in serieNames)
            {
                COLSeries.Add(name);
            }

            txt_Search.AutoCompleteCustomSource = COLSeries;
            //#1 end.
        }


        private void lstVIew_ItemClicked(object sender, EventArgs e)
        {
            //When i make the api request i will put the serie names in the list view, here is one way of doing so, you take the strings 
            //and put them into the listview1. Then when the user clicks one you check which one is selected and get the name from there.

            //ListViewItem item = new ListViewItem();
            //item.Name = "test";

            //listView1.Items.Add("asd", "abd", "aad");
            //listView1.Items.Add("you", "you", "you");


            //This is needed incase the user tries to select multiple series and press ENTER, you can only upload one serie at a time. 
            if (lstView_SeriesFromAPI.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vennligst bare velg en ting du vil laste opp");
                return;
            }

            MessageBox.Show(lstView_SeriesFromAPI.SelectedItems[0].Name);

        }
    }
}
