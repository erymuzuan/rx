using System.Diagnostics;
using System.Web.Security;
using Bespoke.SphCommercialSpaces.Domain;
using NUnit.Framework;

namespace web.test
{
   public class Init
    {
       public const string SPH_DATABASE = "sph";
       [Test]
       public void AddAdminUser()
       {
           if (Membership.GetUser("admin") != null) return;

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
    }
}
