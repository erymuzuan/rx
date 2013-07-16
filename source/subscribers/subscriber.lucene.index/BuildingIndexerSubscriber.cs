using System;
using System.Collections.Generic;
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
            var metada = new BuildingMetadata
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

    class BuildingMetadata : ISearchable
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Summary { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string Title { get; set; }
        public Dictionary<string, object> CustomFields { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string OwnerCode { get; set; }
    }
}
