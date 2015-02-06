using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize]
    public class WorkflowDefinitionController : BaseController
    {

        public ActionResult Visual()
        {
            var vm = new WorkflowDefinitionVisualViewModel();
            vm.ToolboxElements.Add(new ScreenActivity());
            return View(vm);
        }

        public async Task<ActionResult> Import(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                foreach (var postedFile in files)
                {
                    var fileName = Path.GetFileName(postedFile.FileName);
                    if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("Filename is empty or null");

                    var zip = Path.Combine(Path.GetTempPath(), fileName);
                    postedFile.SaveAs(zip);

                    var packager = new WorkflowDefinitionPackage();
                    var wd = await packager.UnpackAsync(zip);

                    this.Response.ContentType = APPLICATION_JAVASCRIPT;
                    var result = new { success = true, wd };
                    return Content(result.ToJsonString());

                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
            return Json(new { success = false });


        }


        public async Task<ActionResult> Export()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var package = new WorkflowDefinitionPackage();
            var zd = await package.PackAsync(wd);
            return Json(new { success = true, status = "OK", url = this.Url.Action("Get", "BinaryStore", new { id = zd.Id }) });
        }




        public async Task<ActionResult> GetXsdElementName(string id)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(id);
            using (var stream = new MemoryStream(content.Content))
            {
                var xsd = XElement.Load(stream);

                XNamespace x = "http://www.w3.org/2001/XMLSchema";
                var elements = xsd.Elements(x + "element").Select(e => e.Attribute("name").Value).ToList();
                return Json(elements, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public async Task<ActionResult> Compile()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var buildValidation = wd.ValidateBuild();

            await this.Save("Compile", wd);

            if (!buildValidation.Result)
                return Json(buildValidation);


            var options = new CompilerOptions
            {
                SourceCodeDirectory = Path.Combine(ConfigurationManager.UserSourceDirectory, wd.Id)
            };
            options.AddReference(typeof(Controller));
            options.AddReference(typeof(WorkflowDefinitionController));
            options.AddReference(typeof(JsonConvert));



            var result = wd.Compile(options);

            if (!result.Result)
            {
                return Json(new { success = false, version = wd.Version, status = "ERROR", result.Errors });
            }

            return Json(new { success = true, status = "OK", message = "Your workflow has been successfully compiled  : " + Path.GetFileName(result.Output) });

        }

        public async Task<ActionResult> Publish()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var buildValidation = wd.ValidateBuild();

            if (!buildValidation.Result)
                return Json(buildValidation);


            // compile , then save
            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.SphSourceDirectory
            };
            options.AddReference(typeof(Controller));
            options.AddReference(typeof(WorkflowDefinitionController));
            options.AddReference(typeof(JsonConvert));

            var result = wd.Compile(options);
            if (!result.Result || !System.IO.File.Exists(result.Output))
            {
                return Json(new { success = false, version = wd.Version, status = "ERROR", result.Errors });
            }

            // save
            //archive the WD
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var archived = new BinaryStore
            {
                Id = string.Format("wd.{0}.{1}", wd.Id, wd.Version),
                Content = Encoding.Unicode.GetBytes(wd.ToJsonString(true)),
                Extension = ".xml",
                FileName = string.Format("wd.{0}.{1}.xml", wd.Id, wd.Version)

            };
            await store.DeleteAsync(archived.Id);
            await store.AddAsync(archived);
            await this.Save("Publish", wd);

            return Json(new { success = true, version = wd.Version, status = "OK", message = "Your workflow has been successfully compiled and published : " + Path.GetFileName(result.Output) });
        }

        public async Task<ActionResult> Save()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            if (string.IsNullOrWhiteSpace(wd.Name))
                return Json(new { success = false, status = "Not OK", message = "Name cannot be empty" });
            if (wd.IsNewItem && string.IsNullOrWhiteSpace(wd.SchemaStoreId))
            {
                wd.Id = wd.Name.ToIdFormat();
                // get the empty schema
                var store = ObjectBuilder.GetObject<IBinaryStore>();
                var xsd = new BinaryStore
                {
                    Extension = ".xsd",
                    FileName = "Empty.xsd",
                    WebId = Guid.NewGuid().ToString(),
                    Id = Guid.NewGuid().ToString(),
                    Content = System.IO.File.ReadAllBytes(Server.MapPath(@"~/App_Data/empty.xsd"))
                };
                await store.AddAsync(xsd);
                wd.SchemaStoreId = xsd.Id;

            }
            var id = await this.Save(string.IsNullOrWhiteSpace(wd.Id) ? "Add" : "Update", wd);
            return Json(new { success = !string.IsNullOrWhiteSpace(wd.Id), id, status = "OK" });
        }

        public async Task<ActionResult> Remove()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(wd);
                await session.SubmitChanges();
            }

            return Json(new { success = true, id = wd.Id, status = "OK" });
        }

        public async Task<ActionResult> GetVariablePath(string id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == id);
            var list = wd.VariableDefinitionCollection.Select(v => v.Name).ToList();
            var schema = wd.GetCustomSchema();
            if (null != schema)
            {
                var xsd = new XsdMetadata(schema);
                foreach (var v in wd.VariableDefinitionCollection.OfType<ComplexVariable>())
                {
                    list.AddRange(xsd.GetMembersPath(v.TypeName).Select(x => v.Name + "." + x));
                }
            }

            foreach (var v in wd.VariableDefinitionCollection.OfType<ClrTypeVariable>())
            {
                var v1 = v;
                var entity = await context.LoadOneAsync<EntityDefinition>(e => e.Name == v1.Type.Name);
                if (null != entity)
                {
                    list.AddRange(entity.GetMembersPath().Select(x => v.Name + "." + x));
                }
                else
                {
                    var properties = v.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    list.AddRange(properties.Select(x => v.Name + "." + x.Name));
                }
            }

            return Json(list.Select(d => new { Path = d }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        private async Task<string> Save(string operation, WorkflowDefinition wd, params Entity[] entities)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException("wd");


            using (var session = context.OpenSession())
            {
                if (entities.Any())
                    session.Attach(entities);

                session.Attach(wd);
                await session.SubmitChanges(operation);
            }
            return wd.Id;
        }

        public ActionResult GetLoadedAssemblies()
        {
            var assesmblies = AppDomain.CurrentDomain.GetAssemblies();
            var refAssemblies = from a in assesmblies
                                where a.IsDynamic == false
                                let name = a.GetName()
                                select new ReferencedAssembly
                                {
                                    Version = name.Version.ToString(),
                                    FullName = name.FullName,
                                    Location = a.Location,
                                    Name = name.Name
                                };

            return Json(refAssemblies.ToArray(), JsonRequestBehavior.AllowGet);
        }




    }
}