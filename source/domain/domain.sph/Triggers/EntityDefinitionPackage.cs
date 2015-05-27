using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Bespoke.Sph.Domain
{
    public class EntityDefinitionPackage
    {

        public async Task<WorkflowDefinition> UnpackAsync(string zipFile)
        {
            var context = new SphDataContext();
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            setting.Converters.Add(new StringEnumConverter());

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
                await store.AddAsync(xsd);
            }

            // get the pages
            using (var session = context.OpenSession())
            {
                wd.Id = string.Empty;
                session.Attach(wd);
                await session.SubmitChanges("Import");
            }
            using (var session = context.OpenSession())
            {
                foreach (var pf in Directory.GetFiles(folder, "page.*.json"))
                {
                    var pageJson = File.ReadAllText(pf);
                    var page = JsonConvert.DeserializeObject<Page>(pageJson, setting);
                    page.ChangeWorkflowDefinitionVersion(id, wd.Id);

                    session.Attach(page);
                }
                await session.SubmitChanges("Import");
            }

            return wd;

        }

        public async Task<string> PackAsync(EntityDefinition ed, bool includeData = false)
        {
            var path = Path.Combine(Path.GetTempPath(), "rx.package." + ed.Id);
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";
            var context = new SphDataContext();

            // forms
            var formQuery = context.EntityForms.Where(f => f.Entity == ed.Name);
            var viewQuery = context.EntityForms.Where(f => f.Entity == ed.Name);
            var triggerQuery = context.EntityForms.Where(f => f.Entity == ed.Name);

            var forms = (await context.LoadAsync(formQuery, 1, 50, true)).ItemCollection;
            var views = (await context.LoadAsync(viewQuery, 1, 50, true)).ItemCollection;
            var triggers = (await context.LoadAsync(triggerQuery, 1, 50, true)).ItemCollection;

            var store = ObjectBuilder.GetObject<IBinaryStore>();

            var icon = await store.GetContentAsync(ed.IconStoreId);
            if (null != icon)
                File.WriteAllBytes(Path.Combine(path, "BinaryStore_" + ed.IconStoreId), icon.Content);

            File.WriteAllBytes(Path.Combine(path, "EntityDefinition" + ed.Id + ".json"), Encoding.UTF8.GetBytes(ed.ToJsonString()));


            foreach (var form in forms)
            {
                File.WriteAllBytes(Path.Combine(path, $"EntityForm_{form.Id}.json"), Encoding.UTF8.GetBytes(form.ToJsonString(true)));

                // get the partial,view and viewmodels
                Copy(form.Route + ".html", "views", path);
                Copy(form.Route + ".js", "viewmodels", path);
                Copy(form.Route + ".js", "partial", path);


                if (string.IsNullOrEmpty(form.IconStoreId)) continue;
                var formIcon = await store.GetContentAsync(form.IconStoreId);
                if (null != formIcon)
                    File.WriteAllBytes(Path.Combine(path, "BinaryStore_" + ed.IconStoreId), formIcon.Content);
            }

            foreach (var v in views)
            {
                File.WriteAllBytes(Path.Combine(path, $"EntityView_{v.Id}.json"), Encoding.UTF8.GetBytes(v.ToJsonString(true)));

                // get the partial,view and viewmodels
                Copy(v.Route + ".html", "views", path);
                Copy(v.Route + ".js", "viewmodels", path);
                Copy(v.Route + ".js", "partial", path);

                if (string.IsNullOrEmpty(v.IconStoreId)) continue;
                var formIcon = await store.GetContentAsync(v.IconStoreId);
                if (null != formIcon)
                    File.WriteAllBytes(Path.Combine(path, "BinaryStore_" + ed.IconStoreId), formIcon.Content);
            }
            foreach (var t in triggers)
            {
                File.WriteAllBytes(Path.Combine(path, $"Trigger_{t.Id}.json"), Encoding.UTF8.GetBytes(t.ToJsonString(true)));

            }

            if (includeData)
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["sph"].ConnectionString))
                using (var cmd = new SqlCommand($"SELECT [Id], [Json] FROM [{ConfigurationManager.ApplicationName}].[{ed.Name}]", conn))
                {
                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var id = reader.GetString(0);
                            var data = reader.GetString(1);
                            File.WriteAllText($"{path}\\data_{id}.json", data);
                        }
                    }
                }
            }

            if (File.Exists(zip))
                File.Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);


            return zip;
        }

        private static void Copy(string fileName, string folder, string path)
        {
            var view = Path.Combine(ConfigurationManager.WebPath, $"SphApp\\{folder}\\{fileName}");
            if (File.Exists(view))
                File.Copy(view, $"{path}\\SphApp_{folder}_{fileName}", true);
        }
    }
}