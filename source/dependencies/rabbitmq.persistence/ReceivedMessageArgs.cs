using System;
using RabbitMQ.Client;

namespace Bespoke.Station.MessagingPersistences
{
    public class ReceivedMessageArgs : EventArgs
    {
        public string ConsumerTag { get; set; }
        public ulong DeliveryTag { get; set; }
        public bool Redelivered { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public IBasicProperties Properties { get; set; }
        public byte[] Body { get; set; }
    }
}