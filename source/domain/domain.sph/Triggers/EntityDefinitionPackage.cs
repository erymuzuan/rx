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

        public async Task<EntityDefinition> UnpackAsync(string zipFile)
        {
            var context = new SphDataContext();
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            setting.Converters.Add(new StringEnumConverter());

            var folder = Directory.CreateDirectory(Path.GetTempFileName() + "extract").FullName;
            ZipFile.ExtractToDirectory(zipFile, folder);

            var edFile = Directory.GetFiles(folder, "EntityDefinition_*.json").Single();

            var wdJson = File.ReadAllText(edFile);
            var ed = JsonConvert.DeserializeObject<EntityDefinition>(wdJson, setting);

            var logger = ObjectBuilder.GetObject<ILogger>();
            var views = Directory.GetFiles(folder, "EntityView_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityView>());
            var forms = Directory.GetFiles(folder, "EntityForm_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityForm>());
            var triggers = Directory.GetFiles(folder, "Trigger_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<Trigger>());
            views.Select(x => new LogEntry { Message = $"EntityView:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            forms.Select(x => new LogEntry { Message = $"EntityForm:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            triggers.Select(x => new LogEntry { Message = $"Trigger:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            await Task.Delay(500);


            // var store = ObjectBuilder.GetObject<IBinaryStore>();
            //if (!string.IsNullOrWhiteSpace(ed.IconStoreId))
            //{
            //    var xsdFile = Path.Combine(folder, ed.IconStoreId + ".xsd");
            //    var xsd = new BinaryStore
            //    {
            //        Content = File.ReadAllBytes(xsdFile),
            //        Extension = ".xsd",
            //        Id = ed.SchemaStoreId,
            //        WebId = ed.SchemaStoreId,
            //        FileName = "schema.xsd"
            //    };
            //    await store.AddAsync(xsd);
            //}

            // get the pages
            //using (var session = context.OpenSession())
            //{
            //    ed.Id = string.Empty;
            //    session.Attach(ed);
            //    await session.SubmitChanges("Import");
            //}


            return ed;

        }

        public async Task<string> PackAsync(EntityDefinition ed, bool includeData = false)
        {
            var path = Path.Combine(Path.GetTempPath(), "rx.package." + ed.Id);
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";
            var context = new SphDataContext();

            // forms
            var formQuery = context.EntityForms.Where(f => f.EntityDefinitionId == ed.Id);
            var viewQuery = context.EntityViews.Where(f => f.EntityDefinitionId == ed.Id);
            var triggerQuery = context.Triggers.Where(f => f.Entity == ed.Name);

            var forms = (await context.LoadAsync(formQuery, 1, 50, true)).ItemCollection;
            var views = (await context.LoadAsync(viewQuery, 1, 50, true)).ItemCollection;
            var triggers = (await context.LoadAsync(triggerQuery, 1, 50, true)).ItemCollection;

            var store = ObjectBuilder.GetObject<IBinaryStore>();

            var icon = await store.GetContentAsync(ed.IconStoreId);
            if (null != icon)
                File.WriteAllBytes(Path.Combine(path, "BinaryStore_" + ed.IconStoreId), icon.Content);

            File.WriteAllBytes(Path.Combine(path, "EntityDefinition_" + ed.Id + ".json"), Encoding.UTF8.GetBytes(ed.ToJsonString()));


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