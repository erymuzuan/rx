namespace Bespoke.Sph.Domain
{
    public partial class Space : Entity
    {
        public override string ToString()
        {
            return string.Format("{0} {1}", this.RegistrationNo, this.LotName);
        }

        public int[] ApplicationTemplateOptions { get; set; }
    }
   
}
