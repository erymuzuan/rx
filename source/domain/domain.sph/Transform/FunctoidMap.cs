namespace Bespoke.Sph.Domain
{
    public partial class FunctoidMap : Map
    {
        public Functoid Functoid { get; set; }
        
        public override string GenerateCode()
        {
            return this.Functoid.GeneratePreCode(this)
                + "\r\n" +
                string.Format("                dest.{1} = {0};", this.Functoid.GenerateCode(), this.Destination);

        }
    }
}