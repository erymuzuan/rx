using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.CustomTriggers
{
    public class TriggerSubscriber : Subscriber<Trigger>
    {
        public override string QueueName
        {
            get { return "trigger_subs"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "Trigger.#.Publish" }; }
        }

        protected override async Task ProcessMessage(Trigger item, MessageHeaders header)
        {
            this.WriteMessage("Compiling new trigger");
            if (header.Crud == CrudOperation.Deleted)
            {
                this.QueueUserWorkItem(DeleteTriggerAssembly, item);
            }
            this.WriteMessage("Restarting the subscriber, changed detected to {0}", item);
            var options = new CompilerOptions{IsDebug = true};
            var result = await item.CompileAsync(options);
            this.WriteMessage("Compile result {0}", result.Result);
            result.Errors.ForEach(e => this.WriteError(new Exception(e.Message)));

            // NOTE : copy dlls, this will cause the appdomain to unload and we want it happend
            // after the Ack to the broker
            this.QueueUserWorkItem(DeployTriggerAssembly, item);
        }

        private void DeleteTriggerAssembly(Trigger trigger)
        {
            Thread.Sleep(1000);
            var dll = Path.Combine(ConfigurationManager.SubscriberPath, string.Format("subscriber.trigger.{0}.dll", trigger.TriggerId));
            if (File.Exists(dll))
                File.Delete(dll);
        }


        private static void DeployTriggerAssembly(object obj)
        {
            Thread.Sleep(1000);
            var item = (Trigger)obj;
            var dll = string.Format("subscriber.trigger.{0}.dll", item.TriggerId);
            var pdb = string.Format("subscriber.trigger.{0}.pdb", item.TriggerId);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);
        
        }

    }
}
