using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IPersistence
    {
        Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session);
        Task<SubmitOperation> SubmitChanges(Entity item);
    }
}
