using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
   public partial class Building : Entity
    {
       [XmlAttribute]
       private int m_buildingId;

       public int BuildingId
       {
           get { return m_buildingId; }
           set
           {
               m_buildingId = value;
               RaisePropertyChanged();
           }
       }
    }
}
