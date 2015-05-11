using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    public class EntityViewBuilder : Builder<EntityView>
    {

        public override async Task RestoreAllAsync()
        {
            await base.RestoreAllAsync();

            var folder = ConfigurationManager.SphSourceDirectory + @"\EntityView";
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var view = json.DeserializeFromJson<EntityView>();
                await InsertIconAsync(view);
            }
            Console.WriteLine("Done EntityViews");

        }

        private async Task InsertIconAsync(EntityView view)
        {
            var icon = $"{ConfigurationManager.SphSourceDirectory}\\EntityView\\{view.Id}.png";
            if (!File.Exists(icon)) return;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(icon),
                Extension = ".png",
                Id = view.IconStoreId,
                FileName = view.Id + ".png",
                WebId = view.IconStoreId
            };
            await store.DeleteAsync(view.IconStoreId);
            await store.AddAsync(schema);
        }

    }
}