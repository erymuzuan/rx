using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

namespace Bespoke.Sph.SourceBuilders
{
    internal class AdapterBuilder : Builder<Adapter>
    {
        public override async Task RestoreAllAsync()
        {
            this.Initialize();
            var list = this.GetItems();
            foreach (var adapter in list)
            {
                await RestoreAsync(adapter);

            }
        }


        private async  Task CompileAsync(Adapter item)
        {
            var folder = $"{ConfigurationManager.GeneratedSourceDirectory}\\{item.Name}";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.SphSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(typeof(Controller).Assembly.Location);
            options.ReferencedAssembliesLocation.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(typeof(JsonConvert).Assembly.Location);
            var outputPath = ConfigurationManager.CompilerOutputPath;
            var customDllPattern = ConfigurationManager.ApplicationName + ".*.dll";
            var entityAssembiles = Directory.GetFiles(outputPath, customDllPattern);
            foreach (var dll in entityAssembiles)
            {
                options.ReferencedAssembliesLocation.Add(dll);
            }


            var result = await item.CompileAsync();
            result.Errors.ForEach(Console.WriteLine);

        }

     

        
        public override async Task RestoreAsync(Adapter wd)
        {
            Console.WriteLine("Compiling : {0} ", wd.Name);
            try
            {
                await this.CompileAsync(wd);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to compile Adapter {0}", wd.Name);
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
            }
         

        }
    }
}