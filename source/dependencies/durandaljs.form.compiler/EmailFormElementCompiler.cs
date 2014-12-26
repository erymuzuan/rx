using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof(EmailFormElement))]
    public class EmailFormElementCompiler : DurandalJsElementCompiler<EmailFormElement>
    {

        public string GetKnockoutBindingExpression()
        {
            var path = this.Element.Path.ConvertJavascriptObjectToFunction();

            return string.Format("value: {0}, visible :{1}, enable: {2}",
                path,
                this.Element.Visible,
                this.Element.Enable ?? "true");
        }

        protected override string EditorRazorTemplate
        {
            get
            {
                var razor = Properties.Resources.ComboBox;
                return System.Text.Encoding.UTF8.GetString(razor);
            }
        }

    }
}