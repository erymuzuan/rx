namespace Bespoke.CommercialSpace.Domain
{
    public partial class CommercialSpace : Entity
    {

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
