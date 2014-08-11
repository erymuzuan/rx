using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Parse double", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class DoubleFunctoid : Functoid
    {
        public override string GenerateAssignmentCode()
        {
            return string.Format("double.Parse(item.{0}, System.Globalization.NumberStyles.{1})", this.SourceField, this.NumberStyles);
        }
    }
}