using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Address",TypeName = "Address", Description = "")]
    public partial class AddressElement : FormElement
    {

        public override string GetDesignSurfaceElement()
        {
            return @"<span class=""error"">The address element is not implemented</span>";
        }
    }
}
