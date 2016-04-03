using System;
using Bespoke.Sph.Web.App_Start;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.OwinMiddlewares;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.WebApi;

[assembly: OwinStartup(typeof(Startup))]
namespace Bespoke.Sph.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ObjectBuilder.AddCacheList<IEndpointPermissionRepository>(new EndpointPermissionRepository());
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
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ResourceAuthorizeAttribute());
            config.Services.Replace(typeof(IExceptionHandler), ObjectBuilder.GetObject<IExceptionHandler>());
            config.EnsureInitialized();

            app.UseResourceAuthorization(new CustomPolicyAuthorizationManager());

            app.UseJwt()
                .UseApiMetering()
                .UseWebApi(config);

        }


    }
}