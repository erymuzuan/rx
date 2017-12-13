using System.Threading;

namespace Bespoke.Sph.Domain.Messaging
{
    public interface IEntityChangedListener<T> where T : Entity
    {
        void Run();
        void Stop();
        event EntityChangedEventHandler<T> Changed;
        void Run(SynchronizationContext synchronizationContext);
    }
}