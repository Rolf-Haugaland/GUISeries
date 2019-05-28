using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;

namespace GUISeries
{
    public class DatabaseConfiguration
    {
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
                for (int i = 0; databases.Count > i; i++)
                {
                    if (DatabaseCheckEqual(databases[i], database))
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
                }
            }
            OverWriteDatabases(databases, path);
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


        public bool CheckIfTableExists(Database database)
        {
            MySqlConnection con = new MySqlConnection("Server=" + database.DatabaseIP + ";Port=" + database.DatabasePort + ";Database=" + database.DatabaseName + ";Uid=" +
                database.DatabaseUname + ";Pwd=" + database.DatabasePW + ";");
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES LIKE 'Series'", con);
            con.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> Tables = new List<string>();
            while (reader.Read())
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

            while (reader.Read())
            {
                for (int i = 0; 15 > i; i++)
                {
                    try
                    {
                        reader[i].ToString();
                        if (i == 14)
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

        public string GetConnectionstring()
        {
            if (StaticInfo.CurrentDatabase != null)
                if (CheckDatabaseConnection(StaticInfo.CurrentDatabase))
                    return "Server=" + StaticInfo.CurrentDatabase.DatabaseIP + ";Port=" + StaticInfo.CurrentDatabase.DatabasePort + ";Database=" + StaticInfo.CurrentDatabase.DatabaseName + ";Uid=" +
                        "" + StaticInfo.CurrentDatabase.DatabaseUname + ";Pwd=" + StaticInfo.CurrentDatabase.DatabasePW + ";";
                else
                    return "";
            else
                return "";
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
    }
}
