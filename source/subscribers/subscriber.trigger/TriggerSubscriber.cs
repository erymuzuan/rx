using System;
using System.IO;
using System.Net;
using System.Net.Http;
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
            get { return new[] { "Trigger.#.Publish", "Trigger.#.Depublish" }; }
        }

        protected override async Task ProcessMessage(Trigger item, MessageHeaders header)
        {
            this.WriteMessage("Compiling new trigger");
            if (header.Crud == CrudOperation.Deleted)
            {
                this.QueueUserWorkItem(DeleteTrigger, item);
                return;
            }
            if (item.IsActive == false)
            {
                this.QueueUserWorkItem(DeleteTrigger, item);
                return;
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

        private async void DeleteTrigger(Trigger trigger)
        {
            Thread.Sleep(1000);
            var dll = Path.Combine(ConfigurationManager.SubscriberPath, string.Format("subscriber.trigger.{0}.dll", trigger.TriggerId));
            if (File.Exists(dll))
                File.Delete(dll);
            var pdb = Path.Combine(ConfigurationManager.SubscriberPath, string.Format("subscriber.trigger.{0}.pdb", trigger.TriggerId));
            if (File.Exists(pdb))
                File.Delete(pdb);
            

            Thread.Sleep(1000);
            this.WriteMessage("Deleted the trigger dll");
            dynamic connection = ObjectBuilder.GetObject("IBrokerConnection");
            var url = string.Format("http://{0}:{1}", connection.Host, connection.ManagementPort);

            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(connection.UserName, connection.Password)
            };
            using (var client = new HttpClient(handler){BaseAddress = new Uri(url)})
            {
                this.WriteMessage("Deleting the queue for trigger : " + trigger.Name);
                var response = await client.DeleteAsync(string.Format("/api/queues/{1}/trigger_subs_{0}", trigger.TriggerId, ConfigurationManager.ApplicationName));
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    this.WriteError(new Exception(string.Format("Cannot delete queue trigger_subs_{0} for trigger {0}- return code is {1}", trigger.TriggerId, response.StatusCode)));
                }
            }
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
