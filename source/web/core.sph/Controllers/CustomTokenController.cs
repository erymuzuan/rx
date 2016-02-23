using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("api/auth-tokens")]
    public class CustomTokenController : BaseApiController
    {

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 20)
        {
            var repos = ObjectBuilder.GetObject<ITokenRepository>();
            var lo = await repos.LoadAsync(DateTime.Today, page, size);
            var tokens = from t in lo.ItemCollection
                         select t.ToJson();
            var json = $"[{string.Join(",", tokens)}]";

            return Json(json);
        }

        [Authorize(Roles = "administrators,developers")]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> CreateTokenAsync([RequestBody]GetTokenModel model)
        {
            if (model.grant_type == "password" && !Membership.ValidateUser(model.username, model.password))
                return Json(new { success = false, status = 403, message = "Cannot validate your username or password" });

            if (model.grant_type == "admin" && !User.IsInRole("administrators"))
                return Json(new { success = false, status = 403, message = "You are not in administrator role" });


            var tokenService = ObjectBuilder.GetObject<ITokenService>();

            var context = new SphDataContext();
            var user = await context.LoadOneAsync<UserProfile>(x => x.UserName == model.username);
            var roles = Roles.GetRolesForUser(model.username);

            var claim = await tokenService.CreateTokenAsync(user, roles, model.expiry);
            var token = claim.GenerateToken();
            var json = claim.ToJson()
                .Replace("\"WebId\"", $"\"token\":\"{token}\",\r\n\"WebId\"");

            return Json(json);
        }

        [Authorize(Roles = "administrators,developers")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> GetTokenAsync(string id)
        {
            var repos = ObjectBuilder.GetObject<ITokenRepository>();
            var token = await repos.LoadOneAsync(id);
            if (null == token) return NotFound();

            return Ok(token.GenerateToken());
        }

        [Authorize(Roles = "administrators,developers")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> RemoveTokenAsync(string id)
        {
            var repos = ObjectBuilder.GetObject<ITokenRepository>();
            await repos.RemoveAsync(id);
            return Json(new { success = true, status = 200, message = id + " has been successfully deleted" });
        }

        [Route("test")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidateToken()
        {
            var headers = this.Request.Headers.GetValues("Authorization");
            if (null == headers)
                return Json(new { status = 403, message = "No authorization headers" });
            var token = headers.FirstOrDefault();
            if (null == token)
                return Json(new { status = 403, message = "No authorization header" });

            token = token.Replace("Bearer ", "");

            var tokenService = ObjectBuilder.GetObject<ITokenService>();
            var claim = await tokenService.ValidateAsync(token, true);
            if (null == claim)
                return Unauthorized(new AuthenticationHeaderValue("Authorization", "Bearer"));
            return Json(new
            {
                status = true,
                name = claim.Identity.Name,
                email = claim.FindFirst(c => c.Type == ClaimTypes.Email).Value,
                developers = claim.IsInRole("developers"),
                administrators = claim.IsInRole("administrators")
            });

        }

        [Authorize]
        [HttpGet]
        [Route("test-protected-resource")]
        public IHttpActionResult AuthorizedResource()
        {
            return Json(new
            {
                status = 200,
                message = "secret message",
                username = User.Identity.Name,
                roles = Roles.GetRolesForUser()
            });
        }
        [Authorize(Roles = "developers")]
        [HttpGet]
        [Route("test-protected-resource/developers")]
        public IHttpActionResult AuthorizedResourceWithRole()
        {
            return Json(new
            {
                status = 200,
                message = "secret message",
                username = User.Identity.Name,
                roles = Roles.GetRolesForUser()
            });
        }

    }
}