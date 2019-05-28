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
using System.Net.Mail;

namespace GUISeries
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartupCheck();
        }

        //All the things that happen on startup
        void StartupCheck()
        {
            //Creates the nessecary files and folders
            if (!Directory.Exists(StaticInfo.FolderPath))
                Directory.CreateDirectory(StaticInfo.FolderPath);

            if (!Directory.Exists(StaticInfo.LocalSeriesPath))
                Directory.CreateDirectory(StaticInfo.LocalSeriesPath);

            if (!File.Exists(StaticInfo.DatabaseConfPath))
                File.Create(StaticInfo.DatabaseConfPath).Close();

            if (!File.Exists(StaticInfo.SettingsPath))
                File.Create(StaticInfo.SettingsPath).Close();

            if (!File.Exists(StaticInfo.CheckSeriesPath))
                File.Create(StaticInfo.CheckSeriesPath).Close();

            if (!File.Exists(StaticInfo.FuncDatabasesPath))
                File.Create(StaticInfo.FuncDatabasesPath).Close();

            if (!File.Exists(StaticInfo.NonFuncDatabasesPath))
                File.Create(StaticInfo.NonFuncDatabasesPath).Close();

            SetFunctionalDatabase();
            UpdateTextBoxAutoComplete();
            SetUploadSuggestions();
        }

        void SetFunctionalDatabase()
        {
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            List<Database> databases = dbconf.GetFunctionalDatabases();

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
            else if(databases.Count >= 1)
            {
                StaticInfo.CurrentDatabase = databases[0];
                lbl_CurrentDatabase.Text = "Current database: " + StaticInfo.CurrentDatabase.DatabaseName;
            }
        }

        void UpdateTextBoxAutoComplete()
        {
            AutoCompleteStringCollection COLSeries = new AutoCompleteStringCollection();

            ConfigurationManager manager = new ConfigurationManager();

            DirectoryInfo info = new DirectoryInfo(StaticInfo.LocalSeriesPath);

            foreach (FileInfo file in info.GetFiles())
            {
                JObject JOserie = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(StaticInfo.LocalSeriesPath + file.Name));
                CLSerie serie = manager.GetSeriesFromJson(JOserie);
                COLSeries.Add(serie.name);
            }

            //#1 start: Adds autocompletion options to the textbox. They are taken from the GetSerieNames method which gets it from 
            //the database
            if (StaticInfo.CurrentDatabase != null)
            {
                List<string> serieNames = GetSerieNames();

                foreach (string name in serieNames)
                {
                    if (!COLSeries.Contains(name))
                        COLSeries.Add(name);
                }
            }
            //#1 end.

            txt_Search.AutoCompleteCustomSource = COLSeries;
        }

        void SetUploadSuggestions()
        {
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand("select distinct ShowName from (Select * from Series order by UploadTimeStamp) as s limit 10", con);
            if (StaticInfo.CurrentDatabase == null)
                return;
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> shows = new List<string>();
            while (reader.Read())
            {
                shows.Add(reader["ShowName"].ToString());
            }
            con.Close();
            if (shows.Count > 0)
            {
                //Since it is ordered by UploadTimeStamp(changing SQL query from ASC to DESC doesent seem to do anything so i just reverse the list instead)
                shows.Reverse();
                List<KeyValuePair<string, int>> ShowsAndNumbers = new List<KeyValuePair<string, int>>();
                foreach (string name in shows)
                {
                    ConfigurationManager manager = new ConfigurationManager();

                    lstVw_UploadSuggestions.Items.Add(name + ", episode: " + (manager.LatestEpisode(name) + 1));
                }
            }
        }

        List<string> GetSerieNames()
        {
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            if(string.IsNullOrWhiteSpace(StaticInfo.CurrentDatabase.DatabaseName))
                return new List<string>();

            Database database = StaticInfo.CurrentDatabase;
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
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

        List<CLSerie> currentList = new List<CLSerie>();

        private void lstVIew_ItemActivated(object sender, EventArgs e)
        {
            //This is needed incase the user tries to select multiple series and press ENTER, you can only upload one serie at a time. 
            if(StaticInfo.CurrentDatabase == null)
            {
                MessageBox.Show("There doesent seem to be a functional database configured. Please configure one and try again.");
                return;
            }
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
            CLSerie serie = currentList.Find(x => x.name == lstView_SeriesFromAPI.SelectedItems[0].Name);
            AddSeries addSeries = new AddSeries(serie);
            addSeries.ShowDialog();
        }

        private void btn_ConfirmSearch_Click(object sender, EventArgs e)
        {
            ConfigurationManager manager = new ConfigurationManager();
            List<CLSerie> Series = manager.GetSeries(txt_Search.Text);
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
            UpdateDBLabel();
        }

        private void MnStrp_RemoveDB(object sender, EventArgs e)
        {
            DatabaseSelectRemove rmdb = new DatabaseSelectRemove("RemoveDatabase");
            rmdb.ShowDialog();
            UpdateDBLabel();
        }

        void UpdateDBLabel()
        {
            if (StaticInfo.CurrentDatabase != null)
                lbl_CurrentDatabase.Text = "Current database: " + StaticInfo.CurrentDatabase.DatabaseName;
            else
                lbl_CurrentDatabase.Text = "Current database: None";
        }

        private void mnStrp_SetDB(object sender, EventArgs e)
        {
            DatabaseSelectRemove DBSM = new DatabaseSelectRemove("SetDatabase");
            DBSM.ShowDialog();
            UpdateDBLabel();
        }

        private void txt_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Shift && e.KeyCode == Keys.Enter && File.Exists(StaticInfo.LocalSeriesPath + txt_Search.Text + ".json"))
            {
                ConfigurationManager manager = new ConfigurationManager();
                JObject JOserie = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(StaticInfo.LocalSeriesPath + txt_Search.Text + ".json"));
                CLSerie serie = manager.GetSeriesFromJson(JOserie);
                AddSeries addSeries = new AddSeries(serie);
                addSeries.ShowDialog();
            }
            else if (e.KeyCode == Keys.Enter)
                btn_ConfirmSearch.PerformClick();
            else if (e.Control && e.KeyCode == Keys.L)
            {
                LatestWatched watched = new LatestWatched();
                watched.ShowDialog();
            }
        }

        private void ChangeDefaultDB_Click(object sender, EventArgs e)
        {
            DatabaseSelectRemove setDefault = new DatabaseSelectRemove("ChangeDefaultDatabase");
            setDefault.ShowDialog();
        }

        private void GeneralKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.L)
            {
                LatestWatched watched = new LatestWatched();
                watched.ShowDialog();
            }
        }

        private void mnStrp_LatestEpsWatched_Click(object sender, EventArgs e)
        {
            if(StaticInfo.CurrentDatabase != null)
            {
                LatestWatched latestWatched = new LatestWatched();
                latestWatched.ShowDialog();
            }
        }

        private void mnStrp_RepairDB_Click(object sender, EventArgs e)
        {
            RepairDB repair = new RepairDB();
            repair.ShowDialog();
        }
    }
}
