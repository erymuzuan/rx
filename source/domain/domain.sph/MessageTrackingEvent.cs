using System;

namespace Bespoke.Sph.Domain
{
    public class MessageTrackingEvent
    {
        public MessageTrackingEvent() { }

        public MessageTrackingEvent(MessageSlaEvent sla) : this(sla.Entity, sla.ItemId, sla.MessageId, sla.RoutingKey)
        {
            this.Worker = sla.Worker;
        }
        public MessageTrackingEvent(string entity, string itemId, string messageId, string routingKey)
        {
            this.MessageId = messageId;
            this.RoutingKey = routingKey;
            this.Entity = entity;
            this.DateTime = DateTime.Now;
            this.ItemId = itemId;
        }
        public MessageTrackingEvent(Entity item, string messageId, string routingKey) : this(string.Empty, string.Empty, messageId, routingKey)
        {
            if (null == item) return;

            var type = item.GetEntityType();
            this.Entity = type.Name;
            this.EntityNamespace = type.Namespace;
            this.ItemId = item.Id;
        }

        public string EntityNamespace { get; }

        public MessageTrackingEvent(Entity item, string messageId, string routingKey, string worker) : this(item, messageId, routingKey)
        {
            Worker = worker;
        }
        public string MessageId { get; set; }
        public string RoutingKey { get; set; }
        public string ItemId { get; set; }
        public string Entity { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string MachineName { get; set; } = Environment.GetEnvironmentVariable("COMPUTERNAME");
        public string ProcessName { get; set; } = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        public string Event { get; set; }
        public TimeSpan ProcessingTimeSpan { get; set; }
        public string Worker { get; set; }
        public double ProcessingTimeSpanInMiliseconds => this.ProcessingTimeSpan.TotalMilliseconds;

    }

    public class MessageSlaEvent
    {
        public string MessageId { get; set; }
        public string RoutingKey { get; set; }
        public string ItemId { get; set; }
        public string Entity { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Event { get; set; }
        public string Worker { get; set; }
        public double ProcessingTimeSpanInMiliseconds { get; set; }
    }
}