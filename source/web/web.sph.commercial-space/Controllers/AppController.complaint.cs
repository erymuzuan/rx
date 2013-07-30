using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public async Task<ActionResult> ComplaintFormHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == templateId);

            return View(template.CustomFieldCollection);
        }

    }
}
