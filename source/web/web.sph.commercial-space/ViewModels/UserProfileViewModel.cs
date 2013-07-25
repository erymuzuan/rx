using System.Linq;
using System.Web.Security;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.ViewModels
{
    public class UserProfileViewModel 
    {
        public UserProfile Profile { get; set; }
        public MembershipUser User { get; set; }
        public string[] StartModuleOptions { get; set; }
    }
}