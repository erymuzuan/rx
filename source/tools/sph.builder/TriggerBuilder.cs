﻿using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace sph.builder
{
    public class TriggerBuilder : Builder<Trigger>
    {

        public override async Task Restore()
        {
            this.Initialize();
            var triggers = this.GetItems();
            foreach (var trigger in triggers)
            {
                await this.InsertAsync(trigger);
                await this.Compile(trigger);

            }
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

            var dll = string.Format("subscriber.trigger.{0}.dll", item.TriggerId);
            var pdb = string.Format("subscriber.trigger.{0}.pdb", item.TriggerId);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);

        }
    }
}
