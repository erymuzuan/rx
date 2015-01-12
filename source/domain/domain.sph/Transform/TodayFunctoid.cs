using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof (Functoid))]
    [DesignerMetadata(Name = "Today", BootstrapIcon = "calendar", Category = FunctoidCategory.DATE)]
    public class TodayFunctoid : Functoid
    {
        public override string GenerateAssignmentCode()
        {
            return "DateTime.Today";
        }
    }
}