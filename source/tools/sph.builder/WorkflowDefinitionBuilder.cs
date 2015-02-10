using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    internal class WorkflowDefinitionBuilder : Builder<WorkflowDefinition>
    {
        public override async Task RestoreAllAsync()
        {
            this.Initialize();
            var workflowDefinitions = this.GetItems();
            foreach (var wd in workflowDefinitions)
            {
                await RestoreAsync(wd);

            }
        }

        private async Task<bool> InsertSchemaAsync(WorkflowDefinition wd)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var folder = Path.Combine(wc, typeof(WorkflowDefinition).Name);
            var xsd = Path.Combine(folder, wd.Name + ".xsd");
            if (!File.Exists(xsd)) return false;

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(xsd),
                Extension = ".xsd",
                Id = wd.SchemaStoreId,
                FileName = wd.Name + ".xsd",
                WebId = wd.SchemaStoreId
            };
            await store.AddAsync(schema);
            return true;
        }

        private async Task CompileAsync(WorkflowDefinition item)
        {
            var options = new CompilerOptions();
            var result = await item.CompileAsync(options).ConfigureAwait(false);
            result.Errors.ForEach(Console.WriteLine);

            this.Deploy(item);
        }

        private void Deploy(WorkflowDefinition item)
        {
            var dll = string.Format("workflows.{0}.{1}.dll", item.Id, item.Version);
            var pdb = string.Format("workflows.{0}.{1}.pdb", item.Id, item.Version);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }


        private IEnumerable<ScreenActivityForm> GetPublishPages(WorkflowDefinition wd)
        {
            if (null == wd) throw new ArgumentNullException("wd");
            var screens = wd.ActivityCollection.OfType<ScreenActivity>();



            return screens.Select(scr => new ScreenActivityForm
            {
                WorkflowDefinitionId = wd.Id,
                Version = wd.Version,
                WebId = Guid.NewGuid().ToString()
            }).ToList();

        }


        public override async Task RestoreAsync(WorkflowDefinition wd)
        {
            Console.WriteLine("Compiling : {0} ", wd.Name);
            var exist = await this.InsertSchemaAsync(wd);
            if (!exist) return;

            await this.InsertAsync(wd);
            try
            {
                await this.CompileAsync(wd).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to compile workflow {0}", wd.Name);
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
                return;
            }
            // save
            var pages = this.GetPublishPages(wd);
            //archive the WD
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var archived = new BinaryStore
            {
                Id = string.Format("wd.{0}.{1}", wd.Id, wd.Version),
                Content = Encoding.Unicode.GetBytes(wd.ToJsonString(true)),
                Extension = ".json",
                FileName = string.Format("wd.{0}.{1}.json", wd.Id, wd.Version)

            };
            await store.DeleteAsync(archived.Id);
            await store.AddAsync(archived);
            var pageBuilder = new Builder<ScreenActivityForm>();
            pageBuilder.Initialize();
            foreach (var page in pages)
            {
                page.Id = (Guid.NewGuid()).ToString();
                await pageBuilder.InsertAsync(page);
            }
        }
    }
}