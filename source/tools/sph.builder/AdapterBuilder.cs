using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;
using Console = Colorful.Console;

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


        private async Task CompileAsync(Adapter item)
        {
            var folder = $"{ConfigurationManager.GeneratedSourceDirectory}\\{item.Name}";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.GeneratedSourceDirectory
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
            if (result.Errors.Count > 0)
            {
                Console.WriteLine($" ============== {result.Errors.Count} errors ===============");
                result.Errors.ForEach(x => Console.WriteLine(x, Color.Red));
            }

        }

        public override async Task RestoreAsync(Adapter adapter)
        {
            Console.WriteLine("Compiling : {0} ", adapter.Name);
            try
            {
                await this.CompileAsync(adapter);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to compile Adapter {0}", adapter.Name, Color.Red);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace, Color.Yellow);
                Console.ResetColor();
            }


        }
    }
}