using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace GUISeries
{
    public partial class AddSeries : Form
    {
        public AddSeries()
        {
            InitializeComponent();
        }

        CLSerie currentSerie = null;

        public void Initialize(CLSerie serie)
        {
            lbl_Heading.Text = serie.name + ", which episodes have you watched ?";
            currentSerie = serie;
            ConfigurationManager manager = new ConfigurationManager();
            int currentEpisode = manager.LatestEpisode(serie.name) + 1;
            if(currentEpisode != -1)
                txt_EpisodesWatched.Text = currentEpisode.ToString() + "-" + currentEpisode.ToString();
        }

        void AddSerie(CLSerie serie)
        {
            //Get the datetime from textbox
            ConfigurationManager manager = new ConfigurationManager();
            List<int> integers = GetTwoInts(txt_EpisodesWatched.Text);
            List<CLEpisode> episodes = manager.GetEpisodes(serie, integers[0], integers[1]);
            CultureInfo info = new CultureInfo("nb-NO");
            if (DateTime.TryParse(txt_TimeStamp.Text, info, DateTimeStyles.None, out DateTime result))
            {
                manager.UploadEpisodes(episodes, serie, result);
            }
            else
            {
                manager.UploadEpisodes(episodes, serie, DateTime.Now);
            }
        }

        List<int> GetTwoInts(string integers)
        {
            List<int> TwoInts = new List<int>();
            for(int i = 0; integers.Length > i; i++)
            {
                if(integers[i] == '-')
                {
                    TwoInts.Add(Convert.ToInt32(integers.Substring(0, i)));
                    TwoInts.Add(Convert.ToInt32(integers.Substring(i + 1, integers.Length - i - 1)));
                }
            }

            return TwoInts;
        }

        private void MenStrp_HelpClick(object sender, EventArgs e)
        {
            DisplayInformation Info = new DisplayInformation();
            Info.Setup("AddSeries.Help");
            Info.ShowDialog();
        }

        private void Frm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.Enter)
            {
                //Add the series
            }
            if (e.KeyCode == Keys.Up && !string.IsNullOrWhiteSpace(txt_EpisodesWatched.Text))
            {
                txt_EpisodesWatched.Text = AddOne(txt_EpisodesWatched.Text);
            }
            if (e.KeyCode == Keys.Down && !string.IsNullOrWhiteSpace(txt_EpisodesWatched.Text))
                txt_EpisodesWatched.Text = RemoveOne(txt_EpisodesWatched.Text);
        }

        string RemoveOne(string episode)
        {
            int startingEpisode;
            int endingEpisode;

            int fIntLength = FInt(episode);

            startingEpisode = int.Parse(episode.Substring(0, fIntLength));
            endingEpisode = int.Parse(episode.Substring(fIntLength + 1, (episode.Length - fIntLength) - 1));
            endingEpisode -= 1;
            return startingEpisode.ToString() + "-" + endingEpisode.ToString();
        }

        /// <summary>
        /// In the format of 2 numbers seperated with a dash(-) it will add one to the last number. Example: 31-32 will become 31-33
        /// </summary>
        /// <param name="episode">the string to add one to</param>
        /// <returns>The string with an incremented ending integer.</returns>
        string AddOne(string episode)
        {
            int startingEpisode;
            int endingEpisode;

            int fIntLength = FInt(episode);

            startingEpisode = int.Parse(episode.Substring(0, fIntLength));
            endingEpisode = int.Parse(episode.Substring(fIntLength + 1, (episode.Length - fIntLength) - 1));
            endingEpisode++;
            return startingEpisode.ToString() + "-" + endingEpisode.ToString();
        }

        int FInt(string episode)
        {
            for (int i = 0; episode.Length > i; i++)
            {
                if (!int.TryParse(episode[i].ToString(), out int result))
                    return i;
            }
            throw new Exception("Method fInt within method AddOne within the AddSeries form got a string of fully parsable integers which it is " +
                "not supposed to, allegedly the user typed in a manual input that did not include a non integer.");
        }

        private void txt_EpisodeWatched_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Up && !string.IsNullOrWhiteSpace(txt_EpisodesWatched.Text))
                txt_EpisodesWatched.Text = AddOne(txt_EpisodesWatched.Text);
            if (e.KeyCode == Keys.Down && !string.IsNullOrWhiteSpace(txt_EpisodesWatched.Text))
                txt_EpisodesWatched.Text = RemoveOne(txt_EpisodesWatched.Text);
        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            AddSerie(currentSerie);
        }

        private void txt_TimeStamp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CultureInfo info = new CultureInfo("nb-NO");
                DateTime TimeStamp = DateTime.Parse(txt_TimeStamp.Text, info);
                string Month = TimeStamp.Month.ToString();
                if (Month.Length == 1)
                    Month = "0" + Month;
                lbl_TimeStamp.Text = "Date: " + TimeStamp.Day + ", Month: " + Month + Environment.NewLine + "Year: " + TimeStamp.Year + ", Hour: " + TimeStamp.Hour
                    + ", Minute: " + TimeStamp.Minute;
            }
            catch
            {
                lbl_TimeStamp.Text = "No date detected" + Environment.NewLine + "Example: 1.1.2001 23:59";
            }
        }

        private void AddSeries_Load(object sender, EventArgs e)
        {
            ActiveControl = btn_Confirm;
        }
    }
}
