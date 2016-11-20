using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Mono.Cecil;
using Mono.Cecil.Rocks;

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
            "NLog", "Topshelf","Thinktecture",
            "Invoke","Monads","Mono","JWT",
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
            "restapi.adapter",
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

        private readonly Func<TypeDefinition, bool> m_typePredicate = x => x.IsClass && x.IsPublic
                                                         && !x.IsInterface
                                                         && !x.IsAbstract
                                                         && !x.Name.EndsWith("Controller");


        private class AssemblyDefinitionComparer :IEqualityComparer<AssemblyDefinition>
        {
            public bool Equals(AssemblyDefinition x, AssemblyDefinition y)
            {
                return x.Name.Name.Equals(y.Name.Name, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(AssemblyDefinition obj)
            {
                return obj.Name.Name.GetHashCode();
            }
        }
        [HttpGet]
        [Route("")]
        public IHttpActionResult Assemblies()
        {
            const string KEY = "assemblies-list";
            var json = this.CacheManager.Get<string>(KEY);
            if (null != json) return Json(json);

            var assemblies = LoadAssemblyDefinitions().Distinct(new AssemblyDefinitionComparer()).OrderBy(x => x.Name.Name);
            var refAssemblies = (from a in assemblies
                                 let module = a.MainModule
                                 select new
                                 {
                                     Version = a.Name.Version.ToString(),
                                     a.FullName,
                                     a.Name.Name,
                                     Types = module.Types
                                         .Where(m_typePredicate)
                                         .Select(x => new
                                         {
                                             x.Namespace,
                                             x.Name
                                         }).ToArray()
                                 }).ToArray();

            var result = refAssemblies.OrderBy(x => x.Name).ToArray();
            json = result.ToJson();
            this.CacheManager.Insert(KEY, json, TimeSpan.FromSeconds(300));
            return Json(json);
        }

        [HttpGet]
        [Route("{type}/object-schema")]
        public IHttpActionResult GetObjectSchema(string type)
        {
            var t = Strings.GetTypeDefinition(type);
            if (null == t)
            {
                string message = $"Cannot find {type} in your {ConfigurationManager.WebPath}/bin or {ConfigurationManager.CompilerOutputPath}, Please build it if you have not done so";

                ObjectBuilder.GetObject<ILogger>()
                    .Log(new LogEntry(new Exception(message)));
                return NotFound(message);

            }

            var schema = t.GetJsonSchema();
            return Json(schema);
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
                          let sc = t.GetJsonSchema()
                          select $@"""{t.Name}"" : {sc}";
            sb.AppendLine(string.Join(", ", schemes));
            sb.AppendLine("     }");
            sb.AppendLine("}");
            return Json(sb.ToString());
        }

        [HttpGet]
        [Route("{dll}/types")]
        public IHttpActionResult GetTypes(string dll,
            [FromUri(Name = "includeAdapter")]bool includeAdapter = false,
            [FromUri(Name = "includeInternal")]bool includeInternal = false,
            [FromUri(Name = "includeGeneric")]bool includeGeneric = false)
        {
            var assemblies = LoadAssemblyDefinitions();
            var assembly = assemblies.FirstOrDefault(x => x.Name.Name == dll);
            if (null == assembly) return NotFound("Cannot find assembly " + dll);

            var types = assembly.MainModule.Types.AsQueryable()
                .WhereIf(x => !x.Name.EndsWith("Adapter"), !includeAdapter)
                .WhereIf(x => x.IsPublic, !includeInternal)
                .WhereIf(x => !x.HasGenericParameters, !includeGeneric)
                .Where(m_typePredicate)
                .OrderBy(x => x.FullName);

            return Json(types.Select(x => new
            {
                FullName = x.FullName + ", " + dll,
                TypeName = x.FullName,
                x.Name
            }).ToArray());
        }

        private static IEnumerable<AssemblyDefinition> LoadAssemblyDefinitions()
        {
            var files = Directory.GetFiles($"{ConfigurationManager.CompilerOutputPath}", "*.dll")
                .Concat(Directory.GetFiles($"{ConfigurationManager.WebPath}\\bin", "*.dll"))
                .Concat(Directory.GetFiles($"{ConfigurationManager.Home}\\packages", "*.dll", SearchOption.AllDirectories))
                .Where(x => !Ignores.Any(f => (Path.GetFileName(x) ?? f).StartsWith(f)));

            var assemblies = files.Select(AssemblyDefinition.ReadAssembly);
            return assemblies;
        }

        [HttpGet]
        [Route("types/exceptions")]
        public IHttpActionResult GetExceptions()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assemblies
                                let name = a.GetName()
                                where a.IsDynamic == false
                                select a;
            var types = new List<string>();
            foreach (var a in refAssemblies)
            {
                try
                {
                    var list = a.GetTypes()
                                .Where(x => typeof(Exception).IsAssignableFrom(x))
                                .Where(x => x.Name.EndsWith("Exception"))
                                .Where(x => !x.Name.Contains("+"))
                                .Where(x => !x.FullName.Contains("<"))
                                .Where(x => !x.FullName.Contains(">"))
                                .Select(x => x.FullName);
                    types.AddRange(list);

                }
                catch (Exception)
                {
                    //ignore
                }
            }
            return Json(types.OrderBy(x => x).ToArray());
        }


        private static TypeDefinition FindType(string dll, string type)
        {
            var assemblies = LoadAssemblyDefinitions();
            var assembly = assemblies.SingleOrDefault(x => x.Name.Name == dll);
            return assembly?.MainModule.Types.SingleOrDefault(x => x.Name == type || x.FullName == type);
        }


        [HttpGet]
        [Route("{dll}/types/{type}/methods")]
        public IHttpActionResult GetMethods(string dll, string type)
        {
            var clrType = FindType(dll, type);
            if (null == clrType)
                return NotFound($"Cannot load type '{type}' in {dll}");
            var methods = clrType.GetMethods()
                            .Where(x => x.IsPublic)
                            .ToList()
                            .Where(x => !string.IsNullOrWhiteSpace(x.Name))
                            .ToList()
                            .Where(x => !x.Name.StartsWith("get_"))
                            .Where(x => !x.Name.StartsWith("set_"))
                            .Where(x => !x.Name.StartsWith("add_"))
                            .Where(x => !x.Name.StartsWith("remove_"))
                            .Where(x => x.DeclaringType.FullName != typeof(object).FullName)
                            .Where(x => x.DeclaringType.FullName != typeof(DomainObject).FullName);


            var list = from m in methods
                       select new
                       {
                           m.Name,
                           RetVal = m.ReturnType.FullName,
                           IsVoid = m.ReturnType.FullName == "System.Void",
                           IsGenericMethod = m.HasGenericParameters,
                           Parameters = m.Parameters.Select(p => new
                           {
                               p.Name,
                               Type = $"{p.ParameterType}",
                               TypeName = p.ParameterType.FullName + ", " + p.ParameterType.Scope.Name,
                               p.IsOut,
                               p.IsReturnValue,
                               p.IsIn
                           })
                       };

            return Json(list.ToArray());
        }
    }
}