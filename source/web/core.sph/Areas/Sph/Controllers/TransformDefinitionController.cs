using System;
using System.ComponentModel.Composition;
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
        public Lazy<Functoid, IFunctoidDesignerMetadata>[] Functoids { get; set; }

        public ActionResult GetFunctoids()
        {
            ObjectBuilder.ComposeMefCatalog(this);
            var list = from f in Functoids
                       select f.Metadata;
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
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
    }
}