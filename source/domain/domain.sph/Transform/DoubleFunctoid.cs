namespace Bespoke.Sph.Domain
{
    public partial class DoubleFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return string.Format("double.Parse(item.{0}, System.Globalization.NumberStyles.{1})", this.SourceField, this.NumberStyles);
        }
    }
}