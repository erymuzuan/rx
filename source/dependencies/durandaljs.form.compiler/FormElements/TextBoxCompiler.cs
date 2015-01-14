using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormElements
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(TextBox))]
    public class TextBoxCompiler : DurandalJsElementCompiler<TextBox>
    {

        public string GetKnockoutBindingExpression()
        {
            var textBox = this.Element;
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

            return string.Format("{2}: {0}, visible :{1}, enable :{3} {4}", path, textBox.Visible, binding, textBox.Enable, unique);

        }
    }
}