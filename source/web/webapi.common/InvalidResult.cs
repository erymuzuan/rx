using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bespoke.Sph.WebApi
{
    public class NotFoundTextPlainActionResult : IHttpActionResult
    {
        public NotFoundTextPlainActionResult(string message, HttpRequestMessage request)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Message = message;
            Request = request;
        }

        public string Message { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
            response.Content = new StringContent(Message); // Put the message in the response body (text/plain content).
            response.RequestMessage = Request;
            return response;
        }
    }
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