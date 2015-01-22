using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Paragrapah text", Order = 8d, TypeName = "TextAreaElement", FontAwesomeIcon = "desktop", Description = "Creates a munltiline text input")]
    public partial class TextAreaElement : FormElement
    {


    }
}

