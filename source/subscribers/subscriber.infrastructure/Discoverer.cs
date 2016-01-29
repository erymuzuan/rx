using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Discoverer : MarshalByRefObject, IDisposable
    {
        private string m_path;

        public string ProbingPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_path))
                    return ConfigurationManager.SubscriberPath;
                return m_path;
            }
            set { m_path = value; }
        }

        public SubscriberMetadata[] Find()
        {
            try
            {
                var subscribers = new List<SubscriberMetadata>();
                var assemblies = Directory.GetFiles(this.ProbingPath, "subscriber.*.dll");
                foreach (var dll in assemblies)
                {
                    var list = FindSubscriber(dll);
                    subscribers.AddRange(list);
                }

                return subscribers.ToArray();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("*-*-*-*-*--");
                Console.WriteLine(e.FileName);
                Console.WriteLine("*-*-*-*-*--");
            }
            return new SubscriberMetadata[] { };
        }

        private static IEnumerable<SubscriberMetadata> FindSubscriber(string dll)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dll)) return new SubscriberMetadata[] { };
                var fileName = Path.GetFileName(dll);

                if (fileName == "subscriber.infrastructure.dll") return new SubscriberMetadata[] { };
                if (!fileName.StartsWith("subscriber")) return new SubscriberMetadata[] { };

                var fullFilePath = Path.Combine(ConfigurationManager.SubscriberPath, fileName);
                Console.WriteLine("Loading : {0}", fullFilePath);
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainReflectionOnlyAssemblyResolve;
                var assembly = Assembly.ReflectionOnlyLoadFrom(dll);

                return (from t in assembly.GetTypes()
                        where t.IsMarshalByRef &&
                        !t.IsAbstract &&
                        t.FullName.EndsWith("Subscriber")
                        let count = ConfigurationManager.AppSettings[$"sph:{t.FullName}:Instance"]
                        let instance = string.IsNullOrWhiteSpace(count) ? 1 : int.Parse(count)
                        select new SubscriberMetadata
                        {
                            Assembly = fullFilePath,
                            FullName = t.FullName,
                            Name = Path.GetFileNameWithoutExtension(dll),
                            Instance = instance
                        })
                    .ToArray();

            }
            catch (FileNotFoundException e)
            {

                Console.WriteLine("=================");
                Console.WriteLine(e.FileName);
                Console.WriteLine("****************");
                throw;
            }
            catch (ReflectionTypeLoadException e)
            {
                Console.WriteLine("=================");
                Console.WriteLine(e);
                foreach (var loaderException in e.LoaderExceptions)
                {
                    if (null != e.InnerException)
                        Console.WriteLine(e.InnerException);
                    Console.WriteLine(loaderException);
                }
                Console.WriteLine("****************");
                throw;
            }

        }

        static Assembly CurrentDomainReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs e)
        {
            if (!e.Name.StartsWith(ConfigurationManager.ApplicationName)) return Assembly.ReflectionOnlyLoad(e.Name);
            Console.WriteLine("Fail to load {0}", e.RequestingAssembly.Location);
            var dll = Path.Combine(ConfigurationManager.CompilerOutputPath, e.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).First() + ".dll");
            Console.WriteLine("Cannot find {0}, now loading from {1}", e.Name, dll);
            return Assembly.ReflectionOnlyLoad(File.ReadAllBytes(dll));
        }


        public void Dispose()
        {

        }
    }
}
