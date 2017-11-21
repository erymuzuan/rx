using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.CosmosDbRepository
{
    public class Respository : IReadOnlyRepository
    {
        public Task TruncateAsync(string entity)
        {
            throw new NotImplementedException();
        }

        public Task CleanAsync(string entity)
        {
            throw new NotImplementedException();
        }

        public Task CleanAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LoadOperation<Entity>> SearchAsync(string[] entities, QueryDsl query)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(string entity)
        {
            throw new NotImplementedException();
        }
    }
}