using System.Linq;
using Bespoke.Sph.Domain;
using System.Collections.Generic;

namespace Bespoke.DevV1.Integrations.Transforms
{
    public partial class SocToSnbSalesOrder
    {

        private static List<Bespoke.DevV1.Products.Domain.Product> m_products = new List<Bespoke.DevV1.Products.Domain.Product>();
        private static List<Bespoke.DevV1.SurchargeAddOns.Domain.SurchargeAddOn> m_surcharges = new List<Bespoke.DevV1.SurchargeAddOns.Domain.SurchargeAddOn>();

        partial void BeforeTransform(Bespoke.DevV1.SalesOrders.Domain.SalesOrder item, Bespoke.DevV1.Adapters.SnbWebNewAccount.PostSalesOrdersRequest destination)
        {
            var context = new SphDataContext();
            if (m_products.Count == 0)
            {
                var query = context.CreateQueryable<Bespoke.DevV1.Products.Domain.Product>()
                                .Where(p => p.Id != "0");
                var productRepos = ObjectBuilder.GetObject<IRepository<Bespoke.DevV1.Products.Domain.Product>>();
                var lo = productRepos.LoadAsync(query, 1, 200, false).Result;
                m_products.AddRange(lo.ItemCollection);
            }
            if (m_surcharges.Count == 0)
            {
                var surchargeQuery = context.CreateQueryable<Bespoke.DevV1.SurchargeAddOns.Domain.SurchargeAddOn>()
                                .Where(p => p.Id != "0");
                var surchargeRepos = ObjectBuilder.GetObject<IRepository<Bespoke.DevV1.SurchargeAddOns.Domain.SurchargeAddOn>>();
                var scLo = surchargeRepos.LoadAsync(surchargeQuery, 1, 200, false).Result;
                m_surcharges.AddRange(scLo.ItemCollection);
            }
        }

        partial void AfterTransform(Bespoke.DevV1.SalesOrders.Domain.SalesOrder item, Bespoke.DevV1.Adapters.SnbWebNewAccount.PostSalesOrdersRequest destination)
        {

            // write some some code since LoopingFunctoid has some designers issue
            var consignments = (from c in item.Consignments
                               select new Bespoke.DevV1.Adapters.SnbWebNewAccount.ConsignmentsItem
                               {
                                   ConNoteNo = c.ConNoteNumberParent,
                                   ActualWeight = c.Weight,
                                   ReceiverPostCode = c.ShiptoPartyPostcode,
                                   ProductCode = m_products.Where(x => x.SocCode == c.ProductCodeMaterial).Select(x => x.Code).LastOrDefault() ?? "-",
                                   ItemCategory = "1"
                               }).ToList();
            foreach (var con in consignments)
            {
                var source = item.Consignments.Single(x => x.ConNoteNumberParent == con.ConNoteNo);

                var surcharges = m_surcharges.Where(x => x.Code == source.SurchargeCode).Select(x => x.SnbCode).ToArray();
                con.Surcharges.AddRange(surcharges);

                var services = m_surcharges.Where(x => x.Code == source.ValueAdded).Select(x => x.SnbCode).ToArray();
                con.ValueAddedServices.AddRange(services);


            }
            destination.Body.Consignments.AddRange(consignments);
            destination.Body.WebId = item.Id;
        }

    }
}
