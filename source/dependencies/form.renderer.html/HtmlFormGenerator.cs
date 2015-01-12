using System.ComponentModel.Composition;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace form.renderer.html
{
    [Export("FormRenderer", typeof(IFormRenderer))]
    [FormMetadata(Name = "Html")]
    public class HtmlFormGenerator : IFormRenderer
    {
        public async Task<BuildValidationResult> RenderAsync(EntityForm item)
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

            return new BuildValidationResult();

        }
    }
}
