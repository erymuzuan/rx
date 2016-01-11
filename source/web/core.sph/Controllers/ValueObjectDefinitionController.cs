using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("value-object-definition")]
    public class ValueObjectDefinitionController : BaseController
    {
        static ValueObjectDefinitionController()
        {
            DeveloperService.Init();
        }
        public const string ED_SCHEMA = "ed-schema";

        private void DeleteEdSchemaCache()
        {
            System.Web.HttpContext.Current.Cache.Remove(ED_SCHEMA);
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            this.DeleteEdSchemaCache();
            var ed = this.GetRequestJson<ValueObjectDefinition>();
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
        public async Task<ActionResult> Delete(string id)
        {
            this.DeleteEdSchemaCache();
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == id);
            if (null == ed) return new HttpNotFoundResult("Cannot find entity definition to delete, id : " + id);

            var forms = context.LoadFromSources<EntityForm>(f => f.EntityDefinitionId == id);
            var views = context.LoadFromSources<EntityView>(f => f.EntityDefinitionId == id);
            var triggers = context.LoadFromSources<Trigger>(f => f.Entity == id);

            using (var session = context.OpenSession())
            {
                session.Delete(ed);
                session.Delete(forms.Cast<Entity>().ToArray());
                session.Delete(views.Cast<Entity>().ToArray());
                session.Delete(triggers.Cast<Entity>().ToArray());
                // TODO : drop the tables and elastic search mappings
                await session.SubmitChanges("delete");
            }
            return Json(new { success = true, status = "OK", message = "Your entity definition has been successfully deleted", id = ed.Id });

        }

        
    }
}