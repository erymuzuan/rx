using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class InvoiceController : Controller
    {
       
        public async Task<ActionResult> SaveAdhoc(IEnumerable<Invoice> adhocInvoice)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(adhocInvoice.Cast<Entity>().ToArray());
                await session.SubmitChanges();
            }

            return Json(true);
        }   

    }
}
    