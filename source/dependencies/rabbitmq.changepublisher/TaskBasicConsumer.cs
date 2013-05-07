using System;
using RabbitMQ.Client;

namespace Bespoke.Station.RabbitMqPublisher
{
    public class TaskBasicConsumer : DefaultBasicConsumer
    {

        public TaskBasicConsumer(IModel channel)
            : base(channel)
        {

        }

        public EventHandler<ReceivedMessageArgs> Received;
 
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange
            , string routingKey, IBasicProperties properties, byte[] body)
        {
            if (null != Received)
            {
                var args = new ReceivedMessageArgs
                    {
                        ConsumerTag = consumerTag,
                        DeliveryTag = deliveryTag,
                        Redelivered = redelivered,
                        Exchange = exchange,
                        RoutingKey = routingKey,
                        Body = body,
                        Properties = properties
                    };
                Received(this, args);
            }
            base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
        }
    }
}
