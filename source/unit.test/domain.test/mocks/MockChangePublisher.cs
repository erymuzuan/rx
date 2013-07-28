using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

namespace domain.test
{
    internal class MockChangePublisher : IEntityChangePublisher
    {
        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection)
        {
            await Task.Delay(500).ConfigureAwait(false);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs)
        {
            await Task.Delay(500).ConfigureAwait(false);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection)
        {
            await Task.Delay(500).ConfigureAwait(false);
        }
    }
}