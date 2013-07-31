using System;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class DatePicker : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            var cf = new CustomField
                {
                    Name = this.Path,
                    Type = typeof(DateTime).Name
                };
            this.CustomField = cf;
            return cf;
        }

    }
}