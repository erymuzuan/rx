using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("api/value-object-definition")]
    public class ValueObjectDefinitionController : BaseApiController
    {

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]ValueObjectDefinition ed)
        {
            var context = new SphDataContext();
            var canSave = ed.CanSave();
            if (!canSave.Result)
            {
                return Json(new { success = false, status = "ERROR", message = "Your ValueObjectDefinition cannot be save", errors = canSave.Errors.ToArray() });
            }

            var brandNewItem = ed.IsNewItem;
            if (brandNewItem)
            {
                ed.Id = ed.Name.ToIdFormat();
            }
            else
            {
                using (var session = context.OpenSession())
                {
                    session.Attach(ed);
                    await session.SubmitChanges("Save");
                }
                return Json(new { success = true, status = "OK", message = "Your ValueObjectDefinition has been successfully saved ", id = ed.Id });

            }



            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.Id });


        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            var context = new SphDataContext();
            var vod = await context.LoadOneAsync<ValueObjectDefinition>(e => e.Id == id);
            if (null == vod) return NotFound("Cannot find value object definition to delete, id : " + id);
            var warnings = await vod.CanDeleteAsync();
            if (warnings.Any()) return Invalid(HttpStatusCode.Forbidden, warnings);

            using (var session = context.OpenSession())
            {
                session.Delete(vod);
                await session.SubmitChanges("delete");
            }
            return Json(new { success = true, status = "OK", message = "Your value object definition has been successfully deleted", id = vod.Id });

        }

    }
}