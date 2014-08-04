namespace Bespoke.Sph.Domain
{
    public partial class SourceFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return "item." + this.Field;
        }
    }
}