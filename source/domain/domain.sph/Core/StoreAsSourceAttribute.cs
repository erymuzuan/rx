using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Bespoke.Sph.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StoreAsSourceAttribute : Attribute
    {
        public bool IsElasticsearch { get; set; }
        public bool IsSqlDatabase { get; set; }

        private static readonly ConcurrentDictionary<Type, StoreAsSourceAttribute> m_store = new ConcurrentDictionary<Type, StoreAsSourceAttribute>();
        private static  readonly ConcurrentDictionary<Type, bool> m_hasSourceAttribute = new ConcurrentDictionary<Type, bool>();

        public static StoreAsSourceAttribute GetAttribute<T>() where T :Entity
        {
            return GetAttribute(typeof (T));

        }

        public static StoreAsSourceAttribute GetAttribute(Type type)
        {
            bool noSource;
            if (m_hasSourceAttribute.TryGetValue(type, out noSource))
                return null;

            StoreAsSourceAttribute store;
            if (m_store.TryGetValue(type, out store))
                return store;

            store = type.GetCustomAttribute<StoreAsSourceAttribute>();
            if (null != store)
            {
                m_store.TryAdd(type, store);
                return store;
            }
            m_hasSourceAttribute.TryAdd(type, false);


            return null;
        }
    }
}