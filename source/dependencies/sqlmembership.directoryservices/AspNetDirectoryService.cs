using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Monads.NET;

namespace Bespoke.Sph.DirectoryServices
{
    public class AspNetDirectoryService : IDirectoryService
    {
        public string CurrentUserName
        {
            get
            {
                var user = HttpContext.Current.With(c => c.User)
                    .With(u => u.Identity)
                    .With(i => i.Name);
                if (!string.IsNullOrWhiteSpace(user)) return user;
                user = Membership
                    .GetUser()
                    .With(u => u.UserName);
                if (!string.IsNullOrWhiteSpace(user)) return user;

                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                    return Thread.CurrentPrincipal.Identity.Name;
                if (!string.IsNullOrWhiteSpace(Environment.UserDomainName))
                    return Environment.UserDomainName;
                return Environment.UserName;
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
            return Task.FromResult(Membership.ValidateUser(userName, password));
        }

        public Task<UserProfile> GetUserAsync(string userName)
        {
            var user = Membership.GetUser(userName);
            if (null != user)
                return Task.FromResult(new UserProfile
                {
                    UserName = userName,
                    IsLockedOut = user.IsLockedOut
                   
                });
            return Task.FromResult<UserProfile>(null);
        }
    }
}