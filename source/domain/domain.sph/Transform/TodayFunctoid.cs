using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof (Functoid))]
    [FunctoidDesignerMetadata(Name = "Today", BootstrapIcon = "calendar")]
    public class TodayFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            return "DateTime.Today";
        }
    }
}