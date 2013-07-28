using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Dependencies
{
    class DummyChangeTracker : IEntityChangePublisher
    {
        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection)
        {
            await Task.Delay(500);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs)
        {
            await Task.Delay(500);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection)
        {
            await Task.Delay(500);
        }
    }
}
