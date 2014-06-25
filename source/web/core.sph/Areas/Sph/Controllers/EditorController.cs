using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize(Roles = "developers")]
    public class EditorController : Controller
    {
        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Save(string file, string code)
        {
            var filed = Server.MapPath(file);
            System.IO.File.WriteAllText(filed, code);
            return Json(new {success = true, status = "OK"});
        }
        public ActionResult Code(string id)
        {
            var file = Server.MapPath(id);
            if (!System.IO.File.Exists(file))
                System.IO.File.WriteAllText(file, "");
            var js = System.IO.File.ReadAllText(file);
            return Content(js);
        }

        public ActionResult File(string id)
        {
            var ext = System.IO.Path.GetExtension(id);
            string mode;

            switch (ext)
            {
                case ".html":
                case ".htm":
                    mode = "html";
                    break;
                case ".cs":
                    mode = "csharp";
                    break;
                case ".css":
                    mode = "css";
                    break;
                case ".js":
                    mode = "javascript";
                    break;
                default:
                    throw new Exception("Don't know any for " + ext);
            }
            var vm = new
            {
                File = id,
                Mode = mode
            };
            return View(vm);
        }


        public ActionResult Ace(string file = null)
        {
            if (null != file)
            {
                return View(new { File = file });
            }
            return View();
        }
        public async Task<ActionResult> Page(int id)
        {
            var context = new SphDataContext();
            var page = await context.LoadOneAsync<Page>(p => p.PageId == id);
            var vm = new EditorPageViewModel { Page = page };
            return View(vm);
        }

        [NoCache]
        public ActionResult Snippets(string id)
        {
            var folder = Server.MapPath("~/App_Data/snippets/" + id);
            var snippets = from f in System.IO.Directory.GetFiles(folder, "*.json")
                           let json = System.IO.File.ReadAllText(f)
                           select json;

            this.Response.ContentType = "application/json";
            return Content("[" + string.Join(",", snippets) + "]");
        }

        public ActionResult SaveSnippet()
        {
            var snippet = this.GetRequestJson<Snippet>();
            var file = Server.MapPath(string.Format("~/App_Data/snippets/{0}/{1}.json", snippet.Lang, snippet.Title));
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            System.IO.File.WriteAllText(file, JsonConvert.SerializeObject(snippet, Formatting.Indented, settings));
            return Json(new { status = "OK", success = true });
        }
    }
}