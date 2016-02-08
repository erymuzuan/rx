using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class FileResult : IHttpActionResult
    {
        private readonly byte[] m_contents;
        private readonly string m_mimeType;
        private readonly string m_downloadFileName;
        private readonly int m_maxAge;
        private readonly HttpStatusCode m_statusCode;

        public FileResult(byte[] contents, string mimeType, string downloadFileName, int maxAge, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            m_contents = contents;
            m_mimeType = mimeType;
            m_downloadFileName = downloadFileName;
            m_maxAge = maxAge;
            m_statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new ByteArrayContent(m_contents)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(m_mimeType);
            if (!string.IsNullOrWhiteSpace(m_downloadFileName))
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(m_downloadFileName);
            if (m_maxAge > 0)
                response.Headers.CacheControl = new CacheControlHeaderValue { MaxAge = System.TimeSpan.FromSeconds(m_maxAge) };
            return Task.FromResult(response);
        }
    }
}