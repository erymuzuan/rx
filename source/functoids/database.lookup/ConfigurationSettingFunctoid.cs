using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof (Functoid))]
    [DesignerMetadata(Name = "ConfigurationSettingFunctoid", FontAwesomeIcon = "tasks", Category = FunctoidCategory.DATABASE)]
    public class ConfigurationSettingFunctoid : Functoid
    {
    }
}