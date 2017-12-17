using System;

namespace Bespoke.Sph.Domain.Messaging
{
    public class SubscriberOption
    {
        public string QueueName { get; }
        public string Name { get; }
        public int PrefetchCount { get; set; } = 1;

        public SubscriberOption(string queueName) : this(queueName, Guid.NewGuid().ToString("N").ToUpperInvariant())
        {
        }
        public SubscriberOption(string queueName, string name)
        {
            QueueName = queueName;
            Name = name;
        }
    }
}