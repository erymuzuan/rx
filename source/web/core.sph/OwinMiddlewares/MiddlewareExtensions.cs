using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Hubs;
using Owin;

namespace Bespoke.Sph.Web.OwinMiddlewares
{
    public static class MiddlewareExtensions
    {
        public static IAppBuilder MapSignalRConnection(this IAppBuilder app)
        {
            app.MapSignalR();
            app.MapSignalR<MessageConnection>("/signalr_message");
            app.MapSignalR<SolutionConnection>("/signalr_solution");
            app.MapSignalR<AuditTrailConnection>("/signalr_audittrail");

            return app;


        }
        public static IAppBuilder UseApiMetering(this IAppBuilder app)
        {
            app.Use<MeteringMiddleware>();
            return app;

        }
        public static IAppBuilder UseCoreResource(this IAppBuilder app, bool debug)
        {
            app.Map("/SphApp", cfg =>
            {
                cfg.Use<ResourceMiddleware>("SphApp", debug);
            });
            app.Map("/Content", cfg =>
            {
                cfg.Use<ResourceMiddleware>("Content", debug);
            });
            app.Map("/scripts", cfg =>
            {
                cfg.Use<ResourceMiddleware>("Scripts", debug);
            });
            app.Map("/images", cfg =>
            {
                cfg.Use<ResourceMiddleware>("Images", debug);
            });
            app.Map("/kendo", cfg =>
            {
                cfg.Use<ResourceMiddleware>("kendo", debug);
            });

            app.Map("/fonts", cfg =>
            {
                cfg.Use<ResourceMiddleware>("fonts", debug);
            });


            return app;
        }


        public static IAppBuilder RegisterCustomEntityDependencies(this IAppBuilder app)
        {
            var context = new SphDataContext();
            var list = context.LoadFromSources<EntityDefinition>(x => x.IsPublished).ToList();
            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var esAssembly = Assembly.Load("elasticsearch.repository");
            var esRepositoryType = esAssembly.GetType("Bespoke.Sph.ElasticsearchRepository.ReadonlyRepository`1");

            foreach (var ed in list)
            {
                var ed1 = ed;
                try
                {
                    var edAssembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.{ed1.Name}");
                    var edTypeName = $"{ed1.CodeNamespace}.{ed1.Name}";
                    var edType = edAssembly.GetType(edTypeName);
                    if (null == edType)
                        Console.WriteLine("Cannot create type " + edTypeName);

                    var reposType = sqlRepositoryType.MakeGenericType(edType);
                    var repository = Activator.CreateInstance(reposType);
                    var ff = typeof(IRepository<>).MakeGenericType(edType);
                    ObjectBuilder.AddCacheList(ff, repository);


                    var esReposType = esRepositoryType.MakeGenericType(edType);
                    var readonlyRepository = Activator.CreateInstance(esReposType);
                    var rf = typeof(IReadonlyRepository<>).MakeGenericType(edType);
                    ObjectBuilder.AddCacheList(rf, readonlyRepository);
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

            return app;

        }
    }
}