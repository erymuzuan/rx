using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Messaging
{
    public class BuildingIndexerSubscriber : Subscriber<Building>
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
            var metada = new SearchMetadata
                {
                    Title = item.Name,
                    Text = item.ToString(),
                    Created = item.CreatedDate,
                    OwnerCode = item.CreatedBy,
                    Summary = item.ToString(),
                    Code = item.LotNo,
                    Type = typeof(Building).Name
                };
            var indexer = new LuceneIndexer(@"g:\temp\index");
            indexer.AddDocuments(metada);

        }
    }
}
