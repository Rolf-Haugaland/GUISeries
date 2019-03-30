﻿using System;
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
            else if(databases.Count >= 1)
            {
                StaticInfo.CurrentDatabase = databases[0];
                lbl_CurrentDatabase.Text = "Current database: " + StaticInfo.CurrentDatabase.DatabaseName;
            }
        }

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

        private void Form1_Load(object sender, EventArgs e)
        {
            StartupCheck();
            UpdateTextBoxAutoComplete();
            SetUploadSuggestions();
        }

        void UpdateTextBoxAutoComplete()
        {
            AutoCompleteStringCollection COLSeries = new AutoCompleteStringCollection();

            ConfigurationManager manager = new ConfigurationManager();

            DirectoryInfo info = new DirectoryInfo(StaticInfo.LocalSeriesPath);

            foreach(FileInfo file in info.GetFiles())
            {
                JObject JOserie = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(StaticInfo.LocalSeriesPath + file.Name));
                CLSerie serie = manager.GetSeriesFromJson(JOserie);
                COLSeries.Add(serie.name);
            }

            //#1 start: Adds autocompletion options to the textbox. They are taken from the GetSerieNames method which gets it from 
            //the database
            if(StaticInfo.CurrentDatabase != null)
            {
                List<string> serieNames = GetSerieNames();

                foreach (string name in serieNames)
                {
                    if(!COLSeries.Contains(name))
                        COLSeries.Add(name);
                }
            }
            //#1 end.

            txt_Search.AutoCompleteCustomSource = COLSeries;
        }

        void SetUploadSuggestions()
        {
            //Potentionally add series to json files. So that you wont have to call the api for a serie. When the textbox autofills, 
            //if the user clicks SHIFT+ENTER then it will automatically select that serie and use it from the json file instead of 
            //putting it in the list box. Also update this regularly like every week to make sure the locally saved json is up to date. 

            //Get the most recent database uploads(CreationTimeStamp, not watchTimeStamp.) Then search the database for the 'highest' 
            //number episode in that series that the user has watched. Put that serie in the lstVw_UploadSuggestions, do this for like the 
            //5-10 most recent uploads, if there are that many. 
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
        }

        private void MnStrp_RemoveDB(object sender, EventArgs e)
        {
            DatabaseSelectRemove rmdb = new DatabaseSelectRemove();
            rmdb.Initialize("RemoveDatabase");
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
            DatabaseSelectRemove DBSM = new DatabaseSelectRemove();
            DBSM.Initialize("SetDatabase");
            DBSM.ShowDialog();
            if (StaticInfo.CurrentDatabase != null)
                lbl_CurrentDatabase.Text = "Current database: " + StaticInfo.CurrentDatabase.DatabaseName;
            else
                lbl_CurrentDatabase.Text = "Current database: no functional database found";
        }

        private void txt_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Shift && e.KeyCode == Keys.Enter && File.Exists(StaticInfo.LocalSeriesPath + txt_Search.Text + ".json"))
            {
                JObject JOserie = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(StaticInfo.LocalSeriesPath + txt_Search.Text + ".json"));
                ConfigurationManager manager = new ConfigurationManager();
                CLSerie serie = manager.GetSeriesFromJson(JOserie);
                AddSeries addSeries = new AddSeries();
                addSeries.Initialize(serie);
                addSeries.ShowDialog();
            }
            else if (e.KeyCode == Keys.Enter)
                btn_ConfirmSearch.PerformClick();
        }

        private void ChangeDefaultDB_Click(object sender, EventArgs e)
        {
            //Do this sht
        }
    }
}
