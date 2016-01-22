using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{

    public interface ICacheManager
    {
        void Insert<T>(string key, T data, string fileDependency = null);
        void Insert<T>(string key, T data, TimeSpan absoluteExpiration, string fileDependency = null);
        T Get<T>(string key);
    }
}
