using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.SubscribersInfrastructure;
using Microsoft.Win32.TaskScheduler;
using Task = System.Threading.Tasks.Task;

namespace subscriber.deletedelay
{
    [Obsolete("Use delayed messaging")]
    public class DeleteDelaySubscriber : Subscriber<Tracker>
    {
        public override string QueueName => "delete_delay_subscriber";

        public override string[] RoutingKeys => new[] { "Tracker.#.DeleteDelayActivity" };

        protected override Task ProcessMessage(Tracker item, BrokeredMessage header)
        {
            var path = item.WebId;
            using (var ts = new TaskService())
            {
                ts.RootFolder.DeleteTask(path);
            }
            return Task.FromResult(0);
        }
    }
}