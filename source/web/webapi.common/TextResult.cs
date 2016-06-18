using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class TextResult : IHttpActionResult
    {
        private readonly string m_content;
        private readonly string m_mime;
        private readonly CacheMetadata m_cache;
        private readonly HttpStatusCode m_statusCode;

        public TextResult(string content, string mime = "text/plain" , CacheMetadata cache = null,  HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            m_content = content;
            m_mime = mime;
            m_cache = cache;
            m_statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_content)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(m_mime?? "text/plain");
            m_cache?.SetMetadata(response);
            return Task.FromResult(response);
        }
    }
}