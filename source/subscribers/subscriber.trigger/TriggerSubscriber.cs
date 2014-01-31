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
            get { return new[] { "Trigger.#.#" }; }
        }

        protected override Task ProcessMessage(Trigger item, MessageHeaders header)
        {
            this.WriteMessage("Restarting the subscriber, changed detected to {0}", item);
            // NOTE : copy dlls, this will cause the appdomain to unload and we want it happend
            // after the Ack to the broker
            var message = string.Format("{0}[{3}] was {1} on {2}\r\n", item.Name, header.Crud, DateTime.Now,
                item.TriggerId);
            ThreadPool.QueueUserWorkItem(WriteChangedLogs, message);
            return Task.FromResult(0);
        }


        private static void WriteChangedLogs(object obj)
        {
            Thread.Sleep(1000);
            var file = Path.Combine(ConfigurationManager.SubscriberPath, "trigger.log");
            File.AppendAllText(file, obj.ToString());
        }

    }
}
