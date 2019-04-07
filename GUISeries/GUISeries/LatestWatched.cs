using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using static System.Windows.Forms.ListViewItem;

namespace GUISeries
{
    public partial class LatestWatched : Form
    {
        public LatestWatched()
        {
            InitializeComponent();
            StartUp();
        }

        bool ConfirmedLastCLicked = false;

        void StartUp()
        {
            OriginalLatestEpisodesInShows = GetKeyValuePairsFromDB();
            items = SetShowHistory();
            cmbBox_AscOrDesc.Items.Add("Ascending");
            cmbBox_AscOrDesc.Items.Add("Descending");
            cmbBox_AscOrDesc.SelectedItem = cmbBox_AscOrDesc.Items[0];
            OrderByDate();
        }

        List<ListViewItem> items;

        List<ListViewItem> SetShowHistory()
        {
            List<CLSerie> WholeHistory = new List<CLSerie>();

            ConfigurationManager manager = new ConfigurationManager();
            MySqlConnection con = new MySqlConnection(manager.GetConnectionstring());

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Series", con);

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> boolFromDB = new List<string>();
            while(reader.Read())
            {
                WholeHistory.Add(new CLSerie()
                {
                    DBID = Convert.ToInt32(reader["id"].ToString()),
                    episodeName = reader["Name"].ToString(),
                    episodeCount = Convert.ToInt32(reader["EpisodeCount"].ToString()),
                    ageRating = reader["AgeRating"].ToString(),
                    synopsis = reader["Synopsis"].ToString(),
                    totalLength = Convert.ToInt32(reader["TotalShowLength"].ToString()),
                    length = Convert.ToInt32(reader["Length"].ToString()),
                    EpisodeNumber = Convert.ToInt32(reader["EpisodeNumber"]),
                    seasonNumber = reader["SeasonNumber"].ToString(),
                    showName = reader["ShowName"].ToString(),
                    TimeStamp = Convert.ToDateTime(reader["TimeStamp"]),
                    status = reader["Status"].ToString(),
                    genres = reader["Genres"].ToString().Split(',').ToList()
                });
                boolFromDB.Add(reader["NSFW"].ToString());
            }
            //Change it so that there is a button to use all episodes, not just current date. This will unlock abillity to sort the items from 1 
            //spesific date by name and (redundant) date
            con.Close();
            for(int i = 0; WholeHistory.Count > i; i++)
            {
                if (boolFromDB[i] == "0")
                    WholeHistory[i].NSFW = false;
                else
                    WholeHistory[i].NSFW = true;
            }

            List<ListViewItem> items = new List<ListViewItem>();
            foreach (CLSerie episode in WholeHistory)
            {
                ListViewItem item = new ListViewItem(episode.showName);
                item.SubItems.Add(episode.EpisodeNumber.ToString());
                item.SubItems.Add(episode.seasonNumber);
                item.SubItems.Add(episode.TimeStamp.ToString());
                items.Add(item);
            }

            return items;
        }
        List<KeyValuePair<string, int>> OriginalLatestEpisodesInShows = new List<KeyValuePair<string, int>>();

        private void LatestWatched_Load(object sender, EventArgs e)
        {
            UpdateListBox("");
        }

        List<KeyValuePair<string,int>> GetKeyValuePairsFromDB()
        {
            List<KeyValuePair<string, int>> returnThis = new List<KeyValuePair<string, int>>();
            ConfigurationManager manager = new ConfigurationManager();
            MySqlConnection con = new MySqlConnection(manager.GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Series", con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                KeyValuePair<string, int> add = new KeyValuePair<string, int>((string)reader["ShowName"], (int)reader["EpisodeNumber"]);
                returnThis.Add(add);
            }
            con.Close();
            return returnThis;
        }

        void UpdateListBox(string filter)
        {
            listBox1.Items.Clear();
            List<string> AlreadyAdded = new List<string>();
            List<KeyValuePair<string, int>> LatestEpisodesInShows = OriginalLatestEpisodesInShows.ToList();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                LatestEpisodesInShows = LatestEpisodesInShows.FindAll(x => x.Key.ToUpper().Contains(filter.ToUpper()));
            }
            if (LatestEpisodesInShows.Count == 0)
                return;
            for (int i = 0; LatestEpisodesInShows.Count > i; i++)
            {
                var AllButNotAlreadyAdded = LatestEpisodesInShows.FindAll(x => !AlreadyAdded.Contains(x.Key));
                var HighestNumberIndex = AllButNotAlreadyAdded.FindIndex(x => x.Value == AllButNotAlreadyAdded.Max(y => y.Value));
                KeyValuePair<string, int> HighestIntKeyValPair = AllButNotAlreadyAdded[HighestNumberIndex];
                listBox1.Items.Add(HighestIntKeyValPair.Key + ", Current episode: " + HighestIntKeyValPair.Value.ToString());

                var OnlyCurrentShow = LatestEpisodesInShows.FindAll(x => x.Key == HighestIntKeyValPair.Key);
                LatestEpisodesInShows.RemoveAll(x => x.Value != OnlyCurrentShow.Max(y => y.Value) && x.Key == HighestIntKeyValPair.Key);

                AlreadyAdded.Add(HighestIntKeyValPair.Key);
            }

        }

