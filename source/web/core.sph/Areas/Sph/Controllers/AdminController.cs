﻿using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using static System.IO.File;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "administrators")]
    public class AdminController : Controller
    {
        public ActionResult AddRole(string role)
        {
            var result = Roles.RoleExists(role);
            if (!result)
                Roles.CreateRole(role);

            return Json(!result);
        }

        public ActionResult DeleteRole(string role)
        {
            if (Roles.RoleExists(role))
            {
                var users = Roles.GetUsersInRole(role);
                Roles.RemoveUsersFromRole(users, role);
                Roles.DeleteRole(role);
            }

            return Json(true);
        }
        public ActionResult ValidateUserName(string userName)
        {
            var user = Membership.GetUser(userName);
            if (null != user)
                return Json(new { status = "DUPLICATE", message = $"nama pengguna '{userName}' sudah digunakan" });
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(true));

        }

        public ActionResult ValidateEmail(string email)
        {
            var emailExist = Membership.GetUserNameByEmail(email);
            if (null != emailExist)
                return Json(new { status = "DUPLICATE", message = $"email '{email}' sudah digunakan" });
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(true));

        }

        public async Task<ActionResult> AddUser(Profile profile)
        {
            var context = new SphDataContext();
            var userName = profile.UserName;
            if (string.IsNullOrWhiteSpace(profile.Designation)) throw new ArgumentNullException("Designation for  " + userName + " cannot be set to null or empty");
            var designation = await context.LoadOneAsync<Designation>(d => d.Name == profile.Designation);
            if (null == designation) throw new InvalidOperationException("Cannot find designation " + profile.Designation);
            var roles = designation.RoleCollection.ToArray();

            var em = Membership.GetUser(userName);

            if (null != em)
            {
                profile.Roles = roles;
                em.Email = profile.Email;

                var originalRoles = Roles.GetRolesForUser(userName);
                if (originalRoles.Length > 0)
                    Roles.RemoveUserFromRoles(userName, originalRoles);

                Roles.AddUserToRoles(userName, profile.Roles);
                Membership.UpdateUser(em);
                await CreateProfile(profile, designation);
                return Json(profile);
            }


            Membership.CreateUser(userName, profile.Password, profile.Email);
            Roles.AddUserToRoles(userName, roles);
            profile.Roles = roles;

            await CreateProfile(profile, designation);

            return Json(profile);
        }


        private static async Task<UserProfile> CreateProfile(Profile profile, Designation designation)
        {
            if (null == profile) throw new ArgumentNullException(nameof(profile));
            if (null == designation) throw new ArgumentNullException(nameof(designation));
            if (string.IsNullOrWhiteSpace(designation.Name)) throw new ArgumentNullException(nameof(designation), "Designation Name cannot be null, empty or whitespace");
            if (string.IsNullOrWhiteSpace(profile.UserName)) throw new ArgumentNullException(nameof(profile), "Profile UserName cannot be null, empty or whitespace");

            var context = new SphDataContext();
            var usp = await context.LoadOneAsync<UserProfile>(p => p.UserName == profile.UserName) ?? new UserProfile();
            usp.UserName = profile.UserName;
            usp.FullName = profile.FullName;
            usp.Designation = profile.Designation;
            usp.Department = profile.Department;
            usp.Mobile = profile.Mobile;
            usp.Telephone = profile.Telephone;
            usp.Email = profile.Email;
            usp.RoleTypes = string.Join(",", profile.Roles);
            usp.StartModule = designation.StartModule;
            if (usp.IsNewItem) usp.Id = profile.UserName.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(usp);
                await session.SubmitChanges();
            }

            return usp;
        }

        public async Task<ActionResult> UpdateUser(UserProfile profile)
        {
            var context = new SphDataContext();
            var userprofile = await context.LoadOneAsync<UserProfile>(p => p.UserName == User.Identity.Name)
                ?? new UserProfile();
            userprofile.UserName = User.Identity.Name;
            userprofile.Email = profile.Email;
            userprofile.Telephone = profile.Telephone;
            userprofile.FullName = profile.FullName;
            userprofile.StartModule = profile.StartModule;
            userprofile.Language = profile.Language;

            if (userprofile.IsNewItem) userprofile.Id = userprofile.UserName.ToIdFormat();

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

        [HttpGet]
        public async Task<ActionResult> ExportSecuritySettings()
        {
            var context = new SphDataContext();
            var lo = await context.LoadAsync(context.Designations, includeTotalRows: true);
            var designations = lo.ItemCollection;
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(context.Designations, page: lo.CurrentPage + 1, includeTotalRows: true);
                designations.AddRange(lo.ItemCollection);
            }

            var departments = await context.LoadOneAsync<Setting>(s => s.Key == "Departments");

            var path = Path.Combine(Path.GetTempPath(), "rx.package");
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";

            WriteAllText($"{path}\\departments.json", departments.ToJsonString(true));
            designations.ForEach(x => WriteAllText($"{path}\\designation.{x.Name}.json", x.ToJsonString(true)));
            var roles = Roles.GetAllRoles();
            WriteAllText($"{path}\\roles.txt", string.Join(",", roles));

            if (Exists(zip))
                Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);

            return File(zip, MimeMapping.GetMimeMapping(zip),
                $"rx.developer.securities.settings.{Environment.MachineName}.{DateTime.Now:yyyyMMdd.HHmm}.zip");
        }

        [HttpPost]
        public async Task<ActionResult> Import(string id)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(id);
            if (null == doc) return HttpNotFound($"{id} does not exists in BinaryStore");
            var zipFile = Path.Combine(Path.GetTempPath(), $"{id}.zip");
            var folder = Path.Combine(Path.GetTempPath(), id);
            if (Exists(zipFile))
                Delete(zipFile);
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);
            Directory.CreateDirectory(folder);
            WriteAllBytes(zipFile, doc.Content);

            ZipFile.ExtractToDirectory(zipFile, folder);
            var context = new SphDataContext();
            var existingDesignations = (await context.LoadAsync(context.Designations, 1, 400)).ItemCollection;

            var designations = Directory.GetFiles(folder, "designation.*.json")
                    .Select(f => ReadAllText(f).DeserializeFromJson<Designation>())
                    .ToList();

            var departments = ReadAllText(Path.Combine(folder, "departments.json")).DeserializeFromJson<Setting>();

            using (var session = context.OpenSession())
            {
                session.Attach(departments);
                session.Attach(designations.Cast<Entity>().ToArray());

                foreach (var d in designations)
                {
                    var exist = existingDesignations.SingleOrDefault(x => x.Name == d.Name);
                    if (null != exist)
                    {
                        session.Delete(exist);
                    }
                }


                await session.SubmitChanges("import");
            }

            var roles = ReadAllText(Path.Combine(folder, "roles.txt")).Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(r => !Roles.RoleExists(r))
                .Select(AddRole);

            Delete(zipFile);
            Directory.Delete(folder, true);


            this.Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(new { designations, departments, success = true, status = "OK", roles });

        }
    }

}
