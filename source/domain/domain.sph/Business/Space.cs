namespace Bespoke.Sph.Domain
{
    public partial class Space : Entity
    {
        
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.RegistrationNo, this.LotName,this.State);
        }

        public string SearchKeywords
        {
            get { return string.Format("{0} {1} {2}", this.RegistrationNo, this.LotName, this.State); }
        }

        public int[] ApplicationTemplateOptions { get; set; }
    }
   
}
