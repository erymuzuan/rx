using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Station.Domain;

namespace Bespoke.Station.Windows.Dependencies
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
