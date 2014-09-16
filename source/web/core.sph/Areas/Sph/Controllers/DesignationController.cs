using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Roles = System.Web.Security.Roles;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("sph-designation")]
    public class DesignationController : Controller
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings).Select(r => r.Role).ToArray();

            var designation = this.GetRequestJson<Designation>();
            var newItem = designation.IsNewItem;
            if (newItem)
            {
                designation.Id = designation.Name.ToIdFormat();
            }

            var context = new SphDataContext();
            var lo = await context.LoadAsync(context.UserProfiles.Where(u => u.Designation == designation.Name));
            foreach (var user in lo.ItemCollection)
            {
                user.RoleTypes = string.Join(",", designation.RoleCollection);
                user.StartModule = designation.StartModule;

                var member = Membership.GetUser(user.UserName);
                Message message = null;
                if (null == member)
                {
                    var password = Guid.NewGuid().ToString().Substring(0, 8);
                    Membership.CreateUser(user.UserName, password, user.Email);

                    message = new Message
                    {
                        Subject = "User is created for " + user.UserName,
                        Body = "Password is " + password,
                        UserName = User.Identity.Name
                    };

                }
                using (var session = context.OpenSession())
                {
                    if (null != message)
                        session.Attach(message);

                    session.Attach(user);
                    await session.SubmitChanges("Update user designation");
                }

            }
            // add or remove roles for the users
            var userNames = lo.ItemCollection.Select(u => u.UserName).ToArray();
            foreach (var role in roles.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                if (!Roles.RoleExists(role))
                    Roles.CreateRole(role);
                foreach (var user in userNames.Where(s => !string.IsNullOrWhiteSpace(s)))
                {
                    try
                    {
                        Roles.RemoveUserFromRole(user, role);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }

                }
            }
            foreach (var role in designation.RoleCollection)
            {
                if (!Roles.RoleExists(role))
                    Roles.CreateRole(role);
            }
            foreach (var user in userNames.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                Roles.AddUserToRoles(user, designation.RoleCollection.ToArray());
            }

            using (var session = context.OpenSession())
            {
                session.Attach(designation);
                await session.SubmitChanges("Add/update new designation");
            }
            var id = designation.Id;

            this.Response.ContentType = "application/json; charset=utf-8";
            this.Response.StatusCode = newItem ? 201 : 202;
            var location = string.Format("{0}/sph-designation/{1}", ConfigurationManager.BaseUrl, id);
            this.Response.AddHeader("Location", location);
            return Content(JsonConvert.SerializeObject(new
            {
                status = "OK",
                success = true,
                links = new object[]
                {
                    new
                    {
                        rel = "self",
                        href = "/sph-designation/" + id
                    },
                    new
                    {
                        rel = "lock",
                        href = "/sph-designation/" + id + "/lock"
                    }
                }
            }));

        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetOneAsync(string id)
        {
            var context = new SphDataContext();
            var dsg = await context.LoadOneAsync<Designation>(x => x.Id == id);
            var result = new
            {
                designation = dsg,
                links = new object[]
                {
                    new
                    {
                        rel = "self",
                        href = "/sph-designation/" + id
                    },
                    new
                    {
                        rel = "lock",
                        href = string.Format("/sph-designation/{0}/lock", id)
                    }
                }
            };

            return Content(JsonConvert.SerializeObject(result), "application/json", Encoding.UTF8);

        }
    }
}
