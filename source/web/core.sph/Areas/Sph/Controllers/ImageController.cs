using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class ImageController : Controller
    {
        public async Task<ActionResult> Store(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/images/no-image.png");

            if (id == "sph-img-list")
                return Redirect("/images/list.png");
            if (id == "sph-img-document")
                return Redirect("/images/document.png");

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(id);
            if (null == content)
                return Redirect("/images/no-image.png");

            return File(content.Content, MimeMapping.GetMimeMapping(content.FileName));


        }

        [OutputCache(VaryByParam = "id",Duration = 300)]
        public async Task<ActionResult> Thumbnail(string id, float height = 150)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/images/no-image.png");

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var content = await store.GetContentAsync(id);
            if (null == content)
                return Redirect("/images/no-image.png");

            var thumbnail = Server.MapPath("~/thumbnails/" + id + content.Extension);

            if (System.IO.File.Exists(thumbnail)) return File(thumbnail, MimeMapping.GetMimeMapping(thumbnail));

            Console.WriteLine("Creating thumbnails... please wait");
            var file = Path.GetTempFileName() + content.Extension;
            System.IO.File.WriteAllBytes(file, content.Content);

            var setting = $"height={height};format=jpg;mode=max";
            var i = new ImageResizer.ImageJob(file, "~/thumbnails/" + id + content.Extension,
                new ImageResizer.Instructions(setting)) { CreateParentDirectory = true };
            i.Build();
            System.IO.File.Delete(file);
            return File(thumbnail, MimeMapping.GetMimeMapping(thumbnail));
        }

        [OutputCache(CacheProfile = "Long")]
        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/images/blank.png");
            var type = Strings.GetType(id);
            if (type == typeof(DocumentField))
                return RedirectPermanent("~/images/DocumentField.png");
            if (type == typeof(FunctionField))
                return RedirectPermanent("~/images/FunctionField.png");
            if (type == typeof(ConstantField))
                return RedirectPermanent("~/images/ConstantField.png");
            if (type == typeof(SetterAction))
                return RedirectPermanent("~/images/SetterAction.png");
            if (type == typeof(EmailAction))
                return RedirectPermanent("~/images/EmailAction.png");
            if (type == typeof(StartWorkflowAction))
                return RedirectPermanent("~/images/StartWorkflowAction.png");
            if (type == typeof(PropertyChangedField))
                return RedirectPermanent("~/images/PropertyChangedField.png");
            if (type == typeof(AssemblyField))
                return RedirectPermanent("~/images/AssemblyField.png");
            if (type == typeof(JavascriptExpressionField))
                return RedirectPermanent("~/images/JavascriptExpressionField.png");
            if (type == typeof(Field))
                return RedirectPermanent("~/images/Field.png");

            if (null != type && System.IO.File.Exists(Server.MapPath("~/images/" + type.Name + ".png")))
            {
                return Redirect("~/images/" + type.Name + ".png");
            }

            return Redirect("/images/no-image.png");
        }

    }
}
