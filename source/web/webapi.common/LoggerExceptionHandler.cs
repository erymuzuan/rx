using System;
using System.Security;
using System.Text;
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
            var entry = LogOtherException(ex, context);
            if (this.IsDebuggingEnabled)
                context.Result = new LocalExceptionResult(entry);
            else
                context.Result = new LocalExceptionResult(entry.Id);

            return Task.FromResult(0);
        }


        private LogEntry LogOtherException(Exception e, ExceptionHandlerContext context)
        {
            var id = Guid.NewGuid().ToString();
            var entry = new LogEntry(e, new[] { "url", context.Request.RequestUri.ToString() }) { Id = id };
            var details = new StringBuilder();

            var type = e.GetType();
            if (type.GetMethod("ToString").DeclaringType == type)
            {
                details.AppendLine("================ EXCEPTION TOSTRING ===================");
                details.Append(e);
            }

            details.AppendLine("================ REQUEST DETAILS ===================");
            details.AppendLine($"{context.Request.Method} {context.Request.RequestUri}");
            foreach (var header in context.Request.Headers)
            {
                details.AppendLine($"{header.Key}: {header.Value.ToString2()}");
            }
            details.AppendLine();
            try
            {
                if (null != context.Request.Content)
                    details.AppendLine(context.Request.Content.ReadAsStringAsync().Result);
            }
            catch
            {
                // ignored
            }

            entry.Details += details;

            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.Log(entry);
            return entry;
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
