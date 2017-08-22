using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class MessageSlaNotificationAction : DomainObject
    {
        public virtual bool Execute(MessageTrackingStatus status, Entity item, MessageSlaEvent @event)
        {
            throw new Exception("NotImplemented");
        }

        public virtual Task<bool> ExecuteAsync(MessageTrackingStatus status, Entity item, MessageSlaEvent @event)
        {
            throw new Exception("NotImplemented");
        }
        [JsonIgnore]
        public virtual bool UseAsync => throw new Exception("NotImplemented");
    }
}