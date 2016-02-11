using System;
using System.Web;
using System.Web.Caching;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Helpers
{
    public class WebCacheManager : ICacheManager
    {

        public void Insert<T>(string key, T data, string fileDependency = null)
        {
            var cacheKey = $"{typeof(T).FullName}:{key}";
            if (HttpContext.Current.Cache.Get(cacheKey) != null)
            {
                HttpContext.Current.Cache[cacheKey] = data;
                return;
            }

            if (System.IO.File.Exists(fileDependency))
            {
                var fd = new CacheDependency(fileDependency, DateTime.Now);
                HttpContext.Current.Cache.Insert(cacheKey, data, fd);
                return;

            }
            HttpContext.Current.Cache.Insert(cacheKey, data);
        }

        public void Insert<T>(string key, T data, TimeSpan absoluteExpiration, string fileDependency = null)
        {
            var cacheKey = $"{typeof(T).FullName}:{key}";
            if (HttpContext.Current.Cache.Get(cacheKey) != null)
            {
                HttpContext.Current.Cache[cacheKey] = data;
                return;
            }

            if (System.IO.File.Exists(fileDependency))
            {
                var fd = new CacheDependency(fileDependency, DateTime.Now);
                HttpContext.Current.Cache.Insert(cacheKey, data, fd, DateTime.Now.Add(absoluteExpiration), Cache.NoSlidingExpiration);
                return;

            }
            HttpContext.Current.Cache.Insert(cacheKey, data);
        }
        public T Get<T>(string key)
        {
            var cacheKey = $"{typeof(T).FullName}:{key}";
            var obj = HttpContext.Current.Cache.Get(cacheKey);
            if (obj == null)
            {
                return default(T);
            }
            return (T)obj;
        }
    }
}