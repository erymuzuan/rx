using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class ImageController : Controller
    {

        public async Task<ActionResult> Store(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/images/no-image.png");

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

            var setting = string.Format("height={0};format=jpg;mode=max", height);
            var i = new ImageResizer.ImageJob(file, "~/thumbnails/" + id + content.Extension,
                new ImageResizer.ResizeSettings(setting)) { CreateParentDirectory = true };
            i.Build();
            System.IO.File.Delete(file);
            return File(thumbnail, MimeMapping.GetMimeMapping(thumbnail));
        }


        public ActionResult Index(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Redirect("/images/blank.png");
            var type = Type.GetType(id);
            if (type == typeof(DocumentField))
                return RedirectPermanent("~/images/documentfield.png");
            if (type == typeof(FunctionField))
                return RedirectPermanent("~/images/FunctionField.png");
            if (type == typeof(ConstantField))
                return RedirectPermanent("~/images/ConstantField.png");
            if (type == typeof(SetterAction))
                return RedirectPermanent("~/images/SetterAction.png");
            if (type == typeof(EmailAction))
                return RedirectPermanent("~/images/EmailAction.png");
            if (type == typeof(PropertyChangedField))
                return RedirectPermanent("~/images/PropertyChangedField.png");
            if (type == typeof(AssemblyField))
                return RedirectPermanent("~/images/AssemblyField.png");

            return Content("www");
        }

    }
}
