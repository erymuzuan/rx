using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
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