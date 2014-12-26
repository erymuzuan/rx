using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export(Strings.FORM_ELEMENT_CONTRACT, typeof(FormElement))]
    [DesignerMetadata(Name = "Single Line Text", TypeName = "TextBox", FontAwesomeIcon = "text-width", Order = 1d, Description = "Creates na input for single line text")]
    public partial class TextBox : FormElement
    {
    }
}