using System.Diagnostics;
using System.Web.Security;
using Newtonsoft.Json;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class MembershipAndRoles
    {
        [Test]
        public void AddRoles()
        {
            var json = System.IO.File.ReadAllText(@"./roles.config.js");
            var jsonArrays = JsonConvert.DeserializeObject(json) as Newtonsoft.Json.Linq.JArray;
            foreach (var r in jsonArrays)
            {
                var role = r.Value<string>("Role");
                Debug.WriteLine(role);
                if (!Roles.IsUserInRole("admin", role))
                    Roles.AddUserToRole("admin", role);
            }

        }

    }
}
