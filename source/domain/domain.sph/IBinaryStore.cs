using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface IBinaryStore
    {
        void Add(BinaryStore document);
        BinaryStore GetContent(string id);
        Task<BinaryStore> GetContentAsync(string id);
        Task AddAsync(BinaryStore document);
        Task DeleteAsync(string storeId);
    }
}