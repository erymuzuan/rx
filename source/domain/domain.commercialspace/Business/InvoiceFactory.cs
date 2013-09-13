using System;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class InvoiceFactory
    {
        public async Task<Invoice> CreateRentalInvoice(Contract contract)
        {
            var date = DateTime.Now;
            var c1 = contract;
            var rent = new Rent 
             {
                 Date = date,
                 Month = date.Month,
                 Year = date.Year,
                 Amount = c1.CommercialSpace.RentalRate,
                 TenantIdSsmNo = c1.Tenant.IdSsmNo,
                 ContractNo = c1.ReferenceNo,
                 InvoiceNo = string.Format("{0}/{1:MMyyyy}", c1.ReferenceNo, date),
                 Type = InvoiceType.Rental,
                 Tenant = c1.Tenant
             };

            return await Task.FromResult(rent);
        }
    }
}
