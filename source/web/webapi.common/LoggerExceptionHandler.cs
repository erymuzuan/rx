using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public class LocalExceptionResult : IHttpActionResult
    {
        private readonly Exception m_exception;

        public LocalExceptionResult(Exception exception)
        {
            m_exception = exception;
        }

        Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
        {
            var log = new LogEntry(m_exception);
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new JsonContent(log)
            };
            

            return Task.FromResult(response);
        }
    }

    public class LoggerExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var ex = context.Exception;

            if (this.HandleSecurityException(ex))
            {
                // set 403
            }
            /* other exceptions */
            LogOtherException(ex, context);

            context.Result = new LocalExceptionResult(ex);
            return Task.FromResult(0);
        }


        private void LogOtherException(Exception e, ExceptionHandlerContext context)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.Log(new LogEntry(e, new[] { "url", context.Request.RequestUri.ToString() }));


            //if (context.Re.Response.StatusCode == 404) return;
            if (e.Message.Contains("The file") && e.Message.Contains("does not exist")) return;
            // for dev. we stay putd
            //if (this.Application.Request.IsLocal) return;

            //this.Application.Response.Redirect(errorPage);
            //this.Application.Response.End();
            //this.Application.Server.ClearError();
        }

        private bool HandleSecurityException(Exception ex)
        {

            var exs = ex as SecurityException;

            if (null != exs)
            {
                return true;
            }
            if (null != ex.InnerException)
                return this.HandleSecurityException(ex.InnerException);

            return false;

        }

    }
}
