using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Messaging.AzureMessaging.Extensions
{
    public static class DictionaryExtension
    {

        public static string GetStringValue(this IDictionary<string, object> hash, string key)
        {
            if (hash[key] is string text)
                return text;
            if (hash[key] is byte[] vals)
                return Encoding.UTF8.GetString(vals);
            return null;

        }

        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static string[] GetStringValues(this IList<object> list)
        {
            var ret = from object v in list
                      select Encoding.UTF8.GetString((byte[])v);
            return ret.ToArray();
        }
        public static TRetVal? GetValue<TKey, TRetVal>(this IDictionary<TKey, object> list, TKey key) where TRetVal : struct
        {
            if (!list.ContainsKey(key)) return default;
            return (TRetVal)list[key];
        }

    }
}