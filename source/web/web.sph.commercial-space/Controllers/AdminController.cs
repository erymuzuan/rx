using System;
using System.Collections.Generic;
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

        public async Task<ActionResult> ValidateUserName(string userName)
        {
            await Task.Delay(2000);
            var user = Membership.GetUser(userName);
            if (null != user)
                return Json(new {status = "DUPLICATE", message = string.Format("Username '{0}' already exists!!!!",userName)});

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(true));

        }

        public async Task<ActionResult> AddUser(Profile profile)
        {
            var context = new SphDataContext();
            var designation = await context.LoadOneAsync<Designation>(d => d.Name == profile.Designation);
            if (null == designation) throw new InvalidOperationException("Cannot find designation " + profile.Designation);
            var roles = designation.RoleCollection.ToArray();

            Membership.CreateUser(profile.UserName, profile.Password, profile.Email);
            Roles.AddUserToRoles(profile.UserName, roles);
            profile.Roles = roles;
            var userprofile = await CreateProfile(profile);

            return Json(userprofile);
        }

        private static async Task<UserProfile> CreateProfile(Profile profile)
        {
            var context = new SphDataContext();
            var userprofile = await context.LoadOneAsync<UserProfile>(p => p.Username == profile.UserName)?? new UserProfile();
            userprofile.Username = profile.UserName;
            userprofile.FullName = profile.FullName;
            userprofile.Designation = profile.Designation;
            userprofile.Department = profile.Department;
            userprofile.Mobile = profile.Mobile;
            userprofile.Telephone = profile.Telephone;
            userprofile.Email = profile.Email;
            userprofile.RoleTypes = string.Join(",", profile.Roles);

            using (var session = context.OpenSession())
            {
                session.Attach(userprofile);
                await session.SubmitChanges();
            }

            return userprofile;
        }

        public async Task<ActionResult> UpdateUser(Profile profile)
        {
            var context = new SphDataContext();
            profile.Roles = roles;
            var userprofile = await CreateProfile(profile);

            return Json(userprofile);
        }
    }

}
