using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IEntityChangePublisher
    {
        Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection);
        Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection);
        Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection);
    }
}