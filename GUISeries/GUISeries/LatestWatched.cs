using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace GUISeries
{
    public partial class LatestWatched : Form
    {
        public LatestWatched()
        {
            DaysNotWatched();
            InitializeComponent();
            OriginalLatestEpisodesInShows = GetKeyValuePairsFromDB();
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

        List<DateTime> DaysNotWatched()
        {
            ConfigurationManager manager = new ConfigurationManager();
            MySqlConnection con = new MySqlConnection(manager.GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Series", con);

            List<DateTime> AllDates = new List<DateTime>();

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
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
            MessageBox.Show("Starting from day " + first.Date.ToString("dd.MM.yyyy")+ " to day " + second.Date.ToString("dd.MM.yyyy") + " you have not watched any episodes " + NotWatched.Count.ToString() + " days");
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
