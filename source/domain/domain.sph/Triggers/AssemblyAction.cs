using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Custom assembly", Description = "Execute a method is a custom assembly", FontAwesomeIcon = "gear")]
    public partial class AssemblyAction : CustomAction
    {
        public override bool UseAsync
        {
            get { return true; }
        }
    }
}