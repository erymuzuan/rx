using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class JsonCachedResult : IHttpActionResult
    {
        private readonly string m_json;
        private readonly CacheMetadata m_cache;
        private readonly HttpStatusCode m_statusCode;

        public JsonCachedResult(string json, CacheMetadata cache = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            m_json = json;
            m_cache = cache;
            m_statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_json)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            m_cache?.SetMetadata(response);
            return Task.FromResult(response);
        }
    }
}