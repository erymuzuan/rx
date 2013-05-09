using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Reflection;
using Spring.Context.Support;

namespace Bespoke.CommercialSpace.Domain
{
    public static class ObjectBuilder
    {
        private static readonly Dictionary<Type, object> m_cacheList = new Dictionary<Type, object>();
        private static CompositionContainer m_container;
        
        public static void RegisterSpring(params string[] uri)
        {
        }

        public static void ComposeMefCatalog(object part)
        {

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            catalog.Catalogs.Add(new DirectoryCatalog("."));
            catalog.Catalogs.Add(new DirectoryCatalog(".\\bin"));

            m_container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddPart(part);
            batch.AddExportedValue(m_container);

            try
            {
                m_container.Compose(batch);

            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException);
                Debugger.Break();
            }
        }

        public static void AddCacheList<T>(T dependency)
        {
            var key = typeof(T);
            if (m_cacheList.ContainsKey(key))
                m_cacheList[key] = dependency;
            else
                m_cacheList.Add(key, dependency);
        }

        public static T GetObject<T>() where T : class
        {
            var key = typeof(T);
            if (m_cacheList.ContainsKey(key))
                return m_cacheList[key] as T;

            var springObject = ContextRegistry.GetContext().GetObject<T>();
            if (null != springObject)
            {
                m_cacheList.Add(typeof(T),springObject);
                return springObject;
            }
            if (null == m_container)
            {
                throw new Exception("MEF has not been composed");
            }
            var k = m_container.GetExportedValue<T>();
            if (null != k)
            {
                m_cacheList.Add(typeof(T), k);
                return k;
            }


            throw new InvalidOperationException("Cannot find any object for " + typeof(T).FullName);
        }

    }
}
