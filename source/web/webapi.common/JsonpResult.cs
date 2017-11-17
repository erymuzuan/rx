using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class JsonpResult : IHttpActionResult
    {
        private readonly string m_script;
        private readonly HttpStatusCode m_statusCode;

        public JsonpResult(HttpStatusCode statusCode, string script)
        {
            m_statusCode = statusCode;
            m_script = script;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_script)
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/javascript");
            return Task.FromResult(response);
        }
    }
}