using System;
using System.Threading;


namespace Bespoke.Sph.Domain
{
    public interface IEntityChangedListener<T> where T : Entity
    {
        void Run();
        void Stop();
        event EntityChangedEventHandler<T> Changed;
        void Run(SynchronizationContext synchronizationContext);
    }

    [Serializable]
    public delegate void EntityChangedEventHandler<T>(object sender, EntityChangedEventArgs<T> e) where T : Entity;

    public class EntityChangedEventArgs<T> : EventArgs where T : Entity
    {
        public T Item { get; set; }
        public AuditTrail AuditTrail { get; set; }
    }
}