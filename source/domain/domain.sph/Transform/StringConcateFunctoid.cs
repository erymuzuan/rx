using System.ComponentModel.Composition;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "String concatenation", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class StringConcateFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            var codes = this.ArgumentCollection.Select(a => a.Functoid.GenerateCode());
            return string.Join(" + ", codes);
        }
    }
}