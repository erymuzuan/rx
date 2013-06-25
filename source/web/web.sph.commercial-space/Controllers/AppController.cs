using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Bespoke.Station.Web.Controllers
{
    public partial class AppController : Controller
    {
        public const string APPLICATION_JAVASCRIPT = "application/javascript";
        public const string TEXT_HTML = "text/html";
        
        public ActionResult ViewModels(string id)
        {
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderScript(id.Replace(".js", string.Empty) + "Js");
            return Content(script);
        }
        public ActionResult Views(string id)
        {
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderHtml(id.Replace(".", string.Empty));
            return Content(script);
        }

        private string RenderHtml(string view)
        {
            var html = new StringBuilder();
            using (var sw = new StringWriter(html))
            {
                var viewContext = new ViewContext(ControllerContext, new RazorView(ControllerContext, "fakePath", null, false, null), ViewData, TempData, sw);
                var helper = new HtmlHelper(viewContext, new ViewPage());
                helper.RenderAction(view);

                sw.Flush();
                sw.Close();
            }


            return html.ToString();
        }

        private string RenderScript(string view)
        {
            var html = this.RenderHtml(view);

            const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline;

            var matches = Regex.Matches(html, @"<script type=\""text/javascript\"" data-script=\""true\"">(?<script>.*?)</script>", option);
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

    }
}
