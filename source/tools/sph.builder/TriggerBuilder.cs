using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

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
                await this.Compile(trigger);
            }
        }

        public override async Task RestoreAsync(Trigger item)
        {
            await this.Compile(item);
        }

        private async Task Compile(Trigger item)
        {
            var options = new CompilerOptions { IsDebug = true };
            var result = await item.CompileAsync(options);
            result.Errors.ForEach(Console.WriteLine);
            
        }

    }
}
