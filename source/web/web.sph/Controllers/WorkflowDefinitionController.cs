using System;
using System.IO;
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

        public ActionResult ScreenHtml()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Space).Name };
            return View(vm);
        }

        public ActionResult ScreenJs()
        {
            var vm = new TemplateFormViewModel { Entity = typeof(Space).Name };
            vm.FormElements.RemoveAll(
                f => f.GetType() == typeof(FormElement) || f.GetType() == typeof(CustomListDefinitionElement));
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
            await this.Save(wd);
            try
            {
                var options = new CompilerOptions();
                options.ReferencedAssemblies.Add(typeof(Controller).Assembly);
                options.ReferencedAssemblies.Add(typeof(WorkflowDefinitionController).Assembly);
                var result = wd.Compile(options);
                return Json(new { success = true, status = "OK", message = "Your workflow has been successfully compiled  : " + Path.GetFileName(result.Output) });

            }
            catch (Exception e)
            {
                return Json(new { success = true, status = "OK", message = e.Message });

            }

        }

        public async Task<ActionResult> Publish()
        {
            var wd = this.GetRequestJson<WorkflowDefinition>();
            wd.Version += 1;// publish will increase the version

            await this.Save(wd);

            var options = new CompilerOptions();
            options.ReferencedAssemblies.Add(typeof(Controller).Assembly);
            options.ReferencedAssemblies.Add(typeof(WorkflowDefinitionController).Assembly);

            var result = wd.Compile(options);
            // copy the output to bin
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

        private async Task<int> Save(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            if (null == wd) throw new ArgumentNullException("wd");
            var screens = (from s in wd.ActivityCollection.OfType<ScreenActivity>()
                           select new Page
                           {
                               VirtualPath = string.Format("~/Views/Workflow_{0}_{1}/{2}.cshtml", wd.WorkflowDefinitionId, wd.Version, s.Title.Replace(" ", string.Empty)),
                               Code = s.GetView(wd),
                               Title = s.Title,
                               IsPartial = false,
                               IsRazor = true,
                               WebId = Guid.NewGuid().ToString()
                           })
                              .ToArray();
            var paths = screens.Select(s => s.VirtualPath).ToArray();

            using (var session = context.OpenSession())
            {
                session.Attach(wd);
                session.Attach(screens.Cast<Entity>().ToArray());


                if (paths.Any())
                {
                    var existingPages = await context.LoadAsync(context.Pages.Where(p => paths.Contains(p.VirtualPath)));
                    if (existingPages.ItemCollection.Any())
                        session.Delete(existingPages.ItemCollection.Cast<Entity>().ToArray());

                }

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