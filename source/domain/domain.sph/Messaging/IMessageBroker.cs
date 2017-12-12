using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Messaging
{
    public interface IMessageBroker : IDisposable
    {
        void StartsConsume();
        event EventHandler<EventArgs> ConnectionShutdown;
        Task ConnectAsync();
        void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, double timeOut = double.MaxValue);
    }

    public enum MessageReceiveStatus
    {
        Accepted,
        Rejected,
        Requeue
    }

    public class BrokeredMessage
    {
        public string RoutingKey { get; set; }
        public string Id { get; set; }
        public Dictionary<string, object> Headers { get; } = new Dictionary<string, object>();
        public byte[] Body { get; set; }
        public string Operation { get; set; }
        public CrudOperation Crud { get; set; }
        public int? TryCount { get; set; }
        public string Username { get; set; }
    }
    
    public enum CrudOperation
    {
        None,
        Added,
        Changed,
        Deleted
    }
    
    
}
