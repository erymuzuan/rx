using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class PaymentController : Controller
    {
        public async Task<Payment> Save(Payment payment)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(payment);
                await session.SubmitChanges();
            }

            return payment;
        }

    }
}
