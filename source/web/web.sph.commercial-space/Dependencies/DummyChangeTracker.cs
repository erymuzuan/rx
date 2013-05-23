using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Dependencies
{
    class DummyChangeTracker : IEntityChangePublisher
    {
        public async Task PublishAdded(IEnumerable<Entity> attachedCollection)
        {
            await Task.Delay(500);
        }

        public async Task PublishChanges(IEnumerable<Entity> attachedCollection)
        {
            await Task.Delay(500);
        }

        public async Task PublishDeleted(IEnumerable<Entity> deletedCollection)
        {
            await Task.Delay(500);
        }
    }
}
