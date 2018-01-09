using System;

namespace Bespoke.Sph.Domain.Messaging
{
    [Obsolete("Moving to IMessageBroker")]
    [Serializable]
    public delegate void EntityChangedEventHandler<T>(object sender, EntityChangedEventArgs<T> e) where T : Entity;
}