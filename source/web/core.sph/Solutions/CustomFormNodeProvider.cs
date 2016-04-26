using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class CustomFormNodeProvider : IItemsProvider
    {
        public Task<SolutionItem> GetItemAsync()
        {
            var node = new SolutionItem { id = "custom.forms", text = "Custom Forms", icon = "fa fa-edit" };
            var config = $"{ConfigurationManager.WebPath}\\App_Data\\routes.config.json";
            if (!File.Exists(config)) return Task.FromResult(node);
            var scripts = from r in JsonConvert.DeserializeObject<JsRoute[]>(File.ReadAllText(config))
                where !string.IsNullOrWhiteSpace(r.ModuleId)
                let name = r.ModuleId.Replace("viewmodels/", "")
                select new SolutionItem
                {
                    id = $"{r.ModuleId}.js",
                    text = $"{name}.js",
                    icon = "fa fa-file-text-o",
                    codeEditor = $"/sphapp/{r.ModuleId}.js"
                };
            var views = from r in JsonConvert.DeserializeObject<JsRoute[]>(File.ReadAllText(config))
                where !string.IsNullOrWhiteSpace(r.ModuleId)
                let name = r.ModuleId.Replace("viewmodels/", "")
                select new SolutionItem
                {
                    id = $"{r.ModuleId}.html",
                    text = $"{name}.html",
                    icon = "fa fa-file-code-o",
                    codeEditor = $"/sphapp/views/{name}.html"
                };
            var forms = views.Concat(scripts).OrderBy(x => x.text);
            node.itemCollection.AddRange(forms);
            return Task.FromResult(node);
        }

        public Task<IEnumerable<SolutionItem>> GetItemsAsync(SolutionItem parent)
        {
            return Task.FromResult(Array.Empty<SolutionItem>().AsEnumerable());
        }
    }
}