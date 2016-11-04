using System.Drawing;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Console = Colorful.Console;

namespace Bespoke.Sph.SourceBuilders
{
    public class TriggerBuilder : Builder<Trigger>
    {

        public override async Task RestoreAllAsync()
        {
            this.Initialize();
            var triggers = this.GetItems();
            foreach (var trigger in triggers)
            {
                await this.CompileAsync(trigger);
            }
        }

        public override async Task RestoreAsync(Trigger item)
        {
            await this.CompileAsync(item);
        }

        private async Task CompileAsync(Trigger item)
        {
            var options = new CompilerOptions
            {
                IsDebug = true,
                SourceCodeDirectory = $"{ConfigurationManager.GeneratedSourceDirectory}\\Trigger.{item.Name}"
            };
            if (!System.IO.Directory.Exists(options.SourceCodeDirectory))
                System.IO.Directory.CreateDirectory(options.SourceCodeDirectory);

            Console.WriteLine($"Compiling trigger : {item.Name} ......");
            var result = await item.CompileAsync(options).ConfigureAwait(false);
            Console.WriteLine(result.Errors.Count > 0
                ? $" ================ {result.Errors} errors , 1 failed =================="
                : " ================ 0 errors , 1 succeeded ==================", Color.Cyan);
            result.Errors.ForEach(x => Console.WriteLine(x, Color.Red));

        }

    }
}
