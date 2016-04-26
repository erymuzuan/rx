using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class EntityDefinitionNodesProvider : IItemsProvider
    {
        public Task<SolutionItem> GetItemAsync()
        {
            return Task.FromResult(default(SolutionItem));
        }

        public Task<IEnumerable<SolutionItem>> GetItemsAsync(SolutionItem parent)
        {
            var list = new List<SolutionItem>();
            if (null != parent) return Task.FromResult(list.AsEnumerable());

            var folder = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition";
            if (!Directory.Exists(folder)) return Task.FromResult(list.AsEnumerable());

            foreach (var f in Directory.GetFiles(folder, "*.json"))
            {
                var ed = f.DeserializeFromJsonFile<EntityDefinition>();
                var entity = new SolutionItem
                {
                    id = ed.Id,
                    text = ed.Name,
                    icon = ed.IconClass ?? "fa fa-database",
                    url = "entity.details/" + ed.Id,
                    Tag = ed
                };

                var rules =
                    ed.BusinessRuleCollection.Select(
                        x => new SolutionItem { id = x.Name, text = x.Name, icon = "fa fa-graduation-cap" });
                entity.itemCollection.AddRange(rules);
                list.Add(entity);
            }

            return Task.FromResult(list.AsEnumerable());
        }
    }
}