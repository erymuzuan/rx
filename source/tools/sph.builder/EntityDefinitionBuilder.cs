using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using subscriber.entities;

namespace sph.builder
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
                let json = File.ReadAllText(f)
                let ed = json.DeserializeFromJson<EntityDefinition>()
                select this.RestoreAsync(ed);

            await Task.WhenAll(tasks);

            Console.WriteLine("Done Custom Entities");

        }

        private async Task DeleteElasticSearchType(EntityDefinition ed)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync(ConfigurationManager.ApplicationName.ToLowerInvariant() + "/" + ed.Name.ToLowerInvariant() );
                Console.WriteLine("DELETE {1} type : {0}", response.StatusCode, ed.Name.ToLowerInvariant());
            }
        }


        public async override Task RestoreAsync(EntityDefinition ed)
        {
            await this.DeleteElasticSearchType(ed);
            await InsertAsync(ed);

            var type = CompileEntityDefinition(ed);
            Console.WriteLine("Compiled : {0}", type);

            var sqlSub = new SqlTableSubscriber { NotificicationService = new ConsoleNotification(null) };
            await sqlSub.ProcessMessageAsync(ed);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                // mapping - get a clone to differ than the on the disk
                var clone = ed.Clone();
                clone.MemberCollection.Add(new Member { Name = "__builder", Type = typeof(string), IsNullable = true, IsExcludeInAll = true });

                var subs = new EntityIndexerMappingSubscriber { NotificicationService = new ConsoleNotification(null) };
                await subs.PutMappingAsync(clone);
            }
            await InsertIconAsync(ed);

            Console.WriteLine("Deploying : {0}", ed.Name);
            DeployCustomEntity(ed);

        }


        private async Task InsertIconAsync(EntityDefinition ed)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var folder = Path.Combine(wc, typeof(EntityDefinition).Name);
            var icon = Path.Combine(folder, ed.Name + ".png");
            if (!File.Exists(icon)) return;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(icon),
                Extension = ".png",
                Id = ed.IconStoreId,
                FileName = ed.Name + ".png",
                WebId = ed.IconStoreId
            };
            await store.DeleteAsync(ed.IconStoreId);
            await store.AddAsync(schema);
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
            var type = assembly.GetType(string.Format("Bespoke.Dev_{0}.Domain.{1}", ed.Id, ed.Name));
            return type;
        }



        private static void DeployCustomEntity(EntityDefinition ed)
        {
            var dll = string.Format("{0}.{1}.dll", ConfigurationManager.ApplicationName, ed.Name);
            var pdb = string.Format("{0}.{1}.pdb", ConfigurationManager.ApplicationName, ed.Name);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.WebPath + @"\bin\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.WebPath + @"\bin\" + pdb, true);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

            File.Copy(dllFullPath, ConfigurationManager.ToolsPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.ToolsPath + @"\" + pdb, true);
        }

    }
}