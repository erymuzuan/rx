using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SignalRHub))]

namespace Bespoke.Sph.Web.App_Start
{
    public class SignalRHub
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.MapSignalR<MessageConnection>("/signalr_message");
            app.MapSignalR<SolutionConnection>("/signalr_solution");
            app.MapSignalR<AuditTrailConnection>("/signalr_audittrail");
        }
        
    }
}