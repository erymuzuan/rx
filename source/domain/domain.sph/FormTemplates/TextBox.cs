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
            element.AppendFormat("<input type='text' data-bind='{2}:{0}' name='{1}'></input>", this.Path, this.Name, binding);
            return element.ToString();
        }

        public override string GetKnockoutBindingExpression()
        {

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

            return string.Format("{2}: {0}, visible :{1}",path,this.Visible, binding);
        }
    }
}