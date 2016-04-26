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
    public class CustomScriptNodeProvider : IItemsProvider
    {
        public Task<SolutionItem> GetItemAsync()
        {

            var node = new SolutionItem { id = "custom.scrpts", text = "Custom Scripts", icon = "fa fa-file-text-o" };
            var  scriptConfig = $"{ConfigurationManager.WebPath}\\App_Data\\custom-script.json";
            if (File.Exists(scriptConfig))
            {
                var scripts = JArray.Parse(File.ReadAllText(scriptConfig))
                    .Select(a => Extensions.Value<string>(a.SelectToken("name")))
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-file-text-o",
                        text = x.ToString(),
                        id = x.ToString(),
                        codeEditor = $"/sphapp/services/{x}.js"
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