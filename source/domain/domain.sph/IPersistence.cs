using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bespoke.Sph.Domain
{
    public interface IPersistence
    {
        Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems, PersistenceSession session, string user);
        Task<SubmitOperation> SubmitChanges(Entity item, string user);
    }
}
