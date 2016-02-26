using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("api/assemblies")]
    public class ClrAssembliesController : BaseApiController
    {
        public ClrAssembliesController()
        {
            this.CacheManager = ObjectBuilder.GetObject<ICacheManager>();
        }

        public ICacheManager CacheManager { get; set; }

        public static readonly string[] Ignores =
        {
            "domain.sph",
            "ff",
            "core.sph",
            "Microsoft",
            "Spring","WebGrease","WebActivator","WebMatrix",
            "workflows","Antlr3.Runtime","core.sph", "web.sph", "mscorlib",
            "DiffPlex","Common.Logging","EntityFramework","App_global", "Mono.Math",
            "Humanizer","ImageResizer","windows",
            "Invoke","Monads",
            "NCrontab","Newtonsoft",
            "RazorGenerator","RazorEngine",
            "Antlr3",
            "App_Code",
            "App_Web",
            "SQLSpatialTools",
            "email.service",
            "elasticsearch.logger",
            "MySql.Data",
            "log4net",
            "Oracle.ManagedDataAccess",
            "rabbitmq",
            "razor.template",
            "report.sqldatasource",
            "sqlmembership",
            "subscriber",
            "trigger.action",
            "word.document.generator",
            "SMDiagnostics",
            "RazorEngine",
            "http.adapter",
            "mysql.adapter",
            "oracle.adapter",
            "sqlserver.adapter",
            "sql.repository",
            "SuperSocket",
            "memory.broker",
            "web.console.logger",
            "roslyn.scriptengine",
            "Glimpse",
            "webapi",
            "elasticsearch",
            "FileHelpers",
            "Org.Mentalis.Security.Cryptography",
            "DotNetOpenAuth","System","Owin","RabbitMQ.Client","Roslyn",
            "LinqToQuerystring", "Mindscape","Polly","Raygun","raygun","flatfile",
            $"{ConfigurationManager.ApplicationName}.ServiceContract.",
            $"{ConfigurationManager.ApplicationName}.QueryEndpoint.",
            $"{ConfigurationManager.ApplicationName}.OperationEndpoint."
        };

        private readonly Func<Type, bool> m_typePredicate = x => x.IsClass && x.IsPublic
                                                         && !x.IsInterface
                                                         && !x.IsAbstract
                                                         && !x.Name.EndsWith("Controller");



        [HttpGet]
        [Route("")]
        public IHttpActionResult Assemblies()
        {
            const string KEY = "assemblies-list";
            var json = this.CacheManager.Get<string>(KEY);
            if (null != json) return Json(json);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = (from a in assemblies
                                 let name = a.GetName()
                                 where a.IsDynamic == false
                                       && !Ignores.Any(x => name.Name.StartsWith(x))
                                 select new
                                 {
                                     Version = name.Version.ToString(),
                                     name.FullName,
                                     name.Name,
                                     Types = a.GetTypes()
                                         .Where(m_typePredicate)
                                         .Select(x => new
                                         {
                                             x.Namespace,
                                             x.Name
                                         }).ToArray()
                                 }).ToArray();

            var assemblies2 = Directory.GetFiles(ConfigurationManager.CompilerOutputPath, "*.dll")
                .Select(Path.GetFileNameWithoutExtension)
                .Where(d => !refAssemblies.Select(x => x.Name).Contains(d))
                .Where(d => !Ignores.Any(d.StartsWith))
                .Where(d => !d.StartsWith("subscriber"))
                .Where(d => !d.StartsWith("workflow"))
                .Select(d => Path.Combine(ConfigurationManager.CompilerOutputPath, d + ".dll"))
                .Select(Assembly.LoadFile);
            var refAssemblies2 = from a in assemblies2
                                 let name = a.GetName()
                                 where a.IsDynamic == false
                                       && !Ignores.Any(x => name.Name.StartsWith(x))
                                 select new
                                 {
                                     Version = name.Version.ToString(),
                                     name.FullName,
                                     name.Name,
                                     Types = a.GetTypes()
                                         .Where(m_typePredicate)
                                         .Select(x => new
                                         {
                                             x.Namespace,
                                             x.Name
                                         }).ToArray()
                                 };

            var result = refAssemblies.Concat(refAssemblies2).OrderBy(x => x.Name).ToArray();
            json = result.ToJson();
            this.CacheManager.Insert(KEY, json, TimeSpan.FromSeconds(300));
            return Json(json);
        }

        [HttpGet]
        [Route("{type}/json-schema")]
        public IHttpActionResult Schema(string type)
        {
            var t = Strings.GetType(type);
            if (null == t)
            {
                string message = $"Cannot find {type} in your {ConfigurationManager.WebPath}/bin or {ConfigurationManager.CompilerOutputPath}, Please build it if you have not done so";

                ObjectBuilder.GetObject<ILogger>()
                    .Log(new LogEntry(new Exception(message)));
                return NotFound(message);

            }

            var schema = JsonSerializerService.GetJsonSchemaFromObject(t);
            return Json(schema.ToString());
        }

        [HttpGet]
        [Route("{dll}/types/{type}/json-schema")]
        public IHttpActionResult Schema(string dll, string type)
        {
            var ct = FindType(dll, type);
            var cts = new[] { ct };

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("     \"types\":[\"object\", null],");
            sb.AppendLine("     \"properties\": {");
            var schemes = from t in cts
                          let sc = JsonSerializerService.GetJsonSchemaFromObject(t)
                          select $@"""{t.Name}"" : {sc}";
            sb.AppendLine(string.Join(", ", schemes));
            sb.AppendLine("     }");
            sb.AppendLine("}");
            return Json(sb.ToString());
        }

        [HttpGet]
        [Route("{dll}/types")]
        public IHttpActionResult GetTypes(string dll)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assemblies
                                let name = a.GetName()
                                where a.IsDynamic == false
                                      && !Ignores.Any(x => name.Name.StartsWith(x))
                                select a;
            var assembly = refAssemblies.FirstOrDefault(x => x.GetName().Name == dll);
            if (null == assembly) return NotFound("Cannot find assembly " + dll);

            var types = assembly.GetTypes()
                .Where(m_typePredicate);
            return Json(types.Select(x => new
            {
                FullName = x.FullName + ", " + dll,
                TypeName = x.FullName,
                x.Name
            }).ToArray());
        }


        private static Type FindType(string dll, string type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assemblies
                                let name = a.GetName()
                                where a.IsDynamic == false
                                      && !Ignores.Any(x => name.Name.StartsWith(x))
                                select a;
            var assembly = refAssemblies.SingleOrDefault(x => x.GetName().Name == dll);
            return assembly?.GetType(type);
        }


        [HttpGet]
        [Route("{dll}/types/{type}/methods")]
        public IHttpActionResult GetMethods(string dll, string type)
        {
            var clrType = FindType(dll, type);
            var methods = clrType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)
                            .Where(x => null != x)
                            .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                            .Where(x => !x.Name.StartsWith("get_"))
                            .Where(x => !x.Name.StartsWith("set_"))
                            .Where(x => !x.Name.StartsWith("add_"))
                            .Where(x => !x.Name.StartsWith("remove_"))
                            .Where(x => x.DeclaringType != typeof(object))
                            .Where(x => x.DeclaringType != typeof(DomainObject));
            

            var list = from x in methods
                       select new
                       {
                           x.Name,
                           RetVal = $"{x.ReturnType}",
                           IsAsync = x.ReturnType?.BaseType == typeof(Task),
                           x.IsGenericMethod,
                           x.IsGenericMethodDefinition,
                           x.IsStatic,
                           IsVoid = x.ReturnType == typeof(void),
                           Parameters = x.GetParameters().Select(p => new
                           {
                               p.Name,
                               Type = $"{p.ParameterType}",//?.GetShortAssemblyQualifiedName(),
                               p.IsOut,
                               p.IsRetval,
                               p.IsIn
                           })
                       };

            return Json(list.ToArray());
        }
    }
}