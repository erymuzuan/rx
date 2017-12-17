using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Messaging
{
    public interface IMessageBroker : IDisposable
    {
        void StartsConsume();
        Task ConnectAsync(Action<string, object> disconnected);
        void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, SubscriberOption subscription, double timeOut = double.MaxValue);


        Task<QueueStatistics> GetStatisticsAsync(string queue);
        Task CreateSubscriptionAsync(QueueSubscriptionOption option);
        Task SendToDeadLetterQueue(BrokeredMessage message);
        Task SendAsync(BrokeredMessage message);
        Task<BrokeredMessage> ReadFromDeadLetterAsync();

        Task<BrokeredMessage> GetMessageAsync(string queue);
        Task RemoveSubscriptionAsync(string queue);
    }

    public class SubscriberOption
    {
        public string Name { get; }
        public int PrefetchCount { get; set; } = 1;

        public SubscriberOption(string name)
        {
            Name = name;
        }
    }
}
