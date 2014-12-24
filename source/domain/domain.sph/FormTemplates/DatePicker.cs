
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "DatePicker",TypeName = "DatePicker", Order = 10d, FontAwesomeIcon = "calendar", Description = "Creates an input for date entry")]
    public partial class DatePicker : FormElement
    {
        public  string GetKnockoutBindingExpression()
        {
            if (this.IsCompact)
                return string.Format("kendoDate: {0}, visible :{1}, enable :{2}",
                    this.Path,
                    this.Visible, this.Enable);
            return string.Format("kendoDate: {0}, enable :{1}", this.Path, this.Enable);
        }

    }
}