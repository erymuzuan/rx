using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;

namespace Bespoke.Sph.CustomTriggers
{
    public class TriggerSubscriber : Subscriber<Trigger>
    {
        public override string QueueName => "trigger_subs";
        public override string[] RoutingKeys => new[] { "Trigger.#.Publish", "Trigger.#.Depublish" };

        protected override async Task ProcessMessage(Trigger item, BrokeredMessage header)
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

            this.WriteMessage($"Restarting the subscriber, changed detected to {item}");
            var result = await item.CompileAsync();
            if (!result.Result)
            {
                this.WriteMessage("Fail to build your Trigger ");
                result.Errors.ForEach(e => this.WriteError(new Exception(e.ToString())));
            }

        }

        private async void DeleteTrigger(Trigger trigger)
        {

            await Task.Delay(500);
            // Broker.RemoveSubscriptionAsync();

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
