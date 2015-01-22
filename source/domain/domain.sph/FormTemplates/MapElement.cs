using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "MapElement",TypeName = "MapElement", Description = "")]
    public partial class MapElement : FormElement
    {
        public override string GetDesignSurfaceElement()
        {
            return @"<span class=""error"">The map element is not implemented</span>";
        }
    }
}