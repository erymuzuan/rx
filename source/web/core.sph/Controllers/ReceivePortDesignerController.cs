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
            return Ok(new { sucess = true });
        }
    }
}