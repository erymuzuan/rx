using System;
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
            get { return new []{"Trigger.#"}; }
        }

        protected override Task ProcessMessage(Trigger item, MessageHeaders header)
        {
            throw new NotImplementedException();
        }

    }
}
