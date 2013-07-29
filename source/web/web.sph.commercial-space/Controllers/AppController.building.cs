using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {

        public async Task<ActionResult> BuildingDetailHtml(int id = 1)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.QueryString["id"]))
                id = int.Parse(this.Request.QueryString["id"]);

            var context = new SphDataContext();
            var template = await context.LoadOneAsync<BuildingTemplate>(t => t.BuildingTemplateId == id);

            return View(template.CustomFieldCollection);
        }

    }
}
