using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Dependencies
{
    class DummyChangeTracker : IEntityChangePublisher
    {
        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, Dictionary<string, object> headers)
        {
            await Task.Delay(500);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, Dictionary<string, object> headers)
        {
            await Task.Delay(500);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, Dictionary<string, object> headers)
        {
            await Task.Delay(500);
        }
    }
}
