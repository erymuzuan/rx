using System.Threading;
using System.Web;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Dependencies
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