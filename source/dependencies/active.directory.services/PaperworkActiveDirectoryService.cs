using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.DirectoryService
{
    public class PaperworkActiveDirectoryService : IDirectoryService
    {
        public string CurrentUserName { get; private set; }
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
