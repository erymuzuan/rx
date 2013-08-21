using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController : Controller
    {
        public const string APPLICATION_JAVASCRIPT = "application/javascript";
        public const string TEXT_HTML = "text/html";

        public ActionResult ViewModels(string id)
        {
            var customJsRoute = this.GetCustomJsRouting(id, true);
            if (!string.IsNullOrWhiteSpace(customJsRoute))
                return Redirect(customJsRoute);

            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderScript(id.Replace(".", string.Empty));
            return Content(script);
        }

        public ActionResult Views(string id)
        {
            var customJsRoute = this.GetCustomJsRouting(id, false);
            if (!string.IsNullOrWhiteSpace(customJsRoute))
                return Redirect(customJsRoute);

            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.ContentType = TEXT_HTML;
            var script = this.RenderHtml(id.Replace(".", string.Empty));
            return Content(script);
        }

        private string GetCustomJsRouting(string id, bool js)
        {
            var v = js ? "viewmodels" : "views";
            var extension = js ? "js" : "html";

            if (id.Contains("-"))
            {
                var splits = id.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                var module = splits.First();
                var file = string.Format("~/App/{0}/{1}.{2}", v, module, extension);
                if (System.IO.File.Exists(Server.MapPath(file)))
                    return file;

                var parameters = splits.Where(s => s.Contains("."))
                    .Select(p => p.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries))
                    .Where(ss => ss.Length >= 2)
                    .Select(ss => string.Format("{0}={1}", ss.First(), ss[1])).ToList();

                return string.Format("/App/{0}{1}?{2}", module.Replace(".", string.Empty), extension, string.Join("&", parameters));
            }
            return null;
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
