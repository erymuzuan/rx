using System.Threading;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Dependencies
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