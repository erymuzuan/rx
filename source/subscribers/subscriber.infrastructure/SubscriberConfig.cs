using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [StoreAsSource]
    public class WorkersConfig : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string MachineName { get; set; }

        public ObjectCollection<SubscriberConfig> SubscriberConfigs { get; set; } = new ObjectCollection<SubscriberConfig>();
    }
    
    public class SubscriberConfig 
    {
        public ushort? InstancesCount { get; set; }
        public ushort? PrefetchCount { get; set; }
        public ushort? Priority { get; set; }
        public string QueueName { get; set; }
        public string FullName { get; set; }
        public string Assembly { get; set; }
    }
}