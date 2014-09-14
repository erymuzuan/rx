using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    public class EntityFormBuilder : Builder<EntityForm>
    {

        public override async Task RestoreAllAsync()
        {
            await base.RestoreAllAsync();

            var folder = ConfigurationManager.SphSourceDirectory + @"\EntityForm";
            var tasks = from file in Directory.GetFiles(folder, "*.json")
                        let json = File.ReadAllText(file)
                        let form = json.DeserializeFromJson<EntityForm>()
                        select InsertIconAsync(form);

            await Task.WhenAll(tasks);
            Console.WriteLine("Done EntityForms");

        }

        public override async Task RestoreAsync(EntityForm item)
        {
            try
            {

                await InsertAsync(item);
                await InsertIconAsync(item);
            }
            catch (Exception)
            {

                Console.WriteLine("EntityForm -> {0}", item.Route);
                throw;
            }
        }

        private async Task InsertIconAsync(EntityForm form)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var folder = Path.Combine(wc, typeof(EntityForm).Name);
            var icon = Path.Combine(folder, form.Name + ".png");
            if (!File.Exists(icon)) return;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(icon),
                Extension = ".png",
                Id = form.IconStoreId,
                FileName = form.Name + ".png",
                WebId = form.IconStoreId
            };
            await store.DeleteAsync(form.IconStoreId);
            await store.AddAsync(schema);
        }

    }
}