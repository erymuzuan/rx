using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Messaging
{
    public interface IMessageBroker : IDisposable
    {
        Task ConnectAsync(Action<string, object> disconnected);
        void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, SubscriberOption subscription, double timeOut = double.MaxValue);
        
        Task CreateSubscriptionAsync(QueueDeclareOption option);
        Task SendAsync(BrokeredMessage message);
        Task<BrokeredMessage> GetMessageAsync(string queue);

        Task<BrokeredMessage> ReadFromDeadLetterAsync();
        Task SendToDeadLetterQueue(BrokeredMessage message);

        Task<QueueStatistics> GetStatisticsAsync(string queue);

        Task RemoveSubscriptionAsync(string queue);
    }
}
