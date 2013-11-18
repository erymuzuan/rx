using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.DirectoryServices
{
    public class AspNetDirectoryService : IDirectoryService
    {
        public string CurrentUserName
        {
            get
            {
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    return Thread.CurrentPrincipal.Identity.Name;
                return string.Empty;
            }
        }

        public Task<string[]> GetUserInRolesAsync(string role)
        {
            return Task.FromResult(Roles.GetUsersInRole(role));
        }

        public Task<string[]> GetUserRolesAsync(string userName)
        {
            return Task.FromResult(Roles.GetRolesForUser(userName));
        }

        public Task<bool> AuthenticateAsync(string userName, string password)
        {
            return Task.FromResult(Membership.ValidateUser(userName,password));
        }
    }
}