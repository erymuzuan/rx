using System;

namespace Bespoke.Sph.Domain.Messaging
{
    public class QueueDeclareOption
    {
        public string QueueName { get; }

        public QueueDeclareOption(string queueName, params string[] routingKeys)
        {
            QueueName = queueName;
            RoutingKeys = routingKeys;
        }

        public string[] RoutingKeys { get; set; }
        public string DeadLetterQueue { get; set; }
        public string DeadLetterTopic { get; set; }
        public string DelayedQueue { get; set; }
        public TimeSpan Ttl { get; set; }
        public int PrefetchCount { get; set; }
    }
}