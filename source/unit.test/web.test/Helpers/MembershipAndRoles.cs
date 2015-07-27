using System.Web.Security;
using NUnit.Framework;

namespace Bespoke.Sph.WebTests.Helpers
{
    [TestFixture]
    public class MembershipAndRoles
    {
        [Test]
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
