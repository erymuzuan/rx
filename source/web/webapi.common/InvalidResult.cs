using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{
    public class InvalidResult : IHttpActionResult
    {
        private readonly HttpRequestMessage m_request;
        private readonly BusinessRule[] m_brokenRules;
        private readonly string m_message;
        private readonly HttpStatusCode m_statusCode;

        public InvalidResult(HttpStatusCode statusCode, string message)
        {
            m_statusCode = statusCode;
            m_message = message;
        }
        public InvalidResult(HttpRequestMessage request, BusinessRule[] brokenRules)
        {
            m_request = request;
            m_brokenRules = brokenRules;
            m_statusCode = (HttpStatusCode) 422;
            m_message = $"You have {brokenRules.Length} business rules broken";
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_message)
            };
            if (null != m_brokenRules)
            {
                response.Content = new StringContent(JsonConvert.SerializeObject(m_brokenRules));
            }
            return Task.FromResult(response);
        }
    }
}