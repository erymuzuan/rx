using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class NotModifiedResult : IHttpActionResult
    {
        private readonly HttpRequestMessage m_request;
        public NotModifiedResult(HttpRequestMessage request)
        {
            m_request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (m_request.Headers.IfNoneMatch.Count == 0 || m_request.Headers.IfModifiedSince == null)
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) {Content = new StringContent("Missing If-none-match or If-Modified-Since header") });


            var response = new HttpResponseMessage(HttpStatusCode.NotModified)
            {
                Content = new StringContent("") 
            };
            return Task.FromResult(response);
        }
    }
    public class HtmlResult : IHttpActionResult
    {
        private readonly string m_html;
        private readonly HttpStatusCode m_statusCode;

        public HtmlResult(HttpStatusCode statusCode, string html)
        {
            m_statusCode = statusCode;
            m_html = html;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_html) 
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return Task.FromResult(response);
        }
    }
}