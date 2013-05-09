namespace Bespoke.CommercialSpace.Domain
{
   public partial class Building : Entity
    {

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
