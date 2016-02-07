using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class InvalidResult : IHttpActionResult
    {
        private readonly string m_message;
        private readonly HttpStatusCode m_statusCode;

        public InvalidResult(HttpStatusCode statusCode, string message)
        {
            m_statusCode = statusCode;
            m_message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_message)
            };
            return Task.FromResult(response);
        }
    }
}