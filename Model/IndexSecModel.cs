using System;
using System.IO;
using System.Runtime.Serialization;
using YunLib;
using YunLib.StructData;

namespace YunLib.Model
{

    [DataContract]
    public class IndexSecModel : TickModel
    {

        public string GetDataKey()
        {
            int time = this.time / 100000;
            String sRet = String.Format("index:{0}:{1}", this.foxxcode, time);
            return sRet;
        }

        public string GetLastDataKey()
        {
            String sRet = String.Format("index:{0}", this.foxxcode);
            return sRet;
        }


        public string ToJsonString()
        {
            string data = string.Empty;
            data = JsonLib.stringify(this);
            return data;
        }

        public static IndexSecModel FromStruct(IndexTickNode iNode)
        {
            IndexSecModel iData = new IndexSecModel();

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

            return iData;
        }



    }
}
