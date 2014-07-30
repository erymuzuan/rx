namespace Bespoke.Sph.Domain
{
    public partial class FormattingFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return "string.Format(\"" + this.Format +"\", item."+ this.SourceField +")";
        }
    }
}