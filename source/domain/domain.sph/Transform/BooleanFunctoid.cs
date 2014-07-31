namespace Bespoke.Sph.Domain
{
    public partial class BooleanFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return string.Format("bool.Parse(item.{0})", this.SourceField);
        }
    }
}