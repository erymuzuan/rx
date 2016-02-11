using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;
using Roles = System.Web.Security.Roles;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "administrators")]
    [RoutePrefix("custom-token")]
    public class CustomTokenController : BaseController
    {

        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAll(int page = 1, int size = 20)
        {
            var repos = ObjectBuilder.GetObject<ITokenRepository>();
            var lo = await repos.LoadAsync(DateTime.Today, page, size);
            var tokens = from t in lo.ItemCollection
                         select t.ToJson();
            var json = $"[{string.Join(",", tokens)}]";

            return Content(json, "application/json");
        }
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> GetTokenAsync([RequestBody]GetTokenModel model)
        {
            if (model.grant_type == "password" && !Membership.ValidateUser(model.username, model.password))
                return Json(new { success = false, status = 403, message = "Cannot validate your username or password" });

            if (model.grant_type == "admin" && !User.IsInRole("administrators"))
                return Json(new { success = false, status = 403, message = "You are not in administrator role" });


            var tokenService = ObjectBuilder.GetObject<ITokenService>();
            
            var id = Strings.GenerateId();
            var context = new SphDataContext();
            var user = await context.LoadOneAsync<UserProfile>(x => x.UserName == model.username);
            var roles = Roles.GetRolesForUser(model.username);

            var encryptedToken = await tokenService.CreateTokenAsync(user, roles, model.expiry);
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
                                                     encryptedToken,
                                                     Convert.ToInt32((model.expiry-DateTime.Now).TotalSeconds),
                                                     DateTime.Now,
                                                     model.expiry,
                                                     model.username,
                                                     id);

            return Content(json, "application/json");
        }

        [Authorize(Roles = "administrators")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult> GetTokenAsync(string id)
        {
            var repos = ObjectBuilder.GetObject<ITokenRepository>();
            var token = await repos.LoadOneAsync(id);
            if (null == token) return HttpNotFound();

            return Content(token.GenerateToken());
        }

        [Authorize(Roles = "administrators")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> RemoveTokenAsync(string id)
        {
            var repos = ObjectBuilder.GetObject<ITokenRepository>();
            await repos.RemoveAsync(id);
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