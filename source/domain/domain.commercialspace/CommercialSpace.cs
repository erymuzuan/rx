using System.Xml.Serialization;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class CommercialSpace : Entity
    {
     [XmlAttribute]
       private int m_commercialSpaceId;

       public int CommercialSpaceId
       {
           get { return m_commercialSpaceId; }
           set
           {
               m_commercialSpaceId = value;
               RaisePropertyChanged();
           }
       }
    }
   
}
