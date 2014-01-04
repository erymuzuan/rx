using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult AddRole(string role)
        {
            Roles.CreateRole(role);
            return Json(true);
        }

        public async Task<ActionResult> ValidateUserName(string userName)
        {
            var user = Membership.GetUser(userName);
            if (null != user)
                return Json(new { status = "DUPLICATE", message = string.Format("nama pengguna '{0}' sudah digunakan", userName) });
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(true));

        }
        public async Task<ActionResult> ValidateEmail(string email)
        {
            var emailExist = Membership.GetUserNameByEmail(email);
            if (null != emailExist)
                return Json(new { status = "DUPLICATE", message = string.Format("email '{0}' sudah digunakan", email) });
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(true));

        }
        public async Task<ActionResult> AddUser(Profile profile)
        {
            var context = new SphDataContext();
            var designation = await context.LoadOneAsync<Designation>(d => d.Name == profile.Designation);
            if (null == designation) throw new InvalidOperationException("Cannot find designation " + profile.Designation);
            var roles = designation.RoleCollection.ToArray();

            var em = Membership.GetUser(profile.UserName);

            if (null != em)
            {
                var userroles = Roles.GetRolesForUser(profile.UserName);
                if (userroles.Any())
                    Roles.RemoveUserFromRoles(profile.UserName, roles);

                Roles.AddUserToRoles(profile.UserName, profile.Roles);
                em.Email = profile.Email;
                Membership.UpdateUser(em);
                profile.Roles = roles;
                await CreateProfile(profile, designation);
                return Json(profile);
            }


            Membership.CreateUser(profile.UserName, profile.Password, profile.Email);
            Roles.AddUserToRoles(profile.UserName, roles);
            profile.Roles = roles;

            await CreateProfile(profile, designation);

            return Json(profile);
        }


        private static async Task<UserProfile> CreateProfile(Profile profile, Designation designation)
        {
            var context = new SphDataContext();
            var userprofile = await context.LoadOneAsync<UserProfile>(p => p.Username == profile.UserName) ?? new UserProfile();
            userprofile.Username = profile.UserName;
            userprofile.FullName = profile.FullName;
            userprofile.Designation = profile.Designation;
            userprofile.Department = profile.Department;
            userprofile.Mobile = profile.Mobile;
            userprofile.Telephone = profile.Telephone;
            userprofile.Email = profile.Email;
            userprofile.RoleTypes = string.Join(",", profile.Roles);
            userprofile.StartModule = designation.StartModule;

            using (var session = context.OpenSession())
            {
                session.Attach(userprofile);
                await session.SubmitChanges();
            }

            return userprofile;
        }

        public async Task<ActionResult> UpdateUser(UserProfile profile)
        {
            var context = new SphDataContext();
            var userprofile = await context.LoadOneAsync<UserProfile>(p => p.Username == profile.Username)
                ?? new UserProfile { Username = User.Identity.Name };
            userprofile.Email = profile.Email;
            userprofile.Telephone = profile.Telephone;
            userprofile.FullName = profile.FullName;
            userprofile.StartModule = profile.StartModule;

            using (var session = context.OpenSession())
            {
                session.Attach(userprofile);
                await session.SubmitChanges();
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(userprofile));


        }


        public ActionResult ResetPassword(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return Json(new { OK = false, messages = "Please specify new Password" });

            var em = Membership.GetUser(userName);
            if (null == em) return Json(new { OK = false, messages = "User does not exist" });
            if (em.IsLockedOut)
            {
                em.UnlockUser();
            }

            var oldPassword = em.ResetPassword();
            var result = em.ChangePassword(oldPassword, password);
            Membership.UpdateUser(em);
            return Json(new { OK = result, messages = "Password for user has been reset." });
        }
    }

}
