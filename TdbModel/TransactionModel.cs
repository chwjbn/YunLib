using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using YunLib;
using YunLib.StructData;

namespace YunLib.TdbModel
{
    [DataContract]
    public class TransactionModel
    {

        [DataMember]
        public String _id { get; set; }

        [DataMember]
        public String foxxcode { get; set; }

        [DataMember]
        public String _class { get; set; }

        [DataMember]
        public int date { get; set; }              //日期（自然日）格式:YYMMDD

        [DataMember]
        public int time { get; set; }             //成交时间(精确到毫秒HHMMSSmmm)

        [DataMember]
        public int index { get; set; }             //成交编号(从1开始，递增1)

        [DataMember]
        public String functionCode { get; set; }     //成交代码: 'C', 0

        [DataMember]
        public String orderKind { get; set; }       //委托类别

        [DataMember]
        public String bSFlag { get; set; }          //BS标志

        [DataMember]
        public int tradePrice { get; set; }        //成交价格((a double number + 0.00005) *10000)

        [DataMember]
        public int vol { get; set; }       //成交数量

        [DataMember]
        public int askOrder { get; set; }          //叫卖序号

        [DataMember]
        public int bidOrder { get; set; }         //叫买序号

        public TransactionNode ToStructNode()
        {
            TransactionNode iNode = new TransactionNode();

            try
            {
                iNode.foxxcode = this.foxxcode;
                iNode.date = this.date;
                iNode.time = this.time;
                iNode.code = this.foxxcode;
                iNode.ask_order = this.askOrder;
                iNode.bid_order = this.bidOrder;
                iNode.bs_flag = this.bSFlag.ToCharArray()[0];
                iNode.function_code = this.functionCode;
                iNode.index = this.index;
                iNode.order_kind = this.orderKind;
                iNode.vol = this.vol;

                long tempMoney = this.tradePrice;
                long tempVol = this.vol;
         
                iNode.money = tempMoney*tempVol;
                iNode.money = iNode.money / 10000;  //换算成元

                iNode.price = this.tradePrice;
            }
            catch (Exception)
            {
            }

            return iNode;
        }
    }
}
