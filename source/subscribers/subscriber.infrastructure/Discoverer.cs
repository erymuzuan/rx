using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Discoverer
    {
        public SubscriberMetadata[] Find()
        {
            var subscribers = new List<SubscriberMetadata>();
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"..\subscribers", "*.dll");
            foreach (var dll in assemblies)
            {
                var list = FindSubscriber(dll);
                subscribers.AddRange(list);
            }

            return subscribers.ToArray();
        }

        private static IEnumerable<SubscriberMetadata> FindSubscriber(string dll)
        {
            if (string.IsNullOrWhiteSpace(dll)) return new SubscriberMetadata[] {};
            var fileName = Path.GetFileName(dll);

            if (fileName == "subscriber.infrastructure.dll") return new SubscriberMetadata[] { };
            if (!fileName.StartsWith("subscriber")) return new SubscriberMetadata[] {};
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

        public dynamic FindSubscriber()
        {
            var aseemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");

            var list = new List<dynamic>();
            foreach (var dll in aseemblies)
            {
                var assembly = Assembly.LoadFile(dll);
                var providers = assembly.GetTypes()
                   .Where(t => t.FullName.EndsWith("SubscriberProvider"));
                foreach (var type in providers)
                {
                    dynamic pv = Activator.CreateInstance(type);
                    var sms = pv.GetSubscribers();
                    list.AddRange(sms);
                    Console.WriteLine(sms);
                }
            }

            return list;
        }

    }
}
