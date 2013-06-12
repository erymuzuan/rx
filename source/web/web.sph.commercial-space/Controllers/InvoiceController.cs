using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class InvoiceController : Controller
    {
        public async Task<ActionResult> GetAddhocInvoiceNo(int contractId, string type)
        {
            var context = new SphDataContext();
            var template = await context.GetScalarAsync<Setting, string>(s => s.Key == "Invoice.Adhoc.Format", s => s.Value);
            var contract = await context.LoadOneAsync<Contract>(c => c.ContractId == contractId);
            var maxId = await context.GetMaxAsync(context.Invoices.Where(c => c.ContractNo == contract.ReferenceNo), v => v.InvoiceId);
            var model = new
                {
                    Contract = contract,
                    MaxId = maxId,
                    Type = type
                };
            var tempateEngine = ObjectBuilder.GetObject<ITemplateEngine>();
            var no = await tempateEngine.GenerateAsync(template, model);

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(no));

        }

        public async Task<ActionResult> SaveAdhocInvoice(AdhocInvoice invoice)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(invoice);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> SaveRent(Rent invoice)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(invoice);
                await session.SubmitChanges();
            }

            return Json(true);
        }

    }
}
