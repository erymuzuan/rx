using System;

namespace Bespoke.Sph.Domain
{
    public partial class DatePicker : FormElement
    {


        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;
            return string.Format("kendoDate: {0}, visible :{1}",
                path,
                this.Visible);
        }

    }
}