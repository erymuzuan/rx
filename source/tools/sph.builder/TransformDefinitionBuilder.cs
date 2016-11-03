using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Console = Colorful.Console;

namespace Bespoke.Sph.SourceBuilders
{
    internal class TransformDefinitionBuilder : Builder<TransformDefinition>
    {
        public override async Task RestoreAllAsync()
        {
            this.Initialize();
            var maps = this.GetItems();
            foreach (var m in maps)
            {
                await RestoreAsync(m);
            }
        }


        private async Task CompileAsync(TransformDefinition map)
        {
            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.GeneratedSourceDirectory
            };
            options.ReferencedAssembliesLocation.Add(ConfigurationManager.WebPath + @"\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(typeof(JsonConvert).Assembly.Location);
            var outputPath = ConfigurationManager.CompilerOutputPath;
            var customDllPattern = ConfigurationManager.ApplicationName + ".*.dll";
            var entityAssembiles = Directory.GetFiles(outputPath, customDllPattern);
            foreach (var dll in entityAssembiles)
            {
                options.ReferencedAssembliesLocation.Add(dll);
            }
            var codes = map.GenerateCode();
            var sources = map.SaveSources(codes);
            var result = await map.CompileAsync(options, sources);
            if (result.Errors.Count > 0)
            {
                Console.WriteLine($" ============== {result.Errors.Count} errors ===============");
                result.Errors.ForEach(x => Console.WriteLine(x, Color.Red));
            }

        }




        public override async Task RestoreAsync(TransformDefinition map)
        {
            Console.WriteLine("Compiling : {0} ", map.Name);
            try
            {
                await this.CompileAsync(map);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to compile TransformDefinition {0}", map.Name, Color.Red);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace, Color.Yellow);
                Console.ResetColor();
            }


        }
    }
}