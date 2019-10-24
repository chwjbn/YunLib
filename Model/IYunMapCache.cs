using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.Model
{
    public interface IYunMapCache
    {

        void InitCache(string dataKey,long maxCount);

        bool IsCacheValid();
        void Destory();
        
    }
}
