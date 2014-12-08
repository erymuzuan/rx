using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof (Activity))]
    [DesignerMetadata(Name = "Receive", TypeName = "Receive", Description = "Wait for a message to be delivered")]
    public partial class ReceiveActivity : Activity
    {
        public override bool IsAsync
        {
            get { return true; }
        }
    }
}