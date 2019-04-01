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
            if (currentEpisode != 0)
                txt_EpisodesWatched.Text = currentEpisode.ToString() + "-" + currentEpisode.ToString();
            else
                txt_EpisodesWatched.Text = "1-1";
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
            CultureInfo info = new CultureInfo("nb-NO");
            DateTime TodayCheck = DatetimeWithToday();
            if(TodayCheck != new DateTime())
            {
                if(!txt_TimeStamp.Text.Contains("0:0") && TodayCheck.Minute == 0 && TodayCheck.Hour == 0 && TodayCheck.Second == 0 && TodayCheck.Millisecond == 0)
                {
                    string Month = TodayCheck.Month.ToString();
                    if (Month.Length == 1)
                        Month = "0" + Month;
                    lbl_TimeStamp.Text = "Date: " + TodayCheck.Day + ", Month: " + Month + Environment.NewLine + "Year: " + TodayCheck.Year + ", no time was detected." + Environment.NewLine +
                        "setting time to 00:00(earliest point of the date shown)";
                }
                else
                {
                    string Month = TodayCheck.Month.ToString();
                    if (Month.Length == 1)
                        Month = "0" + Month;
                    lbl_TimeStamp.Text = "Date: " + TodayCheck.Day + ", Month: " + Month + Environment.NewLine + "Year: " + TodayCheck.Year + ", Hour: " + TodayCheck.Hour
                    + ", Minute: " + TodayCheck.Minute;
                }
            }
            else
            {
                if(DateTime.TryParse(txt_TimeStamp.Text, info, DateTimeStyles.None, out DateTime result))
                {
                    string Month = result.Month.ToString();
                    if (Month.Length == 1)
                        Month = "0" + Month;
                    lbl_TimeStamp.Text = "Date: " + result.Day + ", Month: " + Month + Environment.NewLine + "Year: " + result.Year + ", Hour: " + result.Hour
                        + ", Minute: " + result.Minute;
                }
                else
                    lbl_TimeStamp.Text = "No date detected" + Environment.NewLine + "Example: 1.1.2001 23:59";
            }
        }

        DateTime DatetimeWithToday()
        {
            if (txt_TimeStamp.Text.Length < 5)
                return new DateTime();
            if (txt_TimeStamp.Text.Substring(0, 5) == "today")
            {
                if (txt_TimeStamp.Text.Length >= 9)
                {
                    DateTime TimeStamp = DateTime.Now.Date;
                    if(txt_TimeStamp.Text.Length >= 11)
                    {
                        if (DateTime.TryParse(txt_TimeStamp.Text.Substring(6, 5), out DateTime result3))
                        {
                            return result3;
                        }
                        else
                            return TimeStamp;//It makes sense to return TimeStamp. If the textbox contains 12 chars then it wont fit into the other things either(there would be left over text 
                        //or numbers) Just trust me it makes sense for it to be there.
                    }
                    else if(txt_TimeStamp.Text.Length >= 10)
                    {
                        if (DateTime.TryParse(txt_TimeStamp.Text.Substring(6, 4), out DateTime result2))
                        {
                            return result2;
                        }
                        else
                            return TimeStamp;
                    }
                    else if (DateTime.TryParse(txt_TimeStamp.Text.Substring(6, 3), out DateTime result))
                    {
                        return result;
                    }
                    else
                    {
                        return TimeStamp;
                    }
                }
                else
                    return DateTime.Now.Date;
            }
            else
                return new DateTime();
        }

        private void AddSeries_Load(object sender, EventArgs e)
        {
            ActiveControl = btn_Confirm;
        }
    }
}
