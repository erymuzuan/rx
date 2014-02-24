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

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
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
            return Json(new { success = false });


        }


        public async Task<ActionResult> Export()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var package = new WorkflowDefinitionPackage();
            var zd = await package.PackAsync(wd);
            return Json(new { success = true, status = "OK", url = this.Url.Action("Get", "BinaryStore", new { id = zd.StoreId }) });
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

            if (!buildValidation.Result)
                return Json(buildValidation);

            await this.Save("Compile", wd);

            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.WorkflowSourceDirectory
            };
            options.ReferencedAssemblies.Add(typeof(Controller).Assembly);
            options.ReferencedAssemblies.Add(typeof(WorkflowDefinitionController).Assembly);
            options.ReferencedAssemblies.Add(typeof(Newtonsoft.Json.JsonConvert).Assembly);

            var customeEntityAssembiles = Directory.GetFiles(ConfigurationManager.WorkflowCompilerOutputPath,
                ConfigurationManager.ApplicationName + ".*.dll");
            foreach (var dll in customeEntityAssembiles)
            {
                options.ReferencedAssemblies.Add(Assembly.LoadFrom(dll));
            }

            var result = wd.Compile(options);

            if (!result.Result || !System.IO.File.Exists(result.Output))
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
                SourceCodeDirectory = ConfigurationManager.WorkflowSourceDirectory
            };
            options.ReferencedAssemblies.Add(typeof(Controller).Assembly);
            options.ReferencedAssemblies.Add(typeof(WorkflowDefinitionController).Assembly);
            options.ReferencedAssemblies.Add(typeof(Newtonsoft.Json.JsonConvert).Assembly);
            var customeEntityAssembiles = Directory.GetFiles(ConfigurationManager.WorkflowCompilerOutputPath,
              ConfigurationManager.ApplicationName + ".*.dll");
            foreach (var dll in customeEntityAssembiles)
            {
                options.ReferencedAssemblies.Add(Assembly.LoadFrom(dll));
            }

            var result = wd.Compile(options);
            if (!result.Result || !System.IO.File.Exists(result.Output))
            {
                return Json(new { success = false, version = wd.Version, status = "ERROR", result.Errors });
            }

            // save
            var pages = await GetPublishPagesAsync(wd);
            await this.DeletePreviousPagesAsync(wd);
            //archive the WD
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var archived = new BinaryStore
            {
                StoreId = string.Format("wd.{0}.{1}", wd.WorkflowDefinitionId, wd.Version),
                Content = Encoding.Unicode.GetBytes(wd.ToXmlString()),
                Extension = ".xml",
                FileName = string.Format("wd.{0}.{1}.xml", wd.WorkflowDefinitionId, wd.Version)

            };
            await store.DeleteAsync(archived.StoreId);
            await store.AddAsync(archived);
            await this.Save("Publish", wd, pages.Cast<Entity>().ToArray());

            return Json(new { success = true, version = wd.Version, status = "OK", message = "Your workflow has been successfully compiled and published : " + Path.GetFileName(result.Output) });
        }

        public async Task<ActionResult> Save()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var id = await this.Save("Update", wd);
            return Json(new { success = id > 0, id, status = "OK" });
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

            return Json(new { success = true, id = wd.WorkflowDefinitionId, status = "OK" });
        }

        public async Task<ActionResult> GetVariablePath(int id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == id);
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
            // TODO : get the CLR type info too

            return Json(list.Select(d => new { Path = d }).ToArray(), JsonRequestBehavior.AllowGet);
        }


        private async Task DeletePreviousPagesAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            var pages = new List<Page>();
            foreach (var act in wd.ActivityCollection.OfType<ScreenActivity>())
            {
                var act1 = act;
                var page = await context.LoadOneAsync<Page>(p =>
                                p.Version == wd.Version &&
                                p.Tag == string.Format("wf_{0}_{1}", wd.WorkflowDefinitionId, act1.WebId));
                if (null != page)
                    pages.Add(page);
            }
            using (var session = context.OpenSession())
            {
                session.Delete(pages.Cast<Entity>().ToArray());
                await session.SubmitChanges();
            }
        }

        private async Task<IEnumerable<Page>> GetPublishPagesAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException("wd");
            var screens = wd.ActivityCollection.OfType<ScreenActivity>();
            var pages = new List<Page>();
            foreach (var scr in screens)
            {
                // copy the previous version pages if there's any
                var scr1 = scr;
                var tag = string.Format("wf_{0}_{1}", wd.WorkflowDefinitionId, scr1.WebId);
                var currentVersion = await context.GetMaxAsync<Page, int>(p => p.Tag == tag, p => p.Version);
                var previousPage = await context.LoadOneAsync<Page>(p => p.Tag == tag && p.Version == currentVersion);
                var code = previousPage != null ? previousPage.Code : scr1.GetView(wd);
                var page = new Page
                {
                    Code = code,
                    Name = scr1.Name,
                    IsPartial = false,
                    IsRazor = true,
                    Tag = tag,
                    Version = wd.Version,
                    WebId = Guid.NewGuid().ToString(),
                    VirtualPath = string.Format("~/Views/Workflow_{0}_{1}/{2}.cshtml", wd.WorkflowDefinitionId,
                        wd.Version, scr1.ActionName)
                };


                pages.Add(page);

            }


            return pages;

        }

        private async Task<int> Save(string operation, WorkflowDefinition wd, params Entity[] entities)
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
            return wd.WorkflowDefinitionId;
        }


    }
}