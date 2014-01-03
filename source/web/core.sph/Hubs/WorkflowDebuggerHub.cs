using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    public class WorkflowDebuggerHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}