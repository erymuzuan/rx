using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Single Line Text", IsEnabled = true, TypeName = "TextBox", FontAwesomeIcon = "text-width", Order = 1d, Description = "Creates na input for single line text")]
    public partial class TextBox : FormElement
    {
    }
}