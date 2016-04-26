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
    public class CustomDialogSolutionItemProviders : IItemsProvider
    {
        public Task<SolutionItem> GetItemAsync()
        {
            var dialogNode = new SolutionItem
            {
                id = "custom.dialogs",
                text = "Custom Dialogs",
                icon = "fa fa-files-o",
                createDialog = "custom.form.dialog.dialog"
            };
            var dialogConfig = $"{ConfigurationManager.WebPath}\\App_Data\\custom-dialog.json";
            if (File.Exists(dialogConfig))
            {
                var scripts = JArray.Parse(File.ReadAllText(dialogConfig))
                    .Select(a => Extensions.Value<string>(a.SelectToken("name")))
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-files-o",
                        text = $"{x}.js",
                        id = $"{x}.js",
                        codeEditor = $"/sphapp/viewmodels/{x}.js"
                    });

                var views = JArray.Parse(File.ReadAllText(dialogConfig))
                    .Select(a => a.SelectToken("name").Value<string>())
                    .Select(x => new SolutionItem
                    {
                        icon = "fa fa-file-code-o",
                        text = $"{x}.html",
                        id = $"{x}.html",
                        codeEditor = $"/sphapp/views/{x}.html"
                    });
                var dialogs = scripts.Concat(views).OrderBy(x => x.text);
                dialogNode.itemCollection.AddRange(dialogs);
            }
            return Task.FromResult(dialogNode);
        }

        public Task<IEnumerable<SolutionItem>> GetItemsAsync(SolutionItem parent)
        {
            return Task.FromResult(Array.Empty<SolutionItem>().AsEnumerable());
        }
        
    }

    
}