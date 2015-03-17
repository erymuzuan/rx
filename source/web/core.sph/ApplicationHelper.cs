using System;
using System.Security;
using System.Web;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web
{
    public class ApplicationHelper
    {
        public HttpApplication Application { get; set; }


        private void LogOtherException(Exception e, string errorPage = "~/error.aspx?error_id=")
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.Log(new LogEntry(e, new[] { "url", this.Application.Request.RawUrl }));

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
            LogOtherException(ex, errorPage);
        }
    }
}