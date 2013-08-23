using System.Threading;
using Bespoke.SphCommercialSpaces.Domain;

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
    }
}
