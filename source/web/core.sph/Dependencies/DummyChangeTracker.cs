using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Dependencies
{
    class DummyChangeTracker : IEntityChangePublisher
    {
        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, IDictionary<string, object> headers)
        {
            await Task.Delay(500);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, IDictionary<string, object> headers)
        {
            await Task.Delay(500);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers)
        {
            await Task.Delay(500);
        }

        public Task SubmitChangesAsync(string operation, IEnumerable<Entity> attachedEntities, IEnumerable<Entity> deletedEntities)
        {
            return Task.FromResult(0);
        }

    }
}
