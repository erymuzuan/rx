using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace sph.builder
{
    public class TriggerBuilder : Builder<Trigger>
    {

        public override async Task RestoreAllAsync()
        {
            this.Initialize();
            var triggers = this.GetItems();
            foreach (var trigger in triggers)
            {
                await this.InsertAsync(trigger);
                await this.Compile(trigger);
            }
        }

        public override async Task RestoreAsync(Trigger item)
        {
            await this.InsertAsync(item);
            await this.Compile(item);
        }

        private async Task Compile(Trigger item)
        {
            var options = new CompilerOptions { IsDebug = true };
            var result = await item.CompileAsync(options);
            result.Errors.ForEach(Console.WriteLine);

            this.QueueUserWorkItem(DeployTriggerAssembly, item);
        }

        private static void DeployTriggerAssembly(Trigger item)
        {

            var dll = string.Format("subscriber.trigger.{0}.dll", item.Id);
            var pdb = string.Format("subscriber.trigger.{0}.pdb", item.Id);
            var dllFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }
    }
}
