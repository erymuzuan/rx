﻿using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    public class EntityFormBuilder : Builder<EntityForm>
    {

        public override async Task Restore()
        {
            await base.Restore();

            var folder = ConfigurationManager.WorkflowSourceDirectory + @"\EntityForm";
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var view = json.DeserializeFromJson<EntityForm>();
                await InsertIconAsync(view);
            }
            Console.WriteLine("Done EntityForms");

        }

        private async Task InsertIconAsync(EntityForm ed)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var folder = Path.Combine(wc, typeof(EntityForm).Name);
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