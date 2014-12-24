using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof(TextBox))]
    public class TextBoxCompiler : FormElementCompiler<TextBox>
    {
        public override string GenerateEditor(TextBox textBox)
        {
            return
                string.Format(
                    @"     <input {0}
    class=""{1} form-control  {2}"" 
    title=""{3}""
    data-bind=""{4}""
    id=""{5}"" 
    type=""text"" 
    name=""{6}"" />",
                    textBox.IsRequired ? "required" : string.Empty,
                    textBox.CssClass,
                    textBox.Size,
                    textBox.Tooltip,
                    this.GetKnockoutBindingExpression(textBox),
                    textBox.ElementId,
                    textBox.Path
                    );
        }


        private string GenerateMarkup(TextBox text)
        {
            var element = new StringBuilder();
            var binding = "value";
            if (text.FieldValidation.Mode == "Number")
                binding = "money";
            if (text.FieldValidation.Mode == "Currency")
                binding = "money";
            element.AppendFormat("<input type='text' data-bind='{2}:{0}' name='{1}'></input>", text.Path, text.Name, binding);
            return element.ToString();
        }

        private string GetKnockoutBindingExpression(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Enable))
                textBox.Enable = "true";


            var path = textBox.Path.ConvertJavascriptObjectToFunction();

            var binding = "value";
            if (textBox.FieldValidation.Mode == "Number")
                binding = "money";


            if (!string.IsNullOrWhiteSpace(textBox.AutoCompletionEntity)
                && !string.IsNullOrWhiteSpace(textBox.AutoCompletionField)
                )
            {
                var query = string.IsNullOrWhiteSpace(textBox.AutoCompletionQuery)
                    ? textBox.AutoCompletionEntity + "Id gt 0"
                    : textBox.AutoCompletionQuery.Replace("'", "\\'");
                return string.Format("value: {0}, visible :{1}, autocomplete :{{ entity:'{2}', field :'{3}', query:'{4}' }}",
                    path,
                    textBox.Visible,
                    textBox.AutoCompletionEntity,
                    textBox.AutoCompletionField,
                    query);
            }

            var unique = textBox.IsUniqueName ? ",uniqueName:true" : "";
            if (textBox.IsCompact)
                return string.Format("{2}: {0}, visible :{1}, enable :{3} {4}", path, textBox.Visible, binding, textBox.Enable, unique);
            return string.Format("{1}: {0}, enable :{2} {3}", path, binding, textBox.Enable, unique);
        }
    }
}