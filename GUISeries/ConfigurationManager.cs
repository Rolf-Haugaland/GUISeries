using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MySql.Data.MySqlClient;

namespace GUISeries
{
    class ConfigurationManager
    {
        private List<List<string>> GetAllSettingsBP(List<string> Settings)
        {
            List<List<string>> ACSettings = new List<List<string>>();

            foreach (string s in Settings)
            {
                for (int i = 0; s.Length > i; i++)
                {
                    if (s[i] == '#' && s[i + 1] == ':')
                    {
                        List<string> Items = new List<string>();
                        Items.Add(s.Substring(0, i));
                        Items.Add(s.Substring(i + 2, s.Length - (i + 2)));
                        ACSettings.Add(Items);
                    }
                }
            }
            return ACSettings;
        }

        public List<List<string>> GetAllSettings()
        {
            List<string> Settings = new List<string>(File.ReadAllLines("Settings.txt"));

            return GetAllSettingsBP(Settings);
        }

        public List<string> GetAllSettingsWithoutValues()
        {
            List<string> Settings = new List<string>(File.ReadAllLines("Settings.txt"));

            List<List<string>> ACSettings = GetAllSettingsBP(Settings);

            List<string> AllPresentSettings = ACSettings.SelectMany(x => x).ToList();
            bool EveryOther = false;

            for (int i = 0; AllPresentSettings.Count > i; i++)
            {
                if (EveryOther)
                    AllPresentSettings[i] = "#remove me#";
                EveryOther = !EveryOther;
            }

            var c = AllPresentSettings.RemoveAll(x => x == "#remove me#");

            return AllPresentSettings;
        }

        public void AddSetting(string setting, string value)
        {
            List<string> Settings = new List<string>(File.ReadAllLines("Settings.txt"));

            List<List<string>> ACSettings = GetAllSettingsBP(Settings);

            List<string> NewSetting = new List<string>();
            NewSetting.Add(setting);
            NewSetting.Add(value);

            ACSettings.Add(NewSetting);

            string[] ToFile = new string[ACSettings.Count];
            for (int i = 0; ACSettings.Count > i; i++)
            {
                ToFile[i] = ACSettings[i][0] + "#:" + ACSettings[i][1];
            }
            File.WriteAllLines("Settings.txt", ToFile);
        }

        public void SetSetting(string setting, string value)
        {
            List<string> Settings = new List<string>(File.ReadAllLines("Settings.txt"));
            List<List<string>> ACSettings = GetAllSettingsBP(Settings);

            int index = GetIndex(ACSettings, setting);
            if(index == -1)
                throw new Exception("SetSetting method in SetingFileManager did not find setting(GetIndex returned -1)");
            ACSettings[index][1] = value;
            string[] ToFile = new string[ACSettings.Count];
            for (int i = 0; ACSettings.Count > i; i++)
            {
                ToFile[i] = ACSettings[i][0] + "#:" + ACSettings[i][1];
            }
            File.WriteAllLines("Settings.txt", ToFile);
        }

        /// <summary>
        /// Returns the result of a setting. Searching for a setting will return its value only. Example: GetSetting("Default Database") will return 1 or 2 or 3 etc, 
        /// depending on what the default database is. It only returns the value of the setting. 
        /// </summary>
        /// <param name="setting">The setting to look for. Example: Default Database or Database1 Name</param>
        /// <returns></returns>
        public string GetSetting(string setting)
        {
            List<string> Settings = new List<string>(File.ReadAllLines("Settings.txt"));
            List<List<string>> ACSettings = GetAllSettingsBP(Settings);

            int index = GetIndex(ACSettings, setting);
            if (index == -1)
                return "";
            else
                return ACSettings[index][1];
        }

        private int GetIndex(List<List<string>> ACSetting, string Setting)
        {
            for (int i = 0; ACSetting.Count > i; i++)
            {
                if (ACSetting[i].Contains(Setting, StringComparer.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        public bool CheckDatabaseFromFile()
        {
            ConfigurationManager manager = new ConfigurationManager();
            List<List<string>> AllSettings = manager.GetAllSettings();

            string DefaultDatabase = GetSetting("Default Database");
            string DatabaseName = "";
            string DatabaseIP = "";
            string DatabaseUname = "";
            string DatabasePW = "";
            string DatabasePort = "";

            foreach (List<string> ls in AllSettings)
            {
                if (ls[0].Equals("Database" + DefaultDatabase + " Name", StringComparison.CurrentCultureIgnoreCase))
                    DatabaseName = ls[1];
                else if (ls[0].Equals("Database" + DefaultDatabase + " IP", StringComparison.CurrentCultureIgnoreCase))
                    DatabaseIP = ls[1];
                else if (ls[0].Equals("Database" + DefaultDatabase + " Username", StringComparison.CurrentCultureIgnoreCase))
                    DatabaseUname = ls[1];
                else if (ls[0].Equals("Database" + DefaultDatabase + " Password", StringComparison.CurrentCultureIgnoreCase))
                    DatabasePW = ls[1];
                else if (ls[0].Equals("Database" + DefaultDatabase + " Port", StringComparison.CurrentCultureIgnoreCase))
                    DatabasePort = ls[1];
            }

            MySqlConnection con = new MySqlConnection("Server=" + DatabaseIP + ";Database=" + DatabaseName + ";User Id=" + DatabaseUname + ";Password = " + DatabasePW + ";Port=" + DatabasePort + "; ");
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
