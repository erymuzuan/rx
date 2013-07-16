using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        [ActionName("complaint.custom.field.html")]
        public async Task<ActionResult> ComplaintCustomFieldHtml(int id)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == id);
            return View("ComplaintCustomFieldHtml", template);
        }

        public ActionResult ComplaintListHtml()
        {
            return View();
        }

        public ActionResult ComplaintListJs()
        {
            return View();
        }

    }
}
