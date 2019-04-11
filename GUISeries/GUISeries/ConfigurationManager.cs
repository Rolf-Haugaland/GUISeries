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
            MySqlConnection con = new MySqlConnection(GetConnectionstring());
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
            MySqlConnection con = new MySqlConnection(GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand();
            string SQL = "";//Fix insert or update
                            //if(InstOrUpdate == "insert")
            SQL = "Insert into Series(Name,EpisodeCount,AgeRating,NSFW,Synopsis,TotalShowLength,Length,EpisodeNumber,SeasonNumber,ShowName,TimeStamp,Genres,Status) VALUES(";
            int i = 0;
            int count = 1;
            foreach (CLEpisode episode in episodes)
            {
                i++;
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

        public void UpdateDBEntry(CLSerie Serie)
        {
            MySqlConnection con = new MySqlConnection(GetConnectionstring());
            MySqlCommand cmd = new MySqlCommand();
            string SQL = "UPDATE Series(Name,EpisodeCount,AgeRating,NSFW,Synopsis,TotalShowLength,Length,EpisodeNumber,SeasonNumber,ShowName,TimeStamp,Genres,Status) VALUES(";

            SQL += " WHERE ID = '" + Serie.DBID.ToString() + "'";
        }

        /// <summary>
        /// Sets the default database to the database provided. Also sets the previous default database to no longer be a default database.
        /// Only takes FunctionalDatabases.json into consideration.
        /// </summary>
        /// <param name="database"></param>
        private void SetDefaultDB(Database database, string DBFilePath)
        {
            List<Database> databases = GetDBFromFile(DBFilePath);
            List<Database> defaultDB = databases.FindAll(x => x.DefaultDB);
            if (defaultDB.Count > 1)//Arguabely bad error handeling, will look into prompting the user to resolve this later, but the exam is closing in, it is not priority work.
                throw new Exception("SetDefaultDB found that there exists more than 1 default database, this is not supposed to happen");
            else if (defaultDB.Count == 1)
            {
                //If the databases are equal then might as well return since that database is already the default one. 
                if (!DatabaseCheckEqual(database, defaultDB[0]))
                {
                    int RemoveAt = databases.FindIndex(x => x.DefaultDB);
                    databases.RemoveAt(RemoveAt);
                    defaultDB[0].DefaultDB = false;
                    databases.Add(defaultDB[0]);

                    RemoveAt = 0;

                    foreach (Database db in databases)
                    {
                        RemoveAt++;
                        if (DatabaseCheckEqual(database, db))
                        {
                            break;
                        }
                    }

                    databases.RemoveAt(RemoveAt - 1);

                    database.DefaultDB = true;

                    databases.Add(database);
                    OverWriteDatabases(databases, DBFilePath);
                }
                else//Futile return
                    return;
            }
            else if (defaultDB.Count == 0)
            {
                for(int i = 0; databases.Count > i; i++)
                {
                    if(DatabaseCheckEqual(databases[i], database))
                    {
                        databases.RemoveAt(i);
                    }
                }
                database.DefaultDB = true;
                databases.Add(database);
                OverWriteDatabases(databases, DBFilePath);
            }
        }

        public void ChangeDefaultDB(Database database)
        {
            SetDefaultDB(database, StaticInfo.NonFuncDatabasesPath);
            SetDefaultDB(database, StaticInfo.FuncDatabasesPath);
            SetDefaultDB(database, StaticInfo.DatabaseConfPath);
        }

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
                using (HttpResponseMessage response = client.GetAsync(serie.linkToEpisodes + "?&[page]offset=" + offset + "&[page]limit=" + Limitint.ToString()).Result)
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

        public List<Database> GetDBFromJsonString(string JsonString)
        {
            List<Database> databases = new List<Database>();
            JArray jArray = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(JsonString);
            foreach (JObject DB in jArray)
            {
                databases.Add(new Database()
                {
                    DatabaseName = DB.SelectToken("DatabaseName").ToString(),
                    DatabaseIP = DB.SelectToken("DatabaseIP").ToString(),
                    DatabasePort = DB.SelectToken("DatabasePort").ToString(),
                    DatabasePW = DB.SelectToken("DatabasePW").ToString(),
                    DatabaseUname = DB.SelectToken("DatabaseUname").ToString(),
                    DefaultDB = Convert.ToBoolean(DB.SelectToken("DefaultDB").ToString())
                });
            }
            return databases;
        }

        public void OverWriteDatabases(List<Database> databases, string path)
        {
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(databases);
            File.WriteAllText(path, data);
        }

        public void RemoveDatabase(Database database, string path)
        {
            List<Database> databases = new List<Database>();
            string currentDatabases = File.ReadAllText(path);
            if (!string.IsNullOrWhiteSpace(currentDatabases))
                databases = GetDBFromJsonString(currentDatabases);
            //Loops through all the databases to find the one to remove. Once it is found it removes it and stops the function.
            for (int i = 0; databases.Count > i; i++)
            {
                if (DatabaseCheckEqual(databases[i], database))
                {
                    databases.RemoveAt(i);
                    OverWriteDatabases(databases, path);
                    return;
                }
            }
        }

        public int AddDatabase(Database database)
        {
            //The list that will be written to the file
            List<Database> databases = new List<Database>();

            string currentDatabases = File.ReadAllText(StaticInfo.DatabaseConfPath);
            if (!string.IsNullOrWhiteSpace(currentDatabases))
            {
                databases = GetDBFromJsonString(currentDatabases);
            }

            databases.Add(database);

            //If there are more than 1 databases in the databases list that means there is already(at least) one database and the user is adding another one
            if (databases.Count > 1)
            {
                //This loop checks each and every element in the databases and if it is a duplicate it returns 1, which indicates failure due to duplicate database
                for (int i = 0; databases.Count - 1 > i; i++)
                {
                    if (DatabaseCheckEqual(databases[i], databases.Last()))
                        return 1;
                }
            }

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(databases);
            File.WriteAllText(StaticInfo.DatabaseConfPath, data);
            return 0;
        }

        public int AddNonFuncDB(Database database)
        {
            //The list that will be written to the file
            List<Database> databases = new List<Database>();

            string currentDatabases = File.ReadAllText(StaticInfo.NonFuncDatabasesPath);
            if (!string.IsNullOrWhiteSpace(currentDatabases))
            {
                databases = GetDBFromJsonString(currentDatabases);
            }

            databases.Add(database);

            //If there are more than 1 databases in the databases list that means there is already(at least) one database and the user is adding another one
            if (databases.Count > 1)
            {
                //This loop checks each and every element in the databases and if it is a duplicate it returns 1, which indicates failure due to duplicate database
                for (int i = 0; databases.Count - 1 > i; i++)
                {
                    if (DatabaseCheckEqual(databases[i], databases.Last()))
                        return 1;
                }
            }

            string data = Newtonsoft.Json.JsonConvert.SerializeObject(databases);
            File.WriteAllText(StaticInfo.NonFuncDatabasesPath, data);
            return 0;
        }

        public bool DatabaseCheckEqual(Database database1, Database database2)
        {
            //The counter for how many equal items there are
            int count = 0;

            if (database1.DatabaseName == database2.DatabaseName)
                count++;
            if (database1.DatabaseIP == database2.DatabaseIP)
                count++;
            if (database1.DatabasePort == database2.DatabasePort)
                count++;
            if (database1.DatabasePW == database2.DatabasePW)
                count++;
            if (database1.DatabaseUname == database2.DatabaseUname)
                count++;

            //If all 5 items are equal, then they are 2 identical databases
            if (count == 5)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns all the functional databases in the Settings.txt file. If there is a database has default boolean set to true, it will be at the start of the list.
        /// Returns a new List<Database> if none are found
        /// </summary>
        /// <returns>All functional databases in Settings.txt. Default DB at start of list. Returns new List<Database> if none are found</returns>
        public List<Database> GetFunctionalDatabases()
        {
            List<Database> databases = new List<Database>();

            string allDatabases = File.ReadAllText(StaticInfo.FuncDatabasesPath);

            if (!string.IsNullOrWhiteSpace(allDatabases))
            {
                databases = GetDBFromJsonString(allDatabases);
            }

            List<Database> CheckedDB = new List<Database>();
            foreach (Database database in databases)
            {
                if (CheckDatabaseConnection(database))
                    CheckedDB.Add(database);
            }
            Database DefaultDB = CheckedDB.Find(x => x.DefaultDB == true);
            if (DefaultDB != null)
            {
                CheckedDB.Remove(DefaultDB);
                CheckedDB.Insert(0, DefaultDB);
            }
            return CheckedDB;
        }

        public List<Database> GetDBFromFile(string path)
        {
            List<Database> databases = new List<Database>();

            string allDatabases = File.ReadAllText(path);

            if (!string.IsNullOrWhiteSpace(allDatabases))
            {
                databases = GetDBFromJsonString(allDatabases);
            }
            Database DefaultDB = databases.Find(x => x.DefaultDB == true);
            if (DefaultDB != null)
            {
                databases.Remove(DefaultDB);
                databases.Insert(0, DefaultDB);
            }
            return databases;
        }

        /// <summary>
        /// Attemps to open a mysql connection with MysqlConnection, returns true if succsessful and false if failure.
        /// </summary>
        /// <param name="database">The database to test</param>
        /// <returns></returns>
        public bool CheckDatabaseConnection(Database database)
        {
            MySqlConnection con = new MySqlConnection("Server=" + database.DatabaseIP + ";Port=" + database.DatabasePort + ";Database=" + database.DatabaseName + ";Uid=" +
                database.DatabaseUname + ";Pwd=" + database.DatabasePW + ";connection timeout=2;");
            try
            {
                con.Open();
                con.Close();
                return true;
            }
            catch
            {
                con.Close();
                return false;
            }
        }

        public bool CheckIfTableExists(Database database)
        {
            MySqlConnection con = new MySqlConnection("Server=" + database.DatabaseIP + ";Port=" + database.DatabasePort + ";Database=" + database.DatabaseName + ";Uid=" +
                database.DatabaseUname + ";Pwd=" + database.DatabasePW + ";");
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES LIKE 'Series'", con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> Tables = new List<string>();
            while(reader.Read())
            {
                Tables.Add(reader[0].ToString());
            }
            con.Close();
            if (Tables.Contains("Series"))
                return true;
            else
                return false;
        }

        public bool CheckIfTableIsValid(Database database)
        {
            MySqlConnection con = new MySqlConnection("Server=" + database.DatabaseIP + ";Port=" + database.DatabasePort + ";Database=" + database.DatabaseName + ";Uid=" +
    database.DatabaseUname + ";Pwd=" + database.DatabasePW + ";");
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Series", con);

            con.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                for(int i = 0; 15 > i; i++)
                {
                    try
                    {
                        reader[i].ToString();
                        if(i == 14)
                        {
                            try
                            {
                                reader[15].ToString();
                            }
                            catch
                            {
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            con.Close();
            return true;
        }

        /// <summary>
        /// THIS WILL OVERWRITE THE CURRENT Series TABLE! If this method is called, it will delete the old table(if it exists) and 
        /// create a new fresh one. If it doesent already exist, it will just create one
        /// </summary>
        public void CreateTable(Database database, string ThisWillDELETEAllDataInSeriesTbl)
        {
            MySqlConnection con = new MySqlConnection("Server=" + database.DatabaseIP + ";Port=" + database.DatabasePort + ";Database=" + database.DatabaseName + ";Uid=" +
database.DatabaseUname + ";Pwd=" + database.DatabasePW + ";");
            MySqlCommand cmd = new MySqlCommand("DROP TABLE IF EXISTS Series", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            cmd = new MySqlCommand("CREATE TABLE `Series`(`ID` int(11) NOT NULL AUTO_INCREMENT,`Name` mediumtext,`EpisodeCount` int(11) DEFAULT NULL,`AgeRating` varchar(45) DEFAULT NULL,`NSFW` varchar(45) DEFAULT NULL,`Synopsis` mediumtext,`TotalShowLength` int(11) DEFAULT NULL,`Length` int(11) DEFAULT NULL,`EpisodeNumber` int(11) DEFAULT NULL,`SeasonNumber` int(11) DEFAULT NULL,`ShowName` mediumtext,`TimeStamp` datetime DEFAULT NULL,`Genres` text,`Status` tinytext,`UploadTimeStamp` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,PRIMARY KEY (`ID`)) ENGINE=InnoDB AUTO_INCREMENT=217 DEFAULT CHARSET=utf8;"
                , con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public string GetConnectionstring()
        {
            ConfigurationManager manager = new ConfigurationManager();
            if (StaticInfo.CurrentDatabase != null)
                if (manager.CheckDatabaseConnection(StaticInfo.CurrentDatabase))
                    return "Server=" + StaticInfo.CurrentDatabase.DatabaseIP + ";Port=" + StaticInfo.CurrentDatabase.DatabasePort + ";Database=" + StaticInfo.CurrentDatabase.DatabaseName + ";Uid=" +
                        "" + StaticInfo.CurrentDatabase.DatabaseUname + ";Pwd=" + StaticInfo.CurrentDatabase.DatabasePW + ";";
                else
                    return "";
            else
                return "";
        }

        public void AddFuncDBToFile(Database database)
        {
            List<Database> FuncDBToFile = new List<Database>();
            string jsonstring = File.ReadAllText(StaticInfo.FuncDatabasesPath);
            if (!string.IsNullOrWhiteSpace(jsonstring))
            {
                FuncDBToFile = GetDBFromJsonString(jsonstring);
            }
            FuncDBToFile.Add(database);

            OverWriteDatabases(FuncDBToFile, StaticInfo.FuncDatabasesPath);
        }
    }
}
