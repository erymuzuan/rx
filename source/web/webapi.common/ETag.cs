using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Bespoke.Sph.WebApi
{
    public abstract class ETagMatchAttribute : ParameterBindingAttribute
    {
        private readonly ETagMatch m_match;

        protected ETagMatchAttribute(ETagMatch match)
        {
            m_match = match;
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(ETag))
            {
                return new ETagParameterBinding(parameter, m_match);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }

    public class IfMatchAttribute : ETagMatchAttribute
    {
        public IfMatchAttribute()
            : base(ETagMatch.IfMatch)
        {
        }
    }

    public class IfNoneMatchAttribute : ETagMatchAttribute
    {
        public IfNoneMatchAttribute()
            : base(ETagMatch.IfNoneMatch)
        {
        }
    }
    public class ETagParameterBinding : HttpParameterBinding
    {
        readonly ETagMatch m_match;

        public ETagParameterBinding(HttpParameterDescriptor parameter, ETagMatch match)
            : base(parameter)
        {
            m_match = match;
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            EntityTagHeaderValue etagHeader = null;
            switch (m_match)
            {
                case ETagMatch.IfNoneMatch:
                    etagHeader = actionContext.Request.Headers.IfNoneMatch.FirstOrDefault();
                    break;

                case ETagMatch.IfMatch:
                    etagHeader = actionContext.Request.Headers.IfMatch.FirstOrDefault();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ETag etag = null;
            if (etagHeader != null)
            {
                etag = new ETag { Tag = etagHeader.Tag };
            }
            actionContext.ActionArguments[Descriptor.ParameterName] = etag;

            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }
    }

    public enum ETagMatch
    {
        IfMatch,
        IfNoneMatch
    }

    public class ETag
    {
        public string Tag { get; set; }
    }

}
