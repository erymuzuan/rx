using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ContractSettingController : Controller
    {
        public async Task<ActionResult> Save(ContractTemplate template)
        {
            var context = new SphDataContext();
            var item = await
                       context.LoadOneAsync<ContractTemplate>(t => t.ContractTemplateId == template.ContractTemplateId)
                       ?? template;
       

            using (var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges();
            }
            return Json(template);
        }

        public async Task<ActionResult> SaveDocumentTemplate(int id,ObjectCollection<DocumentTemplate> documentTemplates)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<ContractTemplate>(c => c.ContractTemplateId == id);
            dbItem.DocumentTemplateCollection.ClearAndAddRange(documentTemplates);
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }
            return Json(id);
        }

    }
}
