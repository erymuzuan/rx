using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Messaging;

namespace Bespoke.Sph.Messaging.AzureMessaging
{
    public class ServiceBusMessageBroker : IMessageBroker
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void StartsConsume()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> ConnectionShutdown;
        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public async void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, double timeOut = double.MaxValue)
        {
            var message = new BrokeredMessage();// TODO : read from queue
            var status = await processItem(message);
            switch (status)
            {
                case MessageReceiveStatus.Accepted:
                    break;
                case MessageReceiveStatus.Rejected:
                    break;
                case MessageReceiveStatus.Requeued:
                    break;
                case MessageReceiveStatus.Dropped:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public Task<QueueStatistics> GetStatisticsAsync(string queue)
        {
            throw new NotImplementedException();
        }

        public Task CreateSubscriptionAsync(QueueSubscriptionOption option)
        {
            throw new NotImplementedException();
        }
       

        public Task SendToDeadLetterQueue(BrokeredMessage message)
        {
            throw new NotImplementedException();
        }

        public Task<BrokeredMessage> ReadFromDeadLetterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BrokeredMessage> GetMessageAsync(string queue)
        {
            throw new NotImplementedException();
        }

        public Task RemoveSubscriptionAsync(string queue)
        {
            throw new NotImplementedException();
        }
    }
}
