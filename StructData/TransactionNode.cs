using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.StructData
{
    public struct TransactionNode: IFoxxData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string foxxcode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string code;

        public int date;
        public int time;
        public int ask_order;
        public int bid_order;
        public int bs_flag;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string function_code;

        public int index;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string order_kind;

        public int price;
        public long money;
        public long vol;

        public bool HasFoxxCode()
        {
            return DataHelper.IsFoxxcode(foxxcode);
        }

        public string PrintData()
        {
            return string.Format("{0}_{1}_{2}",foxxcode,date,time);
        }
    }
}
