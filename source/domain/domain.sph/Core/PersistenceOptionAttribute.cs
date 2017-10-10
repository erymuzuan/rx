using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Bespoke.Sph.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PersistenceOptionAttribute : Attribute
    {
        public bool IsElasticsearch { get; set; }
        public bool IsSqlDatabase { get; set; }
        public virtual bool IsSource { get; set; }
        public virtual bool EnableAuditing { get; set; }
        /// <summary>
        /// JsonSerializer must use TypeNameHandling.All, thus the object with derived types as it's aggregates might not be deserialize correctly using StreamRead
        /// </summary>
        public bool HasDerivedTypes { get; set; }

        private static readonly ConcurrentDictionary<Type, PersistenceOptionAttribute> m_store = new ConcurrentDictionary<Type, PersistenceOptionAttribute>();

        public static PersistenceOptionAttribute GetAttribute<T>() where T : Entity
        {
            return GetAttribute(typeof(T));

        }

        public static PersistenceOptionAttribute GetAttribute(Type type)
        {
            if (m_store.TryGetValue(type, out var options))
                return options;
            
            options = type.GetCustomAttribute<PersistenceOptionAttribute>();
            if (null != options)
            {
                m_store.TryAdd(type, options);
                return options;
            }

            // the default
            var defaultOption = new PersistenceOptionAttribute { IsSqlDatabase = true, IsElasticsearch = true, IsSource = false , EnableAuditing = false};
            m_store.TryAdd(type, defaultOption);

            return defaultOption;
        }
    }


    // for backward compatibility, so that we don't have to recompile every EntityDefinition
    [AttributeUsage(AttributeTargets.Class)]
    public class StoreAsSourceAttribute : PersistenceOptionAttribute
    {
        public override bool IsSource { get { return true; } set { } }
    }
}
