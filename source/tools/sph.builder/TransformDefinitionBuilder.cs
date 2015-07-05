using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

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


        private async  Task CompileAsync(TransformDefinition map)
        {
            var options = new CompilerOptions
            {
                SourceCodeDirectory = ConfigurationManager.SphSourceDirectory
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
            result.Errors.ForEach(Console.WriteLine);

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to compile TransformDefinition {0}", map.Name);
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
            }
         

        }
    }
}