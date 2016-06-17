using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class BinaryResult : IHttpActionResult
    {
        private readonly byte[] m_content;
        private readonly string m_mimeType;
        private readonly CacheMetadata m_cache;
        private readonly HttpStatusCode m_statusCode;

        public BinaryResult(byte[] content, string mimeType, CacheMetadata cache = null,  HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            m_content = content;
            m_mimeType = mimeType;
            m_cache = cache;
            m_statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new ByteArrayContent(m_content)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(m_mimeType);
            m_cache?.SetMetadata(response);
            return Task.FromResult(response);
        }
    }
}