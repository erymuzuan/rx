using System;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class DatePicker : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return new CustomField
                 {
                     Name = this.Path,
                     Type = typeof(DateTime).Name,
                     IsRequired = this.IsRequired
                 };
        }


        public override string GetKnockoutBindingExpression()
        {
            return string.Format("kendoDate: {0}, visible :{1}",
                this.Path,
                this.Visible);
        }

    }
}