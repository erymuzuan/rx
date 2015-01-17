using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
namespace Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers
{
    [Export("ViewRenderer", typeof(FormRenderer))]
    [FormRendererMetadata(FormType = typeof(EntityForm))]
    public class EntityFormHtmlViewRenderer : FormRenderer
    {
        public async override Task<string> GenerateCodeAsync(IForm form, IProjectProvider project)
        {
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            var resource = Encoding.Default.GetString(Properties.Resources.Html2ColsWithAuditTrail);
            // TODO : remove the BOM marker
            var start = resource.IndexOf('@');
            var template = resource.Substring(start, resource.Length - start);

            Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "System.Runtime.dll"));

            var vm = new FormRendererViewModel
            {
                Project = project,
                Form = form,
                Compiler = this.Compiler
            };
            var html = await razor.GenerateAsync(template, vm);
            return html;

        }

        public FormCompiler Compiler { get; private set; }

        [ImportingConstructor]
        public EntityFormHtmlViewRenderer([Import(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]FormCompiler compiler)
        {
            this.Compiler = compiler;
        }
    }
}