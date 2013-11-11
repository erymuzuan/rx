using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Scheduler.Sph.Rental
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
