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
        public Task<SolutionItem> GetItemAsync(string file)
        {
            var parent = Directory.GetParent(file).Name;
            if (parent != typeof(T).Name) return Task.FromResult(default(SolutionItem));
            dynamic item = this.GetSolutionItem(file);
            if (null == item) return Task.FromResult(default(SolutionItem));
            string edName = item.Entity;
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Name == edName);

            var node = new SolutionItem
            {
                id = $"__{typeof(T).Name}__{Path.GetFileNameWithoutExtension(file)}",
                text = this.GetName(item),
                url = this.GetUrl(item),
                icon = this.Icon,
                type = ed?.Id
            };
            return Task.FromResult(node);
        }

        protected virtual T GetSolutionItem(string file)
        {
            if (!File.Exists(file)) return default(T);
            var item = file.DeserializeFromJsonFile<T>();
            return item;
        }
        public Task<SolutionItem> GetItemAsync()
        {
            return Task.FromResult(default(SolutionItem));
        }

        protected abstract string Icon { get; }
        protected abstract string GetUrl(T item);
        protected abstract string GetName(T item);
        protected abstract string GetEntityDefinitionName(T item);

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
                       where null != item && ed.Name == this.GetEntityDefinitionName(item)
                       select new SolutionItem
                       {
                           id = $"__{typeof(T).Name}__{item.Id}",
                           text = this.GetName(item),
                           icon = this.Icon,
                           url = this.GetUrl(item)
                       };

            return Task.FromResult(list.AsEnumerable());
        }
    }
}