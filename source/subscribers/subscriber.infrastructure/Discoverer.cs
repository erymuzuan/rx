using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Discoverer
    {
        private string m_path;

        public string ProbingPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_path))
                    return AppDomain.CurrentDomain.BaseDirectory + @"..\subscribers";
                return m_path;
            }
            set { m_path = value; }
        }

        public SubscriberMetadata[] Find()
        {
            var subscribers = new List<SubscriberMetadata>();
            var assemblies = Directory.GetFiles(this.ProbingPath, "*.dll");
            foreach (var dll in assemblies)
            {
                var list = FindSubscriber(dll);
                subscribers.AddRange(list);
            }

            return subscribers.ToArray();
        }

        private static IEnumerable<SubscriberMetadata> FindSubscriber(string dll)
        {
            if (string.IsNullOrWhiteSpace(dll)) return new SubscriberMetadata[] { };
            var fileName = Path.GetFileName(dll);

            if (fileName == "subscriber.infrastructure.dll") return new SubscriberMetadata[] { };
            if (!fileName.StartsWith("subscriber")) return new SubscriberMetadata[] { };
            try
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomainReflectionOnlyAssemblyResolve;
                var assembly = Assembly.ReflectionOnlyLoadFrom(dll);

                return assembly.GetTypes()
                        .Where(t => t.IsMarshalByRef)
                        .Where(t => !t.IsAbstract)
                        .Where(t => t.FullName.EndsWith("Subscriber"))
                        .Select(t => new SubscriberMetadata
                            {
                                Assembly = t.Assembly.FullName,
                                FullName = t.FullName,
                                Name = Path.GetFileNameWithoutExtension(dll),
                                Type = t
                            })
                        .ToArray();



            }
            catch (ReflectionTypeLoadException e)
            {
                Console.WriteLine(e);
                foreach (var loaderException in e.LoaderExceptions)
                {
                    Console.WriteLine(loaderException);
                }
                throw;
            }

        }

        static Assembly CurrentDomainReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        public Subscriber[] FindSubscriber()
        {
            var assemblies = Directory.GetFiles(this.ProbingPath, "subscriber.*.dll");

            var list = new List<Subscriber>();
            foreach (var dll in assemblies)
            {
                var assembly = Assembly.LoadFile(dll);
                var providers = assembly.GetTypes()
                   .Where(t => t.FullName.EndsWith("SubscriberProvider"));
                foreach (var type in providers)
                {
                    dynamic pv = Activator.CreateInstance(type);
                    Subscriber[] sms = pv.GetSubscribers();
                    list.AddRange(sms);
                }
            }

            return list.ToArray();
        }

    }
}
