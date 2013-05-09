using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Bespoke.Sph.Subscribers.Infrastructure;

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
                var list = this.FindSubscriber(dll);
                subscribers.AddRange(list);
            }

            return subscribers.ToArray();
        }

        private SubscriberMetadata[] FindSubscriber(string dll)
        {
            var assembly = Assembly.LoadFile(dll);
            var types = assembly.GetTypes()
                            .Where(t => t.IsMarshalByRef)
                            .Where(t => t.BaseType == typeof(Subscriber))
                            .Where(t => !t.IsAbstract)
                            .Where(t => t.FullName.EndsWith("Subscriber"))
                            .Select(t => new SubscriberMetadata
                            {
                                Assembly = t.Assembly.FullName,
                                FullName = t.FullName,
                                Type = t
                            });

            return types.ToArray();

        }
    }
}
