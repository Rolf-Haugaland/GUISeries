using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUISeries
{
    public static class StaticInfo
    {
        public static Database CurrentDatabase;
        public static string DatabaseConfPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Databases.txt";
        public static string FolderPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\";
        public static string SettingsPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Settings.txt";
    }
}
