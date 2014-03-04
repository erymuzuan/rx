using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class DocumentTemplateController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<DocumentTemplate>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.DocumentTemplateId });
        }
        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var template = this.GetRequestJson<DocumentTemplate>();
            template.IsPublished = true;
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == template.Entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully published", id = template.DocumentTemplateId });

        }
    }
}