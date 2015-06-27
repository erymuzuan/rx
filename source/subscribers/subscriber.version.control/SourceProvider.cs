using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public abstract class SourceProvider<T> where T : Entity
    {
        public abstract Task ProcessItem(T item);

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
        protected async Task PersistDocumentAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return;

            var folder = $"{ConfigurationManager.SphSourceDirectory}\\BinaryStores";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(id);

            if (null == doc) return;
            File.WriteAllBytes($"{folder}\\{id}", doc.Content);
            File.WriteAllText($"{folder}\\{id}.json", doc.ToJsonString(true));
        }
    }
}