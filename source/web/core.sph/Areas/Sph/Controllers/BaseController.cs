using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class BaseController : Controller
    {

        public const string APPLICATION_JAVASCRIPT = "application/javascript";
        public const string TEXT_HTML = "text/html";

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
        protected string RenderRazorViewToJs(string viewName, object model)
        {
            var html = this.RenderRazorViewToHtml(viewName, model);
            return ExtractScriptFromHtml(html);
        }
        protected string RenderRazorViewToHtml(string viewName, object model)
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