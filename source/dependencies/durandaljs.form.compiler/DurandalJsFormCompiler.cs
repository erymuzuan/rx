using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS)]
    public class DurandalJsFormCompiler : FormCompiler
    {
        public override async Task<WorkflowCompilerResult> CompileAsync(IForm item)
        {
            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            var project = await item.LoadProjectAsync();
            var vm = new FormRendererViewModel { Form = item, Project = project };

            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + item.Route.ToLower() + ".html");
            var markup = await razor.GenerateAsync(Encoding.Default.GetString(Properties.Resources.FormViewModels), vm);
            File.WriteAllText(html, markup);

            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + item.Route.ToLower() + ".js");
            var script = await razor.GenerateAsync(Encoding.Default.GetString(Properties.Resources.FormViewModels), vm);
            File.WriteAllText(js, script);

            var result = new WorkflowCompilerResult
            {
                Result = true,
                Outputs = new[] { html, js }
            };
            return result;
        }
    }
}
