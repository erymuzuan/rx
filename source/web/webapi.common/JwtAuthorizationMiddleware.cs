using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
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
                await m_next(environment);
                return;
            }

            token = token.Replace("Bearer ", "");
            var claim = await m_tokenService.ValidateAsync(token, true);
            if (null != claim)
                ctx.Request.User = claim;

            await m_next(environment);
        }
    }
}