using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    //[Authorize(Roles = "developers")]
    [RoutePrefix("transform-definition")]
    public class TransformDefinitionController : Controller
    {
        [ImportMany("FunctoidDesigner", typeof(Functoid), AllowRecomposition = true)]
        public Lazy<Functoid, IDesignerMetadata>[] Functoids { get; set; }

        [HttpPost]
        [Route("validate")]
        public async Task<ActionResult> Validate([RequestBody] TransformDefinition map)
        {
            var erros = await map.ValidateBuildAsync();
            if (!erros.Result)
                return Json(erros);

            return Json(new { success = true, status = "OK", message = "Your map has been successfully validated" });

        }

        [HttpPost]
        [Route("validate-fix")]
        public async Task<ActionResult> ValidateFix([RequestBody] TransformDefinition map)
        {
            if (string.IsNullOrWhiteSpace(map.Id))
                return await Validate(map);

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(map);
                await session.SubmitChanges("Save");
            }
            return await Validate(map);
        }

        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult> Publish([RequestBody] TransformDefinition map)
        {
            var erros = await map.ValidateBuildAsync();
            if (!erros.Result)
                return Json(erros);


            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.UserSourceDirectory
            };
            options.AddReference<Controller>();
            options.AddReference<TransformDefinitionController>();
            options.AddReference<Newtonsoft.Json.JsonConverter>();

            var codes = map.GenerateCode();
            var sources = map.SaveSources(codes);
            var result = await map.CompileAsync(options, sources);

            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                return Json(result);



            map.IsPublished = true;
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(map);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your map has been successfully published", id = map.Id });

        }

        [HttpGet]
        [Route("functoids")]
        public ActionResult GetFunctoids()
        {
            if (null == this.Functoids)
                ObjectBuilder.ComposeMefCatalog(this);
            var list = from f in Functoids
                       let v = f.Value
                       let g = v.Initialize()
                       orderby f.Metadata.Category
                       select new { designer = f.Metadata, functoid = v };
            this.Response.ContentType = "application/json";
            return Content(list.ToJsonString(true));
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> SaveAsync()
        {
            var ed = this.GetRequestJson<TransformDefinition>();
            var context = new SphDataContext();

            if (ed.IsNewItem)
            {
                ed.Id = ed.Name.ToIdFormat();
            }

            ed.FunctoidCollection.RemoveAll(f => null == f);
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", message = "Your mapping has been successfully saved ", id = ed.Id });


        }

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
            "Org.Mentalis.Security.Cryptography",
            "DotNetOpenAuth","System","Owin","RabbitMQ.Client","Roslyn"
        };


        [HttpGet]
        [Route("assemblies")]
        public ActionResult Assemblies()
        {
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
                                                 .Where(x => !x.IsAbstract)
                                                 .Where(x => !x.IsInterface)
                                                 .Where(x => x.IsPublic)
                                                 .Where(x => x.IsClass)
                                                 .Select(x => new
                                                 {
                                                     x.Namespace,
                                                     x.Name
                                                 }).ToArray()
                                 }).ToArray();

            var assemblies2 = Directory.GetFiles(ConfigurationManager.WorkflowCompilerOutputPath, "*.dll")
                .Select(Path.GetFileNameWithoutExtension)
                .Where(d => !refAssemblies.Select(x => x.Name).Contains(d))
                .Where(d => !Ignores.Any(d.StartsWith))
                .Where(d => !d.StartsWith("subscriber"))
                .Where(d => !d.StartsWith("workflow"))
                .Select(d => Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, d + ".dll"))
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
                                                 .Where(x => !x.IsAbstract)
                                                 .Where(x => !x.IsInterface)
                                                 .Where(x => x.IsPublic)
                                                 .Where(x => x.IsClass)
                                                 .Select(x => new
                                                 {
                                                     x.Namespace,
                                                     x.Name
                                                 }).ToArray()
                                 };

            return Json(refAssemblies.Concat(refAssemblies2).OrderBy(x => x.Name).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("json-schema/{type}")]
        public ActionResult Schema(string type)
        {
            var t = Type.GetType(type);
            if (null == t)
            {
                var splits = type.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
                var dll = Assembly.LoadFile($"{ConfigurationManager.WorkflowCompilerOutputPath}\\{splits.Last().Trim()}.dll");
                t = dll.GetType(splits.First().Trim());
            }
            var schema = JsonSerializerService.GetJsonSchemaFromObject(t);

            return Content(schema.ToString(), "application/json", Encoding.UTF8);
        }

        [HttpPost]
        [Route("json-schema")]
        public ActionResult Schema([RequestBody]TransformDefinition map)
        {
            if (!string.IsNullOrWhiteSpace(map.InputTypeName))
                return Schema(map.InputTypeName);

            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("     \"types\":[\"object\", null],");
            sb.AppendLine("     \"properties\": {");
            var schemes = from t in map.InputCollection
                          let sc = JsonSerializerService.GetJsonSchemaFromObject(t.Type)
                          select $@"""{t.Name}"" : {sc}";
            sb.AppendLine(string.Join(", ", schemes));
            sb.AppendLine("     }");
            sb.AppendLine("}");
            return Content(sb.ToString(), "application/json", Encoding.UTF8);
        }

        [HttpGet]
        [Route("types/{dll}")]
        public ActionResult GetTypes(string dll)
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assemblies
                                let name = a.GetName()
                                where a.IsDynamic == false
                                && !Ignores.Any(x => name.Name.StartsWith(x))
                                select a;
            var assembly = refAssemblies.SingleOrDefault(x => x.GetName().Name == dll);
            if (null == assembly) return HttpNotFound("Cannot find assembly " + dll);

            var types = assembly.GetTypes()
                .Where(x => !x.IsAbstract)
                .Where(x => !x.IsInterface)
                .Where(x => x.IsPublic)
                .Where(x => x.IsClass);
            return Json(types.Select(x => new
            {
                FullName = x.FullName + ", " + dll,
                TypeName = x.FullName,
                x.Name
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("methods/{dll}/{type}")]
        public ActionResult GetMethods(string dll, string type)
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assemblies
                                let name = a.GetName()
                                where a.IsDynamic == false
                                && !Ignores.Any(x => name.Name.StartsWith(x))
                                select a;
            var assembly = refAssemblies.SingleOrDefault(x => x.GetName().Name == dll);
            if (null == assembly) return HttpNotFound("Cannot find assembly " + dll);

            var clrType = assembly.GetType(type);
            if (null == clrType)
                return new HttpNotFoundResult("Cannot find " + type + " in " + dll);
            var methods = clrType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static)
                .Where(x => !x.Name.StartsWith("get_"))
                .Where(x => !x.Name.StartsWith("set_"))
                .Where(x => !x.Name.StartsWith("add_"))
                .Where(x => !x.Name.StartsWith("remove_"))
                .Where(x => x.DeclaringType != typeof(object))
                .Where(x => x.DeclaringType != typeof(DomainObject))
                ;

            return Json(methods.Select(x => new
            {
                x.Name,
                Display = $"",
                RetVal = x.ReturnType.FullName,
                IsAsync = x.ReturnType.FullName.StartsWith("System.Threading.Tasks.Task"),
                x.IsStatic,
                IsVoid = x.ReturnType == typeof(void),
                Parameters = x.GetParameters().Select(p => new
                {
                    p.Name,
                    Type = p.ParameterType.GetShortAssemblyQualifiedName(),
                    p.IsOut,
                    p.IsRetval,
                    p.IsIn
                })
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("functoid/{extension}/{type}")]
        public ActionResult Functoid(string extension, string type)
        {
            if (null == this.Functoids)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Functoids) throw new InvalidOperationException("Cannot compose MEF");

            var functoid = this.Functoids.Single(x => x.Value.GetType().GetShortAssemblyQualifiedName()
                .ToLowerInvariant() == type).Value;
            if (extension == "js")
            {
                this.Response.ContentType = "application/javascript";
                var js = functoid.GetEditorViewModel();
                return Content(js);
            }
            this.Response.ContentType = "text/html";
            var html = functoid.GetEditorView();
            return Content(html);
        }
    }
}