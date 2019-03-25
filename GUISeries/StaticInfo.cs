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
        public static string path = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries\\Settings.txt";
        public static string FolderPath = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Documents\\GUISeries";
        public static string Connectionstring = GetConnectionstring();
        private static string GetConnectionstring()
        {
            return "Server=" + CurrentDatabase.DatabaseIP + ";Port=" + CurrentDatabase.DatabasePort + ";Database=" + CurrentDatabase.DatabaseName + ";Uid=" +
                "" + CurrentDatabase.DatabaseUname + ";Pwd=" + CurrentDatabase.DatabasePW + ";";
        }
    }
}
