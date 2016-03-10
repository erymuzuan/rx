using System;
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
            actionContext.ActionArguments[Descriptor.ParameterName] = new ModifiedSinceHeader(gmt);

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }

    public class ModifiedSinceHeader
    {
        public DateTimeOffset? Offset { get; set; }

        public ModifiedSinceHeader(DateTimeOffset? offset)
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

            DateTime dateTime;
            if (DateTime.TryParse(input, out dateTime))
            {
                return this.IsMatch(dateTime);
            }
            return false;
        }
    }
}