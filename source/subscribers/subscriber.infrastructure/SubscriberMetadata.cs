using System;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class SubscriberMetadata
    {
        public string Assembly { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public int Instance { get; set; } = 1;
    }
}