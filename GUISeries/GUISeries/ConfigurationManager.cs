using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

namespace GUISeries
{
    class ConfigurationManager
    {
        /// <summary>
        /// Searches the database for the serie name provided. Returns an integer that is the highest number episode watched. 
        /// Example: i have added episode 1-5 of naruto. This method gets 'naruto' as SerieName and returns 5 since its the highest 
        /// episode number. Returns -1 if the database holds no record of the show
        /// </summary>
        /// <param name="SerieName">The name of the series to return the highest episode number of</param>
        /// <returns></returns>
        public int LatestEpisode(string SerieName)
        {
            MySqlCommand cmd = new MySqlCommand("Select * from Series where ShowName = '" + SerieName + "'");
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
            cmd.Connection = con;
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<int> Episodes = new List<int>();
            while (reader.Read())
            {
                Episodes.Add(Convert.ToInt16(reader["EpisodeNumber"]));
            }
            if (Episodes.Count != 0)
                return Episodes.Max();
            else
                return -1;
        }

        /// <summary>
        /// returns all the databases in the Settings
        /// working or not. GetFunctionalDatabases() only returns functional databases
        /// </summary>
        /// <returns>All databases in the Settings.txt file, connectable or not</returns>

        public List<CLSerie> GetSeries(string SearchQuery)
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
                    foreach (JToken child in FirstSerie)
                    {
                        try
                        {
                            JObject child2 = (JObject)child;
                            if (child2.TryGetValue("id", out JToken JSerie))
                            {
                                //Write child2 to file and use GetSeriesFromJson(JObject JSerie) to read it again. 

                                CLSerie add = GetSeriesFromJson(child2);
                                Series.Add(add);
                                if (add.status != "finished")
                                {
                                    List<string> SeriesToCheck = new List<string>();
                                    JArray jArray = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(StaticInfo.CheckSeriesPath));
                                    if (jArray != null)
                                    {
                                        foreach (var jToken in jArray)
                                        {
                                            SeriesToCheck.Add(jToken.ToString());
                                        }
                                    }
                                    if (!SeriesToCheck.Contains(add.name))
                                        SeriesToCheck.Add(add.name);
                                    string writeToFile = Newtonsoft.Json.JsonConvert.SerializeObject(SeriesToCheck);
                                    File.WriteAllText(StaticInfo.CheckSeriesPath, writeToFile);
                                }
                                File.WriteAllText(StaticInfo.FolderPath + "Series\\" + add.name + ".json", child2.ToString());
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

        public string GetOneSerie(string SerieName)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
            using (HttpResponseMessage response = client.GetAsync("https://kitsu.io/api/edge/anime?[page]limit=1&filter[text]=" + SerieName).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            return "";
        }

        public CLSerie GetSeriesFromJson(JObject JSerie)
        {
            JToken attributes = JSerie.GetValue("attributes");

            JObject jObject = (JObject)attributes;

            CLSerie add = Newtonsoft.Json.JsonConvert.DeserializeObject<CLSerie>(jObject.ToString());

            add.linkToEpisodes = JSerie.SelectToken("relationships.episodes.links.related").ToString();

            add.linkToGenres = JSerie.SelectToken("relationships.genres.links.related").ToString();

            return add;
        }

        public void UploadEpisodes(List<CLEpisode> episodes, CLSerie serie, DateTime timestamp)
        {
            CultureInfo info = CultureInfo.CreateSpecificCulture("nb-NO");
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand();
            string SQL = "";//Fix insert or update
                            //if(InstOrUpdate == "insert")
            SQL = "Insert into Series(Name,EpisodeCount,AgeRating,NSFW,Synopsis,TotalShowLength,Length,EpisodeNumber,SeasonNumber,ShowName,TimeStamp,Genres,Status) VALUES(";
            int i = 0;
            int count = 1;
            foreach (CLEpisode episode in episodes)
            {
                i++;
                //I need closing parentesies if it is the last iteration of the loop
                if (count == episodes.Count)
                {
                    SQL += "@Name" + i.ToString() + ",@EpisodeCount" + i.ToString() + ",@AgeRating" + i.ToString() + ",@NSFW" + i.ToString() + ",@Synopsis" +
    i.ToString() + ",@TotalShowLength" + i.ToString() + ",@Length" + i.ToString() + ",@EpisodeNumber" + i.ToString() + ",@SeasonNumber" +
    i.ToString() + ",@ShowName" + i.ToString() + ",@TimeStamp" + i.ToString() + ",@Genres" + i.ToString() + ",@Status" + i.ToString() + ")";
                }
                else
                {
                    SQL += "@Name" + i.ToString() + ",@EpisodeCount" + i.ToString() + ",@AgeRating" + i.ToString() + ",@NSFW" + i.ToString() + ",@Synopsis" +
    i.ToString() + ",@TotalShowLength" + i.ToString() + ",@Length" + i.ToString() + ",@EpisodeNumber" + i.ToString() + ",@SeasonNumber" +
    i.ToString() + ",@ShowName" + i.ToString() + ",@TimeStamp" + i.ToString() + ",@Genres" + i.ToString() + ",@Status" + i.ToString() + "),(";
                }
                MySqlParameter parName = new MySqlParameter()
                {
                    ParameterName = "@Name" + i.ToString(),
                    Value = episode.episodeName
                };
                cmd.Parameters.Add(parName);
                MySqlParameter parEpisodeCount = new MySqlParameter()
                {
                    ParameterName = "@EpisodeCount" + i.ToString(),
                    Value = serie.episodeCount
                };
                cmd.Parameters.Add(parEpisodeCount);
                MySqlParameter parAgeRating = new MySqlParameter()
                {
                    ParameterName = "@AgeRating" + i.ToString(),
                    Value = serie.ageRating
                };
                cmd.Parameters.Add(parAgeRating);
                MySqlParameter parNSFW = new MySqlParameter()
                {
                    ParameterName = "@NSFW" + i.ToString(),
                    Value = serie.NSFW
                };
                cmd.Parameters.Add(parNSFW);
                MySqlParameter parSynopsis = new MySqlParameter()
                {
                    ParameterName = "@Synopsis" + i.ToString(),
                    Value = episode.synopsis
                };
                cmd.Parameters.Add(parSynopsis);
                MySqlParameter parTotalShowLength = new MySqlParameter()
                {
                    ParameterName = "@TotalShowLength" + i.ToString(),
                    Value = serie.totalLength
                };
                cmd.Parameters.Add(parTotalShowLength);
                MySqlParameter parLength = new MySqlParameter()
                {
                    ParameterName = "@Length" + i.ToString(),
                    Value = episode.length
                };
                cmd.Parameters.Add(parLength);
                MySqlParameter parEpisodeNumber = new MySqlParameter()
                {
                    ParameterName = "@EpisodeNumber" + i.ToString(),
                    Value = episode.EpisodeNumber
                };
                cmd.Parameters.Add(parEpisodeNumber);
                MySqlParameter parSeasonNumber = new MySqlParameter()
                {
                    ParameterName = "@SeasonNumber" + i.ToString(),
                    Value = episode.seasonNumber
                };
                cmd.Parameters.Add(parSeasonNumber);
                MySqlParameter parShowName = new MySqlParameter()
                {
                    ParameterName = "@ShowName" + i.ToString(),
                    Value = serie.name
                };
                cmd.Parameters.Add(parShowName);
                MySqlParameter parTimeStamp = new MySqlParameter()
                {
                    ParameterName = "@TimeStamp" + i.ToString(),
                    Value = timestamp
                };
                cmd.Parameters.Add(parTimeStamp);
                MySqlParameter parGenres = new MySqlParameter()
                {
                    ParameterName = "@Genres" + i.ToString(),
                    Value = serie.genres
                };
                cmd.Parameters.Add(parGenres);
                MySqlParameter parStatus = new MySqlParameter()
                {
                    ParameterName = "@Status" + i.ToString(),
                    Value = serie.status
                };
                cmd.Parameters.Add(parStatus);
                count++;
            }
            cmd.Connection = con;
            cmd.CommandText = SQL;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        //public void UpdateDBEntry(List<CLSerie> Serie)
        //{
        //    DatabaseConfiguration dbconf = new DatabaseConfiguration();
        //    MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
        //    MySqlCommand cmd = new MySqlCommand();
        //    string SQL = "UPDATE Series(EpisodeName,EpisodeCount,AgeRating,NSFW,Synopsis,TotalShowLength,EpisodeLength,EpisodeNumber,SeasonNumber,ShowName,TimeStamp,Genres,Status) " +
        //        "VALUES(@Name,@EpisodeCount,@AgeRating,@NSFW,@Synopsis,@TotalShowLength,@Length,@EpisodeNumber,@SeasonNumber,@ShowName,@TimeStamp,@Genres,@Status),(";
            
        //    MySqlParameter parName = new MySqlParameter()
        //    {
        //        ParameterName = "@Name",
        //        Value = Serie.episodeName
        //    };
        //    cmd.Parameters.Add(parName);
        //    MySqlParameter parEpisodeCount = new MySqlParameter()
        //    {
        //        ParameterName = "@EpisodeCount",
        //        Value = Serie.episodeCount
        //    };
        //    cmd.Parameters.Add(parEpisodeCount);
        //    MySqlParameter parAgeRating = new MySqlParameter()
        //    {
        //        ParameterName = "@AgeRating",
        //        Value = Serie.ageRating
        //    };
        //    cmd.Parameters.Add(parAgeRating);
        //    MySqlParameter parNSFW = new MySqlParameter()
        //    {
        //        ParameterName = "@NSFW",
        //        Value = Serie.NSFW
        //    };
        //    cmd.Parameters.Add(parNSFW);
        //    MySqlParameter parSynopsis = new MySqlParameter()
        //    {
        //        ParameterName = "@Synopsis",
        //        Value = Serie.synopsis
        //    };
        //    cmd.Parameters.Add(parSynopsis);
        //    MySqlParameter parTotalShowLength = new MySqlParameter()
        //    {
        //        ParameterName = "@TotalShowLength",
        //        Value = Serie.totalLength
        //    };
        //    cmd.Parameters.Add(parTotalShowLength);
        //    MySqlParameter parLength = new MySqlParameter()
        //    {
        //        ParameterName = "@Length",
        //        Value = Serie.length
        //    };
        //    cmd.Parameters.Add(parLength);
        //    MySqlParameter parEpisodeNumber = new MySqlParameter()
        //    {
        //        ParameterName = "@EpisodeNumber",
        //        Value = Serie.EpisodeNumber
        //    };
        //    cmd.Parameters.Add(parEpisodeNumber);
        //    MySqlParameter parSeasonNumber = new MySqlParameter()
        //    {
        //        ParameterName = "@SeasonNumber",
        //        Value = Serie.seasonNumber
        //    };
        //    cmd.Parameters.Add(parSeasonNumber);
        //    MySqlParameter parShowName = new MySqlParameter()
        //    {
        //        ParameterName = "@ShowName",
        //        Value = Serie.name
        //    };
        //    cmd.Parameters.Add(parShowName);
        //    MySqlParameter parGenres = new MySqlParameter()
        //    {
        //        ParameterName = "@Genres",
        //        Value = Serie.genres
        //    };
        //    cmd.Parameters.Add(parGenres);
        //    MySqlParameter parStatus = new MySqlParameter()
        //    {
        //        ParameterName = "@Status",
        //        Value = Serie.status
        //    };

        //    SQL += " WHERE ID = '" + Serie.DBID.ToString() + "'";
        //}



        public List<CLEpisode> GetEpisodes(CLSerie serie, int startEpisode, int endEpisode)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
            List<CLEpisode> CLEpisodes = new List<CLEpisode>();
            int Episodes = AmountBetweenNumbers(startEpisode, endEpisode);
            int RunXTimes = AmountOfTimes(Episodes);
            int offset;
            if (startEpisode == 0)
                offset = 0;
            else
                offset = startEpisode - 1;

            for (int i = 0; RunXTimes > i; i++)
            {//There are a bunch of integers going up and down, look into it and figure it out if nessecary. That will be just as hard as me trying to explain wtf is 
                //going on.
                int Limitint = LowerIntTo20(Episodes);
                Episodes -= Limitint;
                string Link = serie.linkToEpisodes + "?&[page]offset=" + offset + "&[page]limit=" + Limitint.ToString();
                using (HttpResponseMessage response = client.GetAsync(Link).Result)
                {
                    offset += Limitint;
                    if (response.IsSuccessStatusCode)
                    {
                        Task<string> jsonString = response.Content.ReadAsStringAsync();
                        string ree = jsonString.Result;
                        JToken allEpisodesToken = JObject.Parse(ree).SelectToken("data");
                        JArray AllEpisodesJArray = (JArray)allEpisodesToken;
                        foreach (JObject episode in AllEpisodesJArray)
                        {
                            JToken jTokenEpisode = episode.SelectToken("attributes");
                            JObject jObjectEpisode = (JObject)jTokenEpisode;
                            CLEpisode add = Newtonsoft.Json.JsonConvert.DeserializeObject<CLEpisode>(jObjectEpisode.ToString());
                            add.showName = serie.name;
                            add.linkToEpisode = Link;
                            CLEpisodes.Add(add);
                        }
                    }
                }
            }
            return CLEpisodes;
        }

        int AmountBetweenNumbers(int one, int two)
        {
            if (one != 0)
                return (two - one) + 1;
            else
                return two - one;
        }

        int AmountBetweenNumbersMax20(int one, int two)
        {
            int ToReturn = 0;
            if (one != 0)
                ToReturn = (two - one) + 1;
            else
                ToReturn = two - one;
            if (ToReturn > 20)
                return 20;
            else
                return ToReturn;
        }

        int AmountOfTimes(int runs)
        {
            int CheckInt = runs;
            if (runs < 20)
            {
                return 1;
            }
            else if (runs > 20)
            {
                for (int i = 0; CheckInt > i; i++)
                {
                    runs -= 20;
                    if (runs <= 0)
                        return i + 1;
                }
            }

            return -1;
        }

        int LowerIntTo20(int i)
        {
            if (i > 20)
                return 20;
            else
                return i;
        }
    }
}
