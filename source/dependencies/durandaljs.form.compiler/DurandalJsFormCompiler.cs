using System.ComponentModel.Composition;
using System.IO;
using System.Net.Http;
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

            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + item.Route.ToLower() + ".html");
            using (var client = new HttpClient())
            {
                var uri = ConfigurationManager.BaseUrl + "/Sph/EntityFormRenderer/Html/" + item.Route;
                var markup = await client.GetStringAsync(uri);
                File.WriteAllText(html, markup);
            }


            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + item.Route.ToLower() + ".js");
            using (var client = new HttpClient())
            {
                var script = await client.GetStringAsync(ConfigurationManager.BaseUrl + "/Sph/EntityFormRenderer/Js/" + item.Route);
                File.WriteAllText(js, script);
            }

            var result = new WorkflowCompilerResult
            {
                Result = true,
                Output = js
            };
            return result;
        }
    }
}
