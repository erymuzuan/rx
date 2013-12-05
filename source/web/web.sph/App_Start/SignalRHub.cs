using System;
using Bespoke.Sph.Web.Hubs;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Bespoke.Sph.Web.App_Start.SignalRHub))]

namespace Bespoke.Sph.Web.App_Start
{
    public class SignalRHub
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.MapSignalR<MessageConnection>("/signalr_message");
            Console.WriteLine("Runing.....SignalRHub XXXXXXXXXXXX");
        }
    }
}