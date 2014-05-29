using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    public class DocumentTemplateBuilder : Builder<DocumentTemplate>
    {

        public override async Task RestoreAllAsync()
        {
            await base.RestoreAllAsync();

            var folder = ConfigurationManager.WorkflowSourceDirectory + @"\DocumentTemplate";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                return;
            }
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var view = json.DeserializeFromJson<DocumentTemplate>();
                await InsertTemplateAsync(view);
            }
            Console.WriteLine("Done DocumentTemplate");

        }

        private async Task InsertTemplateAsync(DocumentTemplate ed)
        {
            const string extension = ".docx";
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var folder = Path.Combine(wc, typeof(DocumentTemplate).Name);
            var icon = Path.Combine(folder, ed.Name + extension);
            if (!File.Exists(icon)) return;
            
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(icon),
                Extension = extension,
                StoreId = ed.WordTemplateStoreId,
                FileName = ed.Name + extension,
                WebId = ed.WordTemplateStoreId
            };
            await store.DeleteAsync(ed.WordTemplateStoreId);
            await store.AddAsync(schema);
        }

    }
}