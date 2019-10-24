using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.StructData
{
    public struct OrderNode : IFoxxData
    {

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string foxxcode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string code;
        public int date;
        public int time;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string function_code;

        public int order;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string order_kind;

        public int price;
        public int vol;

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
