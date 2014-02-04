using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Humanizer;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityDefinitionController : Controller
    {
        public async Task<ActionResult> GetVariablePath(int id)
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(w => w.EntityDefinitionId == id);
            var list = ed.GetMembersPath();


            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Save()
        {
            var ed = this.GetRequestJson<EntityDefinition>();
            var context = new SphDataContext();

            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.EntityDefinitionId });


        }


        public ActionResult GetPlural(string id)
        {
            return Content(id.Pluralize());
        }
        public async Task<ActionResult> Schemas()
        {
            var context = new SphDataContext();
            var query = context.EntityDefinitions;
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var list = new ObjectCollection<EntityDefinition>(lo.ItemCollection);

            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                list.AddRange(lo.ItemCollection);
            }

            var script = new StringBuilder();
            foreach (var ef in list)
            {
                var code = await ef.GenerateCustomXsdJavascriptClassAsync();
                script.AppendLine(code);
            }

            this.Response.ContentType = "application/javascript";
            return Content(script.ToString());
        }

        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityDefinition>();
            var buildValidation = ed.ValidateBuild();
            var form = await context.LoadOneAsync<EntityForm>(f => f.IsDefault == true
                && f.EntityDefinitionId == ed.EntityDefinitionId);
            if (null == form)
            {
                buildValidation.Result = false;
                buildValidation.Errors.Add(new BuildError(ed.WebId, "Please set a default form"));
            }

            if (!buildValidation.Result)
                return Json(buildValidation);

            ed.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully published", id = ed.EntityDefinitionId });


        }

        public ActionResult BusinessRuleDialog()
        {
            return View();
        }


    }
}