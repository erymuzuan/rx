using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Web.App_Start;

namespace Bespoke.Sph.WebSph
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ApplicationHelper m_webApplicationHelper;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AuthConfig.RegisterAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes).Wait();
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

            WebApplicationHelper.Application_Error();

        }



    }

}
