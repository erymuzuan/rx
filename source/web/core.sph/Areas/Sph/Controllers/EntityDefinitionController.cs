using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityDefinitionController : Controller
    {

        public async Task<ActionResult> Save()
        {
            var ed = this.GetRequestJson<EntityDefinition>();
            var context = new SphDataContext();

            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Save");
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(ed.EntityDefinitionId.ToString(CultureInfo.InvariantCulture));

        }
    }
}