﻿using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Single Line Text", TypeName = "TextBox", FontAwesomeIcon = "text-width", Order = 1d, Description = "Creates an input for single line text")]
    public partial class TextBox : FormElement
    {

        public override string GenerateMarkup()
        {
            var element = new StringBuilder();
            var binding = "value";
            if (this.FieldValidation.Mode == "Number")
                binding = "money";
            if (this.FieldValidation.Mode == "Currency")
                binding = "money";
            element.AppendFormat("<input type='text' data-bind='{2}:{0}' name='{1}'></input>", this.Path, this.Name, binding);
            return element.ToString();
        }

        public override string GetKnockoutBindingExpression()
        {
            if (string.IsNullOrWhiteSpace(this.Enable))
                this.Enable = "true";


            var path = this.Path.ConvertJavascriptObjectToFunction();

            var binding = "value";
            if (this.FieldValidation.Mode == "Number")
                binding = "money";


            if (!string.IsNullOrWhiteSpace(this.AutoCompletionEntity)
                && !string.IsNullOrWhiteSpace(this.AutoCompletionField)
                )
            {
                var query = string.IsNullOrWhiteSpace(this.AutoCompletionQuery)
                    ? "Id ne \\'0\\'"
                    : this.AutoCompletionQuery.Replace("'", "\\'");
                return
                    $"value: {path}, visible :{this.Visible}, autocomplete :{{ entity:'{this.AutoCompletionEntity}', field :'{this.AutoCompletionField}', query:'{query}' }}";
            }

            var unique = this.IsUniqueName ? ",uniqueName:true" : "";
            if (this.IsCompact)
                return $"{binding}: {path}, visible :{this.Visible}, enable :{this.Enable} {unique}";
            return $"{binding}: {path}, enable :{this.Enable} {unique}";
        }
    }
}