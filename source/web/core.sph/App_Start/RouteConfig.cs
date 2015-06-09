﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

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
                    var edTypeName = $"Bespoke.{ConfigurationManager.ApplicationName}_{ed1.Id}.Domain.{ed1.Name}";
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
                    Debug.WriteLine(e);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

        }

        public static async Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var user = ad.CurrentUserName;

            var context = new SphDataContext();
            // ReSharper disable RedundantBoolCompare
            var formQuery = context.EntityForms.Where(e => e.IsPublished == true);
            var viewQuery = context.EntityViews.Where(e => e.IsPublished == true);
            var edQuery = context.EntityDefinitions.Where(e => e.IsPublished == true);
            var rdlQuery = context.ReportDefinitions.Where(t => t.IsActive == true || (t.IsPrivate && t.CreatedBy == user));

            var rdlTask = context.LoadAsync(rdlQuery, includeTotalRows: true);
            var edTasks = context.LoadAsync(edQuery, includeTotalRows: true);
            var formTask = context.LoadAsync(formQuery, includeTotalRows: true);
            var viewTask = context.LoadAsync(viewQuery, includeTotalRows: true);
            // ReSharper restore RedundantBoolCompare
            await Task.WhenAll(rdlTask, edTasks, formTask, viewTask);


            var rdlLo = await rdlTask;
            var edLo = await edTasks;
            var viewLo = await viewTask;
            var formsLo = await formTask;
            var routes = new List<JsRoute>();

            var reportDefinitions = new ObjectCollection<ReportDefinition>(rdlLo.ItemCollection);
            var entityDefinitions = new ObjectCollection<EntityDefinition>(edLo.ItemCollection);
            var views = new ObjectCollection<EntityView>(viewLo.ItemCollection);
            var forms = new ObjectCollection<EntityForm>(formsLo.ItemCollection);


            while (edLo.HasNextPage)
            {
                edLo = await context.LoadAsync(edQuery, edLo.CurrentPage + 1, includeTotalRows: true);
                entityDefinitions.AddRange(edLo.ItemCollection);
            }

            while (formsLo.HasNextPage)
            {
                formsLo = await context.LoadAsync(formQuery, formsLo.CurrentPage + 1, includeTotalRows: true);
                forms.AddRange(formsLo.ItemCollection);
            }
            while (viewLo.HasNextPage)
            {
                viewLo = await context.LoadAsync(viewQuery, viewLo.CurrentPage + 1, includeTotalRows: true);
                views.AddRange(viewLo.ItemCollection);
            }

            while (rdlLo.HasNextPage)
            {
                rdlLo = await context.LoadAsync(rdlQuery, rdlLo.CurrentPage + 1, includeTotalRows: true);
                reportDefinitions.AddRange(rdlLo.ItemCollection);
            }

            RegisterCustomEntityDependencies(entityDefinitions);

            // get valid users for ed
            var edDashboardUserTasks = from ed in entityDefinitions
                                       let p = ed.Performer
                                       where p.IsPublic
                                       || (!string.IsNullOrWhiteSpace(p.UserProperty)
                                       && !string.IsNullOrWhiteSpace(p.Value))
                                       select ed.Performer.GetUsersAsync(ed);
            var edDashboardUsers = await Task.WhenAll(edDashboardUserTasks);

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

            var edRoutes = entityDefinitions
                .Where(t => t.Performer.Validate())
                .Select((t, i) => new
                {
                    Index = i,
                    Entity = t,
                    Permission = t.Performer,
                    Users = edDashboardUsers[i]
                })
                .Where(t => t.Permission.IsPublic || t.Users.Contains(user))
                .Select(t => t.Entity)
                .Select(t => new JsRoute
                           {
                               Title = t.Plural,
                               Route = $"{t.Name.ToLowerInvariant()}",
                               Caption = t.Plural,
                               Icon = t.IconClass,
                               ModuleId = $"viewmodels/{t.Name.ToLowerInvariant()}",
                               Nav = t.IsShowOnNavigationBar
                           });

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
            if (null == RouteConfig.AdapterDesigner)
            {
                RouteConfig.AdapterDesigner = new AdapterDesigner();
            }
            var adapterRoutes = RouteConfig.AdapterDesigner.GetRoutes();
            routes.AddRange(adapterRoutes);

            routes.AddRange(viewRoutes);
            routes.AddRange(formRoutes);
            routes.AddRange(edRoutes);
            routes.AddRange(rdlRoutes);
            return routes;
        }
    }
}