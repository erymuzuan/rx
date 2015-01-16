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
        [ImportMany("ViewModelRenderer", typeof(FormRenderer), AllowRecomposition = true)]
        public Lazy<FormRenderer, IFormRendererMetadata>[] HtmlRenderers { get; set; }

        [ImportMany("ViewRenderer", typeof(FormRenderer), AllowRecomposition = true)]
        public Lazy<FormRenderer, IFormRendererMetadata>[] JavascriptRenderers { get; set; }

        public override async Task<WorkflowCompilerResult> CompileAsync(IForm form)
        {
            var project = await form.LoadProjectAsync();
            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + form.Route.ToLower() + ".html");
            var vc = this.HtmlRenderers.SingleOrDefault(x => x.Metadata.FormType == form.GetType()); 
            if (null != vc)
            {
                var markup = await vc.Value.GenerateCodeAsync(form, project);
                File.WriteAllText(html, markup);
            }
            else
            {
                Warn("Cannot find view compiler for {0}", form.GetType().FullName);
            }

            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + form.Route.ToLower() + ".js");
            var vmc = this.HtmlRenderers.SingleOrDefault(x => x.Metadata.FormType == form.GetType());
            if (null != vmc)
            {
                var script = await vmc.Value.GenerateCodeAsync(form, project);
                File.WriteAllText(js, script);
            }
            else
            {
                Warn("Cannot find view model compiler for {0}", form.GetType().FullName);
            }

            var result = new WorkflowCompilerResult
            {
                Result = null != vmc && null != vc,
                Outputs = new[] { html, js }
            };
            return result;
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
