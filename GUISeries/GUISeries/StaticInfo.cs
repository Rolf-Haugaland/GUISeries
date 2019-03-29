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
        public static Database CurrentDatabase;
        public static string DatabaseConfPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Databases.json";
        public static string FolderPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\";
        public static string SettingsPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Settings.json";
        public static string CheckSeriesPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\ToCheck.json";
        private static Thread updateDB = new Thread(updateDBFiles);
        private static void updateDBFiles()
        {
            DateTime lastCheck = new DateTime();
            if (!File.Exists(FolderPath + "LastCheck.txt"))
                File.Create(FolderPath + "LastCheck.txt");
            string strLastCheck = File.ReadAllText(FolderPath + "LastCheck.txt");
            if (DateTime.TryParse(strLastCheck, out DateTime result))
            {
                if (result != null)
                    lastCheck = result;
            }
        loop:
            Thread.Sleep(TimeSpan.FromMinutes(2));
            if (lastCheck < DateTime.Now.AddDays(-1))
            {
                UpdateFiles();
            }
            else
                goto loop;
        }

        private static void UpdateFiles()
        {
            string path = FolderPath;
            DirectoryInfo info = new DirectoryInfo(path + "\\Series");
            ConfigurationManager manager = new ConfigurationManager();
            foreach (FileInfo f in info.GetFiles())
            {
                string fileContent = File.ReadAllText(path + f.Name);
                JObject jObject = (JObject)JsonConvert.DeserializeObject(fileContent);
                string name = jObject.SelectToken("attributes.canonicalTitle").ToString();
                string strSerie = manager.GetOneSerie(name);
                JObject JSerie = (JObject)JsonConvert.DeserializeObject(strSerie);
                string jName = JSerie.SelectToken("attributes.canonicalTitle").ToString();
                if(name == jName)
                {

                }
            }
        }
    }
}
