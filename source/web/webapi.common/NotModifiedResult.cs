using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class NotModifiedResult : IHttpActionResult
    {
        private readonly HttpRequestMessage m_request;
        private readonly CacheMetadata m_cache;

        public NotModifiedResult(HttpRequestMessage request, CacheMetadata cache)
        {
            m_request = request;
            m_cache = cache;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (m_request.Headers.IfNoneMatch.Count == 0 && m_request.Headers.IfModifiedSince == null)
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = new StringContent("Missing If-None-Match or If-Modified-Since header") });


            var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                Content = new StringContent("") 
            };
            m_cache?.SetMetadata(response);
            return Task.FromResult(response);
        }
    }
}