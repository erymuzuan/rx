namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class CommercialSpace : Entity
    {
        public override string ToString()
        {
            return string.Format("{0} {1}", this.RegistrationNo, this.LotName);
        }

        public int[] ApplicationTemplateOptions { get; set; }
    }
   
}
