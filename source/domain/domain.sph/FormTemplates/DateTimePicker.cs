using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "DateTimePicker", Order = 12d, FontAwesomeIcon = "clock-o",TypeName = "DateTimePicker", Description = "Creates an input for date and time entry")]
    public partial class DateTimePicker : FormElement
    {

        public  string GetKnockoutBindingExpression()
        {
            if (this.IsCompact)
                return string.Format("kendoDateTime: {0}, visible :{1}, enable :{2}",
                    this.Path.ConvertJavascriptObjectToFunction(),
                    this.Visible, this.Enable);
            return string.Format("kendoDateTime: {0}, enable :{1}", this.Path, this.Enable ?? "true");
        }

    }
}