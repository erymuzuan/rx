using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Hubs;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

[assembly: OwinStartup(typeof(Startup))]
namespace Bespoke.Sph.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            RegisterCustomEntityDependencies();

            app.MapSignalR();
            app.MapSignalR<MessageConnection>("/signalr_message");
            app.MapSignalR<SolutionConnection>("/signalr_solution");
            app.MapSignalR<AuditTrailConnection>("/signalr_audittrail");

            var config = new HttpConfiguration();
            config.MessageHandlers.Add(new MethodOverrideHandler());

            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.None
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            setting.Formatting = Formatting.Indented;

            config.Formatters.JsonFormatter.SerializerSettings = setting;
            config.MapHttpAttributeRoutes();


            config.Services.Replace(typeof(IExceptionHandler), ObjectBuilder.GetObject<IExceptionHandler>());
            config.EnsureInitialized();

            app.UseJwt();

            app.UseWebApi(config);

        }

        public static void RegisterCustomEntityDependencies()
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

        }

    }
}