using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebSph
{
    public class ApplicationHelper
    {
        public HttpApplication Application { get; set; }


        private async Task LogOtherException(Exception e, string errorPage = "~/error.aspx?error_id=")
        {

#if DEBUG

            var color = Console.ForegroundColor;
            try
            {
                if (HttpContext.Current.IsDebuggingEnabled)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("{0} => {1}", e.GetType().FullName, e.Message);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var log = Path.Combine(ConfigurationManager.UserSourceDirectory,
                        "logs\\" + DateTime.Now.ToString("s").Replace(":", ""));
                    while (File.Exists(log))
                    {
                        log += Guid.NewGuid().ToString().Substring(1, 2);
                    }
                    Console.WriteLine("Stack Trace is written to " + log);
                    File.WriteAllText(log + ".log", e.ToString());
                }
            }
            finally
            {
                Console.ForegroundColor = color;
            }
#endif


            var logger = ObjectBuilder.GetObject<ILogger>();
            await logger.LogAsync(e);

            if (null == this.Application) return;

            if (this.Application.Response.StatusCode == 404) return;
            if (e.Message.Contains("The file") && e.Message.Contains("does not exist")) return;
            // for dev. we stay putd
            if (this.Application.Request.IsLocal) return;

            this.Application.Response.Redirect(errorPage);
            this.Application.Response.End();
            this.Application.Server.ClearError();
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


        public void Application_Error(string errorPage = "~/error.aspx?error_id=", string unauthorizedPage = "~/unauthorized_request.aspx")
        {

            var ex = this.Application.Server.GetLastError();

            if (this.HandleSecurityException(ex))
            {
                this.Application.Response.StatusCode = 403;
                this.Application.Response.End();
                this.Application.Server.ClearError();
                return;
            }
            if (ex.Message.Contains("/error.aspx"))
                return;


            /* other exceptions */
            LogOtherException(ex, errorPage).Wait();
        }
    }
}