using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IMessageDeliverySla
    {
        Task RegisterAcceptanceAsync(SlaEvent eventData);
        Task RegisterStartProcessingAsync(SlaEvent eventData);
        Task RegisterCompletedAsync(SlaEvent eventData);
        Task RegisterDlqedAsync(SlaEvent eventData);
        Task RegisterRetriedAsync(SlaEvent eventData);
        Task RegisterDelayedAsync(SlaEvent eventData);
    }

    public class SlaEvent
    {
        public SlaEvent()
        {}

        public SlaEvent(Entity item, string messageId, string routingKey)
        {
            this.MessageId = messageId;
            this.RoutingKey = routingKey;
            this.Entity = item.GetEntityType().Name;
            this.ItemId = item.Id;
            this.DateTime = DateTime.Now;
        }
        public string MessageId { get; set; }
        public string RoutingKey { get; set; }
        public string ItemId { get; set; }
        public string Entity { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string MachineName { get; set; } = Environment.GetEnvironmentVariable("COMPUTERNAME");
        public string ProcessName { get; set; } = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        public string Event { get; set; }


    }
}