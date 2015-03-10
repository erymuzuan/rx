using System.Web.Security;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class UserProfileViewModel 
    {
        public UserProfile Profile { get; set; }
        public MembershipUser User { get; set; }
        public string[] StartModuleOptions { get; set; }
        public string[] LanguageOptions { get; set; }
    }
}