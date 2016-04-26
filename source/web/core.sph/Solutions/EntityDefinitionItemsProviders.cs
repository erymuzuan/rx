using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    public abstract class EntityDefinitionItemsProviders<T> : IItemsProvider where T : Entity
    {
        public Task<SolutionItem> GetItemAsync()
        {
            return Task.FromResult(default(SolutionItem));
        }

        protected virtual string Icon => "fa fa-file-o";
        protected virtual string GetUrl(T item) => "";

        public Task<IEnumerable<SolutionItem>> GetItemsAsync(SolutionItem parent)
        {
            var empty = Array.Empty<SolutionItem>().AsEnumerable();
            var ed = parent?.Tag as EntityDefinition;
            if (null == ed)
                return Task.FromResult(empty);

            var folder = $"{ConfigurationManager.SphSourceDirectory}\\{typeof(T).Name}";
            if (!Directory.Exists(folder)) return Task.FromResult(empty);
            var list = from f in Directory.GetFiles(folder, "*.json")
                       let item = f.DeserializeFromJsonFile<T>()
                       let form = (IEntityDefinitionAsset)item
                       where null != item && null != form && ed.Name == form.Entity
                       select new SolutionItem
                       {
                           id = form.Id,
                           text = form.Name,
                           icon = this.Icon,
                           url = this.GetUrl(item)
                       };

            return Task.FromResult(list.AsEnumerable());
        }
    }
}