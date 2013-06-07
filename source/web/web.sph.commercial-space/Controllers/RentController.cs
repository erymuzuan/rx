using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class RentController : Controller
    {
        public async Task<Rent>Save(int id, IEnumerable<PaymentDistribution> rents)
        {
            var context = new SphDataContext();
            var rent = await context.LoadOneAsync<Rent>(d => d.RentId == id);
            if (rent.PaymentDistributionCollection.Any())
                rent.PaymentDistributionCollection.ClearAndAddRange(rents);
            
            else rent.PaymentDistributionCollection.AddRange(rents);

            using (var session = context.OpenSession())
            {
                session.Attach(rent);
                await session.SubmitChanges();
            }

            return rent;


        }

    }
}
