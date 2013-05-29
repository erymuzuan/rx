using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ContractSettingController : Controller
    {
        public async Task<ActionResult> SaveContractType(string contractType)
        {
            var context = new SphDataContext();
            var contractTemplate = new ContractTemplate
                {
                    Type = contractType
                };
            using (var session = context.OpenSession())
            {
                session.Attach(contractTemplate);
                await session.SubmitChanges();
            }
            return Json(contractType);
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
