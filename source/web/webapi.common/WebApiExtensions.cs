using System;
using System.Security.Claims;
using Bespoke.Sph.Domain;
using Owin;

namespace Bespoke.Sph.WebApi
{
    public static class WebApiExtensions
    {
        public static IAppBuilder UseJwt(this IAppBuilder app)
        {
            app.Use<JwtAuthorizationMiddleware>(ObjectBuilder.GetObject<ITokenService>());

            return app;

        }

        public static bool HasClaims2(this ClaimsPrincipal principal, Predicate<Claim> match)
        {
            if (principal.HasClaim(match))
                return true;
            var identity = principal.Identity as ClaimsIdentity;
            if (null != identity)
            {
                return identity.HasClaim(match);
            }

            return false;
        }
    }
}