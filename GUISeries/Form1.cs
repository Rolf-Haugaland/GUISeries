using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace GUISeries
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartupCheck();
        }

        void StartupCheck()
        {
            if (!File.Exists("Settings.txt"))
                File.Create("Settings.txt").Close();
            if (CheckConfiguration())
            {
                DialogResult result = MessageBox.Show("Det ser ikke ut at du har lagt til en database enda, vil du gjøre det nå?", "Konfigurere database?", MessageBoxButtons.YesNo);
                if(result == DialogResult.Yes)
                {
                    DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration();
                    databaseConfiguration.ShowDialog();
                }
            }
        }

        bool CheckConfiguration()
        {
            ConfigurationManager manager = new ConfigurationManager();
            List<string> AllPresentSettings = manager.GetAllSettingsWithoutValues();
            string DefaultDatabase = manager.GetSetting("Default Database");
            if (CheckStrings(AllPresentSettings,"Database" + DefaultDatabase + " Name", "Database" + DefaultDatabase + " IP", "Database" + DefaultDatabase + " Port", "Database" + DefaultDatabase + " Username", "Database" + DefaultDatabase + " Password") && manager.CheckDatabaseFromFile())
                return false;
            else
                return true;
        }

        bool CheckStrings(List<string> listStrings, params string[] strings)
        {
            foreach(string s in strings)
            {
                if (!listStrings.Contains(s, StringComparer.CurrentCultureIgnoreCase))
                    return false;
            }

            return true;
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

        private void lstVIew_ItemActivated(object sender, EventArgs e)
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
        async Task<List<CLSerie>> GetSerie()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://kitsu.com.io/api/edge");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("anime?filter[text]=snafu");
            if(response.IsSuccessStatusCode)
            {
                var test = await response.Content.ReadAsStringAsync();
                MessageBox.Show(test);
            }
            return new List<CLSerie>();
        }

        private void btn_ConfirmSearch_Click(object sender, EventArgs e)
        {
            AddSeries a = new AddSeries();
            a.Show();
            //GetSerie().GetAwaiter().GetResult();
        }

        private void MnStrp_ConfigureDB(object sender, EventArgs e)
        {
            DatabaseConfiguration databaseConfiguration = new DatabaseConfiguration();
            databaseConfiguration.ShowDialog();
        }
    }
}
