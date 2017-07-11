using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    /// <summary>
    /// Just helper for the Dictionary object
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add a new key-value, if not exist, else will replace the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddOrReplace<T, T1>(this IDictionary<T, T1> dictionary, T key , T1 value)
        {
            if(dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key,value);
            
        }
        /// <summary>
        /// Add a new key-value, if not exist else do nothing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void AddIfNotExist<T, T1>(this IDictionary<T, T1> dictionary, T key , T1 value)
        {
            if(!dictionary.ContainsKey(key))dictionary.Add(key,value);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="props"></param>
        /// <param name="headers"></param>
        public static void Copy<T, T1>(this IDictionary<T, T1> props, IDictionary<T, T1> headers)
        {
            if (null == headers) return;
            foreach (var k in headers.Keys)
            {
                if (!props.ContainsKey(k))
                    props.Add(k, headers[k]);

            }
        }
    }
}