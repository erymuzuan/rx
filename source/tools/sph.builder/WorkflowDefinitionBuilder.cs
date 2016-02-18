using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
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


        private void Compile(WorkflowDefinition item)
        {
            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.GeneratedSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(typeof(Controller).Assembly.Location);
            options.ReferencedAssembliesLocation.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(typeof(Newtonsoft.Json.JsonConvert).Assembly.Location);
            var outputPath = ConfigurationManager.CompilerOutputPath;
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
            var dll = $"workflows.{item.Id}.{item.Version}.dll";
            var pdb = $"workflows.{item.Id}.{item.Version}.pdb";
            var dllFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }
        


        public override Task RestoreAsync(WorkflowDefinition wd)
        {
            Console.WriteLine("Compiling : {0} ", wd.Name);
            try
            {
                this.Compile(wd);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to compile workflow {0}", wd.Name);
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
            }
            // save
            return Task.FromResult(0);
        }
    }
}