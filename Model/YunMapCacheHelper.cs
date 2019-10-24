using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.Model
{
    public static class YunMapCacheHelper
    {
        /// <summary>
        /// 获取实例全名
        /// </summary>
        /// <param name="mLastDateStr"></param>
        /// <param name="dbName"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static string getInstanceName(string mLastDateStr, string dbName, long maxCount)
        {
            string dataKey = string.Format("{0}_{1}_{2}", dbName, maxCount, mLastDateStr);
            return dataKey;
        }

        /// <summary>
        /// 获取缓存根目录
        /// </summary>
        /// <returns></returns>
        public static string getMapCacheRoot()
        {
            string sRet = string.Empty;

            sRet = AppDomain.CurrentDomain.BaseDirectory + "yun_data";

            if (!Directory.Exists(sRet))
            {
                Directory.CreateDirectory(sRet);
            }

            sRet = Path.GetFullPath(sRet);

            return sRet;
        }


    }
}
