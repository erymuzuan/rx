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
        Task RemoveSubscriptionAsync(string queue);
    }
}
