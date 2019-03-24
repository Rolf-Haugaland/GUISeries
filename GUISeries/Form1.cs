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
using Newtonsoft.Json.Linq;

namespace GUISeries
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartupCheck();
        }

        List<CLSerie> currentList = new List<CLSerie>();

        void SetFunctionalDatabase()
        {
            ConfigurationManager manager = new ConfigurationManager();
            List<Database> databases = manager.GetFunctionalDatabases();

            if (databases.Count == 0)
            {
                DialogResult result = MessageBox.Show("Det ser ikke ut at du har lagt til en funksjonell database enda, vil du gjøre det nå?", "Konfigurere database?", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    AddDatabase databaseConfiguration = new AddDatabase();
                    databaseConfiguration.ShowDialog();
                    SetFunctionalDatabase();
                }
                else if (result == DialogResult.No)
                    lbl_CurrentDatabase.Text = "Current database: No functional database found";
            }
            else if(databases.Count == 1)
            {
                StaticInfo.CurrentDatabase = databases[0];
                lbl_CurrentDatabase.Text = "Current database: " + StaticInfo.CurrentDatabase.DatabaseName;
            }
            else if(databases.Count > 1)
            {
                SetDatabase setDatabase = new SetDatabase();
                setDatabase.ShowDialog();
            }
        }

        void StartupCheck()
        {
            //Creates a settings file if it doesent already exist
            if (!File.Exists("Settings.txt"))
                File.Create("Settings.txt").Close();
            SetFunctionalDatabase();
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

            AddSeries addSeries = new AddSeries();
            CLSerie serie = currentList.Find(x => x.name == lstView_SeriesFromAPI.SelectedItems[0].Name);
            addSeries.Initialize(serie);
            addSeries.Show();
        }
        public async Task<List<CLSerie>> GetSerie(string SearchQuery)
        {

            HttpClient client = new HttpClient();

            List<CLSerie> Series = new List<CLSerie>();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
            using (HttpResponseMessage response = client.GetAsync("https://kitsu.io/api/edge/anime?filter[text]=" + SearchQuery).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var x = await response.Content.ReadAsStringAsync();
                    JObject y = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(x);
                    var FirstSerie = y.Children().Children().Children().ToArray();
                    foreach(JToken child in FirstSerie)
                    {
                        try
                        {
                            JObject child2 = (JObject)child;
                            if (child2.TryGetValue("id", out JToken JSerie))
                            {
                                Series.Add(GetSeriesFromJson(child2));
                            }
                        }
                        catch (Exception ex)
                        {
                            //The conversion might fail it there is only one item in child etc. This happends every time so we just try catch the expected error.
                        }
                    }
                }
            }

            return Series;
        }

        CLSerie GetSeriesFromJson(JObject JSerie)
        {
            JToken attributes = JSerie.GetValue("attributes");

            JObject jObject = (JObject)attributes;

            CLSerie add = Newtonsoft.Json.JsonConvert.DeserializeObject<CLSerie>(jObject.ToString());

            return add;
        }

        private void btn_ConfirmSearch_Click(object sender, EventArgs e)
        {
            List<CLSerie> Series = GetSerie(txt_Search.Text).GetAwaiter().GetResult();
            currentList = Series;
            foreach(CLSerie Serie in Series)
            {
                lstView_SeriesFromAPI.Items.Add(Serie.name, Serie.name, Serie.name);
            }
        }

        private void MnStrp_ConfigureDB(object sender, EventArgs e)
        {
            AddDatabase databaseConfiguration = new AddDatabase();
            databaseConfiguration.ShowDialog();
        }

        private void MnStrp_RemoveDB(object sender, EventArgs e)
        {
            RemoveDatabase rmdb = new RemoveDatabase();
            rmdb.ShowDialog();
            ConfigurationManager manager = new ConfigurationManager();
            List<Database> databases = manager.GetFunctionalDatabases();
            if(databases.Count == 0)
            {
                lbl_CurrentDatabase.Text = "Current database: No functional database found";
            }
        }

        private void mnStrp_SetDB(object sender, EventArgs e)
        {

        }
    }
}
