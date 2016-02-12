using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Helpers;
using ConfigurationManager = Bespoke.Sph.Domain.ConfigurationManager;

namespace web.sph
{
    public class MvcApplication : HttpApplication
    {
        private ApplicationHelper m_webApplicationHelper;

        protected void Application_Start()
        {
            ConfigurationManager.AddConnectionString();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();

            ModelBinders.Binders.Add(typeof(IEnumerable<Rule>), new RuleModelBinder());
            ObjectBuilder.GetObject<ILogger>().Log(new LogEntry
            {
                Message = "Web application starts",
                Severity = Severity.Critical,
                Log = EventLog.WebServer,
                Operation = "Starts",
                Source = "Application"

            });

        }



        protected void Application_Stop()
        {
            ObjectBuilder.GetObject<ILogger>().Log(new LogEntry
            {
                Message = "Web application stopped",
                Severity = Severity.Critical,
                Log = EventLog.WebServer,
                Operation = "Stop",
                Source = "Application"

            });
        }

        public ApplicationHelper WebApplicationHelper
        {
            get
            {
                return m_webApplicationHelper ?? (m_webApplicationHelper = new ApplicationHelper { Application = this });
            }
            set { m_webApplicationHelper = value; }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Console.WriteLine("ERROR*-***/*/*/*/*");
            WebApplicationHelper.Application_Error();
            Console.WriteLine("ERROR*******************");
        }
      
    }

}
