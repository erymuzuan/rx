using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bespoke.Sph.Domain
{
    public interface IEntityChangePublisher
    {
        Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, Dictionary<string, object> headers);
        Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, Dictionary<string, object> headers);
        Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, Dictionary<string, object> headers);
    }
}