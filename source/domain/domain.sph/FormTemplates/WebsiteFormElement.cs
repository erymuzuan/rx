﻿using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Url", TypeName = "WebsiteFormElement",Order = 5, FontAwesomeIcon = "link", Description = "Creates an input for web address")]
    public partial class WebsiteFormElement : FormElement
    {
        public override string GetKnockoutBindingExpression()
        {
            return $"value: {this.Path}, visible :{this.Visible}, enable: {this.Enable}";
        }
    }
}