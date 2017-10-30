using System;
using System.Collections.Generic;
using Bespoke.Sph.Web.App_Start;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.OwinMiddlewares;
using Bespoke.Sph.WebApi;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.WebApi;

[assembly: OwinStartup(typeof(Startup))]
namespace Bespoke.Sph.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                // TODO : these login/logout path should be set only for ASP.Net MVC view
                //LoginPath = new PathString("/sph/sphaccount/login"),
                //LogoutPath = new PathString("/sph/sphaccount/logoff"),
                //ReturnUrlParameter = "returnUrl",
                AuthenticationType = ConfigurationManager.ApplicationName + "Cookie",
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(30),
                SlidingExpiration =  true,
                CookieName = $".{ConfigurationManager.ApplicationName}.Cookie"
            });

            app.RegisterCustomEntityDependencies()
                .UseCoreResource(true)
                .MapSignalRConnection();


            var config = new HttpConfiguration();
            config.MessageHandlers.Add(new MethodOverrideHandler());

            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            setting.Formatting = Formatting.Indented;

            config.Formatters.JsonFormatter.SerializerSettings = setting;
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());

            config.Filters.Add(new ResourceAuthorizeAttribute());
            config.Services.Replace(typeof(IExceptionHandler), ObjectBuilder.GetObject<IExceptionHandler>());
            config.EnsureInitialized();

            app.UseResourceAuthorization(new CustomPolicyAuthorizationManager());

            app.UseJwt()
                .UseApiMetering()
                .UseWebApi(config);

        }


    }

    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
            GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            // inherit route attributes decorated on base class controller's actions
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>
                (inherit: true);
        }
    }
}