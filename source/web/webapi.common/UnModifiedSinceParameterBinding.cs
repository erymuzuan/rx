using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Bespoke.Sph.WebApi
{
    public class UnmodifiedSinceParameterBinding : HttpParameterBinding
    {

        public UnmodifiedSinceParameterBinding(HttpParameterDescriptor parameter)
            : base(parameter)
        {
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var gmt = actionContext.Request.Headers.IfUnmodifiedSince;
            actionContext.ActionArguments[Descriptor.ParameterName] = new UnmodifiedSinceHeader(gmt);

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }

    public class UnmodifiedSinceHeader
    {
        public DateTimeOffset? Offset { get; set; }

        public UnmodifiedSinceHeader(DateTimeOffset? offset)
        {
            Offset = offset;
        }

        public bool IsMatch(DateTime dateTime)
        {
            if (!this.Offset.HasValue) return false;
            var header = $"{this.Offset.Value.LocalDateTime:s}";
            var actual = dateTime.ToString("s");
            return header == actual;
        }
        public bool IsMatch(DateTime? dateTime)
        {
            return dateTime.HasValue && this.IsMatch(dateTime.Value);
        }

        public bool IsMatch(string input)
        {
            return DateTime.TryParse(input, out var dateTime) && this.IsMatch(dateTime);
        }
    }
}