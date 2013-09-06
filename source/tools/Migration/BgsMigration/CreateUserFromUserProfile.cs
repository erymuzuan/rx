using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Migration.Sph
{
    public class CreateUserFromUserProfile
    {
        static void Main()
        {
            Console.WriteLine("Create Membership");
            CreateMembership()
               .ContinueWith(
                   _ =>
                   {
                       if (_.IsFaulted)
                       {
                           Console.WriteLine("*****************ERROR*************");
                           Console.WriteLine(_.Exception);
                       }
                       else
                       {
                           Console.WriteLine("Successfull");
                       }
                   })
               .Wait(TimeSpan.FromMinutes(5));
            
        }

        private static async Task CreateMembership()
        {
            var context = new SphDataContext();
            var userprofile = await context.LoadAsync(context.UserProfiles.Where(t => t.UserProfileId >10));
            const string defaultPassword = "123456";

                //Membership.CreateUser(userprofile.Username, defaultPassword, userprofile.Email);
                //Roles.AddUserToRoles(userprofile.Username, new[] { "can_view_tenant_details" });
                  
        }
    }
}
