using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Bespoke.Sph.WebApi
{
    public class ModifiedSinceParameterBinding : HttpParameterBinding
    {

        public ModifiedSinceParameterBinding(HttpParameterDescriptor parameter)
            : base(parameter)
        {
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var gmt = actionContext.Request.Headers.IfModifiedSince;
            if (gmt.HasValue)
                actionContext.ActionArguments[Descriptor.ParameterName] = gmt.Value.LocalDateTime;
            else
                actionContext.ActionArguments[Descriptor.ParameterName] = null;

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }
}