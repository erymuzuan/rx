using System.Linq;
using Bespoke.Sph.Domain;
using System.Collections.Generic;

namespace Bespoke.DevV1.Integrations.Transforms
{
    public partial class SocToSnbSalesOrder
    {

        private static List<Bespoke.DevV1.Products.Domain.Product> m_products = new List<Bespoke.DevV1.Products.Domain.Product>();
        private static List<Bespoke.DevV1.SurchargeAddOns.Domain.SurchargeAddOn> m_surcharges = new List<Bespoke.DevV1.SurchargeAddOns.Domain.SurchargeAddOn>();
        private static List<Bespoke.DevV1.ItemCategories.Domain.ItemCategory> m_categories = new List<Bespoke.DevV1.ItemCategories.Domain.ItemCategory>();

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
            if (m_categories.Count == 0)
            {
                var categoryQuery = context.CreateQueryable<Bespoke.DevV1.ItemCategories.Domain.ItemCategory>()
                                .Where(p => p.Id != "0");
                var categoryRepos = ObjectBuilder.GetObject<IRepository<Bespoke.DevV1.ItemCategories.Domain.ItemCategory>>();
                var categoriesLo = categoryRepos.LoadAsync(categoryQuery, 1, 200, false).Result;
                m_categories.AddRange(categoriesLo.ItemCollection);
            }
        }

        partial void AfterTransform(Bespoke.DevV1.SalesOrders.Domain.SalesOrder item, Bespoke.DevV1.Adapters.SnbWebNewAccount.PostSalesOrdersRequest destination)
        {

            foreach (var con in destination.Body.Consignments)
            {
                var source = item.Consignments.Single(x => x.ConNoteNumberParent == con.ConNoteNo);
                
                var cat = m_categories.Where(x => x.Code == source.ItemCategoryType).LastOrDefault();
                if(null != cat)
                    con.ItemCategory = cat.Name;

                var surcharges = m_surcharges.Where(x => x.Code == source.SurchargeCode).Select(x => x.SnbCode).ToArray();
                con.Surcharges.AddRange(surcharges);

                var services = m_surcharges.Where(x => x.Code == source.ValueAdded).Select(x => x.SnbCode).ToArray();
                con.ValueAddedServices.AddRange(services);
                
                
                con.ProductCode = m_products.Where(x => x.SocCode == source.ProductCodeMaterial).Select(x => x.Code).LastOrDefault() ?? "-";
                
                // split the dimensions
                if(!string.IsNullOrWhiteSpace(source.VolumetricDimension))
                {
                    var dimensions = source.VolumetricDimension.Split(new []{"X"}, System.StringSplitOptions.RemoveEmptyEntries);
                    if(dimensions.Length == 3)
                    {
                        con.Length = decimal.Parse(dimensions[0]);
                        con.Height = decimal.Parse(dimensions[1]);
                    }
                }
                
                //con.SenderPostcode
            }
            
            
            destination.Body.WebId = item.Id;
        }

    }
}
