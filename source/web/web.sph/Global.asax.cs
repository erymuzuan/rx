using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Helpers;

namespace web.sph
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ApplicationHelper m_webApplicationHelper;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes).Wait();
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ModelBinders.Binders.Add(typeof(IEnumerable<Rule>), new RuleModelBinder());

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
