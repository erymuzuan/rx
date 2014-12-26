
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "DatePicker",TypeName = "DatePicker", Order = 10d, FontAwesomeIcon = "calendar", Description = "Creates an input for date entry")]
    public partial class DatePicker : FormElement
    {
        

    }
}