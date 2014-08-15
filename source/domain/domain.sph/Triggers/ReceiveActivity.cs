using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Receive", Description = "Wait for a message to be delivered")]
    public partial class ReceiveActivity : Activity { }
}