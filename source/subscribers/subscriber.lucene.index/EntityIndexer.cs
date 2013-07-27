using System;
using System.Configuration;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Messaging
{
    public abstract class EntityIndexer<T> : Subscriber<T> where T : Entity
    {
        public override string QueueName
        {
            get { return typeof(T).Name.ToLowerInvariant() + "_lucene"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(T).Name + ".*" }; }
        }

        protected abstract Task<SearchMetadata> GetMetadata(T item, MessageHeaders header);
        protected async override Task ProcessMessage(T item, MessageHeaders headers)
        {
            await Task.Delay(500);
            var metadata = await this.GetMetadata(item, headers);
            var indexer = new LuceneIndexer(ConfigurationManager.AppSettings["lucene.index"]);
            switch (headers.Crud)
            {
                case CrudOperation.Added:
                    indexer.AddDocuments(metadata);
                    break;
                case CrudOperation.Deleted:
                    indexer.RemoveDocuments(metadata);
                    break;
                case CrudOperation.Changed:
                    try
                    {
                        indexer.RemoveDocuments(metadata);
                    }
                    catch (Exception e)
                    {
                        this.WriteError(e);
                    }
                    indexer.AddDocuments(metadata);
                    break;
            }

            this.WriteMessage("Indexer : {0} {1} id = {2}",typeof(T).Name, headers.Crud, metadata.Id);

        }
    }
}