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
        protected override Task<RxCompilerResult> CompileAssetAsync(Adapter item)
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

            return item.CompileAsync();
        }

      



       
    }
}