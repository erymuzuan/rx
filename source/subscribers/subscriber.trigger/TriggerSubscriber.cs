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
            var result = await item.CompileAsync();
            if (!result.Result)
            {
                this.WriteMessage("Fail to build your Trigger ");
                result.Errors.ForEach(e => this.WriteError(new Exception(e.ToString())));
            }

        }

        private async void DeleteTrigger(Trigger trigger)
        {
            this.WriteMessage($"Deleting trigger_subs_{trigger.Id} queue");
            var url = $"http://{ConfigurationManager.RabbitMqHost}:{ConfigurationManager.RabbitMqManagementPort}";
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(ConfigurationManager.RabbitMqUserName, ConfigurationManager.RabbitMqPassword)
            };
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                this.WriteMessage("Deleting the queue for trigger : " + trigger.Name);
                var response = await client.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/trigger_subs_{trigger.Id}");
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    this.WriteError(new Exception($"Cannot delete queue trigger_subs_{trigger.Id} for trigger {trigger.Id}- return code is {response.StatusCode}"));
                }
            }
            this.WriteMessage("mark the trigger dll for deletion");
            var mark = $"{ConfigurationManager.SubscriberPath}\\mark.for.delete.txt";
            File.AppendAllLines(mark, new[]
            {
                $"subscriber.trigger.{trigger.Id}.dll",
                $"subscriber.trigger.{trigger.Id}.pdb"
            });

        }


    }
}
