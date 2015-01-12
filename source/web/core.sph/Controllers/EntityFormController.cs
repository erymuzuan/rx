using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("entity-form")]
    public class EntityFormController : Controller
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<EntityForm>();
            var context = new SphDataContext();

            if (string.IsNullOrWhiteSpace(ef.Id) || ef.Id == "0")
                ef.Id = ef.Route.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.Id });
        }

        [HttpPost]
        [Route("depublish")]
        public async Task<ActionResult> Depublish()
        {
            var context = new SphDataContext();
            var form = this.GetRequestJson<EntityForm>();

            // look for column which points to the form
// ReSharper disable RedundantBoolCompare
            var viewQuery = context.EntityViews.Where(e => e.IsPublished == true && e.EntityDefinitionId == form.EntityDefinitionId);
// ReSharper restore RedundantBoolCompare
            var viewLo = await context.LoadAsync(viewQuery, includeTotalRows: true);
            var views = new ObjectCollection<EntityView>(viewLo.ItemCollection);
            while (viewLo.HasNextPage)
            {
                viewLo = await context.LoadAsync(viewQuery, viewLo.CurrentPage + 1, includeTotalRows: true);
                views.AddRange(viewLo.ItemCollection);
            }

            var violations = (from vw in views
                             where vw.ViewColumnCollection.Any(c => c.IsLinkColumn 
                                 && c.FormRoute == form.Route)
                             select vw.Name).ToArray();
            if (violations.Any()) 
                return Json(new { success = false, status = "NO", message = "These views has a link to your form ", views = violations, id = form.Id });


            form.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Depublish");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully depublished", id = form.Id });


        }

        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var form = this.GetRequestJson<EntityForm>();
            form.IsPublished = true;
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == form.EntityDefinitionId);

            var buildValidation = await form.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully published", id = form.Id });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<EntityForm>(e => e.Id == id);
            if (null == form)
                return new HttpNotFoundResult("Cannot find form to delete , Id : " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(form);
                await session.SubmitChanges("Remove");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully deleted", id = form.Id });

        }
    }
}