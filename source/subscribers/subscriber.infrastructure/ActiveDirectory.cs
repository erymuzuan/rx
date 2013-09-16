using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    class ActiveDirectory : IDirectoryService
    {
        public string CurrentUserName { get { return "AMQP Broker"; } }
    }
}
