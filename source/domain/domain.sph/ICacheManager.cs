using System;

namespace Bespoke.Sph.Domain
{

    public interface ICacheManager
    {
        void Insert<T>(string key, T data, params string[] dependencies);
        void Insert<T>(string key, T data, TimeSpan absoluteExpiration, string fileDependency = null);
        T Get<T>(string key);
    }
}
