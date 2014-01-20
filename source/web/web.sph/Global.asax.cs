using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.WorkflowHelpers;

namespace web.sph
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WorkflowConfig.Register(Server);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes).Wait();
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            HostingEnvironment.RegisterVirtualPathProvider(new WorkflowScreenActivityPathProvider());


            ModelBinders.Binders.Add(typeof(IEnumerable<Rule>), new RuleModelBinder());
    
        }
    }
}
