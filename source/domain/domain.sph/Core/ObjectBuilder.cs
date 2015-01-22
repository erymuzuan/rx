using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Spring.Context.Support;
using Spring.Objects.Factory;

namespace Bespoke.Sph.Domain
{
    public static class ObjectBuilder
    {
        private static readonly object m_lock = new object();
        private static readonly Dictionary<Type, object> m_cacheList = new Dictionary<Type, object>();
        private static CompositionContainer m_container;

        public static void RegisterSpring(params string[] uri)
        {

        }

        static readonly string[] m_ignores = {
                    "Microsoft","Spring","WebGrease","WebActivator","WebMatrix",
                    "workflows","Antlr3.Runtime",
                    "DiffPlex","Common.Logging","EntityFramework",
                    "Humanizer","ImageResizer",
                    "Invoke","Monads",
                    "NCrontab","Newtonsoft",
                    "RazorGenerator","RazorEngine","SQLSpatialTools","System",
                    "Antlr3","RazorEngine",
                    "DotNetOpenAuth","System","Owin","RabbitMQ.Client","Roslyn",
                    "domain.sph"
                };
        public static void ComposeMefCatalog(object part, params Assembly[] assemblies)
        {
            var catalog = new AggregateCatalog();
            var calling = Assembly.GetCallingAssembly();
            var executing = Assembly.GetExecutingAssembly();
            catalog.Catalogs.Add(new AssemblyCatalog(calling));
            catalog.Catalogs.Add(new AssemblyCatalog(executing));
            var ignores = m_ignores.ToList();
            ignores.Add(executing.GetName().Name);
            ignores.Add(calling.GetName().Name);

            foreach (var dll in assemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(dll));
            }

            var color = Console.ForegroundColor;
            Action<string> loadAssemblyCatalog = directory =>
            {
                if (!Directory.Exists(directory)) return;
                foreach (var x in Directory.GetFiles(directory, "*.dll"))
                {
                    var name = Path.GetFileName(x) ?? "";
                    if (ignores.Any(name.StartsWith)) continue;
                    if (executing.Location == x) continue;
                    if (calling.Location == x) continue;
                    try
                    {
                        catalog.Catalogs.Add(new AssemblyCatalog(x));
                        if (DebuggerHelper.IsVerbose)
                            Console.WriteLine("Loaded {0}", name);
                    }
                    catch (BadImageFormatException)
                    {
                        Console.WriteLine("BadImageFormatException for {0}", x);
                    }
                }

            };
            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("============= LOADING MEF ================");
                loadAssemblyCatalog(AppDomain.CurrentDomain.BaseDirectory);
                var webbin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
                loadAssemblyCatalog(webbin);

            }
            finally
            {
                Console.ForegroundColor = color;
            }



            m_container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddPart(part);
            batch.AddExportedValue(m_container);

            try
            {
                m_container.Compose(batch);

            }
            catch (ReflectionTypeLoadException rtle)
            {
                color = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    rtle.LoaderExceptions.Select(x => x.Message).ToList().ForEach(Console.WriteLine);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    rtle.LoaderExceptions.Select(x => x.StackTrace).ToList().ForEach(Console.WriteLine);
                }
                finally
                {
                    Console.ForegroundColor = color;
                }

            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException);
                Debugger.Break();
            }
        }

        public static void AddCacheList(Type type, object dependency)
        {
            var key = type;
            lock (m_lock)
            {
                if (m_cacheList.ContainsKey(key))
                    m_cacheList[key] = dependency;
                else
                    m_cacheList.Add(key, dependency);
            }
        }
        public static void AddCacheList<T>(T dependency)
        {
            var key = typeof(T);
            lock (m_lock)
            {
                if (m_cacheList.ContainsKey(key))
                    m_cacheList[key] = dependency;
                else
                    m_cacheList.Add(key, dependency);
            }
        }

        public static T GetObject<T>() where T : class
        {
            var key = typeof(T);
            lock (m_lock)
            {
                if (m_cacheList.ContainsKey(key))
                    return m_cacheList[key] as T;
            }

            try
            {
                var springObject = ContextRegistry.GetContext().GetObject<T>();
                if (null != springObject)
                {
                    lock (m_lock)
                    {
                        if (!m_cacheList.ContainsKey(typeof(T)))
                            m_cacheList.Add(typeof(T), springObject);
                    }
                    return springObject;
                }

            }
            catch (NoSuchObjectDefinitionException)
            {

            }
            if (null == m_container)
            {
                throw new Exception("MEF has not been composed");
            }
            var k = m_container.GetExportedValue<T>();
            if (null != k)
            {
                lock (m_lock)
                {
                    if (!m_cacheList.ContainsKey(typeof(T)))
                        m_cacheList.Add(typeof(T), k);
                }
                return k;
            }


            throw new InvalidOperationException("Cannot find any object for " + typeof(T).FullName);
        }

        public static dynamic GetObject(Type key)
        {
            lock (m_lock)
            {
                if (m_cacheList.ContainsKey(key))
                    return m_cacheList[key];
            }

            var name = key.ToString();
            if (key.IsGenericType)
            {
                //Bespoke.Sph.Domain.IRepository`1[Bespoke.Sph.Domain.Trigger]
                name = name.Replace("Bespoke.Sph.Domain.", string.Empty)
                    .Replace("`1[", "<")
                    .Replace("]", ">")
                    ;

            }
            var springObject = ContextRegistry.GetContext().GetObject(name);
            if (null != springObject)
            {
                lock (m_lock)
                {
                    m_cacheList.Add(key, springObject);
                }
                return springObject;
            }

            throw new InvalidOperationException("Cannot find any object for " + key.FullName);
        }
        public static dynamic GetObject(string name)
        {
            var springObject = ContextRegistry.GetContext().GetObject(name);
            return springObject;
        }

    }
}
