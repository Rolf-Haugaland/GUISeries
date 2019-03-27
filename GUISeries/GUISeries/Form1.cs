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
using MySql.Data.MySqlClient;

namespace GUISeries
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            //Creates the nessecary files and folders
            if (!Directory.Exists(StaticInfo.FolderPath))
                Directory.CreateDirectory(StaticInfo.FolderPath);
            if (!File.Exists(StaticInfo.DatabaseConfPath))
                File.Create(StaticInfo.DatabaseConfPath).Close();
            if (!File.Exists(StaticInfo.SettingsPath))
                File.Create(StaticInfo.SettingsPath);

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
            ConfigurationManager manager = new ConfigurationManager();
            if(string.IsNullOrWhiteSpace(StaticInfo.CurrentDatabase.DatabaseName))
                return new List<string>();

            Database database = StaticInfo.CurrentDatabase;
            MySqlConnection con = new MySqlConnection(manager.GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand("Select ShowName from Series GROUP BY ShowName", con);
            con.Open();
            List<string> showNames = new List<string>();

            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                showNames.Add(reader["ShowName"].ToString());
            }
            con.Close();
            return showNames;
        }

        void SetFunctionalDatabaseNoPrompt()
        {
            ConfigurationManager manager = new ConfigurationManager();
            List<Database> databases = manager.GetFunctionalDatabases();
            if(databases.Count == 0)
            {
                lbl_CurrentDatabase.Text = "Current database: No functional database found";
            }
            else
            {
                StaticInfo.CurrentDatabase = databases[0];
                lbl_CurrentDatabase.Text = "Current database: " + StaticInfo.CurrentDatabase.DatabaseName;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StartupCheck();
            if(StaticInfo.CurrentDatabase != null)
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

            if (string.IsNullOrWhiteSpace(StaticInfo.CurrentDatabase.DatabaseName))
            {
                MessageBox.Show("There doesent seem to be a functional database configured. Please configure one and try again.");
                return;
            }

            if (lstView_SeriesFromAPI.SelectedItems.Count > 1)
            {
                MessageBox.Show("Vennligst bare velg en ting du vil laste opp");
                return;
            }
            if(lstView_SeriesFromAPI.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vennligst velg en ting du vil laste opp");
                return;
            }
            AddSeries addSeries = new AddSeries();
            CLSerie serie = currentList.Find(x => x.name == lstView_SeriesFromAPI.SelectedItems[0].Name);
            addSeries.Initialize(serie);
            addSeries.ShowDialog();
            ConfigurationManager manager = new ConfigurationManager();
            List<Database> databases = manager.GetFunctionalDatabases();
            if(databases.Count > 0)
            {
                Database database = databases.Find(x => x.DefaultDB == true);
                if (!string.IsNullOrWhiteSpace(database.DatabaseUname))
                {
                    StaticInfo.CurrentDatabase = database;
                    lbl_CurrentDatabase.Text = "Current database: " + database.DatabaseName;
                }
                else
                {
                    StaticInfo.CurrentDatabase = databases[0];
                    lbl_CurrentDatabase.Text = "Current database " + databases[0].DatabaseName;
                }
            }
        }

        public List<CLSerie> GetSerie(string SearchQuery)
        {

            HttpClient client = new HttpClient();

            List<CLSerie> Series = new List<CLSerie>();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
            using (HttpResponseMessage response = client.GetAsync("https://kitsu.io/api/edge/anime?filter[text]=" + SearchQuery).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    var x = response.Content.ReadAsStringAsync();
                    JObject y = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(x.Result);
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

            add.linkToEpisodes = JSerie.SelectToken("relationships.episodes.links.related").ToString();

            return add;
        }

        private void btn_ConfirmSearch_Click(object sender, EventArgs e)
        {
            List<CLSerie> Series = GetSerie(txt_Search.Text);
            currentList = Series;
            lstView_SeriesFromAPI.Items.Clear();
            foreach(CLSerie Serie in Series)
            {
                lstView_SeriesFromAPI.Items.Add(Serie.name, Serie.name, Serie.name);
            }
            if(lstView_SeriesFromAPI.Items.Count > 0)
            {
                lstView_SeriesFromAPI.Focus();
                lstView_SeriesFromAPI.Items[0].Selected = true;
            }
        }

        private void MnStrp_ConfigureDB(object sender, EventArgs e)
        {
            AddDatabase databaseConfiguration = new AddDatabase();
            databaseConfiguration.ShowDialog();
            SetFunctionalDatabaseNoPrompt();
        }

        private void MnStrp_RemoveDB(object sender, EventArgs e)
        {
            RemoveDatabase rmdb = new RemoveDatabase();
            rmdb.ShowDialog();
            SetFunctionalDatabaseNoPrompt();
        }

        private void mnStrp_SetDB(object sender, EventArgs e)
        {
            //When you fix this and let them set a database then remove the comment from SetFunctionalDatabaseNoPrompt();
            //SetFunctionalDatabaseNoPrompt();
        }

        private void txt_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_ConfirmSearch.PerformClick();
        }
    }
}
