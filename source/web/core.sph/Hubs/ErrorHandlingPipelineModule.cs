using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR.Hubs;

namespace Bespoke.Sph.Web.Hubs
{
    public class ErrorHandlingPipelineModule : HubPipelineModule
    {
        protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.Log(new LogEntry(exceptionContext.Error));
            base.OnIncomingError(exceptionContext, invokerContext);
        }
    }
}