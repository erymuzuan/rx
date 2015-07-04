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
            foreach (var wd in maps)
            {
                await RestoreAsync(wd);

            }
        }


        private async  Task CompileAsync(TransformDefinition item)
        {
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


            var result = await item.CompileAsync(options);
            result.Errors.ForEach(Console.WriteLine);

        }

     

        
        public override async Task RestoreAsync(TransformDefinition wd)
        {
            Console.WriteLine("Compiling : {0} ", wd.Name);
            try
            {
               await this.CompileAsync(wd);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to compile Trasnform Definition {0}", wd.Name);
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(e.StackTrace);
                Console.ResetColor();
            }
         

        }
    }
}