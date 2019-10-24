using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.StructData
{
    public struct OrderQueueNode : IFoxxData
    {

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string foxxcode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string code;

        public int date;
        public int time;

        // 明细个数，目前最大50档
        public int ABItems;

        //订单明细
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public int[] ABVolume;

        //订单数量
        public int Orders;

        //委托价格
        public int Price;

        //买卖方向('B':Bid 'A':Ask)
        public int Side;

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
