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
            if (HttpContext.Current.Cache.Get(key) != null)
            {
                HttpContext.Current.Cache[key] = data;
                return;
            }

            if (System.IO.File.Exists(fileDependency))
            {
                var fd = new CacheDependency(fileDependency, DateTime.Now);
                HttpContext.Current.Cache.Insert(key, data, fd);
                return;

            }
            HttpContext.Current.Cache.Insert(key, data);
        }

        public void Insert<T>(string key, T data, TimeSpan absoluteExpiration, string fileDependency = null)
        {
            if (HttpContext.Current.Cache.Get(key) != null)
            {
                HttpContext.Current.Cache[key] = data;
                return;
            }

            if (System.IO.File.Exists(fileDependency))
            {
                var fd = new CacheDependency(fileDependency, DateTime.Now);
                HttpContext.Current.Cache.Insert(key, data, fd, DateTime.Now.Add(absoluteExpiration), Cache.NoSlidingExpiration);
                return;

            }
            HttpContext.Current.Cache.Insert(key, data);
        }
        public T Get<T>(string key)
        {
            if (HttpContext.Current.Cache.Get(key) == null)
            {
                return default(T);
            }
            return (T)HttpContext.Current.Cache.Get(key);
        }
    }
}