using Bespoke.Sph.Web.App_Start;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.OwinMiddlewares;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

[assembly: OwinStartup(typeof(Startup))]
namespace Bespoke.Sph.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
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


            config.Services.Replace(typeof(IExceptionHandler), ObjectBuilder.GetObject<IExceptionHandler>());
            config.EnsureInitialized();

            app.UseJwt()
                .UseWebApi(config);

        }


    }
}