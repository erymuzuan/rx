using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Parse double", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class DoubleFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return string.Format("double.Parse(item.{0}, System.Globalization.NumberStyles.{1})", this.SourceField, this.NumberStyles);
        }
    }
}