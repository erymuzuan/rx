using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Single Line Text", TypeName = "TextBox", FontAwesomeIcon = "text-width",Order = 1d, Description = "Creates an input for single line text")]
    public partial class TextBox : FormElement
    {
    }
}