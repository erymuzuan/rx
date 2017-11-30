using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadOnlyRepository
    {
        Task TruncateAsync(string entity);
        Task CleanAsync(string entity);
        Task CleanAsync();
        Task<LoadOperation<Entity>> SearchAsync(string[] entities, QueryDsl query);

        Task<int> GetCountAsync(string entity);
    }

    public interface IReadOnlyRepositorySyncManager
    {
        Task AddAsync(Entity item);
        Task DeleteAsync(Entity item);
        Task UpdateAsync(Entity item);
        Task BulkInsertAsync(Entity[] entities);
    }
}