using Bespoke.Sph.Domain;
using Owin;

namespace Bespoke.Sph.WebApi
{
    public static class WebApiExtensions
    {
        public static void UseJwt(this IAppBuilder app)
        {
            var tokenService =(JwtMiddleware) ObjectBuilder.GetObject<ITokenService>();
            tokenService.Configure(app);

        }
    }
}