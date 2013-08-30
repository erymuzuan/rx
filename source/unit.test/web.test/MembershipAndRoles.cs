using System;
using System.Diagnostics;
using System.Web.Security;
using Bespoke.SphCommercialSpaces.Domain;
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

        [Test]
        public void AddAdminUser()
        {
            if(Membership.GetUser("admin") != null)return;

            var u = Membership.CreateUser("admin", "123456", "admin@bespoke.com.my");
            Debug.WriteLine(u);

            var profile = new UserProfile
            {
                Username = "admin",
                Department = "admin",
                Email = "admin@bespoke.com.my",
                FullName = "administrator",
                StartModule = "admindashboard"
            };

        }

        [Test]
        public void AddLandAdminUser()
        {

            
        }
    }
}
