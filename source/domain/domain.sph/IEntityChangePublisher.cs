using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bespoke.Sph.Domain
{
    public interface IEntityChangePublisher
    {
        Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, IDictionary<string, object> headers);
        Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, IDictionary<string, object> headers);
        Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers);
        Task SubmitChangesAsync(string operation, IEnumerable<Entity> attachedEntities, IEnumerable<Entity> deletedEntities, IDictionary<string, object> headers);
    }
}