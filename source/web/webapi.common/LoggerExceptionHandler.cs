using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public class LoggerExceptionHandler : IExceptionHandler
    {
        public bool IsDebuggingEnabled { get; set; }
        public bool IsDisableLoggingWhileDebugging { get; set; }
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var ex = context.Exception;
            if (this.HandleSecurityException(ex))
            {
                // set 403
            }
            /* other exceptions */
            var id = LogOtherException(ex, context);
            if (this.IsDebuggingEnabled)
                context.Result = new LocalExceptionResult(ex);
            else
                context.Result = new LocalExceptionResult(id);

            return Task.FromResult(0);
        }


        private string LogOtherException(Exception e, ExceptionHandlerContext context)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var id = Guid.NewGuid().ToString();
            logger.Log(new LogEntry(e, new[] { "url", context.Request.RequestUri.ToString() }) { Id = id });
            return id;
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
