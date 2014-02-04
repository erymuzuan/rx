using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using subscriber.entities;

namespace sph.builder
{
    public class Program
    {
        private const string SphConnection = "sph";

        static void Main()
        {
            var program = new Program();
            program.RestoreEntityDefinitions().Wait();
            program.RestoreEntityForms().Wait();
            program.RestoreEntityViews().Wait();
        }


        public async Task RestoreEntityViews()
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "EntityView");
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var view = json.DeserializeFromJson<EntityView>();
                await this.InsertEntityViewAsync(view);

            }
        }
        public async Task RestoreEntityForms()
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "EntityForm");
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var form = json.DeserializeFromJson<EntityForm>();
                await this.InsertEntityFormAsync(form);

            }
        }
        /*
        public async Task RestoreWorkflowDefinitions()
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "WorkflowDefinitions");
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var wd = json.DeserializeFromJson<WorkflowDefinition>();
                await this.InsertWorkflowDefinitionAsync(wd);

            }
        }

        public async Task RestoreTriggers()
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "Triggers");
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var trigger = json.DeserializeFromJson<Trigger>();
                await this.InsertTriggerAsync(form);

            }
        }
        */
        public async Task RestoreEntityDefinitions()
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, "EntityDefinition");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync("dev");
                Console.WriteLine("DELETE dev : {0}", response.StatusCode);
                await client.PutAsync("dev", new StringContent(""));

            }
            foreach (var file in Directory.GetFiles(folder, "*.json"))
            {

                var json = File.ReadAllText(file);
                var ed = json.DeserializeFromJson<EntityDefinition>();

                await InsertEntityDefinitionIntoSql(ed);
                var type = CompileEntityDefinition(ed);
                Console.WriteLine("Compiled : {0}", type);

                var sqlSub = new SqlTableSubscriber();
                await sqlSub.ProcessMessageAsync(ed);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                    // mapping
                    var subs = new EntityIndexerMappingSubscriber();
                    await subs.ProcessMessageAsync(ed);
                }

                DeployCustomEntity(ed);
            }

        }


        private static Type CompileEntityDefinition(EntityDefinition ed)
        {
            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };
            var webDir = ConfigurationManager.WebPath;
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(webDir + @"\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(webDir + @"\bin\core.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(webDir + @"\bin\Newtonsoft.Json.dll")));

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
        }

        private async Task InsertEntityFormAsync(EntityForm form)
        {

            SphConnection.ExecuteNonQuery("TRUNCATE TABLE [Sph].[EntityForm]");
            var sql = string.Format(@"INSERT INTO [Sph].[EntityForm]
           ([Data]
           ,[Name]
           ,[Route]
           ,[EntityDefinitionId]
           ,[IsDefault]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           (@xml
           ,'{0}'
           ,'{1}'
           ,{2}
           ,{3}
           ,GETDATE()
           ,'admin'
           ,GETDATE()
           ,'admin')", form.Name, form.Route, form.EntityDefinitionId, form.IsDefault ? "1" : "0");
            await SphConnection.ExecuteNonQueryAsync(sql, new SqlParameter("@xml", form.ToXmlString()));
        }

        private async Task InsertEntityViewAsync(EntityView view)
        {

            SphConnection.ExecuteNonQuery("TRUNCATE TABLE [Sph].[EntityView]");
            var sql = string.Format(@"INSERT INTO [Sph].[EntityView]
           ([Data]
           ,[Name]
           ,[Route]
           ,[EntityDefinitionId]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           (@xml
           ,'{0}'
           ,'{1}'
           ,{2}
           ,GETDATE()
           ,'admin'
           ,GETDATE()
           ,'admin')", view.Name, view.Route, view.EntityDefinitionId);
            await SphConnection.ExecuteNonQueryAsync(sql, new SqlParameter("@xml", view.ToXmlString()));
        }

        private async Task InsertEntityDefinitionIntoSql(EntityDefinition ed)
        {

            SphConnection.ExecuteNonQuery("TRUNCATE TABLE [Sph].[EntityDefinition]");
            var insertCustomerDefinitionSql = string.Format(@"INSERT INTO [Sph].[EntityDefinition]
           ([Data]
           ,[Name]
           ,[Plural]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ChangedDate]
           ,[ChangedBy])
     VALUES
           (@xml
           ,'{0}'
           ,'{1}'
           ,GETDATE()
           ,'admin'
           ,GETDATE()
           ,'admin')", ed.Name, ed.Plural);
            await SphConnection.ExecuteNonQueryAsync(insertCustomerDefinitionSql, new SqlParameter("@xml", ed.ToXmlString()));
        }

    }
}
