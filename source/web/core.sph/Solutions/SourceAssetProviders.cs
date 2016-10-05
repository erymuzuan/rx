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
        public Task<SolutionItem> GetItemAsync(string file)
        {
            var parent = Directory.GetParent(file).Name;
            if (parent != typeof(T).Name) return Task.FromResult(default(SolutionItem));
            var item = this.GetSolutionItem(file);
            if (null == item) return Task.FromResult(default(SolutionItem));
            var node = new SolutionItem
            {
                id = $"__{typeof(T).Name}__{Path.GetFileNameWithoutExtension(file)}",
                text = GetName(item),
                url = GetEditUrl(item),
                icon = GetIcon(item),
                type = parent

            };
            return Task.FromResult(node);
        }

        public async Task WaitReadyAsync(string fileName)
        {
            while (true)
            {
                try
                {
                    using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        Console.WriteLine($"Output file {fileName} ready. " + stream);
                        break;
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"Output file {fileName} not yet ready ({ex.Message})");
                }
                await Task.Delay(500);
            }
        }
        protected virtual T GetSolutionItem(string file)
        {
            WaitReadyAsync(file).Wait(2500);
            var item = file.DeserializeFromJsonFile<T>();
            return item;
        }
        protected abstract string Icon { get; }
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
            if (!Directory.Exists(folder)) return Task.FromResult(parent);

            foreach (var f in Directory.GetFiles(folder, "*.json"))
            {
                var item = f.DeserializeFromJsonFile<T>();
                var node = new SolutionItem
                {
                    id = $"__{typeof(T).Name}__{Path.GetFileNameWithoutExtension(f)}",
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