        private void txt_Filter_TextChanged(object sender, EventArgs e)
        {
            UpdateListBox(txt_Filter.Text);
        }

        private void rdBtn_CheckChanged(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            if(btn.Name == "rdBtn_OrderByDate")
            {
                OrderByDate();
            }
            else if(btn.Name == "rdBtn_OrderByName")
            {
                OrderByName();
            }
        }

        private void cmBoxIndexChanged(object sender, EventArgs e)
        {
            UpdateBasedOnSelections();
        }

        void UpdateBasedOnSelections()
        {
            if (ConfirmedLastCLicked)
            {
                SelectAndOrderByDate(dtPicker.Value.Date);
            }
            if (rdBtn_OrderByDate.Checked)
            {
                OrderByDate();
            }
            else if (rdBtn_OrderByName.Checked)
            {
                OrderByName();
            }
            else
                OrderByDate();
        }

        void OrderByName()
        {
            ConfirmedLastCLicked = false;
            lstVw_ShowHistory.Items.Clear();
            List<ListViewItem> temp = items.ToList();
            if(chkBx_DateOrAll.Checked)
            {
                temp = GetAllItemsForDate(dtPicker.Value.Date);
            }
            if (cmbBox_AscOrDesc.SelectedItem.ToString() == "Ascending")
                temp = temp.OrderBy(x => x.SubItems[0].Text).ToList();
            else
                temp = temp.OrderByDescending(x => x.SubItems[0].Text).ToList();
            lstVw_ShowHistory.Items.Clear();
            foreach (ListViewItem item in temp)
            {
                lstVw_ShowHistory.Items.Add(item);
            }
            lbl_LstItemCount.Text = "The list currently has " + temp.Count.ToString() + " episodes";
        }

        void OrderByDate()
        {
            ConfirmedLastCLicked = false;
            lstVw_ShowHistory.Items.Clear();
            List<ListViewItem> AddToLstView = items.ToList();
            if(chkBx_DateOrAll.Checked)
            {
                AddToLstView = GetAllItemsForDate(dtPicker.Value.Date);
            }
            if(cmbBox_AscOrDesc.SelectedItem.ToString() == "Descending")
                AddToLstView = AddToLstView.OrderBy(x => Convert.ToDateTime(x.SubItems[3].Text)).ToList();
            else
                AddToLstView = AddToLstView.OrderByDescending(x => Convert.ToDateTime(x.SubItems[3].Text)).ToList();

            foreach (ListViewItem item in AddToLstView)
            {
                lstVw_ShowHistory.Items.Add(item);
            }
            lbl_LstItemCount.Text = "The list currently has " + AddToLstView.Count.ToString() + " episodes";
        }

        void SelectAndOrderByDate(DateTime dateToCheck)
        {
            ConfirmedLastCLicked = true;
            DateTime date = dateToCheck.Date;

            lstVw_ShowHistory.Items.Clear();
            List<ListViewItem> AddDates = GetAllItemsForDate(dtPicker.Value.Date.Date);
            if (cmbBox_AscOrDesc.SelectedItem.ToString() == "Ascending")
                AddDates = AddDates.OrderBy(x => Convert.ToDateTime(x.SubItems[3].Text)).ToList();
            else
                AddDates = AddDates.OrderByDescending(x => Convert.ToDateTime(x.SubItems[3].Text)).ToList();

            foreach (ListViewItem item in AddDates)
            {
                lstVw_ShowHistory.Items.Add(item);
            }
            label1.Text = "How many episodes have i watched at this date: " + AddDates.Count;
            lbl_LstItemCount.Text = "The list currently has " + AddDates.Count.ToString() + " episodes";
        }

        List<ListViewItem> GetAllItemsForDate(DateTime date)
        {
            List<ListViewItem> OnlyFromOneDate = items.ToList();
            OnlyFromOneDate = OnlyFromOneDate.FindAll(x => Convert.ToDateTime(x.SubItems[3].Text).Date == date.Date);

            return OnlyFromOneDate;
        }

        private void dtPckrValChanged(object sender, EventArgs e)
        {
            if(chkBx_DateOrAll.Checked)
                SelectAndOrderByDate(dtPicker.Value.Date);
        }

        private void chkBx_DateOrAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBx_DateOrAll.Checked)
                SelectAndOrderByDate(dtPicker.Value.Date);
            else
                UpdateBasedOnSelections();
        }
    }
}
