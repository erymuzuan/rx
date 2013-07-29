using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.CustomTriggers
{
    class ActiveDirectory : IDirectoryService
    {
        public string CurrentUserName { get { return "Trigger"; } }
    }
}
