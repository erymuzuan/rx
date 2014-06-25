using System.Text;

namespace Bespoke.Sph.Domain
{
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
                    ? this.AutoCompletionEntity + "Id gt 0"
                    : this.AutoCompletionQuery.Replace("'", "\\'");
                return string.Format("value: {0}, visible :{1}, autocomplete :{{ entity:'{2}', field :'{3}', query:'{4}' }}",
                    path,
                    this.Visible,
                    this.AutoCompletionEntity,
                    this.AutoCompletionField,
                    query);
            }

            var unique = this.IsUniqueName ? ",uniqueName:true" : "";
            if (this.IsCompact)
                return string.Format("{2}: {0}, visible :{1}, enable :{3} {4}", path, this.Visible, binding, this.Enable, unique);
            return string.Format("{1}: {0}, enable :{2} {3}", path, binding, this.Enable, unique);
        }
    }
}