using System.Runtime.InteropServices;

namespace YunLib.StructData
{
    public struct L2TenTransactionNode:IFoxxData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string foxxcode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string code;
        public int date;
        public int time;
        public int status;
        public int bs_flag;
        public long vol;
        public long money;

        public bool HasFoxxCode()
        {
            return DataHelper.IsFoxxcode(this.foxxcode);
        }

        public string PrintData()
        {
            return string.Format("{0}_{1}_{2}", this.foxxcode, this.date, this.time);
        }
    }
}
