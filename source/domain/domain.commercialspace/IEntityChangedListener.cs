using System;
using System.Threading;


namespace Bespoke.SphCommercialSpaces.Domain
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
        private readonly ObjectCollection<Change> m_changeCollection = new ObjectCollection<Change>();
        
        public ObjectCollection<Change> ChangeCollection
        {
            get { return m_changeCollection; }
        }

    }
}