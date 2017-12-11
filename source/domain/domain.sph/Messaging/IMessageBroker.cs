using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Messaging
{
    public interface IMessageBroker : IDisposable
    {
        void StartsConsume();
        event EventHandler<EventArgs> ConnectionShutdown;
        Task ConnectAsync();
    }
}
