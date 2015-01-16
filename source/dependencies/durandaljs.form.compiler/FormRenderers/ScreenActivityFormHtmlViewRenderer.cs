using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    [Export("ViewRenderer", typeof(FormRenderer))]
    [FormRendererMetadata(FormType = typeof(ScreenActivityForm))]
    public class ScreenActivityFormHtmlViewRenderer : FormRenderer
    {
        public async override Task<string> GenerateCodeAsync(IForm form, IProjectProvider project)
        {
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            var template = Encoding.Default.GetString(Properties.Resources.ScreenActivityFormTemplate);
            var vm = new FormRendererViewModel
            {
                Project = project,
                Form = form,
                Compiler = this.Compiler
            };
            var html = await razor.GenerateAsync(template, vm);
            // TODO : remove the BOM marker
            var start = html.IndexOf('<');
            return html.Substring(start, html.Length - start);

        }

        public FormCompiler Compiler { get; private set; }

        [ImportingConstructor]
        public ScreenActivityFormHtmlViewRenderer([Import(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]FormCompiler compiler)
        {
            this.Compiler = compiler;
        }
    }
}