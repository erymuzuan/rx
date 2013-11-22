using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Controllers
{
    public class WorkflowDefinitionController : Controller
    {
        public const string APPLICATION_JAVASCRIPT = "application/javascript";
        public const string TEXT_HTML = "text/html";


        public ActionResult Visual()
        {
            var vm = new WorkflowDefinitionVisualViewModel();
            vm.ToolboxElements.Add(new ScreenActivity());
            return View(vm);
        }


        public async Task<ActionResult> Export()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var path = Path.GetTempPath() + @"/wd" + wd.WorkflowDefinitionId;
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";
            var context = new SphDataContext();

            var store = ObjectBuilder.GetObject<IBinaryStore>();

            var schema = await store.GetContentAsync(wd.SchemaStoreId);
            System.IO.File.WriteAllBytes(Path.Combine(path, wd.SchemaStoreId + ".xsd"), schema.Content);
            System.IO.File.WriteAllBytes(Path.Combine(path, "wd_" + wd.WorkflowDefinitionId + ".json"), Encoding.UTF8.GetBytes(wd.ToJsonString()));
            // get the screen view
            foreach (var screen in wd.ActivityCollection.OfType<ScreenActivity>())
            {
                var screen1 = screen;
                var page =
                    await
                        context.LoadOneAsync<Page>(
                            p => p.Version == wd.Version && p.Tag == string.Format("wf_{0}_{1}", wd.WorkflowDefinitionId, screen1.WebId));
                if (null != page)
                {
                    System.IO.File.WriteAllBytes(Path.Combine(path, "page." + page.PageId + ".json"), Encoding.UTF8.GetBytes(page.ToJsonString()));

                }
            }
            if (System.IO.File.Exists(zip))
                System.IO.File.Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);
            var zd = new BinaryStore
            {
                StoreId = Guid.NewGuid().ToString(),
                Content = System.IO.File.ReadAllBytes(zip),
                Extension = ".zip",
                FileName = string.Format("wd_{0}_{1}.zip", wd.WorkflowDefinitionId, wd.Version),
                WebId = Guid.NewGuid().ToString()
            };
            await store.AddAsync(zd);
            return Json(new { success = true, status = "OK", url = this.Url.Action("Get", "BinaryStore", new { id = zd.StoreId }) });
        }

        public ActionResult ScreenHtml()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Space).Name };
            return View(vm);
        }

        public ActionResult ScreenJs()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Space).Name };
            vm.FormElements.RemoveAll(
                f => f.GetType() == typeof(FormElement));
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderRazorViewToJs("ScreenJs", vm);
            return Content(script);
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

            await this.Save(wd);

            var options = new CompilerOptions();
            options.ReferencedAssemblies.Add(typeof(Controller).Assembly);
            options.ReferencedAssemblies.Add(typeof(WorkflowDefinitionController).Assembly);
            var result = wd.Compile(options);
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
                SourceCodeDirectory = ConfigurationManager.AppSettings["sph:WorkflowSourceDirectory"] ?? string.Empty
            };
            options.ReferencedAssemblies.Add(typeof(Controller).Assembly);
            options.ReferencedAssemblies.Add(typeof(WorkflowDefinitionController).Assembly);

            var result = wd.Compile(options);
            if (!result.Result || !System.IO.File.Exists(result.Output))
            {
                return Json(new { success = false, version = wd.Version, status = "ERROR", messages = result.Errors });
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
            await this.Save(wd, pages.Cast<Entity>().ToArray());


            // Deploy
            System.IO.File.Copy(result.Output, Server.MapPath("~/bin/" + Path.GetFileName(result.Output)), true);
            var pdb = result.Output.Replace(".dll", ".pdb");
            if (System.IO.File.Exists(pdb))
                System.IO.File.Copy(pdb, Server.MapPath("~/bin/" + Path.GetFileName(pdb)), true);
            return Json(new { success = true, version = wd.Version, status = "OK", message = "Your workflow has been successfully compiled and published : " + Path.GetFileName(result.Output) });
        }

        public async Task<ActionResult> Save()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            var id = await this.Save(wd);
            return Json(id);
        }

        public async Task<ActionResult> GetVariablePath(int id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == id);
            var list = wd.VariableDefinitionCollection.Select(v => v.Name).ToList();
            var schema = wd.GetCustomSchema();
            var xsd = new XsdMetadata(schema);
            foreach (var v in wd.VariableDefinitionCollection.OfType<ComplexVariable>())
            {
                list.AddRange(xsd.GetMembersPath(v.TypeName).Select(x => v.Name + "." + x));
            }

            return Json(list, JsonRequestBehavior.AllowGet);
        }


        private async Task DeletePreviousPagesAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            var pages = new List<Page>();
            foreach (var act in wd.ActivityCollection.OfType<ScreenActivity>())
            {
                var act1 = act;
                var page =
                    await
                        context.LoadOneAsync<Page>(
                            p =>
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
                    Title = scr1.Name,
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

        private async Task<int> Save(WorkflowDefinition wd, params Entity[] entities)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException("wd");


            using (var session = context.OpenSession())
            {
                if (entities.Any())
                    session.Attach(entities);

                session.Attach(wd);
                await session.SubmitChanges();
            }
            return wd.WorkflowDefinitionId;
        }



        private static string ExtractScriptFromHtml(string html)
        {
            const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            var matches = Regex.Matches(html,
                @"<script type=\""text/javascript\"" data-script=\""true\"">(?<script>.*?)</script>", option);
            if (matches.Count == 1)
                return matches[0].Groups["script"].Value;
            if (matches.Count == 0)
                return string.Empty;

            var scripts = new StringBuilder();
            foreach (Match m in matches)
            {
                scripts.AppendLine(m.Groups["script"].Value);
                scripts.AppendLine();
            }

            return scripts.ToString();
        }
        public string RenderRazorViewToJs(string viewName, object model)
        {
            var html = this.RenderRazorViewToHtml(viewName, model);
            return ExtractScriptFromHtml(html);
        }
        public string RenderRazorViewToHtml(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}