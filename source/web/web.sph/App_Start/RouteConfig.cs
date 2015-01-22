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

namespace Bespoke.Sph.Web.App_Start
{
    public class RouteConfig
    {
        public async static Task RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("glimpse.axd");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            await RegisterFormRoutesAsync(routes).ConfigureAwait(false);
            //await RegisterEntitySearchAndApiRoutesAsync(routes).ConfigureAwait(false);
        }

        private static async Task RegisterFormRoutesAsync(RouteCollection routes)
        {
            var context = new SphDataContext();

            var query = context.EntityForms.Where(e => e.IsPublished == true);
            var lo = await context.LoadAsync(query, includeTotalRows: true).ConfigureAwait(false);
            var forms = new ObjectCollection<EntityForm>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true).ConfigureAwait(false);
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
                    var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed1.Id, ed1.Name);
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

    }
}