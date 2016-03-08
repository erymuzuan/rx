using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Web.App_Start
{
    public class RouteConfig
    {
        public static AdapterDesigner AdapterDesigner { get; set; }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("glimpse.axd");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces:new []{ "Bespoke.Sph.Web.Controllers" }
            );

        }

        public static Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var user = ad.CurrentUserName;
            var routes = new List<JsRoute>();


            var context = new SphDataContext();
            var reportDefinitions = context.LoadFromSources<ReportDefinition>(rdl => rdl.IsActive || (rdl.IsPrivate && rdl.CreatedBy == user));
            var views = context.LoadFromSources<EntityView>(x => x.IsPublished);
            var forms = context.LoadFromSources<EntityForm>(x => x.IsPublished) ;
            var wfForms = context.LoadFromSources<WorkflowForm>(x => x.IsPublished) ;

            var wfFormsRoutes = from t in wfForms
                             select new JsRoute
                             {
                                 Title = t.Name,
                                 Route = $"{t.Route.ToLowerInvariant()}/:id",
                                 Caption = t.Name,
                                 Icon = t.IconClass,
                                 ModuleId = $"viewmodels/{t.Route.ToLowerInvariant()}",
                                 Nav = false
                             };
            var formRoutes = from t in forms
                             select new JsRoute
                             {
                                 Title = t.Name,
                                 Route = $"{t.Route.ToLowerInvariant()}/:id",
                                 Caption = t.Name,
                                 Icon = t.IconClass,
                                 ModuleId = $"viewmodels/{t.Route.ToLowerInvariant()}",
                                 Nav = false
                             };
            var viewRoutes = from t in views
                             select new JsRoute
                             {
                                 Title = t.Name,
                                 Route = t.GenerateRoute(),
                                 Caption = t.Name,
                                 Icon = t.IconClass,
                                 ModuleId = $"viewmodels/{t.Route.ToLowerInvariant()}",
                                 Nav = false
                             };


            var rdlRoutes = from t in reportDefinitions
                            select new JsRoute
                            {
                                Title = t.Title,
                                Route = $"reportdefinition.execute-id.{t.Id}/:id",
                                Caption = t.Title,
                                Icon = "icon-bar-chart",
                                ModuleId = $"viewmodels/reportdefinition.execute-id.{t.Id}"
                            };

            // adapters
            if (null == AdapterDesigner)
            {
                AdapterDesigner = new AdapterDesigner();
            }
            var adapterRoutes = AdapterDesigner.GetRoutes();
            routes.AddRange(adapterRoutes);

            routes.AddRange(viewRoutes);
            routes.AddRange(formRoutes);
            routes.AddRange(wfFormsRoutes);
            routes.AddRange(rdlRoutes);
            return Task.FromResult(routes.AsEnumerable());
        }
    }
}