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

            var folder = ConfigurationManager.WorkflowSourceDirectory + @"\EntityView";
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var view = json.DeserializeFromJson<EntityView>();
                await InsertIconAsync(view);
            }
            Console.WriteLine("Done EntityViews");

        }

        private async Task InsertIconAsync(EntityView ed)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var folder = Path.Combine(wc, typeof(EntityView).Name);
            var icon = Path.Combine(folder, ed.Name + ".png");
            if (!File.Exists(icon)) return;
            
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(icon),
                Extension = ".png",
                StoreId = ed.IconStoreId,
                FileName = ed.Name + ".png",
                WebId = ed.IconStoreId
            };
            await store.DeleteAsync(ed.IconStoreId);
            await store.AddAsync(schema);
        }

    }
}