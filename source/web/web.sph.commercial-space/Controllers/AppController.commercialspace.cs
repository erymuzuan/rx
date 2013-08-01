using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public async Task<ActionResult> CommercialSpaceDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<CommercialSpaceTemplate>(t => t.CommercialSpaceTemplateId == templateId);

            return View(template.CustomFieldCollection);
        }

    }
}
