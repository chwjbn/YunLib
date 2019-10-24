using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YunLib
{
    public class LogWriter
    {
        private static object lockObjet = new object();


        public static void Log(string data)
        {
            lock(lockObjet)
            {
                string filePath = String.Format("{0}log", AppDomain.CurrentDomain.BaseDirectory);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = string.Format("{0}/YunLib.{1}.log",filePath,DateTime.Now.ToString("yyyyMMdd"));
                data = string.Format("[{0}]{1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),data,Environment.NewLine);
                File.AppendAllText(filePath, data);

            }
        }

        public static void Log(string data, params object[] pms)
        {
            data = string.Format(data,pms);
            Log(data);
        }

        public static void Debug(string data)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "debug.lock";
            if (!File.Exists(filePath))
            {
                return;
            }

            Log(data);
        }

        public static void Debug(string data, params object[] pms)
        {
            data = string.Format(data, pms);
            Debug(data);
        }

    }
}
