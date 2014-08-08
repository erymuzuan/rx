using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json.Schema;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class TransformDefinitionController : Controller
    {
        [ImportMany("FunctoidDesigner", typeof(Functoid), AllowRecomposition = true)]
        public Lazy<Functoid, IDesignerMetadata>[] Functoids { get; set; }


        public async Task<ActionResult> Validate([RequestBody] TransformDefinition map)
        {
            var erros = await map.ValidateBuildAsync();
            if (!erros.Result)
                return Json(erros);

            return Json(new { success = true, status = "OK", message = "Your map has been successfully validated" });

        }
        public async Task<ActionResult> ValidateFix([RequestBody] TransformDefinition map)
        {
            //map.MapCollection.OfType<FunctoidMap>().Where(x => x.).ForEach(x => x.Functoid.RemoveInvalidArgument());
            if (map.TransformDefinitionId <= 0) 
                return await Validate(map);
            
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(map);
                await session.SubmitChanges("Save");
            }
            return await Validate(map);
        }

        public async Task<ActionResult> Publish([RequestBody] TransformDefinition map)
        {
            var erros = await map.ValidateBuildAsync();
            if (!erros.Result)
                return Json(erros);


            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.WorkflowSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));

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
            return Json(new { success = true, status = "OK", message = "Your map has been successfully published", id = map.TransformDefinitionId });

        }

        public ActionResult GetFunctoids()
        {
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
        public async Task<ActionResult> Index()
        {
            var ed = this.GetRequestJson<TransformDefinition>();
            var context = new SphDataContext();


            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", message = "Your mapping has been successfully saved ", id = ed.TransformDefinitionId });


        }

        readonly string[] m_ignores =
        {
            "Microsoft","Spring","WebGrease","WebActivator","WebMatrix",
            "workflows","Antlr3.Runtime","core.sph", "web.sph", "mscorlib",
            "DiffPlex","Common.Logging","EntityFramework","App_global", "Mono.Math",
            "Humanizer","ImageResizer","windows",
            "Invoke","Monads",
            "NCrontab","Newtonsoft",
            "RazorGenerator","RazorEngine",
            "Antlr3","RazorEngine",
            "DotNetOpenAuth","System","Owin","RabbitMQ.Client","Roslyn"
        };
        public ActionResult Assemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assemblies
                                let name = a.GetName()
                                where a.IsDynamic == false
                                && !m_ignores.Any(x => name.Name.StartsWith(x))
                                select new ReferencedAssembly
                                {
                                    Version = name.Version.ToString(),
                                    FullName = name.FullName,
                                    Location = a.Location,
                                    Name = name.Name
                                };

            return Json(refAssemblies.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Schema(string type)
        {
            var t = Type.GetType(type);
            var gen = new JsonSchemaGenerator();
            var schema = gen.Generate(t);

            return Content(schema.ToString(), "application/json", Encoding.UTF8);
        }

        public ActionResult GetTypes(string dll)
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assemblies
                                let name = a.GetName()
                                where a.IsDynamic == false
                                && !m_ignores.Any(x => name.Name.StartsWith(x))
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
                x.Name
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Functoid(string id, string type)
        {
            if(null == this.Functoids)
                ObjectBuilder.ComposeMefCatalog(this);

            var functoid = this.Functoids.Single(x => x.Value.GetType().GetShortAssemblyQualifiedName()
                .ToLowerInvariant()== type).Value;
            if (id == "js")
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