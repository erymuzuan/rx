using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.App_Start
{
    public class RouteConfig
    {
        public async static Task RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("glimpse.axd");


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            var context = new SphDataContext();

            var query = context.EntityForms.Where(e => e.IsPublished == true);
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var forms = new ObjectCollection<EntityForm>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                forms.AddRange(lo.ItemCollection);
            }
            foreach (var form in forms)
            {
                routes.MapRoute(
                    name: form.Route,
                    url: string.Format("App/viewmodels/{0}.js", form.Route),
                    defaults: new { controller = "EntityForm", action = "Form", Area = "Sph", id = form.Route }
                );

            }
        }

        public static void RegisterCustomEntityDependencies(IEnumerable<EntityDefinition> entityDefinitions)
        {
            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            foreach (var ed in entityDefinitions)
            {
                var ed1 = ed;
                try
                {
                    var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed1.Name);
                    var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed1.EntityDefinitionId, ed1.Name);
                    var edType = edAssembly.GetType(edTypeName);
                    if (null == edType)
                        Console.WriteLine("Cannot create type " + edTypeName);

                    var reposType = sqlRepositoryType.MakeGenericType(edType);
                    var repository = Activator.CreateInstance(reposType);

                    var ff = typeof(IRepository<>).MakeGenericType(new[] { edType });

                    ObjectBuilder.AddCacheList(ff, repository);
                }
                catch (FileNotFoundException e)
                {
                  //  Console.WriteLine(e);
                }
            }

        }

        public static async Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var user = ad.CurrentUserName;

            var context = new SphDataContext();
            // ReSharper disable RedundantBoolCompare
            var rdlTask = context.LoadAsync(context.ReportDefinitions.Where(t => t.IsActive == true || (t.IsPrivate && t.CreatedBy == user)), includeTotalRows: true);
            var edTasks = context.LoadAsync(context.EntityDefinitions.Where(e => e.IsPublished == true), includeTotalRows: true);
            var formTask = context.LoadAsync(context.EntityForms.Where(e => e.IsPublished == true), includeTotalRows: true);
            var viewTask = context.LoadAsync(context.EntityViews.Where(e => e.IsPublished == true), includeTotalRows: true);
            // ReSharper restore RedundantBoolCompare
            await Task.WhenAll(rdlTask, edTasks);


            var reportDefinitionLoadOperation = await rdlTask;
            var entityDefinitionLoadOperation = await edTasks;
            var viewsLoadOperation = await viewTask;
            var formLoadOperation = await formTask;
            var routes = new List<JsRoute>();

            var reportDefinitions = new ObjectCollection<ReportDefinition>(reportDefinitionLoadOperation.ItemCollection);
            var entityDefinitions = new ObjectCollection<EntityDefinition>(entityDefinitionLoadOperation.ItemCollection);
            var views = new ObjectCollection<EntityView>(viewsLoadOperation.ItemCollection);
            var forms = new ObjectCollection<EntityForm>(formLoadOperation.ItemCollection);


            while (entityDefinitionLoadOperation.HasNextPage)
            {
                entityDefinitionLoadOperation = await context.LoadAsync(
                        context.EntityDefinitions, entityDefinitionLoadOperation.CurrentPage + 1, includeTotalRows: true);
                entityDefinitions.AddRange(entityDefinitionLoadOperation.ItemCollection);
            }

            while (formLoadOperation.HasNextPage)
            {
                formLoadOperation = await context.LoadAsync(
                        context.EntityForms, formLoadOperation.CurrentPage + 1, includeTotalRows: true);
                forms.AddRange(formLoadOperation.ItemCollection);
            }
            while (viewsLoadOperation.HasNextPage)
            {
                viewsLoadOperation = await context.LoadAsync(
                        context.EntityViews, viewsLoadOperation.CurrentPage + 1, includeTotalRows: true);
                views.AddRange(viewsLoadOperation.ItemCollection);
            }

            while (reportDefinitionLoadOperation.HasNextPage)
            {
                reportDefinitionLoadOperation = await context.LoadAsync(
                        context.ReportDefinitions, reportDefinitionLoadOperation.CurrentPage + 1, includeTotalRows: true);
                reportDefinitions.AddRange(reportDefinitionLoadOperation.ItemCollection);
            }

            RegisterCustomEntityDependencies(entityDefinitions);

            var formRoutes = from t in forms
                             select new JsRoute
                             {
                                 Title = t.Name,
                                 Route = string.Format("{0}/:id", t.Route.ToLowerInvariant()),
                                 Caption = t.Name,
                                 Icon = t.IconClass,
                                 ModuleId = string.Format("viewmodels/{0}", t.Route.ToLowerInvariant()),
                                 Nav = false
                             };
            var viewRoutes = from t in views
                             select new JsRoute
                             {
                                 Title = t.Name,
                                 Route = string.Format("{0}", t.Route.ToLowerInvariant()),
                                 Caption = t.Name,
                                 Icon = t.IconClass,
                                 ModuleId = string.Format("viewmodels/{0}", t.Route.ToLowerInvariant()),
                                 Nav = false
                             };
            var edRoutes = from t in entityDefinitions
                           select new JsRoute
                           {
                               Title = t.Plural,
                               Route = string.Format("{0}", t.Name.ToLowerInvariant()),
                               Caption = t.Plural,
                               Icon = t.IconClass,
                               ModuleId = string.Format("viewmodels/{0}", t.Name.ToLowerInvariant()),
                               Nav = true
                           };

            var rdlRoutes = from t in reportDefinitions
                            select new JsRoute
                            {
                                Title = t.Title,
                                Route = string.Format("reportdefinition.execute-id.{0}/:id", t.ReportDefinitionId),
                                Caption = t.Title,
                                Icon = "icon-bar-chart",
                                ModuleId = string.Format("viewmodels/reportdefinition.execute-id.{0}", t.ReportDefinitionId)
                            };

            routes.AddRange(viewRoutes);
            routes.AddRange(formRoutes);
            routes.AddRange(edRoutes);
            routes.AddRange(rdlRoutes);
            return routes;
        }
    }
}