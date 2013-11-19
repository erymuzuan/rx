using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.WorkflowHelpers;

namespace Bespoke.Sph.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WorkflowConfig.PreStart(Server);
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            HostingEnvironment.RegisterVirtualPathProvider(new WorkflowScreenActivityPathProvider());


            ModelBinders.Binders.Add(typeof(IEnumerable<Rule>), new RuleModelBinder());
        }


    }
}