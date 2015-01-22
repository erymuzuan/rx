using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "EmailFormElement",Order = 6d, FontAwesomeIcon = "envelope",TypeName = "EmailFormElement", Description = "Creates an input for email address")]
    public partial class EmailFormElement : FormElement
    {
    }
}