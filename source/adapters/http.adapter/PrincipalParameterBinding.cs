using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PrincipalParameterBinding : HttpParameterBinding
    {
        public PrincipalParameterBinding(HttpParameterDescriptor p) : base(p) { }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            IPrincipal p = Thread.CurrentPrincipal;
            SetValue(actionContext, p);


            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }
}