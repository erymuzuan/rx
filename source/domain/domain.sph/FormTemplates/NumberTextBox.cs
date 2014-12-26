using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "NumberTextBox", Order = 4d, FontAwesomeIcon = "xing", TypeName = "NumberTextBox", Description = "Creates an input for numeric value")]
    public partial class NumberTextBox : FormElement
    {
       


    }
}