using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    public abstract class SourceAssetProviders<T> : IItemsProvider where T : Entity
    {

        protected abstract  string Icon { get; }
        protected abstract string GetIcon(T item); 
        protected abstract string GetEditUrl(T item);
        protected abstract string GetName(T item);

        public Task<SolutionItem> GetItemAsync()
        {
            var parent = new SolutionItem
            {
                text = typeof(T).Name,
                id = typeof(T).Name,
                icon = this.Icon
            };
            var folder = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}";
            foreach (var f in Directory.GetFiles(folder, "*.json"))
            {
                var item = f.DeserializeFromJsonFile<T>();
                var node = new SolutionItem
                {
                    id = "__" + typeof(T).Name + "__" + Path.GetFileNameWithoutExtension(f),
                    text = GetName(item),
                    url = GetEditUrl(item),
                    icon = GetIcon(item)
                };
                if (!string.IsNullOrWhiteSpace(node.url))
                    parent.AddItems(node);
            }
            return Task.FromResult(parent);
        }

        public Task<IEnumerable<SolutionItem>> GetItemsAsync(SolutionItem parent)
        {
            return Task.FromResult(Array.Empty<SolutionItem>().AsEnumerable());
        }
        
    }
}