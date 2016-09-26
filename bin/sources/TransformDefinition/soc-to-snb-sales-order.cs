using System.Linq;

namespace Bespoke.DevV1.Integrations.Transforms
{
    public partial class SocToSnbSalesOrder
    {


        partial void BeforeTransform(Bespoke.DevV1.SalesOrders.Domain.SalesOrder item, Bespoke.DevV1.Adapters.SnbWebNewAccount.PostSalesOrdersRequest destination)
        {

        }

        partial void AfterTransform(Bespoke.DevV1.SalesOrders.Domain.SalesOrder item, Bespoke.DevV1.Adapters.SnbWebNewAccount.PostSalesOrdersRequest destination)
        {

            // write some some code since LoopingFunctoid has some designers issue
            var consignments = from c in item.Consignments
                            select new Bespoke.DevV1.Adapters.SnbWebNewAccount.ConsignmentsItem
                            {
                                ConNoteNo = c.ConNoteNumberParent,
                                ActualWeight = c.Weight,
                                ReceiverPostCode = c.ShiptoPartyPostcode
                            };
            destination.Body.Consignments.AddRange(consignments);
        }

    }
}
