using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace GUISeries
{
    public partial class LatestWatched : Form
    {
        public LatestWatched()
        {
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
    }
}
