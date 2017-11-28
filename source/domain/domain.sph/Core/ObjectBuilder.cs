using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Extensions;
using Spring.Context.Support;
using Spring.Objects.Factory;

namespace Bespoke.Sph.Domain
{
    public static class ObjectBuilder
    {
        private static readonly ConcurrentDictionary<Type, object> m_cacheList = new ConcurrentDictionary<Type, object>();
        private static CompositionContainer m_container;

        public static void RegisterSpring(params string[] uri)
        {

        }

        public static void ComposeMefCatalog(object part, params Assembly[] assemblies)
        {
            ILogger logger = new DummyLogger();
            if (part.GetType() != typeof(Logger))
                logger = GetObject<ILogger>();

            var catalog = new AggregateCatalog();
            var callingAssembly = Assembly.GetCallingAssembly();
            catalog.Catalogs.Add(new AssemblyCatalog(callingAssembly));

            var executing = Assembly.GetExecutingAssembly();
            if (executing.FullName != callingAssembly.FullName)
                catalog.Catalogs.Add(new AssemblyCatalog(executing));

            foreach (var dll in assemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(dll));
            }
            var ignores = new[]
                {
                    "Esent", "GalaSoft","ICSharpCode", "JWT","Mono","FileHelpers","Colorful",
                    "Microsoft","Spring","WebGrease","WebActivator","WebMatrix",
                    "workflows","Antlr3.Runtime", "LinqToQuerystring",
                    "DiffPlex","Common.Logging","EntityFramework",
                    "Humanizer","ImageResizer",
                    "Invoke","Monads",
                    "NCrontab","Newtonsoft",
                    "RazorGenerator","RazorEngine","SQLSpatialTools","System",
                    "Antlr3","RazorEngine",
                    "DotNetOpenAuth","System","Owin","RabbitMQ.Client","Roslyn",
                    "Telerik", "Dapper","Thinktecture","Polly", "Mindscape",
                    "domain.sph", executing.GetName().Name
                };

            void LoadAssemblyCatalog(string x)
            {
                try
                {
                    catalog.Catalogs.Add(new AssemblyCatalog(x));
                }
                catch (BadImageFormatException)
                {
                    logger.WriteWarning($"cannot load {x}");
                }
                catch (Exception e)
                {
                    logger.WriteWarning($"Error loading {x} : {e.Message}");
                    logger.Log(new LogEntry(e));
                }
            }

            foreach (var file in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll"))
            {
                var name = Path.GetFileName(file);
                if (ignores.Any(name.StartsWith)) continue;

                LoadAssemblyCatalog(file);
                logger.WriteDebug($"Loaded {name}");
            }

            var bin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (Directory.Exists(bin))
            {
                // for web
                foreach (var file in Directory.GetFiles(bin, "*.dll"))
                {
                    var name = Path.GetFileName(file);
                    if (ignores.Any(name.StartsWith)) continue;
                    LoadAssemblyCatalog(file);
                    logger.Log(new LogEntry { Message = $"Loaded from bin {name}", Severity = Severity.Debug });
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
                    .ForEach(e => logger.Log(new LogEntry(e)));
                //Debugger.Break();
            }
            catch (CompositionException compositionException)
            {
                logger.Log(new LogEntry(compositionException));
                Debugger.Break();
            }
            catch (InvalidOperationException ioe) when (ioe.Message.Contains("Currently composing another batch in this ComposablePartExportProvider"))
            {
                ComposeMefCatalog(part, assemblies);
            }

        }

        public static void AddCacheList(Type type, object dependency)
        {
            m_cacheList.AddOrUpdate(type, dependency, (k, o) => dependency);
        }

        public static void AddCacheList<T>(T dependency)
        {
            var key = typeof(T);
            m_cacheList.AddOrUpdate(key, dependency, (k, o) => dependency);
        }

        public static T GetObject<T>() where T : class
        {
            var key = typeof(T);

            if (m_cacheList.TryGetValue(key, out var item))
                return item as T;

            try
            {
                var springObject = ContextRegistry.GetContext().GetObject<T>();
                if (null != springObject)
                {
                    m_cacheList.AddOrUpdate(typeof(T), springObject, (t, o) => springObject);
                    return springObject;
                }
            }
            catch (NoSuchObjectDefinitionException) { }

            if (null == m_container)
            {
                throw new Exception("MEF has not been composed");
            }
            var mefObject = m_container.GetExportedValue<T>();
            m_cacheList.AddOrUpdate(typeof(T), mefObject, (t, o) => mefObject);
            return mefObject;
        }

        public static dynamic GetObject(Type key)
        {
            if (m_cacheList.TryGetValue(key, out var item))
                return item;


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
                m_cacheList.GetOrAdd(key, springObject);
                return springObject;
            }

            throw new InvalidOperationException("Cannot find any object for " + key.FullName);
        }
        public static dynamic GetObject(string name)
        {
            var springObject = ContextRegistry.GetContext().GetObject(name);
            return springObject;
        }

        #region "DUMMY LOGGER"

        private class DummyLogger : ILogger
        {
            public Severity TraceSwitch { get; set; } = Severity.Info;
            public Task LogAsync(LogEntry entry)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(entry.ToString());
                }
                finally
                {
                    Console.ResetColor();
                }
                return Task.FromResult(0);
            }

            public void Log(LogEntry entry)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(entry.ToString());
                }
                finally
                {
                    Console.ResetColor();
                }
            }


        }

        #endregion
    }
}
