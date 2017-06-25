using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;


namespace gen_alg
{
    class FilesIni
    {
        const int size = 255;
        string path;     //имя файла

        //Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateString(string section, string key, string def, StringBuilder buffer, int size, string path);

        //Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WritePrivateString(string section, string key, string str, string path);

        public FilesIni()
        {
            path = "";
        }

        public FilesIni(string path)
        {
            this.path = path;
        }

        public string GetPrivateString(string key)
        {
            StringBuilder buffer = new StringBuilder(size);

            GetPrivateString("main", key, null, buffer, size, path);

            return buffer.ToString();
        }

        public void WritePrivateString(string key, string value)
        {
            WritePrivateString("main", key, value, path);
        }

        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
            }
        }


    }
}
