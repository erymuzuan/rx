using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS)]
    public class DurandalJsViewCompiler : ViewCompiler
    {
        [Import]
        public EntityViewHtmlRenderer HtmlRenderer { get; set; }
        [Import]
        public EntityViewJsRenderer JsRenderer { get; set; }
        [Import]
        public EntityViewPartialRenderer PartialRenderer { get; set; }



        public override async Task<SphCompilerResult> CompileAsync(EntityView view)
        {
            var context = new SphDataContext();
            var project = await context.LoadOneAsync<EntityDefinition>(x => x.Id == view.EntityDefinitionId);
            var html = await CompileHtmlView(view, project);
            var js = await CompileViewModel(view, project);
            var partial = await CompilePartial(view, project);

            var result = new SphCompilerResult
            {
                Result = !string.IsNullOrWhiteSpace(js) && !string.IsNullOrWhiteSpace(html),
                Outputs = new[] { html, js, partial }
            };
            return result;
        }

        private async Task<string> CompileHtmlView(EntityView form, IProjectProvider project)
        {
            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + form.Route.ToLower() + ".html");
            var vc = this.HtmlRenderer;
            if (null != vc)
            {
                var markup = await vc.GenerateCodeAsync(form, project);
                File.WriteAllText(html, markup, Encoding.ASCII);
            }
            else
            {
                Warn("Cannot find view compiler for {0}", form.GetType().FullName);
            }
            return html;
        }

        private async Task<string> CompileViewModel(EntityView view, IProjectProvider project)
        {
            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + view.Route.ToLower() + ".js");
            var vmc = this.JsRenderer;
            if (null != vmc)
            {
                var script = await vmc.GenerateCodeAsync(view, project);
                File.WriteAllText(js, script);
            }
            else
            {
                Warn("Cannot find view model compiler for {0}", view.GetType().FullName);
            }
            return js;
        }

        private async Task<string> CompilePartial(EntityView view, IProjectProvider project)
        {
            var partial = Path.Combine(ConfigurationManager.WebPath, "SphApp/partial/" + view.Route.ToLower() + ".js");
            var partialCompiler = this.PartialRenderer;
            if (null != partialCompiler)
            {
                var script = await partialCompiler.GenerateCodeAsync(view, project);
                File.WriteAllText(partial, script);
            }
            else
            {
                Warn("Cannot find view model compiler for {0}", view.GetType().FullName);
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