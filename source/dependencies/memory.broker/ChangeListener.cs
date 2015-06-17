using System;
using System.Threading;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Messaging
{
    public class ChangeListener<T> : IEntityChangedListener<T> where T : Entity
    {
        public void Run()
        {
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>() as Broker;
            publisher?.RegisterListener(this);
        }

        public void Stop()
        {
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>() as Broker;
            publisher?.RemoveListener(this);
        }

        internal void SendMessage(T t, AuditTrail log)
        {
            var arg = new EntityChangedEventArgs<T>
            {
                Item = t,
                AuditTrail = log
            };

            if (null != this.Changed && null != t)
            {
                if (null != m_context)
                    m_context.Post(d => this.Changed(this, arg), arg);
                else
                    this.Changed(this, arg);// worker thread

            }
        }

        public event EntityChangedEventHandler<T> Changed;
        private SynchronizationContext m_context;
        public void Run(SynchronizationContext synchronizationContext)
        {
            m_context = synchronizationContext;
        }
    }
}
