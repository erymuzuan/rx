using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Areas.Sph.Controllers;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Roles = System.Web.Security.Roles;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("custom-token")]
    public class CustomTokenController : BaseController
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> GetTokenAsync([RequestBody]GetTokenModel model)
        {
            if (model.grant_type == "password" && !Membership.ValidateUser(model.username, model.password))
                return Json(new { success = false, status = 403, message = "Cannot validate your username or password" });

            if (model.grant_type == "admin" && !User.IsInRole("administrators"))
                return Json(new { success = false, status = 403, message = "You are not in administrator role" });

            var expiresIn = TimeSpan.FromDays(14).TotalSeconds;
            if (model.expiry != default(DateTime))
                expiresIn = (model.expiry - DateTime.Now).TotalSeconds;

            var id = Strings.GenerateId();
            var st = new SphSecurityToken
            {
                Username = model.username,
                Issued = DateTime.Now,
                Expired = DateTime.Now.AddSeconds(expiresIn),
                Roles = Roles.GetRolesForUser(model.username),
                Id = id
            };

            var tokenJson = st.ToJsonString(true);
            var encryptedToken = (new Encryptor()).Encrypt(tokenJson);
            var json = string.Format(@"{{
    ""success"": true,
    ""id"":""{5}"",
    ""access_token"":""{0}"",
    ""token_type"":""bearer"",
    ""expires_in"":{1},
    ""userName"":""{4}"",
    "".issued"":""{2:R}"",
    "".expires"":""{3:R}""
}}",
                                                     encryptedToken, Convert.ToInt32(expiresIn),
                                                     st.Issued,
                                                     st.Expired,
                                                     model.username,
                                                     id);
            var setting = new Setting
            {
                UserName = model.username,
                Key = "Token",
                Value = tokenJson,
                Id = id
            };
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(setting);
                await session.SubmitChanges("token");
            }
            this.Response.ContentType = "application/json";
            return Content(json);
        }

        [Authorize(Roles = "administrators")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult> GetTokenAsync(string id)
        {
            var context = new SphDataContext();
            var setting = await context.LoadOneAsync<Setting>(x => x.Id == id);

            return Content((new Encryptor()).Encrypt(setting.Value));
        }

        [Authorize(Roles = "administrators")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> RemoveTokenAsync(string id)
        {
            var context = new SphDataContext();
            var setting = await context.LoadOneAsync<Setting>(x => x.Id == id);
            if (null == setting)
                return new HttpNotFoundResult("Cannot find token with id " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(setting);
                await session.SubmitChanges("token");
            }
            return Json(new { success = true, status = 200, message = id + " has been successfully deleted" });
        }

        [Route("test")]
        public ActionResult ValidateToken()
        {
            var headers = this.Request.Headers.GetValues("Authorization");
            if (null == headers)
                return Json(new { status = 403, message = "No authorization headers" }, JsonRequestBehavior.AllowGet);
            var token = headers.FirstOrDefault();
            if (null == token)
                return Json(new { status = 403, message = "No authorization header" }, JsonRequestBehavior.AllowGet);

            try
            {
                var json = (new Encryptor()).Decrypt(token);
                var st = json.DeserializeFromJson<SphSecurityToken>();
                return Json(new { status = 200, username = st.Username, expired = st.Expired.ToString("R"), issued = st.Issued.ToString("R") }, JsonRequestBehavior.AllowGet);
            }
            catch (CryptographicException)
            {
                return Json(new { status = 403, message = "Cannot validate your token" }, JsonRequestBehavior.AllowGet);
            }
            catch (JsonReaderException)
            {
                return Json(new { status = 403, message = "Cannot validate your token" }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [Route("auth")]
        public ActionResult AuthorizedResource()
        {
            return Json(new
            {
                status = 200,
                message = "secret message",
                username = User.Identity.Name,
                roles = Roles.GetRolesForUser()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}