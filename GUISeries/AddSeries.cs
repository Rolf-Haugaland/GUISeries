﻿using System;
using System.Windows.Forms;

namespace GUISeries
{
    public partial class AddSeries : Form
    {
        public AddSeries()
        {
            InitializeComponent();
        }

        private void MenStrp_HelpClick(object sender, EventArgs e)
        {
            DisplayInformation Info = new DisplayInformation();
            Info.setup("AddSeries.Help");
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

            int fIntLength = fInt(episode);

            startingEpisode = int.Parse(episode.Substring(0, fIntLength));
            endingEpisode = int.Parse(episode.Substring(fIntLength + 1, (episode.Length - fIntLength) - 1));
            endingEpisode++;
            return startingEpisode.ToString() + "-" + endingEpisode.ToString();
        }

        int fInt(string episode)
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
        }
    }
}
