using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("receive-ports")]
    public class ReceivePortDesignerController : BaseApiController
    {
        [HttpPost]
        [PostRoute("")]
        public async Task<IHttpActionResult> SaveAsync([JsonBody]ReceivePort port)
        {
            var portIsNewItem = port.IsNewItem;
            if (portIsNewItem)
                port.Id = port.Name.ToIdFormat();

            if (portIsNewItem)
            {
                // generate the mapping
                var fields = await port.TextFormatter.GetFieldMappingsAsync();
                port.FieldMappingCollection.ClearAndAddRange(fields);
            }
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(port);
                await session.SubmitChanges();

            }
            if (portIsNewItem)
                return Created("/receive-ports/" + port.Id, new { success = true, id = port.Id });
            return Ok(new { success = true, id = port.Id });
        }

        [HttpPost]
        [PostRoute("{id}/publish")]
        public async Task<IHttpActionResult> CompileAsync([JsonBody]ReceivePort port, string id)
        {
            var portIsNewItem = port.IsNewItem;
            if (portIsNewItem)
                port.Id = port.Name.ToIdFormat();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(port);
                await session.SubmitChanges();
            }

            var cr = await port.CompileAsync();
            if (!cr.Result)
                return Invalid(HttpStatusCode.BadRequest, cr);

            if (portIsNewItem)
                return Created($"/receive-ports/{port.Id}/publish", new { success = true });
            return Ok(new { success = true });
        }
        [HttpPost]
        [PostRoute("{id}/generate-entity-definition")]
        public async Task<IHttpActionResult> GenerateEntityDefinitionAsync([JsonBody]ReceivePort port, string id)
        {
            var ed = await port.GenerateEntityDefinitionAsync();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges();
            }

            return Created($"/entity-definitions/{ed.Id}", new { success = true });
        }

        [HttpGet]
        [Route("{id}/text")]
        public async Task<IHttpActionResult> GetSampleTextAsync(string id)
        {
            var context = new SphDataContext();
            var port = context.LoadOneFromSources<ReceivePort>(x => x.Id == id);
            if (null == port) return NotFound($"Cannot find port with id '{id}'");
            var storeId = port.TextFormatter.SampleStoreId;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(storeId);
            if (null == doc) return NotFound($"Cannot load any sample for port '{port.Name}'");
            var text = System.Text.Encoding.UTF8.GetString(doc.Content);

            return Ok(text, "text/plain");

        }
    }
}