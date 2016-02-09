using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public class LocalExceptionResult : IHttpActionResult
    {
        private readonly Exception m_exception;
        private readonly string m_log;

        public LocalExceptionResult(string log)
        {
            m_log = log;
        }
        public LocalExceptionResult(Exception exception)
        {
            m_exception = exception;
        }
        public LocalExceptionResult(Exception exception, string log)
        {
            m_exception = exception;
            m_log = log;
        }

        Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
        {
            JsonContent content = null;
            if (!string.IsNullOrWhiteSpace(m_log))
                content = new JsonContent(new { id = m_log, message = "Internal Server Error, please call your administrator with the Log Id" });

            if(null != m_exception)
                content = new JsonContent(new LogEntry(m_exception));
            if(null == content)
                throw new InvalidOperationException("You have to set the log id or the exception");
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = content
            };


            return Task.FromResult(response);
        }
    }
}