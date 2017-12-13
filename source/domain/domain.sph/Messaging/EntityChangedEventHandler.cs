using System;

namespace Bespoke.Sph.Domain.Messaging
{
    [Serializable]
    public delegate void EntityChangedEventHandler<T>(object sender, EntityChangedEventArgs<T> e) where T : Entity;
}