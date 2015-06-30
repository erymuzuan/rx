using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public abstract class SourceProvider<T> where T : Entity
    {
        public abstract Task ProcessItem(T item);
        public abstract Task RemoveItem(T item);

        protected void SaveJsonSource(T item)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = typeof(T).Name;
            var folder = Path.Combine(wc, type);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Id + ".json");
            File.WriteAllText(file, item.ToJsonString(true));

        }
        protected void RemoveJsonSource(T item)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = typeof(T).Name;
            var folder = Path.Combine(wc, type);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Id + ".json");
            if (File.Exists(file))
                File.Delete(file);

        }
        protected async Task PersistDocumentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;

            var folder = $"{ConfigurationManager.SphSourceDirectory}\\BinaryStores";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(id);

            if (null == doc) return;
            string privatePath = $"{folder}\\{id}";
            if (!Directory.Exists(privatePath))
                Directory.CreateDirectory(privatePath);
            File.WriteAllBytes($"{privatePath}\\{doc.FileName}", doc.Content);

            File.WriteAllText($"{folder}\\{id}.json", doc.ToJsonString(true));
        }

        protected async Task RemoveDocumentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;

            var folder = $"{ConfigurationManager.SphSourceDirectory}\\BinaryStores";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            await store.DeleteAsync(id);

            if (Directory.Exists($"{folder}\\{id}"))
                Directory.Delete($"{folder}\\{id}", true);

            if (File.Exists($"{folder}\\{id}.json"))
                File.Delete($"{folder}\\{id}.json");
        }
    }
}