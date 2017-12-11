using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public class ReadOnlyRepositoryRepositorySyncManager : IReadOnlyRepositorySyncManager
    {
        public Task AddAsync(Entity item)
        {
            return Task.FromResult(0);
        }

        public Task DeleteAsync(Entity item)
        {
            return Task.FromResult(0);
        }

        public Task UpdateAsync(Entity item)
        {
            return Task.FromResult(0);
        }

        public Task BulkInsertAsync(Entity[] entities)
        {
            throw new System.NotImplementedException();
        }
    }
}
