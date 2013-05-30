using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class BinaryStoreController : Controller
    {
        public async Task<ActionResult> Upload(IEnumerable<HttpPostedFileBase> files)
        {

            var storeId = Guid.NewGuid().ToString();

            foreach (var file in files)
            {
                var fileName = System.IO.Path.GetFileName(file.FileName);
                var extension = System.IO.Path.GetExtension(file.FileName);
                System.IO.Stream stream = file.InputStream;  //initialise new stream
                var content = new byte[stream.Length];
                stream.Read(content, 0, content.Length); // read from stream to byte array

                var gpx = new BinaryStore
                {
                    Content = content,
                    Extension = extension,
                    StoreId = storeId,
                    WebId = storeId,
                    FileName = fileName
                };
                var binaryStore = ObjectBuilder.GetObject<IBinaryStore>();
                await binaryStore.AddAsync(gpx);


            }

            return Json(new { storeId });
        }


        public async Task<ActionResult> Get(string id)
        {
            var binaryStore = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await binaryStore.GetContentAsync(id);
            if (null == doc)
                return Content("");

            return File(doc.Content, MimeMapping.GetMimeMapping(doc.Extension), doc.FileName);

        }

        [HttpPost]
        public async Task<ActionResult> Remove(string fileNames)
        {
            var storeId = Session["GpxStoreId"] as string;
            if (string.IsNullOrWhiteSpace(storeId)) return Json(new { OK = true });

            var binaryStore = ObjectBuilder.GetObject<IBinaryStore>();
            await binaryStore.DeleteAsync(storeId);
            return Json(new { OK = true });

        }

    }
}
