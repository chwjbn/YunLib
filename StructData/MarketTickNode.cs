using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.StructData
{
    public struct MarketTickNode: IFoxxData
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


        public long buy1;

        public long buy2;

        public long buy3;

        public long buy4;

        public long buy5;

        public long buy6;

        public long buy7;

        public long buy8;

        public long buy9;

        public long buy10;


        public long buy1_count;

        public long buy2_count;

        public long buy3_count;

        public long buy4_count;

        public long buy5_count;

        public long buy6_count;

        public long buy7_count;

        public long buy8_count;

        public long buy9_count;

        public long buy10_count;



        public long sell1;

        public long sell2;

        public long sell3;

        public long sell4;

        public long sell5;

        public long sell6;

        public long sell7;

        public long sell8;

        public long sell9;

        public long sell10;


        public long sell1_count;

        public long sell2_count;

        public long sell3_count;

        public long sell4_count;

        public long sell5_count;

        public long sell6_count;

        public long sell7_count;

        public long sell8_count;

        public long sell9_count;

        public long sell10_count;

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
