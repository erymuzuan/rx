using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
   public partial class Rent : Entity
    {

       public decimal? Accrued
       {
           get
           {
               var accrued = this.Amount - this.PaymentDistributionCollection.Sum(a => a.Amount);
               return accrued;
           }
       }
    }
}
