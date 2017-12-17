namespace Bespoke.Sph.Domain.Messaging
{
    public class SubscriberOption
    {
        public string QueueName { get; }
        public int PrefetchCount { get; set; } = 1;

        public SubscriberOption(string queueName)
        {
            QueueName = queueName;
        }
    }
}