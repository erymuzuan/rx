using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            var resource = Properties.Resources.ScreenActivityFormTemplate;
            // TODO : remove the BOM marker
            var start = resource.IndexOf('@');
            var template = resource.Substring(start, resource.Length - start);
            var wd = (WorkflowDefinition)project;
            wd.ReferencedAssemblyCollection.Select(x => Path.GetFileName(x.Location))
                .Select(x => Path.Combine(ConfigurationManager.WebPath, @"bin\" + x))
                .ToList()
                .ForEach(x =>
                {
                    Assembly.LoadFile(x);
                    Console.WriteLine("Loading " + x);
                });

            var mscorlib = Path.Combine(ConfigurationManager.WebPath, @"bin\System.Runtime.dll");
            if (!File.Exists(mscorlib))
                mscorlib = (typeof(object).Assembly.Location);
            Assembly.LoadFile(mscorlib);
            Console.WriteLine(mscorlib);
            Assembly.Load(typeof(System.Net.Mail.SmtpClient).Assembly.GetName());

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
        public ScreenActivityFormHtmlViewRenderer([Import(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]FormCompiler compiler)
        {
            this.Compiler = compiler;
        }
    }
}