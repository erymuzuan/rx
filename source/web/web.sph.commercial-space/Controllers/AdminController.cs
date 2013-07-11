using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Commerspace.Web.Models;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult AddRole(string role)
        {
            Roles.CreateRole(role);
            return Json(true);
        }
        public async Task<ActionResult> AddUser(Profile profile)
        {
            var context = new SphDataContext();
            var setting = await context.LoadOneAsync<Setting>(s => s.Key == "Designation.Role");
            var designation = await JsonConvert.DeserializeObjectAsync<Designation[]>(setting.Value);
            var userRoles = designation.Where(d => d.Name == profile.Designation).Select(d => d.Roles);
            var em = Membership.GetUser(profile.UserName);
            var role = userRoles.FirstOrDefault();
            if (null != em)
            {
                var roles = Roles.GetRolesForUser(profile.UserName);
                if (roles.Any())
                    Roles.RemoveUserFromRoles(profile.UserName, roles);

                Roles.AddUserToRoles(profile.UserName, role);
                em.Email = profile.Email;
                Membership.UpdateUser(em);
                var user= await CreateProfile(profile);
                return Json(user);
            }
            Membership.CreateUser(profile.UserName, profile.Password, profile.Email);
            Roles.AddUserToRoles(profile.UserName, role);
            profile.Roles = role;
           var userprofile = await CreateProfile(profile);

           return Json(userprofile);
        }

        private static async Task<UserProfile> CreateProfile(Profile profile)
        {
            var context = new SphDataContext();
            var userprofile = new UserProfile
                {
                    Username = profile.UserName,
                    FullName = profile.FullName,
                    Designation = profile.Designation,
                    Department = profile.Department,
                    Mobile = profile.Mobile,
                    Telephone = profile.Telephone,
                    Email = profile.Email,
                    RoleTypes = string.Join(",",profile.Roles)
                };

            using (var session = context.OpenSession())
            {
                session.Attach(userprofile);
                await session.SubmitChanges();
            }

            return userprofile;
        }

        public ActionResult LoadRoles()
        {
            var roleCollection = new List<string>();
            var roles = Roles.GetAllRoles();
            roleCollection.AddRange(roles);
            return Json(roleCollection);
        }
    }

    public class Designation
    {
        public string Name { get; set; }
        public string[] Roles { get; set; }
    }
}
