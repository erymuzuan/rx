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
        private readonly LogEntry m_logEntry;
        private readonly string m_logEntryId;

        public LocalExceptionResult(string logEntryId)
        {
            m_logEntryId = logEntryId;
        }
        public LocalExceptionResult(LogEntry logEntry)
        {
            m_logEntry = logEntry;
        }
      

        Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
        {
            JsonContent content = null;
            if (!string.IsNullOrWhiteSpace(m_logEntryId))
                content = new JsonContent(new { id = m_logEntryId, message = "Internal Server Error, please call your administrator with the Log Id" });

            if(null != m_logEntry)
                content = new JsonContent(m_logEntry);
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