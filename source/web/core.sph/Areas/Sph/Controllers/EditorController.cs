using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static  System.IO.File;

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
            WriteAllText(filed, code);
            return Json(new { success = true, status = "OK" });
        }

        [NoCache]
        public ActionResult Code(string id)
        {
            var file = Server.MapPath(id);
            if (!Exists(file))
                WriteAllText(file, "");
            var js = ReadAllText(file);
            return Content(js);
        }

        public ActionResult File(string id)
        {
            var ext = Path.GetExtension(id);
            string mode;

            switch (ext)
            {
                case ".cshtml":
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
                    throw new Exception("Don't know any extension mode for " + ext);
            }
            var vm = new
            {
                File = id,
                Mode = mode
            };
            return View(vm);
        }


        public ActionResult Download()
        {
            var path = Server.MapPath("~/App_Data/snippets");
            var zip = Path.GetTempPath() + $"\\rx.package.snippets{DateTime.Now.Ticks}.zip";

            ZipFile.CreateFromDirectory(path, zip);
            return File(zip, MimeMapping.GetMimeMapping(zip), $"rx.package.snippets.{DateTime.Now:yyyyMMdd}.zip");
        }


        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                foreach (var postedFile in files)
                {
                    var fileName = Path.GetFileName(postedFile.FileName);
                    if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("Filename is empty or null");


                    var zip = Path.Combine(Path.GetTempPath(), fileName);
                    postedFile.SaveAs(zip);

                    var folder = Directory.CreateDirectory(Path.GetTempFileName() + "extract").FullName;
                    ZipFile.ExtractToDirectory(zip, folder);
                    foreach (var json in Directory.GetFiles(folder, "*.json", SearchOption.AllDirectories))
                    {
                        var lang = "javascript";
                        if (json.Contains("html"))
                            lang = "html";
                        if (json.Contains("csharp"))
                            lang = "csharp";
                        if (json.Contains("css"))
                            lang = "css";
                        var path = Server.MapPath($"~/App_Data/snippets/{lang}/{Path.GetFileName(json)}");
                        Copy(json, path, true);
                    }

                    return Json(new { success = true, status = "OK" });

                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
            return Json(new { success = false });


        }

        public ActionResult Ace(string file = null)
        {
            if (null != file)
            {
                return View(new { File = file });
            }
            return View();
        }
        public async Task<ActionResult> Page(string id)
        {
            var context = new SphDataContext();
            var page = await context.LoadOneAsync<Page>(p => p.Id == id);
            var vm = new EditorPageViewModel { Page = page };
            return View(vm);
        }

        [NoCache]
        public ActionResult Snippets(string id)
        {
            var folder = Server.MapPath("~/App_Data/snippets/" + id);
            var snippets = from f in Directory.GetFiles(folder, "*.json")
                           let json = ReadAllText(f)
                           select json;

            this.Response.ContentType = "application/json";
            return Content("[" + string.Join(",", snippets) + "]");
        }

        public ActionResult SaveSnippet()
        {
            var snippet = this.GetRequestJson<Snippet>();
            var file = Server.MapPath($"~/App_Data/snippets/{snippet.Lang}/{snippet.Title}.json");
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            WriteAllText(file, JsonConvert.SerializeObject(snippet, Formatting.Indented, settings));
            return Json(new { status = "OK", success = true });
        }
    }
}