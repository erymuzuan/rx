using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class PartialViewNodeProvider : IItemsProvider
    {
        public Task<SolutionItem> GetItemAsync()
        {

            var node = new SolutionItem
            {
                id = "partial.views",
                text = "Partial Views",
                icon = "fa fa-file-o",
                createDialog = "custom.form.dialog.dialog",
                createdUrl = ""
            };
            var config = $"{ConfigurationManager.WebPath}\\App_Data\\custom-partial-view.json";
            if (File.Exists(config))
            {
                var scripts = JArray.Parse(File.ReadAllText(config))
                    .Select(a => Extensions.Value<string>(a.SelectToken("name")))
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-file-code-o",
                        text = x.ToString(),
                        id = x.ToString(),
                        codeEditor = $"/sphapp/views/{x}.html"
                    });
                node.itemCollection.AddRange(scripts);
            }
            return Task.FromResult(node);
        }

        public Task<IEnumerable<SolutionItem>> GetItemsAsync(SolutionItem parent)
        {
            return Task.FromResult(Array.Empty<SolutionItem>().AsEnumerable());
        }
    }
}