using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Select List", TypeName = "ComboBox", Order = 2d, FontAwesomeIcon = "chevron-down", Description = "Creates an input with list of options")]
    public partial class ComboBox : FormElement
    {


    }
}