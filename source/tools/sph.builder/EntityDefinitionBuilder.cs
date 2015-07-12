using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;
using subscriber.entities;

namespace Bespoke.Sph.SourceBuilders
{
    public class EntityDefinitionBuilder : Builder<EntityDefinition>
    {

        public override async Task RestoreAllAsync()
        {
            var folder = ConfigurationManager.SphSourceDirectory + @"\EntityDefinition";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync(ConfigurationManager.ApplicationName);
                Console.WriteLine("DELETE {1} index : {0}", response.StatusCode, ConfigurationManager.ApplicationName);
                await client.PutAsync(ConfigurationManager.ApplicationName, new StringContent(""));

            }
            this.Initialize();
            this.Clean();
            Console.WriteLine("Reading from " + folder);

            var tasks = from f in Directory.GetFiles(folder, "*.json")
                        let ed = f.DeserializeFromJsonFile<EntityDefinition>()
                        select this.RestoreAsync(ed);

            await Task.WhenAll(tasks);

            Console.WriteLine("Done Custom Entities");

        }

        private async Task DeleteElasticSearchType(EntityDefinition ed)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync(ConfigurationManager.ApplicationName.ToLowerInvariant() + "/_mapping/" + ed.Name.ToLowerInvariant());
                Console.WriteLine("DELETE {1} type : {0}", response.StatusCode, ed.Name.ToLowerInvariant());
            }
        }

        public async override Task RestoreAsync(EntityDefinition ed)
        {
            await this.DeleteElasticSearchType(ed);

            var type = CompileEntityDefinition(ed);
            Console.WriteLine("Compiled : {0}", type);


            if (ed.TreatDataAsSource)
            {
                // clean all data
                var builder = new Builder { EntityDefinition = ed, Name = ed.Name };
                try
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    builder.Initialize();
                    await "Sph".ExecuteNonQueryAsync($"TRUNCATE TABLE [{ConfigurationManager.ApplicationName}].[{ed.Name}]");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please wait.. retrying....");
                    Console.WriteLine(e.ToString());

                    var sqlSub2 = new SqlTableSubscriber { NotificicationService = new ConsoleNotification() };
                    await sqlSub2.ProcessMessageAsync(ed);

                    await RestoreAsync(ed);
                    return;
                }
                finally
                {
                    Console.ResetColor();
                }

                var sqlSub1 = new SqlTableSubscriber { NotificicationService = new ConsoleNotification() };
                await sqlSub1.ProcessMessageAsync(ed);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);

                    var typeName = ed.Name.ToLowerInvariant();
                    var index = ConfigurationManager.ApplicationName.ToLowerInvariant();
                    var mappingFile = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{ed.Name}.mapping";
                    string mappingUrl = $"{index}/_mapping/{typeName}";

                    await client.DeleteAsync(mappingUrl);
                    if (File.Exists(mappingFile))
                    {
                        var mappingContent = new StringContent(File.ReadAllText(mappingFile));
                        await client.PutAsync(mappingUrl, mappingContent);

                    }

                    var files = Directory.
                    GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{ed.Name}", "*.json");
                    foreach (var f in files)
                    {
                        var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                        dynamic ent = JsonConvert.DeserializeObject(File.ReadAllText(f), setting);
                        ent.Id = Path.GetFileNameWithoutExtension(f);
                        await builder.InsertAsync(ent);


                        var setting2 = new JsonSerializerSettings();
                        var json = JsonConvert.SerializeObject(ent, setting2);

                        var content = new StringContent(json);

                        var url = $"{index}/{typeName}/{ent.Id}";
                        var response = await client.PutAsync(url, content);
                        Console.WriteLine($"{ent.Id} -> {response.StatusCode}");
                    }
                }

                return;
            }


            var sqlSub = new SqlTableSubscriber { NotificicationService = new ConsoleNotification() };
            await sqlSub.ProcessMessageAsync(ed);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                // mapping - get a clone to differ than the on the disk
                var clone = ed.Clone();
                clone.MemberCollection.Add(new Member { Name = "__builder", Type = typeof(string), IsNullable = true, IsExcludeInAll = true });

                var subs = new EntityIndexerMappingSubscriber { NotificicationService = new ConsoleNotification() };
                await subs.PutMappingAsync(clone);
                await subs.MigrateDataAsync(ed.Name);
            }

            Console.WriteLine("Deploying : {0}", ed.Name);

        }


        private static Type CompileEntityDefinition(EntityDefinition ed)
        {
            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };
            var webDir = ConfigurationManager.WebPath;
            options.AddReference(Path.GetFullPath(webDir + @"\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath(webDir + @"\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath(webDir + @"\bin\Newtonsoft.Json.dll"));

            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            var result = ed.Compile(options, sources);
            result.Errors.ForEach(Console.WriteLine);

            var assembly = Assembly.LoadFrom(result.Output);
            var type = assembly.GetType($"{ed.CodeNamespace}.{ed.Name}");
            return type;
        }



    }
}