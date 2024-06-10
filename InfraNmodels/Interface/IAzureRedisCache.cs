using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraNmodels.Interface
{
    public interface IAzureRedisCache
    {
        T GetCacheData<T>(string key);
        bool SetCacheData<T>(string key, T value, DateTimeOffset expirationTime);
        object RemoveData(string key);
    }
}
