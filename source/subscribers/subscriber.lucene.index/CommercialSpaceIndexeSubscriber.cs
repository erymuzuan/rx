using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Messaging
{
    public class CommercialSpaceIndexeSubscriber : Subscriber<CommercialSpace>
    {
        public override string QueueName
        {
            get { return "cs_lucene"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "CommercialSpace.*" }; }
        }

        protected async override Task ProcessMessage(string operation, CommercialSpace item)
        {
            await Task.Delay(500);
            Console.WriteLine("{0}:{1}", item.RegistrationNo, operation);
            var metada = new SearchMetadata
                {
                    Title = item.RegistrationNo,
                    Text = item.ToString(),
                    Created = item.CreatedDate,
                    OwnerCode = item.CreatedBy,
                    Summary = item.ToString(),
                    Code = item.LotName,
                    Type = typeof(Building).Name
                };
            var indexer = new LuceneIndexer(@"g:\temp\index");
            indexer.AddDocuments(metada);

        }
    }
}
