using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.LedgerMsxl
{
    public class JournalEntry
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal Balance { get; set; }
        public Entity   Transaction { get; set; }
    }
}