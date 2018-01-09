using System;
using BrokeredMessage = Bespoke.Sph.Domain.Messaging.BrokeredMessage;
using AzureBrokeredMessage = Microsoft.ServiceBus.Messaging.BrokeredMessage;

namespace Bespoke.Sph.Messaging.AzureMessaging.Extensions
{
    public static class MessageBrokerExtension
    {
        public static AzureBrokeredMessage ToAzureMessage(this BrokeredMessage message)
        {
            var msg = new AzureBrokeredMessage(message.Body)
            {
                Label = message.RoutingKey,
                MessageId = message.Id
            };

            var topics = message.RoutingKey.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (topics.Length == 3)
            {
                msg.Properties.Add("entity", topics[0]);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(message.Entity))
                    msg.Properties.Add("entity", message.Entity);
            }
            msg.Properties.Add("crud", message.Crud.ToString());
            msg.Properties.Add("operation", message.Operation);

            msg.Properties.Add("try-count", message.TryCount);
            msg.Properties.Add("message-id", message.Id);
            msg.Properties.Add("data-import", message.IsDataImport);
            msg.Properties.Add("reply-to", message.ReplyTo);
            msg.Properties.Add("routing-key", message.RoutingKey);
            msg.Properties.Add("retry-delay", message.RetryDelay);
            msg.Properties.Add("username", message.Username);

            return msg;
        }

    }
}
