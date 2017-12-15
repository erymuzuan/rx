using System;

namespace Bespoke.Sph.Domain.Messaging
{
    public class QueueSubscriptionOption
    {
        public string Name { get; }

        public QueueSubscriptionOption(string name, params string[] routingKeys)
        {
            Name = name;
            RoutingKeys = routingKeys;
        }

        public string[] RoutingKeys { get; set; }
        public string DeadLetterQueue { get; set; }
        public string DelayedQueue { get; set; }
        public TimeSpan Ttl { get; set; }
        public int PrefetchCount { get; set; }
    }
}