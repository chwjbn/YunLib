using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.Model
{

    [DataContract]
    public class TDFDataModel
    {
        [DataMember]
        public string foxxcode { get; set; }
    }
}
