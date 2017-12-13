using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Messaging;

namespace Bespoke.Sph.MessageTrackerSla
{
    public static class MessageBrokerExtension
    {
        public const string DELAY_EXCHANGE = "rx.delay.exchange.messages.sla";
        private const string DELAY_QUEUE = "rx.delay.queue.messages.sla";
        private const string NOTIFICATION_EXCHANGE = "rx.notification.exchange.messages.sla";
        public const string NOTIFICATION_QUEUE = "rx.notification.queue.messages.sla";
        public const int PERSISTENT_DELIVERY_MODE = 2;
        
        public static Task CreateSlaMonitorQueueAsync(this IMessageBroker broker)
        {
            return broker.CreateSubscriptionAsync(new QueueSubscriptionOption(NOTIFICATION_QUEUE, NOTIFICATION_QUEUE)
            {
                DeadLetterQueue = NOTIFICATION_EXCHANGE,
                DelayedQueue = DELAY_QUEUE,
                Ttl = TimeSpan.FromMinutes(5)
            });

        }
        
     
        

    }
}