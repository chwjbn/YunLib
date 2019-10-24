using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace YunLib
{
    public static class JsonLib
    {
        public static string stringify(object jsonObject)
        {
            string output = JsonConvert.SerializeObject(jsonObject);
            return output;
        }

        public static T parse<T>(string jsonString)
        {

            
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                try { 
                    return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
                }catch(Exception e)
                {
                    Console.WriteLine( e.Message);
                    return default(T);
                }
            }
        }
    }


}
