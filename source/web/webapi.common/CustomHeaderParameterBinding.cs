using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Bespoke.Sph.WebApi
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class CustomHeaderAttribute : ParameterBindingAttribute
    {
        private readonly string m_header;
        private readonly bool m_useX;

        public CustomHeaderAttribute(string header, bool useX = true)
        {
            m_header = header;
            m_useX = useX;
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new CustomHeaderParameterBinding(parameter, m_header, m_useX);
        }
    }

    public class CustomHeaderParameterBinding : HttpParameterBinding
    {
        private readonly string m_header;
        private readonly bool m_useX;

        public CustomHeaderParameterBinding(HttpParameterDescriptor parameter, string header, bool useX = true)
            : base(parameter)
        {
            m_header = header;
            m_useX = useX;
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var x = m_useX ? "X-" : "";
            var values = actionContext.Request.Headers.GetValues($"{x}{m_header}");
            actionContext.ActionArguments[Descriptor.ParameterName] = values;

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }
}