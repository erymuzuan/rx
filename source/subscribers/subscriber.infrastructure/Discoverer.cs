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
            var aseemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            foreach (var dll in aseemblies)
            {
                var list = FindSubscriber(dll);
                subscribers.AddRange(list);
            }

            return subscribers.ToArray();
        }

        private static IEnumerable<SubscriberMetadata> FindSubscriber(string dll)
        {
            var assembly = Assembly.LoadFile(dll);


            var metadata = assembly.GetTypes()
                            .Where(t => t.IsMarshalByRef)
                            .Where(t => !t.IsAbstract)
                            .Where(t => t.FullName.EndsWith("Subscriber"))
                            .Select(t => new SubscriberMetadata
                            {
                                Assembly = t.Assembly.FullName,
                                FullName = t.FullName,
                                Type = t
                            })
                            .ToList();



            return metadata.ToArray();

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
