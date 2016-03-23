using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class BinaryStoreController : Controller
    {
        public async Task<ActionResult> Upload(IEnumerable<HttpPostedFileBase> files)
        {

            var storeId = Guid.NewGuid().ToString();

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(file.FileName) ?? "";
                Console.WriteLine("EXT " + extension);
                var stream = file.InputStream;  //initialise new stream
                var content = new byte[stream.Length];
                stream.Read(content, 0, content.Length); // read from stream to byte array
                
                var document = new BinaryStore
                {
                    Content = content,
                    Extension = extension,
                    Id = storeId,
                    WebId = storeId,
                    FileName = fileName
                };
                var binaryStore = ObjectBuilder.GetObject<IBinaryStore>();
                await binaryStore.AddAsync(document);


            }

            return Json(new { storeId });
        }


        public async Task<ActionResult> Get(string id)
        {
            if (id == "sph-img-list")
                return Redirect("/images/list.png");

            var binaryStore = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await binaryStore.GetContentAsync(id);
            if (null == doc)
                return Content("");

            return File(doc.Content, MimeMapping.GetMimeMapping(doc.Extension), doc.FileName);

        }

        [HttpPost]
        public async Task<ActionResult> Remove(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return Json(new { success = false, status = "FAIL" });
            var binaryStore = ObjectBuilder.GetObject<IBinaryStore>();
            await binaryStore.DeleteAsync(id);
            return Json(new { success = true, status = " OK" });

        }

    }
}
