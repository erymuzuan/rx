using System;

namespace Bespoke.Sph.Domain.Messaging
{
    public class EntityChangedEventArgs<T> : EventArgs where T : Entity
    {
        public T Item { get; set; }
        public AuditTrail AuditTrail { get; set; }
    }
}