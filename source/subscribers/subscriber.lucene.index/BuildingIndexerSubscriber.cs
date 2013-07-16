using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Messaging
{
    public class LuceneIndexSubscriber : Subscriber<Building>
    {
        public override string QueueName
        {
            get { return "building_lucene"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] {"Building.*"}; }
        }

        protected async override Task ProcessMessage(string operation, Building item)
        {
            await Task.Delay(500);
            Console.WriteLine("{0}:{1}", item.Name, operation);
        }
    }
}
