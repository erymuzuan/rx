using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Hubs;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]
namespace Bespoke.Sph.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.MapSignalR<MessageConnection>("/signalr_message");
            app.MapSignalR<SolutionConnection>("/signalr_solution");
            app.MapSignalR<AuditTrailConnection>("/signalr_audittrail");

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);

        }
        
    }
}