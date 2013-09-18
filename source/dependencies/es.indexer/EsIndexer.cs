using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticSearch
{
    public class EsIndexer : IEntityChangePublisher
    {
        public Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection)
        {
            throw new NotImplementedException();
        }

        public Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs)
        {
            throw new NotImplementedException();
        }

        public Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection)
        {
            throw new NotImplementedException();
        }
    }
}
