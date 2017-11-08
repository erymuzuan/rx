using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IReadonlyRepository
    {
        Task TruncateAsync(string entity);
        Task CleanAsync(string entity);
        Task CleanAsync();
        Task<LoadOperation<Entity>> SearchAsync(string[] entities, Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20);

        Task<int> GetCountAsync(string entity);
    }
}