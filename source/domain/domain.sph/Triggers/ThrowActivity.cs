using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Throw", Description = "Throw an exception")]
    public partial class ThrowActivity : Activity { }
}