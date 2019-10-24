using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YunLib
{
    public class DataHelper
    {
        public static byte[] StructToBytes<T>(T structObj)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }


        public static T BytesToStruct<T>(byte[] bytes)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);

                var structObj=Marshal.PtrToStructure(buffer, typeof(T));

                return (T)structObj;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public static string ChartArrayToString(char[] data)
        {
            string sRet = string.Empty;

            StringBuilder sBuil = new StringBuilder();
            sBuil.Append(data);

            sRet = sBuil.ToString();

            return sRet;
        }

        public static int StructSize<T>()
        {
            int nSize = 0;

            nSize = Marshal.SizeOf(typeof(T));

            return nSize;
        }

        public static bool IsFoxxcode(string foxxcode)
        {
            bool bRet = false;

            if (string.IsNullOrEmpty(foxxcode))
            {
                return bRet;
            }

            foxxcode = foxxcode.ToLower();

            if (!foxxcode.Contains(".sh")&&!foxxcode.Contains(".sz"))
            {
                return bRet;
            }

            bRet = true;

            return bRet;
        }


        public static DateTime GetTodayTimeFromStockTime(int dateBase,int timeBase)
        {
            int dateBaseYear = dateBase / 10000;
            dateBase = dateBase - dateBaseYear * 10000;

            int dateBaseMonth = dateBase / 100;

            int dateBaseDay = dateBase - dateBaseMonth * 100;

            DateTime nowTime = DateTime.Now;

            int timeBaseHour = timeBase / 10000000;

            timeBase = timeBase - timeBaseHour * 10000000;
            int timeBaseMin = timeBase / 100000;

            timeBase = timeBase - timeBaseMin * 100000;
            int timeBaseSec = timeBase / 1000;

            int timeBaseMill = timeBase - timeBaseSec * 1000;

            DateTime iDateTime = new DateTime(dateBaseYear, dateBaseMonth, dateBaseDay, timeBaseHour, timeBaseMin, timeBaseSec, timeBaseMill);

            return iDateTime;
        }

    }
}
