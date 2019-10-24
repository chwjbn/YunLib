using System;
using System.Runtime.Serialization;
using YunLib;
using YunLib.StructData;

namespace YunLib.Model
{

    [DataContract]
    public class MarketSecModel:TickModel
    {
        [DataMember]
        public long buy1 { get; set; }
        [DataMember]
        public long buy2 { get; set; }
        [DataMember]
        public long buy3 { get; set; }
        [DataMember]
        public long buy4 { get; set; }
        [DataMember]
        public long buy5 { get; set; }
        [DataMember]
        public long buy6 { get; set; }
        [DataMember]
        public long buy7 { get; set; }
        [DataMember]
        public long buy8 { get; set; }
        [DataMember]
        public long buy9 { get; set; }
        [DataMember]
        public long buy10 { get; set; }

        [DataMember]
        public long buy1_count { get; set; }
        [DataMember]
        public long buy2_count { get; set; }
        [DataMember]
        public long buy3_count { get; set; }
        [DataMember]
        public long buy4_count { get; set; }
        [DataMember]
        public long buy5_count { get; set; }
        [DataMember]
        public long buy6_count { get; set; }
        [DataMember]
        public long buy7_count { get; set; }
        [DataMember]
        public long buy8_count { get; set; }
        [DataMember]
        public long buy9_count { get; set; }
        [DataMember]
        public long buy10_count { get; set; }


        [DataMember]
        public long sell1 { get; set; }
        [DataMember]
        public long sell2 { get; set; }
        [DataMember]
        public long sell3 { get; set; }
        [DataMember]
        public long sell4 { get; set; }
        [DataMember]
        public long sell5 { get; set; }
        [DataMember]
        public long sell6 { get; set; }
        [DataMember]
        public long sell7 { get; set; }
        [DataMember]
        public long sell8 { get; set; }
        [DataMember]
        public long sell9 { get; set; }
        [DataMember]
        public long sell10 { get; set; }

        [DataMember]
        public long sell1_count { get; set; }
        [DataMember]
        public long sell2_count { get; set; }
        [DataMember]
        public long sell3_count { get; set; }
        [DataMember]
        public long sell4_count { get; set; }
        [DataMember]
        public long sell5_count { get; set; }
        [DataMember]
        public long sell6_count { get; set; }
        [DataMember]
        public long sell7_count { get; set; }
        [DataMember]
        public long sell8_count { get; set; }
        [DataMember]
        public long sell9_count { get; set; }
        [DataMember]
        public long sell10_count { get; set; }

        public string GetDataKey()
        {
            int time = this.time / 100000;
            String sRet = String.Format("market:{0}:{1}", this.foxxcode, time);
            return sRet;
        }

        public string GetLastDataKey()
        {
            String sRet = String.Format("market:{0}", this.foxxcode);
            return sRet;
        }


        public string ToJsonString()
        {
            string data = string.Empty;
            data = JsonLib.stringify(this);
            return data;
        }

        public static MarketSecModel FromStruct(MarketTickNode iNode)
        {
            MarketSecModel iData = new MarketSecModel();

            iData.foxxcode = iNode.foxxcode;
            iData.code = iNode.code;
            iData.date = iNode.date;
            iData.time = iNode.time;
            iData.status = iNode.status;
            iData.preclose = iNode.preclose;
            iData.close = iNode.close;
            iData.ratio = 0;
            iData.dopen = iNode.dopen;
            iData.dhigh = iNode.dhigh;
            iData.dlow = iNode.dlow;
            iData.vol = iNode.vol;
            iData.money = iNode.money;
            iData.transnum = iNode.transnum;
            iData.name = iNode.name;
            iData.buyorder = iNode.buyorder;
            iData.sellorder = iNode.sellorder;
            iData.buyorder_aveprice = iNode.buyorder_aveprice;
            iData.sellorder_aveprice = iNode.sellorder_aveprice;

            iData.buy1 = iNode.buy1;

            iData.buy2 = iNode.buy2;

            iData.buy3 = iNode.buy3;

            iData.buy4 = iNode.buy4;

            iData.buy5 = iNode.buy5;

            iData.buy6 = iNode.buy6;

            iData.buy7 = iNode.buy7;

            iData.buy8 = iNode.buy8;

            iData.buy9 = iNode.buy9;

            iData.buy10 = iNode.buy10;


            iData.buy1_count = iNode.buy1_count;

            iData.buy2_count = iNode.buy2_count;

            iData.buy3_count = iNode.buy3_count;

            iData.buy4_count = iNode.buy4_count;

            iData.buy5_count = iNode.buy5_count;

            iData.buy6_count = iNode.buy6_count;

            iData.buy7_count = iNode.buy7_count;

            iData.buy8_count = iNode.buy8_count;

            iData.buy9_count = iNode.buy9_count;

            iData.buy10_count = iNode.buy10_count;



            iData.sell1 = iNode.sell1;

            iData.sell2 = iNode.sell2;

            iData.sell3 = iNode.sell3;

            iData.sell4 = iNode.sell4;

            iData.sell5 = iNode.sell5;

            iData.sell6 = iNode.sell6;

            iData.sell7 = iNode.sell7;

            iData.sell8 = iNode.sell8;

            iData.sell9 = iNode.sell9;

            iData.sell10 = iNode.sell10;


            iData.sell1_count = iNode.sell1_count;

            iData.sell2_count = iNode.sell2_count;

            iData.sell3_count = iNode.sell3_count;

            iData.sell4_count = iNode.sell4_count;

            iData.sell5_count = iNode.sell5_count;

            iData.sell6_count = iNode.sell6_count;

            iData.sell7_count = iNode.sell7_count;

            iData.sell8_count = iNode.sell8_count;

            iData.sell9_count = iNode.sell9_count;

            iData.sell10_count = iNode.sell10_count;

            return iData;
        }


    }
}
