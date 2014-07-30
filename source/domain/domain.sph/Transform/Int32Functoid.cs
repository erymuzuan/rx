namespace Bespoke.Sph.Domain
{
    public partial class Int32Functoid : Functoid
    {
        public override string GenerateCode()
        {
            return string.Format("int.Parse(item.{0}, System.Globalization.NumberStyles.{1})", this.SourceField, this.NumberStyles);
        }
    }
}