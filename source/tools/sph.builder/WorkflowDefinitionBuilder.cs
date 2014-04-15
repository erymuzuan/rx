using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace sph.builder
{
    internal class WorkflowDefinitionBuilder : Builder<WorkflowDefinition>
    {
        public override async Task Restore()
        {
            this.Initialize();
            var workflowDefinitions = this.GetItems();
            foreach (var wd in workflowDefinitions)
            {
                Console.WriteLine("Compiling : {0} ", wd.Name);
                await this.InsertSchemaAsync(wd);
                await this.InsertAsync(wd);
                this.Compile(wd);

            }
        }

        private async Task InsertSchemaAsync(WorkflowDefinition wd)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var folder = Path.Combine(wc, typeof(WorkflowDefinition).Name);
            var xsd = Path.Combine(folder, wd.Name + ".xsd");


            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = new BinaryStore
            {
                Content = File.ReadAllBytes(xsd),
                Extension = ".xsd",
                StoreId = wd.SchemaStoreId,
                FileName = wd.Name + ".xsd",
                WebId = wd.SchemaStoreId
            };
            await store.AddAsync(schema);
        }

        private void Compile(WorkflowDefinition item)
        {
            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.WorkflowSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(typeof(Controller).Assembly.Location);
            options.ReferencedAssembliesLocation.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(typeof(Newtonsoft.Json.JsonConvert).Assembly.Location);
            var outputPath = ConfigurationManager.WorkflowCompilerOutputPath;
            var customDllPattern = ConfigurationManager.ApplicationName + ".*.dll";
            var entityAssembiles = Directory.GetFiles(outputPath, customDllPattern);
            foreach (var dll in entityAssembiles)
            {
                options.ReferencedAssembliesLocation.Add(dll);
            }


            var result = item.Compile(options);
            result.Errors.ForEach(Console.WriteLine);

            this.Deploy(item);
        }

        private void Deploy(WorkflowDefinition item)
        {
            var dll = string.Format("workflows.{0}.{1}.dll", item.WorkflowDefinitionId, item.Version);
            var pdb = string.Format("workflows.{0}.{1}.pdb", item.WorkflowDefinitionId, item.Version);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }

    }
}