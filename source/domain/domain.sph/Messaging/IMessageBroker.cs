using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Messaging
{
    public interface IMessageBroker : IDisposable
    {
        void StartsConsume();
        event EventHandler<EventArgs> ConnectionShutdown;
        Task ConnectAsync();
        void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, double timeOut = double.MaxValue);
        Task<QueueStatistics> GetStatisticsAsync(string queue);
        Task CreateSubscriptionAsync(QueueSubscriptionOption option);
        Task SendToDeathLetter(BrokeredMessage message);
        Task<BrokeredMessage> ReadFromDeadLetterAsync();
        Task<BrokeredMessage> GetMessageAsync(string queue);
    }

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
    }
}
