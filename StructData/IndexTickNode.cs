using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.StructData
{
    public struct IndexTickNode:IFoxxData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string foxxcode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string code;

        public int date;
        public int time;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string status;

        public int preclose;
        public int close;
        public int dopen;
        public int dhigh;
        public int dlow;
        public long vol;
        public long money;
        public long transnum;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string name;

        public long buyorder;
        public long sellorder;
        public int buyorder_aveprice;
        public int sellorder_aveprice;

        public bool HasFoxxCode()
        {
            return DataHelper.IsFoxxcode(foxxcode);
        }

        public string PrintData()
        {
            return string.Format("{0}_{1}_{2}", foxxcode, date, time);
        }
    }
}
