using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin;
using Newtonsoft.Json;
using AppFunc = System.Func<
System.Collections.Generic.IDictionary<string, object>,
System.Threading.Tasks.Task
>;


namespace Bespoke.Sph.WebApi
{
    public class JwtAuthorizationMiddleware
    {
        private readonly ITokenService m_tokenService;
        private readonly AppFunc m_next;

        public JwtAuthorizationMiddleware(AppFunc next, ITokenService tokenService)
        {
            m_next = next;
            m_tokenService = tokenService;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var ctx = new OwinContext(environment);
            var headers = ctx.Request.Headers.GetValues("Authorization");
            var token = headers?.FirstOrDefault();
            if (token == null)
            {
                var subject = new ClaimsIdentity("Anonymous", ClaimTypes.Anonymous, ClaimTypes.Anonymous);
                subject.AddClaim(new Claim(ClaimTypes.Anonymous, "true", ClaimValueTypes.Boolean));
                var principal = new ClaimsPrincipal(subject);
                ctx.Request.User = principal;
                await m_next(environment);
                return;
            }

            token = token.Replace("Bearer ", "");
            var claim = await m_tokenService.ValidateAsync(token, true);
            if (null == claim)
            {
                ctx.Response.ContentType = "application/json";
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                ctx.Response.ReasonPhrase = "Not Authorized";

                ctx.Response.Write(JsonConvert.SerializeObject(new { message = "invalid authorization header" }));
                return;
            }

            ctx.Request.User = claim;
            await m_next(environment);

        }
    }
}