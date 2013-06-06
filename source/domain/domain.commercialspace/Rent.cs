using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
   public partial class Rent : Entity
    {
       [XmlAttribute]
       public decimal? Accrued
       {
           get
           {
               var accrued = this.Amount - this.PaymentDistributionCollection.Sum(a => a.Amount);
               return accrued;
           }
       }

       [XmlAttribute]
       public decimal? AccumulatedAccrued
       {
           get
           {
               var aaccrued = this.Amount - this.PaymentDistributionCollection.Sum(a => a.Amount);
               return aaccrued;
           }
       }
    }
}
