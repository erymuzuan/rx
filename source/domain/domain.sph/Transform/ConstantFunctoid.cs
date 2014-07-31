using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Constant", FontAwesomeIcon = "sort-numeric-asc")]
    public class ConstantFunctoid : Functoid
    {
         
    }
}