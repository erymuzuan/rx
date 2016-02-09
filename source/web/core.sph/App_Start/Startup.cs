using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Hubs;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json;

[assembly: OwinStartup(typeof(Startup))]
namespace Bespoke.Sph.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "rx.developer",
                LoginPath = new PathString("/sph/sphaccount/login")
            });
            app.MapSignalR();
            app.MapSignalR<MessageConnection>("/signalr_message");
            app.MapSignalR<SolutionConnection>("/signalr_solution");
            app.MapSignalR<AuditTrailConnection>("/signalr_audittrail");

            var config = new HttpConfiguration();
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

            app.UseWebApi(config);

        }

    }
}