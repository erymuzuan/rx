using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        class DummyLogger : ILogger
        {
            public void Log(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
            {
                Console.WriteLine("[{2}] {0} => {1}", operation, message, severity);
            }

            public Task LogAsync(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
            {
                Console.WriteLine("[{2}] {0} => {1}",operation, message,severity);
                return Task.FromResult(0);
            }

            public Task LogAsync(Exception exception, IReadOnlyDictionary<string, object> properties)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception.ToString());
                }
                finally
                {
                    Console.ResetColor();
                }
                return Task.FromResult(0);
            }

            public void Log(Exception exception, IReadOnlyDictionary<string, object> properties)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(exception.ToString());
                }
                finally
                {
                    Console.ResetColor();
                }
            }
        }

        public static void ComposeMefCatalog(object part, params Assembly[] assemblies)
        {
            ILogger logger = new DummyLogger();
            if (part.GetType() != typeof(Logger))
                logger = GetObject<ILogger>();

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetCallingAssembly()));
            var executing = Assembly.GetExecutingAssembly();
            catalog.Catalogs.Add(new AssemblyCatalog(executing));

            foreach (var dll in assemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(dll));
            }
            var ignores = new[]
                {
                    "Microsoft","Spring","WebGrease","WebActivator","WebMatrix",
                    "workflows","Antlr3.Runtime",
                    "DiffPlex","Common.Logging","EntityFramework",
                    "Humanizer","ImageResizer",
                    "Invoke","Monads",
                    "NCrontab","Newtonsoft",
                    "RazorGenerator","RazorEngine","SQLSpatialTools","System",
                    "Antlr3","RazorEngine",
                    "DotNetOpenAuth","System","Owin","RabbitMQ.Client","Roslyn",
                    "domain.sph", executing.GetName().Name
                };

            Action<string> loadAssemblyCatalog = x =>
            {
                try
                {
                    catalog.Catalogs.Add(new AssemblyCatalog(x));
                }
                catch (BadImageFormatException)
                {
                    logger.Log("cannot load {0}", x);
                }
            };
            foreach (var file in System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
            {

                var name = System.IO.Path.GetFileName(file) ?? "";
                if (ignores.Any(name.StartsWith)) continue;

                loadAssemblyCatalog(file);
                logger.Log("Loaded {0}", name);
            }

            var bin = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (System.IO.Directory.Exists(bin))
            {
                // for web
                foreach (var file in System.IO.Directory.GetFiles(bin, "*.dll"))
                {
                    var name = System.IO.Path.GetFileName(file) ?? "";
                    if (ignores.Any(name.StartsWith)) continue;
                    loadAssemblyCatalog(file);
                    logger.Log("Loaded from bin {0}", name);
                }

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
                rtle.LoaderExceptions.ToList()
                    .ForEach(e => logger.Log(e, null));
                //Debugger.Break();
            }
            catch (CompositionException compositionException)
            {
                logger.Log(compositionException, null);
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
            if (m_cacheList.ContainsKey(key))
                return m_cacheList[key] as T;

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
            if (m_cacheList.ContainsKey(key))
                return m_cacheList[key];

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
