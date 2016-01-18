﻿using System;
using System.Web;
using System.Web.Caching;

namespace Bespoke.Sph.Web.Helpers
{
    public class CacheManager
    {
        private CacheManager() {}
        private static CacheManager m_cache;
        public static CacheManager Default => m_cache ?? (m_cache = new CacheManager());

        public void Insert(string key, object data, string fileDependency = null)
        {
            if (HttpContext.Current.Cache.Get(key) != null)
            {
                HttpContext.Current.Cache[key] = data;
                return;
            }

            if (System.IO.File.Exists(fileDependency))
            {
                var fd = new CacheDependency(fileDependency,DateTime.Now);
                HttpContext.Current.Cache.Insert(key, data, fd);
                return;

            }
            HttpContext.Current.Cache.Insert(key,data);
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