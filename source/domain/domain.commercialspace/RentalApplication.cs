using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
   public partial class RentalApplication : Entity
    {
       [XmlAttribute]
       private int m_rentalApplicationId;

       public int RentalApplicationId
       {
           get { return m_rentalApplicationId; }
           set
           {
               m_rentalApplicationId = value;
               RaisePropertyChanged();
           }
       }
    }
}
