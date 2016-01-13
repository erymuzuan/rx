using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "TabControl", Order = 20d, FontAwesomeIcon = "folder-o",TypeName = "TabControl", Description = "Creates tab control")]
    public partial class TabControl : FormElement
    {
        public override bool IsPathIsRequired => false;
    }
}