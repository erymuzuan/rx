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
        public override string QueueName => "trigger_subs";

        public override string[] RoutingKeys => new[] { "Trigger.#.Publish", "Trigger.#.Depublish" };

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
            var options = new CompilerOptions { IsDebug = true, SourceCodeDirectory = ConfigurationManager.UserSourceDirectory };
            var result = await item.CompileAsync(options);
            if (!result.Result)
            {
                this.WriteMessage("Fail to build your Trigger ");
                result.Errors.ForEach(e => this.WriteError(new Exception(e.Message)));
            }
            
        }

        private async void DeleteTrigger(Trigger trigger)
        {
            Thread.Sleep(1000);
            var dll = Path.Combine(ConfigurationManager.SubscriberPath, $"subscriber.trigger.{trigger.Id}.dll");
            if (File.Exists(dll))
                File.Delete(dll);
            var pdb = Path.Combine(ConfigurationManager.SubscriberPath, $"subscriber.trigger.{trigger.Id}.pdb");
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
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                this.WriteMessage("Deleting the queue for trigger : " + trigger.Name);
                var response = await client.DeleteAsync(string.Format("/api/queues/{1}/trigger_subs_{0}", trigger.Id, ConfigurationManager.ApplicationName));
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    this.WriteError(new Exception(string.Format("Cannot delete queue trigger_subs_{0} for trigger {0}- return code is {1}", trigger.Id, response.StatusCode)));
                }
            }
        }
        

    }
}
