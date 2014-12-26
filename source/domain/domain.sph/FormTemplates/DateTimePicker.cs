using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "DateTimePicker", Order = 12d, FontAwesomeIcon = "clock-o",TypeName = "DateTimePicker", Description = "Creates an input for date and time entry")]
    public partial class DateTimePicker : FormElement
    {


    }
}