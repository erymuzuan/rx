using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Parse decimal", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class DecimalFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return string.Format("decimal.Parse(item.{0}, System.Globalization.NumberStyles.{1})", this.SourceField, this.NumberStyles);
        }
    }
}