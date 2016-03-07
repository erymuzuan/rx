using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Bespoke.Sph.WebApi
{
    public class OkCachedResult<T> : OkNegotiatedContentResult<T>
    {
        private readonly CacheMetadata m_cache;

        public OkCachedResult(T content, CacheMetadata cache, ApiController controller) : base(content, controller)
        {
            m_cache = cache;
        }

        public new async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await base.ExecuteAsync(cancellationToken);
            m_cache?.SetMetadata(response);
            return response;
        }

        
    }
}