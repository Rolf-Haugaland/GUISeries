using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
            Database database = StaticInfo.CurrentDatabase;
            MySqlCommand cmd = new MySqlCommand("Select * from Series where Name = '" + SerieName + "'");
            MySqlConnection con = new MySqlConnection("Server = " + database.DatabaseIP + "; Port = " + database.DatabasePort + "; Database = " + database.DatabaseName + 
                "; Uid = " + database.DatabaseUname + ";Pwd = " + database.DatabasePW + ";");
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
        /// returns all the databases in the Settings.txt working or not. GetFunctionalDatabases() only returns functional databases
        /// </summary>
        /// <returns>All databases in the Settings.txt file, connectable or not</returns>
        public List<Database> GetDatabases()
        {
            string allDatabases = File.ReadAllText(StaticInfo.DatabaseConfPath);

            if (!string.IsNullOrWhiteSpace(allDatabases))
            {
                return GetDBFromJsonString(allDatabases);
            }

            return new List<Database>();
        }

        public List<CLEpisode> GetEpisodes(CLSerie serie, int startEpisode, int endEpisode)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
            List<CLEpisode> CLEpisodes = new List<CLEpisode>();
<<<<<<< HEAD:GUISeries/GUISeries/ConfigurationManager.cs
            int Episodes = AmountBetweenNumbers(startEpisode, endEpisode);
            int RunXTimes = AmountOfTimes(Episodes);
            int offset;
            if (startEpisode == 0)
                offset = 0;
            else
                offset = startEpisode - 1;
            for (int i = 0; RunXTimes > i; i++)
            {
                int Limitint = LowerIntTo20(endEpisode - startEpisode);
                using (HttpResponseMessage response = client.GetAsync(serie.linkToEpisodes + "?&[page]offset=" + offset + "&[page]limit=" + Limitint.ToString()).Result)
                {
                    offset += Limitint;//MIGHT WORK MIGHT NOT. TRY AND FIND OUT
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
=======
            if(endEpisode - startEpisode > 20)
            {
                for (int i = startEpisode; endEpisode - startEpisode > i; i += 20)
                {
                    using (HttpResponseMessage response = client.GetAsync(serie.linkToEpisodes + "?&[page]offset=" + i + "&[page]limit=" + LowerIntTo20(endEpisode - i).ToString()).Result)
                    {
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
>>>>>>> 7677029dd112041ed7ec998c9e35c6ef4fe9ef49:GUISeries/ConfigurationManager.cs
                        }
                    }
                }
            }
<<<<<<< HEAD:GUISeries/GUISeries/ConfigurationManager.cs
            return new List<CLEpisode>();
        }

        int AmountBetweenNumbers(int one, int two)
        {
            if (one != 0)
                return (two - one) + 1;
            else
                return two - one;
        }

        int AmountOfTimes(int runs)
        {
            if (runs < 20)
            {
                return 1;
            }
            else if (runs > 20)
            {
                for (int i = 0; runs > i; i++)
                {
                    runs -= 20;
                    if (runs <= 0)
                        return i;
                }
            }

            return -1;
=======
            else
            {

            }
            return new List<CLEpisode>();
>>>>>>> 7677029dd112041ed7ec998c9e35c6ef4fe9ef49:GUISeries/ConfigurationManager.cs
        }

        int LowerIntTo20(int i)
        {
            if (i > 20)
                return 20;
            else
                return i;
        }

        private List<Database> GetDBFromJsonString(string JsonString)
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

        public void OverWriteDatabases(List<Database> databases)
        {
            string data = Newtonsoft.Json.JsonConvert.SerializeObject(databases);
            File.WriteAllText(StaticInfo.DatabaseConfPath, data);
        }

        public void RemoveDatabase(Database database)
        {
            List<Database> databases = new List<Database>();
            string currentDatabases = File.ReadAllText(StaticInfo.DatabaseConfPath);
            if (!string.IsNullOrWhiteSpace(currentDatabases))
                databases = GetDBFromJsonString(currentDatabases);
            //Loops through all the databases to find the one to remove. Once it is found it removes it and stops the function.
            for (int i = 0; databases.Count > i; i++)
            {
                if (DatabaseCheckEqual(databases[i], database))
                {
                    databases.RemoveAt(i);
                    OverWriteDatabases(databases);
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
            if(databases.Count > 1)
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
        /// Returns all the functional databases in the Settings.txt file. Returns a new List<Database> if none are found
        /// </summary>
        /// <returns>All functional databases in Settings.txt. Returns new List<Database> if none are found</returns>
        public List<Database> GetFunctionalDatabases()
        {
            List<Database> databases = GetDatabases();
            List<Database> CheckedDB = new List<Database>();
            foreach(Database database in databases)
            {
                if (CheckDatabaseConnection(database))
                    CheckedDB.Add(database);
            }
            return CheckedDB;
        }

        /// <summary>
        /// Attemps to open a mysql connection with MysqlConnection, returns true if succsessful and false if failure.
        /// </summary>
        /// <param name="database">The database to test</param>
        /// <returns></returns>
        public bool CheckDatabaseConnection(Database database)
        {
            MySqlConnection con = new MySqlConnection("Server=" + database.DatabaseIP + ";Port=" + database.DatabasePort + ";Database=" + database.DatabaseName + ";Uid=" +
                "" + database.DatabaseUname + ";Pwd=" + database.DatabasePW + "; ");
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

        public string GetConnectionstring()
        {
            if (StaticInfo.CurrentDatabase != null)
                return "Server=" + StaticInfo.CurrentDatabase.DatabaseIP + ";Port=" + StaticInfo.CurrentDatabase.DatabasePort + ";Database=" + StaticInfo.CurrentDatabase.DatabaseName + ";Uid=" +
                "" + StaticInfo.CurrentDatabase.DatabaseUname + ";Pwd=" + StaticInfo.CurrentDatabase.DatabasePW + ";";
            else
                return "";
        }
    }
}
