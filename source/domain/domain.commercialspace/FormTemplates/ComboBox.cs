﻿using System;
using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ComboBox : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return new CustomField
            {
                IsRequired = this.IsRequired,
                Name = this.Path,
                Listing =string.Join(",", this.ComboBoxItemCollection.Select(c => c.Value).ToArray())
            };
        }

        
        public override string GetKnockoutBindingExpression()
        {
            return string.Format("value: {0}, visible :{1}",
                this.Path,
                this.Visible);
        }
    }
}