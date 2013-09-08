using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
   public partial class Deposit : Entity
    {
        [XmlIgnore]
        public decimal DepositPaid
        {
            get
            {
                var sum = this.DepositPaymentCollection.Sum(d => d.Amount);
                return sum;
            }
        }

        [XmlIgnore]
        public decimal DepositBalance
        {
            get
            {
                var balance = this.Amount - this.DepositPaid;
                return balance;
            }
        }
    }
}
