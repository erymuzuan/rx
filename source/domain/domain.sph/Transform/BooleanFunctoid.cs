using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Parse boolean", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class BooleanFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return string.Format("bool.Parse(item.{0})", this.SourceField);
        }
    }
}