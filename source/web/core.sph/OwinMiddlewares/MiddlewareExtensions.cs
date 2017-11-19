using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Controllers;
using Bespoke.Sph.Web.Hubs;
using Microsoft.AspNet.SignalR;
using Owin;

namespace Bespoke.Sph.Web.OwinMiddlewares
{
    public static class MiddlewareExtensions
    {
        public static IAppBuilder MapSignalRConnection(this IAppBuilder app)
        {
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
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
            app.Map("/sphapp/main.js", cfg => cfg.Use<RequireJsBundleMiddleware>(debug));

            app.Map("/SphApp", cfg =>
           {
               cfg.Use<AdapterResourceMiddleware>("SphApp", debug);
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
            var resolver = ObjectBuilder.GetObject<ICustomEntityDependenciesResolver>();
            var context = new SphDataContext();
            var list = context.LoadFromSources<EntityDefinition>(x => x.IsPublished).ToList();

            foreach (var ed in list)
            {
                try
                {
                    var repos = resolver.ResolveRepository(ed);
                    ObjectBuilder.AddCacheList(repos.DependencyType, repos.Implementation);

                    var readOnly = resolver.ResolveReadOnlyRepository(ed);
                    ObjectBuilder.AddCacheList(readOnly.DependencyType, readOnly.Implementation);
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