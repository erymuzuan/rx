using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs.FormRenderers;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS)]
    public class DurandalJsFormCompiler : FormCompiler
    {
        [ImportMany("ViewRenderer", typeof(FormRenderer), AllowRecomposition = true)]
        public Lazy<FormRenderer, IFormRendererMetadata>[] HtmlRenderers { get; set; }

        [ImportMany("ViewModelRenderer", typeof(FormRenderer), AllowRecomposition = true)]
        public Lazy<FormRenderer, IFormRendererMetadata>[] JavascriptRenderers { get; set; }

        [ImportMany("PartialRenderer", typeof(FormRenderer), AllowRecomposition = true)]
        public Lazy<FormRenderer, IFormRendererMetadata>[] PartialRenderers { get; set; }

        public override async Task<WorkflowCompilerResult> CompileAsync(IForm form)
        {
            var project = await form.LoadProjectAsync();
            var html = await CompileHtmlView(form, project);
            var js = await CompileViewModel(form, project);
            var partial = await CompilePartial(form, project);

            var result = new WorkflowCompilerResult
            {
                Result = !string.IsNullOrWhiteSpace(js) && !string.IsNullOrWhiteSpace(html),
                Outputs = new[] { html, js, partial }
            };
            return result;
        }

        private async Task<string> CompileHtmlView(IForm form, IProjectProvider project)
        {
            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + form.Route.ToLower() + ".html");
            var vc = this.HtmlRenderers.SingleOrDefault(x => x.Metadata.FormType == form.GetType());
            if (null != vc)
            {
                var markup = await vc.Value.GenerateCodeAsync(form, project);
                File.WriteAllText(html, markup, System.Text.Encoding.ASCII);
            }
            else
            {
                Warn("Cannot find view compiler for {0}", form.GetType().FullName);
            }
            return html;
        }

        private async Task<string> CompileViewModel(IForm form, IProjectProvider project)
        {
            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + form.Route.ToLower() + ".js");
            var vmc = this.JavascriptRenderers.SingleOrDefault(x => x.Metadata.FormType == form.GetType());
            if (null != vmc)
            {
                var script = await vmc.Value.GenerateCodeAsync(form, project);
                File.WriteAllText(js, script);
            }
            else
            {
                Warn("Cannot find view model compiler for {0}", form.GetType().FullName);
            }
            return js;
        }

        private async Task<string> CompilePartial(IForm form, IProjectProvider project)
        {
            var partial = Path.Combine(ConfigurationManager.WebPath, "SphApp/partial/" + form.Route.ToLower() + ".js");
            var partialCompiler = this.PartialRenderers.SingleOrDefault(x => x.Metadata.FormType == form.GetType());
            if (null != partialCompiler)
            {
                var script = await partialCompiler.Value.GenerateCodeAsync(form, project);
                File.WriteAllText(partial, script);
            }
            else
            {
                Warn("Cannot find view model compiler for {0}", form.GetType().FullName);
            }
            return partial;
        }

        private static void Warn(string format, params object[] arguments)
        {
            var color = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(format, arguments);
            }
            finally
            {
                Console.ForegroundColor = color;
            }
        }
    }
}
