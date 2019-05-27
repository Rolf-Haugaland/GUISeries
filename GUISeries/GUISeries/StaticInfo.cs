using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GUISeries
{
    public static class StaticInfo
    {
        static StaticInfo()
        {
            Thread updateDB = new Thread(KeepUpdated);
            updateDB.IsBackground = true;
            updateDB.Start();
        }

        public static Database CurrentDatabase;
        public static string DatabaseConfPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Databases.json";
        public static string FolderPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\";
        public static string SettingsPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Settings.json";
        public static string CheckSeriesPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\ToCheck.json";
        public static string LocalSeriesPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Series\\";
        public static string FuncDatabasesPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\FunctionalDatabases.json";
        public static string NonFuncDatabasesPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\NonFunctionalDatabases.json";

        /// <summary>
        /// Uses UpdateFiles(); and //UpdateDB(); to keep the local files and database updated. 
        /// </summary>
        private static void KeepUpdated()
        {
            bool first = true;
        loop:
            if(!first)
                Thread.Sleep(TimeSpan.FromMinutes(2));
            first = false;
            DateTime lastCheck = new DateTime();
            if (!File.Exists(FolderPath + "LastCheck.txt"))
                File.Create(FolderPath + "LastCheck.txt").Close();
            string strLastCheck = File.ReadAllText(FolderPath + "LastCheck.txt");
            if (DateTime.TryParse(strLastCheck, out DateTime result))
            {
                if (result != null)
                    lastCheck = result;
            }
            if (lastCheck < DateTime.Now.AddDays(-1))
            {
                UpdateFiles();
                //UpdateDB(); a few functions needs to be updated. You dont nessecarily need to get a finisehd series since the data will update weekly.
                //So you want it to update daily from the API, not wait all the way until it is finished. Also configurationmanager.UpdateDBEntry(CLSerie) is not finished. 
            }
            else
                goto loop;
        }

        /// <summary>
        /// Pulls episodes that has a status that is not finished from the database and checks if there is an updated version and updates them.
        /// </summary>
        private static void UpdateDB(string NOTREADY)
        {
        loop2:
            Thread.Sleep(100);
            if (CurrentDatabase == null)
                goto loop2;
            DatabaseConfiguration dbconf = new DatabaseConfiguration();
            MySqlConnection con = new MySqlConnection(dbconf.GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand("SELECT * From Series where NOT Status = 'finished' OR Status is NULL", con);

            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            List<CLSerie> OutdatedEpisodes = new List<CLSerie>();

            while(reader.Read())
            {
                CLSerie OutdatedEpisode = new CLSerie()
                {
                    episodeName = reader["Name"].ToString(),
                    episodeCount = (int)reader["EpisodeCount"],
                    ageRating = reader["AgeRating"].ToString(),
                    NSFW = Convert.ToBoolean(reader["NSFW"]),
                    synopsis = reader["Synopsis"].ToString(),
                    totalLength = (int)reader["TotalShowLength"],
                    length = (int)reader["Length"],
                    EpisodeNumber = (int)reader["EpisodeNumber"],
                    seasonNumber = reader["SeasonNumber"].ToString(),
                    showName = reader["ShowName"].ToString(),
                    genres = reader["Genres"].ToString().Split(',').ToList(),
                    name = reader["ShowName"].ToString(),
                    DBID = (int)reader["ID"]
                };
                OutdatedEpisodes.Add(OutdatedEpisode);
            }
            con.Close();
            OutdatedEpisodes.RemoveAll(x => x.status != "finished");
            if (OutdatedEpisodes.Count == 0)
                return;

            foreach(CLSerie serie in OutdatedEpisodes)
            {
                CLSerie Finished = GetFinishedSerie(serie.showName);
                if (Finished != null)
                {
                    ConfigurationManager manager = new ConfigurationManager();
                    manager.UpdateDBEntry(Finished);
                }
            }
        }

        /// <summary>
        /// Returns the finished version of the series if it exists. If no series with status=finished could be found, it returns null.
        /// </summary>
        /// <param name="SerieName">The name of the series</param>
        /// <returns></returns>
        public static CLSerie GetFinishedSerie(string SerieName)
        {
            ConfigurationManager manager = new ConfigurationManager();

            if(File.Exists(LocalSeriesPath + SerieName + ".json"))
            {
                string JsonString = File.ReadAllText(LocalSeriesPath + SerieName + ".json");

                CLSerie Serie = manager.GetSeriesFromJson(JObject.Parse(JsonString));
                if (Serie.status == "finished")
                {
                    return Serie;
                }
            }
            else
            {
                string JStrSerie = manager.GetOneSerie(SerieName);
                CLSerie Serie = manager.GetSeriesFromJson((JObject)JObject.Parse(JStrSerie)["data"][0]);
                if (Serie.status == "finished")
                    return Serie;
            }
            return null;
        }

        /// <summary>
        /// updates the local files
        /// </summary>
        private static void UpdateFiles()
        {
            string APath = LocalSeriesPath;
            JArray jArray = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(StaticInfo.CheckSeriesPath));
            ConfigurationManager manager = new ConfigurationManager();
            if (jArray != null)
            {
                foreach (JToken j in jArray)
                {
                    string fileContent = File.ReadAllText(APath + j.ToString() + ".json");
                    JObject JFile = (JObject)JsonConvert.DeserializeObject(fileContent);
                    string NameFromFile = JFile.SelectToken("attributes.canonicalTitle").ToString();
                    string strSerieFromAPI = manager.GetOneSerie(NameFromFile);
                    JObject SerieFromAPI = (JObject)JsonConvert.DeserializeObject(strSerieFromAPI);
                    JArray JONameFromAPI = (JArray)SerieFromAPI["data"];
                    JObject JONameFromAPI2 = (JObject)JONameFromAPI.First;
                    string NameFromAPI = JONameFromAPI2.SelectToken("attributes.canonicalTitle").ToString();
                    if (NameFromFile == NameFromAPI)
                    {
                        File.WriteAllText(FolderPath + "Series\\" + NameFromAPI + ".json", JONameFromAPI2.ToString());
                        string status = JONameFromAPI2.SelectToken("attributes.status").ToString();
                        if (status == "Finished")
                        {
                            string jsonstr = File.ReadAllText(CheckSeriesPath);
                            var x = JsonConvert.DeserializeObject(jsonstr);
                            //remove and write to file
                        }
                    }
                }
            }
            File.WriteAllText(FolderPath + "LastCheck.txt", DateTime.Now.ToString());
        }
    }
}
