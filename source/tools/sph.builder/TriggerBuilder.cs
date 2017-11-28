using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SourceBuilders
{
    public class TriggerBuilder : Builder<Trigger>
    {
        protected override Task<RxCompilerResult> CompileAssetAsync(Trigger item)
        { var options = new CompilerOptions
            {
                IsDebug = true,
                SourceCodeDirectory = $"{ConfigurationManager.GeneratedSourceDirectory}\\Trigger.{item.Name}"
            };
            if (!System.IO.Directory.Exists(options.SourceCodeDirectory))
                System.IO.Directory.CreateDirectory(options.SourceCodeDirectory);

            return item.CompileAsync(options);
        }

    



    }
}
