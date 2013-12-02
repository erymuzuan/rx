using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test
{
    internal class MockChangePublisher : IEntityChangePublisher
    {
        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, Dictionary<string, object> headers)
        {
            await Task.Delay(500).ConfigureAwait(false);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, Dictionary<string, object> headers)
        {
            await Task.Delay(500).ConfigureAwait(false);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, Dictionary<string, object> headers)
        {
            await Task.Delay(500).ConfigureAwait(false);
        }
    }
}