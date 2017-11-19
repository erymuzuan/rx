using System.Web.Security;
using Xunit;

namespace Bespoke.Sph.WebTests.Helpers
{
    public class MembershipAndRoles
    {
        [Fact]
        public void AddRoles()
        {
            var roles = new string[] {"developers", "administrators"};
            foreach (var r in roles)
            {
                if (!Roles.IsUserInRole("admin", r))
                    Roles.AddUserToRole("admin", r);
            }

        }

    }
}
