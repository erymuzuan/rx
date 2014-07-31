using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Fomatting object", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class FormattingFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return "string.Format(\"" + this.Format +"\", item."+ this.SourceField +")";
        }
    }
}