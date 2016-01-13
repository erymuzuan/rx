using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "ChildView", Order = 20d, FontAwesomeIcon = "tablet",TypeName = "ChildView", Description = "Container for your PartialView")]
    public partial class ChildView : FormElement
    {
        public override bool IsPathIsRequired => false;
    }
}