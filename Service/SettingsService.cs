using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JD_Proc.Service
{
    public class SettingsService
    {
        string _path = "C:\\JD\\settings\\settings.ini";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public void Write(string section, string key, string txt)
        {
            WritePrivateProfileString(section, key, txt, _path);
        }

        public string Read(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(256);
            int i = GetPrivateProfileString(Section, Key, "", temp, 256, _path);
            if (temp != null && temp.Length > 0) return temp.ToString();
            else return "null";
        }
    }
}
