namespace Bespoke.Sph.Domain
{
    public partial class DecimalFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return string.Format("decimal.Parse(item.{0}, System.Globalization.NumberStyles.{1})", this.SourceField, this.NumberStyles);
        }
    }
}