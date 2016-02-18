using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class WorkflowDefinitionPackage
    {

        public async Task<WorkflowDefinition> UnpackAsync(string zipFile)
        {
            var context = new SphDataContext();
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            var folder = Directory.CreateDirectory(Path.GetTempFileName() + "extract").FullName;
            ZipFile.ExtractToDirectory(zipFile, folder);

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var id = Strings.RegexSingleValue(Path.GetFileName(zipFile), "wd_(?<id>.*?)_.*?", "id");

            var wdFile = Path.Combine(folder, $"wd_{id}.json");
            var wdJson = File.ReadAllText(wdFile);
            var wd = JsonConvert.DeserializeObject<WorkflowDefinition>(wdJson, setting);

            if (!string.IsNullOrWhiteSpace(wd.SchemaStoreId))
            {
                var xsdFile = Path.Combine(folder, wd.SchemaStoreId + ".xsd");
                var xsd = new BinaryStore
                {
                    Content = File.ReadAllBytes(xsdFile),
                    Extension = ".xsd",
                    Id = wd.SchemaStoreId,
                    WebId = wd.SchemaStoreId,
                    FileName = "schema.xsd"
                };
                await store.DeleteAsync(wd.SchemaStoreId);
                await store.AddAsync(xsd);
            }

            using (var session = context.OpenSession())
            {
                session.Attach(wd);
                await session.SubmitChanges("Import");
            }

            return wd;

        }

        public async Task<BinaryStore> PackAsync(WorkflowDefinition wd)
        {
            var path = Path.GetTempPath() + @"/wd" + wd.Id;
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";
            var context = new SphDataContext();

            var store = ObjectBuilder.GetObject<IBinaryStore>();

            var schema = await store.GetContentAsync(wd.SchemaStoreId);
            File.WriteAllBytes(Path.Combine(path, wd.SchemaStoreId + ".xsd"), schema.Content);
            File.WriteAllBytes(Path.Combine(path, "wd_" + wd.Id + ".json"), Encoding.UTF8.GetBytes(wd.ToJsonString()));
            // get the screen view
          
            if (File.Exists(zip))
                File.Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);
            var zd = new BinaryStore
            {
                Id = Guid.NewGuid().ToString(),
                Content = File.ReadAllBytes(zip),
                Extension = ".zip",
                FileName = $"wd_{wd.Id}_{wd.Version}.zip",
                WebId = Guid.NewGuid().ToString()
            };
            await store.AddAsync(zd);

            return zd;
        }
    }
}