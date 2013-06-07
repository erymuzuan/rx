using System.Collections.Generic;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface ILedgerExport
    {
        string GenerateLedger(Contract contract, IEnumerable<Rent> rents, string filename);
    }
}