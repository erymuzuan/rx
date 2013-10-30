using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Controllers
{
    public class WorkflowController : Controller
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