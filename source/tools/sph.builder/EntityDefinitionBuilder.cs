using System;
using System.IO;
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

        public override async Task Restore()
        {
            var folder = ConfigurationManager.WorkflowSourceDirectory + @"\EntityDefinition";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync(ConfigurationManager.ApplicationName);
                Console.WriteLine("DELETE {1} index : {0}", response.StatusCode, ConfigurationManager.ApplicationName);
                await client.PutAsync(ConfigurationManager.ApplicationName, new StringContent(""));

            }
            this.Initialize();
            Console.WriteLine("Reading from " + folder);
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                Console.WriteLine("Building from :{0} ", file);
                var json = File.ReadAllText(file);
                var ed = json.DeserializeFromJson<EntityDefinition>();

                await InsertAsync(ed);
                var type = CompileEntityDefinition(ed);
                Console.WriteLine("Compiled : {0}", type);

                var sqlSub = new SqlTableSubscriber{NotificicationService = new ConsoleNotification()};
                await sqlSub.ProcessMessageAsync(ed);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                    // mapping - get a clone to differ than the on the disk
                    var clone = ed.Clone();
                    clone.MemberCollection.Add(new Member{Name = "__builder", Type = typeof(string), IsNullable = true, IsExcludeInAll = true});
                    
                    var subs = new EntityIndexerMappingSubscriber{ NotificicationService = new ConsoleNotification()};
                    await subs.PutMappingAsync(clone);
                }
                await InsertIconAsync(ed);

                Console.WriteLine("Deploying : {0}", ed.Name);
                DeployCustomEntity(ed);
            }
            Console.WriteLine("Done Custom Entities");
            // now copy all the output to this tools

        }


        private async Task InsertIconAsync(EntityDefinition ed)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var folder = Path.Combine(wc, typeof(EntityDefinition).Name);
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

            var result = ed.Compile(options);
            result.Errors.ForEach(Console.WriteLine);

            var assembly = Assembly.LoadFrom(result.Output);
            var type = assembly.GetType(string.Format("Bespoke.Dev_{0}.Domain.{1}", ed.EntityDefinitionId, ed.Name));
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