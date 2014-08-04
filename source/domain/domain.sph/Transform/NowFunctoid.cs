using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof (Functoid))]
    [FunctoidDesignerMetadata(Name = "Now", BootstrapIcon = "clock-o", Category = FunctoidCategory.Date)]
    public class NowFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return "DateTime.Now";
        }
    }
}