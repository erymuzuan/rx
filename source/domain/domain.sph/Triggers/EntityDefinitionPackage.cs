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

        public async Task<EntityDefinition> UnpackAsync(string zipFile, string folder)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            setting.Converters.Add(new StringEnumConverter());

            ZipFile.ExtractToDirectory(zipFile, folder);

            var edFile = Directory.GetFiles(folder, "EntityDefinition_*.json").Single();

            var wdJson = File.ReadAllText(edFile);
            var ed = JsonConvert.DeserializeObject<EntityDefinition>(wdJson, setting);

            var logger = ObjectBuilder.GetObject<ILogger>();
            var views = Directory.GetFiles(folder, "EntityView_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityView>());
            var forms = Directory.GetFiles(folder, "EntityForm_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityForm>());
            var triggers = Directory.GetFiles(folder, "Trigger_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<Trigger>());
            var charts = Directory.GetFiles(folder, "EntityChart_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityChart>());
            views.Select(x => new LogEntry { Message = $"Deserializing EntityView:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            forms.Select(x => new LogEntry { Message = $"Deserializing EntityForm:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            triggers.Select(x => new LogEntry { Message = $"Deserializing Trigger:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            charts.Select(x => new LogEntry { Message = $"Deserializing Chart:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            await Task.Delay(500);
            return ed;

        }
        public async Task<EntityDefinition> ImportAsync(string folder)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            setting.Converters.Add(new StringEnumConverter());

            var edFile = Directory.GetFiles(folder, "EntityDefinition_*.json").Single();

            var wdJson = File.ReadAllText(edFile);
            var ed = JsonConvert.DeserializeObject<EntityDefinition>(wdJson, setting);

            var logger = ObjectBuilder.GetObject<ILogger>();
            var views = Directory.GetFiles(folder, "EntityView_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityView>()).ToList();
            var forms = Directory.GetFiles(folder, "EntityForm_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityForm>()).ToList();
            var charts = Directory.GetFiles(folder, "EntityChart_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<EntityChart>()).ToList();
            var triggers = Directory.GetFiles(folder, "Trigger_*.json").Select(x => File.ReadAllText(x).DeserializeFromJson<Trigger>()).ToList();
            views.Select(x => new LogEntry { Message = $"Deserializing EntityView:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            forms.Select(x => new LogEntry { Message = $"Deserializing EntityForm:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            triggers.Select(x => new LogEntry { Message = $"Deserializing Trigger:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));
            charts.Select(x => new LogEntry { Message = $"Deserializing Chart:{x.Name}", Severity = Severity.Info }).ToList().ForEach(x => logger.Log(x));

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                session.Attach(forms.Cast<Entity>().ToArray());
                session.Attach(views.Cast<Entity>().ToArray());
                session.Attach(triggers.Cast<Entity>().ToArray());
                session.Attach(charts.Cast<Entity>().ToArray());

                await session.SubmitChanges("Save").ConfigureAwait(false);
            }

            // copy files
            foreach (var vm in Directory.GetFiles(folder, "SphApp_viewmodels_*.js"))
            {
                File.Copy(vm, $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{(Path.GetFileName(vm) ?? "").Replace("SphApp_viewmodels_", "")}", true);
            }
            foreach (var vm in Directory.GetFiles(folder, "SphApp_partial_*.js"))
            {
                File.Copy(vm, $"{ConfigurationManager.WebPath}\\SphApp\\partial\\{(Path.GetFileName(vm) ?? "").Replace("SphApp_partial_", "")}", true);
            }
            foreach (var html in Directory.GetFiles(folder, "SphApp_views*.html"))
            {
                File.Copy(html, $"{ConfigurationManager.WebPath}\\SphApp\\views\\{(Path.GetFileName(html) ?? "").Replace("SphApp_views_", "")}", true);
            }

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            foreach (var file in Directory.GetFiles(folder, "BinaryStore_*.json"))
            {
                var doc = File.ReadAllText(file).DeserializeFromJson<BinaryStore>();
                doc.Content = File.ReadAllBytes($"{folder}\\doc_{doc.Id}{doc.Extension}");
                await store.DeleteAsync(doc.Id);
                await store.AddAsync(doc);
            }
            var dll = $"{folder}\\{ConfigurationManager.ApplicationName}.{ed.Name}.dll";
            var pdb = $"{folder}\\{ConfigurationManager.ApplicationName}.{ed.Name}.pdb";
            if (File.Exists(dll) && File.Exists(pdb))
            {
                File.Copy(dll, $"{ConfigurationManager.WorkflowCompilerOutputPath}\\{ConfigurationManager.ApplicationName}.{ed.Name}.dll", true);
                File.Copy(pdb, $"{ConfigurationManager.WorkflowCompilerOutputPath}\\{ConfigurationManager.ApplicationName}.{ed.Name}.pdb", true);
            }

            //foreach (var sub in Directory.GetFiles(folder, "subscriber.trigger.*"))
            //{
            //    File.Copy(sub, $"{ConfigurationManager.WorkflowCompilerOutputPath}\\{Path.GetFileName(sub)}", true);
            //    File.Copy(sub, $"{ConfigurationManager.SubscriberPath}\\{Path.GetFileName(sub)}", true);
            //}

            // TODO : drop existing table and elasticsearch mapping

            return ed;

        }



        public async Task ImportDataAsync(string folder)
        {
            var context = new SphDataContext();
            foreach (var js in Directory.GetFiles(folder, "data_*.json"))
            {
                dynamic item = File.ReadAllText(js).DeserializeFromJson<Entity>();
                using (var session = context.OpenSession())
                {
                    session.Attach(item);
                    await session.SubmitChanges("save", null);
                }
            }
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
            var chartsQuery = context.EntityCharts.Where(f => f.Entity == ed.Name);

            var forms = (await context.LoadAsync(formQuery, 1, 50, true)).ItemCollection;
            var views = (await context.LoadAsync(viewQuery, 1, 50, true)).ItemCollection;
            var triggers = (await context.LoadAsync(triggerQuery, 1, 50, true)).ItemCollection;
            var charts = (await context.LoadAsync(chartsQuery, 1, 50, true)).ItemCollection;

            var store = ObjectBuilder.GetObject<IBinaryStore>();

            var icon = await store.GetContentAsync(ed.IconStoreId);
            if (null != icon)
            {
                WriteBinaryDocument(path, icon.Id, icon);
            }

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
                {
                    WriteBinaryDocument(path, form.IconStoreId, formIcon);
                }
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
                {
                    WriteBinaryDocument(path, v.IconStoreId, formIcon);
                }
            }
            foreach (var t in triggers)
            {
                File.WriteAllBytes(Path.Combine(path, $"Trigger_{t.Id}.json"), Encoding.UTF8.GetBytes(t.ToJsonString(true)));
                var trigglerDll = $"{ConfigurationManager.WorkflowCompilerOutputPath}\\subscriber.trigger.{t.Id}.dll";
                var triggerPdb = $"{ConfigurationManager.WorkflowCompilerOutputPath}\\subscriber.trigger.{t.Id}.pdb";
                if (File.Exists(trigglerDll))
                {
                    File.Copy(trigglerDll, $"{path}\\{Path.GetFileName(trigglerDll)}");
                }

                if (File.Exists(triggerPdb))
                {
                    File.Copy(triggerPdb, $"{path}\\{Path.GetFileName(triggerPdb)}");
                }
            }
            foreach (var t in charts)
            {
                File.WriteAllBytes(Path.Combine(path, $"EntityChart_{t.Id}.json"), Encoding.UTF8.GetBytes(t.ToJsonString(true)));
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

            var output = $"{ConfigurationManager.WorkflowCompilerOutputPath}\\{ConfigurationManager.ApplicationName}.{ed.Name}.dll";
            var pdb = $"{ConfigurationManager.WorkflowCompilerOutputPath}\\{ConfigurationManager.ApplicationName}.{ed.Name}.pdb";
            if (File.Exists(output))
            {
                File.Copy(output, $"{path}\\{Path.GetFileName(output)}");
            }

            if (File.Exists(pdb))
            {
                File.Copy(pdb, $"{path}\\{Path.GetFileName(pdb)}");
            }

            if (File.Exists(zip))
                File.Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);


            return zip;
        }

        private static void WriteBinaryDocument(string path, string storeId, BinaryStore document)
        {
            File.WriteAllBytes(Path.Combine(path, "doc_" + storeId + document.Extension), document.Content);
            File.WriteAllText(Path.Combine(path, "BinaryStore_" + storeId + ".json"), document.ToJsonString());
        }

        private static void Copy(string fileName, string folder, string path)
        {
            var view = Path.Combine(ConfigurationManager.WebPath, $"SphApp\\{folder}\\{fileName}");
            if (File.Exists(view))
                File.Copy(view, $"{path}\\SphApp_{folder}_{fileName}", true);
        }

    }
}