﻿namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class TextBox : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            var cf = new CustomField
            {
                Name = this.Path,
                Type = typeof(string).Name
            };
            this.CustomField = cf;
            return cf;
        }
    }
}