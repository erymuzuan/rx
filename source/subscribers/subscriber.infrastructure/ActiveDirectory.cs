using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    class ActiveDirectory : IDirectoryService
    {
        public string CurrentUserName { get { return "AMQP Broker"; } }
        public Task<string[]> GetUserInRolesAsync(string role)
        {
            throw new System.NotImplementedException();
        }

        public Task<string[]> GetUserRolesAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AuthenticateAsync(string userName, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
