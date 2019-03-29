using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Thread updateDB = new Thread(updateDBFiles);
            updateDB.IsBackground = true;
            updateDB.Start();
        }

        public static Database CurrentDatabase;
        public static string DatabaseConfPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Databases.json";
        public static string FolderPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\";
        public static string SettingsPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Settings.json";
        public static string CheckSeriesPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\ToCheck.json";
        public static string LocalSeriesPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Series\\";

        private static void updateDBFiles()
        {
        loop:
            Thread.Sleep(TimeSpan.FromMinutes(2));
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
            }
            else
                goto loop;
        }

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
                        string status = JONameFromAPI2.SelectToken("attributes.status").ToString();
                        if (status == "finished")
                        {
                            File.WriteAllText(FolderPath + "Series\\" + NameFromAPI + ".json", JONameFromAPI2.ToString());
                        }
                    }
                }
            }
            File.WriteAllText(FolderPath + "LastCheck.txt", DateTime.Now.ToString());
        }
    }
}
