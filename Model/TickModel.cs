using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.Model
{
    [DataContract]
    public class TickModel:TDFDataModel
    {
        [DataMember]
        public string code { get; set; }

        [DataMember]
        public int date { get; set; }

        [DataMember]
        public int time { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public int preclose { get; set; }

        [DataMember]
        public int close { get; set; }

        [DataMember]
        public double ratio { get; set; }

        [DataMember]
        public int dopen { get; set; }

        [DataMember]
        public int dhigh { get; set; }

        [DataMember]
        public int dlow { get; set; }

        [DataMember]
        public long vol { get; set; }

        [DataMember]
        public long money { get; set; }

        [DataMember]
        public long transnum { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public long buyorder { get; set; }

        [DataMember]
        public long sellorder { get; set; }

        [DataMember]
        public int buyorder_aveprice { get; set; }

        [DataMember]
        public int sellorder_aveprice { get; set; }


        public string getStatusName(int nStatus)
        {
            string status = string.Empty;

            status = string.Format("{0}",nStatus);

            return status;
        }

        public string getUniqueKey()
        {
            string sRet = string.Empty;

            sRet = string.Format("{0}_{1}_{2}",this.foxxcode,this.date,this.time);

            return sRet;
        }

    }
}
