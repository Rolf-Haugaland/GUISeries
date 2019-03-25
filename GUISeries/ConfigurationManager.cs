using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;

namespace GUISeries
{
    class ConfigurationManager
    {
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

            return Episodes.Max();
        }

        /// <summary>
        /// returns all the databases in the Settings.txt working or not. GetFunctionalDatabases() only returns functional databases
        /// </summary>
        /// <returns>All databases in the Settings.txt file, connectable or not</returns>
        public List<Database> GetDatabases()
        {
            string allDatabases = File.ReadAllText(StaticInfo.path);

            if (!string.IsNullOrWhiteSpace(allDatabases))
            {
                return GetDBFromJsonString(allDatabases);
            }

            return new List<Database>();
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
            File.WriteAllText(StaticInfo.path, data);
        }

        public void RemoveDatabase(Database database)
        {
            List<Database> databases = new List<Database>();
            string currentDatabases = File.ReadAllText(StaticInfo.path);
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

            string currentDatabases = File.ReadAllText(StaticInfo.path);
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
            File.WriteAllText(StaticInfo.path, data);
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
    }
}
