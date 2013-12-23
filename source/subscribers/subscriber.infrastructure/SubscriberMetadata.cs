using System;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class SubscriberMetadata
    {
        public string Assembly { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}