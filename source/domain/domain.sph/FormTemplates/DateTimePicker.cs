﻿namespace Bespoke.Sph.Domain
{
    public partial class DateTimePicker : FormElement
    {

        public override string GetKnockoutBindingExpression()
        {
            if (this.IsCompact)
                return string.Format("kendoDateTime: {0}, visible :{1}, enable :{2}",
                    this.Path.ConvertJavascriptObjectToFunction(),
                    this.Visible, this.Enable);
            return string.Format("kendoDateTime: {0}, enable :{1}", this.Path, this.Enable ?? "true");
        }

    }
}