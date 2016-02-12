using Bespoke.Sph.Domain;
using Owin;

namespace Bespoke.Sph.WebApi
{
    public static class WebApiExtensions
    {
        public static void UseJwt(this IAppBuilder app)
        {
            app.Use<JwtAuthorizationMiddleware>(ObjectBuilder.GetObject<ITokenService>());

        }
    }
}