using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class Offer : DomainObject
    {
        public decimal DepositPaid
        {
            get
            {
                var sum = this.DepositPaymentCollection.Sum(d => d.Amount);
                return sum;
            }
        }
    }
}