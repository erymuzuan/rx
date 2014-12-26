using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Image",Order = 18d, FontAwesomeIcon = "picture-o",TypeName = "ImageElement", Description = "Creates an image element to display a photo for the given StoreId")]
    public partial class ImageElement : FormElement
    {
    }
}