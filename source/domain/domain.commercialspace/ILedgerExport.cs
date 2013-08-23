using System.Collections.Generic;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface ILedgerExport
    {
        string GenerateLedger(Contract contract, IEnumerable<Invoice> invoices,  
            IEnumerable<Rebate> rebates, 
            IEnumerable<Payment> payments, string filename);
    }
}