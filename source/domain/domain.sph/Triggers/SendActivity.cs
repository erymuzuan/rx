using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Send", TypeName = "Send", Description = "Send a message to another system")]
    public partial class SendActivity : Activity{}
}