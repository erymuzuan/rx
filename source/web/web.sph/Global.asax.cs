using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Helpers;

namespace web.sph
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes).Wait();
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();


            ModelBinders.Binders.Add(typeof(IEnumerable<Rule>), new RuleModelBinder());

        }
    }
}
