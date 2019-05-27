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

            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Series", con);

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> boolFromDB = new List<string>();
            while (reader.Read())
            {
                CLSerie serie = new CLSerie()
                {
                    episodeName = reader["Name"].ToString(),
                    ageRating = reader["AgeRating"].ToString(),
                    synopsis = reader["Synopsis"].ToString(),
                    seasonNumber = reader["SeasonNumber"].ToString(),
                    showName = reader["ShowName"].ToString(),
                    genres = reader["Genres"].ToString().Split(',').ToList()
                };
                if (int.TryParse(reader["id"].ToString(), out int DBID))
                    serie.DBID = DBID;

                if (int.TryParse(reader["EpisodeCount"].ToString(), out int EPCount))
                    serie.episodeCount = EPCount;

                if (int.TryParse(reader["TotalShowLength"].ToString(), out int TotShowLength))
                    serie.totalLength = TotShowLength;

                if (int.TryParse(reader["Length"].ToString(), out int EpLength))
                    serie.length = EpLength;

                if (int.TryParse(reader["EpisodeNumber"].ToString(), out int EpNum))
                    serie.EpisodeNumber = EpNum;

                if (DateTime.TryParse(reader["TimeStamp"].ToString(), out DateTime TimeStamp))
                    serie.TimeStamp = TimeStamp;

                WholeHistory.Add(serie);
                boolFromDB.Add(reader["NSFW"].ToString());
            }
            //Change it so that there is a button to use all episodes, not just current date. This will unlock abillity to sort the items from 1 
            //spesific date by name and (redundant) date
            con.Close();
            for (int i = 0; WholeHistory.Count > i; i++)
            {
                if (boolFromDB[i] == "1")
                    WholeHistory[i].NSFW = true;
                else
                    WholeHistory[i].NSFW = false;
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

        List<KeyValuePair<string, int>> GetKeyValuePairsFromDB()
        {
            List<KeyValuePair<string, int>> returnThis = new List<KeyValuePair<string, int>>();
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
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

        // HEAD
        private void rdBtn_CheckChanged(object sender, EventArgs e)
        {
            RadioButton btn = (RadioButton)sender;
            if (btn.Name == "rdBtn_OrderByDate")
            {
                OrderByDate();
            }
            else if (btn.Name == "rdBtn_OrderByName")
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
            if (chkBx_DateOrAll.Checked)
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
            if (chkBx_DateOrAll.Checked)
            {
                AddToLstView = GetAllItemsForDate(dtPicker.Value.Date);
            }
            if (cmbBox_AscOrDesc.SelectedItem.ToString() == "Descending")
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
            if (chkBx_DateOrAll.Checked)
                SelectAndOrderByDate(dtPicker.Value.Date);
        }

        private void chkBx_DateOrAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBx_DateOrAll.Checked)
                SelectAndOrderByDate(dtPicker.Value.Date);
            else
                UpdateBasedOnSelections();
        }

        List<DateTime> DaysNotWatched()
        {
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Series", con);

            List<DateTime> AllDates = new List<DateTime>();

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                AllDates.Add((DateTime)reader["TimeStamp"]);
            }

            con.Close();

            for (int i = 0; AllDates.Count > i; i++)
            {
                DateTime c = AllDates[i];
                AllDates.RemoveAt(i);
                c = c.AddHours(-c.Hour);
                c = c.AddMinutes(-c.Minute);
                c = c.AddSeconds(-c.Second);
                c = c.AddMilliseconds(c.Millisecond);
                AllDates.Insert(i, c);
            }

            DateTime first = AllDates.Min();
            DateTime second = AllDates.Max();
            List<DateTime> AllBetween = DaysBetween(first, second);
            List<DateTime> NotWatched = AllBetween.FindAll(x => !AllDates.Contains(x.Date));
            MessageBox.Show("Starting from day " + first.Date.ToString("dd.MM.yyyy") + " to day " + second.Date.ToString("dd.MM.yyyy") + " you have not watched any episodes " + NotWatched.Count.ToString() + " days");
            return NotWatched;
        }

        /// <summary>
        /// Returns a list of all datetimes between first and second with time set to 12:00:00 and 0 milliseconds 
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        List<DateTime> DaysBetween(DateTime first, DateTime second)
        {
            //Maybe check if first is more than second, since that would mess up the function
            int diff = 1;

            if (first.AddDays(1).Date > second.Date)
                throw new Exception("First date cannot be bigger than second date");

            first = first.AddHours(-first.Hour);
            first = first.AddMinutes(-first.Minute);
            first = first.AddSeconds(-first.Second);
            first = first.AddMilliseconds(-first.Millisecond);

            List<DateTime> allDates = new List<DateTime>();
            allDates.Add(first);

        loop:
            if (first.AddDays(diff).Date != second.Date)
            {
                allDates.Add(first.AddDays(diff));
                diff++;
                goto loop;
            }
            else
            {
                allDates.Add(first.AddDays(diff));
                return allDates;
            }
        }
    }
}
